Describe 'Clear-ImageThumbnailCache' {

    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'clears cached thumbnails' {
        $cacheDir = Join-Path -Path ([System.IO.Path]::GetTempPath()) -ChildPath 'ImagePlayground'
        $cacheDir = Join-Path -Path $cacheDir -ChildPath 'thumbnails'
        if (-not (Test-Path -LiteralPath $cacheDir)) {
            New-Item -Path $cacheDir -ItemType Directory | Out-Null
        }
        $cache = Join-Path $cacheDir 'thumbnail-cache-smoke.txt'
        Set-Content -Path $cache -Value 'cached'

        Test-Path -LiteralPath $cache | Should -BeTrue
        Clear-ImageThumbnailCache
        Test-Path -LiteralPath $cache | Should -BeFalse
    }
}
