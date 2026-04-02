---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Resize-Image
## SYNOPSIS
Resizes an image.

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
Use width/height parameters or Percentage.

## EXAMPLES

### EXAMPLE 1
```powershell
Resize-Image -FilePath in.png -OutputPath out.png -Width 100 -Height 100
```

### EXAMPLE 2
```powershell
Resize-Image -FilePath in.png -OutputPath out.png -Percentage 200
```

## PARAMETERS

### -Async
Use asynchronous processing.

```yaml
Type: SwitchParameter
Parameter Sets: HeightWidth, Percentage
Aliases: 
Possible values: 

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -DontRespectAspectRatio
Only valid when resizing by width or height.

```yaml
Type: SwitchParameter
Parameter Sets: HeightWidth
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
Parameter Sets: HeightWidth, Percentage
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Height
Used with Width when not using .

```yaml
Type: Int32
Parameter Sets: HeightWidth
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutputPath
Supported formats depend on the file extension.

```yaml
Type: String
Parameter Sets: HeightWidth, Percentage
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Percentage
Applies uniform scaling relative to the original size.

```yaml
Type: Int32
Parameter Sets: Percentage
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
Used with Height when not using .

```yaml
Type: Int32
Parameter Sets: HeightWidth
Aliases: 
Possible values: 

Required: False
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

