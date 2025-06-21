---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# ConvertTo-ImageBase64

## SYNOPSIS
Converts an image file to a Base64 encoded string.

## SYNTAX
```powershell
ConvertTo-ImageBase64 [-FilePath] <String> [<CommonParameters>]
```

## DESCRIPTION
`ConvertTo-ImageBase64` reads the image located at `FilePath` and returns its
contents as a Base64 string. This is useful when embedding the image in other
files or sending it through text-only channels.

## EXAMPLES
### Example 1
```powershell
PS C:\> ConvertTo-ImageBase64 -FilePath .\logo.png
```
Returns a Base64 string representing `logo.png`.

## PARAMETERS
### -FilePath
Path to the image file to convert.
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

## INPUTS
None

## OUTPUTS
### System.String

## NOTES

## RELATED LINKS

