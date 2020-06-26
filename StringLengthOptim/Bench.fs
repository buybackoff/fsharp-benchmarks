namespace FSharp.Perf

open System
open System.Linq
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Configs
open Microsoft.FSharp.NativeInterop
open System.Text
open System.Runtime.InteropServices
open System.Buffers

module Data =
    let get value =
        match value with
        | 0 -> String.Empty
        | 1 -> "A"
        | 2 -> "Ab"
        | 3 -> "ABr"
        | 4 -> "ABra"
        | _ -> failwith "wrong value"

module StringLength=
    /// How the original used to be coded: leads to two str.Length calls
    let original (str:string) =
        if String.IsNullOrEmpty str then 0
        else str.Length

    /// How the new version in FSharp.Core.dll now is coded after my PR
    let nullOnly (str:string) =
        if isNull str then 0
        else str.Length

    /// Alternative, obj.ReferenceEquals appears slower than isNull in some cases, but shouldn't be (it's an intrinsic)
    let nullRefEq (str: string) =
        if obj.ReferenceEquals(str, null) then 0
        else str.Length





[<MaxRelativeError(0.01)>]
//[<MaxWarmupCount(5); MinWarmupCount(2)>]
//[<IterationTime(120.)>]
//[<MaxIterationCount 30; MinIterationCount 5>]
//[<LegacyJitX64Job>]
//[<LegacyJitX86Job>]
//[<RyuJitX86Job>] (not available???)
//[<RyuJitX64Job>]
[<SimpleJob(RuntimeMoniker.NetCoreApp31)>]
[<MemoryDiagnoser>]
[<CategoriesColumn; RankColumn>]
//[<GroupBenchmarksBy([|BenchmarkLogicalGroupRule.ByCategory|])>]
[<MarkdownExporterAttribute.GitHub>]
[<DisassemblyDiagnoser>]
type BenchLength() =

    [<DefaultValue>]
    //[<Params(0, 1, 2, 10)>]
    [<Params(1)>]
    val mutable Size: int

    [<Benchmark(OperationsPerInvoke=10_000, Baseline = true)>]
    member this.optimized() =
        let s = Data.get this.Size
        for i=0 to 10000 do String.length s |> ignore


    [<Benchmark(OperationsPerInvoke=10_000)>]
    member this.original() =
        let s = Data.get this.Size
        for i=0 to 10000 do StringLength.original s |> ignore

    [<Benchmark(OperationsPerInvoke=10_000)>]
    member this.nullOnly() =
        let s = Data.get this.Size
        for i=0 to 10000 do StringLength.nullOnly s |> ignore
        

    [<Benchmark(OperationsPerInvoke=10_000)>]
    member this.nullRefEq() =
        let s = Data.get this.Size
        for i=0 to 10000 do StringLength.nullRefEq s |> ignore

