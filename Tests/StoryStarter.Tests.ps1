Describe 'Script and captured-output story starter' {
    BeforeAll {
        if ($env:IMAGEPLAYGROUND_TEST_MODULE_PATH) {
            Import-Module $env:IMAGEPLAYGROUND_TEST_MODULE_PATH -Force -ErrorAction Stop
        } else {
            $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
            Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force -ErrorAction Stop
        }
        $starter = Join-Path $PSScriptRoot '../Examples/Story.ScriptAndOutput.ps1'
        $fast = @{ ShortSide = 480; FramesPerSecond = 2; WritingSeconds = 1; ReadingSeconds = 0.5; ReplaySeconds = 2; EndHoldSeconds = 0.5 }
    }

    It 'reads literal files without executing source and emits all three real GIF and HTML layouts' {
        $marker = Join-Path $TestDrive 'must-not-execute.txt'
        $scriptFile = Join-Path $TestDrive 'demo[1].ps1'
        $outputFile = Join-Path $TestDrive 'output[1].txt'
        [IO.File]::WriteAllText($scriptFile, "Set-Content -LiteralPath '$marker' -Value changed")
        [IO.File]::WriteAllText($outputFile, "First line`n`nLast line")
        $exports = @(& $starter -ScriptPath $scriptFile -OutputPath $outputFile -OutputDirectory (Join-Path $TestDrive 'files') @fast)
        $exports.Count | Should -Be 3
        Test-Path -LiteralPath $marker | Should -BeFalse
        $expected = @{ Square = @(480, 480); Portrait = @(480, 853); Widescreen = @(853, 480) }
        foreach ($export in $exports) {
            $gif = [IO.File]::ReadAllBytes($export.Gif)
            [Text.Encoding]::ASCII.GetString($gif, 0, 6) | Should -Be 'GIF89a'
            [BitConverter]::ToUInt16($gif, 6) | Should -Be $expected[$export.Format][0]
            [BitConverter]::ToUInt16($gif, 8) | Should -Be $expected[$export.Format][1]
            $html = [IO.File]::ReadAllText($export.Html)
            $html | Should -Match 'data-cfx-motion-duration='
            $html | Should -Match 'Replay the output'
            $export.Appearance | Should -Be 'MacOS'
            $export.ColorMode | Should -Be 'Dark'
            $transcript = [IO.File]::ReadAllText($export.Transcript)
            $transcript | Should -Match 'First line'
            $transcript | Should -Match 'Last line'
            $manifest = [IO.File]::ReadAllText($export.Manifest) | ConvertFrom-Json
            $manifest.chapters.Count | Should -Be 3
            $manifest.playback.playCount | Should -Be 0
            Test-Path -LiteralPath $export.Poster | Should -BeTrue
        }
    }

    It 'preserves a single output line and handles a capture with no output' {
        Set-StrictMode -Version Latest
        foreach ($captured in @('42', '')) {
            $suffix = if ($captured.Length -eq 0) { 'empty' } else { 'answer' }
            $export = & $starter -ScriptText '$answer = 6 * 7; $answer' -OutputText $captured -Formats Square `
                -OutputDirectory (Join-Path $TestDrive $suffix) -PlayCount 1 @fast
            $transcript = [IO.File]::ReadAllText($export.Transcript)
            if ($captured.Length -gt 0) { $transcript | Should -Match '42' }
            $manifest = [IO.File]::ReadAllText($export.Manifest) | ConvertFrom-Json
            $manifest.playback.playCount | Should -Be 1
            $manifest.playback.durationSeconds | Should -Be 4
        }
    }

    It 'selects Windows and Linux palettes and highlights the source during typing' {
        foreach ($appearance in @('Windows', 'Linux')) {
            $export = & $starter -ScriptText '$answer = 42' -OutputText '42' -Formats Square `
                -Appearance $appearance -ColorMode Light -OutputDirectory (Join-Path $TestDrive $appearance) @fast
            $html = [IO.File]::ReadAllText($export.Html)
            $export.ColorMode | Should -Be 'Light'
            $start = $html.IndexOf('<svg ')
            $end = $html.IndexOf('</svg>', $start) + 6
            [xml] $animation = $html.Substring($start, $end - $start)
            $writing = @($animation.SelectNodes("//*[local-name()='g' and @data-cfx-scene='write']"))
            $image = $writing[1].SelectSingleNode(".//*[local-name()='image']")
            $payload = $image.GetAttribute('href').Substring('data:image/svg+xml;base64,'.Length)
            [xml] $frame = [Text.Encoding]::UTF8.GetString([Convert]::FromBase64String($payload))
            $style = if ($appearance -eq 'Windows') { 'WindowsTerminal' } else { 'Linux' }
            $chrome = $frame.SelectSingleNode("//*[@data-cfx-role='story-window-chrome']")
            $chrome.GetAttribute('data-cfx-window-style') | Should -Be $style
            $runs = @($frame.SelectNodes("//*[@data-cfx-role='source-text']"))
            # At half a second the script is still being typed, with resolved variable ink.
            ($runs | ForEach-Object { $_.InnerText }) -join '' | Should -BeExactly '$answe'
            $runs[0].SelectSingleNode(".//*[@fill='#175FD4']") | Should -Not -BeNullOrEmpty
            [Text.Encoding]::ASCII.GetString([IO.File]::ReadAllBytes($export.Gif), 0, 6) | Should -Be 'GIF89a'
        }
    }

    It 'checks every requested layout before overwriting files and protects input inside the destination' {
        $destination = Join-Path $TestDrive 'keep'
        $portrait = Join-Path $destination 'portrait'
        $null = New-Item -ItemType Directory -Path $portrait
        $existing = Join-Path $portrait 'story.gif'
        [IO.File]::WriteAllText($existing, 'Existing deliverable')
        { & $starter -ScriptText '42' -OutputText '42' -OutputDirectory $destination @fast } | Should -Throw '*Export already exists*'
        [IO.File]::ReadAllText($existing) | Should -BeExactly 'Existing deliverable'
        Test-Path -LiteralPath (Join-Path $destination 'square') | Should -BeFalse
        $inputFile = Join-Path $portrait 'script.ps1'
        [IO.File]::WriteAllText($inputFile, '42')
        { & $starter -ScriptPath $inputFile -OutputPath $existing -OutputDirectory $destination -Overwrite @fast } | Should -Throw '*outside OutputDirectory*'
        [IO.File]::ReadAllText($inputFile) | Should -BeExactly '42'
        $export = & $starter -ScriptText '43' -OutputText '43' -Formats Portrait -OutputDirectory $destination -Overwrite @fast
        [IO.File]::ReadAllText($export.Transcript) | Should -Match '43'
        [Text.Encoding]::ASCII.GetString([IO.File]::ReadAllBytes($existing), 0, 6) | Should -Be 'GIF89a'
        [IO.File]::ReadAllText($inputFile) | Should -BeExactly '42'
    }

    It 'keeps a Graphite light replay light with the minimal title bar' {
        $export = & $starter -ScriptText '$x = 42' -OutputText '42' -Formats Square `
            -Appearance Graphite -ColorMode Light -OutputDirectory (Join-Path $TestDrive 'graphite-light') @fast
        $html = [IO.File]::ReadAllText($export.Html)
        $start = $html.IndexOf('<svg ')
        $end = $html.IndexOf('</svg>', $start) + 6
        [xml] $animation = $html.Substring($start, $end - $start)
        $replay = @($animation.SelectNodes("//*[local-name()='g' and @data-cfx-scene='replay']"))
        $image = $replay[-1].SelectSingleNode(".//*[local-name()='image']")
        $payload = $image.GetAttribute('href').Substring('data:image/svg+xml;base64,'.Length)
        [xml] $frame = [Text.Encoding]::UTF8.GetString([Convert]::FromBase64String($payload))
        $viewport = $frame.SelectSingleNode("//*[@data-cfx-role='terminal-viewport']")
        $viewport.GetAttribute('fill') | Should -Be '#FFFFFF'
        $rows = @($frame.SelectNodes("//*[@data-cfx-role='terminal-viewport-text']"))
        $rows[-1].InnerText | Should -Be '42'
    }

    It 'protects the <InputKind> input when the destination contains an environment variable' -ForEach @(
        @{ InputKind = 'script' }, @{ InputKind = 'output' }
    ) {
        $caseRoot = Join-Path $TestDrive "environment-$InputKind"
        $destination = Join-Path $caseRoot 'exports'
        $portrait = Join-Path $destination 'portrait'
        $null = New-Item -ItemType Directory -Path $portrait -Force
        $scriptFile = Join-Path $caseRoot 'script.ps1'
        $outputFile = Join-Path $caseRoot 'output.txt'
        [IO.File]::WriteAllText($scriptFile, '$x = 42')
        [IO.File]::WriteAllText($outputFile, '42')
        if ($InputKind -eq 'script') { $scriptFile = Join-Path $portrait 'story.html' }
        else { $outputFile = Join-Path $portrait 'story.txt' }
        $protected = if ($InputKind -eq 'script') { $scriptFile } else { $outputFile }
        $original = if ($InputKind -eq 'script') { '$x = 42' } else { '42' }
        [IO.File]::WriteAllText($protected, $original)
        $oldValue = [Environment]::GetEnvironmentVariable('IMAGESTORY_TEST_EXPORT_FOLDER')
        try {
            [Environment]::SetEnvironmentVariable('IMAGESTORY_TEST_EXPORT_FOLDER', 'exports')
            { & $starter -ScriptPath $scriptFile -OutputPath $outputFile `
                -OutputDirectory (Join-Path $caseRoot '%IMAGESTORY_TEST_EXPORT_FOLDER%') -Overwrite @fast } |
                Should -Throw '*outside OutputDirectory*'
            [IO.File]::ReadAllText($protected) | Should -BeExactly $original
            Test-Path -LiteralPath (Join-Path $destination 'square') | Should -BeFalse
        } finally {
            [Environment]::SetEnvironmentVariable('IMAGESTORY_TEST_EXPORT_FOLDER', $oldValue)
        }
    }

    It 'checks existing exports at the expanded destination and reports the actual output paths' {
        $caseRoot = Join-Path $TestDrive 'environment-existing'
        $destination = Join-Path $caseRoot 'exports'
        $portrait = Join-Path $destination 'portrait'
        $null = New-Item -ItemType Directory -Path $portrait -Force
        $existing = Join-Path $portrait 'story.html'
        [IO.File]::WriteAllText($existing, 'Existing deliverable')
        $oldValue = [Environment]::GetEnvironmentVariable('IMAGESTORY_TEST_EXPORT_FOLDER')
        try {
            [Environment]::SetEnvironmentVariable('IMAGESTORY_TEST_EXPORT_FOLDER', 'exports')
            $unresolved = Join-Path $caseRoot '%IMAGESTORY_TEST_EXPORT_FOLDER%'
            { & $starter -ScriptText '42' -OutputText '42' -OutputDirectory $unresolved @fast } |
                Should -Throw '*Export already exists*'
            [IO.File]::ReadAllText($existing) | Should -BeExactly 'Existing deliverable'
            Test-Path -LiteralPath (Join-Path $destination 'square') | Should -BeFalse
            $export = & $starter -ScriptText '42' -OutputText '42' -OutputDirectory $unresolved -Formats Portrait -Overwrite @fast
            $export.Html | Should -BeExactly $existing
            [IO.File]::ReadAllText($existing) | Should -Match 'data-cfx-motion-duration='
        } finally {
            [Environment]::SetEnvironmentVariable('IMAGESTORY_TEST_EXPORT_FOLDER', $oldValue)
        }
    }

    It 'rejects a destination whose expansion would change again during export' {
        $caseRoot = Join-Path $TestDrive 'environment-nested'
        $oldOuter = [Environment]::GetEnvironmentVariable('IMAGESTORY_TEST_OUTER_FOLDER')
        $oldInner = [Environment]::GetEnvironmentVariable('IMAGESTORY_TEST_INNER_FOLDER')
        try {
            [Environment]::SetEnvironmentVariable('IMAGESTORY_TEST_OUTER_FOLDER', '%IMAGESTORY_TEST_INNER_FOLDER%')
            [Environment]::SetEnvironmentVariable('IMAGESTORY_TEST_INNER_FOLDER', 'exports')
            { & $starter -ScriptText '42' -OutputText '42' `
                -OutputDirectory (Join-Path $caseRoot '%IMAGESTORY_TEST_OUTER_FOLDER%') @fast } |
                Should -Throw '*nested environment variables*'
            Test-Path -LiteralPath $caseRoot | Should -BeFalse
        } finally {
            [Environment]::SetEnvironmentVariable('IMAGESTORY_TEST_OUTER_FOLDER', $oldOuter)
            [Environment]::SetEnvironmentVariable('IMAGESTORY_TEST_INNER_FOLDER', $oldInner)
        }
    }

    It 'protects the <InputKind> input through a linked <Route> before exporting any layout' -ForEach @(
        @{ InputKind = 'script'; Route = 'destination' }, @{ InputKind = 'output'; Route = 'destination' },
        @{ InputKind = 'script'; Route = 'input' }, @{ InputKind = 'output'; Route = 'input' },
        @{ InputKind = 'script'; Route = 'layout' }, @{ InputKind = 'output'; Route = 'layout' }
    ) {
        $caseRoot = Join-Path $TestDrive "$InputKind-$Route"
        $physical = Join-Path $caseRoot 'physical'
        $destination = Join-Path $caseRoot 'exports'
        $null = New-Item -ItemType Directory -Path (Join-Path $physical 'portrait') -Force
        $scriptFile = Join-Path $caseRoot 'script.ps1'
        $outputFile = Join-Path $caseRoot 'output.txt'
        [IO.File]::WriteAllText($scriptFile, '$x = 42')
        [IO.File]::WriteAllText($outputFile, '42')
        $name = if ($InputKind -eq 'script') { 'story.html' } else { 'story.txt' }
        $protected = Join-Path (Join-Path $physical 'portrait') $name
        $original = if ($InputKind -eq 'script') { '$x = 42' } else { '42' }
        [IO.File]::WriteAllText($protected, $original)
        $linkKind = if ($PSVersionTable.PSVersion.Major -le 5 -or $PSVersionTable.Platform -ne 'Unix') { 'Junction' } else { 'SymbolicLink' }
        if ($Route -eq 'destination') {
            $null = New-Item -ItemType $linkKind -Path $destination -Target $physical
            $input = $protected
        } elseif ($Route -eq 'input') {
            $destination = $physical
            $alias = Join-Path $caseRoot 'input-alias'
            $null = New-Item -ItemType $linkKind -Path $alias -Target $physical
            $input = Join-Path (Join-Path $alias 'portrait') $name
        } else {
            $null = New-Item -ItemType Directory -Path $destination
            $null = New-Item -ItemType $linkKind -Path (Join-Path $destination 'portrait') -Target (Join-Path $physical 'portrait')
            $input = $protected
        }
        if ($InputKind -eq 'script') { $scriptFile = $input } else { $outputFile = $input }
        { & $starter -ScriptPath $scriptFile -OutputPath $outputFile -OutputDirectory $destination -Overwrite @fast } |
            Should -Throw '*outside OutputDirectory*'
        [IO.File]::ReadAllText($protected) | Should -BeExactly $original
        Test-Path -LiteralPath (Join-Path $destination 'square') | Should -BeFalse
    }
}
