// Learn more about F# at http://fsharp.org

open System
open BenchmarkDotNet.Running
open FSharp.Perf

[<EntryPoint>]
let main argv =
    let asm = typeof<FSharp.Core.EntryPointAttribute>.Assembly
    printfn "Assembly: %A" (asm.CodeBase)
    BenchmarkRunner.Run<BenchLength>()
    |> ignore
    
    0 // return an integer exit code
