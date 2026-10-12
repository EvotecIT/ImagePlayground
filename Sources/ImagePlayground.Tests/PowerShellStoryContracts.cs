#if NET8_0
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text.Json;
using ChartForgeX.Stories;
using ChartForgeX.Terminal;
using global::ImagePlayground.PowerShell;
using global::ImagePlayground.PowerShell.Stories;
using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Fact]
    public void Test_PowerShell_StoryReplayPreservesOrderedPipelineRecordsWithoutExecutingCommands() {
        using var fixture = new PowerShellPathFixture();
        var marker = Path.Combine(fixture.Location, "must-not-run.txt");
        var replay = Assert.IsType<StoryReplay>(fixture.InvokeScript("""
            @(
                @{ TimestampSeconds = 0; Kind = 'Command'; Text = "Set-Content -LiteralPath '$marker' -Value executed" }
                [pscustomobject]@{ TimestampSeconds = 0.5; Kind = 'Output'; Text = 'Starting' }
                @{ TimestampSeconds = 1; Kind = 'ReplaceLine'; Text = 'Ready'; Tone = 'Success' }
                @{ TimestampSeconds = 1.5; Kind = 'Directory'; Text = '/work' }
                @{ TimestampSeconds = 2; Kind = 'Marker'; Text = 'Result' }
                @{ TimestampSeconds = 2.5; Kind = 'OpenTab'; TabId = 'build'; Title = 'Build' }
                @{ TimestampSeconds = 3; Kind = 'SelectTab'; TabId = 'build' }
                @{ TimestampSeconds = 3.5; Kind = 'Clear' }
            ) | New-ImageStoryReplay -DurationSeconds 4 -WorkingDirectory demo
            """, ("marker", marker)).Single().BaseObject);

        Assert.Equal(8, replay.Events.Count);
        Assert.Equal(TimeSpan.FromSeconds(3.5), replay.Events.Last().Timestamp);
        Assert.Equal(StoryReplayEventKind.Command, replay.Events[0].Kind);
        Assert.Equal(StoryReplayEventKind.Clear, replay.Events.Last().Kind);
        Assert.False(File.Exists(marker));
        var compressed = replay.CompressPauses(TimeSpan.FromSeconds(0.25));
        Assert.Equal(TimeSpan.FromSeconds(0.5), compressed.Events[1].OriginalTimestamp);
        Assert.Equal(TimeSpan.FromSeconds(0.25), compressed.Events[1].Timestamp);
        var panel = Assert.IsType<ImageStoryPanelSpec>(fixture.Invoke("New-ImageStoryPanel",
            ("Id", "replay"), ("Replay", replay), ("TerminalOptions", new VisualStoryTerminalOptions(18))).Single().BaseObject);
        var surface = Assert.IsType<VisualStoryReplaySurface>(panel.Surface);
        Assert.Same(replay, surface.Replay);
        Assert.Contains("Ready", surface.AccessibleText);
    }

    [Theory]
    [InlineData("@{ TimestampSeconds = 0; Kind = 'Command' }", "requires Text")]
    [InlineData("@{ Kind = 'Output'; Text = 'ready' }", "requires TimestampSeconds")]
    [InlineData("@{ TimestampSeconds = 0; Kind = 999 }", "Cannot convert value")]
    public void Test_PowerShell_StoryReplayRejectsIncompleteOrUnsupportedRecords(string record, string message) {
        using var fixture = new PowerShellPathFixture();
        var error = fixture.InvokeScript("try { New-ImageStoryReplay -DurationSeconds 1 -Events (" + record +
            ") -ErrorAction Stop } catch { $_.Exception.Message }").Single().BaseObject.ToString();
        Assert.Contains(message, error);
    }

    [Fact]
    public void Test_PowerShell_StorySourceTimelinePreservesSourceAndViewportOptions() {
        using var fixture = new PowerShellPathFixture();
        var source = StorySourceText.Create(string.Empty, "PowerShell");
        var timeline = StorySourceTimeline.Create(source).Type("Write-Output ready", TimeSpan.FromSeconds(1));
        var options = new VisualStorySourceOptions("demo.ps1", 18, true);
        var panel = Assert.IsType<ImageStoryPanelSpec>(fixture.Invoke("New-ImageStoryPanel",
            ("Id", "source"), ("SourceTimeline", timeline), ("SourceOptions", options)).Single().BaseObject);
        var surface = Assert.IsType<VisualStorySourceSurface>(panel.Surface);
        Assert.Same(timeline, surface.Timeline);
        var actualOptions = Assert.IsType<VisualStorySourceOptions>(surface.Options);
        Assert.Equal("demo.ps1", actualOptions.FileName);
        Assert.Equal(18, actualOptions.FontSize);
    }

    [Fact]
    public void Test_PowerShell_StoryBundleSharesPlaybackAndPreservesExistingFilesOnEncodingFailure() {
        using var fixture = new PowerShellPathFixture();
        var panel = fixture.Invoke("New-ImageStoryPanel", ("Id", "result"), ("Text", "Ready")).Single().BaseObject;
        var scene = fixture.Invoke("New-ImageStoryScene", ("Id", "complete"), ("Title", "Complete"),
            ("DurationSeconds", 1D), ("Panels", new[] { panel })).Single().BaseObject;
        var outcome = fixture.Invoke("New-ImageStoryOutcome", ("Id", "ready"), ("Label", "Ready"), ("PanelId", "result")).Single().BaseObject;
        Directory.CreateDirectory(Path.Combine(fixture.Location, "bundle"));
        File.WriteAllText(Path.Combine(fixture.Location, "bundle", "story.json"), "Previous manifest");
        fixture.Invoke("New-ImageStory", ("Title", "Ready — żółw"), ("Scenes", new[] { scene }), ("Outcomes", new[] { outcome }),
            ("FilePath", "./story.svg"), ("BundlePath", "./bundle"), ("BundleFormats", new[] { "Svg", "Html", "Transcript" }),
            ("Width", 480), ("Height", 320), ("EndHoldSeconds", 0.5D), ("TransitionSeconds", 0D), ("PlayCount", 2), ("Player", true));
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(fixture.Location, "bundle", "story.json")));
        Assert.Equal("Ready — żółw", manifest.RootElement.GetProperty("title").GetString());
        var playback = manifest.RootElement.GetProperty("playback");
        Assert.Equal(1.5, playback.GetProperty("durationSeconds").GetDouble());
        Assert.Equal(2, playback.GetProperty("playCount").GetInt32());
        foreach (var path in new[] { "story.svg", "bundle/story.svg", "bundle/story.html" }) {
            var text = File.ReadAllText(Path.Combine(fixture.Location, path));
            Assert.Contains("data-cfx-motion-duration=\"1.5\"", text);
            Assert.Contains("data-cfx-motion-plays=\"2\"", text);
        }
        Assert.Contains("Ready", File.ReadAllText(Path.Combine(fixture.Location, "bundle", "story.txt")));

        var destination = Path.Combine(fixture.Location, "keep.apng");
        File.WriteAllText(destination, "Previous deliverable");
        var error = fixture.InvokeScript("""
            try {
                New-ImageStory -Title Ready -Scenes $scene -Outcomes $outcome -FilePath ./keep.apng `
                    -FramesPerSecond 60 -MaximumFrames 2 -ErrorAction Stop
            } catch { $_.Exception.Message }
            """, ("scene", scene), ("outcome", outcome));
        Assert.Single(error);
        Assert.Contains("frame budget", error.Single().BaseObject.ToString());
        Assert.Equal("Previous deliverable", File.ReadAllText(destination));
        var conflict = fixture.InvokeScript("""
            try {
                New-ImageStory -Title Ready -Scenes $scene -Outcomes $outcome -FilePath ./conflict.svg `
                    -NoLoop -PlayCount 2 -ErrorAction Stop
            } catch { $_.Exception.Message }
            """, ("scene", scene), ("outcome", outcome));
        Assert.Contains("NoLoop requires PlayCount 1", conflict.Single().BaseObject.ToString());
        Assert.False(File.Exists(Path.Combine(fixture.Location, "conflict.svg")));
    }

    [Theory]
    [InlineData("directory-file")]
    [InlineData("export-directory")]
    [InlineData("file-ancestor")]
    [InlineData("same-path")]
    [InlineData("hard-link")]
    public void Test_PowerShell_StoryDestinationPreflightRejectsUnsafePlansWithoutWriting(string conflict) {
        using var fixture = new PowerShellPathFixture();
        var root = Path.Combine(fixture.Location, "exports");
        Directory.CreateDirectory(root);
        var existing = Path.Combine(root, "first.svg");
        File.WriteAllText(existing, "Original export");
        string[] files = { existing };
        if (conflict == "directory-file") {
            root = existing;
        } else if (conflict == "export-directory") {
            files = new[] { root };
        } else if (conflict == "file-ancestor") {
            files = new[] { Path.Combine(existing, "nested.svg") };
        } else if (conflict == "same-path") {
            files = new[] { existing, Path.Combine(root, "child", "..", "first.svg") };
        } else {
            var alias = Path.Combine(root, "alias.svg");
            fixture.Invoke("New-Item", ("ItemType", "HardLink"), ("Path", alias), ("Target", existing));
            files = new[] { existing, alias };
        }
        Assert.Throws<PSArgumentException>(() => ImageStoryFileSafety.ValidateDestinations(root, files));
        Assert.Equal("Original export", File.ReadAllText(existing));
    }

    [Theory]
    [InlineData("inside-output")]
    [InlineData("destination")]
    [InlineData("hard-link")]
    public void Test_PowerShell_StoryInputPreflightProtectsCapturedInputsFromExportAliases(string conflict) {
        using var fixture = new PowerShellPathFixture();
        var root = Path.Combine(fixture.Location, "exports");
        Directory.CreateDirectory(root);
        var source = Path.Combine(conflict == "inside-output" ? root : fixture.Location, "capture.txt");
        File.WriteAllText(source, "Original captured input");
        var destination = conflict == "destination" ? source : Path.Combine(root, "story.txt");
        if (conflict == "hard-link") {
            fixture.Invoke("New-Item", ("ItemType", "HardLink"), ("Path", destination), ("Target", source));
        }
        Assert.Throws<PSArgumentException>(() => ImageStoryFileSafety.ValidateInputs(root, new[] { source }, new[] { destination }));
        Assert.Equal("Original captured input", File.ReadAllText(source));
        Assert.False(File.Exists(Path.Combine(root, "story.svg")));
    }

    [Fact]
    public void Test_PowerShell_StoryPreflightAllowsDistinctExportsAndExternalInputsWithoutMutatingThem() {
        using var fixture = new PowerShellPathFixture();
        var root = Path.Combine(fixture.Location, "exports");
        Directory.CreateDirectory(root);
        var source = Path.Combine(fixture.Location, "capture.txt");
        var existing = Path.Combine(root, "story.svg");
        var planned = Path.Combine(root, "new.svg");
        File.WriteAllText(source, "Captured output");
        File.WriteAllText(existing, "Existing export");
        ImageStoryFileSafety.ValidateDestinations(root, new[] { existing, planned });
        ImageStoryFileSafety.ValidateInputs(root, new[] { source }, new[] { existing, planned });
        Assert.Equal("Captured output", File.ReadAllText(source));
        Assert.Equal("Existing export", File.ReadAllText(existing));
        Assert.False(File.Exists(planned));
    }
}
#endif
