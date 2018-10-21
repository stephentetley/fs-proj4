// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

open System.Text


#load "..\src\FsProj4\RawFFI.fs"
open FsProj4.RawFFI


/// from the gie.c test suite

let demo01 () : bool = 
    try 
        //
        let ctx : PjContextPtr = proj_context_create ();
        if ctx = IntPtr.Zero then 
            failwith "proj_context_create"
        else 
            printfn "context (%A)" ctx

        let pj : PjPtr = proj_create_crs_to_crs(ctx, "epsg:25832", "epsg:25833", IntPtr.Zero) 
        if pj = IntPtr.Zero then 
            failwith "proj_create_crs_to_crs"
        else    
            printfn "proj (%A)" pj
            let aPtr : PjCoord = proj_coord(0.0, 0.0, 0.0, 0.0)
            if aPtr = IntPtr.Zero then
                failwith "proj_coord"
            else 
                printfn "aPtr (%A)" aPtr
            /// Assign a
            let ds : double [] = [|700000.0; 6000000.0|]
            Marshal.Copy(ds, 0, aPtr, 2)
            printfn "Copied"

            let temp : double [] = [|0.0; 0.0|]
            Marshal.Copy(aPtr, temp, 0, 2)
            printfn "temp {%f, %f}" temp.[0] temp.[1]

            
            let bPtr : PjCoord = proj_trans(pj, 1, aPtr);
            if bPtr = IntPtr.Zero then
                failwith "proj_trans"

            else 
                printfn "bPtr (%A)" bPtr

            //Marshal.Copy(bPtr, ds, 0, 2)

            //printfn "ds {%f, %f}" ds.[0] ds.[1]

            printfn "Cleanup"
            proj_destroy(pj) |> ignore
            proj_context_destroy(ctx) |> ignore
            true         

    with
    | ex ->
        failwithf "Error: Test_proj_create {%A}"  ex
        
