---
external help file: ImagePlayground.PowerShell.dll-Help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeSlovenianUpnQr

## SYNOPSIS

Creates a Slovenian UPN QR payment code image.

## SYNTAX

```powershell
New-ImageQRCodeSlovenianUpnQr -PayerName <String> -PayerAddress <String> -PayerPlace <String>
    -RecipientName <String> -RecipientAddress <String> -RecipientPlace <String>
    -RecipientIban <String> -Description <String> -Amount <Double> -FilePath <String>
    [-Deadline <DateTime>] [-RecipientSiModel <String>] [-RecipientSiReference <String>]
    [-Code <String>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>]
    [-PixelSize <Int32>] [-Async] [<CommonParameters>]
```

## DESCRIPTION

`New-ImageQRCodeSlovenianUpnQr` renders Slovenian UPN payment details into a QR image.
The cmdlet accepts payment fields directly and creates the required payment model internally.

## EXAMPLES

### Example 1: Create a Slovenian UPN QR payment code

```powershell
New-ImageQRCodeSlovenianUpnQr -PayerName 'John Doe' -PayerAddress 'Main Street 1' -PayerPlace 'Ljubljana' -RecipientName 'Evotec d.o.o.' -RecipientAddress 'Business Street 2' -RecipientPlace 'Maribor' -RecipientIban 'SI56192001234567890' -Description 'Invoice 2026-041' -Amount 199.99 -FilePath upn.png
```

Creates a Slovenian UPN QR payment code from the supplied payment fields.

### Example 2: Add deadline and SI reference

```powershell
New-ImageQRCodeSlovenianUpnQr -PayerName 'John Doe' -PayerAddress 'Main Street 1' -PayerPlace 'Ljubljana' -RecipientName 'Evotec d.o.o.' -RecipientAddress 'Business Street 2' -RecipientPlace 'Maribor' -RecipientIban 'SI56192001234567890' -Description 'Invoice 2026-041' -Amount 199.99 -Deadline ([datetime]'2026-04-10') -RecipientSiModel 'SI00' -RecipientSiReference '2026041' -FilePath upn-reference.png
```

Creates a payment QR code with deadline and recipient SI reference details.

### Example 3: Create a styled image and open it

```powershell
New-ImageQRCodeSlovenianUpnQr -PayerName 'John Doe' -PayerAddress 'Main Street 1' -PayerPlace 'Ljubljana' -RecipientName 'Evotec d.o.o.' -RecipientAddress 'Business Street 2' -RecipientPlace 'Maribor' -RecipientIban 'SI56192001234567890' -Description 'Annual subscription' -Amount 49 -FilePath upn-brand.png -ForegroundColor DarkGreen -PixelSize 14 -Show
```

Creates a styled Slovenian UPN QR payment image and opens it after generation.

## PARAMETERS

| Parameter | Description |
| --- | --- |
| `-PayerName` | Payer name. |
| `-PayerAddress` | Payer street address. |
| `-PayerPlace` | Payer postal place. |
| `-RecipientName` | Recipient name. |
| `-RecipientAddress` | Recipient street address. |
| `-RecipientPlace` | Recipient postal place. |
| `-RecipientIban` | Recipient IBAN. |
| `-Description` | Payment description. |
| `-Amount` | Payment amount. |
| `-FilePath` | Output image path. The image format is inferred from the file extension. |
| `-Deadline` | Optional payment deadline. |
| `-RecipientSiModel` | Recipient SI model. Defaults to `SI00`. |
| `-RecipientSiReference` | Optional recipient SI reference. |
| `-Code` | UPN payment code. Defaults to `OTHR`. |
| `-Show` | Opens the image after creation. |
| `-ForegroundColor` | Foreground color of QR modules. Defaults to black. |
| `-BackgroundColor` | Background color of the QR code. Defaults to white. |
| `-PixelSize` | Pixel size for each QR module. Defaults to `20`. |
| `-Async` | Uses asynchronous processing. |

## RELATED LINKS

[ImagePlayground project](https://github.com/EvotecIT/ImagePlayground)
