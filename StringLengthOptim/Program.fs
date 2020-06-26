// Learn more about F# at http://fsharp.org

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Running
open FSharp.Perf
open Perfolizer.Horology

[<EntryPoint>]
let main argv =
    let asm = typeof<FSharp.Core.EntryPointAttribute>.Assembly
    printfn "Assembly: %A" (asm.CodeBase)

    let baseJob = Job.Default
                    .WithWarmupCount(1) // 1 warmup is enough for our purpose
                    .WithIterationTime(TimeInterval.FromMilliseconds(250.0)) // the default is 0.5s per iteration
                    .WithMaxRelativeError(0.01)

    let jobBefore = baseJob.WithId("Before");

    let jobAfter = baseJob.WithCustomBuildConfiguration("LocalBuild").WithId("After");

    let config = DefaultConfig.Instance.AddJob(jobBefore).AddJob(jobAfter).KeepBenchmarkFiles()

    BenchmarkRunner.Run<BenchLength>(config)
    |> ignore
    
    0 // return an integer exit code
