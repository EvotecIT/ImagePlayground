---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageMosaic
## SYNOPSIS
Creates a mosaic image from multiple files.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageMosaic [-FilePaths] <string[]> [-OutputPath] <string> -Columns <int> -Width <int> -Height <int> [-Open] [<CommonParameters>]
```

## DESCRIPTION
Creates a mosaic image from multiple files.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageMosaic -FilePaths img1.png,img2.png,img3.png,img4.png -OutputPath out.png -Columns 2 -Width 100 -Height 100
```

## PARAMETERS

### -Columns
Number of columns in the mosaic.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePaths
Source image paths.

```yaml
Type: String[]
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Height
Height of each tile.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -Open
Open the mosaic after creation.

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

### -OutputPath
Output file path.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Width
Width of each tile.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: named
Default value: 0
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

