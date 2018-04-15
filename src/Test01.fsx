open System

#load @"ProjApi.fs"
open ProjApi


let Test_pj_init_plus () : unit = 
    let arg = "+proj=lcc +lat_1=33 +lat_2=45 +datum=NAD27 +nodefs"
    let projPJ:IntPtr = pj_init_plus arg
    printfn "%A" projPJ 
    if projPJ = IntPtr.Zero then
        printfn "Error: pj_init_plus"
    else pj_free projPJ



let temp01 () = 
    printfn "Hello World"
    let lon0 = 22.350 * Math.PI / 180.0 
    let lat0 = 40.084 * Math.PI / 180.0
    let pj = pj_init_plus "+proj=utm +zone=34"
    //let ans = pj_fwd pj (lon0, lat0)
    //printfn "%A" ans
    printfn "Hello World"