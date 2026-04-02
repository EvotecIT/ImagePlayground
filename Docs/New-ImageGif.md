---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageGif
## SYNOPSIS
Creates an animated GIF from existing images.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageGif [-Frames] <string[]> [-FilePath] <string> [-FrameDelay <int>] [<CommonParameters>]
```

## DESCRIPTION
Creates an animated GIF from existing images.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageGif -Frames img1.png,img2.png -FilePath out.gif -FrameDelay 100
```

## PARAMETERS

### -FilePath
Output path for the GIF animation.

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

### -FrameDelay
Delay between frames in milliseconds.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 100
Accept pipeline input: False
Accept wildcard characters: True
```

### -Frames
Source image paths used as frames.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None

