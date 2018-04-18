open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

#load @"ProjApiV5.fs"
open ProjApiV5
open System.Text


let Test_proj_info () : unit = 
    try 
        let info : PjInfo = proj_info()
        let release : string = Marshal.PtrToStringAnsi(info.ReleasePtr)
        printfn "%i.%i.%i '%s' (path_count = %i)" info.Major info.Minor info.Patch release info.PathCount
    with
    | ex ->
        printfn "Error: Test_proj_info {%A}"  ex

let Test_proj_torad () : unit = 
    try 
        let d45 : double = 45.0
        Printf.printfn "%f:<degrees> = %f:<radians>" d45 (proj_torad d45)
    with
    | ex ->
        printfn "Error: Test_proj_torad {%A}"  ex

let Test_proj_rtodms () : unit = 
    try 
        let mutable sb = new StringBuilder(50)
        ignore <| proj_rtodms(sb,-2.0,'N','S')
        printfn "%s" (sb.ToString ())
    with
    | ex ->
        printfn "Error: Test_proj_rtodms {%A}"  ex
