// Acknowledgement: 
// The are the tests from Eric G. Miller's C# binding translated to F#.

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

#load "..\src\SL\Alt\ApiV4.fs"
open SL.Alt.ApiV4


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

// Note err = -38 is "failed to load datum shift file",  
// See example here: 
// https://github.com/OSGeo/proj.4/wiki/ProjAPI
// Note - sending invalid projection strings can case fatal memory access violations.

let Test_pj_transform () : bool = 
    let mutable x : double[] = [| -16.0 |]      |> Array.map (fun v -> v * DEG_TO_RAD)
    let mutable y : double[] = [| 20.25 |]      |> Array.map (fun v -> v * DEG_TO_RAD)
    let src : IntPtr = pj_init_plus(@"+proj=latlong +ellps=clrk66")
    let dst : IntPtr = pj_init_plus(@"+proj=merc +ellps=clrk66 +lat_ts=33") 
    // src -> src works but not src->dst...
    let errno = pj_transform(src, dst, x.Length, 1, x, y, null)
    pj_free(src)
    pj_free(dst)
    if errno <> 0 then 
        printfn "Error: pj_transform {%i}" errno
        false
    else
        printfn "%A\n%A" x y // (x |> Array.map (fun v -> v * RAD_TO_DEG)) (y |> Array.map (fun v -> v * RAD_TO_DEG)) 
        true


let Test_pj_get_release () : unit = 
    try 
        // Don't free result
        let pRelease : IntPtr = pj_get_release()
        let release : string = Marshal.PtrToStringAnsi(pRelease)
        printfn "%s" release
    with
    | ex ->
        printfn "Error: Test_pj_get_release {%A}"  ex

let Test_pj_pr_list() : bool = 
    try 
        let projPJ : IntPtr = pj_init_plus(@"+proj=merc +ellps=clrk66 +lat_ts=33") 
        pj_pr_list(projPJ)
        pj_free(projPJ)
        true
    with
    | ex ->
        printfn "Error: Test_pj_pr_list {%A}"  ex
        false