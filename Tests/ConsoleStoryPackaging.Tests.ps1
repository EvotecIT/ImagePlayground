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

    It 'declares the story step type accelerator for packaged imports' {
        $buildScript = Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath '..\Build\Build-Module.ps1') -Raw

        $buildScript | Should -Match ([regex]::Escape("'ImagePlayground.PowerShell.ImageConsoleStoryStep'"))
    }
}
