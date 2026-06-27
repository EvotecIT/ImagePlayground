Describe 'New-ImageQRCode' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'creates QR code' {

        $file = Join-Path $TestDir 'qr.png'

        if (Test-Path $file) { Remove-Item $file }

        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file

        Test-Path $file | Should -BeTrue

        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedMessage 'https://evotec.xyz'

    }

    It 'creates QR code in current directory' {

        $file = 'qr_current.png'

        if (Test-Path -Path $file) { Remove-Item -Path $file }

        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file

        Test-Path -Path $file | Should -BeTrue

        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedMessage 'https://evotec.xyz'

        Remove-Item -Path $file

    }

    It 'creates QR code with custom colors' {

        $file = Join-Path $TestDir 'qr_custom.png'

        if (Test-Path $file) { Remove-Item $file }

        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file -ForegroundColor Red -BackgroundColor Yellow -PixelSize 10

        Test-Path $file | Should -BeTrue

        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedMessage 'https://evotec.xyz'

    }

    It 'creates QR code asynchronously' {

        $file = Join-Path $TestDir 'qr_async.png'

        if (Test-Path $file) { Remove-Item $file }

        New-ImageQRCode -Content 'https://evotec.xyz/async' -FilePath $file -Async

        Test-Path $file | Should -BeTrue

        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedMessage 'https://evotec.xyz/async'

    }

    It 'creates QR code with centered logo' {
        $file = Join-Path $TestDir 'qr_logo.png'
        $logo = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'

        if (Test-Path $file) { Remove-Item $file }

        New-ImageQRCode -Content 'https://evotec.xyz/logo' -FilePath $file -LogoPath $logo

        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedMessage 'https://evotec.xyz/logo'
    }

    It 'throws before writing output when logo file is missing' {
        $file = Join-Path $TestDir 'qr_missing_logo.png'
        $missingLogo = Join-Path $TestDir 'missing-logo.png'

        if (Test-Path $file) { Remove-Item $file }

        { New-ImageQRCode -Content 'https://evotec.xyz/missing-logo' -FilePath $file -LogoPath $missingLogo } | Should -Throw

        Test-Path $file | Should -BeFalse
    }

    It 'throws before overwriting output when logo file is unreadable' {
        $file = Join-Path $TestDir 'qr_bad_logo.png'
        $badLogo = Join-Path $TestDir 'bad-logo.png'
        $existingContent = 'existing output'

        [System.IO.File]::WriteAllText($file, $existingContent)
        Set-Content -Path $badLogo -Value 'not an image' -Encoding ASCII

        { New-ImageQRCode -Content 'https://evotec.xyz/bad-logo' -FilePath $file -LogoPath $badLogo } | Should -Throw

        Get-Content -Path $file -Raw | Should -Be $existingContent
    }

    It 'creates QR code icon' {

        $file = Join-Path $TestDir 'qr.ico'

        if (Test-Path $file) { Remove-Item $file }

        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file

        Test-Path $file | Should -BeTrue

    }

    It 'creates QR code icon with centered logo in a new directory' {
        $directory = Join-Path $TestDir 'qr-logo-icon'
        $file = Join-Path $directory 'qr_logo.ico'
        $logo = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'

        if (Test-Path $directory) { Remove-Item $directory -Recurse -Force }

        New-ImageQRCode -Content 'https://evotec.xyz/logo-icon' -FilePath $file -LogoPath $logo

        Test-Path $file | Should -BeTrue
        (Get-Item -Path $file).Length | Should -BeGreaterThan 0
    }

    It 'creates QR code icon with centered logo asynchronously' {
        $file = Join-Path $TestDir 'qr_logo_async.ico'
        $logo = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'

        if (Test-Path $file) { Remove-Item $file }

        New-ImageQRCode -Content 'https://evotec.xyz/logo-icon-async' -FilePath $file -LogoPath $logo -Async

        Test-Path $file | Should -BeTrue
        (Get-Item -Path $file).Length | Should -BeGreaterThan 0
    }

    It 'throws on invalid pixel size' {
        { New-ImageQRCode -Content 'https://evotec.xyz' -FilePath (Join-Path $TestDir 'qr_invalid.png') -PixelSize 0 } | Should -Throw
    }

}

