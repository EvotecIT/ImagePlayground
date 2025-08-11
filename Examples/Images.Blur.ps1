#Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Blur-Image -FilePath $PSScriptRoot\Samples\PrzemyslawKlysAndKulkozaurr.jpg -OutputPath $PSScriptRoot\Output\PrzemyslawKlysAndKulkozaurrBlur.jpg -Sigma 5
