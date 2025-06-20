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

        (Get-ImageBarCode -FilePath $file).Message | Should -Be '9012341234571'

    }

    It 'creates and reads data matrix code' {

        $file = Join-Path $TestDir 'datamatrix.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageBarCode -Type DataMatrix -Value 'MatrixTest' -FilePath $file
        Test-Path $file | Should -BeTrue
        (Get-ImageBarCode -FilePath $file).Message | Should -Be 'MatrixTest'
    }

    It 'creates and reads pdf417 code' {

        $file = Join-Path $TestDir 'pdf417.png'
        if (Test-Path $file) { Remove-Item $file }
        New-ImageBarCode -Type PDF417 -Value 'Pdf417Example' -FilePath $file
        Test-Path $file | Should -BeTrue
        (Get-ImageBarCode -FilePath $file).Message | Should -Be 'Pdf417Example'
    }

}
