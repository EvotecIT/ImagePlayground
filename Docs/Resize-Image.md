---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# Resize-Image

## SYNOPSIS
{{ Fill in the Synopsis }}

## SYNTAX

### HeightWidth (Default)
```
Resize-Image [-FilePath <String>] [-OutputPath <String>] [-Width <Int32>] [-Height <Int32>]
 [-DontRespectAspectRatio] [<CommonParameters>]
```

### Percentage
```
Resize-Image [-FilePath <String>] [-OutputPath <String>] [-Percentage <Int32>] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -FilePath
{{ Fill FilePath Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutputPath
{{ Fill OutputPath Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
{{ Fill Width Description }}

```yaml
Type: Int32
Parameter Sets: HeightWidth
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Height
{{ Fill Height Description }}

```yaml
Type: Int32
Parameter Sets: HeightWidth
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Percentage
{{ Fill Percentage Description }}

```yaml
Type: Int32
Parameter Sets: Percentage
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DontRespectAspectRatio
If you want to ignore aspect ratio use this switch. It only affects Width and Height parameters that are used separately.

```yaml
Type: SwitchParameter
Parameter Sets: HeightWidth
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
