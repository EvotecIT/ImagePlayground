---
title: Animated Visual Story
description: Generate script-free SVG and HTML stories, plus matching static PNG output, from native ChartForgeX blocks.
weight: 35
---

Use `New-ImageVisualStory` when a profile card, release summary, report header, or dashboard should reveal information in a restrained sequence.

```powershell
$theme = 'Light'
$item = New-ImageVisualGridItem -TargetId projects -Block (
    New-ImageMetricCard -Label 'Maintained projects' -Value 24 -Caption 'Reusable libraries' -Theme $theme -Width 336 -Height 176
)
$grid = New-ImageVisualGrid -Title 'Engineering portfolio' -Content $item -Columns 1 -Padding 16 -Theme $theme
$grid | New-ImageVisualStory -MotionDefinition {
    New-ImageVisualMotionCue -TargetId title -Effect Reveal -DurationSeconds 0.65
    New-ImageVisualMotionCue -TargetId projects -Effect Rise -DelaySeconds 0.25
} -FilePath '.\portfolio.svg'
```

The story model is generic: the same targets and cues work for profiles, contribution summaries, product releases, operational reports, and project portfolios. SVG and complete HTML pages animate without JavaScript. PNG, print, and reduced-motion rendering preserve the completed state so motion never hides the information.

`Examples\VisualStory.Profile.ps1` authors wide and compact versions in either theme. Motion preserves each fixed composition; it does not reflow a wide SVG into a narrow layout.
