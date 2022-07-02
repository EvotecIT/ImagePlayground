function New-ImageChartBar {
    [cmdletBinding()]
    param(
        [alias('Label')][string] $Name,
        [object] $Value
    )
    [PSCustomObject] @{
        ObjectType = 'Bar'
        Name       = $Name
        Value      = $Value
    }
}