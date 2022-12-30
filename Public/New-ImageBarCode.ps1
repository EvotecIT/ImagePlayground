function New-ImageBarCode {
    [cmdletBinding()]
    param(
        [parameter(Mandatory)][ImagePlayground.BarCode+BarcodeTypes] $Type,
        [parameter(Mandatory)][string] $Value,
        [parameter(Mandatory)][string] $FilePath
    )
    [ImagePlayground.BarCode]::Generate($Type, $Value, $filePath)
}