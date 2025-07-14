using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for ImagePlayground.
/// </summary>
public partial class ImagePlayground {
    internal readonly string _directoryWithImages;
    internal readonly string _directoryWithTests;

    public ImagePlayground() {
        _directoryWithImages = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images");
        Setup(_directoryWithImages);
        _directoryWithTests = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Tests");
        Setup(_directoryWithTests);
    }

    internal static void Setup(string path) {
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
    }
}
