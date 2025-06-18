---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageGif

## SYNOPSIS
Creates an animated GIF from image frames.

## SYNTAX
```
New-ImageGif [-Frames] <String[]> [-FilePath] <String> [-FrameDelay <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Generates a GIF animation using the provided image files. The frame delay determines how long each frame is displayed.

## EXAMPLES

### Example 1
```powershell
PS C:\> $frames = 'frame1.png','frame2.png'
PS C:\> New-ImageGif -Frames $frames -FilePath .\anim.gif -FrameDelay 50
```
Creates a GIF from the specified frames.

## PARAMETERS

### -Frames
Paths to image files used as frames of the animation.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Output path for the GIF file.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FrameDelay
Time in milliseconds between frames.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 100
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
