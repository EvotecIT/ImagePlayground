---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageQRContact
## SYNOPSIS
New-ImageQRContact [-FilePath] <string> [-OutputType <PayloadGenerator+ContactData+ContactOutputType>] [-Firstname <string>] [-Lastname <string>] [-Nickname <string>] [-Phone <string>] [-MobilePhone <string>] [-WorkPhone <string>] [-Email <string>] [-Birthday <datetime>] [-Website <string>] [-Street <string>] [-HouseNumber <string>] [-City <string>] [-ZipCode <string>] [-Country <string>] [-Note <string>] [-StateRegion <string>] [-AddressOrder <PayloadGenerator+ContactData+AddressOrder>] [-Org <string>] [-OrgTitle <string>] [-Show] [<CommonParameters>]

## SYNTAX
### __AllParameterSets
```powershell
New-ImageQRContact [-FilePath] <string> [-OutputType <PayloadGenerator+ContactData+ContactOutputType>] [-Firstname <string>] [-Lastname <string>] [-Nickname <string>] [-Phone <string>] [-MobilePhone <string>] [-WorkPhone <string>] [-Email <string>] [-Birthday <datetime>] [-Website <string>] [-Street <string>] [-HouseNumber <string>] [-City <string>] [-ZipCode <string>] [-Country <string>] [-Note <string>] [-StateRegion <string>] [-AddressOrder <PayloadGenerator+ContactData+AddressOrder>] [-Org <string>] [-OrgTitle <string>] [-Show] [<CommonParameters>]
```

## DESCRIPTION
New-ImageQRContact [-FilePath] <string> [-OutputType <PayloadGenerator+ContactData+ContactOutputType>] [-Firstname <string>] [-Lastname <string>] [-Nickname <string>] [-Phone <string>] [-MobilePhone <string>] [-WorkPhone <string>] [-Email <string>] [-Birthday <datetime>] [-Website <string>] [-Street <string>] [-HouseNumber <string>] [-City <string>] [-ZipCode <string>] [-Country <string>] [-Note <string>] [-StateRegion <string>] [-AddressOrder <PayloadGenerator+ContactData+AddressOrder>] [-Org <string>] [-OrgTitle <string>] [-Show] [<CommonParameters>]

## EXAMPLES

### EXAMPLE 1
```powershell
New-ImageQRContact -FilePath 'C:\Path'
```

## PARAMETERS

### -AddressOrder
{{ Fill AddressOrder Description }}

```yaml
Type: AddressOrder
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: Default, Reversed

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Birthday
{{ Fill Birthday Description }}

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

### -City
{{ Fill City Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Country
{{ Fill Country Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Email
{{ Fill Email Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
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
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: True
```

### -Firstname
{{ Fill Firstname Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -HouseNumber
{{ Fill HouseNumber Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Lastname
{{ Fill Lastname Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -MobilePhone
{{ Fill MobilePhone Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Nickname
{{ Fill Nickname Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Note
{{ Fill Note Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Org
{{ Fill Org Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -OrgTitle
{{ Fill OrgTitle Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -OutputType
{{ Fill OutputType Description }}

```yaml
Type: ContactOutputType
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: MeCard, VCard21, VCard3, VCard4

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Phone
{{ Fill Phone Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
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

### -StateRegion
{{ Fill StateRegion Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Street
{{ Fill Street Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Website
{{ Fill Website Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -WorkPhone
{{ Fill WorkPhone Description }}

```yaml
Type: String
Parameter Sets: __AllParameterSets
Aliases: None
Possible values: 

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -ZipCode
{{ Fill ZipCode Description }}

```yaml
Type: String
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

- `System.String`

## OUTPUTS

- `System.Object`

## RELATED LINKS

- None

