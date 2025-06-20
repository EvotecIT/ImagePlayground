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
        New-ImageThumbnail -DirectoryPath $srcDir -OutputDirectory $outDir -Width 20 -Height 20
        (Test-Path $outDir) | Should -BeTrue
        (Get-ChildItem -Path $outDir -File | Measure-Object).Count | Should -BeGreaterThan 0
        $file = Get-ChildItem -Path $outDir -File | Select-Object -First 1
        $img = [ImagePlayground.Image]::Load($file.FullName)
        $img.Width | Should -Be 20
        $img.Height | Should -Be 20
        $img.Dispose()
    }
}
