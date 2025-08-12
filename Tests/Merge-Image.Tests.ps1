Describe 'Merge-Image' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'merges two images vertically' {

        $src1 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'

        $src2 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $dest = Join-Path $TestDir 'merged.png'

        if (Test-Path $dest) { Remove-Item $dest }

        Merge-Image -FilePath $src1 -FilePathToMerge $src2 -FilePathOutput $dest

        Test-Path $dest | Should -BeTrue

        $img1 = [ImagePlayground.Image]::Load($src1)

        $img2 = [ImagePlayground.Image]::Load($src2)

        $expectedWidth = [Math]::Max($img1.Width, $img2.Width)

        $expectedHeight = $img1.Height + $img2.Height

        $img1.Dispose()

        $img2.Dispose()

        $img = [ImagePlayground.Image]::Load($dest)

        $img.Width | Should -Be $expectedWidth

        $img.Height | Should -Be $expectedHeight

        $img.Dispose()

    }

    It 'merges images into non-existent directory' {
        $src1 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $src2 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'merged.png'
        Merge-Image -FilePath $src1 -FilePathToMerge $src2 -FilePathOutput $dest
        Test-Path $dest | Should -BeTrue
    }

}

