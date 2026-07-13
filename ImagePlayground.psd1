@{
    AliasesToExport        = @()
    Author                 = 'Przemyslaw Klys'
    CmdletsToExport        = @('Add-ImageText', 'Add-ImageTextBox', 'Add-ImageWatermark', 'Clear-ImageThumbnailCache', 'Compare-Image', 'ConvertFrom-ImageBase64', 'ConvertTo-Image', 'ConvertTo-ImageBase64', 'Export-ImageMetadata', 'Get-Image', 'Get-ImageExif', 'Get-ImageHeifInfo', 'Get-ImageHeifXmp', 'Import-ImageMetadata', 'Merge-Image', 'New-ImageAvatar', 'New-ImageCrop', 'New-ImageGif', 'New-ImageGrid', 'New-ImageIcon', 'New-ImageMosaic', 'New-ImageThumbnail', 'Remove-ImageExif', 'Remove-ImageHeifXmp', 'Remove-ImageMetadata', 'Resize-Image', 'Save-Image', 'Set-ImageAdjust', 'Set-ImageBlur', 'Set-ImageExif', 'Set-ImageHeifXmp', 'Set-ImageRotation', 'Set-ImageSharpen')
    CompanyName            = 'Evotec'
    CompatiblePSEditions   = @('Desktop', 'Core')
    Copyright              = '(c) 2011 - 2026 Przemyslaw Klys @ Evotec. All rights reserved.'
    Description            = 'PowerShell commands for image conversion, resizing, composition, text, watermarks, metadata, thumbnails, icons, mosaics, grids, avatars, and GIFs.'
    DotNetFrameworkVersion = '4.7.2'
    FunctionsToExport      = @()
    GUID                   = 'ff5469f2-c542-4318-909e-fd054d16821f'
    ModuleVersion          = '3.0.0'
    PowerShellVersion      = '5.1'
    PrivateData            = @{
        PSData = @{
            IconUri                    = 'https://evotec.xyz/wp-content/uploads/2022/07/ImagePlayground.png'
            LicenseUri                 = 'https://github.com/EvotecIT/ImagePlayground/blob/master/LICENSE'
            ProjectUri                 = 'https://github.com/EvotecIT/ImagePlayground'
            RequireLicenseAcceptance   = $false
            Tags                       = @('windows', 'image', 'image-processing', 'exif', 'metadata')
            ExternalModuleDependencies = @()
        }
    }
    RootModule             = 'ImagePlayground.psm1'
    RequiredModules        = @()
    ScriptsToProcess       = @()
}
