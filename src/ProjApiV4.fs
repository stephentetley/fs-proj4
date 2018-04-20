// Acknowledgement: Largely derived from Eric G. Miller's C# code.

module ProjApiV4

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop


// Target 4.9.3
[<Literal>]
let ProjDLL = __SOURCE_DIRECTORY__ +  @"\..\lib\lib.4.9.3_x64\proj.dll"


let RAD_TO_DEG : double = 57.295779513082321
let DEG_TO_RAD : double = 0.017453292519943296

type PjPtr = IntPtr

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
type ProjXYZ = ProjUVW
type ProjLPZ = ProjUVW



// projXY pj_fwd(projLP, projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_fwd", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjUV pj_fwd(ProjUV LP, IntPtr projPJ);

// projLP pj_inv(projXY, projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_inv", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjUV pj_inv(ProjXY XY, IntPtr projPJ);

// projXYZ pj_fwd3d(projLPZ, projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_fwd3d", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjXYZ pj_fwd3d(ProjLPZ LP, IntPtr projPJ);

// projLPZ pj_inv3d(projXYZ, projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_inv3d", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjLPZ pj_inv3d(ProjXYZ XY, IntPtr projPJ);

// int pj_transform( projPJ src, projPJ dst, long point_count, int point_offset,
//                   double *x, double *y, double *z );
[<DllImport(ProjDLL, EntryPoint="pj_transform", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_transform(IntPtr src, IntPtr dst, int point_count, int point_offset,
    [<In;Out>] double[] x, 
    [<In;Out>] double[] y, 
    [<In;Out>] double[] z);

// int pj_datum_transform( projPJ src, projPJ dst, long point_count, int point_offset,
//                         double *x, double *y, double *z );
[<DllImport(ProjDLL, EntryPoint="pj_datum_transform", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_datum_transform(IntPtr src, IntPtr dst, int point_count, int point_offset,
    [<In;Out>] double[] x, 
    [<In;Out>] double[] y, 
    [<In;Out>] double[] z);


// int pj_geocentric_to_geodetic( double a, double es,
//                                long point_count, int point_offset,
//                                double *x, double *y, double *z );
[<DllImport(ProjDLL, EntryPoint="pj_geocentric_to_geodetic", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_geocentric_to_geodetic( double a, double es, 
    int32 point_count, int point_offset,
    [<In;Out>] double[] x, 
    [<In;Out>] double[] y, 
    [<In;Out>] double[] z);

// int pj_geodetic_to_geocentric( double a, double es,
//                                long point_count, int point_offset,
//                                double *x, double *y, double *z );
[<DllImport(ProjDLL, EntryPoint="pj_geodetic_to_geocentric", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_geodetic_to_geocentric( double a, double es, 
    int32 point_count, int point_offset,
    [<In;Out>] double[] x, 
    [<In;Out>] double[] y, 
    [<In;Out>] double[] z);

// int pj_compare_datums( projPJ srcdefn, projPJ dstdefn );
[<DllImport(ProjDLL, EntryPoint="pj_compare_datums", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_compare_datums( PjPtr srcdefn, PjPtr dstdefn);

// void pj_deallocate_grids(void);
[<DllImport(ProjDLL, EntryPoint="pj_deallocate_grids", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_deallocate_grids();

// void pj_clear_initcache(void);
[<DllImport(ProjDLL, EntryPoint="pj_clear_initcache", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_clear_initcache();

// int pj_is_latlong(projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_is_latlong", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_is_latlong(PjPtr projPJ);

// int pj_is_geocent(projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_is_geocent", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_is_geocent(PjPtr projPJ);


// void pj_pr_list(projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_pr_list", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_pr_list(PjPtr projPJ);


// void pj_free(projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_free", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_free(PjPtr projPJ);


// projPJ pj_init(int, char **);
[<DllImport(ProjDLL, EntryPoint="pj_init", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjPtr pj_init(int argc, 
    [<MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStr, SizeParamIndex=1s)>] string[] argv);

// projPJ pj_init_plus(const char *);
[<DllImport(ProjDLL, EntryPoint="pj_init_plus", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjPtr pj_init_plus([<MarshalAs(UnmanagedType.LPStr)>] string pjstr);





[<DllImport(ProjDLL, EntryPoint="pj_get_release", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr pj_get_release();



[<DllImport(ProjDLL, EntryPoint="pj_run_selftests", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_run_selftests(int verbosity);


