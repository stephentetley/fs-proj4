// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

open System.Text


#load "..\src\FsProj4\RawFFI.fs"
#load "..\src\FsProj4\Proj4.fs"
open FsProj4.RawFFI
open FsProj4.Proj4

let demo01 () = 
    use ctx : ProjCtx = new ProjCtx ()
    use pj : Pj = projCreateCrsToCrs ctx "epsg:4326" "epsg:27700"
    if pj.Invalid then 
        printfn "pj init falied"
    else 
        printfn "okay1" 
    if ctx.Invalid then 
        printfn "ctx init falied"
    else
        let ans = projTrans pj PjDirection.PjFwd { X1 = degToRad -5.716117; Y1 = degToRad 50.06861; Z1 = 0.0; T1 = 0.0 }
        printfn "Coord { easting=%f; northing=%f }" ans.X1 ans.Y1


let demo02 () : bool = 
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

            let mutable aC : PjCoord = proj_coord(0.0, 0.0, 0.0, 0.0)
            let mutable bC : PjCoord = proj_coord(0.0, 0.0, 0.0, 0.0)
            printfn "A: x=%f, y=%f"  aC.D1  aC.D2
            
            /// Assign a
            aC.D1 <- 700000.0
            aC.D2 <- 6000000.0

            bC <- proj_trans(pj, PjDirection.PjFwd, aC)

            printfn "B: x=%f, y=%f"  bC.D1  bC.D2


            printfn "Cleanup"
            proj_destroy(pj) |> ignore
            proj_context_destroy(ctx) |> ignore
            true         

    with
    | ex ->
        failwithf "Error: Test_proj_create {%A}"  ex
        
