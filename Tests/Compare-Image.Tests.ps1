Describe 'Compare-Image' {

    BeforeAll {

        Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force



        $TestDir = Join-Path $PSScriptRoot 'Artifacts'

        if (-not (Test-Path $TestDir)) { New-Item -Path $TestDir -ItemType Directory | Out-Null }

    }

    It 'compares two images and returns result' {

        $img1 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $img2 = Join-Path $TestDir 'qr_comp.png'

        if (Test-Path $img2) { Remove-Item $img2 }

        Add-ImageText -FilePath $img1 -OutputPath $img2 -Text 'X' -X 1 -Y 1 -Color ([SixLabors.ImageSharp.Color]::Red)

        $result = Compare-Image -FilePath $img1 -FilePathToCompare $img2

        $result.PixelErrorCount | Should -BeGreaterThan 0

    }

    It 'saves difference mask when output path provided' {

        $img1 = Join-Path $PSScriptRoot '../Sources/ImagePlayground.Tests/Images/QRCode1.png'

        $modified = Join-Path $TestDir 'qr_mod.png'

        if (Test-Path $modified) { Remove-Item $modified }

        Add-ImageText -FilePath $img1 -OutputPath $modified -Text 'Diff' -X 1 -Y 1 -Color ([SixLabors.ImageSharp.Color]::Red)

        $dest = Join-Path $TestDir 'diff.png'

        if (Test-Path $dest) { Remove-Item $dest }

        Compare-Image -FilePath $img1 -FilePathToCompare $modified -OutputPath $dest

        Test-Path $dest | Should -BeTrue

    }

}

