@{
    AliasesToExport      = @('New-ImageQRCode', 'New-QRCodeWiFi')
    Author               = 'Przemyslaw Klys'
    CmdletsToExport      = @()
    CompanyName          = 'Evotec'
    CompatiblePSEditions = @('Desktop', 'Core')
    Copyright            = '(c) 2011 - 2022 Przemyslaw Klys @ Evotec. All rights reserved.'
    Description          = 'ImagePlayground'
    FunctionsToExport    = @('New-ImageChart', 'New-ImageChartBar', 'New-ChartLegend', 'New-ImageChartLine', 'New-ImageChartPie', 'New-QRCode', 'New-ImageQRCodeWiFi', 'New-ImageQRContact')
    GUID                 = 'ff5469f2-c542-4318-909e-fd054d16821f'
    ModuleVersion        = '0.0.1'
    PowerShellVersion    = '5.1'
    PrivateData          = @{
        PSData = @{
            Tags       = @('windows', 'macos', 'linux')
            ProjectUri = 'https://github.com/EvotecIT/ImagePlayground'
        }
    }
    RootModule           = 'ImagePlayground.psm1'
}