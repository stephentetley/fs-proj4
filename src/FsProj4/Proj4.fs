// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause


module FsProj4.Proj4

open System
open System.Runtime.InteropServices
open System.Text

open FsProj4.RawFFI


let degToRad (deg:double) : double = proj_torad deg

let radTodeg (rad:double) : double = proj_todeg rad

type ProjCtx =
    val internal Ptr : PjContextPtr

    new () = { Ptr = proj_context_create() }

    member x.Invalid : bool = 
        x.Ptr = IntPtr.Zero

    interface IDisposable with
        member x.Dispose() = 
            proj_context_destroy(x.Ptr) |> ignore


type Pj = 
    val internal Ptr : PjPtr

    new (ctx:ProjCtx, definition:string) = { Ptr = proj_create(ctx.Ptr, definition) }
    
    internal new (pjPtr:PjPtr) = { Ptr = pjPtr }

    member x.Invalid : bool = 
        x.Ptr = IntPtr.Zero

    interface IDisposable with
        member x.Dispose() = 
            proj_destroy(x.Ptr) |> ignore

let projCreateCrsToCrs (ctx:ProjCtx) (sridFrom:string) (sridTo:string) : Pj = 
    let ptr : PjPtr = proj_create_crs_to_crs(ctx.Ptr, sridFrom, sridFrom, IntPtr.Zero) 
    new Pj (ptr)
