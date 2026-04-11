---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Compare-Image
## SYNOPSIS
Compares two images and optionally saves a difference mask.

## SYNTAX
### __AllParameterSets
```powershell
Compare-Image [-FilePath] <string> [-FilePathToCompare] <string> [[-OutputPath] <string>] [<CommonParameters>]
```

## DESCRIPTION
When OutputPath is omitted, the cmdlet writes the comparison result to the pipeline instead of creating a file.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> Compare-Image -FilePath img1.png -FilePathToCompare img2.png
```

## PARAMETERS

### -FilePath
First image path.

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

### -FilePathToCompare
Second image path.

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

### -OutputPath
When provided, the cmdlet writes a visual difference image instead of only returning the comparison result.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: 2
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

