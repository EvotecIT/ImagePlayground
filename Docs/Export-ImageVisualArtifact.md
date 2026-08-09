---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# Export-ImageVisualArtifact
## SYNOPSIS
Exports a ChartForgeX visual artifact to static SVG, HTML, or PNG output.

## SYNTAX
### __AllParameterSets
```powershell
Export-ImageVisualArtifact [-Artifact] <VisualArtifact> [-FilePath] <string> [-Watermark <VisualWatermark[]>] [-Dpi <double>] [-TopologyLayoutPreset <TopologyLayoutPreset>] [-IncludeTopologyDiagnostics] [-Show] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
Applies shared watermarks, PNG DPI metadata, topology layout presets, and optional layout diagnostics without changing the source artifact.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $artifact | Export-ImageVisualArtifact -FilePath report.png -Watermark $mark -Dpi 144
```

Exports the artifact through the shared ChartForgeX static rendering pipeline.

## PARAMETERS

### -Artifact
Visual artifact to export.

```yaml
Type: VisualArtifact
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Dpi
PNG physical resolution metadata in dots per inch.

```yaml
Type: Double
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Destination .svg, .html, .htm, or .png path.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IncludeTopologyDiagnostics
Draw a developer-oriented topology layout diagnostic overlay.

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

### -PassThru
Write the source artifact to the pipeline after export.

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

### -Show
Open the generated artifact after creation.

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

### -TopologyLayoutPreset
Reusable topology spacing and presentation profile.

```yaml
Type: TopologyLayoutPreset
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Automatic, Dense, Compact, Balanced, Presentation

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Watermark
Watermarks applied in declaration order.

```yaml
Type: VisualWatermark[]
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

- `ChartForgeX.VisualArtifacts.VisualArtifact`

## OUTPUTS

- `ChartForgeX.VisualArtifacts.VisualArtifact`

## RELATED LINKS

- None
