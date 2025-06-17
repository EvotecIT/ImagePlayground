Describe 'ConvertTo-Image' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'converts image format' {

        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $dest = Join-Path $TestDir 'qr.jpg'

        if (Test-Path $dest) { Remove-Item $dest }

        ConvertTo-Image -FilePath $src -OutputPath $dest -Quality 80

        Test-Path $dest | Should -BeTrue

    }

}

