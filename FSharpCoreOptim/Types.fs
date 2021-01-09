namespace FSharpCoreOptim 

open System
open System.Reflection

type StaticReadonly =
    static member private Init() = DateTime.UtcNow.Hour;
    static member JitConstant = StaticReadonly.Init()
    


module Types =

    [<Struct>]
    type IntRecord =
          { Value1 : int
            Value2 : int
          }
            
    type IntRefRecord =
          { Value1 : int
            Value2 : int
          }
          
    [<StructuralEquality;CustomComparison>]
    type IntLike =
        struct
           val Value: int
           new(v:int) = {Value = v}
           member x.CompareTo(y:IntLike) = x.Value.CompareTo(y.Value)
        end
        
        interface IComparable<IntLike> with
            member x.CompareTo(y) = x.CompareTo(y)
            
        interface IComparable with
            member x.CompareTo(y) = x.CompareTo(y :?> IntLike) 


    type RefLike =
        val Value: int
        new(v:int) = {Value = v}
        member x.CompareTo(y:RefLike) = x.Value.CompareTo(y.Value)
        
        interface IComparable<RefLike> with
            member x.CompareTo(y) = x.CompareTo(y)
            
        interface IComparable with
            member x.CompareTo(y) = x.CompareTo(y :?> RefLike)
            
    type RefLikeNonGenCmp =
        val Value: int
        new(v:int) = {Value = v}
        member x.CompareTo(y:RefLikeNonGenCmp) = x.Value.CompareTo(y.Value)

        interface IComparable with
            member x.CompareTo(y) = x.CompareTo(y :?> RefLikeNonGenCmp)

    [<StructuralEquality;CustomComparison>]
    type IntLikeNonGenCmp =
        struct
           val Value: int
           new(v:int) = {Value = v}
           member x.CompareTo(y:IntLikeNonGenCmp) = x.Value.CompareTo(y.Value)
        end
        
        interface IComparable with
            member x.CompareTo(y) = x.CompareTo(y :?> IntLikeNonGenCmp)
