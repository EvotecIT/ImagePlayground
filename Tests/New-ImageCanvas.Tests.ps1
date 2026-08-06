Describe 'New-ImageCanvas' {
    BeforeAll {
        $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'renders a social-preview canvas from native ChartForgeX layers' {
        $file = Join-Path -Path $TestDir -ChildPath 'social-preview.png'
        Remove-Item -Path $file -ErrorAction SilentlyContinue

        $canvas = New-ImageCanvas -Preset SocialPreview -Title 'ChartForgeX release' -Backdrop TechHorizon -LayerDefinition {
            New-ImageCanvasText -X 72 -Y 72 -Width 1000 -Text 'ChartForgeX 1.3' -FontSize 58 -Color White -Emphasized
            New-ImageCanvasInfoTile -X 72 -Y 190 -Width 380 -Height 150 -Icon SVG -Label Renderer -Value 'Dependency-free' -Detail 'SVG + PNG' -MiniChartKind Area -MiniValues 2, 4, 5, 8
        } -FilePath $file -PassThru

        $canvas | Should -BeOfType 'ChartForgeX.Composition.VisualCanvas'
        $canvas.Width | Should -Be 1200
        $canvas.Height | Should -Be 630
        $canvas.Layers.Count | Should -Be 2
        Test-Path -Path $file | Should -BeTrue
        $image = [ImagePlayground.Image]::Load($file)
        try {
            $image.Width | Should -Be 1200
            $image.Height | Should -Be 630
        } finally {
            $image.Dispose()
        }
    }

    It 'rejects unsupported output before invoking canvas authoring' {
        $script:canvasDefinitionInvoked = $false
        {
            New-ImageCanvas -LayerDefinition {
                $script:canvasDefinitionInvoked = $true
                New-ImageCanvasText -X 10 -Y 10 -Width 100 -Text Test
            } -FilePath (Join-Path -Path $TestDir -ChildPath 'canvas.invalid')
        } | Should -Throw
        $script:canvasDefinitionInvoked | Should -BeFalse
    }

    It 'rejects multiple pipeline canvases for one output path' {
        $canvases = @(
            [ChartForgeX.Composition.VisualCanvas]::Create(100, 100)
            [ChartForgeX.Composition.VisualCanvas]::Create(100, 100)
        )
        { $canvases | New-ImageCanvas -FilePath (Join-Path -Path $TestDir -ChildPath 'multiple.svg') } | Should -Throw
    }
}
