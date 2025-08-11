Import-Module $PSScriptRoot/../ImagePlayground.psd1 -Force

$source = Join-Path -Path $PSScriptRoot -ChildPath '../Samples/LogoEvotec.png'
$destination = Join-Path -Path $PSScriptRoot -ChildPath '../Output/logo-invalid-dimension.png'

# This call fails because width is zero
Resize-Image -FilePath $source -OutputPath $destination -Width 0 -Height 10

