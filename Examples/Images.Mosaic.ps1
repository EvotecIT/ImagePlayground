#Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$mosaicParams = @{
    FilePaths   = @(
        "$PSScriptRoot\Samples\LogoEvotec.png",
        "$PSScriptRoot\Samples\QRCode.png",
        "$PSScriptRoot\Samples\BarcodeEAN7.png",
        "$PSScriptRoot\Samples\BarcodeEAN13.png"
    )
    OutputPath = "$PSScriptRoot\Output\Mosaic.png"
    Columns    = 2
    Width      = 100
    Height     = 100
}

New-ImageMosaic @mosaicParams
