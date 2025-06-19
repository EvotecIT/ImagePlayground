Describe 'Add-ImageTextBox' {
    BeforeAll {
        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path $PSScriptRoot 'Artifacts'
        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }
    }

    It 'adds wrapped text inside a box' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'textbox.png'
        if (Test-Path $dest) { Remove-Item $dest }
        Add-ImageTextBox -FilePath $src -OutputPath $dest -Text 'Test Wrap' -X 1 -Y 1 -Width 100 -Color ([SixLabors.ImageSharp.Color]::Red)
        Test-Path $dest | Should -BeTrue
    }
}
