$output = Join-Path -Path $PSScriptRoot -ChildPath 'Output\engineering-portfolio.svg'

New-ImageVisualStory -StoryScript {
    param($Story)

    $projects = [ChartForgeX.VisualBlocks.MetricCard]::Create()
    [void] $projects.WithMetric('Maintained projects', '24').WithCaption('reusable libraries')

    $community = [ChartForgeX.VisualBlocks.MetricCard]::Create()
    [void] $community.WithMetric('Community stars', '8.2K').WithCaption('across active projects')

    [void] $Story.WithTitle('Engineering portfolio').WithSubtitle('A reusable visual story from native ChartForgeX blocks')
    [void] $Story.WithColumns(2).WithGap(16).WithPadding(24).WithFrame()
    [void] $Story.Add('projects', $projects)
    [void] $Story.Add('community', $community)
} -MotionDefinition {
    New-ImageVisualMotionCue -TargetId title -Effect Reveal -DurationSeconds 0.65
    New-ImageVisualMotionCue -TargetId subtitle -Effect Fade -DelaySeconds 0.12 -DurationSeconds 0.5
    New-ImageVisualMotionCue -TargetId projects -Effect Rise -DelaySeconds 0.28 -DurationSeconds 0.6
    New-ImageVisualMotionCue -TargetId community -Effect Rise -DelaySeconds 0.4 -DurationSeconds 0.6
} -FilePath $output
