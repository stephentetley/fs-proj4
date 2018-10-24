// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause


module FsProj4.Proj4

open System
open System.Runtime.InteropServices
open System.Text

open FsProj4.RawFFI


let degToRad (deg:double) : double = proj_torad deg

let radToDeg (rad:double) : double = proj_todeg rad

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
    let ptr : PjPtr = proj_create_crs_to_crs(ctx.Ptr, sridFrom, sridTo, IntPtr.Zero) 
    new Pj (ptr)

type Coord = 
    { X1: double
      Y1: double
      Z1: double
      T1: Double }
    member x.ToPjCoord () : PjCoord = proj_coord(x.X1, x.Y1, x.Z1, x.T1)

    static member FromPjCoord (x:PjCoord) : Coord = 
        { X1 = x.D1; Y1 = x.D2; Z1 = x.D3; T1 = x.D4 }

    


let projTrans (pj:Pj) (direction:PjDirection) (c:Coord) : Coord = 
    let mutable pjArg:PjCoord = c.ToPjCoord()
    let mutable pjAns:PjCoord = proj_coord(0.0, 0.0, 0.0, 0.0)
    pjAns <- proj_trans(pj.Ptr, direction, pjArg)
    Coord.FromPjCoord pjAns