namespace ImagePlayground;

/// <summary>
/// Helper methods for directory creation.
/// </summary>
public static partial class Helpers {
    /// <summary>
    /// Creates the parent directory for the specified file path if it does not exist.
    /// </summary>
    /// <param name="fullPath">Full file path.</param>
    public static void CreateParentDirectory(string fullPath) {
        string? directory = System.IO.Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory)) {
            System.IO.Directory.CreateDirectory(directory);
        }
    }
}
