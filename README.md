# FakeScript

## Install

From Package Manager Console
````
Install-Package FAKEBuildScript
````
## Usuage

After installed, run __RunBuild.Bat__ from the command line. This will automatically detect the solution file in your folder. This will build all projects in the solution, by default this is done in Debug configuration.

### Command Line options
First parameter is buildType, simply pass "Debug" or "Release"

````
..RunBuild.bat "Release"
````
Second Parameter is for the Target, if running on CI, it is advisalbe to change it to
````
..RunBuild.bat "Release" "Zip Compiled Source"
````
Which will stop the creation of the two sites on the build server

### Publishing Profile
A publishing profile is required to be able to create the test version of the site. The format for this name should be [SolutionName]PublishProfile.

### Unit Tests
Unit test projects should be named "*.UnitTests" these will then automatically be picked up. A Single TestResult.xml file is then created at the same level of the solution

### Cloud Services
Cloud projects will automatically be created if the project type exists.

### Database Projects
Database projects can have the .dacpac files created, providing you have publish profile with the format Database.Publish.xml and the project name is *.Database. You also should make sure in your build configuration for the solution that this is not built as part of a Release build.

### Assemby Versioning
Assembly versioning will work on your chosen CI - this should use the format buildVersion number and follow semantic versioning. You can then override the Major and Minor values through Environment Variables.

### Zipping
A zip file will be created of the compiled website, version number will be the same as mentioned above.

### Azure Webjobs
Azure webjobs can be built and packaged providing that the project name ends with .webjob.csproj. These will then be pacakged and added to the publish directory

### DNX Support
_Coming Soon_

## Limitations

* Will only Work for one solution file
* Currently Only Support for NUnit Tests
* SpecFlow test support with NUnit
* IIS Publishing requires Local IIS to be installed - Port 5050 for compiled views, port 7070 for development version

## Contributions

Contributions welcome, please submit a pull request
