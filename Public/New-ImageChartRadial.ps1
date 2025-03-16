function New-ImageChartRadial {
    [cmdletbinding()]
    param(
        [alias('Label')]
        [string] $Name,
        [double] $Value
    )
    [PSCustomObject] @{
        ObjectType = 'Radial'
        Name       = $Name
        Value      = $Value
    }
}
