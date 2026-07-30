Describe 'Generic visual stories' {
    BeforeAll {
        $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'collects piped panels into one ordered scene' {
        $source = New-ImageStoryPanel -Id source -Text 'Write-Output ready'
        $result = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized

        $scenes = @($source, $result) | New-ImageStoryScene -Id complete -Title Complete -Layout Split

        @($scenes).Count | Should -Be 1
        @($scenes.Panels).Count | Should -Be 2
        $scenes.Panels[0].Id | Should -Be source
        $scenes.Panels[1].Id | Should -Be result
    }

    It 'tokenizes PowerShell with the native parser without changing source text' {
        $text = '$items = Get-Process | Sort-Object CPU -Descending # hottest'
        $source = ConvertTo-ImageStorySource -Text $text -Language PowerShell

        $source.Text | Should -BeExactly $text
        $source.Language | Should -Be 'powershell'
        $source.Spans.Count | Should -BeGreaterThan 3
        @($source.Spans | ForEach-Object { $_.Kind.ToString() }) | Should -Contain 'Command'
        @($source.Spans | ForEach-Object { $_.Kind.ToString() }) | Should -Contain 'Comment'
    }

    It 'builds a source-to-result story whose completed state contains the outcome' {
        $file = Join-Path -Path $TestDir -ChildPath 'generic-story.svg'
        $source = ConvertTo-ImageStorySource -Text 'Write-Output "ready"' -Language PowerShell
        $sourcePanel = New-ImageStoryPanel -Id source -Source $source
        $resultPanel = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized
        $write = New-ImageStoryScene -Id write -Title 'Write the code' -Panels $sourcePanel
        $complete = New-ImageStoryScene -Id complete -Title 'See the result' -Layout Split -Panels $sourcePanel, $resultPanel
        $outcome = New-ImageStoryOutcome -Id ready -Label 'The ready result is visible.' -PanelId result

        $story = New-ImageStory -Title 'Source to result' -Description 'A complete generic story.' `
            -Scenes $write, $complete -Outcomes $outcome -FilePath $file -PassThru

        $story | Should -BeOfType 'ChartForgeX.Stories.VisualStory'
        Test-Path -Path $file | Should -BeTrue
        $svg = [System.IO.File]::ReadAllText($file)
        $svg | Should -Match 'data-cfx-scene="complete"'
        $svg | Should -Match 'prefers-reduced-motion:reduce'
        ([ChartForgeX.Stories.VisualStoryTranscriptRenderer]::new().Render($story)) |
            Should -Match 'The ready result is visible'
    }

    It 'rejects stories that promise an outcome absent from the completed scene' {
        $resultPanel = New-ImageStoryPanel -Id result -Text 'visible too early'
        $summaryPanel = New-ImageStoryPanel -Id summary -Text 'done'
        $early = New-ImageStoryScene -Id early -Title Early -Panels $resultPanel
        $complete = New-ImageStoryScene -Id complete -Title Complete -Panels $summaryPanel
        $outcome = New-ImageStoryOutcome -Id result -Label 'Result remains visible.' -PanelId result

        {
            New-ImageStory -Title 'Invalid promise' -Scenes $early, $complete -Outcomes $outcome `
                -FilePath (Join-Path -Path $TestDir -ChildPath 'invalid-story.png')
        } | Should -Throw
    }

    It 'requires an explicit adapter for CSharp and Bash instead of regex coloring' {
        {
            ConvertTo-ImageStorySource -Text 'Console.WriteLine("ready");' -Language CSharp
        } | Should -Throw '*tokenizer*'
    }

    It 'emits a portable bundle with a completed PNG and declared outcome' {
        $file = Join-Path -Path $TestDir -ChildPath 'bundle-primary.svg'
        $bundle = Join-Path -Path $TestDir -ChildPath 'bundle'
        if (Test-Path -Path $bundle) {
            Remove-Item -Path $bundle -Recurse -Force
        }
        $panel = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized
        $scene = New-ImageStoryScene -Id complete -Title Complete -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label 'Ready is visible.' -PanelId result

        New-ImageStory -Title 'Portable result' -Description 'Resolved output.' -Scenes $scene `
            -Outcomes $outcome -FilePath $file -BundlePath $bundle -BundleFormats Svg, Transcript

        Test-Path -Path (Join-Path -Path $bundle -ChildPath 'story.json') | Should -BeTrue
        Test-Path -Path (Join-Path -Path $bundle -ChildPath 'bundle-primary.png') | Should -BeTrue
        $manifest = Get-Content -Path (Join-Path -Path $bundle -ChildPath 'story.json') -Raw |
            ConvertFrom-Json
        $manifest.schemaVersion | Should -Be 1
        $manifest.outcome | Should -Be 'Ready is visible.'
        ($manifest.artifacts | Where-Object role -EQ completed).format | Should -Be png
        $manifest.PSObject.Properties.Name | Should -Not -Contain 'generatedAtUtc'
        $manifest.producer | Should -Match '^ImagePlayground \d+\.\d+\.\d+$'
    }

    It 'records an explicitly supplied capture time in UTC' {
        $file = Join-Path -Path $TestDir -ChildPath 'captured-primary.svg'
        $bundle = Join-Path -Path $TestDir -ChildPath 'captured-bundle'
        if (Test-Path -Path $bundle) {
            Remove-Item -Path $bundle -Recurse -Force
        }
        $panel = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized
        $scene = New-ImageStoryScene -Id complete -Title Complete -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label 'Ready is visible.' -PanelId result

        New-ImageStory -Title 'Captured result' -Scenes $scene -Outcomes $outcome -FilePath $file `
            -BundlePath $bundle -BundleFormats Transcript -CapturedAtUtc '2026-07-30T12:00:00+02:00'

        $manifest = Get-Content -Path (Join-Path -Path $bundle -ChildPath 'story.json') -Raw |
            ConvertFrom-Json
        ([DateTimeOffset] $manifest.generatedAtUtc).ToUniversalTime().ToString('yyyy-MM-ddTHH:mm:ssK') |
            Should -Be '2026-07-30T10:00:00+00:00'
    }
}
