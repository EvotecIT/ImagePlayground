---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Resize-Image
## SYNOPSIS
Resize-Image [-FilePath] <string> [-OutputPath] <string> [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [-Async] [<CommonParameters>]

Resize-Image [-FilePath] <string> [-OutputPath] <string> [-Percentage <int>] [-Async] [<CommonParameters>]

## SYNTAX
### HeightWidth (Default)
```powershell
Resize-Image [-FilePath] <string> [-OutputPath] <string> [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [-Async] [<CommonParameters>]
```

### Percentage
```powershell
Resize-Image [-FilePath] <string> [-OutputPath] <string> [-Percentage <int>] [-Async] [<CommonParameters>]
```

## DESCRIPTION
Resize-Image [-FilePath] <string> [-OutputPath] <string> [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [-Async] [<CommonParameters>]

Resize-Image [-FilePath] <string> [-OutputPath] <string> [-Percentage <int>] [-Async] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
Resize-Image -FilePath 'C:\Path'
```

## PARAMETERS

### -Async
{{ Fill Async Description }}

```yaml
Type: SwitchParameter
Parameter Sets: HeightWidth, Percentage
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -DontRespectAspectRatio
{{ Fill DontRespectAspectRatio Description }}

```yaml
Type: SwitchParameter
Parameter Sets: HeightWidth
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
{{ Fill FilePath Description }}

```yaml
Type: String
Parameter Sets: HeightWidth, Percentage
Aliases: None
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Height
{{ Fill Height Description }}

```yaml
Type: Int32
Parameter Sets: HeightWidth
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutputPath
{{ Fill OutputPath Description }}

```yaml
Type: String
Parameter Sets: HeightWidth, Percentage
Aliases: None
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Percentage
{{ Fill Percentage Description }}

```yaml
Type: Int32
Parameter Sets: Percentage
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
{{ Fill Width Description }}

```yaml
Type: Int32
Parameter Sets: HeightWidth
Aliases: None
Possible values: 

Required: False
Position: named
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

