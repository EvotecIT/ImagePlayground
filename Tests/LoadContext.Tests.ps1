Describe 'Assembly Load Context' {

    It 'creates a dedicated context on import' -Skip:($PSVersionTable.PSVersion.Major -lt 7) {
        Import-Module (Join-Path -Path $PSScriptRoot -ChildPath '..\ImagePlayground.psm1') -Force
        $context = [System.Runtime.Loader.AssemblyLoadContext]::All | Where-Object Name -eq 'ImagePlayground'
        $context | Should -Not -BeNullOrEmpty
    }

    It 'imports packaged binaries by default when development mode is disabled' -Skip:($PSVersionTable.PSVersion.Major -lt 7) {
        $moduleScriptPath = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '..\ImagePlayground.psm1')).Path
        $pwshPath = (Get-Process -Id $PID).Path
        $sanitizedModulePath = @(
            $env:PSModulePath -split [System.IO.Path]::PathSeparator | Where-Object {
                $_ -and
                $_ -notmatch '[\\/]Documents[\\/]PowerShell[\\/]Modules(?:[\\/]|$)' -and
                $_ -notmatch '[\\/]Documents[\\/]WindowsPowerShell[\\/]Modules(?:[\\/]|$)'
            }
        ) -join [System.IO.Path]::PathSeparator
        $scriptPath = Join-Path -Path ([System.IO.Path]::GetTempPath()) -ChildPath ("ImagePlayground.LoadContext.{0}.ps1" -f ([guid]::NewGuid().ToString('N')))
        $escapedModuleScriptPath = $moduleScriptPath.Replace("'", "''")
        $escapedModulePathList = $sanitizedModulePath.Replace("'", "''")
        $command = @"
`$ErrorActionPreference = 'Stop'
`$VerbosePreference = 'Continue'
`$env:PSModulePath = '$escapedModulePathList'
Remove-Item -Path Env:IMAGEPLAYGROUND_DEVELOPMENT -ErrorAction SilentlyContinue
`$importOutput = Import-Module '$escapedModuleScriptPath' -Force -Verbose 4>&1
`$module = Get-Module -Name ImagePlayground -ErrorAction Stop
`$verboseMessages = [System.Collections.Generic.List[string]]::new()
foreach (`$record in `$importOutput) {
    if (`$record -is [System.Management.Automation.VerboseRecord]) {
        [void] `$verboseMessages.Add(`$record.Message)
    }
}
`$binaryImportMessage = `$verboseMessages | Where-Object { `$_ -like 'Importing binary module from *' } | Select-Object -First 1
[pscustomobject]@{
    ImportedModulePath = `$module.Path
    BinaryImportMessage = `$binaryImportMessage
    ExportedCommandCount = `$module.ExportedCommands.Count
} | ConvertTo-Json -Compress
"@

        try {
            Set-Content -Path $scriptPath -Value $command -Encoding UTF8
            $result = & $pwshPath -NoProfile -File $scriptPath 2>&1
            $resultText = ($result | ForEach-Object { $_.ToString() }) -join [Environment]::NewLine

            $LASTEXITCODE | Should -Be 0 -Because $resultText

            $json = $result | Select-Object -Last 1
            $data = $json | ConvertFrom-Json

            $data.ImportedModulePath | Should -Be $moduleScriptPath
            $data.BinaryImportMessage | Should -Match '[\\/]Lib[\\/]'
            $data.BinaryImportMessage | Should -Not -Match '[\\/]Sources[\\/]'
            $data.ExportedCommandCount | Should -BeGreaterThan 0
        } finally {
            Remove-Item -Path $scriptPath -Force -ErrorAction SilentlyContinue
        }
    }
}
