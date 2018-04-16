// Acknowledgement: Largeley derived from Eric G. Miller's C# code.

module ProjApi

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop


let RAD_TO_DEG : double = 57.295779513082321
let DEG_TO_RAD : double = 0.017453292519943296


[<Struct; StructLayout(LayoutKind.Sequential)>]
type ProjUV = 
    new (u, v) = { U2 = u; V2 = v }
    val U2 : double
    val V2 : double

[<Struct; StructLayout(LayoutKind.Sequential)>]
type ProjUVW = 
    new (u, v, w) = { U3 = u; V3 = v; W3 = w }
    val U3 : double
    val V3 : double
    val W3 : double

type ProjXY = ProjUV
type ProjLP = ProjUV
type ProjXYZ = ProjUVZ
type ProjLPZ = ProjUVZ


[<Literal>]
let ProjDLL = @"C:\Program Files\QGIS 3.0\bin\proj.dll"

// projXY pj_fwd(projLP, projPJ);
// projLP pj_inv(projXY, projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_fwd", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjUV pj_fwd(ProjUV LP, IntPtr projPJ);

[<DllImport(ProjDLL, EntryPoint="pj_inv", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjUV pj_inv(ProjUV XY, IntPtr projPJ);


[<DllImport(ProjDLL, EntryPoint="pj_transform", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_transform(IntPtr src, IntPtr dst, int point_count, int point_offset,
     [<In;Out>] double[] x, 
     [<In;Out>] double[] y, 
     [<In;Out>] double[] z);

//[<DllImport(ProjDLL, EntryPoint="pj_transform", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
//extern int pj_transform(IntPtr src, IntPtr dst, int point_count, int point_offset,
//     IntPtr x, 
//     IntPtr y, 
//     IntPtr z);



[<DllImport(ProjDLL, EntryPoint="pj_init", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr pj_init(int argc, 
    [<MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStr, SizeParamIndex=1s)>] string[] argv);

[<DllImport(ProjDLL, EntryPoint="pj_init_plus", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr pj_init_plus([<MarshalAs(UnmanagedType.LPStr)>] string pjstr);



[<DllImport(ProjDLL, EntryPoint="pj_free", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_free(IntPtr projPJ);


[<DllImport(ProjDLL, EntryPoint="pj_get_release", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr pj_get_release();



[<DllImport(ProjDLL, EntryPoint="pj_run_selftests", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_run_selftests(int verbosity);


