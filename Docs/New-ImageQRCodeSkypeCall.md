---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeSkypeCall

## SYNOPSIS
Creates a QR code starting a Skype call.

## SYNTAX
```powershell
New-ImageQRCodeSkypeCall [-UserName] <String> [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Generates a Skype call URI encoded as a QR code.

## EXAMPLES
### Example 1
```powershell
PS C:\> New-ImageQRCodeSkypeCall -UserName 'echo123' -FilePath .\call.png
```
Creates call.png that will launch Skype.

## PARAMETERS
### -UserName
Skype username to call.
### -FilePath
Path to the output file.
### -Show
Open the image after creation.
### CommonParameters
This cmdlet supports the common parameters.
