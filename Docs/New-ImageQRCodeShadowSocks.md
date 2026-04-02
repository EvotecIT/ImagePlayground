---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeShadowSocks
## SYNOPSIS
New-ImageQRCodeShadowSocks [-Host] <string> [-Port] <int> [-Password] <string> [-Method] <PayloadGenerator+ShadowSocksConfig+Method> [[-Tag] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeShadowSocks [-Host] <string> [-Port] <int> [-Password] <string> [-Method] <PayloadGenerator+ShadowSocksConfig+Method> [[-Tag] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]
```

## DESCRIPTION
New-ImageQRCodeShadowSocks [-Host] <string> [-Port] <int> [-Password] <string> [-Method] <PayloadGenerator+ShadowSocksConfig+Method> [[-Tag] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRCodeShadowSocks -FilePath 'C:\Path'
```

## PARAMETERS

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

### -Host
{{ Fill Host Description }}

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

### -Method
{{ Fill Method Description }}

```yaml
Type: Method
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Chacha20IetfPoly1305, Aes128Gcm, Aes192Gcm, Aes256Gcm, XChacha20IetfPoly1305, Aes128Cfb, Aes192Cfb, Aes256Cfb, Aes128Ctr, Aes192Ctr, Aes256Ctr, Camellia128Cfb, Camellia192Cfb, Camellia256Cfb, Chacha20Ietf, Aes256Cb, Aes128Ofb, Aes192Ofb, Aes256Ofb, Aes128Cfb1, Aes192Cfb1, Aes256Cfb1, Aes128Cfb8, Aes192Cfb8, Aes256Cfb8, Chacha20, BfCfb, Rc4Md5, Salsa20, DesCfb, IdeaCfb, Rc2Cfb, Cast5Cfb, Salsa20Ctr, Rc4, SeedCfb, Table

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Password
{{ Fill Password Description }}

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

### -Port
{{ Fill Port Description }}

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 1
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

### -Tag
{{ Fill Tag Description }}

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None

