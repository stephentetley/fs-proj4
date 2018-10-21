// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

open System.Text


#load "..\src\FsProj4\RawFFI.fs"
open FsProj4.RawFFI

let inline degreeToRadian (d : double) : double = 
        1.0 * (Math.PI/180.0) *  d

let demo01 () : bool = 
    try 
        //
        let ctx : PjContextPtr = proj_context_create ();
        if ctx = IntPtr.Zero then 
            failwith "proj_context_create"
        else 
            printfn "context (%A)" ctx

        let pj : PjPtr = proj_create_crs_to_crs(ctx, "epsg:4326", "epsg:27700", IntPtr.Zero) 
        if pj = IntPtr.Zero then 
            failwith "proj_create_crs_to_crs"
        else    
            printfn "proj (%A)" pj

            /// Lands end: -5.716117 50.06861
            let mutable aC : PjCoord = proj_coord(degreeToRadian -5.716117, degreeToRadian 50.06861, 0.0, 0.0)
            let mutable bC : PjCoord = proj_coord(0.0, 0.0, 0.0, 0.0)
            printfn "A: x=%f, y=%f"  aC.D1  aC.D2
            
            /// Assign a
            bC <- proj_trans(pj, PjDirection.PjFwd, aC)

            printfn "B: x=%f, y=%f"  bC.D1  bC.D2


            printfn "Cleanup"
            proj_destroy(pj) |> ignore
            proj_context_destroy(ctx) |> ignore
            true         

    with
    | ex ->
        failwithf "Error: Test_proj_create {%A}"  ex

        
