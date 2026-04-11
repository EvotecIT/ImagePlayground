---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeSms
## SYNOPSIS
Generates a QR code containing an SMS message.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeSms [-Number] <string> [[-Message] <string>] [-FilePath] <string> [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet when a scan should open the SMS app with recipient and optional message body prefilled.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageQRCodeSms -Number '+123456789' -Message 'Hello' -FilePath sms.png
```

Creates a QR code that opens the SMS app with the recipient number and message body prefilled.

### EXAMPLE 2
```powershell
PS> New-ImageQRCodeSms -Number '+48 500 600 700' -Message 'RSVP: I will attend' -FilePath rsvp-sms.png -ForegroundColor Teal -PixelSize 16 -Show
```

Generates a ready-to-send RSVP QR code for invitations or registration desks.

## PARAMETERS

### -Async
Use asynchronous processing.

```yaml
Type: SwitchParameter
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -BackgroundColor
Background color of the QR code.

```yaml
Type: Color
Parameter Sets: __AllParameterSets
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
Position: 2
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -ForegroundColor
Foreground color of QR modules.

```yaml
Type: Color
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 000000FF
Accept pipeline input: False
Accept wildcard characters: True
```

### -Message
Text message.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Number
Recipient phone number.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -PixelSize
Pixel size for each QR module.

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 20
Accept pipeline input: False
Accept wildcard characters: True
```

### -Show
Open the image after creation.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `System.String`

## OUTPUTS

- `None`

## RELATED LINKS

- None

