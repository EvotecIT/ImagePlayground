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
}
