[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

[pscustomobject[]] @(
    [pscustomobject] @{
        Check  = 'PowerShell'
        Status = 'PASS'
        Detail = $PSVersionTable.PSVersion.ToString()
    }
    [pscustomobject] @{
        Check  = 'Operating system'
        Status = 'PASS'
        Detail = [System.Environment]::OSVersion.VersionString
    }
    [pscustomobject] @{
        Check  = 'Working directory'
        Status = 'PASS'
        Detail = (Get-Location).Path
    }
) | Format-Table -AutoSize
