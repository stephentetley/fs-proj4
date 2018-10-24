// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause


module FsProj4.TemporaryAPI

open FsProj4.RawFFI
open FsProj4.Proj4


type WGS84Coord = 
    { LongitudeInDegrees: double 
      LatitideInDegrees: double }

type OSGB36Coord = 
    { EastingInMetres: double
      NorthingInMetres: double }

/// May throw an error!
let osgb36ToWGS84 (osgb:OSGB36Coord) : WGS84Coord = 
    use ctx : ProjCtx = new ProjCtx ()
    use pj : Pj = projCreateCrsToCrs ctx "epsg:27700" "epsg:4326"
    if ctx.Invalid || pj.Invalid then 
        failwith "osgb36ToWGS84 - init failed."
    else
        let coordOSGB = { X1 = osgb.EastingInMetres
                        ; Y1 = osgb.NorthingInMetres
                        ; Z1 = 0.0; T1 = 0.0 }
        let coordWGS : Coord = projTrans pj PjDirection.PjFwd coordOSGB

        { LongitudeInDegrees = radToDeg coordWGS.X1
        ; LatitideInDegrees = radToDeg coordWGS.Y1 }

/// May throw an error!
let wgs84ToOSGB36 (wgs:WGS84Coord) :OSGB36Coord = 
    use ctx : ProjCtx = new ProjCtx ()
    use pj : Pj = projCreateCrsToCrs ctx "epsg:4326" "epsg:27700" 
    if ctx.Invalid || pj.Invalid then 
        failwith "wgs84ToOSGB36 - init failed."
    else
        let coordWGS = { X1 = degToRad wgs.LongitudeInDegrees
                        ; Y1 = degToRad wgs.LatitideInDegrees
                        ; Z1 = 0.0; T1 = 0.0 }
        let coordOSGB : Coord = projTrans pj PjDirection.PjFwd coordWGS

        { EastingInMetres = coordOSGB.X1
        ; NorthingInMetres = coordOSGB.Y1}