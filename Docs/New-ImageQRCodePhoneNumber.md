---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodePhoneNumber

## SYNOPSIS
Creates a QR code that dials a phone number.

## SYNTAX
```powershell
New-ImageQRCodePhoneNumber [-Number] <String> [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Generates a phone URI QR code.

## EXAMPLES
### Example 1
```powershell
PS C:\> New-ImageQRCodePhoneNumber -Number '+123456' -FilePath .\tel.png
```
Creates tel.png to start a phone call.

## PARAMETERS
### -Number
Telephone number to dial.
### -FilePath
Path to save the QR code.
### -Show
Opens the image when done.
### CommonParameters
This cmdlet supports the common parameters.
