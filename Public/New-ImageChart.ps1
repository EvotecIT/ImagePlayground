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
    $Values = [System.Collections.Generic.List[double]]::new()
    $Labels = [System.Collections.Generic.List[string]]::new()
    $Positions = [System.Collections.Generic.List[int]]::new()
    $Plot = [ScottPlot.Plot]::new($Width, $Height)
    $Position = 0

    if ($ChartsDefinition) {
        $OutputDefintion = & $ChartsDefinition
        foreach ($Definition in $OutputDefintion) {
            if ($Definition.ObjectType -eq 'Bar') {
                $Type = 'Bar'
                for ($i = 0; $i -lt $Definition.Value.Count; $i++) {
                    if (-not $ValueHash["$i"]) {
                        $ValueHash["$i"] = [System.Collections.Generic.List[double]]::new()
                    }
                    $ValueHash["$i"].Add($Definition.Value[$i])
                }
                $Labels.Add($Definition.Name)
            } elseif ($Definition.ObjectType -eq 'Line') {
                $Type = 'Line'

                #if (-not $ValueHash["$Position"]) {
                # $ValueHash["$Position"] = [System.Collections.Generic.List[double]]::new()
                #}
                $ValueHash["$($Definition.Name)"] = $Definition.Value

            } elseif ($Definition.ObjectType -eq 'Pie') {
                $Type = 'Pie'
                $Values.Add($Definition.Value)
                $Labels.Add($Definition.Name)
            } elseif ($Definition.ObjectType -eq 'Radial') {
                $Type = 'Radial'
                $Values.Add($Definition.Value)
                $Labels.Add($Definition.Name)
            }
            $Positions.Add($Position)
            $Position++
        }
    }
    if ($Type -eq 'Bar') {
        #void SetAxisLimits(System.Nullable[double] xMin, System.Nullable[double] xMax, System.Nullable[double] yMin, System.Nullable[double] yMax, int xAxisIndex, int yAxisIndex)
        #void SetAxisLimits(ScottPlot.AxisLimits limits, int xAxisIndex, int yAxisIndex)
        # adjust axis limits so there is no padding below the bar graph
        # $Plot.SetAxisLimits(yMin: 0);
        if ($ValueHash) {
            foreach ($Value in $ValueHash.Keys) {
                $null = $Plot.AddBar($ValueHash[$Value])
            }
        }
        if ($Labels) {
            $null = $Plot.XTicks($Positions, $Labels)
        }

    } elseif ($Type -eq 'Line') {
        if ($ValueHash) {
            foreach ($Value in $ValueHash.Keys) {
                #ScottPlot.Plottable.SignalPlot AddSignal(double[] ys, double sampleRate = 1, System.Nullable[System.Drawing.Color] color = default, string label = null)
                #ScottPlot.Plottable.SignalPlotGeneric[T] AddSignal[T](T[] ys, double sampleRate = 1, System.Nullable[System.Drawing.Color] color = default, string label = null)
                $null = $Plot.AddSignal($ValueHash[$Value], 1, $null, $Value)
            }
        }
        $null = $Plot.Legend($true, 7)
    } elseif ($Type -eq 'Pie') {
        $PieChart = $Plot.AddPie($Values)
        $PieChart.SliceLabels = $Labels
        $PieChart.ShowLabels = $true
    } elseif ($Type -eq 'Radial') {
        $RadialChart = $Plot.AddRadialGauge($Values)
        $RadialChart.Labels = $Labels
        $null = $Plot.Legend($true, 7)
    }


    if (-not $FilePath) {
        $FilePath = [System.IO.Path]::Combine([System.IO.Path]::GetTempPath(), "$($([System.IO.Path]::GetRandomFileName()).Split('.')[0]).png")
        Write-Warning -Message "New-ImageChart - No file path specified, saving to $FilePath"
    }
    try {
        $null = $Plot.SaveFig($FilePath)
        if ($Show) {
            Invoke-Item -LiteralPath $FilePath
        }
    } catch {
        Write-Warning -Message "New-ImageChart - Error creating image chart $($_.Exception.Message)"
    }
}