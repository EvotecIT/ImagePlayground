function New-ImageQRContact {
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string] $FilePath,
        [string] $Firstname,
        [string] $Lastname,
        [string] $Nickname ,
        [string] $Phone ,
        [string] $MobilePhone ,
        [string] $WorkPhone ,
        [string] $Email ,
        [DateTime] $Birthday ,
        [string] $Website ,
        [string] $Street ,
        [string] $HouseNumber ,
        [string] $City ,
        [string] $ZipCode ,
        [string] $Country ,
        [string] $Note ,
        [string] $StateRegion ,
        [ValidateSet('Default', 'Reversed')]
        [string] $AddressOrder = 'Default',
        [string] $Org ,
        [string] $OrgTitle,
        [switch] $Show,
        [QRCoder.PayloadGenerator+ContactData+ContactOutputType] $OutputType = [QRCoder.PayloadGenerator+ContactData+ContactOutputType]::VCard4
    )
    if ($FilePath) {
        $FilePath = $PSCmdlet.GetUnresolvedProviderPathFromPSPath($FilePath)
        if (Test-Path -LiteralPath $FilePath -PathType Leaf) {
            Write-Warning -Message "New-ImageQRContact - File $FilePath found. Please check the path."
            return
        }
    }
    try {
        [ImagePlayground.QrCode]::GenerateContact(
            $FilePath, $OutputType, $firstname,
            $lastname, $nickname, $phone,
            $mobilePhone, $workPhone, $email,
            $birthday, $website, $street, $houseNumber,
            $city, $zipCode, $country, $note, $stateRegion, $addressOrder,
            $org, $orgTitle, $false
        )

        if ($Show) {
            Invoke-Item -LiteralPath $FilePath
        }
    } catch {
        if ($PSBoundParameters.ErrorAction -eq 'Stop') {
            throw
        } else {
            Write-Warning -Message "New-ImageQRContact - Error creating image $($_.Exception.Message)"
        }
    }
}
