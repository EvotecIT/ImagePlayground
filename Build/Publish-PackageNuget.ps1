Import-Module PSPublishModule -Force -ErrorAction Stop

$NugetAPI = Get-Content -Raw -LiteralPath "C:\Support\Important\NugetOrgEvotec.txt"
Publish-NugetPackage -Path "$PSScriptRoot\..\Sources\ImagePlayground.Core\bin\Release" -ApiKey $NugetAPI
Publish-NugetPackage -Path "$PSScriptRoot\..\Sources\ImagePlayground.BarCode\bin\Release" -ApiKey $NugetAPI
Publish-NugetPackage -Path "$PSScriptRoot\..\Sources\ImagePlayground.Chart\bin\Release" -ApiKey $NugetAPI
Publish-NugetPackage -Path "$PSScriptRoot\..\Sources\ImagePlayground.QRCode\bin\Release" -ApiKey $NugetAPI