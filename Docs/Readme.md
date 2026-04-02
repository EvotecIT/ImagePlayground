---
Module Name: ImagePlayground
Module Guid: ff5469f2-c542-4318-909e-fd054d16821f
Download Help Link: https://github.com/EvotecIT/ImagePlayground
Help Version: 2.0.0
Locale: en-US
---
# ImagePlayground Module
## Description
ImagePlayground is a PowerShell module that provides a set of functions for image processing. Among other things it can create QRCodes, BarCodes, Charts, and do image processing that can help with daily tasks.

## ImagePlayground Cmdlets
### [Add-ImageText](Add-ImageText.md)
Add-ImageText [-FilePath] <string> [-OutputPath] <string> [-Text] <string> [-X] <float> [-Y] <float> [-Color <Color>] [-FontSize <float>] [-FontFamily <string>] [-ShadowColor <Color>] [-ShadowOffsetX <float>] [-ShadowOffsetY <float>] [-OutlineColor <Color>] [-OutlineWidth <float>] [<CommonParameters>]

### [Add-ImageTextBox](Add-ImageTextBox.md)
Add-ImageTextBox [-FilePath] <string> [-OutputPath] <string> [-Text] <string> [-X] <float> [-Y] <float> [-Width] <float> [[-Height] <float>] [-Color <Color>] [-FontSize <float>] [-FontFamily <string>] [-HorizontalAlignment <HorizontalAlignment>] [-VerticalAlignment <VerticalAlignment>] [-ShadowColor <Color>] [-ShadowOffsetX <float>] [-ShadowOffsetY <float>] [-OutlineColor <Color>] [-OutlineWidth <float>] [<CommonParameters>]

### [Add-ImageWatermark](Add-ImageWatermark.md)
Add-ImageWatermark [-FilePath] <string> [-OutputPath] <string> [-WatermarkPath] <string> [-Placement <WatermarkPlacement>] [-Opacity <float>] [-Padding <float>] [-Rotate <int>] [-FlipMode <FlipMode>] [-WatermarkPercentage <int>] [-Spacing <int>] [-Async] [<CommonParameters>]

Add-ImageWatermark [-FilePath] <string> [-OutputPath] <string> [-WatermarkPath] <string> [-X <int>] [-Y <int>] [-Opacity <float>] [-Rotate <int>] [-FlipMode <FlipMode>] [-WatermarkPercentage <int>] [-Spacing <int>] [-Async] [<CommonParameters>]

### [Clear-ImageThumbnailCache](Clear-ImageThumbnailCache.md)
Clear-ImageThumbnailCache [<CommonParameters>]

### [Compare-Image](Compare-Image.md)
Compare-Image [-FilePath] <string> [-FilePathToCompare] <string> [[-OutputPath] <string>] [<CommonParameters>]

### [ConvertFrom-ImageBase64](ConvertFrom-ImageBase64.md)
ConvertFrom-ImageBase64 [-Base64] <string> [-OutputPath] <string> [-Open] [<CommonParameters>]

### [ConvertTo-Image](ConvertTo-Image.md)
ConvertTo-Image [-FilePath] <string> [-OutputPath] <string> [-Quality <int>] [-CompressionLevel <int>] [<CommonParameters>]

### [ConvertTo-ImageBase64](ConvertTo-ImageBase64.md)
ConvertTo-ImageBase64 [-FilePath] <string> [<CommonParameters>]

### [Export-ImageMetadata](Export-ImageMetadata.md)
Export-ImageMetadata [-FilePath] <string> [[-OutputPath] <string>] [<CommonParameters>]

### [Get-Image](Get-Image.md)
Get-Image [-FilePath] <string> [<CommonParameters>]

### [Get-ImageBarCode](Get-ImageBarCode.md)
Get-ImageBarCode [-FilePath] <string> [<CommonParameters>]

### [Get-ImageExif](Get-ImageExif.md)
Get-ImageExif [-FilePath] <string> [-Translate] [<CommonParameters>]

### [Get-ImageQRCode](Get-ImageQRCode.md)
Get-ImageQRCode [-FilePath] <string> [<CommonParameters>]

### [Import-ImageMetadata](Import-ImageMetadata.md)
Import-ImageMetadata [-FilePath] <string> [-MetadataPath] <string> [[-OutputPath] <string>] [<CommonParameters>]

### [Merge-Image](Merge-Image.md)
Merge-Image [-FilePath] <string> [-FilePathToMerge] <string> [-FilePathOutput] <string> [-ResizeToFit] [-Placement <ImagePlacement>] [<CommonParameters>]

### [New-ImageAvatar](New-ImageAvatar.md)
New-ImageAvatar [-FilePath] <string> [-OutputPath] <string> [-Width <int>] [-Height <int>] [-CornerRadius <float>] [-Open] [<CommonParameters>]

New-ImageAvatar [-FilePath] <string> [-OutputStream] <Stream> [-Width <int>] [-Height <int>] [-CornerRadius <float>] [-Open] [<CommonParameters>]

### [New-ImageBarCode](New-ImageBarCode.md)
New-ImageBarCode [-Type] <BarcodeType> [-Value] <string> [-FilePath] <string> [<CommonParameters>]

### [New-ImageChart](New-ImageChart.md)
New-ImageChart [[-ChartsDefinition] <scriptblock>] -FilePath <string> [-Definition <Charts+ChartDefinition[]>] [-AnnotationsDefinition <scriptblock>] [-Annotation <Charts+ChartAnnotation[]>] [-Width <int>] [-Height <int>] [-XTitle <string>] [-YTitle <string>] [-Show] [-ShowGrid] [-Theme <ChartTheme>] [<CommonParameters>]

### [New-ImageChartAnnotation](New-ImageChartAnnotation.md)
New-ImageChartAnnotation [-X] <double> [-Y] <double> [-Text] <string> [-Arrow] [<CommonParameters>]

### [New-ImageChartBar](New-ImageChartBar.md)
New-ImageChartBar [-Name] <string> [-Value] <double[]> [-Color <Color>] [<CommonParameters>]

### [New-ImageChartBarOptions](New-ImageChartBarOptions.md)
New-ImageChartBarOptions [-ShowValuesAboveBars] [<CommonParameters>]

### [New-ImageChartHeatmap](New-ImageChartHeatmap.md)
New-ImageChartHeatmap [-Name] <string> [-Matrix] <double[,]> [<CommonParameters>]

### [New-ImageChartHistogram](New-ImageChartHistogram.md)
New-ImageChartHistogram [-Name] <string> [-Values] <double[]> [-BinSize <int>] [<CommonParameters>]

### [New-ImageChartLine](New-ImageChartLine.md)
New-ImageChartLine [-Name] <string> [-Value] <double[]> [-Color <Color>] [-Marker <MarkerShape>] [-Smooth] [<CommonParameters>]

### [New-ImageChartPie](New-ImageChartPie.md)
New-ImageChartPie [-Name] <string> [-Value] <double> [-Color <Color>] [<CommonParameters>]

### [New-ImageChartPolar](New-ImageChartPolar.md)
New-ImageChartPolar [-Name] <string> [-Angle] <double[]> [-Value] <double[]> [-Color <Color>] [<CommonParameters>]

### [New-ImageChartRadial](New-ImageChartRadial.md)
New-ImageChartRadial [-Name] <string> [-Value] <double> [-Color <Color>] [<CommonParameters>]

### [New-ImageChartScatter](New-ImageChartScatter.md)
New-ImageChartScatter [-Name] <string> [-X] <double[]> [-Y] <double[]> [-Color <Color>] [<CommonParameters>]

### [New-ImageCrop](New-ImageCrop.md)
New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-X <int>] [-Y <int>] [-Width <int>] [-Height <int>] [-Open] [<CommonParameters>]

New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-CenterX <float>] [-CenterY <float>] [-Radius <float>] [-Open] [<CommonParameters>]

New-ImageCrop [-FilePath] <string> [-OutputPath] <string> [-Points <PointF[]>] [-Open] [<CommonParameters>]

### [New-ImageGif](New-ImageGif.md)
New-ImageGif [-Frames] <string[]> [-FilePath] <string> [-FrameDelay <int>] [<CommonParameters>]

### [New-ImageGrid](New-ImageGrid.md)
New-ImageGrid [-FilePath] <string> [-Width] <int> [-Height] <int> [-Color <Color>] [-Open] [<CommonParameters>]

### [New-ImageIcon](New-ImageIcon.md)
New-ImageIcon [-FilePath] <string> [-OutputPath] <string> [-Size <int[]>] [-Open] [<CommonParameters>]

### [New-ImageQRCode](New-ImageQRCode.md)
New-ImageQRCode [-Content] <string> [-FilePath] <string> [-Show] [-Transparent] [<CommonParameters>]

### [New-ImageQRCodeBezahlCode](New-ImageQRCodeBezahlCode.md)
New-ImageQRCodeBezahlCode [-Authority] <PayloadGenerator+BezahlCode+AuthorityType> [-Name] <string> [-Account] <string> [-Bnc] <string> [-Iban] <string> [-Bic] <string> [-Reason] <string> [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodeBitcoin](New-ImageQRCodeBitcoin.md)
New-ImageQRCodeBitcoin [-Currency] <PayloadGenerator+BitcoinLikeCryptoCurrencyAddress+BitcoinLikeCryptoCurrencyType> [-Address] <string> [[-Amount] <double>] [[-Label] <string>] [[-Message] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodeGeoLocation](New-ImageQRCodeGeoLocation.md)
New-ImageQRCodeGeoLocation [-Latitude] <string> [-Longitude] <string> [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodeGirocode](New-ImageQRCodeGirocode.md)
New-ImageQRCodeGirocode [-Iban] <string> [-Bic] <string> [-Name] <string> [-Amount] <decimal> [[-RemittanceInformation] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodeMonero](New-ImageQRCodeMonero.md)
New-ImageQRCodeMonero [-Address] <string> [[-Amount] <float>] [[-PaymentId] <string>] [[-RecipientName] <string>] [[-Description] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodeOtp](New-ImageQRCodeOtp.md)
New-ImageQRCodeOtp [-Payload] <PayloadGenerator+OneTimePassword> [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodePhoneNumber](New-ImageQRCodePhoneNumber.md)
New-ImageQRCodePhoneNumber [-Number] <string> [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodeShadowSocks](New-ImageQRCodeShadowSocks.md)
New-ImageQRCodeShadowSocks [-Host] <string> [-Port] <int> [-Password] <string> [-Method] <PayloadGenerator+ShadowSocksConfig+Method> [[-Tag] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodeSkypeCall](New-ImageQRCodeSkypeCall.md)
New-ImageQRCodeSkypeCall [-UserName] <string> [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodeSlovenianUpnQr](New-ImageQRCodeSlovenianUpnQr.md)
New-ImageQRCodeSlovenianUpnQr [-Payload] <PayloadGenerator+SlovenianUpnQr> [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodeSms](New-ImageQRCodeSms.md)
New-ImageQRCodeSms [-Number] <string> [[-Message] <string>] [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodeSwiss](New-ImageQRCodeSwiss.md)
New-ImageQRCodeSwiss [-Payload] <PayloadGenerator+SwissQrCode> [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRCodeWiFi](New-ImageQRCodeWiFi.md)
New-ImageQRCodeWiFi [-SSID] <string> [-Password] <string> [-FilePath] <string> [-Show] [<CommonParameters>]

### [New-ImageQRContact](New-ImageQRContact.md)
New-ImageQRContact [-FilePath] <string> [-OutputType <PayloadGenerator+ContactData+ContactOutputType>] [-Firstname <string>] [-Lastname <string>] [-Nickname <string>] [-Phone <string>] [-MobilePhone <string>] [-WorkPhone <string>] [-Email <string>] [-Birthday <datetime>] [-Website <string>] [-Street <string>] [-HouseNumber <string>] [-City <string>] [-ZipCode <string>] [-Country <string>] [-Note <string>] [-StateRegion <string>] [-AddressOrder <PayloadGenerator+ContactData+AddressOrder>] [-Org <string>] [-OrgTitle <string>] [-Show] [<CommonParameters>]

### [New-ImageThumbnail](New-ImageThumbnail.md)
New-ImageThumbnail [-DirectoryPath] <string> [-OutputDirectory] <string> [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [-Sampler <Sampler>] [<CommonParameters>]

### [Remove-ImageExif](Remove-ImageExif.md)
Remove-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] -ExifTag <ExifTag[]> [<CommonParameters>]

Remove-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] -All [<CommonParameters>]

### [Resize-Image](Resize-Image.md)
Resize-Image [-FilePath] <string> [-OutputPath] <string> [-Width <int>] [-Height <int>] [-DontRespectAspectRatio] [-Async] [<CommonParameters>]

Resize-Image [-FilePath] <string> [-OutputPath] <string> [-Percentage <int>] [-Async] [<CommonParameters>]

### [Save-Image](Save-Image.md)
Save-Image [-Image] <Image> [[-FilePath] <string>] [-Quality <int>] [-CompressionLevel <int>] [-AsStream] [-Open] [<CommonParameters>]

### [Set-ImageExif](Set-ImageExif.md)
Set-ImageExif [-FilePath] <string> [[-FilePathOutput] <string>] [-ExifTag] <ExifTag> [-Value] <Object> [<CommonParameters>]

