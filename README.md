# FakeScript

[![Build status](https://ci.appveyor.com/api/projects/status/dd41b1ebr4ctap4r/branch/master?svg=true)](https://ci.appveyor.com/project/dashton82/fakescript/branch/master)

[![NuGet Badge](https://buildstats.info/nuget/FAKEBuildScript)](https://www.nuget.org/packages/FAKEBuildScript)

## Install

From Package Manager Console
````
Install-Package FAKEBuildScript
````
## Usage

After installed, you will have in the root of your solution:

- RunBuild.bat,
- Build.fsx
- DefaultTargets.fsx
- CustomTargets.fsx
- Tools\Nuget\Nuget.exe
- Tools\Jasmine\Jasminerunner.js
- Tools\Jasmine\Jasminerunner-nunit.js
- Tools\Jasmine\phantomjs-testrunner.js

Run __RunBuild.Bat__ from the command line. This will automatically detect the solution file in your folder. This will build all projects in the solution, by default this is done in Debug configuration.


### Custom Build Steps
By default the DefaultTargets.fsx is overwritten with every update of the nuget package. The Build.fsx and CustomTargets.fsx are not and are intended for your build usages cusomisation. Within CustomTargets you can put any addtional operations that you wish to be ran, these will then be loaded within Build.fsx. Build.fsx can be modifed so that you can ammend the build like shown below:
```
"Set version number"
   ==>"Set Solution Name"
   ==>"Update Assembly Info Version Numbers"
   ==>"Clean Directories"
   ==>"Clean Projects"
   ==>"Build Projects"
   ==>"Run XUnit Tests"
```
The only ones that are core to other steps are the first two. These are required for setting working directories and determining if certain steps can be run.


### Command Line options
First parameter is buildType, simply pass "Debug" or "Release"

````
..RunBuild.bat "Release"
````
Second Parameter is for the Target
````
..RunBuild.bat "Release" "Publish Solution"
````

### Publishing Profile
A publishing profile is required to be able to create the test version of the site. The format for this name should be [SolutionName]PublishProfile, and must exist in a project named *.web.

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
Cloud projects will automatically be created if the project type exists. It is also possible to create multiple packages if within the cloud project folder you have a folder called Configuration . Within this you should place *.csdef files that differ from the default - this could include different vmsizes for example.

### Database Projects
Database projects can have the .dacpac files created, providing you have publish profile with the format Database.Publish.xml and the project name is *.Database. You also should make sure in your build configuration for the solution that this is not built as part of a Release build.

### Assembly Versioning
Assembly versioning will work on your chosen CI - this should use the format buildVersion number and follow semantic versioning. You can then override the Major and Minor values through Environment Variables, BUILD_MAJORNUMBER and BUILD_MINORNUMBER.
Alternatively, if your CI build number is already dot-formatted (Major.Minor.Patch) this will be used for assembly and nuget versioning instead.

### Zipping
A zip file can be created of the compiled website, version number will be the same as mentioned above. To do this call
```
RunBuild.bat "Release" "Zip Compiled Source"
```

### Azure Webjobs
Azure webjobs can be built and packaged providing that the project name ends with .webjob.csproj. These will then be packaged and added to the publish directory

### DNX Support
_Coming Soon_

## Limitations

* Will only Work for one solution file
* SpecFlow test support with NUnit

## Contributions

Contributions welcome, please submit a pull request
