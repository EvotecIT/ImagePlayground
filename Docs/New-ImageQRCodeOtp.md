---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRCodeOtp
## SYNOPSIS
Generates a QR code for one-time-password configuration.

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRCodeOtp [-Payload] <PayloadGenerator+OneTimePassword> [-FilePath] <string> [-Show] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to create QR codes for authenticator apps that support TOTP or HOTP provisioning.

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRCodeOtp -Type Totp -SecretBase32 'JBSWY3DPEHPK3PXP' -Label 'john.doe@evotec.pl' -Issuer 'Evotec' -FilePath otp.png
```

Creates a QR code that can be scanned into a typical authenticator app for time-based MFA.

### EXAMPLE 2
```powershell
New-ImageQRCodeOtp -Type Hotp -SecretBase32 'JBSWY3DPEHPK3PXP' -Label 'lab-token' -Issuer 'Evotec Lab' -Counter 10 -Digits 8 -Algorithm Sha256 -FilePath hotp.png
```

Creates a counter-based OTP payload for systems that use event-driven one-time passwords.

## PARAMETERS

### -Algorithm
Hash algorithm.

Possible values: Sha1, Sha256, Sha512

```yaml
Type: CodeGlyphX.OtpAlgorithm
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: Sha1
Accept pipeline input: False
Accept wildcard characters: True
```

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

### -Counter
Counter for HOTP.

```yaml
Type: System.Int32
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Digits
Number of digits.

```yaml
Type: System.Int32
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 6
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
Position: 1
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

### -Issuer
Issuer name.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Label
Account label.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Payload
{{ Fill Payload Description }}

```yaml
Type: OneTimePassword
Parameter Sets: __AllParameterSets
Aliases: 
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Period
Period for TOTP.

```yaml
Type: System.Int32
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: 30
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

### -SecretBase32
Base32-encoded secret.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases: 
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

### -Type
OTP type.

Possible values: Totp, Hotp

```yaml
Type: CodeGlyphX.OtpAuthType
Parameter Sets: (All)
Aliases: 
Possible values: 

Required: False
Position: named
Default value: Totp
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

