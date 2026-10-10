Describe 'New-ImageThumbnail' {

    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'creates thumbnails in output directory' {
        $srcDir = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images'
        $outDir = Join-Path $TestDir 'thumbs'
        if (Test-Path $outDir) { Remove-Item $outDir -Recurse -Force }
        New-ImageThumbnail -DirectoryPath $srcDir -OutputDirectory $outDir -Width 20 -Height 20 -Sampler Lanczos3
        (Test-Path $outDir) | Should -BeTrue
        (Get-ChildItem -Path $outDir -File | Measure-Object).Count | Should -BeGreaterThan 0
        $source = [ImagePlayground.Image]::Load((Join-Path $srcDir 'LogoEvotec.png'))
        $img = [ImagePlayground.Image]::Load((Join-Path $outDir 'LogoEvotec.png'))
        try {
            $scale = [math]::Min(20.0 / $source.Width, 20.0 / $source.Height)
            $img.Width | Should -Be ([math]::Round($source.Width * $scale))
            $img.Height | Should -Be ([math]::Round($source.Height * $scale))
        } finally {
            $img.Dispose()
            $source.Dispose()
        }
    }
}
