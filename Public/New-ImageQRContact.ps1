function New-ImageQRContact {
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $FilePath,
        [QRCoder.PayloadGenerator+ContactData+ContactOutputType] $outputType = [QRCoder.PayloadGenerator+ContactData+ContactOutputType]::VCard4,
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
        [ValidateSet('Default', 'Reversed')][string] $AddressOrder = 'Default',
        [string] $Org ,
        [string] $OrgTitle ,
        [switch] $Transparent
    )

    [ImagePlayground.QrCode]::GenerateContact(
        $filePath, $outputType, $firstname,
        $lastname, $nickname, $phone,
        $mobilePhone, $workPhone, $email,
        $birthday, $website, $street, $houseNumber,
        $city, $zipCode, $country, $note, $stateRegion, $addressOrder,
        $org, $orgTitle, $transparent
    )
}