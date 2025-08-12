Describe 'Add-ImageText' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'adds text to an image' {

        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $dest = Join-Path $TestDir 'text.png'

        if (Test-Path $dest) { Remove-Item $dest }

        Add-ImageText -FilePath $src -OutputPath $dest -Text 'Test' -X 1 -Y 1 -Color ([SixLabors.ImageSharp.Color]::Red)

        Test-Path $dest | Should -BeTrue

    }

    It 'accepts FilePath from pipeline' {

        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'text_pipeline.png'
        if (Test-Path $dest) { Remove-Item $dest }
        $src | Add-ImageText -OutputPath $dest -Text 'Pipe' -X 1 -Y 1 -Color ([SixLabors.ImageSharp.Color]::Red)
        Test-Path $dest | Should -BeTrue

    }

    It 'adds text with shadow and outline' {

        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'text_shadow_outline.png'

        if (Test-Path $dest) { Remove-Item $dest }

        Add-ImageText -FilePath $src -OutputPath $dest -Text 'Test' -X 1 -Y 1 -Color ([SixLabors.ImageSharp.Color]::Red) -ShadowColor ([SixLabors.ImageSharp.Color]::Black) -ShadowOffsetX 1 -ShadowOffsetY 1 -OutlineColor ([SixLabors.ImageSharp.Color]::Yellow) -OutlineWidth 1

        Test-Path $dest | Should -BeTrue

    }

    It 'adds text and textbox on same image' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'text_and_box.png'
        if (Test-Path $dest) { Remove-Item $dest }
        $img = Get-Image -FilePath $src
        $img.AddText(10,10,'Demo',[SixLabors.ImageSharp.Color]::Green,12)
        $img.AddTextBox(10,30,'Long text for wrap',80,[SixLabors.ImageSharp.Color]::Blue,12)
        Save-Image -Image $img -FilePath $dest
        $img.Dispose()
        Test-Path $dest | Should -BeTrue
    }

    It 'throws for invalid coordinates' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dest = Join-Path $TestDir 'invalid.png'
        $img = [ImagePlayground.Image]::Load($src)
        $x = $img.Width + 10
        $img.Dispose()
        { Add-ImageText -FilePath $src -OutputPath $dest -Text 'Test' -X $x -Y 0 } | Should -Throw
        { Add-ImageText -FilePath $src -OutputPath $dest -Text 'Test' -X 0 -Y -1 } | Should -Throw
    }

    It 'creates output in non-existent directory' {
        $src = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'
        $dir = Join-Path $TestDir ([guid]::NewGuid())
        $dest = Join-Path $dir 'text.png'
        Add-ImageText -FilePath $src -OutputPath $dest -Text 'Test' -X 1 -Y 1 -Color ([SixLabors.ImageSharp.Color]::Red)
        Test-Path $dest | Should -BeTrue
    }
}

