param($installPath, $toolsPath, $package)

#Forces the Batch and PS scripts for executing the build up to the root solution folder.

New-Item -ItemType Directory -Force -Path "$installPath\..\..\Tools\Nuget"

$Source = "$installPath\tools\temp\"
$Destination = "$installPath\..\..\"
$ExcludeItems = @()

if (Test-Path "$Destination\Build.fsx")
{
    $ExcludeItems += "Build.fsx"
}
if (Test-Path "$Destination\CustomTargets.fsx")
{
    $ExcludeItems += "CustomTargets.fsx"
}

Write-Host "Excluding:"
Write-Host $ExcludeItems

Copy-Item "$Source/*.*" -Destination $Destination -Exclude $ExcludeItems
Copy-Item "$installPath\..\**\Tools\NuGet.exe" -destination "$installPath\..\..\Tools\Nuget" -recurse

Remove-Item "$installPath\tools\temp\" -recurse 