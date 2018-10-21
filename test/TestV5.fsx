// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

open System.Text


#load "..\src\FsProj4\RawFFI.fs"
open FsProj4.RawFFI



let Test_proj_context_create () : bool = 
    try 
        //
        let proj : PjContextPtr = proj_context_create ();
        if proj = IntPtr.Zero then
            printfn "Error: proj_create" 
            false
        else
            printfn "proj_context_create OK"
            ignore <| proj_context_destroy(proj)
            true
    with
    | ex ->
        printfn "Error: Test_proj_context_create {%A}"  ex
        false

// Create with default context...
let Test_proj_create () : bool = 
    try 
        //
        let ctx : PjContextPtr = proj_context_create ();
        if ctx = IntPtr.Zero then
            printfn "Error: proj_create (proj_context_create)" 
            false
        else
            let pj : PjPtr = proj_create(ctx, "+proj=etmerc +lat_0=38 +lon_0=125 +ellps=bessel")
            if pj = IntPtr.Zero then 
                printfn "Error: proj_create"
                false
            else 
                printfn "proj_create OK"
                ignore <| proj_destroy(pj)
                ignore <| proj_context_destroy(ctx)
                true
    with
    | ex ->
        printfn "Error: Test_proj_create {%A}"  ex
        false

let Test_proj_create2 () : bool = 
    try 
        //
        let pj : PjPtr = proj_create(IntPtr.Zero, "+proj=etmerc +lat_0=38 +lon_0=125 +ellps=bessel")
        if pj = IntPtr.Zero then 
            printfn "Error: proj_create"
            false
        else 
            printfn "Test_proj_create2 OK"
            ignore <| proj_destroy(pj)
            true
    with
    | ex ->
        printfn "Error: Test_proj_create2 {%A}"  ex
        false

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

let Test_proj_dmstor () : unit = 
    try 
        let buf : string = "114d35'29.612\"S"
        let ans = proj_dmstor(buf, null)
        printfn "%f" ans
    with
    | ex ->
        printfn "Error: Test_proj_rtodms {%A}"  ex

let Test_proj_rtodms () : unit = 
    try 
        let mutable sb = new StringBuilder(50)
        ignore <| proj_rtodms(sb,-2.0,'N','S')
        printfn "%s" (sb.ToString ())
    with
    | ex ->
        printfn "Error: Test_proj_rtodms {%A}"  ex
