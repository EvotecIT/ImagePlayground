---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeOtp

## SYNOPSIS
Creates a QR code for an OTP configuration.

## SYNTAX
```powershell
New-ImageQRCodeOtp [-Payload] <OneTimePassword> [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Encodes the provided OneTimePassword payload into a QR code for use with authenticator apps.

## EXAMPLES
### Example 1
```powershell
PS C:\> $otp = [QRCoder.PayloadGenerator+OneTimePassword]::new();$otp.Secret='ABC';$otp.Label='User';New-ImageQRCodeOtp -Payload $otp -FilePath .\otp.png
```
Creates otp.png with OTP configuration.

## PARAMETERS
### -Payload
OneTimePassword payload object.
### -FilePath
Output path for the image.
### -Show
Opens the result.
### CommonParameters
This cmdlet supports the common parameters.
