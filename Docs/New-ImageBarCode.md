---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageBarCode

## SYNOPSIS
Creates a barcode image.

## SYNTAX

```
New-ImageBarCode [-Type] <BarcodeTypes> [-Value] <String> [-FilePath] <String> [<CommonParameters>]
```

## DESCRIPTION
Generates a barcode in the specified format and saves it to a file.

## EXAMPLES

### Example 1
```powershell
PS C:\> New-ImageBarCode -Type EAN -Value '9012341234571' -FilePath .\barcode.png
```
Creates an EAN-13 barcode saved as barcode.png.

### Example 2
```powershell
PS C:\> New-ImageBarCode -Type DataMatrix -Value 'demo' -FilePath .\matrix.png
```
Creates a Data Matrix barcode and saves it as matrix.png.

## PARAMETERS

### -FilePath
Path where the barcode image will be written.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Type
Barcode format to create.

```yaml
Type: BarcodeTypes
Parameter Sets: (All)
Aliases:
Accepted values: Code128, Code93, Code39, KixCode, UPCE, UPCA, EAN, DataMatrix

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Value
Text value encoded within the barcode.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
