---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageGif
## SYNOPSIS
New-ImageGif [-Frames] <string[]> [-FilePath] <string> [-FrameDelay <int>] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
New-ImageGif [-Frames] <string[]> [-FilePath] <string> [-FrameDelay <int>] [<CommonParameters>]
```

## DESCRIPTION
New-ImageGif [-Frames] <string[]> [-FilePath] <string> [-FrameDelay <int>] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageGif -FilePath 'C:\Path'
```

## PARAMETERS

### -FilePath
{{ Fill FilePath Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -FrameDelay
{{ Fill FrameDelay Description }}

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Frames
{{ Fill Frames Description }}

```yaml
Type: String[]
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None

