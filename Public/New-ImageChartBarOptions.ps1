function New-ImageChartBarOptions {
    [cmdletBinding()]
    param(
        [switch] $ShowValuesAboveBars
    )

    [PSCustomObject] @{
        ObjectType          = 'BarOptions'
        ShowValuesAboveBars = $ShowValuesAboveBars.IsPresent
    }
}