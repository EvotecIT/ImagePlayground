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
New-ImageQRCodeSwiss [-Iban] <string> [-CreditorName] <string> [[-CreditorStreet] <string>] [[-CreditorHouseNumber] <string>] [[-CreditorPostalCode] <string>] [[-CreditorCity] <string>] [-FilePath] <string> [-IbanType <SwissQrIbanType>] [-Currency <SwissQrCurrency>] [-CreditorAddressType <SwissQrAddressType>] [-CreditorAddressLine1 <string>] [-CreditorAddressLine2 <string>] [-CreditorCountry <string>] [-ReferenceType <SwissQrReferenceType>] [-Reference <string>] [-Amount <Decimal>] [-UnstructuredMessage <string>] [-BillInformation <string>] [-AlternativeProcedure1 <string>] [-AlternativeProcedure2 <string>] [-DebtorName <string>] [-DebtorAddressType <SwissQrAddressType>] [-DebtorStreet <string>] [-DebtorHouseNumber <string>] [-DebtorPostalCode <string>] [-DebtorCity <string>] [-DebtorAddressLine1 <string>] [-DebtorAddressLine2 <string>] [-DebtorCountry <string>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to render Swiss QR bill payment details into a payment QR image.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorStreet 'Main Street' -CreditorHouseNumber '1' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -ReferenceType NON -FilePath swiss.png
```

Renders a Swiss QR payment code from the payment fields supplied to the cmdlet.

### EXAMPLE 2
```powershell
PS> New-ImageQRCodeSwiss -Iban 'CH4431999123000889012' -CreditorName 'Evotec GmbH' -CreditorStreet 'Main Street' -CreditorHouseNumber '1' -CreditorPostalCode '8000' -CreditorCity 'Zurich' -CreditorCountry 'CH' -Amount 249.99 -UnstructuredMessage 'Invoice 2026-041' -FilePath swiss-branded.png -ForegroundColor DarkBlue -BackgroundColor WhiteSmoke -PixelSize 14 -Show
```

Creates a styled Swiss QR payment image and opens it immediately after generation.

## PARAMETERS

### -AlternativeProcedure1
Optional first alternative procedure block.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
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
Aliases: None
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
Type: Decimal
Parameter Sets: __AllParameterSets
Aliases: None
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
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -BillInformation
Optional bill information.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
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
Aliases: None
Possible values:

Required: True
Position: 6
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -ForegroundColor
Foreground color of QR modules.

```yaml
Type: Color
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Iban
Swiss or Liechtenstein IBAN.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
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
Aliases: None
Possible values: Iban, QrIban

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PixelSize
Pixel size for each QR module.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Reference
Reference text for QRR or SCOR reference types.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
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
Aliases: None
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
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UnstructuredMessage
Optional unstructured payment message.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
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
