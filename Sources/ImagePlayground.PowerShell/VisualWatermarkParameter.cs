using System;
using ChartForgeX.VisualArtifacts;

namespace ImagePlayground.PowerShell;

internal static class VisualWatermarkParameter {
    public static VisualWatermark[] Normalize(VisualWatermark[]? watermarks) {
        if (watermarks == null || watermarks.Length == 0) {
            return Array.Empty<VisualWatermark>();
        }
        for (var index = 0; index < watermarks.Length; index++) {
            if (watermarks[index] != null) {
                return watermarks;
            }
        }

        // PowerShell can represent an explicitly supplied $null array argument as
        // a one-element array whose only entry is null. Treat that as "no marks".
        return Array.Empty<VisualWatermark>();
    }
}
