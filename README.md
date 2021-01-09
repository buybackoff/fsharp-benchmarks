# FSharpCoreOptim

BenchmarkDotNet setup for local changes to FSharp.Core.

A fork of https://github.com/abelbraaksma/StringLength-Optimization.

Assumes that this repo is in the same folder as https://github.com/dotnet/fsharp, e.g.:

```
\dev\
    \fsharp
    \fsharp-benchmarks
    \FSCoreMain50        # binaries from Main branch for Before changes
    \FSCoreNuGet50       # binaries from the latest NuGet release
```

Workflow:

* Build FSharp.Core project after changes
* Run `dotnet run` from `\fsharp-benchmarks\FSharpCoreOptim` (or just run the project from IDE)

