$output = Join-Path -Path $PSScriptRoot -ChildPath 'Output\environment-audit.svg'
$script = Join-Path -Path $PSScriptRoot -ChildPath 'Invoke-EnvironmentAudit.ps1'

$transcript = & $script 2>&1 | Out-String -Stream -Width 110
$transcript | New-ImageConsoleStory `
    -CommandText '.\Invoke-EnvironmentAudit.ps1' `
    -Dialect PowerShell `
    -WorkingDirectory 'C:\Audit' `
    -FilePath $output
