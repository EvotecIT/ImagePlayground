---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeCalendar
## SYNOPSIS
Creates a calendar event QR code image.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeCalendar [-Entry] <string> [[-Message] <string>] [[-Location] <string>] [-From] <datetime> [-To] <datetime> [-FilePath] <string> [-AllDayEvent] [-EventEncoding <QrCalendarEncoding>] [-Show] [-ForegroundColor <Color>] [-BackgroundColor <Color>] [-PixelSize <int>] [-Async] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to encode meeting or appointment details into a QR code that can be scanned into calendar applications.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageQRCodeCalendar -Entry 'Project Sync' -Message 'Weekly delivery review' -Location 'Office' -From (Get-Date) -To (Get-Date).AddHours(1) -FilePath qr.png
```

Creates a QR code for a calendar event with explicit start and end times.

### EXAMPLE 2
```powershell
PS> New-ImageQRCodeCalendar -Entry 'Company Offsite' -Location 'Gdansk' -From (Get-Date).Date -To (Get-Date).Date.AddDays(1) -AllDayEvent -EventEncoding ICalComplete -FilePath offsite.png -Show
```

Generates an all-day event payload and opens the QR image after creation.

## PARAMETERS

### -AllDayEvent
Specifies whether the event spans the full day.

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

### -Entry
Event title.

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

### -EventEncoding
Calendar encoding.

Possible values: Universal, ICalComplete

```yaml
Type: QrCalendarEncoding
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: Universal, ICalComplete

Required: False
Position: named
Default value: ICalComplete
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

### -From
Start date.

```yaml
Type: DateTime
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 3
Default value: 0001-01-01 00:00:00
Accept pipeline input: False
Accept wildcard characters: True
```

### -Location
Event location.

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Message
Event description.

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

### -To
End date.

```yaml
Type: DateTime
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 4
Default value: 0001-01-01 00:00:00
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

