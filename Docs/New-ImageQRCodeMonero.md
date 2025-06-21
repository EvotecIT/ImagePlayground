---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeMonero

## SYNOPSIS
Creates a Monero payment QR code.

## SYNTAX
```powershell
New-ImageQRCodeMonero [-Address] <String> [[-Amount] <Single>] [[-PaymentId] <String>] [[-RecipientName] <String>] [[-Description] <String>] [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Generates a QR code initiating a Monero transaction.

## EXAMPLES
### Example 1
```powershell
PS C:\> New-ImageQRCodeMonero -Address '44AFF...' -Amount 1 -FilePath .\xmr.png
```
Creates xmr.png requesting the transfer.

## PARAMETERS
### -Address
Destination address.
### -Amount
Amount of Monero to send.
### -PaymentId
Optional payment ID.
### -RecipientName
Name of recipient.
### -Description
Payment description.
### -FilePath
Path to output image.
### -Show
Opens the file when done.
### CommonParameters
This cmdlet supports the common parameters.
