Describe 'Get-ImageBarCode' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force

    }

    It 'reads bar code from file' {

        $file = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/BarcodeEAN13.png'

        (Get-ImageBarCode -FilePath $file).Message | Should -Be '9012341234571'

    }

}

