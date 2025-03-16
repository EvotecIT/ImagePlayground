function New-ImageChartBar {
    [cmdletBinding()]
    param(
        [alias('Label')]
        [string] $Name,
        [Array] $Value
    )
    [PSCustomObject] @{
        ObjectType = 'Bar'
        Name       = $Name
        Value      = $Value
    }
}
