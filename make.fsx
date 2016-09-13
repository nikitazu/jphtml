#r "packages/FAKE/tools/FakeLib.dll"
open Fake

// Targets
//
Target "test" (fun _ ->
    trace "Testing stuff..."
)

Target "build" (fun _ ->
    !! "jphtml/jphtml.csproj"
    |> MSBuildDebug "jphtml/bin/Debug/" "Build"
    |> Log "build: "
)

// Dependencies
//
"test"
    ==> "build"

RunTargetOrDefault "build"
