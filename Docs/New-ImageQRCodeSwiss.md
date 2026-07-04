---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeSwiss
## SYNOPSIS
Generates a Swiss QR payment code.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeSwiss [-Iban] <string> [-CreditorName] <string> [[-CreditorStreet] <string>] [[-CreditorHouseNumber] <string>] [[-CreditorPostalCode] <string>] [[-CreditorCity] <string>] [-FilePath] <string> [-IbanType <SwissQrIbanType>] [-Currency <SwissQrCurrency>] [-CreditorAddressType <SwissQrAddressType>] [-CreditorAddressLine1 <string>] [-CreditorAddressLine2 <string>] [-CreditorCountry <string>] [-ReferenceType <SwissQrReferenceType>] [-Reference <string>] [-Amount <decimal>] [-UnstructuredMessage <string>] [-BillInformation <string>] [-AlternativeProcedure1 <string>] [-AlternativeProcedure2 <string>] [-DebtorName <string>] [-DebtorAddressType <SwissQrAddressType>] [-DebtorStreet <string>] [-DebtorHouseNumber <string>] [-DebtorPostalCode <string>] [-DebtorCity <string>] [-DebtorAddressLine1 <string>] [-DebtorAddressLine2 <string>] [-DebtorCountry <string>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet when a prepared SwissQrCodePayload should be rendered into a payment QR image.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $swiss = [CodeGlyphX.Payloads.SwissQrCodePayload]::new($iban, $currency, $creditor, $reference)
New-ImageQRCodeSwiss -Payload $swiss -FilePath swiss.png
```

Renders a Swiss payment QR code from a previously prepared payment payload object.

### EXAMPLE 2
```powershell
PS> $swiss = [CodeGlyphX.Payloads.SwissQrCodePayload]::new($iban, $currency, $creditor, $reference)
New-ImageQRCodeSwiss -Payload $swiss -FilePath swiss-branded.png -ForegroundColor DarkBlue -BackgroundColor WhiteSmoke -PixelSize 14 -Show
```

Creates a branded QR image and opens it immediately after generation.

## PARAMETERS

### -AlternativeProcedure1
Optional first alternative procedure block.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -AlternativeProcedure2
Optional second alternative procedure block.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Amount
Optional payment amount.

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Async
Use asynchronous processing.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -BackgroundColor
Background color of the QR code.

```yaml
Type: Color
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: FFFFFFFF
Accept pipeline input: False
Accept wildcard characters: True
```

### -BillInformation
Optional bill information.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CreditorAddressLine1
Creditor first address line for combined addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CreditorAddressLine2
Creditor second address line for combined addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CreditorAddressType
Creditor address type.

```yaml
Type: SwissQrAddressType
Parameter Sets: __AllParameterSets
Aliases:
Possible values: StructuredAddress, CombinedAddress

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CreditorCity
Creditor city for structured addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: 5
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CreditorCountry
Creditor two-letter country code.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CreditorHouseNumber
Optional creditor house number for structured addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CreditorName
Creditor name.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CreditorPostalCode
Creditor postal code for structured addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CreditorStreet
Optional creditor street for structured addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Currency
Payment currency.

```yaml
Type: SwissQrCurrency
Parameter Sets: __AllParameterSets
Aliases:
Possible values: CHF, EUR

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DebtorAddressLine1
Debtor first address line for combined addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DebtorAddressLine2
Debtor second address line for combined addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DebtorAddressType
Debtor address type.

```yaml
Type: SwissQrAddressType
Parameter Sets: __AllParameterSets
Aliases:
Possible values: StructuredAddress, CombinedAddress

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DebtorCity
Debtor city for structured addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DebtorCountry
Debtor two-letter country code.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DebtorHouseNumber
Optional debtor house number for structured addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DebtorName
Debtor name.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DebtorPostalCode
Debtor postal code for structured addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DebtorStreet
Optional debtor street for structured addresses.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
The image format is inferred from the file extension.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: True
Position: 6
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -ForegroundColor
Foreground color of QR modules.

```yaml
Type: Color
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: 000000FF
Accept pipeline input: False
Accept wildcard characters: True
```

### -Iban
Swiss or Liechtenstein IBAN.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IbanType
IBAN kind.

```yaml
Type: SwissQrIbanType
Parameter Sets: __AllParameterSets
Aliases:
Possible values: Iban, QrIban

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Payload
Swiss QR payload data.

```yaml
Type: CodeGlyphX.Payloads.SwissQrCodePayload
Parameter Sets: (All)
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PixelSize
Pixel size for each QR module.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: True
```

### -Reference
Reference text for QRR or SCOR reference types.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ReferenceType
Reference type.

```yaml
Type: SwissQrReferenceType
Parameter Sets: __AllParameterSets
Aliases:
Possible values: QRR, SCOR, NON

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Show
Opens the image once generated.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -UnstructuredMessage
Optional unstructured payment message.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None
