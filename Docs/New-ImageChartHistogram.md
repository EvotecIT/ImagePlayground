---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageChartHistogram

## SYNOPSIS
Creates histogram chart data.

## SYNTAX
```
New-ImageChartHistogram [[-Name] <String>] [[-Values] <Array>] [-BinSize <Int32>] [<CommonParameters>]
```

## DESCRIPTION
`New-ImageChartHistogram` prepares values for a histogram chart.

## EXAMPLES
### Example 1
```powershell
New-ImageChartHistogram -Name 'Sample' -Values 1,2,3,3,4 -BinSize 2
```
Creates histogram data with a bin size of 2.

## PARAMETERS
### -Name
Label for the dataset.
### -Values
Array of numeric values.
### -BinSize
Optional size of each bin.

### CommonParameters
This cmdlet supports the common parameters.

## INPUTS
None

## OUTPUTS
System.Object

## NOTES

## RELATED LINKS
