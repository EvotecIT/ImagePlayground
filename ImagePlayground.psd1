﻿@{
    AliasesToExport        = @('New-QRCode', 'New-QRCodeWiFi')
    Author                 = 'Przemyslaw Klys'
    CmdletsToExport        = @()
    CompanyName            = 'Evotec'
    CompatiblePSEditions   = @('Desktop', 'Core')
    Copyright              = '(c) 2011 - 2024 Przemyslaw Klys @ Evotec. All rights reserved.'
    Description            = 'ImagePlayground is a PowerShell module that provides a set of functions for image processing. Among other things it can create QRCodes, BarCodes, Charts, and do image processing that can help with daily tasks.'
    DotNetFrameworkVersion = '4.7.2'
    FunctionsToExport      = @('ConvertTo-Image', 'Get-Image', 'Get-ImageBarCode', 'Get-ImageExif', 'Get-ImageQRCode', 'Merge-Image', 'New-ImageBarCode', 'New-ImageChart', 'New-ImageChartBar', 'New-ImageChartBarOptions', 'New-ImageChartLine', 'New-ImageChartPie', 'New-ImageChartRadial', 'New-ImageQRCode', 'New-ImageQRCodeWiFi', 'New-ImageQRContact', 'Remove-ImageExif', 'Resize-Image', 'Save-Image', 'Set-ImageExif')
    GUID                   = 'ff5469f2-c542-4318-909e-fd054d16821f'
    ModuleVersion          = '0.0.8'
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