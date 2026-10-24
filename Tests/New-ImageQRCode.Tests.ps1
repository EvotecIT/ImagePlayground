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


    It 'creates QR code with centered logo' {
        $file = Join-Path $TestDir 'qr_logo.png'
        $logo = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'

        if (Test-Path $file) { Remove-Item $file }

        New-ImageQRCode -Content 'https://evotec.xyz/logo' -FilePath $file -LogoPath $logo

        Test-Path $file | Should -BeTrue
        Assert-ImagePlaygroundQrMessage -FilePath $file -ExpectedMessage 'https://evotec.xyz/logo'
    }

    It 'preserves visible logo colours in vector SVG output' {
        $file = Join-Path $TestDir 'qr_logo.svg'
        $logo = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        New-ImageQRCode -Content 'https://evotec.xyz/logo' -FilePath $file -LogoPath $logo
        [xml] $svg = Get-Content -LiteralPath $file -Raw
        $svg.SelectNodes("//*[local-name()='image']").Count | Should -Be 0
        $colours = @($svg.SelectNodes("//*[local-name()='path']") | ForEach-Object { $_.GetAttribute('fill') } | Sort-Object -Unique)
        $colours.Count | Should -BeGreaterThan 2
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

    It 'creates QR code in a supported vector format' {
        $file = Join-Path $TestDir 'qr.svg'

        if (Test-Path -Path $file) { Remove-Item -Path $file }

        New-ImageQRCode -Content 'https://evotec.xyz/vector' -FilePath $file

        Test-Path -Path $file | Should -BeTrue
        (Get-Content -Path $file -Raw) | Should -Match '<svg'
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


    It 'throws on invalid pixel size' {
        { New-ImageQRCode -Content 'https://evotec.xyz' -FilePath (Join-Path $TestDir 'qr_invalid.png') -PixelSize 0 } | Should -Throw
    }

    It 'writes compressed PNG output (<Name>)' -TestCases @(
        @{ Name = 'opaque'; Transparent = $false; Limit = 2KB }
        @{ Name = 'transparent'; Transparent = $true; Limit = 4KB }
    ) {
        param($Name, $Transparent, $Limit)
        $file = Join-Path $TestDir "qr-compressed-$Name.png"
        New-ImageQRCode -Content 'https://codeglyphx.com' -FilePath $file -Transparent:$Transparent -PixelSize 8

        (Get-Item $file).Length | Should -BeLessThan $Limit
        (Get-ImageQRCode -FilePath $file).Text | Should -Be 'https://codeglyphx.com'
    }

    It 'fits -Size including the quiet zone' {
        $file = Join-Path $TestDir 'qr-size.png'
        New-ImageQRCode -Content 'https://codeglyphx.com' -FilePath $file -Size 300 -QuietZone 2

        $image = Get-Image -FilePath $file
        try {
            $image.Width | Should -BeLessOrEqual 300
            $image.Width | Should -BeGreaterThan 250
            $image.Width | Should -Be $image.Height
        } finally {
            $image.Dispose()
        }
        (Get-ImageQRCode -FilePath $file).Text | Should -Be 'https://codeglyphx.com'
    }

    It 'renders styled modules and eyes that still decode' {
        $file = Join-Path $TestDir 'qr-styled.png'
        New-ImageQRCode -Content 'https://codeglyphx.com' -FilePath $file -ModuleShape Rounded -EyeShape Circle -EyeColor '#4c1d95' -ForegroundColor '#1e1b4b' -ErrorCorrection H

        (Get-ImageQRCode -FilePath $file).Text | Should -Be 'https://codeglyphx.com'
    }

    It 'uses a denser symbol for a higher error correction level' {
        $low = Join-Path $TestDir 'qr-ecc-l.png'
        $high = Join-Path $TestDir 'qr-ecc-h.png'
        New-ImageQRCode -Content 'https://codeglyphx.com/error-correction' -FilePath $low -ErrorCorrection L -PixelSize 4
        New-ImageQRCode -Content 'https://codeglyphx.com/error-correction' -FilePath $high -ErrorCorrection H -PixelSize 4

        $a = Get-Image -FilePath $low
        $b = Get-Image -FilePath $high
        try {
            $b.Width | Should -BeGreaterThan $a.Width
        } finally {
            $a.Dispose()
            $b.Dispose()
        }
    }

    It 'renders a coordinated preset with a caption outside the quiet zone' {
        $file = Join-Path $TestDir 'qr-preset-caption.png'
        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file -Preset Sunset -Frame ScanMePill -Caption 'EXPLORE' -ErrorCorrection H
        (Get-ImageQRCode -FilePath $file).Text | Should -Be 'https://evotec.xyz'
        $image = Get-Image -FilePath $file
        try {
            $image.Height | Should -BeGreaterThan $image.Width
        } finally {
            $image.Dispose()
        }
    }

    It 'exports a framed preset as SVG with vector coverage' {
        $file = Join-Path $TestDir 'qr-preset-frame.svg'
        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file -Preset Ink -EyeShape Rounded -EyeBall Circle -Frame CornerViewfinder -Caption 'SCAN ME' -ErrorCorrection H
        [xml] $svg = Get-Content -LiteralPath $file -Raw
        $svg.DocumentElement.LocalName | Should -Be 'svg'
        $svg.DocumentElement.GetAttribute('viewBox') | Should -Not -BeNullOrEmpty
        $svg.GetElementsByTagName('path').Count | Should -BeGreaterThan 0
        $svg.GetElementsByTagName('image').Count | Should -Be 0
    }

    It 'supports two gradient colours and a separate eye ball' {
        $file = Join-Path $TestDir 'qr-gradient-ball.png'
        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file -Preset Ink -EyeBall Circle -Gradient '#431b1b', '#8c2059' -ErrorCorrection H
        (Get-ImageQRCode -FilePath $file).Text | Should -Be 'https://evotec.xyz'
        { New-ImageQRCode -Content 'x' -FilePath $file -Gradient '#431b1b' } | Should -Throw
    }

    It 'applies -LogoScale to the budgeted logo through the Logo alias' {
        $file = Join-Path $TestDir 'qr-logo-scale.png'
        $logo = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file -Logo $logo -LogoScale 0.2 -ErrorCorrection H

        (Get-ImageQRCode -FilePath $file).Text | Should -Be 'https://evotec.xyz'
    }

    It 'keeps a logo inside Size after restoring the protected quiet zone' {
        $file = Join-Path $TestDir 'qr-logo-size-protected.png'
        $logo = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file -Preset Ink -Logo $logo -LogoScale 0.2 -Size 300 -QuietZone 0 -ErrorCorrection H
        $image = Get-Image -FilePath $file
        try {
            $image.Width | Should -BeLessOrEqual 300
            $image.Height | Should -BeLessOrEqual 300
        } finally {
            $image.Dispose()
        }
        (Get-ImageQRCode -FilePath $file).Text | Should -Be 'https://evotec.xyz'
        $before = [Convert]::ToBase64String([IO.File]::ReadAllBytes($file))
        { New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file -Preset Ink -Logo $logo -LogoScale 0.2 -Size 21 -QuietZone 0 -ErrorCorrection H } | Should -Throw
        [Convert]::ToBase64String([IO.File]::ReadAllBytes($file)) | Should -Be $before
    }

    It 'lets explicit foreground replace a preset gradient and explicit Gradient override it again' {
        $file = Join-Path $TestDir 'qr-colour-override.svg'
        $settings = @{ Preset = 'Sunset'; ForegroundColor = '#14213d'; ModuleShape = 'Square'; EyeShape = 'Square'; EyeBall = 'Square'; ErrorCorrection = 'H' }
        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file @settings
        [xml] $svg = Get-Content -LiteralPath $file -Raw
        $colours = @($svg.SelectNodes("//*[local-name()='path']") | ForEach-Object { $_.GetAttribute('fill') } | Sort-Object -Unique)
        ($colours -contains '#14213D') | Should -BeTrue
        $colours.Count | Should -Be 2
        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file @settings -Gradient '#431b1b', '#8c2059'
        [xml] $svg = Get-Content -LiteralPath $file -Raw
        $colours = @($svg.SelectNodes("//*[local-name()='path']") | ForEach-Object { $_.GetAttribute('fill') } | Sort-Object -Unique)
        $colours.Count | Should -BeGreaterThan 2
    }

    It 'keeps Frame None free of an implicit caption' {
        $file = Join-Path $TestDir 'qr-no-frame.png'
        New-ImageQRCode -Content 'https://evotec.xyz' -FilePath $file -Preset Ink -Frame None -ErrorCorrection H
        $image = Get-Image -FilePath $file
        try {
            $image.Height | Should -Be $image.Width
        } finally {
            $image.Dispose()
        }
    }

}
