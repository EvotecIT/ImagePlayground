function New-ImageChart {
    [cmdletBinding()]
    param(
        [scriptblock] $ChartsDefinition,
        [int] $Width = 600,
        [int] $Height = 400,
        [string] $FilePath,
        [switch] $Show
    )

    $Values = [System.Collections.Generic.List[double]]::new()
    $Labels = [System.Collections.Generic.List[string]]::new()
    $Positions = [System.Collections.Generic.List[int]]::new()
    $Plot = [ScottPlot.Plot]::new($Width, $Height)
    $Position = 0

    if ($ChartsDefinition) {
        $OutputDefintion = & $ChartsDefinition
        foreach ($Definition in $OutputDefintion) {
            if ($Definition.ObjectType -eq 'Bar') {
                $Values.Add($Definition.Value)
                $Labels.Add($Definition.Name)
                $Positions.Add($Position)
                $Position++
            }
        }
    }
    if ($Values) {
        $null = $Plot.AddBar($Values)
    }
    if ($Labels) {
        $null = $Plot.XTicks($Positions, $Labels)
    }

    if (-not $FilePath) {
        $FilePath = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), "$($([System.IO.Path]::GetRandomFileName()).Split('.')[0]).png")
    }
    $null = $Plot.SaveFig($FilePath)

    if ($Show) {
        Invoke-Item -LiteralPath $FilePath
    }
}