using System;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

internal static class PowerShellPathResolver {
    internal static string ResolveFileSystemPath(PSCmdlet cmdlet, string path) {
        if (cmdlet == null) {
            throw new ArgumentNullException(nameof(cmdlet));
        }
        if (string.IsNullOrWhiteSpace(path)) {
            throw new PSArgumentException("A non-empty path is required.", nameof(path));
        }
        var resolved = cmdlet.SessionState.Path.GetUnresolvedProviderPathFromPSPath(
            path,
            out var provider,
            out _);
        if (provider == null ||
            !string.Equals(provider.Name, "FileSystem", StringComparison.OrdinalIgnoreCase)) {
            throw new PSArgumentException("Image story paths must use the FileSystem provider.", nameof(path));
        }
        return resolved;
    }

    internal static void ValidateFileDestination(string path, string label, string parameterName) {
        ValidateFileDestinationCore(path, label, parameterName);
        var canonicalPath = FileSystemPathIdentity.GetCanonicalPath(path);
        if (!string.Equals(canonicalPath, Path.GetFullPath(path), StringComparison.Ordinal)) {
            ValidateResolvedSymbolicLinkTarget(path, canonicalPath, label, parameterName);
            ValidateFileDestinationCore(canonicalPath, label, parameterName);
        }
    }

    private static void ValidateFileDestinationCore(string path, string label, string parameterName) {
        if (Directory.Exists(path)) {
            throw new PSArgumentException(
                $"{label} must resolve to a file, but an existing directory was found: {path}",
                parameterName);
        }
        ValidateAncestors(path, label, parameterName);
    }

    internal static void ValidateDirectoryDestination(string path, string label, string parameterName) {
        ValidateDirectoryDestinationCore(path, label, parameterName);
        var canonicalPath = FileSystemPathIdentity.GetCanonicalPath(path);
        if (!string.Equals(canonicalPath, Path.GetFullPath(path), StringComparison.Ordinal)) {
            ValidateResolvedSymbolicLinkTarget(path, canonicalPath, label, parameterName);
            ValidateDirectoryDestinationCore(canonicalPath, label, parameterName);
        }
    }

    private static void ValidateDirectoryDestinationCore(string path, string label, string parameterName) {
        if (File.Exists(path)) {
            throw new PSArgumentException(
                $"{label} must resolve to a directory, but an existing file was found: {path}",
                parameterName);
        }
        ValidateAncestors(path, label, parameterName);
    }

    private static void ValidateResolvedSymbolicLinkTarget(
        string path,
        string canonicalPath,
        string label,
        string parameterName) {
        if (!FileSystemPathIdentity.IsSymbolicLink(path)) return;

        var parent = Path.GetDirectoryName(canonicalPath);
        if (!string.IsNullOrWhiteSpace(parent) && !Directory.Exists(parent) && !File.Exists(parent)) {
            throw new PSArgumentException(
                $"{label} cannot use a dangling symbolic link whose target parent does not exist: {parent}",
                parameterName);
        }
    }

    private static void ValidateAncestors(string path, string label, string parameterName) {
        var current = Path.GetDirectoryName(path);
        while (!string.IsNullOrWhiteSpace(current)) {
            if (File.Exists(current)) {
                throw new PSArgumentException(
                    $"{label} cannot be created because a parent path is an existing file: {current}",
                    parameterName);
            }
            var parent = Path.GetDirectoryName(current);
            if (string.Equals(parent, current, StringComparison.Ordinal)) break;
            current = parent;
        }
    }
}
