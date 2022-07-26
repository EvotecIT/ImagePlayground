$framework = if ($PSVersionTable.PSVersion.Major -eq 5) {
    'net472'
}
else {
    'netcoreapp3.1'
}

# Load the extra deps needed by the module
# This Add-Type can be removed if using cmdlets in the binary module
Add-Type -LiteralPath $PSScriptRoot\bin\$framework\QRCoder.dll

$importModule = Get-Command -Name Import-Module -Module Microsoft.PowerShell.Core
if (-not ('ImagePlayground.PowerShell.NewImageChart' -as [type])) {
    &$importModule ([IO.Path]::Combine($PSScriptRoot, 'bin', $framework, 'ImagePlayground.dll')) -ErrorAction Stop
}
else {
    &$importModule -Force -Assembly ([ImagePlayground.PowerShell.NewImageChart].Assembly)
}

#Get public and private function definition files.
$Public = @( Get-ChildItem -Path $PSScriptRoot\Public\*.ps1 -ErrorAction SilentlyContinue -Recurse )
$Private = @( Get-ChildItem -Path $PSScriptRoot\Private\*.ps1 -ErrorAction SilentlyContinue -Recurse )
$Classes = @( Get-ChildItem -Path $PSScriptRoot\Classes\*.ps1 -ErrorAction SilentlyContinue -Recurse )
$Enums = @( Get-ChildItem -Path $PSScriptRoot\Enums\*.ps1 -ErrorAction SilentlyContinue -Recurse )

#Dot source the files
$FoundErrors = @(
    Foreach ($Import in @($Private + $Public + $Classes + $Enums)) {
        Try {
            . $Import.Fullname
        }
        Catch {
            Write-Error -Message "Failed to import functions from $($import.Fullname): $_"
            $true
        }
    }
)

if ($FoundErrors.Count -gt 0) {
    $ModuleName = (Get-ChildItem $PSScriptRoot\*.psd1).BaseName
    Write-Warning "Importing module $ModuleName failed. Fix errors before continuing."
    break
}

Export-ModuleMember -Function '*' -Cmdlet 'New-ImageQRContact' -Alias '*'