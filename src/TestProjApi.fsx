// Acknowledgement: 
// The are the tests from Eric G. Miller's C# binding translated to F#.

open System

#load @"ProjApi.fs"
open ProjApi


let Test_pj_init () : bool =
    let args : string[] = List.toArray <| ["+proj=lcc"; "+lat_1=33"; "lat_2=45"; "+datum=NAD27"; "+nodefs"]
    let projPJ : IntPtr = pj_init(args.Length, args)
    printfn "%A" projPJ 
    if projPJ = IntPtr.Zero then
        printfn "Error: pj_init"
        false
    else 
        pj_free projPJ
        true

let Test_pj_init_plus () : bool = 
    let arg = "+proj=lcc +lat_1=33 +lat_2=45 +datum=NAD27 +nodefs"
    let projPJ:IntPtr = pj_init_plus arg
    printfn "%A" projPJ 
    if projPJ = IntPtr.Zero then
        printfn "Error: pj_init_plus"
        false
    else 
        pj_free projPJ
        true


let Test_pj_fwd () : bool =
    let projPJ : IntPtr= pj_init_plus(@"+proj=aea +lat_0=0 +lon_0=-120 "
                                        + @"+lat_1=34 +lat_2=40.5 +y_0=-4000000 +datum=NAD83")
    if projPJ = IntPtr.Zero then
        printfn "Error: pj_fwd (pj_init_plus)"
        false
    else  
        let uv : ProjUV = new ProjUV (-120.0 * DEG_TO_RAD, 34.0 * DEG_TO_RAD )
        let ans : ProjUV = pj_fwd (uv, projPJ)
        pj_free projPJ
        if (ans.U2 = System.Double.MaxValue || ans.V2 = System.Double.MaxValue) then
            printfn "Error: pj_fwd"
            false
        else 
            printfn " ({%f},{%f}) " ans.U2 ans.V2
            true


let Test_pj_inv () : bool = 
    let projPJ : IntPtr = pj_init_plus(@"+proj=aea +lat_0=0 +lon_0=-120 "
                                        + @"+lat_1=34 +lat_2=40.5 +y_0=-4000000 +datum=NAD83")
    if projPJ = IntPtr.Zero then
        printfn "Error: pj_inv (pj_init_plus)"
        false
    else  
        let uv : ProjUV = new ProjUV (0.0, -446166.97)
        let ans = pj_inv(uv, projPJ) 
        if (ans.U2 = System.Double.MaxValue || ans.V2 = System.Double.MaxValue) then
            printfn "Error: pj_inv"
            false
        else 
            printfn " ({%f},{%f}) " (ans.U2 * RAD_TO_DEG) (ans.V2 * RAD_TO_DEG)
            true

