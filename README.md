# String.length optimization in FSharp.Core

Just a simple BenchMark test for a small optimization of `String.length` in F#. Previously, the function always called the `System.String::Length` method twice. It's now down to one call only. This optimization has already been merged into F#, see: https://github.com/dotnet/fsharp/pull/9469.

Placing it here to check with [@adamsitnik](https://github.com/adamsitnik) in [BDN issue #1486](https://github.com/dotnet/BenchmarkDotNet/issues/1486) if it is possible to write a single benchmark in BDN that shows the timings against two different `FSharp.Core.dll`s, one for the original, and one for the locally built optimized version. If we can establish a way to do that, it'll be easier to check more complex optimizations that require changes in private functions, or involve compiler-specific syntax like inline IL with stuff like `(# "ldlen.multi 2 0" array : int #) `.

To make this work, we would somehow need to compile with/without `DisableImplicitFSharpCoreReference`.
