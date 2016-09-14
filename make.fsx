#r "packages/FAKE/tools/FakeLib.dll"
open Fake

let testDir = "bin/Tests/"
let testTool = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe"
let runOnUnix = System.Environment.OSVersion.Platform = System.PlatformID.Unix;


// Helpers
//
let quote s = sprintf "\"%s\"" s

let pathsAsString ps =
    ps
    |> Seq.map quote
    |> (fun x -> System.String.Join(" ", x))


// Testing
//
let runTestTool fileName args =
    ExecProcess (fun info ->
        info.FileName <- fileName
        info.WorkingDirectory <- testDir
        info.Arguments <- args
    )(System.TimeSpan.FromSeconds 15.)


// Targets
//
Target "test" (fun _ ->
    // http://fsharp.github.io/FAKE/apidocs/fake-nunitcommon-nunitparams.html
    // not using built-in NUnit task since it fails to work with nunit3-console arguments format
    !! (testDir + "*.Tests.dll")
    |> fun testLibs ->
        let testLibsString = " " + pathsAsString testLibs
        let result =
            if runOnUnix then runTestTool "mono" ("../../" + testTool + testLibsString)
            else runTestTool testTool testLibsString
        if result <> 0 then failwith ("Unit test failed")
)

Target "build-tests" (fun _ ->
    !! "*/*.Tests.csproj"
    |> MSBuildDebug testDir "Build"
    |> Log "build: "
)

Target "build" (fun _ ->
    !! "jphtml/jphtml.csproj"
    |> MSBuildDebug "jphtml/bin/Debug/" "Build"
    |> Log "build: "
)

Target "info" (fun _ ->
    [   "Operating system = " + System.Environment.OSVersion.ToString();
        "Run on unix = " + runOnUnix.ToString()
    ]
    |> Log "info: "
)

// Dependencies
//
"build-tests"
    ==> "test"

RunTargetOrDefault "build"
