---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# ConvertTo-Image

## SYNOPSIS
Converts an existing image file to another format.

## SYNTAX

```
ConvertTo-Image [-FilePath] <String> [-OutputPath] <String> [-Quality <Int32>] [-CompressionLevel <Int32>] [<CommonParameters>]
```

## DESCRIPTION
`ConvertTo-Image` reads an image from `FilePath` and writes a new file using the
extension specified in `OutputPath`. When saving JPEG or WEBP images you can
set `Quality` to control compression. For PNG files you may specify a
`CompressionLevel` from `0` (no compression) to `9` (maximum compression).

## EXAMPLES

### Example 1
```powershell
PS C:\> ConvertTo-Image -FilePath .\input.png -OutputPath .\output.jpg
```
Converts the PNG file to JPEG format.

### Example 2
```powershell
PS C:\> ConvertTo-Image -FilePath .\input.png -OutputPath .\output.webp -Quality 90
```
Creates a WEBP image with 90 percent quality.

## PARAMETERS

### -FilePath
Path to the source image file.
The file must exist before running the cmdlet.

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
Destination path for the converted image.
The extension of this path determines the output format.

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

### -Quality
JPEG and WEBP quality level from 0 to 100.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:
Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CompressionLevel
PNG compression level from 0 (no compression) to 9 (maximum).

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:
Required: False
Position: Named
Default value: None
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
