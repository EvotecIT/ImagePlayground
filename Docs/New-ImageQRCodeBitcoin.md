---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeBitcoin

## SYNOPSIS
Creates a cryptocurrency payment QR code.

## SYNTAX
```powershell
New-ImageQRCodeBitcoin [-Currency] <BitcoinLikeCryptoCurrencyType> [-Address] <String> [[-Amount] <Double>] [[-Label] <String>] [[-Message] <String>] [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Generates a QR code for Bitcoin, Bitcoin Cash or Litecoin payments.

## EXAMPLES
### Example 1
```powershell
PS C:\> New-ImageQRCodeBitcoin -Currency Bitcoin -Address '1Boat...' -Amount 0.01 -FilePath .\btc.png
```
Creates btc.png with a payment request.

## PARAMETERS
### -Currency
Type of cryptocurrency.
### -Address
Wallet address.
### -Amount
Amount of coins to transfer.
### -Label
Optional label.
### -Message
Optional message.
### -FilePath
Output image path.
### -Show
Opens the resulting image.
### CommonParameters
This cmdlet supports the common parameters.
