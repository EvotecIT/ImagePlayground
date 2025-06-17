using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImagePlayground {
    public static partial class Helpers {
        public static string ResolvePath(string path) {
            if (string.IsNullOrWhiteSpace(path)) {
                throw new ArgumentException("Path cannot be null or empty", nameof(path));
            }
            string expanded = Environment.ExpandEnvironmentVariables(path);
            return Path.GetFullPath(expanded);
        }

        public static string ReadFileChecked(string path) {
            string fullPath = ResolvePath(path);
            if (!File.Exists(fullPath)) {
                throw new FileNotFoundException($"File not found: {path}", fullPath);
            }
            return File.ReadAllText(fullPath);
        }

        public static async Task<string> ReadFileCheckedAsync(string path) {
            string fullPath = ResolvePath(path);
            if (!File.Exists(fullPath)) {
                throw new FileNotFoundException($"File not found: {path}", fullPath);
            }
#if NETSTANDARD2_0 || NETFRAMEWORK
            return await Task.Run(() => File.ReadAllText(fullPath)).ConfigureAwait(false);
#else
            return await File.ReadAllTextAsync(fullPath).ConfigureAwait(false);
#endif
        }

        public static async Task<string> GetStringWithProperEncodingAsync(HttpClient client, string url) {
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
                @"<meta[^>]+charset\s*=\s*['""]?([^'"">\s]+)",
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
}
