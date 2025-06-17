using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground {
    public partial class Image {
        public void SaveAsIcon(string filePath, params int[] sizes) {
            string fullPath = Path.GetFullPath(filePath);
            if (sizes == null || sizes.Length == 0) {
                sizes = new[] { 16, 32, 48, 64, 128, 256 };
            }

            List<byte[]> frames = new();
            List<(int Width, int Height)> dims = new();

            foreach (int size in sizes.Distinct().OrderBy(s => s)) {
                using Image<Rgba32> clone = _image.CloneAs<Rgba32>();
                clone.Mutate(ctx => ctx.Resize(new ResizeOptions {
                    Mode = ResizeMode.Stretch,
                    Size = new Size(size, size)
                }));
                using MemoryStream ms = new();
                clone.SaveAsPng(ms);
                frames.Add(ms.ToArray());
                dims.Add((size, size));
            }

            using FileStream fs = File.Create(fullPath);
            using BinaryWriter bw = new(fs);
            bw.Write((ushort)0); // reserved
            bw.Write((ushort)1); // type icon
            bw.Write((ushort)frames.Count);

            int offset = 6 + 16 * frames.Count;
            for (int i = 0; i < frames.Count; i++) {
                var (w, h) = dims[i];
                bw.Write((byte)(w >= 256 ? 0 : w));
                bw.Write((byte)(h >= 256 ? 0 : h));
                bw.Write((byte)0);
                bw.Write((byte)0);
                bw.Write((ushort)1);
                bw.Write((ushort)32);
                bw.Write(frames[i].Length);
                bw.Write(offset);
                offset += frames[i].Length;
            }

            foreach (byte[] data in frames) {
                bw.Write(data);
            }

            bw.Flush();
        }
    }
}
