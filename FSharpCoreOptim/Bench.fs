namespace FSharp.Perf

open System.Runtime.CompilerServices
open BenchmarkDotNet.Attributes
open System.Diagnostics
open BenchmarkDotNet.Configs
open FSharpCoreOptim.Types


module Bench =
    [<Literal>]
    let size = 10;

[<MemoryDiagnoser>]
[<CategoriesColumn; RankColumn>]
[<GroupBenchmarksBy([|BenchmarkLogicalGroupRule.ByMethod|])>]
[<MarkdownExporterAttribute.GitHub>]
type BenchMap() =

    [<DefaultValue>]
    [<Params(Bench.size, 100, 1000)>] // , 1_000_000
//    [<Params(Bench.size)>]
    val mutable Size: int

    [<DefaultValue>]
    val mutable Map: Map<int,int>
    
    [<DefaultValue>]
    val mutable MapIntLike: Map<IntLike,int>
    
    [<DefaultValue>]
    val mutable MapIntLikeNonGenCmp: Map<IntLikeNonGenCmp,int>
    
    [<DefaultValue>]
    val mutable MapString: Map<string,int>
    
    [<DefaultValue>]
    val mutable MapRefLike: Map<RefLike,int>
    
    [<DefaultValue>]
    val mutable MapRefLikeNonGenCmp: Map<RefLikeNonGenCmp,int>
    
    [<DefaultValue>]
    val mutable MapIntRecord: Map<IntRecord,int>
    
    [<DefaultValue>]
    val mutable MapIntRefRecord: Map<IntRefRecord,int>
    
    [<DefaultValue>]
    val mutable Strings: string[]
    
    [<DefaultValue>]
    val mutable RefLikes: RefLike[]
    
    [<DefaultValue>]
    val mutable RefLikeNonGenCmps: RefLikeNonGenCmp[]
        
    [<DefaultValue>]
    val mutable IntRefRecords: IntRefRecord[]
    
    [<GlobalSetup>]
    member this.Setup() =
        let asm = typeof<FSharp.Core.EntryPointAttribute>.Assembly
        let version = FileVersionInfo.GetVersionInfo(asm.Location)
        printfn "Assembly: %A" (asm.CodeBase)
        printfn "Version: %A" (version.FileVersion)
        this.Map <- Map.empty
        this.MapIntLike <- Map.empty
        this.MapIntLikeNonGenCmp <- Map.empty
        this.MapString <- Map.empty
        this.MapRefLike <- Map.empty
        this.MapRefLikeNonGenCmp <- Map.empty
        this.MapIntRecord <- Map.empty
        this.MapIntRefRecord <- Map.empty
        
        this.Strings <- Array.map (fun x -> x.ToString()) [|0..10000|]
        this.RefLikes <- Array.map (fun x -> RefLike(x)) [|0..10000|]
        this.RefLikeNonGenCmps <- Array.map (fun x -> RefLikeNonGenCmp(x)) [|0..10000|]
        this.IntRefRecords <- Array.map (fun x -> {Value1 = x; Value2 = 0}) [|0..10000|]
        
        for i = 0 to this.Size do
            this.Map <- this.Map.Add(i, i*i)
            this.MapIntLike <- this.MapIntLike.Add(new IntLike(i), i)
            this.MapIntLikeNonGenCmp <- this.MapIntLikeNonGenCmp.Add(new IntLikeNonGenCmp(i), i)
            this.MapString <- this.MapString.Add(this.Strings.[i], i)
            this.MapRefLike <- this.MapRefLike.Add(this.RefLikes.[i], i)
            this.MapRefLikeNonGenCmp <- this.MapRefLikeNonGenCmp.Add(this.RefLikeNonGenCmps.[i], i)
            this.MapIntRecord <- this.MapIntRecord.Add({Value1 = i;Value2 = 0}, i)
            this.MapIntRefRecord <- this.MapIntRefRecord.Add(this.IntRefRecords.[i], i)
    
    //////////////////////  Get Item //////////////////////
    
    
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.getItemInt() =
        for i = 0 to Bench.size do
            this.Map.[i] |> ignore
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.getItemIntLike() =
        for i = 0 to Bench.size do
            this.MapIntLike.[Unsafe.As<int,IntLike>(&Unsafe.AsRef(&i))] |> ignore
    
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.getItemString() =
        for i = 0 to Bench.size do
            this.MapString.[this.Strings.[i]] |> ignore
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.getItemRefLike() =
        for i = 0 to Bench.size do
            this.MapRefLike.[this.RefLikes.[i]] |> ignore
             
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.getItemIntRecord() =
        for i = 0 to Bench.size do
            this.MapIntRecord.[{Value1 = i;Value2 = 0}] |> ignore
        
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.getItemIntRefRecord() =
        for i = 0 to Bench.size do
            this.MapIntRefRecord.[this.IntRefRecords.[i]] |> ignore
         
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.getItemIntLikeNonGenCmp() =
        for i = 0 to Bench.size do
            this.MapIntLikeNonGenCmp.[Unsafe.As<int,IntLikeNonGenCmp>(&Unsafe.AsRef(&i))] |> ignore
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.getItemRefLikeNonGenCmp() =
        for i = 0 to Bench.size do
            this.MapRefLikeNonGenCmp.[this.RefLikeNonGenCmps.[i]] |> ignore
             
    //////////////////////  Contains Key //////////////////////            
             
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.containsKeyInt() =
        for i = 0 to Bench.size do
            this.Map.ContainsKey(i) |> ignore
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.containsKeyIntLike() =
        for i = 0 to Bench.size do
            this.MapIntLike.ContainsKey(Unsafe.As<int,IntLike>(&Unsafe.AsRef(&i))) |> ignore
    
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.containsKeyString() =
        for i = 0 to Bench.size do
            this.MapString.ContainsKey(this.Strings.[i]) |> ignore
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.containsKeyRefLike() =
        for i = 0 to Bench.size do
            this.MapRefLike.ContainsKey(this.RefLikes.[i]) |> ignore
             
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.containsKeyIntRecord() =
        for i = 0 to Bench.size do
            this.MapIntRecord.ContainsKey({Value1 = i;Value2 = 0}) |> ignore
        
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.containsKeyIntRefRecord() =
        for i = 0 to Bench.size do
            this.MapIntRefRecord.ContainsKey(this.IntRefRecords.[i]) |> ignore
         
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.containsKeyIntLikeNonGenCmp() =
        for i = 0 to Bench.size do
            this.MapIntLikeNonGenCmp.ContainsKey(Unsafe.As<int,IntLikeNonGenCmp>(&Unsafe.AsRef(&i))) |> ignore
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.containsKeyRefLikeNonGenCmp() =
        for i = 0 to Bench.size do
            this.MapRefLikeNonGenCmp.ContainsKey(this.RefLikeNonGenCmps.[i]) |> ignore
      
    //////////////////////  Item Count //////////////////////            
             
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.itemCountInt() =
        for i = 0 to Bench.size do
            this.Map.Count |> ignore
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.itemCountIntLike() =
        for i = 0 to Bench.size do
            this.MapIntLike.Count |> ignore
    
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.itemCountString() =
        for i = 0 to Bench.size do
            this.MapString.Count |> ignore
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.itemCountRefLike() =
        for i = 0 to Bench.size do
            this.MapRefLike.Count |> ignore
             
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.itemCountIntRecord() =
        for i = 0 to Bench.size do
            this.MapIntRecord.Count |> ignore
        
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.itemCountIntRefRecord() =
        for i = 0 to Bench.size do
            this.MapIntRefRecord.Count |> ignore
         
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.itemCountIntLikeNonGenCmp() =
        for i = 0 to Bench.size do
            this.MapIntLikeNonGenCmp.Count |> ignore
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.itemCountRefLikeNonGenCmp() =
        for i = 0 to Bench.size do
            this.MapRefLikeNonGenCmp.Count |> ignore
      
      
    //////////////////////  Iter Foreach //////////////////////            
             
    [<Benchmark(OperationsPerInvoke=1)>]
    member this.iterForeachInt() =
        let mutable sum = 0
        for kv in this.Map do
            sum <- sum + kv.Value
           
    [<Benchmark(OperationsPerInvoke=1)>]
    member this.iterForeachIntLike() =
        let mutable sum = 0
        for kv in this.MapIntLike do
            sum <- sum + kv.Value
      
    [<Benchmark(OperationsPerInvoke=1)>]
    member this.iterForeachString() =
        let mutable sum = 0
        for kv in this.MapString do
            sum <- sum + kv.Value
            
    [<Benchmark(OperationsPerInvoke=1)>]
    member this.iterForeachRefLike() =
        let mutable sum = 0
        for kv in this.MapRefLike do
            sum <- sum + kv.Value
             
    [<Benchmark(OperationsPerInvoke=1)>]
    member this.iterForeachIntRecord() =
        let mutable sum = 0
        for kv in this.MapIntRecord do
            sum <- sum + kv.Value
        
    [<Benchmark(OperationsPerInvoke=1)>]
    member this.iterForeachIntRefRecord() =
        let mutable sum = 0
        for kv in this.MapIntRefRecord do
            sum <- sum + kv.Value
         
    [<Benchmark(OperationsPerInvoke=1)>]
    member this.iterForeachIntLikeNonGenCmp() =
        let mutable sum = 0
        for kv in this.MapIntLikeNonGenCmp do
            sum <- sum + kv.Value
            
    [<Benchmark(OperationsPerInvoke=1)>]
    member this.iterForeachRefLikeNonGenCmp() =
        let mutable sum = 0
        for kv in this.MapRefLikeNonGenCmp do
            sum <- sum + kv.Value
      
    //////////////////////  Add Item //////////////////////     
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.addItemInt() =
        for i = 0 to Bench.size do
            this.Map <- this.Map.Add(i, i)
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.addItemIntLike() =
        for i = 0 to Bench.size do
            this.MapIntLike <- this.MapIntLike.Add(Unsafe.As<int,IntLike>(&Unsafe.AsRef(&i)), i)
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.addItemString() =
        for i = 0 to Bench.size do
            this.MapString <- this.MapString.Add(this.Strings.[i], i)
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.addItemRefLike() =
        for i = 0 to Bench.size do
            this.MapRefLike <- this.MapRefLike.Add(this.RefLikes.[i], i)
             
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.addItemIntRecord() =
        for i = 0 to Bench.size do
            this.MapIntRecord <- this.MapIntRecord.Add({Value1 = i;Value2 = 0}, i)
        
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.addItemIntRefRecord() =
        for i = 0 to Bench.size do
            this.MapIntRefRecord <- this.MapIntRefRecord.Add(this.IntRefRecords.[i], i)
         
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.addItemIntLikeNonGenCmp() =
        for i = 0 to Bench.size do
            this.MapIntLikeNonGenCmp <- this.MapIntLikeNonGenCmp.Add(Unsafe.As<int,IntLikeNonGenCmp>(&Unsafe.AsRef(&i)), i)
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.addItemRefLikeNonGenCmp() =
        for i = 0 to Bench.size do
            this.MapRefLikeNonGenCmp <- this.MapRefLikeNonGenCmp.Add(this.RefLikeNonGenCmps.[i], i)
            
    //////////////////////  Remove Item //////////////////////
    
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.removeItemInt() =
        for i = 0 to Bench.size do
            this.Map <- this.Map.Remove(i)
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.removeItemIntLike() =
        for i = 0 to Bench.size do
            this.MapIntLike <- this.MapIntLike.Remove(Unsafe.As<int,IntLike>(&Unsafe.AsRef(&i)))

    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.removeItemString() =
        for i = 0 to Bench.size do
            this.MapString <- this.MapString.Remove(this.Strings.[i])
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.removeItemRefLike() =
        for i = 0 to Bench.size do
            this.MapRefLike <- this.MapRefLike.Remove(this.RefLikes.[i])
             
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.removeItemIntRecord() =
        for i = 0 to Bench.size do
            this.MapIntRecord <- this.MapIntRecord.Remove({Value1 = i;Value2 = 0})
        
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.removeItemIntRefRecord() =
        for i = 0 to Bench.size do
            this.MapIntRefRecord <- this.MapIntRefRecord.Remove(this.IntRefRecords.[i])
         
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.removeItemIntLikeNonGenCmp() =
        for i = 0 to Bench.size do
            this.MapIntLikeNonGenCmp <- this.MapIntLikeNonGenCmp.Remove(Unsafe.As<int,IntLikeNonGenCmp>(&Unsafe.AsRef(&i)))
            
    [<Benchmark(OperationsPerInvoke=Bench.size)>]
    member this.removeItemRefLikeNonGenCmp() =
        for i = 0 to Bench.size do
            this.MapRefLikeNonGenCmp <- this.MapRefLikeNonGenCmp.Remove(this.RefLikeNonGenCmps.[i])


[<MemoryDiagnoser>]
[<CategoriesColumn; RankColumn>]
[<GroupBenchmarksBy([|BenchmarkLogicalGroupRule.ByMethod|])>]
[<MarkdownExporterAttribute.GitHub>]
[<DisassemblyDiagnoser>]
type BenchSet() =

    [<DefaultValue>]
    [<Params(Bench.size, 10_000)>]
//    [<Params(Bench.size)>]
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
