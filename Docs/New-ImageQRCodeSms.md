---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeSms

## SYNOPSIS
Creates a QR code encoding an SMS message.

## SYNTAX
```powershell
New-ImageQRCodeSms [-Number] <String> [[-Message] <String>] [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Generates a QR code that can launch the SMS application with the provided phone number and optional text message.

## EXAMPLES
### Example 1
```powershell
PS C:\> New-ImageQRCodeSms -Number '+123456789' -Message 'Hello' -FilePath .\sms.png
```
Creates sms.png containing an SMS payload.

## PARAMETERS
### -Number
Phone number of the recipient.
### -Message
Optional SMS body text.
### -FilePath
Path where the QR code image will be saved.
### -Show
Opens the generated image after creation.
### CommonParameters
This cmdlet supports the common parameters.
