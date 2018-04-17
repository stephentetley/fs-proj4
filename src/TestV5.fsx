open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

#load @"ProjApiV5.fs"
open ProjApiV5


let Test_proj_info () : unit = 
    try 
        let info : PjInfo = proj_info()
        let release : string = Marshal.PtrToStringAnsi(info.ReleasePtr)
        printfn "%i.%i.%i '%s' (path_count = %i)" info.Major info.Minor info.Patch release info.PathCount
    with
    | ex ->
        printfn "Error: Test_proj_info {%A}"  ex
