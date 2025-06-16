@{
    RootModule = '../ImagePlayground.psm1'
    ModuleVersion = '0.0.0'
    CompatiblePSEditions = @('Desktop','Core')
    FunctionsToExport = '*'
    CmdletsToExport = @()
    AliasesToExport = '*'
    PrivateData = @{}
    RequiredModules = @('Microsoft.PowerShell.Management','Microsoft.PowerShell.Utility')
}
