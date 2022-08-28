# ImagePlayground - PowerShell Module

<p align="center">
  <a href="https://dev.azure.com/evotecpl/ImagePlayground/_build/results?buildId=latest"><img src="https://img.shields.io/azure-devops/build/evotecpl/39c74615-8f34-4af0-a835-68dc33f9214f/14?label=Azure%20Pipelines&style=flat-square"></a>
  <a href="https://www.powershellgallery.com/packages/ImagePlayground"><img src="https://img.shields.io/powershellgallery/v/ImagePlayground.svg?style=flat-square"></a>
  <a href="https://www.powershellgallery.com/packages/ImagePlayground"><img src="https://img.shields.io/powershellgallery/vpre/ImagePlayground.svg?label=powershell%20gallery%20preview&colorB=yellow&style=flat-square"></a>
  <a href="https://github.com/EvotecIT/ImagePlayground"><img src="https://img.shields.io/github/license/EvotecIT/ImagePlayground.svg?style=flat-square"></a>
</p>

<p align="center">
  <a href="https://www.powershellgallery.com/packages/ImagePlayground"><img src="https://img.shields.io/powershellgallery/p/ImagePlayground.svg?style=flat-square"></a>
  <a href="https://github.com/EvotecIT/ImagePlayground"><img src="https://img.shields.io/github/languages/top/evotecit/ImagePlayground.svg?style=flat-square"></a>
  <a href="https://github.com/EvotecIT/ImagePlayground"><img src="https://img.shields.io/github/languages/code-size/evotecit/ImagePlayground.svg?style=flat-square"></a>
  <a href="https://www.powershellgallery.com/packages/ImagePlayground"><img src="https://img.shields.io/powershellgallery/dt/ImagePlayground.svg?style=flat-square"></a>
</p>

<p align="center">
  <a href="https://twitter.com/PrzemyslawKlys"><img src="https://img.shields.io/twitter/follow/PrzemyslawKlys.svg?label=Twitter%20%40PrzemyslawKlys&style=flat-square&logo=twitter"></a>
  <a href="https://evotec.xyz/hub"><img src="https://img.shields.io/badge/Blog-evotec.xyz-2A6496.svg?style=flat-square"></a>
  <a href="https://www.linkedin.com/in/pklys"><img src="https://img.shields.io/badge/LinkedIn-pklys-0077B5.svg?logo=LinkedIn&style=flat-square"></a>
</p>

`ImagePlayground` is a PowerShell module that allows you to play with images. It allows to create QR codes, BAR codes or create Charts and do other cool stuff.

## Known Issues

This library will work fine for PowerShell 5.1 and PowerShell 7+.
It works only on Windows as far as I know. Requires testing.

### Features

- [x] QR Codes
  - [x] QR Code Standard
  - [x] QR Code WiFi
  - [x] QR Code Contact
  - [ ] Others to be added
- [x] Barcode
  - [x] EAN
  - [x] Code.39
- [x] Barcode Reader - library can read barcodes
- [x] QR Code Reader - library can read QR codes
- [ ] Image Resize
- [ ] Image Crop
- [ ] Image Rotate
- [ ] Image Grayscale
- [ ] Image Blur

### Installation

```powershell
Install-Module ImagePlayGround -Force -Verbose
```

### Usage

#### Creating and reading QR Codes

Creating basic QR Code

```powershell
New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode.png"
```

Creating QR codes and reading them back is as easy as:

```powershell
New-ImageQRContact -FilePath "$PSScriptRoot\Samples\QRCodeContact.png" -outputType VCard4 -Firstname "Przemek" -Lastname "Klys" -MobilePhone "+48 500 000 000"

$Message = Get-ImageQRCode -FilePath "$PSScriptRoot\Samples\QRCodeContact.png"
$Message | Format-List *

New-ImageQRCodeWiFi -SSID 'Evotec' -Password 'EvotecPassword' -FilePath "$PSScriptRoot\Samples\QRCodeWiFi.png"

$Message = Get-ImageQRCode -FilePath "$PSScriptRoot\Samples\QRCodeWiFi.png"
$Message | Format-List *

New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode.png"

$Message = Get-ImageQRCode -FilePath "$PSScriptRoot\Samples\QRCode.png"
$Message | Format-List *
```

#### Creating charts

```powershell
New-ImageChart {
    New-ImageChartBar -Value 5 -Label "C#"
    New-ImageChartBar -Value 12 -Label "C++"
    New-ImageChartBar -Value 10 -Label "PowerShell"
} -Show -FilePath $PSScriptRoot\Samples\ChartsBar1.png
```

#### Reading bar codes

```powershell
Get-ImageBarCode -FilePath $PSScriptRoot\Samples\BarcodeEAN13.png
Get-ImageBarCode -FilePath $PSScriptRoot\Samples\BarcodeEAN7.png
```




### Libraries

#### Current libraries and their licenses:
- [QRCoder-ImageSharp](https://github.com/JPlenert/QRCoder-ImageSharp) - licensed MIT
- [ScottPlot](https://github.com/ScottPlot/ScottPlot) - licensed MIT
- [Barcoder](https://github.com/huysentruitw/barcoder) - licensed MIT
- [BarcodeReader.ImageSharp](https://github.com/baltermia/barcodereader-imagesharp) - licensed MIT
- [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) - licensed Apache 2.0

#### Future Considerations / Alternatives
If any of the libraries above prove insufficient, we can add more libraries to the list or replace with those

- [OxyPlot](https://github.com/oxyplot/oxyplot) - licensed MIT
- [Microcharts](https://github.com/microcharts-dotnet/Microcharts) - licensed MIT

#### DLL Dependencies and the horror story

This module is tricky to get right DLLs to load properly in both PowerShell 5.1 and PowerShell 7.
To make sure I remember why things are this way I have a few notes here.

- ScottPlot.dll needs to be .NET Standard DLL for PowerShell 7
- ScottPlot.dll needs to be .NET Framework DLL for PowerShell 5.1
- System.Memory.DLL and System.Numerics.Vectors.dll needs to be extracted from VSCode (`[System.AppDomain]::CurrentDomain.GetAssemblies()`) - actual path: `C:\Users\przemyslaw.klys\.vscode\extensions\ms-vscode.powershell-preview-2022.7.0\modules\PowerShellEditorServices\bin\Common`. The ones that come when I build the project are not working correctly.
- Rest of DLLs are coming from `Sources\ImagePlayground.PowerShell\bin` and are .NET Standard
- This ImagePlayground.PowerShell uses special entry `<CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>` in `.csproj` to make sure DLLS are copied