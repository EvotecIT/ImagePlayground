$svgOutput = Join-Path -Path $PSScriptRoot -ChildPath 'Output\environment-audit.svg'
$gifOutput = Join-Path -Path $PSScriptRoot -ChildPath 'Output\environment-audit.gif'
$script = Join-Path -Path $PSScriptRoot -ChildPath 'Invoke-EnvironmentAudit.ps1'

# Execution is explicit and remains under the caller's control.
$transcript = & $script 2>&1 | Out-String -Stream -Width 110
$storyOptions = @{
    CommandText      = '.\Invoke-EnvironmentAudit.ps1'
    Dialect          = 'PowerShell'
    WorkingDirectory = 'C:\Audit'
    Theme            = 'PowerShell'
    WindowStyle      = 'Minimal'
    Speed            = 'Normal'
}

$story = $transcript | New-ImageConsoleStory @storyOptions
$story | Export-ImageConsoleStory -Path $svgOutput
$story | Export-ImageConsoleStory -Path $gifOutput -FramesPerSecond 8 -EndHoldSeconds 1.5
