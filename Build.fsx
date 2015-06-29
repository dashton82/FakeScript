#r @"packages/FAKE.3.35.4/tools/FakeLib.dll"

open Fake

RestorePackages()

let projectName = "MyProject"
let folderPrecompiled = @"\..\"+ projectName + ".Release_precompiled "
let publishingProfile = "PublishForViewCompile"
let nUnitToolPath = currentDirectory @@ @"packages\NUnit.Runners.2.6.4\tools\"

let publishDirectory = getBuildParamOrDefault "publishDirectory"  @"D:\CompiledSource" @@ projectName
let testDirectory = getBuildParamOrDefault "buildMode" "Debug"
let myBuildConfig = if testDirectory = "Release" then MSBuildRelease else MSBuildDebug


Target "Clean Publish Directory" (fun _ ->
    trace "Clean Publish Directory"

    if FileHelper.TestDir(publishDirectory) then
        FileHelper.CleanDir(publishDirectory)
    else
        FileHelper.CreateDir(publishDirectory)

    
    let directoryinfo = FileSystemHelper.directoryInfo(EnvironmentHelper.combinePaths publishDirectory folderPrecompiled)
    let directory = directoryinfo.FullName
    trace directory
    if FileHelper.TestDir(directory) then
        FileHelper.CleanDir(directory)
)

Target "Build Solution"(fun _ ->
    
    let buildMode = getBuildParamOrDefault "buildMode" "Debug"

    let properties = 
                    [
                        ("DebugSymbols", "False");
                        ("Configuration", buildMode);
                        ("PublishProfile", @".\" + publishingProfile + ".pubxml");
                        ("PublishUrl",publishDirectory);
                        ("DeployOnBuild","True");
                        ("ToolsVersion","12");
                    ]

    !! (@"./" + projectName + ".sln")
        |> MSBuildReleaseExt null properties "Build"
        |> Log "Build-Output: "
)

Target "Cleaning Unit Tests" (fun _ ->

    trace "Cleaning Unit Tests"
    !! (".\**\*.UnitTests.csproj")
      |> myBuildConfig "" "Clean"
      |> Log "AppBuild-Output: "

)

Target "Building Unit Tests" (fun _ ->

    trace "Building Unit Tests"
    !! (".\**\*.UnitTests.csproj")
      |> myBuildConfig "" "Rebuild"
      |> Log "AppBuild-Output: "

)

Target "Run NUnit Tests" (fun _ ->


    trace "Run NUnit Tests"
    let testDlls = !! ("./**/bin/" + testDirectory + "/*.UnitTests.dll") 
    
    
    for testDll in testDlls do
        [testDll] |> NUnit (fun p ->
            {p with
                DisableShadowCopy = true;
                ToolPath = nUnitToolPath;
                Framework = "net-4.0";
                OutputFile = fileInfo(testDll).DirectoryName @@ "TestResult.xml"
                })
)

Target "Cleaning Integration Tests" (fun _ ->

    trace "Cleaning Integration Tests"
    !! (".\**\*.IntegrationTests.csproj")
      |> myBuildConfig "" "Clean"
      |> Log "AppBuild-Output: "

)

Target "Building Integration Tests" (fun _ ->

    trace "Building Integration Tests"
    !! (".\**\*.IntegrationTests.csproj")
      |> myBuildConfig "" "Rebuild"
      |> Log "AppBuild-Output: "

)

Target "Run Integration Tests" (fun _ ->


    trace "Run Integration Tests"
    let testDlls = !! ("./**/bin/" + testDirectory + "/*.IntegrationTests.dll") 
    
    
    for testDll in testDlls do
        [testDll] |> NUnit (fun p ->
            {p with
                DisableShadowCopy = true;
                ToolPath = nUnitToolPath;
                IncludeCategory = "Integration";
                Framework = "net-4.5";
                OutputFile = fileInfo(testDll).DirectoryName @@ "TestResult.xml"
                })
)

Target "Compile Views" (fun _ ->
    trace "Compiling views"
    
    let directoryinfo = FileSystemHelper.directoryInfo(EnvironmentHelper.combinePaths publishDirectory (@"\..\" + folderPrecompiled))
    let directory = directoryinfo.FullName
    trace directory

    let result =
        ExecProcess (fun info ->
            info.FileName <- ("C:/Windows/Microsoft.NET/Framework/v4.0.30319/aspnet_compiler.exe")
            info.Arguments <- @"-v \" + folderPrecompiled + " -p . " + directory
            info.WorkingDirectory <- publishDirectory
        ) (System.TimeSpan.FromMinutes 10.)
        
    if result <> 0 then failwith "Failed to compile views"
)


Target "Clean Project" (fun _ ->

    trace "Clean Project"
    trace (@".\" + projectName + "\*.csproj")
    !! (@".\" + projectName + "\*.csproj")
      |> myBuildConfig "" "Clean"
      |> Log "AppBuild-Output: "

)

Target "Build Project" (fun _ ->

    trace "Building Project"
    trace (@".\" + projectName + "\*.csproj")
    !! (@".\" + projectName + "\*.csproj")
      |> myBuildConfig "" "Rebuild"
      |> Log "AppBuild-Output: "

)

Target "Zip Compiled Source" (fun _ ->

    trace "Zip Compiled Source"

    let directoryinfo = FileSystemHelper.directoryInfo(EnvironmentHelper.combinePaths publishDirectory @"\..\" @@ folderPrecompiled)
    let directory = directoryinfo.FullName


    !! (directory + "/**/*.*") 
        -- "*.zip"
        |> Zip directory (publishDirectory @@  (sprintf  @"\..\%s.%s.zip" projectName buildVersion))

)

"Cleaning Integration Tests"
    ==>"Building Integration Tests"
    ==>"Run Integration Tests"


"Clean Publish Directory"
   ==>"Build Solution"
   ==>"Cleaning Unit Tests"
   ==>"Building Unit Tests"
   ==>"Run NUnit Tests"
   ==>"Compile Views"
   ==>"Zip Compiled Source"


RunTargetOrDefault  "Zip Compiled Source"