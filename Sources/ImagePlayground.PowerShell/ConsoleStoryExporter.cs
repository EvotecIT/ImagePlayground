using System;
using System.IO;
using System.Management.Automation;
using ChartForgeX;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Writes one terminal story through the shared ChartForgeX format renderers.</summary>
internal static class ConsoleStoryExporter {
    internal static string ResolveOutputPath(PSCmdlet cmdlet, string path) {
        if (cmdlet == null) throw new ArgumentNullException(nameof(cmdlet));

        var output = PowerShellPathResolver.ResolveFileSystemPath(cmdlet, path);
        ValidateExtension(Path.GetExtension(output));
        if (Directory.Exists(output)) {
            throw new PSArgumentException(
                $"Console story output must resolve to a file, but an existing directory was found: {output}",
                "Path");
        }
        return output;
    }

    internal static string Write(
        PSCmdlet cmdlet,
        TerminalStory story,
        string path,
        TerminalStoryAnimationOptions animationOptions) {
        if (cmdlet == null) throw new ArgumentNullException(nameof(cmdlet));
        if (story == null) throw new ArgumentNullException(nameof(story));
        if (animationOptions == null) throw new ArgumentNullException(nameof(animationOptions));

        var output = ResolveOutputPath(cmdlet, path);
        var extension = Path.GetExtension(output);
        var directory = Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) {
            Directory.CreateDirectory(directory!);
        }

        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase)) {
            story.SaveSvg(output);
        } else if (extension.Equals(".html", StringComparison.OrdinalIgnoreCase) || extension.Equals(".htm", StringComparison.OrdinalIgnoreCase)) {
            story.SaveHtml(output);
        } else if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase)) {
            story.SavePng(output);
        } else if (extension.Equals(".gif", StringComparison.OrdinalIgnoreCase)) {
            story.SaveGif(output, animationOptions);
        } else {
            story.SaveApng(output, animationOptions);
        }

        return output;
    }

    private static void ValidateExtension(string extension) {
        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".html", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".htm", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".png", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".gif", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".apng", StringComparison.OrdinalIgnoreCase)) {
            return;
        }

        throw new PSArgumentException("Console story output supports only .svg, .html, .htm, .png, .gif, or .apng file extensions.", "Path");
    }
}
