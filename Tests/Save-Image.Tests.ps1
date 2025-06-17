Describe 'Save-Image' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'saves an image to a new location' {

        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $dest = Join-Path $TestDir 'saved.png'

        if (Test-Path $dest) { Remove-Item $dest }

        $img = Get-Image -FilePath $src

        Save-Image -Image $img -FilePath $dest -CompressionLevel 6 -Quality 80

        $img.Dispose()

        Test-Path $dest | Should -BeTrue

    }

}

