Describe 'Clear-ImageThumbnailCache' {

    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'clears cached thumbnails' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $img = [ImagePlayground.ImageHelper]::GetImageThumbnail($src, 20, 20)
        $cache = $img.FilePath
        $img.Dispose()
        Test-Path $cache | Should -BeTrue
        Clear-ImageThumbnailCache
        Test-Path $cache | Should -BeFalse
    }
}
