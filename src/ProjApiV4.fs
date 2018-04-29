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

// This is void* in C
type ProjPj = IntPtr    

type ProjXY = ProjUV
type ProjLP = ProjUV
type ProjXYZ = ProjUVW
type ProjLPZ = ProjUVW

// This is void* in C
type ProjCtx = IntPtr 

// projXY pj_fwd(projLP, projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_fwd", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjUV pj_fwd(ProjUV LP, ProjPj projPJ);

// projLP pj_inv(projXY, projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_inv", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjUV pj_inv(ProjXY XY, ProjPj projPJ);

// projXYZ pj_fwd3d(projLPZ, projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_fwd3d", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjXYZ pj_fwd3d(ProjLPZ LP, ProjPj projPJ);

// projLPZ pj_inv3d(projXYZ, projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_inv3d", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjLPZ pj_inv3d(ProjXYZ XY, ProjPj projPJ);

// int pj_transform( projPJ src, projPJ dst, long point_count, int point_offset,
//                   double *x, double *y, double *z );
[<DllImport(ProjDLL, EntryPoint="pj_transform", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_transform(ProjPj src, ProjPj dst, int point_count, int point_offset,
    [<In;Out>] double[] x, 
    [<In;Out>] double[] y, 
    [<In;Out>] double[] z);

// int pj_datum_transform( projPJ src, projPJ dst, long point_count, int point_offset,
//                         double *x, double *y, double *z );
[<DllImport(ProjDLL, EntryPoint="pj_datum_transform", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_datum_transform(ProjPj src, ProjPj dst, int point_count, int point_offset,
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
extern int pj_compare_datums(ProjPj srcdefn, ProjPj dstdefn);

// int pj_apply_gridshift( projCtx, const char *, int,
//                         long point_count, int point_offset,
//                         double *x, double *y, double *z );
[<DllImport(ProjDLL, EntryPoint="pj_apply_gridshift", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_apply_gridshift(ProjCtx ctx, [<MarshalAs(UnmanagedType.LPStr)>] string str, int i,
    int32 point_count, int point_offset,
    [<In;Out>] double[] x, 
    [<In;Out>] double[] y, 
    [<In;Out>] double[] z);

// void pj_deallocate_grids(void);
[<DllImport(ProjDLL, EntryPoint="pj_deallocate_grids", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_deallocate_grids();

// void pj_clear_initcache(void);
[<DllImport(ProjDLL, EntryPoint="pj_clear_initcache", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_clear_initcache();

// int pj_is_latlong(projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_is_latlong", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_is_latlong(ProjPj projPJ);

// int pj_is_geocent(projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_is_geocent", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_is_geocent(ProjPj projPJ);

// void pj_get_spheroid_defn(projPJ defn, double *major_axis, double *eccentricity_squared);


// void pj_pr_list(projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_pr_list", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_pr_list(ProjPj projPJ);


// void pj_free(projPJ);
[<DllImport(ProjDLL, EntryPoint="pj_free", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_free(ProjPj projPJ);

// void pj_set_finder( const char *(*)(const char *) );
// void pj_set_searchpath ( int count, const char **path );

// projPJ pj_init(int, char **);
[<DllImport(ProjDLL, EntryPoint="pj_init", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjPj pj_init(int argc, 
    [<MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStr, SizeParamIndex=1s)>] string[] argv);

// projPJ pj_init_plus(const char *);
[<DllImport(ProjDLL, EntryPoint="pj_init_plus", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjPj pj_init_plus([<MarshalAs(UnmanagedType.LPStr)>] string pjstr);

// projPJ pj_init_plus_ctx( projCtx, const char * );
[<DllImport(ProjDLL, EntryPoint="pj_init_plus_ctx", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjPj pj_init_plus_ctx(ProjCtx ctx, 
    [<MarshalAs(UnmanagedType.LPStr)>] string pjstr);


// char *pj_get_def(projPJ, int);
[<DllImport(ProjDLL, EntryPoint="pj_get_def", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjPj pj_get_def(ProjPj pj, int i);


// projPJ pj_latlong_from_proj( projPJ );
[<DllImport(ProjDLL, EntryPoint="pj_latlong_from_proj", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern ProjPj pj_latlong_from_proj(ProjPj pj);

// void *pj_malloc(size_t);
[<DllImport(ProjDLL, EntryPoint="pj_malloc", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr pj_malloc(int sz);

// void pj_dalloc(void *);
[<DllImport(ProjDLL, EntryPoint="pj_dalloc", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_dalloc(IntPtr);

// void *pj_calloc (size_t n, size_t size);
[<DllImport(ProjDLL, EntryPoint="pj_calloc", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr pj_calloc(int n, int size);

// void *pj_dealloc (void *ptr);
[<DllImport(ProjDLL, EntryPoint="pj_dealloc", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_dealloc(IntPtr);

// char *pj_strerrno(int);
[<DllImport(ProjDLL, EntryPoint="pj_strerrno", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr pj_strerrno(int);

// int *pj_get_errno_ref(void);
[<DllImport(ProjDLL, EntryPoint="pj_get_errno_ref", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr pj_get_errno_ref(int);

// const char *pj_get_release(void);
[<DllImport(ProjDLL, EntryPoint="pj_get_release", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr pj_get_release();


// void pj_acquire_lock(void);
[<DllImport(ProjDLL, EntryPoint="pj_acquire_lock", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_acquire_lock();

// void pj_release_lock(void);
[<DllImport(ProjDLL, EntryPoint="pj_release_lock", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_release_lock();

// void pj_cleanup_lock(void);
[<DllImport(ProjDLL, EntryPoint="pj_cleanup_lock", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern void pj_cleanup_lock();

// int pj_run_selftests (int verbosity);
[<DllImport(ProjDLL, EntryPoint="pj_run_selftests", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int pj_run_selftests(int verbosity);