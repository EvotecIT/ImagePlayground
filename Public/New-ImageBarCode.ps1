function New-ImageBarCode {
    [cmdletbinding()]
    param(
        [string] $Content,
        [string] $FilePath,
        [int] $Width,
        [int] $Height,
        [NetBarcode.BarcodeType] $Type,
        [NetBarcode.LabelPosition] $LabelPosition,
        [System.Drawing.RotateFlipType] $Rotate,
        [switch] $HideLabel
    )

    $Settings = [NetBarcode.BarcodeSettings]::new()
    $Settings.Text = $Content
    if ($PSBoundParameters.ContainsKey( 'Height')) {
        $Settings.BarcodeHeight = $Height
    }
    if ($PSBoundParameters.ContainsKey( 'Width')) {
        $Settings.BarcodeWidth = $Width
    }
    if ($PSBoundParameters.ContainsKey("Type")) {
        $Settings.BarcodeType = $Type
    }
    if ($HideLabel) {
        $Settings.ShowLabel = $false
    }
    if ($PSBoundParameters.ContainsKey('Rotate')) {
        $Settings.Rotate = $Rotate
    }
    if ($PSBoundParameters.ContainsKey("LabelPosition")) {
        $Settings.LabelPosition = $LabelPosition
    }
    #$Settings.LabelFont = $LabelFont
    #$Settings.LineColor = $LineColor
    $BarCode = [NetBarcode.Barcode]::new()
    $null = $BarCode.Configure($Settings)
    $BarCode.SaveImageFile($Content, $FilePath, [System.Drawing.Imaging.ImageFormat]::Png)
}