---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageConsoleStoryPalette
## SYNOPSIS
Creates a reusable terminal color palette for ImagePlayground console stories and tabs.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageConsoleStoryPalette [-Preset <string>] [-PageBackground <string>] [-Background <string>] [-HeaderBackground <string>] [-Border <string>] [-Text <string>] [-Muted <string>] [-Accent <string>] [-Success <string>] [-Warning <string>] [-Error <string>] [-Cursor <string>] [-FontFamily <string>] [<CommonParameters>]
```

## DESCRIPTION
Start from a built-in palette and override only the colors that belong to the demo. Window chrome and shell dialect remain separate choices.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $palette = New-ImageConsoleStoryPalette -Preset Ubuntu -Background '#24071B' -Accent '#FF6A2B'
```

Returns a ChartForgeX TerminalTheme that can be supplied through the Palette parameter.

## PARAMETERS

### -Accent
Prompt and command accent color in hexadecimal notation.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Background
Terminal content color in hexadecimal notation.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Border
Frame and divider color in hexadecimal notation.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Cursor
Cursor color in hexadecimal notation.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Error
Error text color in hexadecimal notation.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FontFamily
CSS font-family stack shared by every tab in the story.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -HeaderBackground
Title-bar color in hexadecimal notation.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Muted
Muted text color in hexadecimal notation.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PageBackground
Outer page color in hexadecimal notation.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Preset
Built-in palette used as the customization baseline.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values: Dark, PowerShell, WindowsPowerShell, Ubuntu, Campbell, Classic, Light

Required: False
Position: named
Default value: Dark
Accept pipeline input: False
Accept wildcard characters: False
```

### -Success
Success text color in hexadecimal notation.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Text
Normal text color in hexadecimal notation.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Warning
Warning text color in hexadecimal notation.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases:
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

- `None`

## OUTPUTS

- `ChartForgeX.Terminal.TerminalTheme`

## RELATED LINKS

- None
