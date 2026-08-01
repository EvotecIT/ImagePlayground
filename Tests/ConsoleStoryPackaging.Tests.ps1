Describe 'Image console story packaging contracts' {
    It 'keeps checked-in external help complete for the story surface' {
        $helpPath = Join-Path -Path $PSScriptRoot -ChildPath '..\en-US\ImagePlayground-help.xml'
        [xml] $help = Get-Content -Path $helpPath -Raw
        $commands = @(
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
    }

    It 'declares the story step type accelerator for packaged imports' {
        $buildScript = Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath '..\Build\Build-Module.ps1') -Raw

        $buildScript | Should -Match ([regex]::Escape("'ImagePlayground.PowerShell.ImageConsoleStoryStep'"))
    }
}
