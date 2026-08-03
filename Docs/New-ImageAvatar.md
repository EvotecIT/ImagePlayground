---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageAvatar
## SYNOPSIS
Creates a rounded avatar image.

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
Creates a rounded avatar image.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageAvatar -FilePath user.jpg -OutputPath avatar.png -Open
```


## PARAMETERS

### -CornerRadius
Corner radius for rounding.

```yaml
Type: Single
Parameter Sets: Path, Stream
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Path to the input image.

```yaml
Type: String
Parameter Sets: Path, Stream
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Height
Height of the avatar.

```yaml
Type: Int32
Parameter Sets: Path, Stream
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Open
Open the avatar after saving.

```yaml
Type: SwitchParameter
Parameter Sets: Path, Stream
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutputPath
Destination path when saving to disk.

```yaml
Type: String
Parameter Sets: Path
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutputStream
Stream that receives the avatar image.

```yaml
Type: Stream
Parameter Sets: Stream
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Width of the avatar.

```yaml
Type: Int32
Parameter Sets: Path, Stream
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None
