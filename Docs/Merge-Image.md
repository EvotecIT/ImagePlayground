---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Merge-Image
## SYNOPSIS
Merges two images and saves the result.

## SYNTAX
### __AllParameterSets
```powershell
Merge-Image [-FilePath] <string> [-FilePathToMerge] <string> [-FilePathOutput] <string> [-ResizeToFit] [-Placement <ImagePlacement>] [<CommonParameters>]
```

## DESCRIPTION
Merges two images and saves the result.

## EXAMPLES

### EXAMPLE 1
```powershell
Merge-Image -FilePath img1.png -FilePathToMerge img2.png -FilePathOutput out.png
```

## PARAMETERS

### -FilePath
Must exist.

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

### -FilePathOutput
Output file path.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePathToMerge
Must exist.

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

### -Placement
Placement of the second image.

Possible values: Bottom, Right, Top, Left

```yaml
Type: ImagePlacement
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: Bottom, Right, Top, Left

Required: False
Position: named
Default value: Bottom
Accept pipeline input: False
Accept wildcard characters: True
```

### -ResizeToFit
Resize images to fit before merging.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None

