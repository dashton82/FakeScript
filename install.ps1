param($installPath, $toolsPath, $package)

#Forces the Batch and PS scripts for executing the build up to the root solution folder.

New-Item -ItemType Directory -Force -Path "$installPath\..\..\Tools\Nuget"

Copy-Item "$installPath\tools\temp\*.*" -destination "$installPath\..\..\" -recurse
Copy-Item "$installPath\..\**\Tools\NuGet.exe" -destination "$installPath\..\..\Tools\Nuget" -recurse

Remove-Item "$installPath\tools\temp\" -recurse 