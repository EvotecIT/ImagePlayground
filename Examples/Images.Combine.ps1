Import-Module .\ImagePlayground.psd1 -Force

$mergeImageSplat = @{
    FilePath        = "$PSScriptRoot\Samples\BarcodeEAN13.png"
    FilePathToMerge = "$PSScriptRoot\Samples\BarcodeEAN7.png"
    FilePathOutput  = "$PSScriptRoot\Output\BarcodeEAN13-7.png"
    ResizeToFit     = $true
    Placement       = 'Bottom'
}

Merge-Image @mergeImageSplat


$mergeImageSplat = @{
    FilePath        = "$PSScriptRoot\Samples\ChartsBar.png"
    FilePathToMerge = "$PSScriptRoot\Samples\LogoEvotec.png"
    FilePathOutput  = "$PSScriptRoot\Output\MergedImage.png"
    ResizeToFit     = $true
    Placement       = 'Left'
}

Merge-Image @mergeImageSplat