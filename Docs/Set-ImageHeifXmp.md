---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Set-ImageHeifXmp
## SYNOPSIS
Sets the XMP metadata packet in a HEIF or HEIC file.

## SYNTAX
### Xmp
```powershell
Set-ImageHeifXmp [-FilePath] <string> [[-FilePathOutput] <string>] [-Xmp] <string> [<CommonParameters>]
```

### XmpPath
```powershell
Set-ImageHeifXmp [-FilePath] <string> [[-FilePathOutput] <string>] [-XmpPath] <string> [<CommonParameters>]
```

## DESCRIPTION
Sets the UTF-8 XMP packet in a HEIF or HEIC file.

This command does not decode HEIC image pixels and does not require native HEIF codecs. Updating requires an existing HEIF XMP item with a single writable file extent. Creating a brand-new HEIF XMP item is not supported.

## EXAMPLES

### EXAMPLE 1
```powershell
Set-ImageHeifXmp -FilePath photo.heic -Xmp $packet
```

Writes the XMP packet stored in `$packet` to `photo.heic`.

### EXAMPLE 2
```powershell
Set-ImageHeifXmp -FilePath photo.heic -XmpPath packet.xmp -FilePathOutput photo-updated.heic
```

Writes the XMP packet from `packet.xmp` to `photo-updated.heic`.

## PARAMETERS

### -FilePath
Path to the HEIF or HEIC file.

```yaml
Type: String
Parameter Sets: Xmp, XmpPath
Aliases:
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -FilePathOutput
Optional output path. When omitted, the source file is overwritten.

```yaml
Type: String
Parameter Sets: Xmp, XmpPath
Aliases:
Possible values:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Xmp
XMP metadata packet to write.

```yaml
Type: String
Parameter Sets: Xmp
Aliases:
Possible values:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -XmpPath
Path to a UTF-8 XMP metadata packet file.

```yaml
Type: String
Parameter Sets: XmpPath
Aliases:
Possible values:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- None

## RELATED LINKS

- None
