# FSharpCoreOptim

BenchmarkDotNet setup for local changes to FSharp.Core.

A fork of https://github.com/abelbraaksma/StringLength-Optimization.

Assumes that this repo is in the same folder as https://github.com/dotnet/fsharp, e.g.:

```
\dev\
    \fsharp
    \fsharp-benchmarks
```

Workflow:

* Build FSharp.Core project after changes
* Run `dotnet run` from `\fsharp-benchmarks\FSharpCoreOptim`

(Not ideal, but works pretty well)
