Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

New-ImageVisualGrid -Title 'Service health' -Subtitle 'Generated from PowerShell objects' -Columns 2 -Theme DashboardLight -ContentDefinition {
    New-ImageVisualGridItem -TargetId requests -Block (
        New-ImageMetricCard -Label Requests -Value 12840 -Trend '+12%' -Status Positive -MiniValues 8, 9, 10, 12
    )
    New-ImageListBlock -Title Checks -Item API, Database -Status Positive, Warning
    New-ImageTableBlock -Title Services -Column Name, Status -Row @(
        @{ Name = 'API'; Status = 'Healthy' }
        @{ Name = 'Database'; Status = 'Warning' }
    ) -Dense
    New-ImageTimelineBlock -Title Activity -ItemDefinition {
        New-ImageTimelineItem -Kind Event -Title 'Build completed' -Timestamp '14:20' -Status Positive
        New-ImageTimelineItem -Kind ChecklistItem -Title 'Smoke tests' -Completed
    }
} -FilePath "$PSScriptRoot\service-health.svg"
