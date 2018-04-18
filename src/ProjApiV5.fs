module ProjApiV5

open System
open System.Runtime.InteropServices
open System.Text


// The API is now defined in <proj.h>

// Target 5.0.1
[<Literal>]
let ProjDLL = @"E:\coding\libs\proj-5.0.1_bin\bin\proj.dll"


// PJ is an opaque struct
type PjPtr = IntPtr
type PjContextPtr = IntPtr

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
type PjXLP = 
    new (lam, phi) = { Lam2 = lam; Phi2 = phi }
    val Lam2 : double
    val Phi2 : double

[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjUVZ = 
    new (u, v, z) = { U3 = u; V3 = v; Z3 = z }
    val U3 : double
    val V3 : double
    val Z3 : double

[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjXYZ = 
    new (x, y, z) = { X3 = x; Y3 = y; Z3 = z }
    val X3 : double
    val Y3 : double
    val Z3 : double

[<Struct; StructLayout(LayoutKind.Sequential)>]
type PjLPZ =
    new (lam, phi, z) = { Lam3 = lam; Phi3 = phi; Z3 = z}
    val Lam3 : double
    val Phi3 : double
    val Z3 : double


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
// PJ  *proj_create_crs_to_crs(PJ_CONTEXT *ctx, const char *srid_from, const char *srid_to, PJ_AREA *area);

// PJ  *proj_destroy (PJ *P);
[<DllImport(ProjDLL, EntryPoint="proj_destroy", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjPtr proj_destroy(PjPtr p);


// PJ_INFO proj_info(void);
[<DllImport(ProjDLL, EntryPoint="proj_info", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjInfo proj_info();

type PjDirection = 
    | PjFwd = 1
    | PjIdent = 0
    | PjInv = -1


// double proj_torad (double angle_in_degrees);
[<DllImport(ProjDLL, EntryPoint="proj_torad", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_torad (double angle_in_degrees);


// double proj_todeg (double angle_in_radians);
[<DllImport(ProjDLL, EntryPoint="proj_todeg", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_todeg (double angle_in_radians);

// double proj_dmstor(const char *is, char **rs);
[<DllImport(ProjDLL, EntryPoint="proj_dmstor", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_dmstor(string is, 
    [<MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStr, SizeParamIndex=1s)>] string[] rs);

// char*  proj_rtodms(char *s, double r, int pos, int neg);
[<DllImport(ProjDLL, EntryPoint="proj_rtodms", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr proj_rtodms(StringBuilder s, double r, char pos, char neg);
