// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

open System.Text

// Lands End
// DMS 50d 4' 7" N, 5d 42' 58.02" W
// Decimal 50.06861 (Lat), -5.716117 (Lon)  (decimal degrees)

// OS Grid Reference: SW3417725338 
// All-numeric format: 134177 (Easting) 25339 (Northing)


#load "..\src\FsProj4\RawFFI.fs"
#load "..\src\FsProj4\Proj4.fs"
#load "..\src\FsProj4\TemporaryAPI.fs"
open FsProj4.RawFFI
open FsProj4.Proj4
open FsProj4.TemporaryAPI

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


let demo02 () : unit = 
    try 
        let osgb = { EastingInMetres = 134177.023475; NorthingInMetres = 25338.881074 }
        printfn "%A" <| osgb36ToWGS84 osgb        
    with
    | ex ->
        failwithf "Error: {%A}"  ex
        

let demo03 () : unit = 
    try 
        let wgs = { LatitideInDegrees = 50.06861; LongitudeInDegrees = -5.716117 }
        printfn "%A" <| wgs84ToOSGB36 wgs        
    with
    | ex ->
        failwithf "Error: {%A}"  ex