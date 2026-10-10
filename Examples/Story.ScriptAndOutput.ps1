<#
.SYNOPSIS
Creates square, portrait and widescreen writing-and-replay stories.
.DESCRIPTION
Reads a PowerShell script and its previously captured output. The script is
displayed, never executed. Each layout receives an animated GIF, an HTML player,
a completed PNG, a text transcript and the native story manifest.
Import ImagePlayground before invoking this starter.
.PARAMETER ScriptPath
Literal path to the UTF-8 script to display. Used with OutputPath.
.PARAMETER OutputPath
Literal path to the UTF-8 output captured separately from a trusted run.
.PARAMETER ScriptText
Inline script text. Used with OutputText instead of file paths.
.PARAMETER OutputText
Captured output as text. An empty string represents a run with no output.
.PARAMETER OutputDirectory
Destination for the square, portrait and widescreen folders.
.PARAMETER Formats
Layouts to generate. All three are selected by default.
.PARAMETER Appearance
Desktop window style: MacOS, Windows or Linux. Graphite uses minimal chrome.
.PARAMETER ColorMode
Dark or light surfaces and syntax colors. Syntax colors appear during typing.
.PARAMETER Command
Display-only terminal command. Defaults to ./ followed by the script filename.
.PARAMETER Overwrite
Replaces existing story exports. Input files must remain outside the destination.
.EXAMPLE
Import-Module ImagePlayground
./Story.ScriptAndOutput.ps1 -ScriptPath ./demo.ps1 -OutputPath ./output.txt `
    -Title 'My PowerShell demo' -OutputDirectory ./stories
.EXAMPLE
./Story.ScriptAndOutput.ps1 -ScriptText '$answer = 6 * 7; $answer' `
    -OutputText '42' -ScriptName answer.ps1 -Formats Square -OutputDirectory ./answer
.NOTES
Supports Windows PowerShell 5.1 and PowerShell 7. Saved output is presented with
authored timing; these timestamps are not a recording of the original execution.
GIF has no audio. MP4 encoding is a separate workflow.
#>
[CmdletBinding(DefaultParameterSetName = 'Files')]
param(
    [Parameter(Mandatory, ParameterSetName = 'Files')]
    [ValidateNotNullOrEmpty()]
    [string] $ScriptPath,

    [Parameter(Mandatory, ParameterSetName = 'Files')]
    [ValidateNotNullOrEmpty()]
    [string] $OutputPath,

    [Parameter(Mandatory, ParameterSetName = 'Text')]
    [ValidateNotNullOrEmpty()]
    [string] $ScriptText,

    [Parameter(Mandatory, ParameterSetName = 'Text')]
    [AllowEmptyString()]
    [string] $OutputText,

    [ValidateNotNullOrEmpty()]
    [string] $Title = 'Script to result',
    [string] $ScriptName,
    [string] $Command,
    [ValidateNotNullOrEmpty()]
    [string] $OutputDirectory = (Join-Path (Get-Location).Path 'StoryOutput'),
    [ValidateSet('Square', 'Portrait', 'Widescreen')]
    [ValidateNotNullOrEmpty()]
    [string[]] $Formats = @('Square', 'Portrait', 'Widescreen'),
    [ValidateSet('MacOS', 'Windows', 'Linux', 'Graphite')]
    [string] $Appearance = 'MacOS',
    [ValidateSet('Dark', 'Light')]
    [string] $ColorMode = 'Dark',
    [ValidateRange(480, 1080)]
    [int] $ShortSide = 720,
    [ValidateRange(14, 32)]
    [int] $FontSize = 22,
    [ValidateRange(1, 60)]
    [double] $WritingSeconds = 8,
    [ValidateRange(0.5, 10)]
    [double] $ReadingSeconds = 2,
    [ValidateRange(2, 60)]
    [double] $ReplaySeconds = 6,
    [ValidateRange(0.5, 10)]
    [double] $EndHoldSeconds = 2,
    [ValidateRange(2, 20)]
    [int] $FramesPerSecond = 6,
    [ValidateRange(2, 3600)]
    [int] $MaximumFrames = 600,
    [ValidateRange(0, 65536)]
    [int] $PlayCount = 0,
    [switch] $Overwrite
)

$ErrorActionPreference = 'Stop'
if (-not (Get-Command -Name New-ImageStory -ErrorAction SilentlyContinue)) {
    throw 'Import ImagePlayground with the Stories cmdlets before running this starter.'
}
$destination = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($OutputDirectory)
if ($PSCmdlet.ParameterSetName -eq 'Files') {
    foreach ($inputPath in @($ScriptPath, $OutputPath)) {
        $resolved = (Get-Item -LiteralPath $inputPath).FullName
        $prefix = $destination.TrimEnd([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar
        if ($resolved.StartsWith($prefix, [StringComparison]::OrdinalIgnoreCase)) {
            throw 'Keep script and captured-output input files outside OutputDirectory.'
        }
    }
    $scriptFile = Get-Item -LiteralPath $ScriptPath
    $ScriptText = [IO.File]::ReadAllText($scriptFile.FullName)
    $OutputText = [IO.File]::ReadAllText((Get-Item -LiteralPath $OutputPath).FullName)
    if ([string]::IsNullOrWhiteSpace($ScriptName)) { $ScriptName = $scriptFile.Name }
}
if ([string]::IsNullOrWhiteSpace($ScriptText)) { throw 'The displayed script must contain text.' }
if ([string]::IsNullOrWhiteSpace($ScriptName)) { $ScriptName = 'script.ps1' }
if ([string]::IsNullOrWhiteSpace($Command)) { $Command = './' + $ScriptName }
$layouts = @($Formats | ForEach-Object { ([ChartForgeX.Stories.VisualStoryFormat] $_).ToString() } | Select-Object -Unique)
foreach ($format in $layouts) {
    $folder = Join-Path $destination $format.ToLowerInvariant()
    foreach ($name in @('story.html', 'story.gif', 'story.png', 'story.txt', 'story.json')) {
        $path = Join-Path $folder $name
        if (-not $Overwrite -and (Test-Path -LiteralPath $path)) {
            throw "Export already exists: $path. Choose another OutputDirectory or use -Overwrite."
        }
    }
}

$source = ConvertTo-ImageStorySource -Text $ScriptText -Language PowerShell
$empty = ConvertTo-ImageStorySource -Text '' -Language PowerShell
$writing = [ChartForgeX.Stories.StorySourceTimeline]::Create($empty).
    Type($source, [TimeSpan]::FromSeconds($WritingSeconds))
$editor = [ChartForgeX.Stories.VisualStorySourceOptions]::new($ScriptName, $FontSize)
$terminal = [ChartForgeX.Stories.VisualStoryTerminalOptions]::new($FontSize)

# Preserve blank lines within output; a final newline terminates its last line.
$normalizedOutput = $OutputText.Replace("`r`n", "`n").Replace("`r", "`n")
if ($normalizedOutput.EndsWith("`n")) {
    $normalizedOutput = $normalizedOutput.Substring(0, $normalizedOutput.Length - 1)
}
[string[]] $lines = @(
    if ($OutputText.Length -gt 0) { $normalizedOutput.Split([char] 10) }
)
$events = [System.Collections.Generic.List[object]]::new()
$events.Add(@{ TimestampSeconds = 0.25; Kind = 'Command'; Text = $Command })
for ($index = 0; $index -lt $lines.Count; $index++) {
    $at = 0.75 + (($index + 1) / [double] $lines.Count) * ($ReplaySeconds - 1.5)
    $events.Add(@{ TimestampSeconds = $at; Kind = 'Output'; Text = $lines[$index] })
}
$replay = New-ImageStoryReplay -DurationSeconds $ReplaySeconds -Events $events.ToArray() -Title 'Captured output'
$scenes = @(
    New-ImageStoryScene -Id write -Title '01  Write the script' -DurationSeconds $WritingSeconds -Panels (
        New-ImageStoryPanel -Id code -SourceTimeline $writing -SourceOptions $editor
    )
    New-ImageStoryScene -Id read -Title '02  Read the script' -DurationSeconds $ReadingSeconds -Panels (
        New-ImageStoryPanel -Id code -Source $source -SourceOptions $editor
    )
    New-ImageStoryScene -Id replay -Title '03  Replay the output' -DurationSeconds $ReplaySeconds -Panels (
        New-ImageStoryPanel -Id terminal -Replay $replay -TerminalOptions $terminal
    )
)
$outcome = New-ImageStoryOutcome -Id captured -Label 'Captured output is visible.' -PanelId terminal
if ($OutputText.Length -eq 0) {
    $outcome = New-ImageStoryOutcome -Id captured -Label 'The captured run produced no output.' -PanelId terminal
}

$light = $ColorMode -eq 'Light'
$theme = switch ($Appearance) {
    'MacOS' { [ChartForgeX.Stories.VisualStoryTheme]::MacOS($light) }
    'Windows' { [ChartForgeX.Stories.VisualStoryTheme]::Windows($light) }
    'Linux' { [ChartForgeX.Stories.VisualStoryTheme]::Linux($light) }
    'Graphite' {
        if ($light) { [ChartForgeX.Stories.VisualStoryTheme]::GraphiteLight() }
        else { [ChartForgeX.Stories.VisualStoryTheme]::GraphiteDark() }
    }
}
foreach ($format in $layouts) {
    $story = [ChartForgeX.Stories.VisualStory]::Create($Title).
        WithFormat([ChartForgeX.Stories.VisualStoryFormat] $format, $ShortSide).
        WithTheme($theme).
        WithDescription('PowerShell source writing followed by captured output with authored replay timing.')
    foreach ($scene in $scenes) {
        $nativeScene = $story.Scene($scene.Id, $scene.Title, $scene.DurationSeconds, $scene.Layout)
        foreach ($panel in $scene.Panels) {
            $null = $nativeScene.Panel($panel.Id, $panel.Surface, $panel.Title, $panel.Weight)
        }
    }
    $null = $story.Outcome($outcome.Id, $outcome.Label, $outcome.PanelId)
    $folder = Join-Path $destination $format.ToLowerInvariant()
    $html = Join-Path $folder 'story.html'
    New-ImageStory -Story $story -FilePath $html -BundlePath $folder -BundleFormats Gif, Transcript `
        -Player -FramesPerSecond $FramesPerSecond -MaximumFrames $MaximumFrames `
        -EndHoldSeconds $EndHoldSeconds -PlayCount $PlayCount
    [pscustomobject]@{
        Format = $format
        Width = $story.Width
        Height = $story.Height
        Appearance = $Appearance
        ColorMode = $ColorMode
        Gif = Join-Path $folder 'story.gif'
        Html = $html
        Poster = Join-Path $folder 'story.png'
        Transcript = Join-Path $folder 'story.txt'
        Manifest = Join-Path $folder 'story.json'
    }
}
