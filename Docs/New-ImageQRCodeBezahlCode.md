---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeBezahlCode
## SYNOPSIS
New-ImageQRCodeBezahlCode [-Authority] <PayloadGenerator+BezahlCode+AuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-Iban] <string> [-Bic] <string> [-Reason] <string> [-FilePath] <string> [-Show] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeBezahlCode [-Authority] <PayloadGenerator+BezahlCode+AuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-Iban] <string> [-Bic] <string> [-Reason] <string> [-FilePath] <string> [-Show] [<CommonParameters>]
```

## DESCRIPTION
New-ImageQRCodeBezahlCode [-Authority] <PayloadGenerator+BezahlCode+AuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-Iban] <string> [-Bic] <string> [-Reason] <string> [-FilePath] <string> [-Show] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRCodeBezahlCode -FilePath 'C:\Path'
```

## PARAMETERS

### -Account
{{ Fill Account Description }}

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

### -Authority
{{ Fill Authority Description }}

```yaml
Type: AuthorityType
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: singlepayment, singlepaymentsepa, singledirectdebit, singledirectdebitsepa, periodicsinglepayment, periodicsinglepaymentsepa, contact, contact_v2

Required: True
Position: 0
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
Position: 5
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Bnc
{{ Fill Bnc Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 3
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
Position: 7
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
Position: 4
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
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Reason
{{ Fill Reason Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 6
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

