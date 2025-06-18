---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageAvatar

## SYNOPSIS
Creates a rounded avatar from an existing image.

## SYNTAX

### Path (Default)
```
New-ImageAvatar [-FilePath] <String> [-OutputPath] <String> [-Width <Int32>] [-Height <Int32>] [-CornerRadius <Single>] [-Open] [<CommonParameters>]
```

### Stream
```
New-ImageAvatar [-FilePath] <String> [-OutputStream] <Stream> [-Width <Int32>] [-Height <Int32>] [-CornerRadius <Single>] [<CommonParameters>]
```

## DESCRIPTION
Creates a circular or rounded avatar image from the specified picture. The avatar can either be saved to disk or returned as a stream.

## EXAMPLES

### Example 1
```powershell
PS C:\> New-ImageAvatar -FilePath .\user.jpg -OutputPath .\avatar.png -Open
```
Generates an avatar image saved to avatar.png and opens it after saving.

## PARAMETERS

### -FilePath
Path to the source image.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutputPath
Destination file path when using the Path parameter set.

```yaml
Type: String
Parameter Sets: Path
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutputStream
Stream that receives the avatar image in the Stream parameter set.

```yaml
Type: Stream
Parameter Sets: Stream
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Width of the generated avatar in pixels.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 200
Accept pipeline input: False
Accept wildcard characters: False
```

### -Height
Height of the generated avatar in pixels.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 200
Accept pipeline input: False
Accept wildcard characters: False
```

### -CornerRadius
Radius in pixels used to round the avatar corners.

```yaml
Type: Single
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: False
```

### -Open
Opens the created avatar file after saving.

```yaml
Type: SwitchParameter
Parameter Sets: Path
Aliases:

Required: False
Position: Named
Default value: False
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
