---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeBitcoin
## SYNOPSIS
New-ImageQRCodeBitcoin [-Currency] <PayloadGenerator+BitcoinLikeCryptoCurrencyAddress+BitcoinLikeCryptoCurrencyType> [-Address] <string> [[-Amount] <double>] [[-Label] <string>] [[-Message] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeBitcoin [-Currency] <PayloadGenerator+BitcoinLikeCryptoCurrencyAddress+BitcoinLikeCryptoCurrencyType> [-Address] <string> [[-Amount] <double>] [[-Label] <string>] [[-Message] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]
```

## DESCRIPTION
New-ImageQRCodeBitcoin [-Currency] <PayloadGenerator+BitcoinLikeCryptoCurrencyAddress+BitcoinLikeCryptoCurrencyType> [-Address] <string> [[-Amount] <double>] [[-Label] <string>] [[-Message] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRCodeBitcoin -FilePath 'C:\Path'
```

## PARAMETERS

### -Address
{{ Fill Address Description }}

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

### -Amount
{{ Fill Amount Description }}

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Currency
{{ Fill Currency Description }}

```yaml
Type: BitcoinLikeCryptoCurrencyType
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Bitcoin, BitcoinCash, Litecoin

Required: True
Position: 0
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

### -Label
{{ Fill Label Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Message
{{ Fill Message Description }}

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

