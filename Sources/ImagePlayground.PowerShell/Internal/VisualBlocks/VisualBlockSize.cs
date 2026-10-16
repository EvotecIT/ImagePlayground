using System.Management.Automation;
using ChartForgeX.Primitives;
using ChartForgeX.VisualBlocks;

namespace ImagePlayground.PowerShell;

/// <summary>Applies optional -Width and -Height parameters to a visual block, keeping the unspecified dimension.</summary>
internal static class VisualBlockSize {
    internal const int Maximum = 10000;

    public static void Apply(PSCmdlet cmdlet, IVisualBlock block, int width, int height) {
        var bound = cmdlet.MyInvocation.BoundParameters;
        var hasWidth = bound.ContainsKey("Width");
        var hasHeight = bound.ContainsKey("Height");
        if (!hasWidth && !hasHeight) {
            return;
        }

        var current = block.Options.Size;
        block.Options.Size = new ChartSize(hasWidth ? width : current.Width, hasHeight ? height : current.Height);
    }
}
