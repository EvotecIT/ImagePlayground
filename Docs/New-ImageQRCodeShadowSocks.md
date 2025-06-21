---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeShadowSocks

## SYNOPSIS
Creates a Shadowsocks configuration QR code.

## SYNTAX
```powershell
New-ImageQRCodeShadowSocks [-Host] <String> [-Port] <Int32> [-Password] <String> [-Method] <Method> [[-Tag] <String>] [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Generates a QR code containing connection settings for a Shadowsocks proxy.

## EXAMPLES
### Example 1
```powershell
PS C:\> New-ImageQRCodeShadowSocks -Host 'example.com' -Port 8388 -Password 'pwd' -Method Aes256Gcm -FilePath .\ss.png
```
Creates ss.png with the proxy configuration.

## PARAMETERS
### -Host
Server hostname.
### -Port
Server port.
### -Password
Proxy password.
### -Method
Encryption method.
### -Tag
Optional tag string.
### -FilePath
Image output path.
### -Show
Opens the created image.
### CommonParameters
This cmdlet supports the common parameters.
