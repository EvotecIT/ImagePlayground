Describe 'New-ImageTableBlock' {
    BeforeAll {
        $PreviousDevelopment = [Environment]::GetEnvironmentVariable('IMAGEPLAYGROUND_DEVELOPMENT', 'Process')
        $modulePath = if ($env:IMAGEPLAYGROUND_TEST_MODULE_PATH) {
            $env:IMAGEPLAYGROUND_TEST_MODULE_PATH
        } else {
            $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
            Join-Path $PSScriptRoot '../ImagePlayground.psd1'
        }
        Import-Module -Name $modulePath -Force -ErrorAction Stop
        $TestDir = Join-Path -Path $TestDrive -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
        $Services = @(
            [pscustomobject] @{ Name = 'Gateway'; Status = 'Healthy'; Requests = '48210'; P95 = '84' }
            [pscustomobject] @{ Name = 'Orders'; Status = 'Warning'; Requests = '15402'; P95 = '318' }
        )
    }

    AfterAll {
        [Environment]::SetEnvironmentVariable('IMAGEPLAYGROUND_DEVELOPMENT', $PreviousDevelopment, 'Process')
    }

    It 'builds one table from pipeline objects with columns from the first row' {
        $table = $Services | New-ImageTableBlock -Title Services

        @($table.Columns | ForEach-Object { $_.Header }) | Should -Be @('Name', 'Status', 'Requests', 'P95')
        $table.Rows.Count | Should -Be 2
        $table.Rows[1].Cells[0].Text | Should -Be 'Orders'
    }

    It 'selects, formats, aligns, and sizes columns like Format-Table' {
        $table = $Services | New-ImageTableBlock -StatusColumn Status -Width 616 -Height 270 -Property Name, Status,
            @{ Name = 'Requests'; Expression = { [int] $_.Requests }; FormatString = 'N0'; Alignment = 'Right' },
            @{ Label = 'P95'; Expression = { "$($_.P95) ms" } }

        @($table.Columns | ForEach-Object { $_.Header }) | Should -Be @('Name', 'Status', 'Requests', 'P95')
        $table.Columns[2].Alignment.ToString() | Should -Be 'Right'
        $table.Rows[0].Cells[2].Text | Should -Be '48,210'
        $table.Rows[1].Cells[3].Text | Should -Be '318 ms'
        $table.StatusColumnIndex | Should -Be 1
        $table.Options.Size.Width | Should -Be 616
        $table.Options.Size.Height | Should -Be 270
    }

    It 'accepts PSCustomObject rows through -Row' {
        $table = New-ImageTableBlock -Column Name, Status -Row ([pscustomobject] @{ Name = 'API'; Status = 'Healthy'; Extra = 1 })

        $table.Rows[0].Cells[0].Text | Should -Be 'API'
        $table.Rows[0].Cells[1].Text | Should -Be 'Healthy'
    }

    It 'keeps hashtable and array rows working' {
        $fromHash = New-ImageTableBlock -Column Name, Status -Row @{ Name = 'API'; Status = 'Healthy' }, @{ Name = 'DB'; Status = 'Warning' }
        $fromArray = New-ImageTableBlock -Column Name, Status -Row @(, @('API', 'Healthy'))

        $fromHash.Rows.Count | Should -Be 2
        $fromArray.Rows[0].Cells[1].Text | Should -Be 'Healthy'
    }

    It 'renames headers and aligns by property name' {
        $table = $Services | New-ImageTableBlock -Property Name, P95 -Header Service, 'p95 (ms)' -Align @{ P95 = 'Right' }

        @($table.Columns | ForEach-Object { $_.Header }) | Should -Be @('Service', 'p95 (ms)')
        $table.Columns[1].Alignment.ToString() | Should -Be 'Right'
    }

    It 'leaves missing properties empty' {
        $table = $Services | New-ImageTableBlock -Property Name, Owner

        $table.Rows[0].Cells[1].Text | Should -Be ''
    }

    It 'rejects an unknown status column' {
        { $Services | New-ImageTableBlock -Property Name -StatusColumn Status -ErrorAction Stop } | Should -Throw '*StatusColumn*'
    }

    It 'rejects unsupported calculated property keys' {
        { $Services | New-ImageTableBlock -Property @{ Name = 'X'; Expression = 'Name'; Colour = 'Red' } -ErrorAction Stop } | Should -Throw '*Colour*'
    }

    It 'sizes list and timeline blocks with -Width and -Height' {
        $list = New-ImageListBlock -Title Checks -Item Backups -Width 300
        $timeline = New-ImageTimelineItem -Kind Event -Title 'Build completed' | New-ImageTimelineBlock -Title Activity -Height 220

        $list.Options.Size.Width | Should -Be 300
        $list.Options.Size.Height | Should -Be 300
        $timeline.Options.Size.Height | Should -Be 220
        $timeline.Items.Count | Should -BeGreaterThan 0
    }

    It 'renders a table built from pipeline objects inside a grid' {
        $file = Join-Path -Path $TestDir -ChildPath 'table-grid.svg'
        $table = $Services | New-ImageTableBlock -Title Services -Property Name, Status -StatusColumn Status

        New-ImageVisualGrid -Columns 1 -Content $table -FilePath $file

        Get-Content -Path $file -Raw | Should -Match 'Orders'
    }
}
