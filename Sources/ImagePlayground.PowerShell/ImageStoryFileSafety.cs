namespace ImagePlayground.PowerShell;

/// <summary>Preflights story export destinations and protects source and captured-output files.</summary>
public static class ImageStoryFileSafety {
    /// <summary>Resolves a filesystem output directory against the PowerShell location for a stable, preflighted export plan.</summary>
    /// <param name="cmdlet">Calling script or cmdlet whose session supplies provider path resolution.</param>
    /// <param name="path">Output directory, with optional environment-variable references.</param>
    /// <returns>An absolute filesystem directory whose path will not change when the writer resolves it again.</returns>
    /// <remarks>Rejects non-filesystem providers and nested environment references instead of checking a different path from the writer.</remarks>
    public static string ResolveOutputDirectory(PSCmdlet cmdlet, string path) {
        var destination = PowerShellPathResolver.ResolveFileSystemPath(cmdlet, path);
        if (!string.Equals(Environment.ExpandEnvironmentVariables(destination), destination, StringComparison.Ordinal)) {
            throw new PSArgumentException("OutputDirectory contains nested environment variables. Supply a directly resolved path so preflight and export use the same destination.", nameof(path));
        }
        return destination;
    }

    /// <summary>Validates destination types, ancestors, links and distinct physical identities for the whole export plan before writing any layout.</summary>
    /// <param name="outputDirectory">Absolute filesystem directory containing the exports.</param>
    /// <param name="exportFiles">Every absolute filesystem file destination in the export plan.</param>
    /// <remarks>Existing regular files may be replaced when the caller allows overwrite. This checks path structure without writing probes; encoding failures and concurrent filesystem changes still belong to the writer.</remarks>
    public static void ValidateDestinations(string outputDirectory, IEnumerable<string> exportFiles) {
        if (outputDirectory == null) {
            throw new ArgumentNullException(nameof(outputDirectory));
        }
        if (exportFiles == null) {
            throw new ArgumentNullException(nameof(exportFiles));
        }
        PowerShellPathResolver.ValidateDirectoryDestination(outputDirectory, "Story output directory", nameof(outputDirectory));
        var destinations = exportFiles.ToArray();
        for (var index = 0; index < destinations.Length; index++) {
            var path = destinations[index];
            PowerShellPathResolver.ValidateFileDestination(path, "Story export", nameof(exportFiles));
            for (var previous = 0; previous < index; previous++) {
                if (FileSystemPathIdentity.AreSamePath(path, destinations[previous])) {
                    throw new PSArgumentException($"Story exports must reference distinct physical files: {path} and {destinations[previous]}", nameof(exportFiles));
                }
            }
        }
    }

    /// <summary>Validates inputs against the physical export directory and every planned file before any export begins.</summary>
    /// <param name="outputDirectory">Absolute filesystem directory containing the exports.</param>
    /// <param name="inputFiles">Absolute filesystem paths of the input files to preserve.</param>
    /// <param name="exportFiles">Every absolute filesystem path in the export plan, including nested layouts.</param>
    /// <remarks>Follows existing directory/file links and detects existing hard-link aliases. This is a preflight check; callers must not change filesystem links during export.</remarks>
    public static void ValidateInputs(string outputDirectory, IEnumerable<string> inputFiles, IEnumerable<string> exportFiles) {
        if (outputDirectory == null) {
            throw new ArgumentNullException(nameof(outputDirectory));
        }
        if (inputFiles == null) {
            throw new ArgumentNullException(nameof(inputFiles));
        }
        if (exportFiles == null) {
            throw new ArgumentNullException(nameof(exportFiles));
        }
        var root = FileSystemPathIdentity.GetCanonicalPath(outputDirectory)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var comparison = FileSystemPathIdentity.GetPathComparison(root);
        var destinations = exportFiles.ToArray();
        foreach (var input in inputFiles) {
            var source = FileSystemPathIdentity.GetCanonicalPath(input);
            if (string.Equals(source, root, comparison) || source.StartsWith(root + Path.DirectorySeparatorChar, comparison) ||
                destinations.Any(destination => FileSystemPathIdentity.AreSamePath(input, destination))) {
                throw new PSArgumentException("Keep script and captured-output input files outside OutputDirectory and all linked export destinations.");
            }
        }
    }
}
