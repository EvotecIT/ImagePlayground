---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Get-ImageQRCode
## SYNOPSIS
Reads QR code information from an image file.

## SYNTAX
### __AllParameterSets
```powershell
Get-ImageQRCode [-FilePath] <string> [<CommonParameters>]
```

## DESCRIPTION
Returns the decoded content and symbology details.

## EXAMPLES

### EXAMPLE 1
```powershell
Get-ImageQRCode -FilePath qr.png
```

### EXAMPLE 2
```powershell
(Get-ImageQRCode -FilePath qr.png).Message
```

## PARAMETERS

### -FilePath
The file must exist.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None

