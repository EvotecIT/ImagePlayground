---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeGirocode

## SYNOPSIS
Creates a SEPA Girocode.

## SYNTAX
```powershell
New-ImageQRCodeGirocode [-Iban] <String> [-Bic] <String> [-Name] <String> [-Amount] <Decimal> [[-RemittanceInformation] <String>] [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Generates a QR code containing SEPA credit transfer information.

## EXAMPLES
### Example 1
```powershell
PS C:\> New-ImageQRCodeGirocode -Iban 'DE123' -Bic 'COBADEFFXXX' -Name 'Test' -Amount 12.34 -FilePath .\giro.png
```
Creates giro.png with payment details.

## PARAMETERS
### -Iban
IBAN of the beneficiary.
### -Bic
BIC of the beneficiary bank.
### -Name
Name of the beneficiary.
### -Amount
Amount to transfer.
### -RemittanceInformation
Purpose of the payment.
### -FilePath
Image file path.
### -Show
Opens the generated file on completion.
### CommonParameters
This cmdlet supports the common parameters.
