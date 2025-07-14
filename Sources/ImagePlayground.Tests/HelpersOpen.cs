using System;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_Open_InvalidPath_Handled() {
        string invalid = Path.Combine(_directoryWithTests, "notexisting.exe");
        using var sw = new StringWriter();
        TextWriter original = Console.Error;
        Console.SetError(sw);
        try {
            Helpers.Open(invalid, true);
        } finally {
            Console.SetError(original);
        }
        string output = sw.ToString();
        Assert.Contains("Unable to open", output);
        Assert.Contains("file does not exist", output);
    }
}
