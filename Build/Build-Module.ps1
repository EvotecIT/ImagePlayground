param(
    [ValidateSet('Manifest', 'Build', 'Publish')]
    [string] $ConfigurationGateMode = 'Build',

    [string] $PowerShellGalleryApiKeyPath = 'C:\Support\Important\PowerShellGalleryAPI.txt',

    [string] $GitHubApiKeyPath = 'C:\Support\Important\GitHubAPI.txt'
)

$psPublishModulePath = $Env:PSPUBLISHMODULE_PATH
if ($psPublishModulePath) {
    Import-Module -Name $psPublishModulePath -Force -ErrorAction Stop
} else {
    Import-Module PSPublishModule -Force -ErrorAction Stop
}

$projectRoot = Split-Path -Parent $PSScriptRoot
$powerShellProjectPath = Join-Path -Path $projectRoot -ChildPath 'Sources\ImagePlayground.PowerShell\ImagePlayground.PowerShell.csproj'

Build-Module -ModuleName 'ImagePlayground' -CsprojPath $powerShellProjectPath {
    # Usual defaults as per standard module
    $Manifest = [ordered] @{
        # Minimum version of the Windows PowerShell engine required by this module
        PowerShellVersion      = '5.1'
        # prevent using over CORE/PS 7
        CompatiblePSEditions   = @('Desktop', 'Core')
        # ID used to uniquely identify this module
        GUID                   = 'ff5469f2-c542-4318-909e-fd054d16821f'
        # Version number of this module.
        ModuleVersion          = '2.0.0'
        # Author of this module
        Author                 = 'Przemyslaw Klys'
        # Company or vendor of this module
        CompanyName            = 'Evotec'
        # Copyright statement for this module
        Copyright              = "(c) 2011 - $((Get-Date).Year) Przemyslaw Klys @ Evotec. All rights reserved."
        # Description of the functionality provided by this module
        Description            = 'ImagePlayground is a PowerShell module that provides a set of functions for image processing. Among other things it can create QRCodes, BarCodes, Charts, and do image processing that can help with daily tasks.'
        # Tags applied to this module. These help with module discovery in online galleries.
        Tags                   = @('windows', 'image', 'charts', 'qrcodes', 'barcodes')
        # A URL to the main website for this project.
        ProjectUri             = 'https://github.com/EvotecIT/ImagePlayground'

        IconUri                = 'https://evotec.xyz/wp-content/uploads/2022/07/ImagePlayground.png'

        LicenseUri             = 'https://github.com/EvotecIT/ImagePlayground/blob/master/License'

        DotNetFrameworkVersion = '4.7.2'
    }
    New-ConfigurationManifest @Manifest
    # Add external module dependencies, using loop for simplicity
    #New-ConfigurationModule -Type ExternalModule -Name 'Microsoft.PowerShell.Management', 'Microsoft.PowerShell.Utility'

    # Add approved modules, that can be used as a dependency, but only when specific function from those modules is used
    # And on that time only that function and dependant functions will be copied over
    # Keep in mind it has it's limits when "copying" functions such as it should not depend on DLLs or other external files
    #New-ConfigurationModule -Type ApprovedModule -Name 'PSSharedGoods', 'PSWriteColor', 'Connectimo', 'PSUnifi', 'PSWebToolbox', 'PSMyPassword'

    $ConfigurationFormat = [ordered] @{
        RemoveComments                              = $false

        PlaceOpenBraceEnable                        = $true
        PlaceOpenBraceOnSameLine                    = $true
        PlaceOpenBraceNewLineAfter                  = $true
        PlaceOpenBraceIgnoreOneLineBlock            = $true

        PlaceCloseBraceEnable                       = $true
        PlaceCloseBraceNewLineAfter                 = $false
        PlaceCloseBraceIgnoreOneLineBlock           = $false
        PlaceCloseBraceNoEmptyLineBefore            = $true

        UseConsistentIndentationEnable              = $true
        UseConsistentIndentationKind                = 'space'
        UseConsistentIndentationPipelineIndentation = 'IncreaseIndentationAfterEveryPipeline'
        UseConsistentIndentationIndentationSize     = 4

        UseConsistentWhitespaceEnable               = $true
        UseConsistentWhitespaceCheckInnerBrace      = $true
        UseConsistentWhitespaceCheckOpenBrace       = $true
        UseConsistentWhitespaceCheckOpenParen       = $true
        UseConsistentWhitespaceCheckOperator        = $true
        UseConsistentWhitespaceCheckPipe            = $true
        UseConsistentWhitespaceCheckSeparator       = $true

        AlignAssignmentStatementEnable              = $true
        AlignAssignmentStatementCheckHashtable      = $true

        UseCorrectCasingEnable                      = $true
    }
    # format PSD1 and PSM1 files when merging into a single file
    # enable formatting is not required as Configuration is provided
    New-ConfigurationFormat -ApplyTo 'OnMergePSM1', 'OnMergePSD1' -Sort None @ConfigurationFormat
    # format PSD1 and PSM1 files within the module
    # enable formatting is required to make sure that formatting is applied (with default settings)
    New-ConfigurationFormat -ApplyTo 'DefaultPSD1', 'DefaultPSM1' -Sort None @ConfigurationFormat
    # when creating PSD1 use special style without comments and with only required parameters
    New-ConfigurationFormat -ApplyTo 'DefaultPSD1', 'OnMergePSD1' -PSD1Style 'Minimal'

    # configuration for documentation, at the same time it enables documentation processing
    New-ConfigurationDocumentation -Enable:$true -SkipExternalHelp -SkipFallbackExamples -PathReadme 'Docs\Readme.md' -Path 'Docs'

    New-ConfigurationImportModule -ImportSelf #-ImportRequiredModules

    $newConfigurationBuildSplat = @{
        Enable                            = $true
        # lets sign module only on my machine for now
        SignModule                        = if ($Env:COMPUTERNAME -eq 'EVOMONSTER') { $true } else { $false }
        MergeModuleOnBuild                = $true
        MergeFunctionsFromApprovedModules = $true
        CertificateThumbprint             = '483292C9E317AA13B07BB7A96AE9D1A5ED9E7703'
        #DotSourceLibraries                = $false
        #DotSourceClasses                  = $false
        #SeparateFileLibraries             = $true
        #DeleteTargetModuleBeforeBuild     = $true

        ResolveBinaryConflicts            = $true
        ResolveBinaryConflictsName        = 'ImagePlayground.PowerShell'
        NETProjectName                    = 'ImagePlayground.PowerShell'
        NETConfiguration                  = 'Release'
        NETFramework                      = 'net8.0', 'net472'
        NETAssemblyLoadContext            = $true
        NETAssemblyTypeAcceleratorMode    = 'AllowList'
        NETAssemblyTypeAccelerators       = @(
            'ChartForgeX.Core.Chart'
            'ChartForgeX.Primitives.ChartColor'
            'ChartForgeX.Primitives.ChartPoint'
            'ChartForgeX.Topology.TopologyCanvasSurfaceStyle'
            'ChartForgeX.Topology.TopologyChart'
            'ChartForgeX.Topology.TopologyDirection'
            'ChartForgeX.Topology.TopologyEdge'
            'ChartForgeX.Topology.TopologyEdgeEmphasis'
            'ChartForgeX.Topology.TopologyEdgeKind'
            'ChartForgeX.Topology.TopologyEdgeLineStyle'
            'ChartForgeX.Topology.TopologyEdgePort'
            'ChartForgeX.Topology.TopologyEdgeRouting'
            'ChartForgeX.Topology.TopologyGroup'
            'ChartForgeX.Topology.TopologyGroupLayoutPolicy'
            'ChartForgeX.Topology.TopologyHealthStatus'
            'ChartForgeX.Topology.TopologyLayoutDirection'
            'ChartForgeX.Topology.TopologyLayoutMode'
            'ChartForgeX.Topology.TopologyNode'
            'ChartForgeX.Topology.TopologyNodeDisplayMode'
            'ChartForgeX.Topology.TopologyNodeKind'
            'ChartForgeX.Topology.TopologyVisualStyle'
            'CodeGlyphX.BarcodeType'
            'CodeGlyphX.OtpAlgorithm'
            'CodeGlyphX.OtpAuthType'
            'CodeGlyphX.Payloads.QrBezahlAuthorityType'
            'CodeGlyphX.Payloads.QrBezahlPeriodicUnit'
            'CodeGlyphX.Payloads.QrBitcoinLikeType'
            'CodeGlyphX.Payloads.QrCalendarEncoding'
            'CodeGlyphX.Payloads.QrContactAddressOrder'
            'CodeGlyphX.Payloads.QrContactAddressType'
            'CodeGlyphX.Payloads.QrContactOutputType'
            'CodeGlyphX.Payloads.QrGeolocationEncoding'
            'CodeGlyphX.Payloads.QrGirocodeEncoding'
            'CodeGlyphX.Payloads.QrGirocodeRemittanceType'
            'CodeGlyphX.Payloads.QrGirocodeVersion'
            'CodeGlyphX.Payloads.QrMailEncoding'
            'CodeGlyphX.Payloads.QrMmsEncoding'
            'CodeGlyphX.Payloads.QrShadowSocksMethod'
            'CodeGlyphX.Payloads.QrSmsEncoding'
            'CodeGlyphX.Payloads.SlovenianUpnQrPayload'
            'CodeGlyphX.Payloads.SwissQrCodePayload'
            'CodeGlyphX.QrErrorCorrectionLevel'
            'CodeGlyphX.QrTextEncoding'
            'CodeGlyphX.SwissQrAddressType'
            'CodeGlyphX.SwissQrCurrency'
            'CodeGlyphX.SwissQrIbanType'
            'CodeGlyphX.SwissQrReferenceType'
            'ImagePlayground.ChartBarOptions'
            'ImagePlayground.ChartHeatmapColorScale'
            'ImagePlayground.ChartLegendPosition'
            'ImagePlayground.ChartMarkerShape'
            'ImagePlayground.ChartPictorialSymbol'
            'ImagePlayground.ChartPieLabelContent'
            'ImagePlayground.ChartRenderOptions'
            'ImagePlayground.ChartTheme'
            'ImagePlayground.Image'
            'ImagePlayground.ImagePlacement'
            'ImagePlayground.Sampler'
            'ImagePlayground.Status'
            'ImagePlayground.WatermarkPlacement'
            'SixLabors.Fonts.HorizontalAlignment'
            'SixLabors.Fonts.VerticalAlignment'
            'SixLabors.ImageSharp.Color'
            'SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag'
            'SixLabors.ImageSharp.PointF'
            'SixLabors.ImageSharp.Processing.FlipMode'
            'SixLabors.ImageSharp.Processing.RotateMode'
            'SixLabors.ImageSharp.Rational'
            'SixLabors.ImageSharp.Rectangle'
        )
        NETBinaryModuleDocumentation      = $true
        #NETExcludeMainLibrary             = $true
        NETExcludeLibraryFilter           = @(
            #'System.Management.*.dll'
        )
        DotSourceLibraries                = $true
        DotSourceClasses                  = $true
        #SeparateFileLibraries             = $true
        DeleteTargetModuleBeforeBuild     = $true
        MergeLibraryDebugging             = $false
        RefreshPSD1Only                   = $false
        NETHandleRuntimes                 = $true
    }

    New-ConfigurationBuild @newConfigurationBuildSplat #-DotSourceLibraries -DotSourceClasses -MergeModuleOnBuild -Enable -SignModule -DeleteTargetModuleBeforeBuild -CertificateThumbprint '483292C9E317AA13B07BB7A96AE9D1A5ED9E7703' -MergeFunctionsFromApprovedModules

    New-ConfigurationProjectBuild -Name 'ImagePlayground' -ConfigPath 'Build\project.build.json' -Enabled:$false -BuildBeforeModule -UseAsReleaseVersionSource -ProvideLocalNuGetFeed -PublishNuget -PublishGitHub
    New-ConfigurationRelease -StageRoot 'Artefacts\UploadReady' -VersionSource ProjectBuild -PrimaryProject 'ImagePlayground' -BuildOrder 'Packages', 'Module' -PublishOrder 'NuGet', 'PowerShellGallery', 'GitHub'

    $newConfigurationArtefactSplat = @{
        Type                = 'Unpacked'
        Enable              = $true
        Path                = "$PSScriptRoot\..\Artefacts\Unpacked"
        ModulesPath         = "$PSScriptRoot\..\Artefacts\Unpacked\Modules"
        RequiredModulesPath = "$PSScriptRoot\..\Artefacts\Unpacked\Modules"
        AddRequiredModules  = $true
        CopyFiles           = @{
            #"Examples\PublishingExample\Example-ExchangeEssentials.ps1" = "RunMe.ps1"
        }
    }
    New-ConfigurationArtefact @newConfigurationArtefactSplat -CopyFilesRelative
    $newConfigurationArtefactSplat = @{
        Type                = 'Packed'
        Enable              = $true
        Path                = "$PSScriptRoot\..\Artefacts\Packed"
        ModulesPath         = "$PSScriptRoot\..\Artefacts\Packed\Modules"
        RequiredModulesPath = "$PSScriptRoot\..\Artefacts\Packed\Modules"
        AddRequiredModules  = $true
        CopyFiles           = @{
            #"Examples\PublishingExample\Example-ExchangeEssentials.ps1" = "RunMe.ps1"
        }
        ArtefactName        = '<ModuleName>.v<ModuleVersion>.zip'
    }
    New-ConfigurationArtefact @newConfigurationArtefactSplat

    #New-ConfigurationTest -TestsPath "$PSScriptRoot\..\Tests" -Enable

    # global options for publishing to github/psgallery
    New-ConfigurationPublish -Type PowerShellGallery -FilePath $PowerShellGalleryApiKeyPath -Enabled:$false -UseAsDependencyVersionSource
    New-ConfigurationPublish -Type GitHub -FilePath $GitHubApiKeyPath -UserName 'EvotecIT' -Enabled:$false -RepositoryName 'ImagePlayground' -OverwriteTagName 'ImagePlayground-PowerShellModule.<TagModuleVersionWithPreRelease>'

    New-ConfigurationGate -Mode $ConfigurationGateMode
} -ExitCode
