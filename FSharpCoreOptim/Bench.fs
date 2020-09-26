namespace FSharp.Perf

open BenchmarkDotNet.Attributes
open System.Diagnostics
open BenchmarkDotNet.Columns
open BenchmarkDotNet.Configs

module Bench =
    [<Literal>]
    let size = 100;

[<MemoryDiagnoser>]
[<CategoriesColumn; RankColumn>]
[<GroupBenchmarksBy([|BenchmarkLogicalGroupRule.ByMethod|])>]
[<MarkdownExporterAttribute.GitHub>]
[<DisassemblyDiagnoser>]
type BenchMap() =

    [<DefaultValue>]
    [<Params(Bench.size, 10_000)>]
//    [<Params(Bench.size)>]
    val mutable Size: int

    [<DefaultValue>]
    val mutable Map: Map<int,int>
    
    [<GlobalSetup>]
    member this.Setup() =
        let asm = typeof<FSharp.Core.EntryPointAttribute>.Assembly
        let version = FileVersionInfo.GetVersionInfo(asm.Location)
        printfn "Assembly: %A" (asm.CodeBase)
        printfn "Version: %A" (version.FileVersion)
        this.Map <- Map.empty
        for i = 0 to this.Size do
            this.Map <- this.Map.Add(i, i*i)
    
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.getItem() =
        for i = 0 to Bench.size do
            this.Map.[i] |> ignore
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.containsKey() =
        for i = 0 to Bench.size do
            this.Map.ContainsKey(i) |> ignore
            
    [<Benchmark(OperationsPerInvoke=1)>]
    member this.itemCount() =
        this.Map.Count |> ignore            

    [<Benchmark(OperationsPerInvoke=1)>]
    member this.iterForeach() =
        let mutable sum = 0
        for kv in this.Map do
            sum <- sum + kv.Key
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.addItem() =
        let mutable m = Map.empty
        for i = 0 to this.Size do
            m <- m.Add(i, i*i)
        
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.removeItem() =
        for i = 0 to this.Size do
            this.Map <- this.Map.Remove(i)


[<MemoryDiagnoser>]
[<CategoriesColumn; RankColumn>]
[<GroupBenchmarksBy([|BenchmarkLogicalGroupRule.ByMethod|])>]
[<MarkdownExporterAttribute.GitHub>]
[<DisassemblyDiagnoser>]
type BenchSet() =

    [<DefaultValue>]
//    [<Params(Bench.size, 10_000)>]
    [<Params(Bench.size)>]
    val mutable Size: int

    [<DefaultValue>]
    val mutable Set: Set<int>
    
     [<DefaultValue>]
    val mutable SubSet: Set<int>
    
    [<GlobalSetup>]
    member this.Setup() =
        let asm = typeof<FSharp.Core.EntryPointAttribute>.Assembly
        let version = FileVersionInfo.GetVersionInfo(asm.Location)
        printfn "Assembly: %A" (asm.CodeBase)
        printfn "Version: %A" (version.FileVersion)
        this.Set <- Set.empty
        this.SubSet <- Set.empty
        for i = 0 to this.Size do
            this.Set <- this.Set.Add(i)
            if i % 3 = 0 then
                this.SubSet <- this.SubSet.Add(i)    
    
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.containsKey() =
        for i = 0 to Bench.size do
            this.Set.Contains(i) |> ignore

    [<Benchmark(OperationsPerInvoke=1)>]
    member this.isSubsetOf() =
        this.SubSet.IsSubsetOf(this.Set) |> ignore
        
    [<Benchmark(OperationsPerInvoke=1)>]
    member this.maxItem() =
        this.Set.MaximumElement |> ignore
        
    [<Benchmark(OperationsPerInvoke=1)>]
    member this.itemCount() =
        this.Set.Count |> ignore            

    [<Benchmark(OperationsPerInvoke=1)>]
    member this.iterForeach() =
        let mutable sum = 0
        for k in this.Set do
            sum <- sum + k
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.addItem() =
        let mutable m = Set.empty
        for i = 0 to this.Size do
            m <- m.Add(i)
        
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.removeItem() =
        for i = 0 to this.Size do
            this.Set <- this.Set.Remove(i)
