---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeSlovenianUpnQr

## SYNOPSIS
Creates a Slovenian UPN QR payment code.

## SYNTAX
```powershell
New-ImageQRCodeSlovenianUpnQr [-Payload] <SlovenianUpnQr> [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Encodes the provided SlovenianUpnQr payload into an image file.

## EXAMPLES
### Example 1
```powershell
PS C:\> $upn = [QRCoder.PayloadGenerator+SlovenianUpnQr]::new('P','A','X','R','RA','RC','SI123','Desc',1);New-ImageQRCodeSlovenianUpnQr -Payload $upn -FilePath .\upn.png
```
Creates upn.png.

## PARAMETERS
### -Payload
SlovenianUpnQr object.
### -FilePath
Destination path for the QR code.
### -Show
Open the image after creation.
### CommonParameters
This cmdlet supports the common parameters.
