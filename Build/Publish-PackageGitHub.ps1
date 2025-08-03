Import-Module PSPublishModule -Force -ErrorAction Stop

$GitHubAccessToken = Get-Content -Raw 'C:\Support\Important\GithubAPI.txt'

$publishGitHubReleaseAssetSplat = @{
    ProjectPath          = "$PSScriptRoot\..\Sources\ImagePlayground"
    GitHubAccessToken    = $GitHubAccessToken
    GitHubUsername       = "EvotecIT"
    GitHubRepositoryName = "ImagePlayground"
    IsPreRelease         = $false
}

Publish-GitHubReleaseAsset @publishGitHubReleaseAssetSplat

$publishGitHubReleaseAssetSplat.ProjectPath = "$PSScriptRoot\..\Sources\ImagePlayground.BarCode"
Publish-GitHubReleaseAsset @publishGitHubReleaseAssetSplat

$publishGitHubReleaseAssetSplat.ProjectPath = "$PSScriptRoot\..\Sources\ImagePlayground.Chart"
Publish-GitHubReleaseAsset @publishGitHubReleaseAssetSplat

$publishGitHubReleaseAssetSplat.ProjectPath = "$PSScriptRoot\..\Sources\ImagePlayground.QRCode"
Publish-GitHubReleaseAsset @publishGitHubReleaseAssetSplat
