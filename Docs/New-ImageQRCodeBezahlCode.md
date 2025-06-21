---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeBezahlCode

## SYNOPSIS
Creates a BezahlCode payment QR.

## SYNTAX
```powershell
New-ImageQRCodeBezahlCode [-Authority] <AuthorityType> [-Name] <String> [-Account] <String> [-Bnc] <String> [-Iban] <String> [-Bic] <String> [-Reason] <String> [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Generates a German BezahlCode for bank transfer information.

## EXAMPLES
### Example 1
```powershell
PS C:\> New-ImageQRCodeBezahlCode -Authority SinglePayment -Name 'Tester' -Account '123' -Bnc '10020030' -Iban 'DE123' -Bic 'BIC' -Reason 'Invoice' -FilePath .\pay.png
```
Creates pay.png containing BezahlCode.

## PARAMETERS
### -Authority
Type of transfer authority.
### -Name
Recipient name.
### -Account
Account number.
### -Bnc
Bank code.
### -Iban
IBAN value.
### -Bic
BIC value.
### -Reason
Payment purpose text.
### -FilePath
Destination image file.
### -Show
Opens the image after generation.
### CommonParameters
This cmdlet supports the common parameters.
