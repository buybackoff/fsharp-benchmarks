// Learn more about F# at http://fsharp.org

open System
open BenchmarkDotNet.Running
open FSharp.Perf
open System.Globalization

module Convert =
    let fromFloat (x: float) = string x
    let fromFloat32 (x: float32) = string x
    let fromUInt64 (x: uint64) = string x
    let fromInt32 (x: int32) = string x
    let fromUInt32 (x: uint32) = string x
    let fromStr (x: string) = string x
    let fromKey (x: ConsoleKey) = string x
    let fromChar (x: char) = string x
    let fromAny x = string x


type MyDU<'T> =
    | Case1 of 'T
    | Case2 of DateTime

    override this.ToString() =
        match this with
        | Case1 x -> string x
        | Case2 x -> string x

type MyEnum =
    | One = 1uy
    | Two = 2uy
    | Three = 3uy

type ToString =
    static member ofEnumOrIntegral(x: 'T  when 'T : enum<_>) = x.ToString()
    static member ofEnumOrIntegral(x: int32) = x.ToString()
    static member ofEnumOrIntegral(x: int64) = x.ToString()


[<EntryPoint>]
let main argv =
    //let asm = typeof<FSharp.Core.EntryPointAttribute>.Assembly
    //printfn "Assembly: %A" (asm.CodeBase)
    //BenchmarkRunner.Run<BenchLength>()
    //|> ignore
    let str = Guid.Empty.ToString()
    let strStr = string str
    let charStr = string 'X'
    let floatStr = string 42.0
    let float32Str = string 32.32f
    let intStr = string 42
    let enumStr = string ConsoleKey.Backspace
    let enumByHand = (box ConsoleKey.Delete :?> IFormattable).ToString(null, CultureInfo.InvariantCulture)
    let intByHand = (box 84 :?> IFormattable).ToString(null, CultureInfo.InvariantCulture)

    let enum1 = ToString.ofEnumOrIntegral(MyEnum.One)
    let enum2 = ToString.ofEnumOrIntegral 64L
    let enum3 = ToString.ofEnumOrIntegral 34

    let maybe = if Environment.CommandLine.StartsWith("FOO") then Some "wrong" else None

    Console.WriteLine("Enum 1: {0}", enum1)
    Console.WriteLine("Not enum 2: {0}", enum2)
    Console.WriteLine("Not enum 3: {0}", enum3)
    Console.WriteLine("Option: {0}", string(Some 42))
    Console.WriteLine("Option: {0}", string maybe)
    Console.WriteLine("Null: {0}", string null)
    Console.WriteLine("DefaultOf option: {0}", string Unchecked.defaultof<option<int>>)
    Console.WriteLine("Byte enum: {0}", string MyEnum.Three)
    Console.WriteLine("From float: {0}", floatStr)
    Console.WriteLine("From float 32: {0}", float32Str)
    Console.WriteLine("From int: {0}", intStr)
    Console.WriteLine("From enum: {0}", enumStr)
    Console.WriteLine("From string {0}", strStr)
    Console.WriteLine("From char: {0}", charStr)
    Console.WriteLine("From int by hand: {0}", intByHand)
    Console.WriteLine("From enum by hand: {0}", enumByHand)

    

    //printfn "From float: %s" (string 42.0)
    //printfn "From int: %s" (StringLength.toStringInt 42)
    //printfn "From obj: %s" (StringLength.toString 42)
    //printfn "Original: %s" (string 42)
    //printfn "Enum: %s" (string System.ConsoleKey.Backspace)
    
    0 // return an integer exit code
