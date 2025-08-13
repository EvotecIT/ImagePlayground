describe 'New-ImageQRCodeCalendar cmdlet' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'creates calendar event QR code' {
        $file = Join-Path -Path $TestDir -ChildPath 'calendar.png'
        if (Test-Path -Path $file) { Remove-Item -Path $file }
        $from = Get-Date -Date '2024-01-01T12:00:00'
        $to = $from.AddHours(1)
        New-ImageQRCodeCalendar -Entry 'Meeting' -Message 'Discuss' -Location 'Office' -From $from -To $to -FilePath $file
        Test-Path -Path $file | Should -BeTrue
        (Get-ImageQRCode -FilePath $file).Message | Should -Match 'BEGIN:VEVENT'
    }

    It 'throws on invalid pixel size' {
        $file = Join-Path -Path $TestDir -ChildPath 'calendar_invalid.png'
        $from = Get-Date -Date '2024-01-01T12:00:00'
        $to = $from.AddHours(1)
        { New-ImageQRCodeCalendar -Entry 'Meeting' -Message 'Discuss' -Location 'Office' -From $from -To $to -FilePath $file -PixelSize 0 } | Should -Throw
    }
}
