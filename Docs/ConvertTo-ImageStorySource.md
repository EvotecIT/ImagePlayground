---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# ConvertTo-ImageStorySource
## SYNOPSIS
Converts exact source text into renderer-neutral syntax spans for visual stories.

## SYNTAX
### __AllParameterSets
```powershell
ConvertTo-ImageStorySource [-Text] <string> [-Language <string>] [-Tokenizer <IStorySourceTokenizer>] [<CommonParameters>]
```

## DESCRIPTION
PowerShell uses the native System.Management.Automation parser. Other languages can provide an optional IStorySourceTokenizer adapter without adding parser dependencies to ChartForgeX.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $source = ConvertTo-ImageStorySource -Text 'Get-Process | Sort-Object CPU -Descending' -Language PowerShell
```

Returns exact source text with semantic spans suitable for New-ImageStoryPanel.

## PARAMETERS

### -Language
Language identifier. PowerShell uses its native parser; Plain preserves text without tokenization.

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

### -Text
Exact source text, including whitespace and line endings.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Tokenizer
Optional tokenizer adapter for C#, Bash, or another language.

```yaml
Type: IStorySourceTokenizer
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

- `System.String`

## OUTPUTS

- `ChartForgeX.Stories.StorySourceText`

## RELATED LINKS

- None
