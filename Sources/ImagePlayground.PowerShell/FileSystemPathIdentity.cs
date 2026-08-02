using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace ImagePlayground.PowerShell;

internal static class FileSystemPathIdentity {
    private const int FileFlagBackupSemantics = 0x02000000;
    private const int FileFlagOpenReparsePoint = 0x00200000;
    private const uint FileSystemControlGetReparsePoint = 0x000900A8;
    private const uint IoReparseTagMountPoint = 0xA0000003;
    private const uint IoReparseTagSymbolicLink = 0xA000000C;

    internal static string GetCanonicalPath(string path) {
        var visitedLinks = new HashSet<string>(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? StringComparer.OrdinalIgnoreCase
                : StringComparer.Ordinal);
        return GetCanonicalPath(path, visitedLinks);
    }

    private static string GetCanonicalPath(string path, HashSet<string> visitedLinks) {
        var fullPath = Path.GetFullPath(path);
        var suffix = new List<string>();
        var candidate = fullPath;
        while (true) {
            if (TryResolveSymbolicLink(candidate, out var target)) {
                if (!visitedLinks.Add(candidate)) {
                    throw new IOException($"Unable to resolve the output path identity because a symbolic-link cycle was found: {candidate}");
                }
                var canonicalTarget = GetCanonicalPath(target, visitedLinks);
                foreach (var segment in suffix) canonicalTarget = Path.Combine(canonicalTarget, segment);
                return Path.GetFullPath(canonicalTarget);
            }
            if (File.Exists(candidate) || Directory.Exists(candidate)) break;

            var name = Path.GetFileName(candidate);
            var parent = Path.GetDirectoryName(candidate);
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(parent)) break;
            suffix.Insert(0, name);
            candidate = parent;
        }

        var canonical = File.Exists(candidate) || Directory.Exists(candidate)
            ? ResolveExistingPath(candidate)
            : candidate;
        foreach (var segment in suffix) canonical = Path.Combine(canonical, segment);
        return Path.GetFullPath(canonical);
    }

    internal static StringComparison GetPathComparison(string path) {
        var directory = Directory.Exists(path) ? path : Path.GetDirectoryName(path);
        while (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory)) {
            directory = Path.GetDirectoryName(directory);
        }

        while (!string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory)) {
            var name = Path.GetFileName(directory);
            var parent = Path.GetDirectoryName(directory);
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(parent)) {
                var alternateName = ToggleCase(name);
                if (!string.Equals(name, alternateName, StringComparison.Ordinal)) {
                    return Directory.Exists(Path.Combine(parent, alternateName))
                        ? StringComparison.OrdinalIgnoreCase
                        : StringComparison.Ordinal;
                }
            }
            directory = parent;
        }

        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
    }

    private static string ToggleCase(string value) {
        var characters = value.ToCharArray();
        for (var index = 0; index < characters.Length; index++) {
            var alternate = char.IsUpper(characters[index])
                ? char.ToLowerInvariant(characters[index])
                : char.ToUpperInvariant(characters[index]);
            if (alternate == characters[index]) continue;
            characters[index] = alternate;
            return new string(characters);
        }
        return value;
    }

    private static string ResolveExistingPath(string path) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            using var handle = CreateFile(
                path,
                0,
                FileShare.ReadWrite | FileShare.Delete,
                IntPtr.Zero,
                FileMode.Open,
                FileFlagBackupSemantics,
                IntPtr.Zero);
            if (handle.IsInvalid) throw UnableToResolve(path);

            var required = GetFinalPathNameByHandle(handle, null, 0, 0);
            if (required == 0) throw UnableToResolve(path);
            var buffer = new StringBuilder((int)required + 1);
            if (GetFinalPathNameByHandle(handle, buffer, buffer.Capacity, 0) == 0) throw UnableToResolve(path);
            return NormalizeWindowsTarget(buffer.ToString());
        }

        var pointer = RealPath(path, IntPtr.Zero);
        if (pointer == IntPtr.Zero) throw UnableToResolve(path);
        try {
            return Marshal.PtrToStringAnsi(pointer) ?? path;
        } finally {
            Free(pointer);
        }
    }

    private static bool TryResolveSymbolicLink(string path, out string target) {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? TryResolveWindowsSymbolicLink(path, out target)
            : TryResolveUnixSymbolicLink(path, out target);
    }

    private static bool TryResolveWindowsSymbolicLink(string path, out string target) {
        target = string.Empty;
        using var handle = CreateFile(
            path,
            0,
            FileShare.ReadWrite | FileShare.Delete,
            IntPtr.Zero,
            FileMode.Open,
            FileFlagBackupSemantics | FileFlagOpenReparsePoint,
            IntPtr.Zero);
        if (handle.IsInvalid) return false;

        var buffer = new byte[16 * 1024];
        if (!DeviceIoControl(
                handle,
                FileSystemControlGetReparsePoint,
                IntPtr.Zero,
                0,
                buffer,
                buffer.Length,
                out _,
                IntPtr.Zero)) {
            return false;
        }

        var tag = BitConverter.ToUInt32(buffer, 0);
        var pathBufferOffset = tag == IoReparseTagSymbolicLink ? 20 : 16;
        if (tag != IoReparseTagSymbolicLink && tag != IoReparseTagMountPoint) return false;
        var substituteOffset = BitConverter.ToUInt16(buffer, 8);
        var substituteLength = BitConverter.ToUInt16(buffer, 10);
        var printOffset = BitConverter.ToUInt16(buffer, 12);
        var printLength = BitConverter.ToUInt16(buffer, 14);
        var usePrintName = printLength > 0;
        var offset = usePrintName ? printOffset : substituteOffset;
        var length = usePrintName ? printLength : substituteLength;
        if (length == 0 || pathBufferOffset + offset + length > buffer.Length) return false;

        target = Encoding.Unicode.GetString(buffer, pathBufferOffset + offset, length);
        var relative = tag == IoReparseTagSymbolicLink && BitConverter.ToUInt32(buffer, 16) == 1;
        if (relative) {
            target = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path) ?? string.Empty, target));
        } else {
            target = NormalizeWindowsTarget(target);
        }
        return true;
    }

    private static bool TryResolveUnixSymbolicLink(string path, out string target) {
        target = string.Empty;
        var buffer = new byte[16 * 1024];
        var length = ReadLink(path, buffer, new UIntPtr((uint)buffer.Length)).ToInt64();
        if (length <= 0 || length >= buffer.Length) return false;
        target = Encoding.UTF8.GetString(buffer, 0, (int)length);
        if (!Path.IsPathRooted(target)) {
            target = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path) ?? string.Empty, target));
        }
        return true;
    }

    private static string NormalizeWindowsTarget(string value) {
        if (value.StartsWith(@"\\?\UNC\", StringComparison.OrdinalIgnoreCase) ||
            value.StartsWith(@"\??\UNC\", StringComparison.OrdinalIgnoreCase)) {
            return @"\\" + value.Substring(8);
        }
        if (value.StartsWith(@"\\?\", StringComparison.OrdinalIgnoreCase) ||
            value.StartsWith(@"\??\", StringComparison.OrdinalIgnoreCase)) {
            return value.Substring(4);
        }
        return value;
    }

    private static IOException UnableToResolve(string path) {
        return new IOException(
            $"Unable to resolve the output path identity: {path}",
            Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeFileHandle CreateFile(
        string fileName,
        int desiredAccess,
        FileShare shareMode,
        IntPtr securityAttributes,
        FileMode creationDisposition,
        int flagsAndAttributes,
        IntPtr templateFile);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern uint GetFinalPathNameByHandle(
        SafeFileHandle file,
        StringBuilder? path,
        int pathLength,
        int flags);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DeviceIoControl(
        SafeFileHandle device,
        uint controlCode,
        IntPtr inputBuffer,
        int inputBufferSize,
        byte[] outputBuffer,
        int outputBufferSize,
        out int bytesReturned,
        IntPtr overlapped);

    [DllImport("libc", EntryPoint = "realpath", SetLastError = true)]
    private static extern IntPtr RealPath(string path, IntPtr resolvedPath);

    [DllImport("libc", EntryPoint = "readlink", SetLastError = true)]
    private static extern IntPtr ReadLink(string path, byte[] buffer, UIntPtr bufferSize);

    [DllImport("libc", EntryPoint = "free")]
    private static extern void Free(IntPtr pointer);
}
