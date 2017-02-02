# FakeScript

## Install

From Package Manager Console
````
Install-Package FAKEBuildScript
````
## Usage

After installed, run __RunBuild.Bat__ from the command line. This will automatically detect the solution file in your folder. This will build all projects in the solution, by default this is done in Debug configuration.

### Command Line options
First parameter is buildType, simply pass "Debug" or "Release"

````
..RunBuild.bat "Release"
````
Second Parameter is for the Target
````
..RunBuild.bat "Release" "Publish Solution"
````
Which will stop the creation of the two sites on the build server

### Publishing Profile
A publishing profile is required to be able to create the test version of the site. The format for this name should be [SolutionName]PublishProfile.

### Unit Tests
Unit test projects using NUnit should be named "*.UnitTests" these will then automatically be picked up. A Single TestResult.xml file is then created at the same level of the solution. For XUnitTests, projects should be named "*.XUnitTests". This will produce a separate xml result file TestResultXunit.xml

### Jasmine Tests
Jasmine tests will be ran if contained in a project name "*.JasmineTests". It will then look for the SpecRunner.html file in the route of this project. PhantomJs is downloaded as part of the bat file and a runner is used from the tools directory. It is possible to enable NUnit formatting of results file, using [larrymyers/jasminereporters](https://github.com/larrymyers/jasmine-reporters) to do this you need to add:

```
<script src="lib/jasmine/jasmine-nunitreporter.js"></script>
<script>
        jasmine.getEnv().addReporter(new jasmineReporters.NUnitXmlReporter({ savePath: '', filename :'TestResultJasmine.xml'}));
</script>
```

And then change the build script to use the phantomjs-testrunner.js as shown below
```
let jasmineRunnerPath = @"tools\jasmine\phantomjs-testrunner.js"
```
Example shown in solution

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
* SpecFlow test support with NUnit
* IIS Publishing requires Local IIS to be installed - Port 5050 for compiled views, port 7070 for development version

## Contributions

Contributions welcome, please submit a pull request
