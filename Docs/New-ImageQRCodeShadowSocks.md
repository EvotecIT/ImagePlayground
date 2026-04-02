---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeShadowSocks
## SYNOPSIS
Generates a QR code for a Shadowsocks configuration.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeShadowSocks [-Host] <string> [-Port] <int> [-Password] <string> [-Method] <PayloadGenerator+ShadowSocksConfig+Method> [[-Tag] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to share a client-ready Shadowsocks connection string as a scannable QR code.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRCodeShadowSocks -Host 'example.com' -Port 8388 -Password 'pwd' -Method Aes256Gcm -FilePath ss.png
```

Creates a QR code for importing a Shadowsocks connection into a compatible client.

### EXAMPLE 2
```powershell
New-ImageQRCodeShadowSocks -Host 'vpn.evotec.pl' -Port 8388 -Password 'StrongSecret!' -Method Chacha20IetfPoly1305 -Tag 'Warsaw Edge' -FilePath ss-warsaw.png -ForegroundColor Purple -PixelSize 14 -Show
```

Generates a named client profile QR code and opens it immediately after creation.

## PARAMETERS

### -BackgroundColor
Background color of the QR code.

```yaml
Type: SixLabors.ImageSharp.Color
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: FFFFFFFF
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
The image format is inferred from the file extension.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 5
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -ForegroundColor
Foreground color of QR modules.

```yaml
Type: SixLabors.ImageSharp.Color
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 000000FF
Accept pipeline input: False
Accept wildcard characters: True
```

### -Host
{{ Fill Host Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Method
Encryption method.

Possible values: Chacha20IetfPoly1305, Aes128Gcm, Aes192Gcm, Aes256Gcm, XChacha20IetfPoly1305, Aes128Cfb, Aes192Cfb, Aes256Cfb, Aes128Ctr, Aes192Ctr, Aes256Ctr, Camellia128Cfb, Camellia192Cfb, Camellia256Cfb, Chacha20Ietf, Aes256Cb, Aes128Ofb, Aes192Ofb, Aes256Ofb, Aes128Cfb1, Aes192Cfb1, Aes256Cfb1, Aes128Cfb8, Aes192Cfb8, Aes256Cfb8, Chacha20, BfCfb, Rc4Md5, Salsa20, DesCfb, IdeaCfb, Rc2Cfb, Cast5Cfb, Salsa20Ctr, Rc4, SeedCfb, Table

```yaml
Type: Method
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: Chacha20IetfPoly1305, Aes128Gcm, Aes192Gcm, Aes256Gcm, XChacha20IetfPoly1305, Aes128Cfb, Aes192Cfb, Aes256Cfb, Aes128Ctr, Aes192Ctr, Aes256Ctr, Camellia128Cfb, Camellia192Cfb, Camellia256Cfb, Chacha20Ietf, Aes256Cb, Aes128Ofb, Aes192Ofb, Aes256Ofb, Aes128Cfb1, Aes192Cfb1, Aes256Cfb1, Aes128Cfb8, Aes192Cfb8, Aes256Cfb8, Chacha20, BfCfb, Rc4Md5, Salsa20, DesCfb, IdeaCfb, Rc2Cfb, Cast5Cfb, Salsa20Ctr, Rc4, SeedCfb, Table

Required: True
Position: 3
Default value: Chacha20IetfPoly1305
Accept pipeline input: False
Accept wildcard characters: True
```

### -Password
Password for the server.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PixelSize
Pixel size for each QR module.

```yaml
Type: System.Int32
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: True
```

### -Port
Server port.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 1
Default value: 0
Accept pipeline input: False
Accept wildcard characters: True
```

### -ServerHost
Server host name.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases: Host
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Show
Opens the image after creation.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -Tag
Optional tag.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
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

- `None`

## RELATED LINKS

- None

