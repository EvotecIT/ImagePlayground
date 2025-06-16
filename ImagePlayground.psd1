@{
    AliasesToExport        = @('New-QRCode', 'New-QRCodeWiFi')
    Author                 = 'Przemyslaw Klys'
    CmdletsToExport        = @('ConvertTo-Image','Resize-Image','Get-Image','Get-ImageBarCode','Merge-Image','Save-Image','Get-ImageQRCode','New-ImageQRCode','New-ImageQRCodeWiFi','New-ImageBarCode','Get-ImageExif','Set-ImageExif','Remove-ImageExif')
    CompanyName            = 'Evotec'
    CompatiblePSEditions   = @('Desktop', 'Core')
    Copyright              = '(c) 2011 - 2025 Przemyslaw Klys @ Evotec. All rights reserved.'
    Description            = 'ImagePlayground is a PowerShell module that provides a set of functions for image processing. Among other things it can create QRCodes, BarCodes, Charts, and do image processing that can help with daily tasks.'
    DotNetFrameworkVersion = '4.7.2'
    FunctionsToExport      = @('New-ImageChart','New-ImageChartBar','New-ImageChartBarOptions','New-ImageChartLine','New-ImageChartPie','New-ImageChartRadial','New-ImageQRContact')
    GUID                   = 'ff5469f2-c542-4318-909e-fd054d16821f'
    ModuleVersion          = '0.0.9'
    PowerShellVersion      = '5.1'
    PrivateData            = @{
        PSData = @{
            ExternalModuleDependencies = @('Microsoft.PowerShell.Management', 'Microsoft.PowerShell.Utility')
            IconUri                    = 'https://evotec.xyz/wp-content/uploads/2022/07/ImagePlayground.png'
            LicenseUri                 = 'https://github.com/EvotecIT/ImagePlayground/blob/master/License'
            ProjectUri                 = 'https://github.com/EvotecIT/ImagePlayground'
            Tags                       = @('windows', 'image', 'charts', 'qrcodes', 'barcodes')
        }
    }
    RequiredModules        = @('Microsoft.PowerShell.Management', 'Microsoft.PowerShell.Utility')
    RootModule             = 'ImagePlayground.psm1'
}