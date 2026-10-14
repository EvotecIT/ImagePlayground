Describe 'New-ImageBarCode' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'creates and reads bar code' {

        $file = Join-Path $TestDir 'barcode.png'

        if (Test-Path $file) { Remove-Item $file }

        New-ImageBarCode -Type EAN -Value '9012341234571' -FilePath $file

        Test-Path $file | Should -BeTrue

        (Get-ImageBarCode -FilePath $file).Text | Should -Be '9012341234571'

    }

    It 'creates and reads data matrix code' {

        $file = Join-Path $TestDir 'datamatrix.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageBarCode -Type DataMatrix -Value 'MatrixTest' -FilePath $file
        Test-Path $file | Should -BeTrue
        (Get-ImageBarCode -FilePath $file).Text | Should -Be 'MatrixTest'
    }


    It 'creates and reads pdf417 code' {

        $file = Join-Path $TestDir 'pdf417.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageBarCode -Type PDF417 -Value 'Pdf417Example' -FilePath $file
        Test-Path $file | Should -BeTrue
        (Get-ImageBarCode -FilePath $file).Text | Should -Be 'Pdf417Example'
    }

    It 'generates <Format> through the matrix owner' -ForEach @(
        @{ Format = 'DotCode' }
        @{ Format = 'HanXin' }
        @{ Format = 'Gs1DataBarStackedOmnidirectional' }
    ) {
        $file = Join-Path $TestDir "$Format.png"
        New-ImageBarCode -Type $Format -Value '1234567890123' -FilePath $file
        Test-Path $file | Should -BeTrue
    }

    It 'rejects MaxiCode without overwriting the destination' {
        $file = Join-Path $TestDir 'maxicode.png'
        Set-Content -LiteralPath $file -Value 'existing output'
        { New-ImageBarCode -Type MaxiCode -Value '1234567890123' -FilePath $file -ErrorAction Stop } | Should -Throw
        (Get-Content -LiteralPath $file -Raw).Trim() | Should -Be 'existing output'
    }

    It 'writes compressed PNG output' {
        $file = Join-Path $TestDir 'barcode-compressed.png'
        New-ImageBarCode -Type Code128 -Value 'EVO-2026-0042' -FilePath $file

        (Get-Item $file).Length | Should -BeLessThan 1024
        (Get-ImageBarCode -FilePath $file -Type Code128).Text | Should -Be 'EVO-2026-0042'
    }

    It 'applies module size, height, colours, and human-readable text' {
        $plain = Join-Path $TestDir 'barcode-plain.png'
        $styled = Join-Path $TestDir 'barcode-styled.png'
        New-ImageBarCode -Type Code128 -Value 'EVO-LAP-0142' -FilePath $plain
        New-ImageBarCode -Type Code128 -Value 'EVO-LAP-0142' -FilePath $styled -ModuleSize 3 -Height 30 -ShowText -ForegroundColor '#1e1b4b'

        $a = Get-Image -FilePath $plain
        $b = Get-Image -FilePath $styled
        try {
            $b.Width | Should -BeGreaterThan $a.Width
            $b.Height | Should -Not -Be $a.Height
        } finally {
            $a.Dispose()
            $b.Dispose()
        }
        (Get-ImageBarCode -FilePath $styled -Type Code128).Text | Should -Be 'EVO-LAP-0142'
    }

    It 'applies module size to matrix symbols' {
        $file = Join-Path $TestDir 'datamatrix-large.png'
        New-ImageBarCode -Type DataMatrix -Value 'LOT-2026-0042' -FilePath $file -ModuleSize 10 -QuietZone 2

        $image = Get-Image -FilePath $file
        try {
            $image.Width % 10 | Should -Be 0
        } finally {
            $image.Dispose()
        }
        (Get-ImageBarCode -FilePath $file -Type DataMatrix).Text | Should -Be 'LOT-2026-0042'
    }

}
