namespace ImagePlayground.PowerShell;

/// <summary>Preflights story export destinations and protects source and captured-output files.</summary>
public static class ImageStoryFileSafety {
    /// <summary>Validates destination types, ancestors and existing links for the whole export plan before writing any layout.</summary>
    /// <param name="outputDirectory">Absolute filesystem directory containing the exports.</param>
    /// <param name="exportFiles">Every absolute filesystem file destination in the export plan.</param>
    /// <remarks>Existing regular files may be replaced when the caller allows overwrite. This checks path structure without writing probes; encoding failures and concurrent filesystem changes still belong to the writer.</remarks>
    public static void ValidateDestinations(string outputDirectory, IEnumerable<string> exportFiles) {
        if (outputDirectory == null) throw new ArgumentNullException(nameof(outputDirectory));
        if (exportFiles == null) throw new ArgumentNullException(nameof(exportFiles));
        PowerShellPathResolver.ValidateDirectoryDestination(outputDirectory, "Story output directory", nameof(outputDirectory));
        foreach (var path in exportFiles)
            PowerShellPathResolver.ValidateFileDestination(path, "Story export", nameof(exportFiles));
    }

    /// <summary>Validates inputs against the physical export directory and every planned file before any export begins.</summary>
    /// <param name="outputDirectory">Absolute filesystem directory containing the exports.</param>
    /// <param name="inputFiles">Absolute filesystem paths of the input files to preserve.</param>
    /// <param name="exportFiles">Every absolute filesystem path in the export plan, including nested layouts.</param>
    /// <remarks>Follows existing directory/file links and detects existing hard-link aliases. This is a preflight check; callers must not change filesystem links during export.</remarks>
    public static void ValidateInputs(string outputDirectory, IEnumerable<string> inputFiles, IEnumerable<string> exportFiles) {
        if (outputDirectory == null) throw new ArgumentNullException(nameof(outputDirectory));
        if (inputFiles == null) throw new ArgumentNullException(nameof(inputFiles));
        if (exportFiles == null) throw new ArgumentNullException(nameof(exportFiles));
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
