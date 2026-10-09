param(
    [string] $OutputDirectory = (Join-Path -Path $PSScriptRoot -ChildPath 'Output'),
    [ValidateSet('Light', 'Dark')]
    [string] $Theme = 'Light'
)

Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

$definition = {
    New-ImageTopologyNode -Id gateway -Label Gateway -Kind Network -Status Healthy -Symbol GW -Width 180 -Height 72
    New-ImageTopologyNode -Id api -Label 'Application API' -Kind Service -Status Healthy -Symbol API -Width 180 -Height 72
    New-ImageTopologyNode -Id database -Label Database -Kind Database -Status Warning -Symbol SQL -Width 180 -Height 72
    New-ImageTopologyEdge -SourceNodeId gateway -TargetNodeId api -Label HTTPS -Kind Connectivity -Status Healthy -Direction Forward
    New-ImageTopologyEdge -SourceNodeId api -TargetNodeId database -Label '32 ms' -Kind Dependency -Status Warning -Direction Forward
}
$suffix = $Theme.ToLowerInvariant()

foreach ($extension in 'svg', 'png', 'html') {
    New-ImageTopology -TopologyDefinition $definition -Title 'Service topology' -Subtitle 'Database latency above target' -Layout Layered -Direction LeftToRight -Theme $Theme -Width 900 -Height 420 -Padding 24 -FitContentToViewport -FilePath (Join-Path -Path $OutputDirectory -ChildPath "service-map-$suffix.$extension")
}
foreach ($extension in 'svg', 'png') {
    New-ImageTopology -TopologyDefinition $definition -Title 'Service topology' -Subtitle 'Database latency above target' -Layout Layered -Direction TopToBottom -Theme $Theme -Width 368 -Height 620 -Padding 16 -FitContentToViewport -FilePath (Join-Path -Path $OutputDirectory -ChildPath "service-map-$suffix-compact.$extension")
}
