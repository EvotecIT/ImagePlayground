#Requires -Modules ImagePlayground
param(
    [string] $OutputDirectory = (Join-Path (Get-Location) 'qr-art')
)

New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
$url = 'https://example.com/menu'

# Choose a coordinated look and keep the caption outside the QR quiet zone.
New-ImageQRCode -Content $url -FilePath (Join-Path $OutputDirectory 'menu.png') `
    -Preset Sunset -Frame ScanMePill -Caption 'VIEW THE MENU' -PixelSize 16 -ErrorCorrection H

# The extension selects SVG. EyeShape and EyeBall can be set independently.
New-ImageQRCode -Content $url -FilePath (Join-Path $OutputDirectory 'vector.svg') `
    -Preset Ink -EyeShape Rounded -EyeBall Circle -Frame CornerViewfinder `
    -Caption 'SCAN TO EXPLORE' -PixelSize 16 -ErrorCorrection H

# Draw the example logo locally; no external image is required.
$logo = Join-Path $OutputDirectory 'logo.png'
New-ImageCanvas -Width 80 -Height 80 -Theme Light -Backdrop Plain `
    -BackgroundTop '#431b1b' -BackgroundBottom '#431b1b' -PngOutputScale 1 `
    -LayerDefinition {
        New-ImageCanvasText -X 26 -Y 2 -Width 50 -Text 'E' -FontSize 56 -Color White -Emphasized
    } -FilePath $logo

New-ImageQRCode -Content $url -FilePath (Join-Path $OutputDirectory 'brand.png') `
    -Preset Brand -Gradient '#431b1b', '#8c2059' -Logo $logo `
    -PixelSize 16 -ErrorCorrection H

# Inspect the generated payloads before distributing the final assets.
Get-ChildItem -LiteralPath $OutputDirectory -Filter '*.png' |
    Where-Object { $_.Name -ne 'logo.png' } |
    ForEach-Object {
        [pscustomobject] @{
            File = $_.Name
            Text = (Get-ImageQRCode -FilePath $_.FullName).Text
        }
    }
