---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeSwiss

## SYNOPSIS
Creates a Swiss QR invoice code.

## SYNTAX
```powershell
New-ImageQRCodeSwiss [-Payload] <SwissQrCode> [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Encodes the provided SwissQrCode payload object into a QR code image.

## EXAMPLES
### Example 1
```powershell
PS C:\> $swiss = [QRCoder.PayloadGenerator+SwissQrCode]::new($iban,$currency,$cred,$ref);New-ImageQRCodeSwiss -Payload $swiss -FilePath .\invoice.png
```
Creates invoice.png.

## PARAMETERS
### -Payload
SwissQrCode payload object.
### -FilePath
Image output path.
### -Show
Opens the file after generation.
### CommonParameters
This cmdlet supports the common parameters.
