// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

open System.Text


#load "..\src\FsProj4\RawFFI.fs"
open FsProj4.RawFFI

let demo01 () : bool = 
    try 
        //
        let ctx : PjContextPtr = proj_context_create ();
        if ctx <> IntPtr.Zero then
            let pj : PjPtr = proj_create_crs_to_crs(ctx, "epsg:4326", "epsg:27700", IntPtr.Zero) 
            if pj <> IntPtr.Zero then 
                printfn "proj_create OK"
                proj_destroy(pj) |> ignore
                proj_context_destroy(ctx) |> ignore
                true

            else 
                printfn "proj_create_crs_to_crs (%A)" pj
                proj_context_destroy(ctx) |> ignore
                failwith "Error: proj_create_crs_to_crs "
        else
            failwith "Error: proj_create (proj_context_create)" 
           

    with
    | ex ->
        failwithf "Error: Test_proj_create {%A}"  ex
        
