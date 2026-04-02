---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageAvatar
## SYNOPSIS
New-ImageAvatar [-FilePath] <string> [-OutputPath] <string> [-Width <int>] [-Height <int>] [-CornerRadius <float>] [-Open] [<CommonParameters>]

New-ImageAvatar [-FilePath] <string> [-OutputStream] <Stream> [-Width <int>] [-Height <int>] [-CornerRadius <float>] [-Open] [<CommonParameters>]

## SYNTAX
### Path (Default)
```powershell
New-ImageAvatar [-FilePath] <string> [-OutputPath] <string> [-Width <int>] [-Height <int>] [-CornerRadius <float>] [-Open] [<CommonParameters>]
```

### Stream
```powershell
New-ImageAvatar [-FilePath] <string> [-OutputStream] <Stream> [-Width <int>] [-Height <int>] [-CornerRadius <float>] [-Open] [<CommonParameters>]
```

## DESCRIPTION
New-ImageAvatar [-FilePath] <string> [-OutputPath] <string> [-Width <int>] [-Height <int>] [-CornerRadius <float>] [-Open] [<CommonParameters>]

New-ImageAvatar [-FilePath] <string> [-OutputStream] <Stream> [-Width <int>] [-Height <int>] [-CornerRadius <float>] [-Open] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageAvatar -FilePath 'C:\Path'
```

## PARAMETERS

### -CornerRadius
{{ Fill CornerRadius Description }}

```yaml
Type: Single
Parameter Sets: Path, Stream
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
Parameter Sets: Path, Stream
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
Parameter Sets: Path, Stream
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Open
{{ Fill Open Description }}

```yaml
Type: SwitchParameter
Parameter Sets: Path, Stream
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
Parameter Sets: Path
Aliases: None
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutputStream
{{ Fill OutputStream Description }}

```yaml
Type: Stream
Parameter Sets: Stream
Aliases: None
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
{{ Fill Width Description }}

```yaml
Type: Int32
Parameter Sets: Path, Stream
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

