Describe 'ImagePlayground module' {
    BeforeAll {
        $runtimeDir = Join-Path $PSScriptRoot 'Sources/ImagePlayground.PowerShell/bin/Debug/net8.0'
        if ($IsLinux) {
            $nativePath = Join-Path $runtimeDir 'runtimes/linux-x64/native'
            if (Test-Path $nativePath) {
                $env:LD_LIBRARY_PATH = "$nativePath" + [IO.Path]::PathSeparator + $env:LD_LIBRARY_PATH
            }
        } elseif ($IsMacOS) {
            $nativePath = Join-Path $runtimeDir 'runtimes/osx-x64/native'
            if (Test-Path $nativePath) {
                $env:DYLD_LIBRARY_PATH = "$nativePath" + [IO.Path]::PathSeparator + $env:DYLD_LIBRARY_PATH
            }
        } else {
            $nativePath = Join-Path $runtimeDir 'runtimes/win-x64/native'
            if (Test-Path $nativePath) {
                $env:PATH = "$nativePath;" + $env:PATH
            }
        }

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

        $avifFixture = Join-Path $PSScriptRoot 'Fixtures/LogoEvotec.avif.b64'
        $heicFixture = Join-Path $PSScriptRoot 'Fixtures/LogoEvotec.heic.b64'
        if (Test-Path $avifFixture) {
            $bytes = [Convert]::FromBase64String((Get-Content $avifFixture -Raw))
            [IO.File]::WriteAllBytes((Join-Path $TestDir 'LogoEvotec.avif'), $bytes)
        }
        if (Test-Path $heicFixture) {
            $bytes = [Convert]::FromBase64String((Get-Content $heicFixture -Raw))
            [IO.File]::WriteAllBytes((Join-Path $TestDir 'LogoEvotec.heic'), $bytes)
        }
    }
    It 'creates and reads QR code' {
        $file = Join-Path $TestDir 'qr.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file
        Test-Path $file | Should -BeTrue
        (Get-ImageQRCode -FilePath $file).Message | Should -Be 'https://evotec.xyz'
    }

    It 'resizes an image' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dest = Join-Path $TestDir 'logo-small.png'
        Resize-Image -FilePath $src -OutputPath $dest -Width 50 -Height 50
        $img = [ImagePlayground.Image]::Load($dest)
        $img.Width | Should -Be 50
        $img.Height | Should -Be 50
        $img.Dispose()
    }

    It 'converts image format' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'qr.jpg'
        if (Test-Path $dest) { Remove-Item $dest }
        ConvertTo-Image -FilePath $src -OutputPath $dest
        Test-Path $dest | Should -BeTrue
    }
    It 'creates and reads bar code' -Skip:$true {
        $file = Join-Path $TestDir 'barcode.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageBarCode -Type EAN -Value '9012341234571' -FilePath $file
        Test-Path $file | Should -BeTrue
        (Get-ImageBarCode -FilePath $file).Message | Should -Be '9012341234571'
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
    It 'loads an image from disk' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $img = Get-Image -FilePath $src
        $img.Width | Should -Be 660
        $img.Height | Should -Be 660
        $img.Dispose()
    }
    It 'saves an image to a new location' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'saved.png'
        if (Test-Path $dest) { Remove-Item $dest }
        $img = Get-Image -FilePath $src
        Save-Image -Image $img -FilePath $dest
        $img.Dispose()
        Test-Path $dest | Should -BeTrue
    }

    It 'loads AVIF image' {
        $src = Join-Path $TestDir 'LogoEvotec.avif'
        $img = Get-Image -FilePath $src
        $img.Width | Should -BeGreaterThan 0
        $img.Height | Should -BeGreaterThan 0
        $img.Dispose()
    }

    It 'loads HEIC image' {
        $src = Join-Path $TestDir 'LogoEvotec.heic'
        $img = Get-Image -FilePath $src
        $img.Width | Should -BeGreaterThan 0
        $img.Height | Should -BeGreaterThan 0
        $img.Dispose()
    }
}

