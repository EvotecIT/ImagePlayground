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
}
