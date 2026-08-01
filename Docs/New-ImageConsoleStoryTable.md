---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageConsoleStoryTable
## SYNOPSIS
Creates a typed table step from ordinary PowerShell objects.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageConsoleStoryTable [-InputObject] <psobject[]> -Property <string[]> [-Header <string[]>] [-Align <IDictionary>] [<CommonParameters>]
```

## DESCRIPTION
Property selects source properties, Header optionally replaces their displayed names, and Align maps a property or displayed header to Left or Right alignment.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $projects | New-ImageConsoleStoryTable -Property Name, Language, Stars -Header PROJECT, STACK, STARS -Align @{ Stars = 'Right' }
```

Creates one table step without exposing the ChartForgeX table builder.

## PARAMETERS

### -Align
Column alignment keyed by source property or displayed header. Values are Left or Right.

```yaml
Type: IDictionary
Parameter Sets: __AllParameterSets
Aliases: Alignment
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Header
Displayed column names. Defaults to Property.

```yaml
Type: String[]
Parameter Sets: __AllParameterSets
Aliases: Columns
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InputObject
Objects used as table rows.

```yaml
Type: PSObject[]
Parameter Sets: __AllParameterSets
Aliases:
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Property
Source properties included in the table, in display order.

```yaml
Type: String[]
Parameter Sets: __AllParameterSets
Aliases: Properties
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.Management.Automation.PSObject[]`

## OUTPUTS

- `ImagePlayground.PowerShell.ImageConsoleStoryStep` — Use the New-ImageConsoleStoryCommand, New-ImageConsoleStoryOutput, New-ImageConsoleStoryTable, New-ImageConsoleStoryBlankLine, and New-ImageConsoleStoryPause cmdlets to create steps.

## RELATED LINKS

- None
