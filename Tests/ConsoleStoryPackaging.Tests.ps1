Describe 'Image console story packaging contracts' {
    It 'keeps checked-in external help complete for the story surface' {
        $helpPath = Join-Path -Path $PSScriptRoot -ChildPath '..\en-US\ImagePlayground-help.xml'
        [xml] $help = Get-Content -Path $helpPath -Raw
        $commands = @(
            'Export-ImageConsoleStory'
            'New-ImageConsoleStory'
            'New-ImageConsoleStoryBlankLine'
            'New-ImageConsoleStoryCommand'
            'New-ImageConsoleStoryOutput'
            'New-ImageConsoleStoryPalette'
            'New-ImageConsoleStoryPause'
            'New-ImageConsoleStoryTab'
            'New-ImageConsoleStoryTable'
            'Select-ImageConsoleStoryTab'
            'New-ImageVisualMotionCue'
            'New-ImageVisualStory'
        )

        foreach ($commandName in $commands) {
            $command = $help.SelectSingleNode("//*[local-name()='command'][*[local-name()='details']/*[local-name()='name' and text()='$commandName']]")
            $command | Should -Not -BeNullOrEmpty
        }

        $consoleStory = $help.SelectSingleNode("//*[local-name()='command'][*[local-name()='details']/*[local-name()='name' and text()='New-ImageConsoleStory']]")
        $parameterNames = @($consoleStory.SelectNodes(".//*[local-name()='parameter']/*[local-name()='name']") | ForEach-Object InnerText)
        $parameterNames | Should -Contain 'Content'
        $parameterNames | Should -Contain 'Step'
        $parameterNames | Should -Contain 'Palette'
        $parameterNames | Should -Contain 'Speed'
        $parameterNames | Should -Contain 'TabHoldSeconds'
        $parameterNames | Should -Contain 'TypingSpeed'

        $exampleText = @($consoleStory.SelectNodes(".//*[local-name()='example']") | ForEach-Object InnerText) -join "`n"
        $exampleText | Should -Match 'WindowsTerminal'
        $exampleText | Should -Match 'New-ImageConsoleStoryTab'
        $exampleText | Should -Match 'Select-ImageConsoleStoryTab'
        $exampleText | Should -Match 'Export-ImageConsoleStory'

        $tab = $help.SelectSingleNode("//*[local-name()='command'][*[local-name()='details']/*[local-name()='name' and text()='New-ImageConsoleStoryTab']]")
        $tabParameterNames = @($tab.SelectNodes(".//*[local-name()='parameter']/*[local-name()='name']") | ForEach-Object InnerText)
        $tabParameterNames | Should -Contain 'Active'
        $tabParameterNames | Should -Contain 'Background'

        $output = $help.SelectSingleNode("//*[local-name()='command'][*[local-name()='details']/*[local-name()='name' and text()='New-ImageConsoleStoryOutput']]")
        $outputParameterNames = @($output.SelectNodes(".//*[local-name()='parameter']/*[local-name()='name']") | ForEach-Object InnerText)
        $outputParameterNames | Should -Contain 'Style'
    }

    It 'keeps fractional story defaults culture-invariant in external help' {
        $helpPath = Join-Path -Path $PSScriptRoot -ChildPath '..\en-US\ImagePlayground-help.xml'
        [xml] $help = Get-Content -Path $helpPath -Raw
        $expectedDefaults = @(
            @{ Command = 'Export-ImageConsoleStory'; Parameter = 'EndHoldSeconds'; Value = '1.2' }
            @{ Command = 'New-ImageConsoleStory'; Parameter = 'InitialDelaySeconds'; Value = '0.35' }
            @{ Command = 'New-ImageConsoleStory'; Parameter = 'LineDelaySeconds'; Value = '0.08' }
            @{ Command = 'New-ImageConsoleStory'; Parameter = 'EndHoldSeconds'; Value = '1.2' }
            @{ Command = 'New-ImageConsoleStoryTab'; Parameter = 'TransitionSeconds'; Value = '0.2' }
            @{ Command = 'Select-ImageConsoleStoryTab'; Parameter = 'TransitionSeconds'; Value = '0.2' }
            @{ Command = 'New-ImageStoryScene'; Parameter = 'DurationSeconds'; Value = '2.5' }
            @{ Command = 'New-ImageStory'; Parameter = 'EndHoldSeconds'; Value = '1.5' }
            @{ Command = 'New-ImageStory'; Parameter = 'TransitionSeconds'; Value = '0.24' }
            @{ Command = 'New-ImageVisualMotionCue'; Parameter = 'DurationSeconds'; Value = '0.7' }
        )

        foreach ($expected in $expectedDefaults) {
            $command = $help.SelectSingleNode("//*[local-name()='command'][*[local-name()='details']/*[local-name()='name' and text()='$($expected.Command)']]")
            $parameters = @($command.SelectNodes(".//*[local-name()='parameter'][*[local-name()='name' and text()='$($expected.Parameter)']]"))
            $parameters.Count | Should -BeGreaterThan 0
            foreach ($parameter in $parameters) {
                $parameter.SelectSingleNode("./*[local-name()='defaultValue']").InnerText | Should -Be $expected.Value
            }
        }
    }

    It 'declares the story step type accelerator for packaged imports' {
        $buildScript = Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath '..\Build\Build-Module.ps1') -Raw

        $buildScript | Should -Match ([regex]::Escape("'ImagePlayground.PowerShell.ImageConsoleStoryStep'"))
    }
}
