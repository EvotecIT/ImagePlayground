Describe 'New-ImageCanvas' {
    BeforeAll {
        if ($env:IMAGEPLAYGROUND_TEST_MODULE_PATH) {
            Import-Module -Name $env:IMAGEPLAYGROUND_TEST_MODULE_PATH -Force
        } else {
            $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
            Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        }
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

    It 'applies canonical canvas colors and preserves an explicit background' -TestCases @(
        @{ Theme = 'Light' }
        @{ Theme = 'Dark' }
    ) {
        param($Theme)
        $tokens = if ($Theme -eq 'Dark') {
            [ChartForgeX.Themes.VisualDesignTokens]::GraphiteDark()
        } else {
            [ChartForgeX.Themes.VisualDesignTokens]::GraphiteLight()
        }
        $file = Join-Path -Path $TestDir -ChildPath ("canvas-{0}.svg" -f $Theme)
        $canvas = New-ImageCanvas -Width 368 -Height 240 -Theme $Theme -BackgroundTop '#334455' -LayerDefinition {
            New-ImageCanvasInfoTile -X 16 -Y 16 -Width 336 -Height 176 -Icon API -Label Requests -Value '12,840'
        } -FilePath $file -PassThru

        $canvas.Width | Should -Be 368
        $canvas.Theme.TileValueColor.ToCss() | Should -BeExactly $tokens.Foreground.ToCss()
        $canvas.Theme.FontFamily | Should -BeExactly $tokens.FontFamily
        $canvas.BackgroundTop.ToCss() | Should -BeExactly ([ChartForgeX.Primitives.ChartColor]::FromHex('#334455')).ToCss()
    }

    It 'preserves a supplied canvas theme when no theme override is requested' {
        $canvas = [ChartForgeX.Composition.VisualCanvas]::Create(368, 240)
        $customColor = [ChartForgeX.Primitives.ChartColor]::FromHex('#ABCDEF')
        $canvas.Theme.TileValueColor = $customColor
        [void] $canvas.WithBackground([ChartForgeX.Primitives.ChartColor]::FromHex('#123456'))
        $exported = $canvas | New-ImageCanvas -FilePath (Join-Path -Path $TestDir -ChildPath 'custom-canvas.svg') -PassThru

        [object]::ReferenceEquals($canvas, $exported) | Should -BeTrue
        $exported.Theme.TileValueColor.ToCss() | Should -BeExactly $customColor.ToCss()
        $exported.BackgroundTop.ToCss() | Should -BeExactly ([ChartForgeX.Primitives.ChartColor]::FromHex('#123456')).ToCss()
    }

    It 'uses the canvas foreground for text without an explicit color in <Theme> output' -TestCases @(
        @{ Theme = 'Light' }
        @{ Theme = 'Dark' }
    ) {
        param($Theme)

        $tokens = if ($Theme -eq 'Dark') {
            [ChartForgeX.Themes.VisualDesignTokens]::GraphiteDark()
        } else {
            [ChartForgeX.Themes.VisualDesignTokens]::GraphiteLight()
        }
        $file = Join-Path -Path $TestDir -ChildPath ("canvas-inherited-text-{0}.png" -f $Theme)
        $svgFile = Join-Path -Path $TestDir -ChildPath ("canvas-inherited-text-{0}.svg" -f $Theme)
        $controlFile = Join-Path -Path $TestDir -ChildPath ("canvas-explicit-text-{0}.png" -f $Theme)
        $canvas = New-ImageCanvas -Width 368 -Height 120 -Theme $Theme -LayerDefinition {
            New-ImageCanvasText -X 16 -Y 16 -Width 336 -Text 'Service health' -FontSize 32
        } -FilePath $file -PassThru
        New-ImageCanvas -Width 368 -Height 120 -Theme $Theme -LayerDefinition {
            New-ImageCanvasText -X 16 -Y 16 -Width 336 -Text 'Service health' -FontSize 32 -Color $tokens.Foreground
        } -FilePath $controlFile

        New-ImageCanvas -Canvas $canvas -FilePath $svgFile
        [xml] $svg = [System.IO.File]::ReadAllText($svgFile)
        $text = $svg.SelectSingleNode("//*[local-name()='text' and @data-cfx-role='visual-canvas-text']")
        $text.GetAttribute('fill') | Should -BeExactly $tokens.Foreground.ToCss()
        $actual = [ImagePlayground.Image]::Load($file)
        $control = [ImagePlayground.Image]::Load($controlFile)
        try {
            $actual.Compare($control).ChangedPixels | Should -Be 0
        } finally {
            $actual.Dispose()
            $control.Dispose()
        }
    }

    It 'preserves an explicitly selected <Color> text color on a light canvas' -TestCases @(
        @{ Color = 'White'; R = 255; G = 255; B = 255; A = 255 }
        @{ Color = 'Transparent'; R = 0; G = 0; B = 0; A = 0 }
        @{ Color = 'Cyan'; R = 0; G = 255; B = 255; A = 255 }
    ) {
        param($Color, $R, $G, $B, $A)

        $expected = [ChartForgeX.Primitives.ChartColor]::FromRgba($R, $G, $B, $A)
        $svgFile = Join-Path -Path $TestDir -ChildPath ("canvas-color-{0}.svg" -f $Color)
        $canvas = New-ImageCanvas -Width 368 -Height 120 -Theme Light -LayerDefinition {
            New-ImageCanvasText -X 16 -Y 16 -Width 336 -Text 'Explicit color' -FontSize 32 -Color $Color
        } -FilePath (Join-Path -Path $TestDir -ChildPath ("canvas-color-{0}.png" -f $Color)) -PassThru

        New-ImageCanvas -Canvas $canvas -FilePath $svgFile
        [xml] $svg = [System.IO.File]::ReadAllText($svgFile)
        $text = $svg.SelectSingleNode("//*[local-name()='text' and @data-cfx-role='visual-canvas-text']")
        $text.GetAttribute('fill') | Should -BeExactly $expected.ToCss()
    }

    It 'rejects multiple pipeline canvases for one output path' {
        $canvases = @(
            [ChartForgeX.Composition.VisualCanvas]::Create(100, 100)
            [ChartForgeX.Composition.VisualCanvas]::Create(100, 100)
        )
        { $canvases | New-ImageCanvas -FilePath (Join-Path -Path $TestDir -ChildPath 'multiple.svg') } | Should -Throw
    }
}
