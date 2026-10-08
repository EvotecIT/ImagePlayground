Describe 'Get-ImageBarCode' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path -Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'reads bar code from file' {

        $file = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/BarcodeEAN13.png'

        (Get-ImageBarCode -FilePath $file).Text | Should -Be '9012341234571'

    }

    It 'prefers a barcode when the image also contains a QR code' {
        $qrFile = Join-Path $TestDir 'mixed-qr.png'
        $barcodeFile = Join-Path $TestDir 'mixed-barcode.png'
        $mixedFile = Join-Path $TestDir 'mixed-symbols.png'

        New-ImageQRCode -Content 'qr-content' -FilePath $qrFile -PixelSize 8
        New-ImageBarCode -Type EAN -Value '9012341234571' -FilePath $barcodeFile
        Merge-Image -FilePath $qrFile -FilePathToMerge $barcodeFile -FilePathOutput $mixedFile -Placement Right

        $result = Get-ImageBarCode -FilePath $mixedFile

        $result.Format.ToString() | Should -Be 'Ean'
        $result.Text | Should -Be '9012341234571'
    }

    It 'surfaces an invalid image instead of treating it as no barcode' {
        $file = Join-Path $TestDir 'invalid-barcode-image.bin'
        Set-Content -Path $file -Value 'not an image' -Encoding ASCII

        { Get-ImageBarCode -FilePath $file -ErrorAction Stop } | Should -Throw
    }

    It 'returns structured scan completion when detailed results are requested' {
        $file = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/BarcodeEAN13.png'
        $options = [CodeGlyphX.ScanOptions]::new()
        $options.Formats = @([CodeGlyphX.SymbolFormat]::Ean)
        $options.TimeoutMilliseconds = 5000
        $options.MaxSymbols = 1
        $result = Get-ImageBarCode -FilePath $file -ScanOptions $options -Detailed
        $result.GetType().FullName | Should -Be 'CodeGlyphX.ScanResult'
        $result.IsSuccess | Should -BeTrue
        $result.Symbols[0].Format.ToString() | Should -Be 'Ean'
        $result.Symbols[0].Text | Should -Be '9012341234571'
    }


}
