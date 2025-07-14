---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageThumbnail

## SYNOPSIS
Creates thumbnails for all images in a directory.

## SYNTAX
```
New-ImageThumbnail [-DirectoryPath] <String> [-OutputDirectory] <String> [-Width <Int32>] [-Height <Int32>] [-DontRespectAspectRatio] [-Sampler <Sampler>] [<CommonParameters>]
```

## DESCRIPTION
`New-ImageThumbnail` scans a directory and saves resized copies of each image to the specified output directory.

## EXAMPLES

### Example 1
```powershell
PS C:\> New-ImageThumbnail -DirectoryPath .\Images -OutputDirectory .\Thumbs -Width 64 -Height 64 -Sampler Lanczos3
```
Creates 64x64 thumbnails using the Lanczos3 sampler.

## PARAMETERS

### -DirectoryPath
Directory containing the source images.

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Required: True
Position: 0
```

### -OutputDirectory
Directory where thumbnails will be saved.

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Required: True
Position: 1
```

### -Width
Thumbnail width in pixels.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:
Required: False
Position: Named
```

### -Height
Thumbnail height in pixels.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:
Required: False
Position: Named
```

### -DontRespectAspectRatio
If present, forces the thumbnails to stretch to the specified width and height.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:
Required: False
Position: Named
```

### -Sampler
Optional resampling algorithm to use when resizing.

```yaml
Type: Sampler
Parameter Sets: (All)
Aliases:
Required: False
Position: Named
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.IO.FileInfo[]
## NOTES

## RELATED LINKS
