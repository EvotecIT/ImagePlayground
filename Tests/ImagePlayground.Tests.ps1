Describe 'ImagePlayground module' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
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
}
