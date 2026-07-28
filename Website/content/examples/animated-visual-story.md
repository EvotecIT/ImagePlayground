---
title: Animated Visual Story
description: Generate script-free SVG and HTML stories, plus matching static PNG output, from native ChartForgeX blocks.
weight: 35
---

Use `New-ImageVisualStory` when a profile card, release summary, report header, or dashboard should reveal information in a restrained sequence.

```powershell
New-ImageVisualStory -StoryScript {
    param($Story)

    $projects = [ChartForgeX.VisualBlocks.MetricCard]::Create()
    [void] $projects.WithMetric('Maintained projects', '24').WithCaption('reusable libraries')

    [void] $Story.WithTitle('Engineering portfolio').WithColumns(1)
    [void] $Story.Add('projects', $projects)
} -MotionDefinition {
    New-ImageVisualMotionCue -TargetId title -Effect Reveal -DurationSeconds 0.65
    New-ImageVisualMotionCue -TargetId projects -Effect Rise -DelaySeconds 0.25
} -FilePath '.\portfolio.svg'
```

The story model is generic: the same targets and cues work for profiles, contribution summaries, product releases, operational reports, and project portfolios. SVG and complete HTML pages animate without JavaScript. PNG, print, and reduced-motion rendering preserve the completed state so motion never hides the information.
