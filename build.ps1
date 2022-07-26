$moduleName = "ImagePlayground"
$version = "0.1.0"  # TODO: Get from psd1

$sourceFolder = [System.IO.Path]::Combine($PSScriptRoot, "Sources", $moduleName)
$moduleBuildFolder = [System.IO.Path]::Combine($PSScriptRoot, "build", $moduleName, $version)
$moduleBinFolder = [System.IO.Path]::Combine($moduleBuildFolder, "bin")
if (Test-Path -LiteralPath $moduleBuildFolder) {
    Remove-Item -LiteralPath $moduleBuildFolder -Force -Recurse
}
New-Item -Path $moduleBuildFolder -ItemType Directory -Force | Out-Null

Copy-Item (Join-Path $PSScriptRoot "$ModuleName.psd1") $moduleBuildFolder
Copy-Item (Join-Path $PSScriptRoot "$ModuleName.psm1") $moduleBuildFolder
Copy-Item (Join-Path $PSScriptRoot "Public") $moduleBuildFolder -Recurse

Push-Location -Path $sourceFolder
try {
    foreach ($framework in @('net472', 'netcoreapp3.1')) {
        dotnet publish --configuration Debug --verbosity q -nologo -p:Version=$version --framework $framework
        if ($LASTEXITCODE) {
            throw "Failed to build for $framework"
        }

        $publishDirFolder = [System.IO.Path]::Combine($sourceFolder, "bin", "Debug", $framework, "publish", "*")
        $moduleBinFrameworkFolder = [System.IO.Path]::Combine($moduleBinFolder, $framework)
        New-Item -Path $moduleBinFrameworkFolder -ItemType Directory | Out-Null
        Copy-Item -Path $publishDirFolder $moduleBinFrameworkFolder -Recurse
    }
}
finally {
    Pop-Location
}


