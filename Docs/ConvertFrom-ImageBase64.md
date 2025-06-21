---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# ConvertFrom-ImageBase64

## SYNOPSIS
Creates an image file from Base64 encoded data.

## SYNTAX
```powershell
ConvertFrom-ImageBase64 [-Base64] <String> [-OutputPath] <String> [-Open] [<CommonParameters>]
```

## DESCRIPTION
`ConvertFrom-ImageBase64` takes a Base64 string and writes the decoded image to
`OutputPath`. Use the `Open` switch to launch the saved file after it is
created.

## EXAMPLES
### Example 1
```powershell
PS C:\> ConvertFrom-ImageBase64 -Base64 $data -OutputPath .\picture.png -Open
```
Saves `$data` as `picture.png` and opens it when finished.

## PARAMETERS
### -Base64
Base64 encoded image data.
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
Path where the decoded image is saved.
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

### -Open
Opens the saved image after writing it to disk.
```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS
None

## OUTPUTS
None

## NOTES

## RELATED LINKS

