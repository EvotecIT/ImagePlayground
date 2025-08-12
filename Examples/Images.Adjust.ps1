Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$adjustSplat = @{
    FilePath   = "$PSScriptRoot\Samples\Snow.jpeg"
    OutputPath = "$PSScriptRoot\Output\Snow_Adjusted.jpeg"
    Brightness = 1.2
    Contrast   = 1.1
}
Set-ImageAdjust @adjustSplat