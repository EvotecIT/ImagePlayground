function New-QRCode {
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Content,
        [Parameter(Mandatory)][string] $FilePath,
        [ValidateSet('png', 'webp')][string] $FileFormat = 'png',
        [int] $Width = 256,
        [int] $Height = 256
    )
    try {
        $FileStream = [System.IO.FileStream]::new($FilePath, [System.IO.FileMode]::OpenOrCreate)
    } catch {
        if ($PSBoundParameters.ErrorAction -eq 'Stop') {
            Write-Error -ErrorRecord $_
            return
        } else {
            Write-Warning -Message "New-QRCode - Couldn't prepare filestream. Error: $($_.Exception.Message)"
        }
    }
    $Vector = [SkiaSharp.QrCode.Image.Vector2Slim]::new($Width, $Height)
    $Encoding = [SkiaSharp.SKEncodedImageFormat]::$FileFormat

    try {
        $QrCode = [SkiaSharp.QrCode.Image.QrCode]::new($Content, $Vector, $Encoding)
        $QrCode.GenerateImage($FileStream)
    } catch {
        if ($PSBoundParameters.ErrorAction -eq 'Stop') {
            Write-Error -ErrorRecord $_
            return
        } else {
            Write-Warning -Message "New-QRCode - Couldn't generate QR code. Error: $($_.Exception.Message)"
        }
    } finally {
        $FileStream.Close()
    }
}