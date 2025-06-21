---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version:
schema: 2.0.0
---

# New-ImageQRCodeGeoLocation

## SYNOPSIS
Creates a QR code pointing to geographic coordinates.

## SYNTAX
```powershell
New-ImageQRCodeGeoLocation [-Latitude] <String> [-Longitude] <String> [-FilePath] <String> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Generates a QR code with geo coordinates that can be opened in a maps application.

## EXAMPLES
### Example 1
```powershell
PS C:\> New-ImageQRCodeGeoLocation -Latitude '52.1' -Longitude '21.0' -FilePath .\geo.png
```
Creates geo.png containing location information.

## PARAMETERS
### -Latitude
Latitude value.
### -Longitude
Longitude value.
### -FilePath
Destination path for the image file.
### -Show
Opens the generated file when finished.
### CommonParameters
This cmdlet supports the common parameters.
