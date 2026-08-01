$svgOutput = Join-Path -Path $PSScriptRoot -ChildPath 'Output\environment-audit.svg'
$gifOutput = Join-Path -Path $PSScriptRoot -ChildPath 'Output\environment-audit.gif'
$script = Join-Path -Path $PSScriptRoot -ChildPath 'Invoke-EnvironmentAudit.ps1'

$transcript = & $script 2>&1 | Out-String -Stream -Width 110
$story = $transcript | New-ImageConsoleStory `
    -CommandText '.\Invoke-EnvironmentAudit.ps1' `
    -Dialect PowerShell `
    -WorkingDirectory 'C:\Audit' `
    -Theme PowerShell `
    -WindowStyle Minimal `
    -FilePath $svgOutput `
    -PassThru

$story | New-ImageConsoleStory `
    -FilePath $gifOutput `
    -FramesPerSecond 8 `
    -EndHoldSeconds 1.5
