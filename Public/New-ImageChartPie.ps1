function New-ImageChartPie {
    [cmdletbinding()]
    param(
        [alias('Label')][string] $Name,
        [double] $Value
    )
    [PSCustomObject] @{
        ObjectType = 'Pie'
        Name       = $Name
        Value      = $Value
    }
}