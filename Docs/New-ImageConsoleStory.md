---
external help file: ImagePlayground-help.xml
Module Name: ImagePlayground
online version: https://github.com/EvotecIT/ImagePlayground
schema: 2.0.0
---
# New-ImageConsoleStory
## SYNOPSIS
Creates a script-free animated console presentation from authored steps, captured transcript lines, or a native ChartForgeX terminal story.

## SYNTAX
### StoryScript (Default)
```powershell
New-ImageConsoleStory [-StoryScript] <scriptblock> -FilePath <string> [-Show] [-PassThru] [<CommonParameters>]
```

### Story
```powershell
New-ImageConsoleStory -Story <TerminalStory> -FilePath <string> [-Show] [-PassThru] [<CommonParameters>]
```

### Transcript
```powershell
New-ImageConsoleStory -InputObject <string> -CommandText <string> -FilePath <string> [-Dialect <TerminalDialect>] [-CustomPrompt <string>] [-Title <string>] [-WorkingDirectory <string>] [-Show] [-PassThru] [<CommonParameters>]
```

## DESCRIPTION
The cmdlet renders deterministic SVG or HTML motion and a completed PNG state. It never executes the displayed command: callers run scripts themselves and pipe captured output when they want a real execution transcript.

## EXAMPLES

### EXAMPLE 1
```powershell
PS> New-ImageConsoleStory -StoryScript {
  param($Console)
  [void] $Console.WithTitle('pwsh - C:\OpenSource').WithWorkingDirectory('C:\OpenSource')
  [void] $Console.Command('Get-Service -Name WinRM')
  [void] $Console.Output('Status   Name               DisplayName', [ChartForgeX.Terminal.TerminalTextTone]::Accent)
  [void] $Console.Output('Running  WinRM              Windows Remote Management', [ChartForgeX.Terminal.TerminalTextTone]::Success)
} -FilePath '.\service-demo.svg'
```

Creates a self-contained SVG with command typing, output reveals, a blinking cursor, and a completed reduced-motion state.

### EXAMPLE 2
```powershell
PS> $output = & .\Invoke-EnvironmentAudit.ps1 2>&1 | Out-String -Stream -Width 110
$output | New-ImageConsoleStory -CommandText '.\Invoke-EnvironmentAudit.ps1' -Dialect PowerShell -FilePath '.\audit-demo.svg'
```

The caller controls execution; the cmdlet only turns the captured lines into a deterministic presentation.

## PARAMETERS

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
Accept wildcard characters: True
```

### -CustomPrompt
Prompt text used when Dialect is Custom.

```yaml
Type: String
Parameter Sets: Transcript
Aliases:
Possible values:

Required: False
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
```

### -Dialect
Prompt dialect used for captured transcript presentations.

Possible values: PowerShell, Bash, CommandPrompt, Python, CSharp, Custom

```yaml
Type: TerminalDialect
Parameter Sets: Transcript
Aliases:
Possible values: PowerShell, Bash, CommandPrompt, Python, CSharp, Custom

Required: False
Position: named
Default value: PowerShell
Accept pipeline input: False
Accept wildcard characters: True
```

### -FilePath
Output file path. Supported extensions are SVG, HTML, HTM, and PNG.

```yaml
Type: String
Parameter Sets: StoryScript, Story, Transcript
Aliases:
Possible values:

Required: True
Position: named
Default value: None
Accept pipeline input: False
Accept wildcard characters: True
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
Accept wildcard characters: True
```

### -PassThru
Write the configured ChartForgeX TerminalStory to the pipeline.

```yaml
Type: SwitchParameter
Parameter Sets: StoryScript, Story, Transcript
Aliases:
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
```

### -Show
Open the generated presentation after creation.

```yaml
Type: SwitchParameter
Parameter Sets: StoryScript, Story, Transcript
Aliases:
Possible values:

Required: False
Position: named
Default value: False
Accept pipeline input: False
Accept wildcard characters: True
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
Accept wildcard characters: True
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
Accept wildcard characters: True
```

### -Title
Terminal title shown for captured transcript presentations.

```yaml
Type: String
Parameter Sets: Transcript
Aliases:
Possible values:

Required: False
Position: named
Default value: PowerShell
Accept pipeline input: False
Accept wildcard characters: True
```

### -WorkingDirectory
Working directory shown in shell prompts for captured transcript presentations.

```yaml
Type: String
Parameter Sets: Transcript
Aliases:
Possible values:

Required: False
Position: named
Default value: C:\
Accept pipeline input: False
Accept wildcard characters: True
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

- `ChartForgeX.Terminal.TerminalStory`
- `System.String`

## OUTPUTS

- `ChartForgeX.Terminal.TerminalStory`

## RELATED LINKS

- None
