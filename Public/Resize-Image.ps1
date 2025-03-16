function Resize-Image {
    <#
    .SYNOPSIS
    Resizes the image to given width and height or percentage.

    .DESCRIPTION
    Resizes the image to given width and height or percentage.
    By default it will respect aspect ratio. If you want to ignore it use -DontRespectAspectRatio switch.
    This means you can only provide Width and the height will be automatically calculated or vice versa.
    You can also use percentage to resize the image maintaining the aspect ratio.

    .PARAMETER FilePath
    File path to the image you want to resize

    .PARAMETER OutputPath
    File path to the output image that will be created

    .PARAMETER Width
    New width of the image

    .PARAMETER Height
    New height of the image

    .PARAMETER Percentage
    Percentage of the image to resize

    .PARAMETER DontRespectAspectRatio
    If you want to ignore aspect ratio use this switch. It only affects Width and Height parameters that are used separately.

    .EXAMPLE
    Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecResize.png -Width 100 -Height 100

    .EXAMPLE
    Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecResizeMaintainAspectRatio.png -Width 300

    .EXAMPLE
    Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecResizePercent.png -Percentage 200

    .NOTES
    General notes
    #>
    [cmdletBinding(DefaultParameterSetName = 'HeightWidth')]
    param(
        [Parameter(ParameterSetName = 'Percentage')]
        [Parameter(ParameterSetName = 'HeightWidth')]
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string] $FilePath,
        [Parameter(ParameterSetName = 'Percentage')]
        [Parameter(ParameterSetName = 'HeightWidth')]
        [Parameter(Mandatory)]
        [string] $OutputPath,
        [Parameter(ParameterSetName = 'HeightWidth')]
        [int] $Width,
        [Parameter(ParameterSetName = 'HeightWidth')]
        [int] $Height,
        [Parameter(ParameterSetName = 'Percentage')]
        [int] $Percentage,
        [Parameter(ParameterSetName = 'HeightWidth')]
        [switch] $DontRespectAspectRatio
    )
    $FilePath = $PSCmdlet.GetUnresolvedProviderPathFromPSPath($FilePath)
    $OutputPath = $PSCmdlet.GetUnresolvedProviderPathFromPSPath($OutputPath)
    if (-Not (Test-Path -LiteralPath $FilePath -PathType Leaf)) {
        Write-Warning -Message "Resize-Image - File $FilePath not found. Please check the path."
        return
    }
    if ($PSCmdlet.ParameterSetName -eq 'Percentage') {
        [ImagePlayground.ImageHelper]::Resize($FilePath, $OutputPath, $Percentage)
    }
    elseif ($PSBoundParameters.ContainsKey('Width') -or $PSBoundParameters.ContainsKey('Height')) {
        [ImagePlayground.ImageHelper]::Resize(
            $FilePath,
            $OutputPath,
            $PSBoundParameters['Width'],
            $PSBoundParameters['Height'],
            $DontRespectAspectRatio.IsPresent
        )
    }
    else {
        Write-Warning -Message 'Resize-Image - Please specify Width or Height or Percentage.'
    }
}
