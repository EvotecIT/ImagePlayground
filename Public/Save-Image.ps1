function Save-Image {
    [cmdletBinding()]
    param(
        [ImagePlayground.Image] $Image,
        [switch] $Open
    )
    if ($null -ne $Image) {
        if ($FilePath) {
            $Image.Save($FilePath, $Open.IsPresent)
        } else {
            $Image.Save($Open.IsPresent)
        }
    }
}