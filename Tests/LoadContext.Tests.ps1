Describe 'Assembly Load Context' {

    It 'creates a dedicated context on import' -Skip:($PSVersionTable.PSVersion.Major -lt 7) {
        $context = [System.Runtime.Loader.AssemblyLoadContext]::All | Where-Object Name -eq 'ImagePlayground'
        $context | Should -Not -BeNullOrEmpty
    }

    It 'imports packaged binaries by default when development mode is disabled' -Skip:($PSVersionTable.PSVersion.Major -lt 7) {
        $modulePath = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '..\ImagePlayground.psd1')).Path
        $moduleScriptPath = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '..\ImagePlayground.psm1')).Path
        $samplePath = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '..\Sources\ImagePlayground.Tests\Images\QRCode1.png')).Path
        $pwshPath = (Get-Process -Id $PID).Path
        $sanitizedModulePath = @(
            $env:PSModulePath -split [System.IO.Path]::PathSeparator | Where-Object {
                $_ -and
                $_ -notmatch '[\\/]Documents[\\/]PowerShell[\\/]Modules(?:[\\/]|$)' -and
                $_ -notmatch '[\\/]Documents[\\/]WindowsPowerShell[\\/]Modules(?:[\\/]|$)'
            }
        ) -join [System.IO.Path]::PathSeparator
        $scriptPath = Join-Path -Path ([System.IO.Path]::GetTempPath()) -ChildPath ("ImagePlayground.LoadContext.{0}.ps1" -f ([guid]::NewGuid().ToString('N')))
        $escapedModulePath = $modulePath.Replace("'", "''")
        $escapedSamplePath = $samplePath.Replace("'", "''")
        $escapedModulePathList = $sanitizedModulePath.Replace("'", "''")
        $command = @"
`$ErrorActionPreference = 'Stop'
`$env:PSModulePath = '$escapedModulePathList'
Remove-Item -Path Env:IMAGEPLAYGROUND_DEVELOPMENT -ErrorAction SilentlyContinue
Import-Module '$escapedModulePath' -Force -ErrorAction Stop
`$module = Get-Module -Name ImagePlayground -ErrorAction Stop
Get-Command -Name 'Get-Image' -Module ImagePlayground -ErrorAction Stop | Out-Null
`$image = ImagePlayground\Get-Image -FilePath '$escapedSamplePath'
try {
    [pscustomobject]@{
        ImportedModulePath = `$module.Path
        AssemblyLocation = [ImagePlayground.Image].Assembly.Location
        Width = `$image.Width
    } | ConvertTo-Json -Compress
} finally {
    if (`$null -ne `$image) {
        `$image.Dispose()
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

            $data.ImportedModulePath | Should -Be $moduleScriptPath
            $data.AssemblyLocation | Should -Match '[\\/]Lib[\\/]'
            $data.AssemblyLocation | Should -Not -Match '[\\/]Sources[\\/]'
            $data.Width | Should -Be 660
        } finally {
            Remove-Item -Path $scriptPath -Force -ErrorAction SilentlyContinue
        }
    }
}
