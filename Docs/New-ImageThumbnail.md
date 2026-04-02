---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageThumbnail
## SYNOPSIS
New-ImageThumbnail [-DirectoryPath] <string> [-OutputDirectory] <string> [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [-Sampler <Sampler>] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
New-ImageThumbnail [-DirectoryPath] <string> [-OutputDirectory] <string> [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [-Sampler <Sampler>] [<CommonParameters>]
```

## DESCRIPTION
New-ImageThumbnail [-DirectoryPath] <string> [-OutputDirectory] <string> [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [-Sampler <Sampler>] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageThumbnail -DirectoryPath 'C:\Path'
```

## PARAMETERS

### -DirectoryPath
{{ Fill DirectoryPath Description }}

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

### -DontRespectAspectRatio
{{ Fill DontRespectAspectRatio Description }}

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

### -Height
{{ Fill Height Description }}

```yaml
Type: Int32
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutputDirectory
{{ Fill OutputDirectory Description }}

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

### -Sampler
{{ Fill Sampler Description }}

```yaml
Type: Nullable`1
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Width
{{ Fill Width Description }}

```yaml
Type: Int32
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

- `None`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None

