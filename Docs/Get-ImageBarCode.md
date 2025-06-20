---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# Get-ImageBarCode

## SYNOPSIS
Reads barcode information from an image file.

## SYNTAX

```
Get-ImageBarCode [[-FilePath] <String>] [<CommonParameters>]
```

## DESCRIPTION
Decodes the barcode stored in an image and returns the read value together with the status.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-ImageBarCode -FilePath .\barcode.png
```
Returns the value encoded in barcode.png.

## PARAMETERS

### -FilePath
Path to the barcode image to decode.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
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
