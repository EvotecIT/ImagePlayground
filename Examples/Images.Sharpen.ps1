#Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Sharpen-Image -FilePath $PSScriptRoot\Samples\PrzemyslawKlysAndKulkozaurr.jpg -OutputPath $PSScriptRoot\Output\PrzemyslawKlysAndKulkozaurrSharpen.jpg -Sigma 5
