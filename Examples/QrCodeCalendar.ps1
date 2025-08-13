Import-Module -Name $PSScriptRoot\..\ImagePlayground.psd1 -Force

$from = Get-Date
$to = $from.AddHours(1)
New-ImageQRCodeCalendar -Entry 'Meeting' -Message 'Discuss project' -Location 'Office' -From $from -To $to -FilePath "$PSScriptRoot\Samples\QRCodeCalendar.png"
