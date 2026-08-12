---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# ConvertTo-ImageVisualArtifact
## SYNOPSIS
Converts a ChartForgeX chart, grid, canvas, story, diagram, table, sequence, flow, or visual block into a reusable visual artifact.

## SYNTAX
### __AllParameterSets
```powershell
ConvertTo-ImageVisualArtifact [-InputObject] <Object> [-Id <string>] [-Title <string>] [-Subtitle <string>] [-AccessibleName <string>] [-AccessibleDescription <string>] [-Language <string>] [-Decorative] [<CommonParameters>]
```

## DESCRIPTION
The artifact is the portable handoff contract for static export and OfficeIMO document placement.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $artifact = $topology | ConvertTo-ImageVisualArtifact -Id service-map -AccessibleDescription 'Gateway to database service path.'
```

Creates a reusable artifact that can be exported by ImagePlayground or placed by PSWriteOffice.

## PARAMETERS

### -AccessibleDescription
Longer accessible description for the visual.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -AccessibleName
Concise accessible name for the visual.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Decorative
Marks the visual as decorative.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Stable artifact identifier. Existing artifact ids are preserved when omitted.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InputObject
ChartForgeX model or existing visual artifact.

```yaml
Type: Object
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Language
Optional BCP 47 language tag for accessible text.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Subtitle
Optional artifact subtitle override.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Title
Optional artifact title override.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.Object`

## OUTPUTS

- `ChartForgeX.VisualArtifacts.VisualArtifact`

## RELATED LINKS

- None
