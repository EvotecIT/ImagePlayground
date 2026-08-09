Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

New-ImageCanvas -Preset SocialPreview -Title 'ChartForgeX release' -Backdrop TechHorizon -LayerDefinition {
    New-ImageCanvasText -X 72 -Y 72 -Width 1000 -Text 'ChartForgeX 1.3' -FontSize 58 -Color White -Emphasized
    New-ImageCanvasInfoTile -X 72 -Y 190 -Width 380 -Height 150 -Icon SVG -Label Renderer -Value 'Dependency-free' -Detail 'SVG + PNG' -MiniChartKind Area -MiniValues 2, 4, 5, 8
} -FilePath "$PSScriptRoot\chartforgex-social-preview.png"
