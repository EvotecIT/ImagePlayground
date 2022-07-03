function New-ImageChart {
    [cmdletBinding()]
    param(
        [scriptblock] $ChartsDefinition,
        [int] $Width = 600,
        [int] $Height = 400,
        [string] $FilePath,
        [switch] $Show
    )
    $ValueHash = [ordered] @{}
    #$Values = [System.Collections.Generic.List[double]]::new()
    $Labels = [System.Collections.Generic.List[string]]::new()
    $Positions = [System.Collections.Generic.List[int]]::new()
    $Plot = [ScottPlot.Plot]::new($Width, $Height)
    $Position = 0

    if ($ChartsDefinition) {
        $OutputDefintion = & $ChartsDefinition
        foreach ($Definition in $OutputDefintion) {
            if ($Definition.ObjectType -eq 'Bar') {
                for ($i = 0; $i -lt $Definition.Value.Count; $i++) {
                    if (-not $ValueHash["$i"]) {
                        $ValueHash["$i"] = [System.Collections.Generic.List[double]]::new()
                    }
                    $ValueHash["$i"].Add($Definition.Value[$i])
                }
                $Labels.Add($Definition.Name)
                #$Positions.Add($Position)
                #$Position++
            }
        }
    }
    if ($ValueHash) {
        foreach ($Value in $ValueHash.Keys) {
            $null = $Plot.AddBar($ValueHash[$Value])
        }
    }
    if ($Labels) {
        $null = $Plot.XTicks($Positions, $Labels)
    }

    if (-not $FilePath) {
        $FilePath = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), "$($([System.IO.Path]::GetRandomFileName()).Split('.')[0]).png")
        Write-Warning -Message "New-ImageChart - No file path specified, saving to $FilePath"
    }


    #void SetAxisLimits(System.Nullable[double] xMin, System.Nullable[double] xMax, System.Nullable[double] yMin, System.Nullable[double] yMax, int xAxisIndex, int yAxisIndex)
    #void SetAxisLimits(ScottPlot.AxisLimits limits, int xAxisIndex, int yAxisIndex)
    # adjust axis limits so there is no padding below the bar graph
    # $Plot.SetAxisLimits(yMin: 0);

    try {
        $null = $Plot.SaveFig($FilePath)

        if ($Show) {
            Invoke-Item -LiteralPath $FilePath
        }
    } catch {
        Write-Warning -Message "Error creating image chart $($_.Exception.Message)"
    }
}