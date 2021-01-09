// Learn more about F# at http://fsharp.org

open System
open System.Diagnostics
open System.Reflection
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Running
open FSharp.Perf
open FSharpCoreOptim
open FSharpCoreOptim.Types
open Perfolizer.Horology
open FSharp.Collections

[<EntryPoint>]
let main argv =

    let asm = typeof<FSharp.Core.EntryPointAttribute>.Assembly
    printfn "Assembly: %A" (asm.CodeBase)      
    
    let arg = new MsBuildArgument("/p:Optimize=true");
    let baseJob = Job.Default
                    .WithWarmupCount(1) // 1 warmup is enough for our purpose
                    .WithIterationTime(TimeInterval.FromMilliseconds(200.0)) // the default is 0.5s per iteration
                    .WithIterationCount(5)
                    .WithMaxRelativeError(0.02) // default is 0.02
                    .WithArguments([|arg|])

    let jobAfter = baseJob.WithCustomBuildConfiguration("After").WithId("After").AsBaseline();
    let jobMain50 = baseJob.WithCustomBuildConfiguration("Main50").WithId("Main50");
    let jobNuGet50 = baseJob.WithCustomBuildConfiguration("NuGet50").WithId("NuGet50");

    let config = DefaultConfig.Instance
                     .AddJob(jobAfter)
                     .AddJob(jobMain50)
                     .AddJob(jobNuGet50)
                     .KeepBenchmarkFiles() // 

    BenchmarkRunner.Run<BenchMap>(config)
    |> ignore
    
    0 // return an integer exit code
