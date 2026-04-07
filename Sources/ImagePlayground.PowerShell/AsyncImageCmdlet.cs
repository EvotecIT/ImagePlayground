using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>
/// Base class for asynchronous image cmdlets with shared validation helpers.
/// </summary>
public abstract class AsyncImageCmdlet : AsyncPSCmdlet {
    /// <summary>
    /// Resolves a file path and terminates the cmdlet when the file does not exist.
    /// </summary>
    /// <param name="path">Path supplied by the caller.</param>
    /// <param name="errorId">Stable PowerShell error id.</param>
    /// <param name="targetObject">Target object for the error record.</param>
    /// <param name="label">Friendly label for the missing item.</param>
    /// <returns>Resolved full path.</returns>
    protected string ResolveExistingFilePath(string path, string errorId, object targetObject, string label = "File") {
        string resolvedPath = Helpers.ResolvePath(path);
        if (!File.Exists(resolvedPath)) {
            var exception = new FileNotFoundException($"{label} not found: {path}", path);
            ThrowTerminatingError(new ErrorRecord(exception, errorId, ErrorCategory.ObjectNotFound, targetObject));
        }

        return resolvedPath;
    }

    /// <summary>
    /// Resolves a directory path and terminates the cmdlet when it does not exist.
    /// </summary>
    /// <param name="path">Path supplied by the caller.</param>
    /// <param name="errorId">Stable PowerShell error id.</param>
    /// <param name="targetObject">Target object for the error record.</param>
    /// <param name="label">Friendly label for the missing item.</param>
    /// <returns>Resolved full path.</returns>
    protected string ResolveExistingDirectoryPath(string path, string errorId, object targetObject, string label = "Directory") {
        string resolvedPath = Helpers.ResolvePath(path);
        if (!Directory.Exists(resolvedPath)) {
            var exception = new DirectoryNotFoundException($"{label} not found: {path}");
            ThrowTerminatingError(new ErrorRecord(exception, errorId, ErrorCategory.ObjectNotFound, targetObject));
        }

        return resolvedPath;
    }
}
