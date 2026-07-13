using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ImagePlayground;

internal static class AsyncFile {
    internal static async Task<byte[]> ReadAllBytesAsync(string filePath, CancellationToken cancellationToken) {
        using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, useAsync: true);
        if (stream.Length > int.MaxValue) {
            throw new IOException($"File is too large to read into memory: {filePath}");
        }

        byte[] buffer = new byte[stream.Length];
        int offset = 0;
        while (offset < buffer.Length) {
            int read = await stream.ReadAsync(buffer, offset, buffer.Length - offset, cancellationToken).ConfigureAwait(false);
            if (read == 0) {
                break;
            }

            offset += read;
        }

        if (offset == buffer.Length) {
            return buffer;
        }

        byte[] result = new byte[offset];
        Buffer.BlockCopy(buffer, 0, result, 0, offset);
        return result;
    }
}
