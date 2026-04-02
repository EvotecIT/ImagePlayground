---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageCrop
## SYNOPSIS
New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-X <int>] [-Y <int>] [-Width <int>] [-Height <int>] [-Open] [<CommonParameters>]

New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-CenterX <float>] [-CenterY <float>] [-Radius <float>] [-Open] [<CommonParameters>]

New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-Points <PointF[]>] [-Open] [<CommonParameters>]

## SYNTAX
### Rectangle (Default)
```powershell
New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-X <int>] [-Y <int>] [-Width <int>] [-Height <int>] [-Open] [<CommonParameters>]
```

### Circle
```powershell
New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-CenterX <float>] [-CenterY <float>] [-Radius <float>] [-Open] [<CommonParameters>]
```

### Polygon
```powershell
New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-Points <PointF[]>] [-Open] [<CommonParameters>]
```

## DESCRIPTION
New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-X <int>] [-Y <int>] [-Width <int>] [-Height <int>] [-Open] [<CommonParameters>]

New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-CenterX <float>] [-CenterY <float>] [-Radius <float>] [-Open] [<CommonParameters>]

New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-Points <PointF[]>] [-Open] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageCrop -FilePath 'C:\Path'
```

## PARAMETERS

### -CenterX
{{ Fill CenterX Description }}

```yaml
Type: Single
Parameter Sets: Circle
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -CenterY
{{ Fill CenterY Description }}

```yaml
Type: Single
Parameter Sets: Circle
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
{{ Fill FilePath Description }}

```yaml
Type: String
Parameter Sets: Rectangle, Circle, Polygon
Aliases: None
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Height
{{ Fill Height Description }}

```yaml
Type: Int32
Parameter Sets: Rectangle
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Open
{{ Fill Open Description }}

```yaml
Type: SwitchParameter
Parameter Sets: Rectangle, Circle, Polygon
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutputPath
{{ Fill OutputPath Description }}

```yaml
Type: String
Parameter Sets: Rectangle, Circle, Polygon
Aliases: None
Possible values: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Points
{{ Fill Points Description }}

```yaml
Type: PointF[]
Parameter Sets: Polygon
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Radius
{{ Fill Radius Description }}

```yaml
Type: Single
Parameter Sets: Circle
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
Parameter Sets: Rectangle
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -X
{{ Fill X Description }}

```yaml
Type: Int32
Parameter Sets: Rectangle
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Y
{{ Fill Y Description }}

```yaml
Type: Int32
Parameter Sets: Rectangle
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

