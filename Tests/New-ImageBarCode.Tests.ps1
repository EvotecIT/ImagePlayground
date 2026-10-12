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

}
