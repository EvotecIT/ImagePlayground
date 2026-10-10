param(
    [string] $OutputDirectory = (Join-Path -Path $PSScriptRoot -ChildPath 'Output'),
    [ValidateSet('Light', 'Dark')]
    [string] $Theme = 'Dark'
)

Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

$tokens = if ($Theme -eq 'Dark') {
    [ChartForgeX.Themes.VisualDesignTokens]::GraphiteDark()
} else {
    [ChartForgeX.Themes.VisualDesignTokens]::GraphiteLight()
}
$suffix = $Theme.ToLowerInvariant()

New-ImageCanvas -Preset SocialPreview -Theme $Theme -Title 'Service health overview' -LayerDefinition {
    New-ImageCanvasText -X 72 -Y 72 -Width 1056 -Text 'Service health' -FontSize 58 -Color $tokens.Foreground -Emphasized
    New-ImageCanvasText -X 72 -Y 154 -Width 1056 -Text 'A clear view of requests, latency, and capacity' -FontSize 26 -Color $tokens.MutedForeground
    New-ImageCanvasInfoTile -X 72 -Y 260 -Width 336 -Height 176 -Icon API -Label Requests -Value '12,840' -Detail '+12% this week' -Accent $tokens.Accent -MiniChartKind Area -MiniValues 8200, 9400, 10100, 12840
    New-ImageCanvasInfoTile -X 432 -Y 260 -Width 336 -Height 176 -Icon DB -Label 'Database latency' -Value '32 ms' -Detail 'Above the 25 ms target' -Accent $tokens.Warning -MiniChartKind Sparkline -MiniValues 18, 21, 24, 32
    New-ImageCanvasInfoTile -X 792 -Y 260 -Width 336 -Height 176 -Icon CPU -Label Capacity -Value '74%' -Detail '26% available' -Accent $tokens.Positive -Progress 0.74
} -FilePath (Join-Path -Path $OutputDirectory -ChildPath "social-preview-$suffix.png")

# A compact delivery is authored vertically instead of shrinking the 1200px social preview.
New-ImageCanvas -Width 368 -Height 620 -Theme $Theme -Title 'Service health overview' -LayerDefinition {
    New-ImageCanvasText -X 16 -Y 24 -Width 336 -Text 'Service health' -FontSize 32 -Color $tokens.Foreground -Emphasized
    New-ImageCanvasText -X 16 -Y 72 -Width 336 -Text 'Requests, latency, and capacity' -FontSize 15 -Color $tokens.MutedForeground
    New-ImageCanvasInfoTile -X 16 -Y 120 -Width 336 -Height 144 -Icon API -Label Requests -Value '12,840' -Detail '+12% this week' -Accent $tokens.Accent -MiniChartKind Area -MiniValues 8200, 9400, 10100, 12840
    New-ImageCanvasInfoTile -X 16 -Y 280 -Width 336 -Height 144 -Icon DB -Label 'Database latency' -Value '32 ms' -Detail 'Above the 25 ms target' -Accent $tokens.Warning -MiniChartKind Sparkline -MiniValues 18, 21, 24, 32
    New-ImageCanvasInfoTile -X 16 -Y 440 -Width 336 -Height 144 -Icon CPU -Label Capacity -Value '74%' -Detail '26% available' -Accent $tokens.Positive -Progress 0.74
} -FilePath (Join-Path -Path $OutputDirectory -ChildPath "social-preview-$suffix-compact.png")
