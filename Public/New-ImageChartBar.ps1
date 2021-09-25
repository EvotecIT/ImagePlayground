function New-ImageChartBar {
    [cmdletbinding()]
    param(
        [float] $Value,
        [string] $Label,
        [string] $ValueLabel,
        [string] $Color,
        [string] $TextColor
    )

    $Entry = [Microcharts.ChartEntry]::new($Value)
    $Entry.Label = $Label
    $Entry.ValueLabel = $ValueLabel
    $Entry.Color = $Color
    $Entry
}