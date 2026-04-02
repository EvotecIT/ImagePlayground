param(
    [string] $ConfigPath = "$PSScriptRoot\project.build.json",
    [Nullable[bool]] $UpdateVersions = $false,
    [Nullable[bool]] $Build = $true,
    [Nullable[bool]] $PublishNuget = $false,
    [Nullable[bool]] $PublishGitHub = $true,
    [Nullable[bool]] $Plan,
    [string] $PlanPath
)

$buildProjectSplat = @{
    ConfigPath = $ConfigPath
}

if ($null -ne $UpdateVersions) {
    $buildProjectSplat.UpdateVersions = $UpdateVersions
}
if ($null -ne $Build) {
    $buildProjectSplat.Build = $Build
}
if ($null -ne $PublishNuget) {
    $buildProjectSplat.PublishNuget = $PublishNuget
}
if ($null -ne $PublishGitHub) {
    $buildProjectSplat.PublishGitHub = $PublishGitHub
}
if ($null -ne $Plan) {
    $buildProjectSplat.Plan = $Plan
}
if ($PlanPath) {
    $buildProjectSplat.PlanPath = $PlanPath
}

& "$PSScriptRoot\Build-Project.ps1" @buildProjectSplat
