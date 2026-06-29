---
external help file: ImagePlayground.PowerShell.dll-Help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeSwiss

## SYNOPSIS

Creates a Swiss QR payment code image.

## SYNTAX

```powershell
New-ImageQRCodeSwiss -Iban <String> -CreditorName <String> -FilePath <String>
    [-IbanType <IbanType>] [-Currency <QrSwissCurrency>]
    [-CreditorAddressType <AddressType>] [-CreditorStreet <String>] [-CreditorHouseNumber <String>]
    [-CreditorPostalCode <String>] [-CreditorCity <String>] [-CreditorAddressLine1 <String>]
    [-CreditorAddressLine2 <String>] [-CreditorCountry <String>]
    [-ReferenceType <ReferenceType>] [-Reference <String>] [-ReferenceTextType <ReferenceTextType>]
    [-Amount <Decimal>] [-UnstructuredMessage <String>] [-BillInformation <String>]
    [-AlternativeProcedure1 <String>] [-AlternativeProcedure2 <String>]
    [-DebtorName <String>] [-DebtorAddressType <AddressType>] [-DebtorStreet <String>]
    [-DebtorHouseNumber <String>] [-DebtorPostalCode <String>] [-DebtorCity <String>]
    [-DebtorAddressLine1 <String>] [-DebtorAddressLine2 <String>] [-DebtorCountry <String>]
    [-UltimateCreditorName <String>] [-UltimateCreditorAddressType <AddressType>]
    [-UltimateCreditorStreet <String>] [-UltimateCreditorHouseNumber <String>]
    [-UltimateCreditorPostalCode <String>] [-UltimateCreditorCity <String>]
    [-UltimateCreditorAddressLine1 <String>] [-UltimateCreditorAddressLine2 <String>]
    [-UltimateCreditorCountry <String>] [-Show] [-ForegroundColor <Color>]
    [-BackgroundColor <Color>] [-PixelSize <Int32>] [-Async] [<CommonParameters>]
```

## DESCRIPTION

`New-ImageQRCodeSwiss` renders Swiss QR bill payment details into a QR image.
The cmdlet accepts payment fields directly and creates the required payment model internally.

Use structured address parameters such as `-CreditorStreet`, `-CreditorHouseNumber`, `-CreditorPostalCode`, and `-CreditorCity` for the default `StructuredAddress` mode.
Use `-CreditorAddressType CombinedAddress` with `-CreditorAddressLine1` and `-CreditorAddressLine2` when the address should be encoded as combined address lines.

## EXAMPLES

### Example 1: Create a basic Swiss QR payment code

```powershell
New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorStreet 'Main Street' -CreditorHouseNumber '1' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -ReferenceType NON -FilePath swiss.png
```

Creates a Swiss QR payment code with a structured creditor address.

### Example 2: Add amount and message

```powershell
New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorStreet 'Main Street' -CreditorHouseNumber '1' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -Amount 199.99 -UnstructuredMessage 'Invoice 2026-041' -FilePath swiss-invoice.png
```

Creates a payment QR code with amount and an unstructured message.

### Example 3: Use combined creditor address lines

```powershell
New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorAddressType CombinedAddress -CreditorName 'Evotec GmbH' -CreditorAddressLine1 'Main Street 1' -CreditorAddressLine2 '8000 Zurich' -CreditorCountry 'CH' -ReferenceType NON -FilePath swiss-combined.png
```

Creates a Swiss QR payment code using combined creditor address lines.

### Example 4: Create a styled image and open it

```powershell
New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorStreet 'Main Street' -CreditorHouseNumber '1' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -Amount 249.99 -UnstructuredMessage 'Invoice 2026-041' -FilePath swiss-branded.png -ForegroundColor DarkBlue -BackgroundColor WhiteSmoke -PixelSize 14 -Show
```

Creates a styled Swiss QR payment image and opens it after generation.

## PARAMETERS

| Parameter | Description |
| --- | --- |
| `-Iban` | Swiss or Liechtenstein IBAN. |
| `-IbanType` | IBAN kind. Defaults to `Iban`. |
| `-Currency` | Payment currency. Defaults to `CHF`. |
| `-CreditorName` | Creditor name. |
| `-CreditorAddressType` | Creditor address format. Use `StructuredAddress` or `CombinedAddress`. |
| `-CreditorStreet` | Creditor street for structured addresses. |
| `-CreditorHouseNumber` | Creditor house number for structured addresses. |
| `-CreditorPostalCode` | Creditor postal code for structured addresses. |
| `-CreditorCity` | Creditor city for structured addresses. |
| `-CreditorAddressLine1` | First creditor address line for combined addresses. |
| `-CreditorAddressLine2` | Second creditor address line for combined addresses. |
| `-CreditorCountry` | Creditor two-letter country code. Defaults to `CH`. |
| `-ReferenceType` | Reference type. Defaults to `NON`. |
| `-Reference` | Reference text for QRR or SCOR reference types. |
| `-ReferenceTextType` | Reference text kind. When omitted, QRR uses QR reference and SCOR uses ISO 11649 creditor reference. |
| `-Amount` | Optional payment amount. |
| `-UnstructuredMessage` | Optional unstructured payment message. |
| `-BillInformation` | Optional bill information. |
| `-AlternativeProcedure1` | Optional first alternative procedure block. |
| `-AlternativeProcedure2` | Optional second alternative procedure block. |
| `-DebtorName` | Optional debtor name. |
| `-DebtorAddressType` | Optional debtor address format. |
| `-DebtorStreet` | Debtor street for structured addresses. |
| `-DebtorHouseNumber` | Debtor house number for structured addresses. |
| `-DebtorPostalCode` | Debtor postal code for structured addresses. |
| `-DebtorCity` | Debtor city for structured addresses. |
| `-DebtorAddressLine1` | First debtor address line for combined addresses. |
| `-DebtorAddressLine2` | Second debtor address line for combined addresses. |
| `-DebtorCountry` | Debtor two-letter country code. Defaults to `CH`. |
| `-UltimateCreditorName` | Optional ultimate creditor name. |
| `-UltimateCreditorAddressType` | Optional ultimate creditor address format. |
| `-UltimateCreditorStreet` | Ultimate creditor street for structured addresses. |
| `-UltimateCreditorHouseNumber` | Ultimate creditor house number for structured addresses. |
| `-UltimateCreditorPostalCode` | Ultimate creditor postal code for structured addresses. |
| `-UltimateCreditorCity` | Ultimate creditor city for structured addresses. |
| `-UltimateCreditorAddressLine1` | First ultimate creditor address line for combined addresses. |
| `-UltimateCreditorAddressLine2` | Second ultimate creditor address line for combined addresses. |
| `-UltimateCreditorCountry` | Ultimate creditor two-letter country code. Defaults to `CH`. |
| `-FilePath` | Output image path. The image format is inferred from the file extension. |
| `-Show` | Opens the image after creation. |
| `-ForegroundColor` | Foreground color of QR modules. Defaults to black. |
| `-BackgroundColor` | Background color of the QR code. Defaults to white. |
| `-PixelSize` | Pixel size for each QR module. Defaults to `20`. |
| `-Async` | Uses asynchronous processing. |

## RELATED LINKS

[ImagePlayground project](https://github.com/EvotecIT/ImagePlayground)
