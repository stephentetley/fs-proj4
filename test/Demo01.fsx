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
            let mutable aC : PjCoord = proj_coord(proj_torad -5.716117, proj_torad 50.06861, 0.0, 0.0)
            let mutable bC : PjCoord = proj_coord(0.0, 0.0, 0.0, 0.0)
            printfn "A: x=%f, y=%f"  aC.D1  aC.D2
            
            /// Assign a
            bC <- proj_trans(pj, PjDirection.PjFwd, aC)

            printfn "B: easting=%f, northing=%f"  bC.D1  bC.D2


            printfn "Cleanup"
            proj_destroy(pj) |> ignore
            proj_context_destroy(ctx) |> ignore
            true         

    with
    | ex ->
        failwithf "Error: Test_proj_create {%A}"  ex

        
let demo02 () : bool = 
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
            let mutable aC : PjCoord = proj_coord(134177.023475, 25338.881074, 0.0, 0.0)
            let mutable bC : PjCoord = proj_coord(0.0, 0.0, 0.0, 0.0)
            printfn "A: x=%f, y=%f"  aC.D1  aC.D2
            
            /// Assign a
            bC <- proj_trans(pj, PjDirection.PjInv, aC)

            printfn "B: x=%f, y=%f"  (proj_todeg bC.D1)  (proj_todeg bC.D2)


            printfn "Cleanup"
            proj_destroy(pj) |> ignore
            proj_context_destroy(ctx) |> ignore
            true         

    with
    | ex ->
        failwithf "Error: Test_proj_create {%A}"  ex
