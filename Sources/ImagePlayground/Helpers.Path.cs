namespace ImagePlayground;

/// <summary>
/// Helper methods for working with file paths.
/// </summary>
public static partial class Helpers {
    /// <summary>
    /// Resolves the provided path to an absolute file system path.
    /// Environment variables are expanded and relative paths are
    /// converted to full paths.
    /// </summary>
    /// <param name="path">File system path to resolve.</param>
    /// <returns>Absolute file path.</returns>
    /// <exception cref="System.ArgumentException">Thrown when path is null or empty.</exception>
    public static string ResolvePath(string path) {
        if (string.IsNullOrWhiteSpace(path)) {
            throw new System.ArgumentException("Path cannot be null or empty", nameof(path));
        }

        string expanded = System.Environment.ExpandEnvironmentVariables(path);
        return System.IO.Path.GetFullPath(expanded);
    }

    /// <summary>
    /// Reads the contents of a file after verifying that it exists.
    /// </summary>
    /// <param name="path">Path to the file.</param>
    /// <returns>File contents.</returns>
    /// <exception cref="System.IO.FileNotFoundException">Thrown when the file does not exist.</exception>
    public static string ReadFileChecked(string path) {
        string fullPath = ResolvePath(path);
        if (!System.IO.File.Exists(fullPath)) {
            throw new System.IO.FileNotFoundException($"File not found: {path}", fullPath);
        }

        return System.IO.File.ReadAllText(fullPath);
    }

    /// <summary>
    /// Asynchronously reads the contents of a file after verifying that it exists.
    /// </summary>
    /// <param name="path">Path to the file.</param>
    /// <returns>File contents.</returns>
    /// <exception cref="System.IO.FileNotFoundException">Thrown when the file does not exist.</exception>
    public static async System.Threading.Tasks.Task<string> ReadFileCheckedAsync(string path) {
        string fullPath = ResolvePath(path);
        if (!System.IO.File.Exists(fullPath)) {
            throw new System.IO.FileNotFoundException($"File not found: {path}", fullPath);
        }

#if NETSTANDARD2_0 || NETFRAMEWORK
        return await System.Threading.Tasks.Task.Run(() => System.IO.File.ReadAllText(fullPath)).ConfigureAwait(false);
#else
        return await System.IO.File.ReadAllTextAsync(fullPath).ConfigureAwait(false);
#endif
}


    /// <summary>
    /// Downloads content from a URL with proper encoding detection.
    /// </summary>
    /// <param name="client">HttpClient to use for the request.</param>
    /// <param name="url">URL to download from.</param>
    /// <returns>Content as a string with proper encoding.</returns>
    public static async System.Threading.Tasks.Task<string> GetStringWithProperEncodingAsync(System.Net.Http.HttpClient client, string url) {
        using var response = await client.GetAsync(url).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

        var contentType = response.Content.Headers.ContentType;
        if (contentType?.CharSet != null) {
            try {
                var encoding = System.Text.Encoding.GetEncoding(contentType.CharSet);
                return encoding.GetString(bytes);
            } catch {
            }
        }

        if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF) {
            return System.Text.Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
        }
        if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE) {
            return System.Text.Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);
        }
        if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF) {
            return System.Text.Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);
        }

        var asciiContent = System.Text.Encoding.ASCII.GetString(bytes);
        var metaMatch = System.Text.RegularExpressions.Regex.Match(
            asciiContent,
            @"<meta[^>]+charset\s*=\s*[\"']?([^\"'>\s]+)",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        if (metaMatch.Success) {
            try {
                var encoding = System.Text.Encoding.GetEncoding(metaMatch.Groups[1].Value);
                return encoding.GetString(bytes);
            } catch {
            }
        }

        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
