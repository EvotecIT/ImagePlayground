Describe 'New-ImageGif' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'creates animated gif' {
        $dest = Join-Path $TestDir 'anim.gif'
        if (Test-Path $dest) { Remove-Item $dest }

        $frames = @(
            Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
            Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.jpg'
        )

        New-ImageGif -Frames $frames -FilePath $dest -FrameDelay 50
        Test-Path $dest | Should -BeTrue
    }

    It 'creates gif in non-existent directory' {
        $frames = @(
            Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        )
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'anim.gif'
        New-ImageGif -Frames $frames -FilePath $dest -FrameDelay 50
        Test-Path $dest | Should -BeTrue
    }
}

