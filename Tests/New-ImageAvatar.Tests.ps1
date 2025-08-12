describe 'New-ImageAvatar' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }
    It 'creates avatar in non-existent directory' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/LogoEvotec.png'
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'avatar.png'
        New-ImageAvatar -FilePath $src -OutputPath $dest -Width 32 -Height 32
        Test-Path $dest | Should -BeTrue
    }
}
