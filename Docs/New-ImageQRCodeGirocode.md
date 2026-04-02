---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeGirocode
## SYNOPSIS
New-ImageQRCodeGirocode [-Iban] <string> [-Bic] <string> [-Name] <string> [-Amount] <decimal> [[-RemittanceInformation] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeGirocode [-Iban] <string> [-Bic] <string> [-Name] <string> [-Amount] <decimal> [[-RemittanceInformation] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]
```

## DESCRIPTION
New-ImageQRCodeGirocode [-Iban] <string> [-Bic] <string> [-Name] <string> [-Amount] <decimal> [[-RemittanceInformation] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRCodeGirocode -FilePath 'C:\Path'
```

## PARAMETERS

### -Amount
{{ Fill Amount Description }}

```yaml
Type: Decimal
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Bic
{{ Fill Bic Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
{{ Fill FilePath Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 5
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Iban
{{ Fill Iban Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Name
{{ Fill Name Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -RemittanceInformation
{{ Fill RemittanceInformation Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Show
{{ Fill Show Description }}

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
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

- `System.String`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None

