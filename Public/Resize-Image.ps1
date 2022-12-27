function Resize-Image {
    [cmdletBinding()]
    param(
        [parameter(Mandatory)][string] $FilePath,
        [parameter(Mandatory)][string] $OutputPath,
        [int] $Width,
        [int] $Height,
        [int] $Percentage
    )
    if ($FilePath -and (Test-Path -LiteralPath $FilePath)) {
        if ($Percentage) {
            [ImagePlayground.ImageHelper]::Resize($FilePath, $OutputPath, $Percentage)
        } else {
            [ImagePlayground.ImageHelper]::Resize($FilePath, $OutputPath, $Width, $Height)
        }
    } else {
        Write-Warning -Message "Resize-Image - File $FilePath not found. Please check the path."
    }
}