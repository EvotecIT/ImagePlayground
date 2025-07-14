#Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$thumbParams = @{
    DirectoryPath    = "$PSScriptRoot\Samples"
    OutputDirectory  = "$PSScriptRoot\Output\Thumbs"
    Width            = 64
    Height           = 64
    Sampler          = 'Lanczos3'
}

New-ImageThumbnail @thumbParams
