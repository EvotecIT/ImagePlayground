using System;
using System.Text;
using ChartForgeX.Interactivity.Html;
using ChartForgeX.Stories;
using OfficeIMO.Drawing;

namespace ImagePlayground.PowerShell;

public sealed partial class NewImageStoryCmdlet {
    private void WriteArtifact(PreparedVisualStory story, VisualStoryFrameOptions frames, string extension, string path) {
        byte[] bytes;
        if (extension.Equals(".gif", StringComparison.OrdinalIgnoreCase)) bytes = story.ToGif(frames);
        else if (extension.Equals(".apng", StringComparison.OrdinalIgnoreCase)) bytes = story.ToApng(frames);
        else if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase)) bytes = story.ToPng();
        else {
            var text = extension.Equals(".svg", StringComparison.OrdinalIgnoreCase) ? story.ToAnimatedSvg(frames)
                : extension.Equals(".html", StringComparison.OrdinalIgnoreCase) || extension.Equals(".htm", StringComparison.OrdinalIgnoreCase)
                    ? Player.IsPresent ? new HtmlMotionPlayerRenderer().RenderPage(story.ToAnimatedSvg(frames), story.Title) : story.ToHtmlPage(frames)
                    : story.ToTranscript();
            bytes = Encoding.UTF8.GetBytes(text);
        }
        OfficeImageFileWriter.WriteAllBytes(path, bytes);
    }
}
