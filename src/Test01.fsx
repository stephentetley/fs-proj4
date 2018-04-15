open System

#load @"ProjApi.fs"
open ProjApi

let temp01 () = 
    printfn "Hello World"
    let lon0 = 22.350 * Math.PI / 180.0 
    let lat0 = 40.084 * Math.PI / 180.0
    let pj = pj_init_plus "+proj=utm +zone=34"
    //let ans = pj_fwd pj (lon0, lat0)
    //printfn "%A" ans
    printfn "Hello World"