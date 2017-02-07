param($installPath, $toolsPath, $package)

Write-Host "Removing $installPath\..\..\RunBuild.bat"
Remove-Item "$installPath\..\..\RunBuild.bat"

Write-Host "Removing $installPath\..\..\DefaultTargets.fsx"
Remove-Item "$installPath\..\..\DefaultTargets.fsx"