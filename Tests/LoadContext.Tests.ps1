Describe 'Assembly Load Context' {

    It 'creates a dedicated context on import' -Skip:($PSVersionTable.PSVersion.Major -lt 7) {
        Import-Module (Join-Path -Path $PSScriptRoot -ChildPath '..\ImagePlayground.psm1') -Force
        $context = [System.Runtime.Loader.AssemblyLoadContext]::All | Where-Object Name -eq 'ImagePlayground'
        $context | Should -Not -BeNullOrEmpty
    }

    It 'imports packaged binaries by default when development mode is disabled' -Skip:($PSVersionTable.PSVersion.Major -lt 7) {
        $moduleManifestPath = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '..\ImagePlayground.psd1')).Path
        $moduleRootPath = Split-Path -Path $moduleManifestPath -Parent
        $developmentPath = Join-Path -Path $moduleRootPath -ChildPath 'Sources\ImagePlayground.PowerShell\bin\Debug'
        $pwshPath = (Get-Process -Id $PID).Path
        $sanitizedModulePath = @(
            $env:PSModulePath -split [System.IO.Path]::PathSeparator | Where-Object {
                $_ -and
                $_ -notmatch '[\\/]Documents[\\/]PowerShell[\\/]Modules(?:[\\/]|$)' -and
                $_ -notmatch '[\\/]Documents[\\/]WindowsPowerShell[\\/]Modules(?:[\\/]|$)'
            }
        ) -join [System.IO.Path]::PathSeparator
        $scriptPath = Join-Path -Path ([System.IO.Path]::GetTempPath()) -ChildPath ("ImagePlayground.LoadContext.{0}.ps1" -f ([guid]::NewGuid().ToString('N')))
        $escapedModuleManifestPath = $moduleManifestPath.Replace("'", "''")
        $escapedDevelopmentPath = $developmentPath.Replace("'", "''")
        $escapedModulePathList = $sanitizedModulePath.Replace("'", "''")
        $command = @"
`$ErrorActionPreference = 'Stop'
`$env:PSModulePath = '$escapedModulePathList'
Remove-Item -Path Env:IMAGEPLAYGROUND_DEVELOPMENT -ErrorAction SilentlyContinue
`$developmentPath = '$escapedDevelopmentPath'
`$hiddenDevelopmentPath = if (Test-Path -LiteralPath `$developmentPath) {
    `$developmentPath + '.hidden'
} else {
    `$null
}

try {
    if (`$hiddenDevelopmentPath) {
        Move-Item -LiteralPath `$developmentPath -Destination `$hiddenDevelopmentPath -Force
    }

    Import-Module '$escapedModuleManifestPath' -Force -ErrorAction Stop
    `$module = Get-Module -Name ImagePlayground -ErrorAction Stop
    `$binaryModulePaths = @(
        Get-Module -Name ImagePlayground.PowerShell -All -ErrorAction SilentlyContinue |
            Select-Object -ExpandProperty Path
    )

    [pscustomobject]@{
        ImportedModulePath = `$module.Path
        BinaryModulePaths = `$binaryModulePaths
        ExportedCommandCount = @(Get-Command -Module ImagePlayground).Count
        DevelopmentPathPresent = Test-Path -LiteralPath `$developmentPath
        DevelopmentPathHidden = [bool] `$hiddenDevelopmentPath
    } | ConvertTo-Json -Compress
} finally {
    if (`$hiddenDevelopmentPath -and (Test-Path -LiteralPath `$hiddenDevelopmentPath)) {
        Move-Item -LiteralPath `$hiddenDevelopmentPath -Destination `$developmentPath -Force
    }
}
"@

        try {
            Set-Content -Path $scriptPath -Value $command -Encoding UTF8
            $result = & $pwshPath -NoProfile -File $scriptPath 2>&1
            $resultText = ($result | ForEach-Object { $_.ToString() }) -join [Environment]::NewLine

            $LASTEXITCODE | Should -Be 0 -Because $resultText

            $json = $result | Select-Object -Last 1
            $data = $json | ConvertFrom-Json

            $data.ImportedModulePath | Should -Match '[\\/]ImagePlayground\.psm1$'
            if ($data.DevelopmentPathPresent) {
                $data.DevelopmentPathHidden | Should -BeTrue
            }
            $data.BinaryModulePaths | Should -Not -BeNullOrEmpty
            $data.ExportedCommandCount | Should -BeGreaterThan 0
            ($data.BinaryModulePaths -join [Environment]::NewLine) | Should -Match '[\\/]Lib[\\/]'
            ($data.BinaryModulePaths -join [Environment]::NewLine) | Should -Match 'ImagePlayground\.PowerShell\.dll'
        } finally {
            Remove-Item -Path $scriptPath -Force -ErrorAction SilentlyContinue
        }
    }
}
