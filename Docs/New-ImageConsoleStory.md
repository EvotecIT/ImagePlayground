---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageConsoleStory
## SYNOPSIS
Creates a reusable script-free console story from PowerShell-native steps, captured transcript lines, or a native ChartForgeX terminal story.

## SYNTAX
### StoryScript (Default)
```powershell
New-ImageConsoleStory [-StoryScript] <scriptblock> [-FilePath <string>] [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

### Content
```powershell
New-ImageConsoleStory -Content <scriptblock> [-Dialect <TerminalDialect>] [-CustomPrompt <string>] [-Title <string>] [-WorkingDirectory <string>] [-Theme <string>] [-Palette <TerminalTheme>] [-WindowStyle <TerminalWindowStyle>] [-Width <int>] [-FontSize <double>] [-LineHeight <double>] [-InitialDelaySeconds <double>] [-TypingSpeed <double>] [-LineDelaySeconds <double>] [-Speed <TerminalStoryPlaybackSpeed>] [-TabHoldSeconds <double>] [-NoFinalPrompt] [-PngOutputScale <int>] [-FilePath <string>] [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

### Step
```powershell
New-ImageConsoleStory -Step <ImageConsoleStoryStep[]> [-Dialect <TerminalDialect>] [-CustomPrompt <string>] [-Title <string>] [-WorkingDirectory <string>] [-Theme <string>] [-Palette <TerminalTheme>] [-WindowStyle <TerminalWindowStyle>] [-Width <int>] [-FontSize <double>] [-LineHeight <double>] [-InitialDelaySeconds <double>] [-TypingSpeed <double>] [-LineDelaySeconds <double>] [-Speed <TerminalStoryPlaybackSpeed>] [-TabHoldSeconds <double>] [-NoFinalPrompt] [-PngOutputScale <int>] [-FilePath <string>] [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

### Story
```powershell
New-ImageConsoleStory -Story <TerminalStory> [-FilePath <string>] [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

### Transcript
```powershell
New-ImageConsoleStory -InputObject <string> -CommandText <string> [-Dialect <TerminalDialect>] [-CustomPrompt <string>] [-Title <string>] [-WorkingDirectory <string>] [-Theme <string>] [-Palette <TerminalTheme>] [-WindowStyle <TerminalWindowStyle>] [-Width <int>] [-FontSize <double>] [-LineHeight <double>] [-InitialDelaySeconds <double>] [-TypingSpeed <double>] [-LineDelaySeconds <double>] [-Speed <TerminalStoryPlaybackSpeed>] [-TabHoldSeconds <double>] [-NoFinalPrompt] [-PngOutputScale <int>] [-FilePath <string>] [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
The recommended Content and Step parameter sets compose objects created by the New-ImageConsoleStoryCommand, Output, Table, BlankLine, Pause, Tab, and Select-ImageConsoleStoryTab cmdlets. StoryScript remains available as the low-level ChartForgeX builder escape hatch.

The cmdlet renders deterministic SVG or HTML motion, animated GIF or APNG motion, and a completed PNG state. It never executes the displayed command: callers run scripts themselves and pipe captured output when they want a real execution transcript.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> $story = New-ImageConsoleStory -WindowStyle WindowsTerminal -Width 1100 -Speed Slow -Content {
  New-ImageConsoleStoryTab -Id PowerShell -Title 'PowerShell' -Profile PowerShell -Active
  New-ImageConsoleStoryCommand -Text 'dotnet build'
  New-ImageConsoleStoryOutput -Text 'Build succeeded.' -Style Success

  New-ImageConsoleStoryTab -Id WindowsPowerShell -Title 'Windows PowerShell' -Profile WindowsPowerShell
  New-ImageConsoleStoryCommand -Text '.\Invoke-LegacyTests.ps1'
  New-ImageConsoleStoryOutput -Text 'PS 5.1 compatibility passed.' -Style Success

  New-ImageConsoleStoryTab -Id Ubuntu -Title 'Ubuntu' -Profile Ubuntu
  New-ImageConsoleStoryCommand -Text './build.sh'
  New-ImageConsoleStoryOutput -Text 'Linux package ready.' -Style Success
}
$story | Export-ImageConsoleStory -Path '.\demo.gif'
```

Creates three persistent tab buffers. Each new tab opens atomically after the previous tab's reading dwell, then the shared story is exported through Export-ImageConsoleStory.

### EXAMPLE 2
```powershell
PS> $story = New-ImageConsoleStory -WindowStyle WindowsTerminal -Speed Slow -Content {
  New-ImageConsoleStoryTab -Id PowerShell -Profile PowerShell -Active
  New-ImageConsoleStoryTab -Id Logs -Title 'Build logs' -Profile PowerShell -Background
  New-ImageConsoleStoryCommand -Text 'dotnet build'
  Select-ImageConsoleStoryTab -Id Logs
  New-ImageConsoleStoryOutput -Text 'Waiting for integration tests...' -Style Muted
  New-ImageConsoleStoryPause -Seconds 1.5
  Select-ImageConsoleStoryTab -Id PowerShell
  New-ImageConsoleStoryCommand -Text 'Get-ChildItem .\artifacts'
}
$story | Export-ImageConsoleStory -Path '.\navigation.gif'
```

Background prepares an inactive tab. Each Select step deliberately switches to a retained buffer; a pause can show that session ready and a later command continues it.

### EXAMPLE 3
```powershell
PS> $output = & .\Invoke-EnvironmentAudit.ps1 2>&1 | Out-String -Stream -Width 110
$story = $output | New-ImageConsoleStory -CommandText '.\Invoke-EnvironmentAudit.ps1' -Dialect PowerShell
$story | Export-ImageConsoleStory -Path '.\audit-demo.svg'
```

The caller controls execution; the cmdlet only turns the captured lines into a deterministic presentation.

### EXAMPLE 4
```powershell
PS> $story = New-ImageConsoleStory -Dialect CSharp -Title 'dotnet run - ChartForgeX' -Content {
  New-ImageConsoleStoryCommand -Text 'var chart = Chart.Create().WithTitle("Weekly builds");'
  New-ImageConsoleStoryCommand -Text 'chart.SavePng("weekly-builds.png");'
  New-ImageConsoleStoryOutput -Text 'Saved weekly-builds.png' -Style Success
}
$story | Export-ImageConsoleStory -Path '.\chart-demo.gif' -FramesPerSecond 10 -EndHoldSeconds 1.5
```

GIF and APNG export sample the same deterministic terminal timeline used by SVG and HTML.

### EXAMPLE 5
```powershell
PS> $story = New-ImageConsoleStory -Speed Normal -TypingSpeed 36 -TabHoldSeconds 2.5 -Content {
  New-ImageConsoleStoryCommand -Text 'Invoke-ProjectBuild'
  New-ImageConsoleStoryOutput -Text 'Build completed.' -Style Success
}
```

TypingSpeed is measured in visible characters per second. A command-level DurationSeconds value remains the most specific override.

## PARAMETERS

### -AnimationScale
Raster density multiplier used for animated GIF and APNG output.

```yaml
Type: Int32
Parameter Sets: StoryScript, Content, Step, Story, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: 1
Accept pipeline input: False
Accept wildcard characters: False
```

### -CommandText
Command text shown before captured transcript lines.

```yaml
Type: String
Parameter Sets: Transcript
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Content
PowerShell-native authoring block that emits command, output, table, pause, tab declaration, and tab-selection steps.

```yaml
Type: ScriptBlock
Parameter Sets: Content
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CustomPrompt
Prompt text used when Dialect is Custom.

```yaml
Type: String
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Dialect
Prompt dialect used for captured transcript presentations.

Possible values: PowerShell, Bash, CommandPrompt, Python, CSharp, Custom

```yaml
Type: TerminalDialect
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values: PowerShell, Bash, CommandPrompt, Python, CSharp, Custom

Required: False
Position: named
Default value: PowerShell
Accept pipeline input: False
Accept wildcard characters: False
```

### -EndHoldSeconds
Completed-state hold time used for animated GIF and APNG output.

```yaml
Type: Double
Parameter Sets: StoryScript, Content, Step, Story, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: 1.2
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Output file path. Supported extensions are SVG, HTML, HTM, PNG, GIF, and APNG.

```yaml
Type: String
Parameter Sets: StoryScript, Content, Step, Story, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FontSize
Terminal font size used by composed and captured stories.

```yaml
Type: Double
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: 14
Accept pipeline input: False
Accept wildcard characters: False
```

### -FramesPerSecond
Frame rate used for animated GIF and APNG output.

```yaml
Type: Int32
Parameter Sets: StoryScript, Content, Step, Story, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: 10
Accept pipeline input: False
Accept wildcard characters: False
```

### -InitialDelaySeconds
Delay before the first animated step.

```yaml
Type: Double
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: 0.35
Accept pipeline input: False
Accept wildcard characters: False
```

### -InputObject
One captured output line. Accepts pipeline input and never executes the displayed command.

```yaml
Type: String
Parameter Sets: Transcript
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -LineDelaySeconds
Delay between output lines.

```yaml
Type: Double
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: 0.08
Accept pipeline input: False
Accept wildcard characters: False
```

### -LineHeight
Terminal line height used by composed and captured stories.

```yaml
Type: Double
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: 22
Accept pipeline input: False
Accept wildcard characters: False
```

### -MaximumFrames
Maximum frame budget used for animated GIF and APNG output.

```yaml
Type: Int32
Parameter Sets: StoryScript, Content, Step, Story, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: 240
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoFinalPrompt
Hide the final prompt and cursor in the completed story.

```yaml
Type: SwitchParameter
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoLoop
Produce a single-play animated GIF or APNG instead of a repeating animation.

```yaml
Type: SwitchParameter
Parameter Sets: StoryScript, Content, Step, Story, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Palette
Optional custom palette, normally created by New-ImageConsoleStoryPalette.

```yaml
Type: TerminalTheme
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Write the configured ChartForgeX TerminalStory to the pipeline.

```yaml
Type: SwitchParameter
Parameter Sets: StoryScript, Content, Step, Story, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -PngOutputScale
PNG output density multiplier used by composed and captured stories.

```yaml
Type: Int32
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: 2
Accept pipeline input: False
Accept wildcard characters: False
```

### -Show
Open the generated presentation after creation.

```yaml
Type: SwitchParameter
Parameter Sets: StoryScript, Content, Step, Story, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Speed
Reusable playback pace. Slow leaves the most reading time, Normal is balanced, and Fast is intended for short demos.

Possible values: Slow, Normal, Fast

```yaml
Type: TerminalStoryPlaybackSpeed
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values: Slow, Normal, Fast

Required: False
Position: named
Default value: Normal
Accept pipeline input: False
Accept wildcard characters: False
```

### -Step
Typed console story steps. Accepts pipeline input and arrays created by the console story step cmdlets.

```yaml
Type: ImageConsoleStoryStep[]
Parameter Sets: Step
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Story
Native ChartForgeX terminal story to render.

```yaml
Type: TerminalStory
Parameter Sets: Story
Aliases: None
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -StoryScript
Script block that receives and configures a new ChartForgeX TerminalStory.

```yaml
Type: ScriptBlock
Parameter Sets: StoryScript
Aliases: None
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TabHoldSeconds
Optional minimum reading time after content appears and before the active tab changes. Overrides the selected Speed preset.

```yaml
Type: Double
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### -Theme
Built-in terminal color palette used by composed and captured stories.

```yaml
Type: String
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values: Dark, PowerShell, WindowsPowerShell, Ubuntu, Campbell, Classic, Light

Required: False
Position: named
Default value: Dark
Accept pipeline input: False
Accept wildcard characters: False
```

### -Title
Terminal title shown for captured transcript presentations.

```yaml
Type: String
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: PowerShell
Accept pipeline input: False
Accept wildcard characters: False
```

### -TypingSpeed
Simulated command typing speed in visible characters per second. Overrides the selected Speed preset.

```yaml
Type: Double
Parameter Sets: Content, Step, Transcript
Aliases: CharactersPerSecond
Possible values:

Required: False
Position: named
Default value: 42
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Logical terminal width used by composed and captured stories.

```yaml
Type: Int32
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: 886
Accept pipeline input: False
Accept wildcard characters: False
```

### -WindowStyle
Visible terminal window chrome, independent of the color palette and prompt dialect.

Possible values: MacOS, WindowsTerminal, Minimal, None

```yaml
Type: TerminalWindowStyle
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values: MacOS, WindowsTerminal, Minimal, None

Required: False
Position: named
Default value: MacOS
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkingDirectory
Working directory shown in shell prompts for captured transcript presentations.

```yaml
Type: String
Parameter Sets: Content, Step, Transcript
Aliases: None
Possible values:

Required: False
Position: named
Default value: C:\
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `ImagePlayground.PowerShell.ImageConsoleStoryStep[]`: Use the console story command, output, table, blank-line, pause, tab, and tab-selection cmdlets to create steps.
- `ChartForgeX.Terminal.TerminalStory`
- `System.String`

## OUTPUTS

- `ChartForgeX.Terminal.TerminalStory`

## RELATED LINKS

- None
