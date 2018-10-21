// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause


module FsProj4.RawFFI

open System
open System.Runtime.InteropServices
open System.Text


// The API is now defined in <proj.h>

// Target is 5.2.0
[<Literal>]
let ProjDLL = __SOURCE_DIRECTORY__ +  @"\..\..\lib\lib.5.2.0_x64\proj.dll"


[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjFactors = 
    val MeridionalScale : double           /// h
    val ParallelScale : double             /// k
    val ArealScale : double                /// s

    val AngularDistortion : double         /// omega
    val MeridianParallelAngle : double    /// theta-prime
    val MeridianConvergence : double       /// alpha

    val TissotSemimajor : double           /// a
    val TissotSemiminor : double           /// b

    val DxDlam : double 
    val DxDphi : double
    val DyDlam : double
    val DyDphi : double



// PJ is an opaque struct
type PjPtr = IntPtr
type PjContextPtr = IntPtr
type PjAreaPtr = IntPtr
type PjCoordPtr = IntPtr

type PjOperationsPtr = IntPtr
type PjEllpsPtr = IntPtr
type PjUnitsPtr = IntPtr
type PjPrimeMeridiansPtr = IntPtr

/// Geodetic, mostly spatiotemporal coordinate types

// typedef struct { double   x,   y,  z, t; }  PJ_XYZT;
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjXYZT = 
    val X4 : double
    val Y4 : double
    val Z4 : double
    val T4 : double

// typedef struct { double   u,   v,  w, t; }  PJ_UVWT;
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjUVWT = 
    val U4 : double
    val V4 : double
    val W4 : double
    val T4 : double

// typedef struct { double lam, phi,  z, t; }  PJ_LPZT;
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjLPZT = 
    val Lam4 : double
    val Phi4 : double
    val Z4 : double
    val T4 : double

// typedef struct { double o, p, k; }          PJ_OPK;  /* Rotations: omega, phi, kappa */
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjOPK = 
    val O3 : double
    val P3 : double
    val K3 : double

// typedef struct { double e, n, u; }          PJ_ENU;  /* East, North, Up */
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjENU = 
    val E3 : double
    val N3 : double
    val U3 : double

//typedef struct { double s, a1, a2; }        PJ_GEOD; /* Geodesic length, fwd azi, rev azi */
[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjGEOD = 
    val S3 : double
    val A13 : double
    val A23 : double

[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjUV = 
    new (u, v) = { U2 = u; V2 = v }
    val U2 : double
    val V2 : double

[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjXY = 
    new (x, y) = { X2 = x; Y2 = y }
    val X2 : double
    val Y2 : double

[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjLP = 
    new (lam, phi) = { Lam2 = lam; Phi2 = phi }
    val Lam2 : double
    val Phi2 : double

[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjXYZ = 
    new (x, y, z) = { X3 = x; Y3 = y; Z3 = z }
    val X3 : double
    val Y3 : double
    val Z3 : double


[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjUVW = 
    new (u, v, w) = { U3 = u; V3 = v; W3 = w }
    val U3 : double
    val V3 : double
    val W3 : double


[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjLPZ =
    new (lam, phi, z) = { Lam3 = lam; Phi3 = phi; Z3 = z}
    val Lam3 : double
    val Phi3 : double
    val Z3 : double

type PjCoord =
    [<FieldOffset(0)>]
    val D4 : double[]
    [<FieldOffset(0)>]
    val XYZT : PjXYZT
    [<FieldOffset(0)>]
    val UVWT : PjUVWT
    [<FieldOffset(0)>]
    val LPZT : PjLPZT
    [<FieldOffset(0)>]
    val GEOD : PjGEOD
    [<FieldOffset(0)>]
    val OPK : PjOPK
    [<FieldOffset(0)>]
    val ENU : PjENU
    [<FieldOffset(0)>]
    val XYZ : PjXYZ
    [<FieldOffset(0)>]
    val UVW : PjUVW
    [<FieldOffset(0)>]
    val LPZ : PjLPZ
    [<FieldOffset(0)>]
    val XY : PjXY
    [<FieldOffset(0)>]
    val UV : PjUV
    [<FieldOffset(0)>]
    val LP : PjLP


[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjInfo = 
    // no user defined constructor...
    val mutable Major : int
    val mutable Minor : int
    val mutable Patch : int
    val ReleasePtr : IntPtr
    val VersionPtr : IntPtr
    val SearchPathPtr : IntPtr
    val PathsPtr : IntPtr
    val mutable PathCount : int 

[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjProjInfo = 
    // no user defined constructor...
    val Id : IntPtr
    val Description : IntPtr
    val Definition : IntPtr
    val mutable HasInverse : int
    val mutable Accuracy : double

[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjGridInfo = 
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)>]
    val Gridname : string
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)>]
    val Filename : string
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)>]
    val Format : string
    val LowerLeft : PjLP
    val UpperRight : PjLP
    val NLon : int
    val NLat : int
    val CsLon : double
    val CsLat : double

//struct PJ_INIT_INFO {
//    char        name[32];           /* name of init file                        */
//    char        filename[260];      /* full path to the init file.              */
//    char        version[32];        /* version of the init file                 */
//    char        origin[32];         /* origin of the file, e.g. EPSG            */
//    char        lastupdate[16];     /* Date of last update in YYYY-MM-DD format */
//};

[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjInitInfo = 
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)>]
    val Name : string
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)>]
    val Filename : string
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)>]
    val Version : string
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)>]
    val Origin : string
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)>]
    val LastUpdate : string


// PJ_CONTEXT *proj_context_create (void);
[<DllImport(ProjDLL, EntryPoint="proj_context_create", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjContextPtr proj_context_create();

// PJ_CONTEXT *proj_context_destroy (PJ_CONTEXT *ctx);
[<DllImport(ProjDLL, EntryPoint="proj_context_destroy", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjContextPtr proj_context_destroy(PjContextPtr ctx);


// PJ  *proj_create (PJ_CONTEXT *ctx, const char *definition);
[<DllImport(ProjDLL, EntryPoint="proj_create", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjPtr proj_create(PjContextPtr ctx, string definition);

// PJ  *proj_create_argv (PJ_CONTEXT *ctx, int argc, char **argv);
[<DllImport(ProjDLL, EntryPoint="proj_create", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjPtr proj_create_argv(PjContextPtr ctx, int argc, string [] argv);

// PJ  *proj_create_crs_to_crs(PJ_CONTEXT *ctx, const char *srid_from, const char *srid_to, PJ_AREA *area);
[<DllImport(ProjDLL, EntryPoint="proj_create", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjPtr proj_create_crs_to_crs(PjContextPtr ctx, string srid_from, string srid_to, PjAreaPtr area);

// PJ  *proj_destroy (PJ *P);
[<DllImport(ProjDLL, EntryPoint="proj_destroy", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjPtr proj_destroy(PjPtr p);



type PjDirection = 
    | PjFwd = 1
    | PjIdent = 0
    | PjInv = -1

// int proj_angular_input (PJ *P, enum PJ_DIRECTION dir);
[<DllImport(ProjDLL, EntryPoint="proj_angular_input", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int proj_angular_input(PjPtr p, PjDirection dir);

// int proj_angular_output (PJ *P, enum PJ_DIRECTION dir);
[<DllImport(ProjDLL, EntryPoint="proj_angular_output", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int proj_angular_output(PjPtr p, PjDirection dir);

//PJ_COORD proj_trans (PJ *P, PJ_DIRECTION direction, PJ_COORD coord);
[<DllImport(ProjDLL, EntryPoint="proj_trans", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjCoord proj_trans (PjPtr p, PjDirection direction, PjCoord coord);

//int proj_trans_array (PJ *P, PJ_DIRECTION direction, size_t n, PJ_COORD *coord);
[<DllImport(ProjDLL, EntryPoint="proj_trans_array", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int  proj_trans_array (PjPtr p, PjDirection direction, int n, PjCoordPtr coord);

//size_t proj_trans_generic (
//    PJ *P,
//    PJ_DIRECTION direction,
//    double *x, size_t sx, size_t nx,
//    double *y, size_t sy, size_t ny,
//    double *z, size_t sz, size_t nz,
//    double *t, size_t st, size_t nt
//);
[<DllImport(ProjDLL, EntryPoint="proj_trans_generic", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int  proj_trans_generic (PjPtr p, PjDirection direction, 
    [<In;Out>] double[] x, int sx, int nx,
    [<In;Out>] double[] y, int sy, int ny, 
    [<In;Out>] double[] z, int sz, int nz,
    [<In;Out>] double[] t, int st, int nt);


/// Initializers
// PJ_COORD proj_coord (double x, double y, double z, double t);
[<DllImport(ProjDLL, EntryPoint="proj_coord", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjCoord proj_coord(double x, double y, double z, double t);


// Measure internal consistency - in forward or inverse direction
// double proj_roundtrip (PJ *P, PJ_DIRECTION direction, int n, PJ_COORD *coord);
[<DllImport(ProjDLL, EntryPoint="proj_roundtrip", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_roundtrip (PjPtr p, PjDirection direction, int n, PjCoordPtr b);

// Geodesic distance between two points with angular 2D coordinates
// double proj_lp_dist (const PJ *P, PJ_COORD a, PJ_COORD b);
[<DllImport(ProjDLL, EntryPoint="proj_lp_dist", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_lp_dist (PjPtr p, PjCoord a, PjCoord b);


// The geodesic distance AND the vertical offset 
// double proj_lpz_dist (const PJ *P, PJ_COORD a, PJ_COORD b);
[<DllImport(ProjDLL, EntryPoint="proj_lpz_dist", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_lpz_dist (PjPtr p, PjCoord a, PjCoord b);

// Euclidean distance between two points with linear 2D coordinates
// double proj_xy_dist (PJ_COORD a, PJ_COORD b);
[<DllImport(ProjDLL, EntryPoint="proj_xy_dist", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_xy_dist (PjCoord a, PjCoord b);


// Euclidean distance between two points with linear 3D coordinates
// double proj_xyz_dist (PJ_COORD a, PJ_COORD b);
[<DllImport(ProjDLL, EntryPoint="proj_xyz_dist", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_xyz_dist (PjCoord a, PjCoord b);

// Geodesic distance (in meter) + fwd and rev azimuth between two points on the ellipsoid
// PJ_COORD proj_geod (const PJ *P, PJ_COORD a, PJ_COORD b);
[<DllImport(ProjDLL, EntryPoint="proj_geod", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjCoord proj_geod (PjPtr p ,PjCoord a, PjCoord b);

/// Set or read error level

// int  proj_context_errno (PJ_CONTEXT *ctx);
[<DllImport(ProjDLL, EntryPoint="proj_context_errno", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int proj_context_errno(PjContextPtr ctx);

// int  proj_errno (const PJ *P);
[<DllImport(ProjDLL, EntryPoint="proj_errno", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int proj_errno(PjPtr ctx);

// int  proj_errno_set (const PJ *P, int err);
[<DllImport(ProjDLL, EntryPoint="proj_errno_set", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int proj_errno_set(PjPtr ctx, int err);

// int  proj_errno_reset (const PJ *P);
[<DllImport(ProjDLL, EntryPoint="proj_errno_reset", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int proj_errno_reset(PjPtr ctx);

// int  proj_errno_restore (const PJ *P, int err);
[<DllImport(ProjDLL, EntryPoint="proj_errno_restore", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern int proj_errno_restore(PjPtr ctx, int err);



// Scaling and angular distortion factors
// PJ_FACTORS proj_factors(PJ *P, PJ_COORD lp);
[<DllImport(ProjDLL, EntryPoint="proj_factors", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjFactors proj_factors(PjPtr ctx, PjCoord lp);



/// Info functions - get information about various PROJ.4 entities 

// PJ_INFO proj_info(void);
[<DllImport(ProjDLL, EntryPoint="proj_info", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjInfo proj_info();

// PJ_PROJ_INFO proj_pj_info(PJ *P);
[<DllImport(ProjDLL, EntryPoint="proj_pj_info", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjProjInfo proj_pj_info(PjPtr p);

// PJ_GRID_INFO proj_grid_info(const char *gridname);
[<DllImport(ProjDLL, EntryPoint="proj_grid_info", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjGridInfo proj_grid_info(string gridname);


// PJ_INIT_INFO proj_init_info(const char *initname);
[<DllImport(ProjDLL, EntryPoint="proj_init_info", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjInitInfo proj_init_info(string initname);

// List functions: 
// Get lists of operations, ellipsoids, units and prime meridians. 
// const PJ_OPERATIONS       *proj_list_operations(void);
[<DllImport(ProjDLL, EntryPoint="proj_list_operations", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjOperationsPtr proj_list_operations();

// const PJ_ELLPS            *proj_list_ellps(void);
[<DllImport(ProjDLL, EntryPoint="proj_list_ellps", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjEllpsPtr proj_list_ellps();

// const PJ_UNITS            *proj_list_units(void);
[<DllImport(ProjDLL, EntryPoint="proj_list_units", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjUnitsPtr proj_list_units();

// const PJ_PRIME_MERIDIANS  *proj_list_prime_meridians(void);
[<DllImport(ProjDLL, EntryPoint="proj_list_prime_meridians", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjPrimeMeridiansPtr proj_list_prime_meridians();


/// Helpers 

// double proj_torad (double angle_in_degrees);
[<DllImport(ProjDLL, EntryPoint="proj_torad", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_torad (double angle_in_degrees);


// double proj_todeg (double angle_in_radians);
[<DllImport(ProjDLL, EntryPoint="proj_todeg", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_todeg (double angle_in_radians);


/// Geographical to geocentric latitude - another of the "simple, but useful"

// PJ_COORD proj_geocentric_latitude (const PJ *P, PJ_DIRECTION direction, PJ_COORD coord);
[<DllImport(ProjDLL, EntryPoint="proj_geocentric_latitude", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjCoord proj_geocentric_latitude (PjPtr p, PjDirection direction, PjCoord coord);


// double proj_dmstor(const char *is, char **rs);
[<DllImport(ProjDLL, EntryPoint="proj_dmstor", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_dmstor(string is, 
    [<MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStr, SizeParamIndex=1s)>] string[] rs);

// char*  proj_rtodms(char *s, double r, int pos, int neg);
[<DllImport(ProjDLL, EntryPoint="proj_rtodms", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr proj_rtodms(StringBuilder s, double r, char pos, char neg);
