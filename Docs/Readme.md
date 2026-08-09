---
Module Name: ImagePlayground
Module Guid: ff5469f2-c542-4318-909e-fd054d16821f
Download Help Link: https://github.com/EvotecIT/ImagePlayground
Help Version: 3.2.7
Locale: en-US
---
# ImagePlayground Module
## Description
Unified PowerShell commands for image processing, charts, topology diagrams, visual stories, QR codes, and barcodes.

## ImagePlayground Cmdlets
### [Add-ImageText](Add-ImageText.md)
Adds text to an image at the provided coordinates and writes the updated
image to disk.

### [Add-ImageTextBox](Add-ImageTextBox.md)
Adds wrapped text to an image within a box.

### [Add-ImageWatermark](Add-ImageWatermark.md)
Adds a watermark image to another image.

### [Clear-ImageThumbnailCache](Clear-ImageThumbnailCache.md)
Clears cached thumbnails.

### [Compare-Image](Compare-Image.md)
Compares two images and optionally saves a difference mask.

### [ConvertFrom-ImageBase64](ConvertFrom-ImageBase64.md)
Converts a Base64 encoded string into an image file.

### [ConvertTo-Image](ConvertTo-Image.md)
Converts an image to a different format.

### [ConvertTo-ImageBase64](ConvertTo-ImageBase64.md)
Converts an image file into a Base64 encoded string.

### [ConvertTo-ImageStorySource](ConvertTo-ImageStorySource.md)
Converts exact source text into renderer-neutral syntax spans for visual stories.

### [Export-ImageConsoleStory](Export-ImageConsoleStory.md)
Exports an authored ImagePlayground console story to SVG, HTML, PNG, GIF, or APNG.

### [Export-ImageMetadata](Export-ImageMetadata.md)
Exports metadata from an image.

### [Get-Image](Get-Image.md)
Loads an image from disk.

### [Get-ImageBarCode](Get-ImageBarCode.md)
Reads barcode information from an image file.

### [Get-ImageExif](Get-ImageExif.md)
Gets EXIF metadata from an image.

### [Get-ImageHeifInfo](Get-ImageHeifInfo.md)
Gets HEIF container metadata without decoding image pixels.

### [Get-ImageHeifXmp](Get-ImageHeifXmp.md)
Gets the XMP metadata packet from a HEIF or HEIC file.

### [Get-ImageMetadata](Get-ImageMetadata.md)
Gets supported metadata profiles and provenance indicators from an image.

### [Get-ImageProvenance](Get-ImageProvenance.md)
Gets embedded C2PA containers and direct XMP generative-AI declarations from an image.

### [Get-ImageQRCode](Get-ImageQRCode.md)
Reads QR code information from an image file.

### [Import-ImageMetadata](Import-ImageMetadata.md)
Imports metadata into an image.

### [Merge-Image](Merge-Image.md)
Merges two images and saves the result.

### [New-ImageAvatar](New-ImageAvatar.md)
Creates a rounded avatar image.

### [New-ImageBarCode](New-ImageBarCode.md)
Creates a barcode image.

### [New-ImageCanvas](New-ImageCanvas.md)
Creates a fixed-size visual canvas for social images, wallpapers, report covers, and announcement cards.

### [New-ImageCanvasInfoTile](New-ImageCanvasInfoTile.md)
Creates a positioned information tile for a visual canvas.

### [New-ImageCanvasText](New-ImageCanvasText.md)
Creates a positioned text layer for a visual canvas.

### [New-ImageChart](New-ImageChart.md)
Creates an image chart from definitions.

### [New-ImageChartAnnotation](New-ImageChartAnnotation.md)
Creates chart annotation data item.

### [New-ImageChartArea](New-ImageChartArea.md)
Creates area chart data item.

### [New-ImageChartBar](New-ImageChartBar.md)
Creates bar chart data item.

### [New-ImageChartBarOptions](New-ImageChartBarOptions.md)
Creates bar chart options.

### [New-ImageChartBoxPlot](New-ImageChartBoxPlot.md)
Creates box-plot chart data item.

### [New-ImageChartBubble](New-ImageChartBubble.md)
Creates bubble chart data item.

### [New-ImageChartBullet](New-ImageChartBullet.md)
Creates bullet chart data item.

### [New-ImageChartCircle](New-ImageChartCircle.md)
Creates circle status chart data item.

### [New-ImageChartDonut](New-ImageChartDonut.md)
Creates donut chart data item.

### [New-ImageChartFunnel](New-ImageChartFunnel.md)
Creates funnel chart item.

### [New-ImageChartGauge](New-ImageChartGauge.md)
Creates gauge chart data item.

### [New-ImageChartHeatmap](New-ImageChartHeatmap.md)
Creates heatmap chart data item.

### [New-ImageChartHistogram](New-ImageChartHistogram.md)
Creates histogram chart data item.

### [New-ImageChartHorizontalBar](New-ImageChartHorizontalBar.md)
Creates horizontal bar chart data item.

### [New-ImageChartLine](New-ImageChartLine.md)
Creates line chart data item.

### [New-ImageChartLollipop](New-ImageChartLollipop.md)
Creates lollipop chart data item.

### [New-ImageChartOptions](New-ImageChartOptions.md)
Creates renderer options for New-ImageChart.

### [New-ImageChartPictorial](New-ImageChartPictorial.md)
Creates pictorial chart row.

### [New-ImageChartPie](New-ImageChartPie.md)
Creates pie chart data item.

### [New-ImageChartPolar](New-ImageChartPolar.md)
Creates polar chart data item.

### [New-ImageChartProgress](New-ImageChartProgress.md)
Creates progress-bar chart row.

### [New-ImageChartRadar](New-ImageChartRadar.md)
Creates radar chart series data.

### [New-ImageChartRadial](New-ImageChartRadial.md)
Creates radial gauge chart data item.

### [New-ImageChartRangeBand](New-ImageChartRangeBand.md)
Creates range-band chart data item.

### [New-ImageChartRangeBar](New-ImageChartRangeBar.md)
Creates range-bar chart data item.

### [New-ImageChartScatter](New-ImageChartScatter.md)
Creates scatter chart data item.

### [New-ImageChartSlope](New-ImageChartSlope.md)
Creates slope chart data item.

### [New-ImageChartStackedArea](New-ImageChartStackedArea.md)
Creates stacked-area chart data item.

### [New-ImageChartStepArea](New-ImageChartStepArea.md)
Creates step-area chart data item.

### [New-ImageChartStepLine](New-ImageChartStepLine.md)
Creates step-line chart data item.

### [New-ImageChartTreemap](New-ImageChartTreemap.md)
Creates treemap chart item.

### [New-ImageChartWaterfall](New-ImageChartWaterfall.md)
Creates waterfall chart data item.

### [New-ImageChartWordCloud](New-ImageChartWordCloud.md)
Creates word cloud chart term.

### [New-ImageConsoleStory](New-ImageConsoleStory.md)
Creates a reusable script-free console story from PowerShell-native steps, captured transcript lines, or a native ChartForgeX terminal story.

### [New-ImageConsoleStoryBlankLine](New-ImageConsoleStoryBlankLine.md)
Creates a blank-line step for an ImagePlayground console story.

### [New-ImageConsoleStoryCommand](New-ImageConsoleStoryCommand.md)
Creates a typed command step for an ImagePlayground console story.

### [New-ImageConsoleStoryOutput](New-ImageConsoleStoryOutput.md)
Creates a typed output step for an ImagePlayground console story.

### [New-ImageConsoleStoryPalette](New-ImageConsoleStoryPalette.md)
Creates a reusable terminal color palette for ImagePlayground console stories and tabs.

### [New-ImageConsoleStoryPause](New-ImageConsoleStoryPause.md)
Creates a silent timeline pause for an ImagePlayground console story.

### [New-ImageConsoleStoryTab](New-ImageConsoleStoryTab.md)
Creates a persistent tab in an ImagePlayground console story.

### [New-ImageConsoleStoryTable](New-ImageConsoleStoryTable.md)
Creates a typed table step from ordinary PowerShell objects.

### [New-ImageCrop](New-ImageCrop.md)
Creates a cropped version of an image using rectangular, circular or
polygonal areas.

### [New-ImageGif](New-ImageGif.md)
Creates an animated GIF from existing images.

### [New-ImageGrid](New-ImageGrid.md)
Creates a simple grid-based image.

### [New-ImageIcon](New-ImageIcon.md)
Creates an icon file from an image.

### [New-ImageListBlock](New-ImageListBlock.md)
Creates a visual list block.

### [New-ImageMetricCard](New-ImageMetricCard.md)
Creates a dashboard metric card.

### [New-ImageMosaic](New-ImageMosaic.md)
Creates a mosaic image from multiple files.

### [New-ImageOrganizationChart](New-ImageOrganizationChart.md)
Creates an organization or team chart.

### [New-ImageOrganizationMember](New-ImageOrganizationMember.md)
Creates one member for an organization or team chart.

### [New-ImageQRCode](New-ImageQRCode.md)
Generates a QR code image from plain text content.

### [New-ImageQRCodeBezahlCode](New-ImageQRCodeBezahlCode.md)
Generates a BezahlCode QR for German banking payloads.

### [New-ImageQRCodeBitcoin](New-ImageQRCodeBitcoin.md)
Generates a QR code for Bitcoin-like payments.

### [New-ImageQRCodeCalendar](New-ImageQRCodeCalendar.md)
Creates a calendar event QR code image.

### [New-ImageQRCodeEmail](New-ImageQRCodeEmail.md)
Generates a QR code that opens an email draft.

### [New-ImageQRCodeGeoLocation](New-ImageQRCodeGeoLocation.md)
Generates a QR code with geolocation data.

### [New-ImageQRCodeGirocode](New-ImageQRCodeGirocode.md)
Generates a Girocode QR code.

### [New-ImageQRCodeMonero](New-ImageQRCodeMonero.md)
Generates a QR code for a Monero transaction.

### [New-ImageQRCodeOtp](New-ImageQRCodeOtp.md)
Generates a QR code for one-time-password configuration.

### [New-ImageQRCodePhoneNumber](New-ImageQRCodePhoneNumber.md)
Generates a QR code for dialling a phone number.

### [New-ImageQRCodeShadowSocks](New-ImageQRCodeShadowSocks.md)
Generates a QR code for a Shadowsocks configuration.

### [New-ImageQRCodeSkypeCall](New-ImageQRCodeSkypeCall.md)
Generates a QR code initiating a Skype call.

### [New-ImageQRCodeSlovenianUpnQr](New-ImageQRCodeSlovenianUpnQr.md)
Generates a Slovenian UPN QR payment code.

### [New-ImageQRCodeSms](New-ImageQRCodeSms.md)
Generates a QR code containing an SMS message.

### [New-ImageQRCodeSwiss](New-ImageQRCodeSwiss.md)
Generates a Swiss QR payment code.

### [New-ImageQRCodeWiFi](New-ImageQRCodeWiFi.md)
Creates a WiFi QR code image.

### [New-ImageQRContact](New-ImageQRContact.md)
Generates a QR code image containing the provided contact details.

### [New-ImageStory](New-ImageStory.md)
Creates a generic source-to-result visual story from resolved scenes and declared outcomes.

### [New-ImageStoryOutcome](New-ImageStoryOutcome.md)
Declares a result that must be visible in the completed visual-story scene.

### [New-ImageStoryPanel](New-ImageStoryPanel.md)
Creates one resolved source, terminal, media, or text panel for a generic visual story.

### [New-ImageStoryScene](New-ImageStoryScene.md)
Groups resolved panels into one timed visual-story scene.

### [New-ImageTableBlock](New-ImageTableBlock.md)
Creates a visual table block.

### [New-ImageThumbnail](New-ImageThumbnail.md)
Creates thumbnails for all images in a directory.

### [New-ImageTimelineBlock](New-ImageTimelineBlock.md)
Creates an activity timeline block.

### [New-ImageTimelineItem](New-ImageTimelineItem.md)
Creates one activity timeline item.

### [New-ImageTopology](New-ImageTopology.md)
Creates a topology diagram image.

### [New-ImageTopologyEdge](New-ImageTopologyEdge.md)
Creates a topology edge definition.

### [New-ImageTopologyGroup](New-ImageTopologyGroup.md)
Creates a topology group definition.

### [New-ImageTopologyMotion](New-ImageTopologyMotion.md)
Creates script-free route motion options for topology output.

### [New-ImageTopologyNode](New-ImageTopologyNode.md)
Creates a topology node definition.

### [New-ImageTopologyScenario](New-ImageTopologyScenario.md)
Creates an ordered topology scenario.

### [New-ImageTopologyScenarioStep](New-ImageTopologyScenarioStep.md)
Creates one node or edge step for a topology scenario.

### [New-ImageVisualGrid](New-ImageVisualGrid.md)
Creates a reusable dashboard grid from charts and visual blocks.

### [New-ImageVisualGridItem](New-ImageVisualGridItem.md)
Creates one chart or visual-block placement for a visual grid.

### [New-ImageVisualMotionCue](New-ImageVisualMotionCue.md)
Creates one named motion cue for an animated ChartForgeX visual story.

### [New-ImageVisualStory](New-ImageVisualStory.md)
Creates a script-free animated visual story from a ChartForgeX visual grid.

### [Remove-ImageExif](Remove-ImageExif.md)
Removes EXIF metadata from an image.

### [Remove-ImageHeifXmp](Remove-ImageHeifXmp.md)
Removes the XMP metadata packet from a HEIF or HEIC file.

### [Remove-ImageMetadata](Remove-ImageMetadata.md)
Removes selected metadata from an image.

### [Resize-Image](Resize-Image.md)
Resizes an image.

### [Save-Image](Save-Image.md)
Saves an image to disk or returns its encoded bytes as a stream.

### [Select-ImageConsoleStoryTab](Select-ImageConsoleStoryTab.md)
Switches an ImagePlayground console story to a previously declared persistent tab.

### [Set-ImageAdjust](Set-ImageAdjust.md)
Adjusts image properties.

### [Set-ImageBlur](Set-ImageBlur.md)
Blurs an image.

### [Set-ImageExif](Set-ImageExif.md)
Sets an EXIF tag value in an image.

### [Set-ImageHeifXmp](Set-ImageHeifXmp.md)
Sets the XMP metadata packet in a HEIF or HEIC file.

### [Set-ImageRotation](Set-ImageRotation.md)
Sets image rotation.

### [Set-ImageSharpen](Set-ImageSharpen.md)
Sharpens an image.
