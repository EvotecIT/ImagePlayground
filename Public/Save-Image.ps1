function Save-Image {
    [cmdletBinding()]
    param(
        [parameter(Mandatory)][ImagePlayground.Image] $Image,
        [string] $FilePath,
        [switch] $Open
    )
    if ($FilePath) {
        $Image.Save($FilePath, $Open.IsPresent)
    } else {
        $Image.Save($Open.IsPresent)
    }
}