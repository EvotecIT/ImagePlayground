using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for HelpersPath.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_ResolvePath_ExpandsEnvironment() {
        string testDir = _directoryWithTests;
        Environment.SetEnvironmentVariable("TEST_PATH", testDir);
        string result = Helpers.ResolvePath("%TEST_PATH%/file.txt");
        Assert.Equal(Path.Combine(testDir, "file.txt"), result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Test_ResolvePath_Invalid(string value) {
        Assert.Throws<ArgumentException>(() => Helpers.ResolvePath(value!));
    }

    [Fact]
    public void Test_ReadFileChecked_ReturnsContent() {
        string file = Path.Combine(_directoryWithTests, "readme.txt");
        File.WriteAllText(file, "hello");
        string content = Helpers.ReadFileChecked(file);
        Assert.Equal("hello", content);
    }

    [Fact]
    public void Test_ReadFileChecked_ThrowsWhenMissing() {
        string file = Path.Combine(_directoryWithTests, "missing.txt");
        if (File.Exists(file)) File.Delete(file);
        Assert.Throws<FileNotFoundException>(() => Helpers.ReadFileChecked(file));
    }

    [Fact]
    public async Task Test_ReadFileCheckedAsync_ReturnsContent() {
        string file = Path.Combine(_directoryWithTests, "readme_async.txt");
        File.WriteAllText(file, "async");
        string content = await Helpers.ReadFileCheckedAsync(file);
        Assert.Equal("async", content);
    }

    [Fact]
    public async Task Test_ReadFileCheckedAsync_ThrowsWhenMissing() {
        string file = Path.Combine(_directoryWithTests, "missing_async.txt");
        if (File.Exists(file)) File.Delete(file);
        await Assert.ThrowsAsync<FileNotFoundException>(async () => await Helpers.ReadFileCheckedAsync(file));
    }

    [Fact]
    public async Task Test_ResolvePath_DownloadsUrlAsync() {
        var tcp = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        tcp.Start();
        int port = ((System.Net.IPEndPoint)tcp.LocalEndpoint).Port;
        tcp.Stop();
        string prefix = $"http://localhost:{port}/";

        using var listener = new HttpListener();
        listener.Prefixes.Add(prefix);
        listener.Start();
        var serverTask = System.Threading.Tasks.Task.Run(() => {
            var context = listener.GetContext();
            byte[] data = System.Text.Encoding.UTF8.GetBytes("hello");
            context.Response.ContentLength64 = data.Length;
            context.Response.OutputStream.Write(data, 0, data.Length);
            context.Response.OutputStream.Close();
        });

        string path = await Helpers.ResolvePathAsync(prefix + "file.txt");
        await serverTask;
        string content = File.ReadAllText(path);
        Assert.Equal("hello", content);
        Helpers.CleanupTempFiles();
        Assert.False(File.Exists(path));
    }

    [Fact]
    public void Test_CleanupTempFiles_Idempotent() {
        var tcp = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        tcp.Start();
        int port = ((System.Net.IPEndPoint)tcp.LocalEndpoint).Port;
        tcp.Stop();
        string prefix = $"http://localhost:{port}/";

        using var listener = new HttpListener();
        listener.Prefixes.Add(prefix);
        listener.Start();
        var serverTask = System.Threading.Tasks.Task.Run(() => {
            var context = listener.GetContext();
            byte[] data = System.Text.Encoding.UTF8.GetBytes("hello");
            context.Response.ContentLength64 = data.Length;
            context.Response.OutputStream.Write(data, 0, data.Length);
            context.Response.OutputStream.Close();
        });

        string path = Helpers.ResolvePath(prefix + "file.txt");
        serverTask.Wait();
        string content = File.ReadAllText(path);
        Assert.Equal("hello", content);
        Helpers.CleanupTempFiles();
        Assert.False(File.Exists(path));
        Helpers.CleanupTempFiles();
    }
}
