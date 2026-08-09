Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

$nodes = @(
    New-ImageTopologyNode -Id client -Label Client -Kind Person -Status Healthy
    New-ImageTopologyNode -Id api -Label API -Kind Service -Status Healthy
    New-ImageTopologyNode -Id database -Label Database -Kind Database -Status Warning
)
$edges = @(
    New-ImageTopologyEdge -Id client-api -SourceNodeId client -TargetNodeId api -Direction Forward
    New-ImageTopologyEdge -Id api-database -SourceNodeId api -TargetNodeId database -Direction Forward
)
$scenario = New-ImageTopologyScenario -Id request -Label 'Request flow' -StepDefinition {
    New-ImageTopologyScenarioStep -Id client-api -Kind Edge
    New-ImageTopologyScenarioStep -Id api-database -Kind Edge
}
$motion = New-ImageTopologyMotion -ScenarioId request -DurationSeconds 4 -FramesPerSecond 12

New-ImageTopology -Node $nodes -Edge $edges -Scenario $scenario -Motion $motion -FilePath "$PSScriptRoot\request-flow.gif" -Width 720 -Height 420
