Describe 'Image command FileSystem paths' {
    BeforeAll {
        $modulePath = if ($env:IMAGEPLAYGROUND_TEST_MODULE_PATH) {
            $env:IMAGEPLAYGROUND_TEST_MODULE_PATH
        } else {
            Join-Path $PSScriptRoot '../ImagePlayground.psd1'
        }
        Import-Module $modulePath -Force -ErrorAction Stop
        $originalProcessDirectory = [Environment]::CurrentDirectory
        $processDirectory = Join-Path $TestDrive 'process'
        $locationDirectory = Join-Path $TestDrive 'location'
        New-Item -ItemType Directory -Path $processDirectory, $locationDirectory | Out-Null
        $seed = Join-Path $locationDirectory 'source.png'
        New-ImageQRCode -Content 'image-path-source' -FilePath $seed -PixelSize 4
        ConvertFrom-ImageBase64 -Base64 'iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAYAAADED76LAAAAEklEQVR4nGO4o6v7Hx9mGBkKAM0gjUGODHAHAAAAAElFTkSuQmCC' -OutputPath (Join-Path $locationDirectory 'logo.png')
    }

    BeforeEach {
        [Environment]::CurrentDirectory = $processDirectory
        Push-Location -LiteralPath $locationDirectory
    }

    AfterEach {
        Pop-Location
        [Environment]::CurrentDirectory = $originalProcessDirectory
    }

    It 'creates and reads a QR code relative to the PowerShell location' {
        New-ImageQRCode -Content 'current-location-qr' -FilePath './relative-qr.png' -PixelSize 4
        [IO.File]::Exists((Join-Path $locationDirectory 'relative-qr.png')) | Should -BeTrue
        [IO.File]::Exists((Join-Path $processDirectory 'relative-qr.png')) | Should -BeFalse
        (Get-ImageQRCode -FilePath './relative-qr.png').Text | Should -Be 'current-location-qr'
    }

    It 'resolves QR logo inputs and specialized QR outputs from the same location' {
        New-ImageQRCode -Content 'relative-logo' -LogoPath './logo.png' -FilePath './logo-qr.png' -PixelSize 8
        (Get-ImageQRCode -FilePath './logo-qr.png').Text | Should -Be 'relative-logo'
        New-ImageQRCodeWiFi -SSID 'PathFixture' -Password 'synthetic-password' -FilePath './wifi-qr.png'
        (Get-ImageQRCode -FilePath './wifi-qr.png').Text | Should -Match 'S:PathFixture;'
    }

    It 'creates and reads barcode paths on a FileSystem PSDrive' {
        $driveName = 'ImagePaths' + [Guid]::NewGuid().ToString('N')
        New-PSDrive -Name $driveName -PSProvider FileSystem -Root $locationDirectory | Out-Null
        try {
            $path = $driveName + ':/provider-barcode.png'
            New-ImageBarCode -Type EAN -Value '9012341234571' -FilePath $path
            [IO.File]::Exists((Join-Path $locationDirectory 'provider-barcode.png')) | Should -BeTrue
            (Get-ImageBarCode -FilePath $path).Text | Should -Be '9012341234571'
        } finally {
            Remove-PSDrive -Name $driveName
        }
    }

    It 'loads and saves images using relative paths without changing process location' {
        $image = Get-Image -FilePath './source.png'
        try {
            Save-Image -Image $image -FilePath './saved.png'
            [IO.File]::Exists((Join-Path $locationDirectory 'saved.png')) | Should -BeTrue
            [IO.File]::Exists((Join-Path $processDirectory 'saved.png')) | Should -BeFalse
            (Get-ImageQRCode -FilePath './saved.png').Text | Should -Be 'image-path-source'
            [Environment]::CurrentDirectory | Should -Be $processDirectory
        } finally {
            $image.Dispose()
        }
    }

    It 'captures relative input and output paths before asynchronous resizing' {
        Resize-Image -FilePath './source.png' -OutputPath './resized.png' -Width 64 -Height 64 -DontRespectAspectRatio
        $image = Get-Image -FilePath './resized.png'
        try {
            $image.Width | Should -Be 64
            $image.Height | Should -Be 64
            [IO.File]::Exists((Join-Path $processDirectory 'resized.png')) | Should -BeFalse
        } finally {
            $image.Dispose()
        }
    }

    It 'preserves literal bracket characters in file inputs and outputs' {
        $base64 = ConvertTo-ImageBase64 -FilePath './source.png'
        ConvertFrom-ImageBase64 -Base64 $base64 -OutputPath './literal[1].png'
        (Get-ImageQRCode -FilePath './literal[1].png').Text | Should -Be 'image-path-source'
    }

    It 'preserves environment expansion for input and output paths' {
        $name = 'IMAGEPLAYGROUND_PATH_' + [Guid]::NewGuid().ToString('N')
        [Environment]::SetEnvironmentVariable($name, $locationDirectory)
        try {
            $prefix = '%' + $name + '%/'
            $base64 = ConvertTo-ImageBase64 -FilePath ($prefix + 'source.png')
            ConvertFrom-ImageBase64 -Base64 $base64 -OutputPath ($prefix + 'expanded.png')
            (Get-ImageQRCode -FilePath './expanded.png').Text | Should -Be 'image-path-source'
        } finally {
            [Environment]::SetEnvironmentVariable($name, $null)
        }
    }

    It 'rejects non-FileSystem providers for image inputs and QR or barcode outputs' {
        { Get-Image -FilePath 'Variable:/image-path-fixture' -ErrorAction Stop } | Should -Throw '*FileSystem provider*'
        { New-ImageQRCode -Content 'provider-rejection' -FilePath 'Variable:/image-path-fixture.png' -ErrorAction Stop } | Should -Throw '*FileSystem provider*'
        { New-ImageBarCode -Type EAN -Value '9012341234571' -FilePath 'Variable:/image-path-fixture.png' -ErrorAction Stop } | Should -Throw '*FileSystem provider*'
    }

    It 'retains process-directory semantics for ordinary C# path helpers' {
        $image = Get-Image -FilePath (Join-Path $locationDirectory 'source.png')
        try {
            $helpers = $image.GetType().Assembly.GetType('ImagePlayground.Helpers', $true)
            $helpers.GetMethod('ResolvePath').Invoke($null, @('core-path.png')) |
                Should -Be (Join-Path $processDirectory 'core-path.png')
        } finally {
            $image.Dispose()
        }
    }
}
