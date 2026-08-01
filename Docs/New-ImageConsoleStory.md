---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageConsoleStory
## SYNOPSIS
Creates a script-free animated console presentation from PowerShell-native steps, captured transcript lines, or a native ChartForgeX terminal story.

## SYNTAX
### StoryScript (Default)
```powershell
New-ImageConsoleStory [-StoryScript] <scriptblock> -FilePath <string> [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

### Content
```powershell
New-ImageConsoleStory -Content <scriptblock> -FilePath <string> [-Dialect <TerminalDialect>] [-CustomPrompt <string>] [-Title <string>] [-WorkingDirectory <string>] [-Theme <string>] [-WindowStyle <TerminalWindowStyle>] [-Width <int>] [-FontSize <double>] [-LineHeight <double>] [-InitialDelaySeconds <double>] [-CharactersPerSecond <double>] [-LineDelaySeconds <double>] [-NoFinalPrompt] [-PngOutputScale <int>] [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

### Step
```powershell
New-ImageConsoleStory -Step <ImageConsoleStoryStep[]> -FilePath <string> [-Dialect <TerminalDialect>] [-CustomPrompt <string>] [-Title <string>] [-WorkingDirectory <string>] [-Theme <string>] [-WindowStyle <TerminalWindowStyle>] [-Width <int>] [-FontSize <double>] [-LineHeight <double>] [-InitialDelaySeconds <double>] [-CharactersPerSecond <double>] [-LineDelaySeconds <double>] [-NoFinalPrompt] [-PngOutputScale <int>] [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

### Story
```powershell
New-ImageConsoleStory -Story <TerminalStory> -FilePath <string> [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

### Transcript
```powershell
New-ImageConsoleStory -InputObject <string> -CommandText <string> -FilePath <string> [-Dialect <TerminalDialect>] [-CustomPrompt <string>] [-Title <string>] [-WorkingDirectory <string>] [-Theme <string>] [-WindowStyle <TerminalWindowStyle>] [-Width <int>] [-FontSize <double>] [-LineHeight <double>] [-InitialDelaySeconds <double>] [-CharactersPerSecond <double>] [-LineDelaySeconds <double>] [-NoFinalPrompt] [-PngOutputScale <int>] [-FramesPerSecond <int>] [-EndHoldSeconds <double>] [-AnimationScale <int>] [-MaximumFrames <int>] [-NoLoop] [-Show] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
The recommended Content and Step parameter sets compose objects created by the New-ImageConsoleStoryCommand, Output, Table, BlankLine, and Pause cmdlets. StoryScript remains available as the low-level ChartForgeX builder escape hatch.

The cmdlet renders deterministic SVG or HTML motion, animated GIF or APNG motion, and a completed PNG state. It never executes the displayed command: callers run scripts themselves and pipe captured output when they want a real execution transcript.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageConsoleStory -Title 'pwsh - C:\OpenSource' -WorkingDirectory 'C:\OpenSource' -Theme PowerShell -WindowStyle WindowsTerminal -Content {
  New-ImageConsoleStoryCommand -Text 'Get-Service -Name WinRM'
  New-ImageConsoleStoryOutput -Text 'Status   Name               DisplayName' -Tone Accent
  New-ImageConsoleStoryOutput -Text 'Running  WinRM              Windows Remote Management' -Tone Success
} -FilePath '.\service-demo.svg'
```

Creates a self-contained SVG with command typing, output reveals, a blinking cursor, and a completed reduced-motion state.

### EXAMPLE 2
```powershell
PS> $output = & .\Invoke-EnvironmentAudit.ps1 2>&1 | Out-String -Stream -Width 110
$output | New-ImageConsoleStory -CommandText '.\Invoke-EnvironmentAudit.ps1' -Dialect PowerShell -FilePath '.\audit-demo.svg'
```

The caller controls execution; the cmdlet only turns the captured lines into a deterministic presentation.

### EXAMPLE 3
```powershell
PS> New-ImageConsoleStory -Dialect CSharp -Title 'dotnet run - ChartForgeX' -Content {
  New-ImageConsoleStoryCommand -Text 'var chart = Chart.Create().WithTitle("Weekly builds");'
  New-ImageConsoleStoryCommand -Text 'chart.SavePng("weekly-builds.png");'
  New-ImageConsoleStoryOutput -Text 'Saved weekly-builds.png' -Tone Success
} -FilePath '.\chart-demo.gif' -FramesPerSecond 10 -EndHoldSeconds 1.5
```

GIF and APNG export sample the same deterministic terminal timeline used by SVG and HTML.

## PARAMETERS

### -AnimationScale
Raster density multiplier used for animated GIF and APNG output.

```yaml
Type: Int32
Parameter Sets: StoryScript, Content, Step, Story, Transcript
Aliases:
Possible values:

Required: False
Position: named
Default value: 1
Accept pipeline input: False
Accept wildcard characters: False
```

### -CharactersPerSecond
Simulated command typing speed.

```yaml
Type: Double
Parameter Sets: Content, Step, Transcript
Aliases:
Possible values:

Required: False
Position: named
Default value: 42
Accept pipeline input: False
Accept wildcard characters: False
```

### -CommandText
Command text shown before captured transcript lines.

```yaml
Type: String
Parameter Sets: Transcript
Aliases:
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Content
PowerShell-native authoring block that emits steps created by the New-ImageConsoleStoryCommand, Output, Table, BlankLine, and Pause cmdlets.

```yaml
Type: ScriptBlock
Parameter Sets: Content
Aliases:
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
Aliases:
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
Aliases:
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
Aliases:
Possible values:

Required: False
Position: named
Default value: 1,2
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Output file path. Supported extensions are SVG, HTML, HTM, PNG, GIF, and APNG.

```yaml
Type: String
Parameter Sets: StoryScript, Content, Step, Story, Transcript
Aliases:
Possible values:

Required: True
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
Aliases:
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
Aliases:
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
Aliases:
Possible values:

Required: False
Position: named
Default value: 0,35
Accept pipeline input: False
Accept wildcard characters: False
```

### -InputObject
One captured output line. Accepts pipeline input and never executes the displayed command.

```yaml
Type: String
Parameter Sets: Transcript
Aliases:
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
Aliases:
Possible values:

Required: False
Position: named
Default value: 0,08
Accept pipeline input: False
Accept wildcard characters: False
```

### -LineHeight
Terminal line height used by composed and captured stories.

```yaml
Type: Double
Parameter Sets: Content, Step, Transcript
Aliases:
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
Aliases:
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
Aliases:
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
Aliases:
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Write the configured ChartForgeX TerminalStory to the pipeline.

```yaml
Type: SwitchParameter
Parameter Sets: StoryScript, Content, Step, Story, Transcript
Aliases:
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
Aliases:
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
Aliases:
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### -Step
Typed console story steps. Accepts pipeline input and arrays created by the New-ImageConsoleStoryCommand, Output, Table, BlankLine, and Pause cmdlets.

```yaml
Type: ImageConsoleStoryStep[]
Parameter Sets: Step
Aliases:
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
Aliases:
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
Aliases:
Possible values:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Theme
Built-in terminal color palette used by composed and captured stories.

```yaml
Type: String
Parameter Sets: Content, Step, Transcript
Aliases:
Possible values: Dark, PowerShell, Classic, Light

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
Aliases:
Possible values:

Required: False
Position: named
Default value: PowerShell
Accept pipeline input: False
Accept wildcard characters: False
```

### -Width
Logical terminal width used by composed and captured stories.

```yaml
Type: Int32
Parameter Sets: Content, Step, Transcript
Aliases:
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
Aliases:
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
Aliases:
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

- `ImagePlayground.PowerShell.ImageConsoleStoryStep[]` — Use the New-ImageConsoleStoryCommand, New-ImageConsoleStoryOutput, New-ImageConsoleStoryTable, New-ImageConsoleStoryBlankLine, and New-ImageConsoleStoryPause cmdlets to create steps.
- `ChartForgeX.Terminal.TerminalStory`
- `System.String`

## OUTPUTS

- `ChartForgeX.Terminal.TerminalStory`

## RELATED LINKS

- None
