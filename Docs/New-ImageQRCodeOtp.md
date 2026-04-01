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
New-ImageQRCodeOtp [-Type] <OtpAuthType> [-SecretBase32] <String> [[-Label] <String>] [[-Issuer] <String>] [-Algorithm <OtpAlgorithm>] [-Digits <Int32>] [-Period <Int32>] [-Counter <Int32>] [-FilePath] <String> [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Encodes the provided OneTimePassword payload into a QR code for use with authenticator apps.

## EXAMPLES
### Example 1
```powershell
PS C:\> New-ImageQRCodeOtp -Type Totp -SecretBase32 'ABC' -Label 'User' -FilePath .\otp.png
```
Creates otp.png with OTP configuration.

## PARAMETERS
### -Type
OTP type (Totp or Hotp).
### -SecretBase32
Base32-encoded secret.
### -Label
Account label.
### -Issuer
Issuer name.
### -Algorithm
Hash algorithm.
### -Digits
Number of digits.
### -Period
Period for TOTP.
### -Counter
Counter for HOTP.
### -FilePath
Output path for the image.
### -Show
Opens the result.
### -ForegroundColor
Foreground color of QR modules.
### -BackgroundColor
Background color of the QR code.
### -PixelSize
Pixel size for each QR module.
### CommonParameters
This cmdlet supports the common parameters.
