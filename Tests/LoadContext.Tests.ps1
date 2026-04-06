Describe 'Assembly Load Context' {

    It 'creates a dedicated context on import' -Skip:($PSVersionTable.PSVersion.Major -lt 7) {
        $context = [System.Runtime.Loader.AssemblyLoadContext]::All | Where-Object Name -eq 'ImagePlayground'
        $context | Should -Not -BeNullOrEmpty
    }

    It 'imports packaged binaries by default when development mode is disabled' -Skip:($PSVersionTable.PSVersion.Major -lt 7) {
        $modulePath = Join-Path -Path $PSScriptRoot -ChildPath '..\ImagePlayground.psd1'
        $moduleScriptPath = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '..\ImagePlayground.psm1')).Path
        $samplePath = Join-Path -Path $PSScriptRoot -ChildPath '..\Sources\ImagePlayground.Tests\Images\QRCode1.png'
        $pwshPath = (Get-Process -Id $PID).Path
        $sanitizedModulePath = @(
            $env:PSModulePath -split [System.IO.Path]::PathSeparator | Where-Object {
                $_ -and
                $_ -notmatch '[\\/]Documents[\\/]PowerShell[\\/]Modules(?:[\\/]|$)' -and
                $_ -notmatch '[\\/]Documents[\\/]WindowsPowerShell[\\/]Modules(?:[\\/]|$)'
            }
        ) -join [System.IO.Path]::PathSeparator
        $command = @"
`$env:PSModulePath = '$sanitizedModulePath'
`$env:IMAGEPLAYGROUND_DEVELOPMENT = `$null
Import-Module '$modulePath' -Force -ErrorAction Stop
`$image = Get-Image -FilePath '$samplePath'
try {
    [pscustomobject]@{
        ImportedModulePath = (Get-Module ImagePlayground).Path
        AssemblyLocation = [ImagePlayground.Image].Assembly.Location
        Width = `$image.Width
    } | ConvertTo-Json -Compress
} finally {
    `$image.Dispose()
}
"@

        $result = & $pwshPath -NoProfile -Command $command
        $data = $result | ConvertFrom-Json

        $data.ImportedModulePath | Should -Be $moduleScriptPath
        $data.AssemblyLocation | Should -Match '[\\/]Lib[\\/]'
        $data.AssemblyLocation | Should -Not -Match '[\\/]Sources[\\/]'
        $data.Width | Should -Be 660
    }
}
