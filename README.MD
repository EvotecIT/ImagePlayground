

### Features

- [x] QR Codes
  - [x] QR Code Standard
  - [x] QR Code WiFi
  - [ ] QR Code Contact
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
- System.Memory.DLL and System.Numerics.Vectors.dll needs to be extracted from VSCode (`[System.AppDomain]::CurrentDomain.GetAssemblies()`) - actual path: `C:\Users\przemyslaw.klys\.vscode\extensions\ms-vscode.powershell-preview-2022.7.0\modules\PowerShellEditorServices\bin\Common`
- Rest of DLLs are coming from `Sources\ImagePlayground.PowerShell\bin` and are .NET Standard
- This ImagePlayground.PowerShell uses special entry `<CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>` in `.csproj` to make sure DLLS are copied

I spent a lot of time getting this right