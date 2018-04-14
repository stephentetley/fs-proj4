// Acknowledgement: Largeley derived from Eric G. Miller's C# code.

module Proj4

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

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


// projXY pj_fwd(projLP, projPJ);
// projLP pj_inv(projXY, projPJ);
[<System.Runtime.InteropServices.DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll",EntryPoint="pj_fwd", 
    CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjUV pj_fwd(ProjUV LP, IntPtr projPJ);

[<System.Runtime.InteropServices.DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll",EntryPoint="pj_inv", 
    CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjUV pj_inv(ProjUV XY, IntPtr projPJ);

[<System.Runtime.InteropServices.DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll",EntryPoint="pj_init_plus", 
    CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr pj_init_plus([<MarshalAs(UnmanagedType.LPStr)>] string pjstr);


[<System.Runtime.InteropServices.DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll",EntryPoint="pj_run_selftests")>]
extern int pj_run_selftests(int verbosity);


//[<System.Runtime.InteropServices.DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll",EntryPoint="pj_release")>]
//extern string pj_release();
    
