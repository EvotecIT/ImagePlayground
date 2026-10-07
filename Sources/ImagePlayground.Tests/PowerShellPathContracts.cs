using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChartForgeX;
using ChartForgeX.Core;
using ChartForgeX.Topology;
using ChartForgeX.VisualArtifacts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using PowerShellHost = System.Management.Automation.PowerShell;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Theory]
    [InlineData("relative")]
    [InlineData("literal")]
    [InlineData("environment")]
    [InlineData("drive")]
    public void Test_PowerShell_CodePathsUseSessionLocation(string pathKind) {
        using var fixture = new PowerShellPathFixture();
        string fileName = pathKind == "literal" ? "code[1].png" : "code.png";
        string path = fixture.InputPath(pathKind, fileName);
        fixture.Invoke("New-ImageQRCode", ("Content", "session-location"), ("FilePath", path), ("PixelSize", 4));
        Assert.Equal("session-location", fixture.Invoke("Get-ImageQRCode", ("FilePath", path)).Single().Properties["Text"].Value);

        using var image = Assert.IsType<PlaygroundImage>(fixture.Invoke("Get-Image", ("FilePath", path)).Single().BaseObject);
        fixture.Invoke("Save-Image", ("Image", image), ("FilePath", "./saved.png"));
        fixture.AssertImageOutput("saved.png", image.Width, image.Height);
        fixture.Invoke("Resize-Image", ("FilePath", path), ("OutputPath", "./resized.png"), ("Width", 32), ("Height", 24), ("DontRespectAspectRatio", true));
        fixture.AssertImageOutput("resized.png", 32, 24);

        string barcodePath = fixture.InputPath(pathKind, "barcode.png");
        fixture.Invoke("New-ImageBarCode", ("Type", "EAN"), ("Value", "9012341234571"), ("FilePath", barcodePath));
        Assert.Equal("9012341234571", fixture.Invoke("Get-ImageBarCode", ("FilePath", barcodePath)).Single().Properties["Text"].Value);
        Assert.True(File.Exists(Path.Combine(fixture.Location, fileName)));
        Assert.False(File.Exists(Path.Combine(fixture.ProcessDirectory, fileName)));
        Assert.Equal(fixture.ProcessDirectory, Environment.CurrentDirectory);
        Assert.Equal(Path.Combine(fixture.ProcessDirectory, "core-path.png"), Helpers.ResolvePath("core-path.png"));
    }

    // Each row protects a separate public command's output parameter and actual file format.
    [Theory]
    [InlineData("Add-ImageText", "OutputPath", "png", 64, 48)]
    [InlineData("Add-ImageTextBox", "OutputPath", "png", 64, 48)]
    [InlineData("Add-ImageWatermark", "OutputPath", "png", 64, 48)]
    [InlineData("Compare-Image", "OutputPath", "png", 64, 48)]
    [InlineData("ConvertFrom-ImageBase64", "OutputPath", "png", 64, 48)]
    [InlineData("ConvertTo-Image", "OutputPath", "jpg", 64, 48)]
    [InlineData("Merge-Image", "FilePathOutput", "png", 64, 96)]
    [InlineData("New-ImageAvatar", "OutputPath", "png", 32, 24)]
    [InlineData("New-ImageCrop", "OutputPath", "png", 32, 24)]
    [InlineData("New-ImageGif", "FilePath", "gif", 64, 48)]
    [InlineData("New-ImageGrid", "FilePath", "png", 32, 24)]
    [InlineData("New-ImageMosaic", "OutputPath", "png", 64, 24)]
    [InlineData("Remove-ImageExif", "FilePathOutput", "png", 64, 48)]
    [InlineData("Remove-ImageMetadata", "OutputPath", "png", 64, 48)]
    [InlineData("Set-ImageAdjust", "OutputPath", "png", 64, 48)]
    [InlineData("Set-ImageBlur", "OutputPath", "png", 64, 48)]
    [InlineData("Set-ImageExif", "FilePathOutput", "png", 64, 48)]
    [InlineData("Set-ImageRotation", "OutputPath", "png", 48, 64)]
    [InlineData("Set-ImageSharpen", "OutputPath", "png", 64, 48)]
    public void Test_PowerShell_ImageOutputsUseSessionLocation(string command, string outputParameter, string extension, int width, int height) {
        using var fixture = new PowerShellPathFixture();
        string outputName = command + "." + extension;
        var parameters = new List<(string, object)> { (outputParameter, "./" + outputName) };
        if (command is not "ConvertFrom-ImageBase64" and not "New-ImageGif" and not "New-ImageGrid" and not "New-ImageMosaic") {
            parameters.Add(("FilePath", "./source.png"));
        }
        switch (command) {
            case "Add-ImageText":
            case "Add-ImageTextBox":
                parameters.AddRange(new[] { ("Text", (object)"Path"), ("X", 1), ("Y", 1), ("FontSize", 8), ("FontFamily", SixLabors.Fonts.SystemFonts.Collection.Families.First().Name) });
                if (command == "Add-ImageTextBox") {
                    parameters.Add(("Width", 48));
                }
                break;
            case "Add-ImageWatermark": parameters.AddRange(new[] { ("WatermarkPath", (object)"./source.png"), ("Padding", 0) }); break;
            case "Compare-Image": parameters.Add(("FilePathToCompare", "./other.png")); break;
            case "ConvertFrom-ImageBase64": parameters.Add(("Base64", Convert.ToBase64String(File.ReadAllBytes(Path.Combine(fixture.Location, "source.png"))))); break;
            case "Merge-Image": parameters.Add(("FilePathToMerge", "./source.png")); break;
            case "New-ImageAvatar":
            case "New-ImageCrop":
            case "New-ImageGrid": parameters.AddRange(new[] { ("Width", (object)32), ("Height", 24) }); break;
            case "New-ImageGif": parameters.Add(("Frames", new[] { "./source.png", "./other.png" })); break;
            case "New-ImageMosaic": parameters.AddRange(new[] { ("FilePaths", (object)new[] { "./source.png", "./other.png" }), ("Columns", 2), ("Width", 32), ("Height", 24) }); break;
            case "Remove-ImageExif": parameters.Add(("All", true)); break;
            case "Remove-ImageMetadata": parameters.AddRange(new[] { ("All", (object)true), ("Confirm", false) }); break;
            case "Set-ImageAdjust": parameters.Add(("Brightness", 0.5f)); break;
            case "Set-ImageBlur":
            case "Set-ImageSharpen": parameters.Add(("Amount", 1f)); break;
            case "Set-ImageExif": parameters.AddRange(new[] { ("ExifTag", (object)ExifTag.Software), ("Value", "path-contract") }); break;
            case "Set-ImageRotation": parameters.Add(("Degrees", 90f)); break;
        }
        fixture.Invoke(command, parameters.ToArray());
        fixture.AssertImageOutput(outputName, width, height);
        string output = Path.Combine(fixture.Location, outputName);
        if (command is "Remove-ImageExif" or "Remove-ImageMetadata") {
            Assert.Empty(PlaygroundImage.GetExifValues(output));
        }
        if (command == "Set-ImageExif") {
            Assert.Contains(PlaygroundImage.GetExifValues(output), value => value.Tag == ExifTag.Software && value.GetValue()?.ToString() == "path-contract");
        }
        if (command is "Add-ImageText" or "Add-ImageTextBox" or "Add-ImageWatermark" or "Set-ImageAdjust" or "Set-ImageBlur" or "Set-ImageSharpen") {
            using var source = SixLabors.ImageSharp.Image.Load<Rgba32>(Path.Combine(fixture.Location, "source.png"));
            using var edited = SixLabors.ImageSharp.Image.Load<Rgba32>(output);
            Assert.Contains(Enumerable.Range(0, width * height), index => source[index % width, index / width] != edited[index % width, index / width]);
        }
        if (command == "New-ImageGif") {
            using var gif = SixLabors.ImageSharp.Image.Load(output);
            Assert.Equal(2, gif.Frames.Count);
        }
    }

    [Fact]
    public void Test_PowerShell_DirectoryMetadataAndContainerOutputsUseSessionLocation() {
        using var fixture = new PowerShellPathFixture();
        fixture.Invoke("New-ImageThumbnail", ("DirectoryPath", "./inputs"), ("OutputDirectory", "./thumbs"), ("Width", 32), ("Height", 24), ("DontRespectAspectRatio", true));
        fixture.AssertImageOutput(Path.Combine("thumbs", "source.png"), 32, 24);
        fixture.Invoke("New-ImageIcon", ("FilePath", "./source.png"), ("OutputPath", "./icon.ico"), ("Size", new[] { 16 }));
        Assert.Equal(new byte[] { 0, 0, 1, 0 }, File.ReadAllBytes(Path.Combine(fixture.Location, "icon.ico")).Take(4));
        Assert.False(File.Exists(Path.Combine(fixture.ProcessDirectory, "icon.ico")));

        fixture.Invoke("Export-ImageMetadata", ("FilePath", "./source.png"), ("OutputPath", "./metadata.json"));
        using (var metadata = System.Text.Json.JsonDocument.Parse(File.ReadAllText(Path.Combine(fixture.Location, "metadata.json")))) {
            Assert.Equal(System.Text.Json.JsonValueKind.Object, metadata.RootElement.ValueKind);
        }
        fixture.Invoke("Import-ImageMetadata", ("FilePath", "./other.png"), ("MetadataPath", "./metadata.json"), ("OutputPath", "./imported.png"));
        fixture.AssertImageOutput("imported.png", 64, 48);
        Assert.Contains(PlaygroundImage.GetExifValues(Path.Combine(fixture.Location, "imported.png")), value => value.Tag == ExifTag.Software && value.GetValue()?.ToString() == "original-path-fixture");

        const string xmp = "<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />";
        File.WriteAllBytes(Path.Combine(fixture.Location, "source.heic"), CreateMinimalHeifWithPrimaryImageExifAndXmp(64, 48, CreateExifPayload("fixture"), "<x:xmpmeta />"));
        fixture.Invoke("Set-ImageHeifXmp", ("FilePath", "./source.heic"), ("FilePathOutput", "./updated.heic"), ("Xmp", xmp));
        Assert.Equal(xmp, PlaygroundImage.GetHeifXmp(Path.Combine(fixture.Location, "updated.heic")));
        fixture.Invoke("Remove-ImageHeifXmp", ("FilePath", "./updated.heic"), ("FilePathOutput", "./removed.heic"));
        Assert.Equal(string.Empty, PlaygroundImage.GetHeifXmp(Path.Combine(fixture.Location, "removed.heic")));
        foreach (string output in new[] { "metadata.json", "updated.heic", "removed.heic" }) {
            Assert.False(File.Exists(Path.Combine(fixture.ProcessDirectory, output)));
        }
    }

    [Fact]
    public void Test_PowerShell_VisualOutputsUseSessionLocation() {
        using var fixture = new PowerShellPathFixture();
        var chart = Chart.Create().WithSize(240, 160).AddBar("Path", ChartPoints.FromValues(1, 2));
        fixture.Invoke("New-ImageChart", ("Chart", chart), ("FilePath", "./chart.svg"));
        var topology = TopologyChart.Create();
        topology.Nodes.Add(new TopologyNode { Id = "path", Label = "Path" });
        fixture.Invoke("New-ImageTopology", ("Chart", topology), ("FilePath", "./topology.svg"));
        fixture.Invoke("Export-ImageVisualArtifact", ("Artifact", chart.ToVisualArtifact()), ("FilePath", "./artifact.svg"));
        foreach (string name in new[] { "chart.svg", "topology.svg", "artifact.svg" }) {
            var document = System.Xml.Linq.XDocument.Load(Path.Combine(fixture.Location, name));
            Assert.Equal("svg", document.Root!.Name.LocalName);
            Assert.Contains("Path", document.ToString());
            Assert.False(File.Exists(Path.Combine(fixture.ProcessDirectory, name)));
        }
    }

    [Theory]
    [InlineData("Get-Image", "FilePath")]
    [InlineData("Resize-Image", "FilePath")]
    [InlineData("New-ImageQRCode", "FilePath")]
    [InlineData("New-ImageBarCode", "FilePath")]
    public void Test_PowerShell_RejectsNonFileSystemProviders(string command, string parameter) {
        using var fixture = new PowerShellPathFixture();
        var parameters = new List<(string, object)> { (parameter, "Variable:/image-path-fixture") };
        if (command == "Resize-Image") {
            parameters.AddRange(new[] { ("OutputPath", (object)"./output.png"), ("Width", 32) });
        }
        if (command == "New-ImageQRCode") {
            parameters.Add(("Content", "provider-rejection"));
        }
        if (command == "New-ImageBarCode") {
            parameters.AddRange(new[] { ("Type", (object)"EAN"), ("Value", "9012341234571") });
        }
        var exception = Assert.ThrowsAny<RuntimeException>(() => fixture.Invoke(command, parameters.ToArray()));
        Assert.Contains("FileSystem provider", exception.Message);
        Assert.False(File.Exists(Path.Combine(fixture.Location, "output.png")));
    }

    [Fact]
    public void Test_PowerShell_RejectsMissingFilesAndBlankInputs() {
        using var fixture = new PowerShellPathFixture();
        var blank = Assert.ThrowsAny<RuntimeException>(() => fixture.Invoke("Get-Image", ("FilePath", " ")));
        Assert.Contains("A non-empty path is required", blank.Message);
        var missing = Assert.ThrowsAny<RuntimeException>(() => fixture.Invoke("Get-Image", ("FilePath", "./missing.png")));
        Assert.StartsWith("GetImageFileNotFound", missing.ErrorRecord.FullyQualifiedErrorId);
        var asyncMissing = Assert.ThrowsAny<RuntimeException>(() => fixture.Invoke("Resize-Image", ("FilePath", "./missing.png"), ("OutputPath", "./resized.png"), ("Width", 32)));
        Assert.StartsWith("ResizeImageFileNotFound", asyncMissing.ErrorRecord.FullyQualifiedErrorId);
        Assert.False(File.Exists(Path.Combine(fixture.Location, "resized.png")));
    }

    [Fact]
    public async Task Test_PowerShell_StillDownloadsHttpInputs() {
        using var fixture = new PowerShellPathFixture();
        byte[] png = File.ReadAllBytes(Path.Combine(fixture.Location, "source.png"));
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        int port = ((IPEndPoint)listener.LocalEndpoint).Port;
        Task response = Task.Run(async () => {
            using var client = await listener.AcceptTcpClientAsync(cancellation.Token);
            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.ASCII, false, 1024, true);
            while (!string.IsNullOrEmpty(await reader.ReadLineAsync(cancellation.Token))) { }
            byte[] header = Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type: image/png\r\nContent-Length: {png.Length}\r\nConnection: close\r\n\r\n");
            await stream.WriteAsync(header, cancellation.Token);
            await stream.WriteAsync(png, cancellation.Token);
        });
        try {
            using var image = Assert.IsType<PlaygroundImage>(fixture.Invoke("Get-Image", ("FilePath", $"http://127.0.0.1:{port}/source.png")).Single().BaseObject);
            Assert.Equal(64, image.Width);
            Assert.Equal(48, image.Height);
            await response;
            Assert.Equal(fixture.ProcessDirectory, Environment.CurrentDirectory);
        } finally {
            cancellation.Cancel();
            listener.Stop();
            Helpers.CleanupTempFiles();
        }
    }

    private sealed class PowerShellPathFixture : IDisposable {
        private readonly string _originalDirectory = Environment.CurrentDirectory;
        private readonly string _root = Path.Combine(Path.GetTempPath(), "ImagePlayground-paths-" + Guid.NewGuid().ToString("N"));
        private readonly string _environmentName = "IMAGEPLAYGROUND_PATH_" + Guid.NewGuid().ToString("N");
        private readonly Runspace _runspace;
        public string Location { get; }
        public string ProcessDirectory { get; }

        public PowerShellPathFixture() {
            Location = Path.Combine(_root, "location");
            ProcessDirectory = Path.Combine(_root, "process");
            Directory.CreateDirectory(Location);
            Directory.CreateDirectory(ProcessDirectory);
            Directory.CreateDirectory(Path.Combine(Location, "inputs"));
            using (var source = new SixLabors.ImageSharp.Image<Rgba32>(64, 48)) {
                for (int y = 0; y < source.Height; y++) {
                    for (int x = 0; x < source.Width; x++) {
                        source[x, y] = new Rgba32((byte)(x * 4), (byte)(y * 5), 128);
                    }
                }
                source.Metadata.ExifProfile = new ExifProfile();
                source.Metadata.ExifProfile.SetValue(ExifTag.Software, "original-path-fixture");
                source.Save(Path.Combine(Location, "source.png"));
                source[0, 0] = new Rgba32(255, 255, 255);
                source.Save(Path.Combine(Location, "other.png"));
            }
            File.Copy(Path.Combine(Location, "source.png"), Path.Combine(Location, "inputs", "source.png"));
            var state = InitialSessionState.CreateDefault2();
            state.ImportPSModule(new[] { "Microsoft.PowerShell.Management", typeof(global::ImagePlayground.PowerShell.GetImageCmdlet).Assembly.Location });
            _runspace = RunspaceFactory.CreateRunspace(state);
            try {
                _runspace.Open();
                Environment.CurrentDirectory = ProcessDirectory;
                Invoke("Set-Location", ("LiteralPath", Location));
                Invoke("New-PSDrive", ("Name", "ImagePaths"), ("PSProvider", "FileSystem"), ("Root", Location));
                Environment.SetEnvironmentVariable(_environmentName, Location);
            } catch {
                Dispose();
                throw;
            }
        }

        public string InputPath(string kind, string name) => kind switch {
            "environment" => "%" + _environmentName + "%/" + name,
            "drive" => "ImagePaths:/" + name,
            _ => "./" + name
        };

        public PSObject[] Invoke(string command, params (string Name, object Value)[] parameters) {
            using var shell = PowerShellHost.Create();
            shell.Runspace = _runspace;
            shell.AddCommand(command);
            foreach (var parameter in parameters) {
                shell.AddParameter(parameter.Name, parameter.Value);
            }
            var result = shell.Invoke();
            Assert.False(shell.HadErrors, string.Join(Environment.NewLine, shell.Streams.Error.Select(error => error.ToString())));
            return result.ToArray();
        }

        public void AssertImageOutput(string relativePath, int width, int height) {
            using var output = SixLabors.ImageSharp.Image.Load(Path.Combine(Location, relativePath));
            Assert.Equal(width, output.Width);
            Assert.Equal(height, output.Height);
            Assert.False(File.Exists(Path.Combine(ProcessDirectory, relativePath)));
            Assert.Equal(ProcessDirectory, Environment.CurrentDirectory);
        }

        public void Dispose() {
            _runspace.Dispose();
            Environment.SetEnvironmentVariable(_environmentName, null);
            Environment.CurrentDirectory = _originalDirectory;
            Directory.Delete(_root, true);
        }
    }
}
