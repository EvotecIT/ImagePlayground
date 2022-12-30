@{
    AliasesToExport        = @('New-QRCode', 'New-QRCodeWiFi')
    Author                 = 'Przemyslaw Klys'
    CmdletsToExport        = @()
    CompanyName            = 'Evotec'
    CompatiblePSEditions   = @('Desktop', 'Core')
    Copyright              = '(c) 2011 - 2022 Przemyslaw Klys @ Evotec. All rights reserved.'
    Description            = 'ImagePlayground is a PowerShell module that provides a set of functions for image processing. Among other things it can create QRCodes, BarCodes, Charts, and do image processing that can help with daily tasks.'
    DotNetFrameworkVersion = '4.7.2'
    FunctionsToExport      = @('ConvertTo-Image', 'Get-Image', 'Get-ImageBarCode', 'Get-ImageQRCode', 'Merge-Image', 'New-ImageBarCode', 'New-ImageChart', 'New-ImageChartBar', 'New-ImageChartBarOptions', 'New-ChartLegend', 'New-ImageChartLine', 'New-ImageChartPie', 'New-ImageQRCode', 'New-ImageQRCodeWiFi', 'New-ImageQRContact', 'Resize-Image', 'Save-Image')
    GUID                   = 'ff5469f2-c542-4318-909e-fd054d16821f'
    ModuleVersion          = '0.0.3'
    PowerShellVersion      = '5.1'
    PrivateData            = @{
        PSData = @{
            Tags       = @('windows', 'image', 'charts', 'qrcodes', 'barcodes')
            LicenseUri = 'https://github.com/EvotecIT/ImagePlayground/blob/master/License'
            ProjectUri = 'https://github.com/EvotecIT/ImagePlayground'
            IconUri    = 'https://evotec.xyz/wp-content/uploads/2022/07/ImagePlayground.png'
        }
    }
    RootModule             = 'ImagePlayground.psm1'
}