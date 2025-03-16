function New-ImageChartLine {
    [CmdletBinding()]
    param(
        [alias('Label')]
        [string] $Name,
        [Array] $Value
    )
    [PSCustomObject] @{
        ObjectType = 'Line'
        Name       = $Name
        Value      = $Value
    }
}
