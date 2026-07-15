---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageChart
## SYNOPSIS
Renders a native ChartForgeX chart to an image or document file.

## SYNTAX
### ChartScript (Default)
```powershell
New-ImageChart [-ChartScript] <scriptblock> -FilePath <string> [-Show] [<CommonParameters>]
```

### Chart
```powershell
New-ImageChart -Chart <Chart> -FilePath <string> [-Show] [<CommonParameters>]
```

## DESCRIPTION
ChartForgeX owns chart construction and options. This cmdlet only resolves the destination, saves the chart, and optionally opens it.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageChart -ChartScript {
    param($chart)
    $points = [ChartForgeX.Core.ChartPoints]::FromValues(35, 42, 58, 61)
    $chart.WithTitle('CPU').AddSmoothLine('Usage', $points).WithGrid()
} -FilePath cpu-usage.png
```

The script receives the native chart and may mutate it or return a replacement chart.

## PARAMETERS

### -Chart
Native ChartForgeX chart object to render.

```yaml
Type: Chart
Parameter Sets: Chart
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -ChartScript
Script block that receives and configures a native ChartForgeX chart.

```yaml
Type: ScriptBlock
Parameter Sets: ChartScript
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
Output file path. The output format is inferred from its extension.

```yaml
Type: String
Parameter Sets: ChartScript, Chart
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Show
Open the rendered file after creation.

```yaml
Type: SwitchParameter
Parameter Sets: ChartScript, Chart
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `ChartForgeX.Core.Chart`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None
