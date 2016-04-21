param($installPath, $toolsPath, $package)

#Forces the Batch and PS scripts for executing the build up to the root solution folder.
Remove-Item "$installPath\..\..\RunBuild.bat" -recurse 
Remove-Item "$installPath\..\..\Build.fsx" -recurse 