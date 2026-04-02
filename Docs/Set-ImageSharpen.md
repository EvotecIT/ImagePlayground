---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Set-ImageSharpen
## SYNOPSIS
Sharpens an image.

## SYNTAX
### __AllParameterSets
```powershell
Set-ImageSharpen [-FilePath] <string> [-OutputPath] <string> [-Amount] <float> [-Async] [<CommonParameters>]
```

## DESCRIPTION
Applies a Gaussian sharpen filter with the specified Amount.

## EXAMPLES

### EXAMPLE 1
```powershell
Set-ImageSharpen -FilePath in.png -OutputPath out.png -Amount 2
```

## PARAMETERS

### -Amount
Specifies the strength of the sharpen filter.

```yaml
Type: Single
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 2
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
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
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
The image must exist.

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

### -OutputPath
Supported formats depend on the file extension.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
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

