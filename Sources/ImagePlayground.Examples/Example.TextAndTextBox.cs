using SixLabors.ImageSharp;
using System;

namespace ImagePlayground.Examples;
internal partial class Example {
    /// <summary>
    /// Demonstrates AddText and AddTextBox methods side by side.
    /// </summary>
    /// <param name="folderPath">Directory with the source image.</param>
    public static void TextAndTextBox1(string folderPath) {
        Console.WriteLine("[*] AddText vs AddTextBox example 1");
        string src = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr.jpg");
        string dest = System.IO.Path.Combine(folderPath, "TextAndTextBox.jpg");
        using (var image = Image.Load(src)) {
            image.AddText(50, 50, "Add-Text example", Color.Red, 32);
            image.AddTextBox(50, 100, "Add-TextBox wraps this very long line of text inside a specified width to show the difference.", 400, Color.Blue, 32);
            image.Save(dest);
        }
    }

    /// <summary>
    /// Shows differences when using a narrow text box.
    /// </summary>
    /// <param name="folderPath">Directory with the source image.</param>
    public static void TextAndTextBox2(string folderPath) {
        Console.WriteLine("[*] AddText vs AddTextBox example 2");
        string src = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr.jpg");
        string dest = System.IO.Path.Combine(folderPath, "TextAndTextBox2.jpg");
        using (var image = Image.Load(src)) {
            image.AddText(10, 10, "Top-left text", Color.Green, 24);
            image.AddTextBox(10, 40, "Add-TextBox with narrow width wraps quickly for comparison.", 150, Color.Orange, 24);
            image.Save(dest);
        }
    }
}
