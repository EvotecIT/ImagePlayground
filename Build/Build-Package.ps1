Import-Module PSPublishModule -Force -ErrorAction Stop

Invoke-DotNetReleaseBuild -ProjectPath "$PSScriptRoot\..\Sources\ImagePlayground.Core" -CertificateThumbprint '483292C9E317AA13B07BB7A96AE9D1A5ED9E7703'
Invoke-DotNetReleaseBuild -ProjectPath "$PSScriptRoot\..\Sources\ImagePlayground.BarCode" -CertificateThumbprint '483292C9E317AA13B07BB7A96AE9D1A5ED9E7703'
Invoke-DotNetReleaseBuild -ProjectPath "$PSScriptRoot\..\Sources\ImagePlayground.Chart" -CertificateThumbprint '483292C9E317AA13B07BB7A96AE9D1A5ED9E7703'
Invoke-DotNetReleaseBuild -ProjectPath "$PSScriptRoot\..\Sources\ImagePlayground.QRCode" -CertificateThumbprint '483292C9E317AA13B07BB7A96AE9D1A5ED9E7703'