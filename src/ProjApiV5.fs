module ProjApiV5

open System
open System.Runtime.InteropServices
open System.Text


// The API is now defined in <proj.h>

// Target 5.0.1
[<Literal>]
let ProjDLL = @"E:\coding\libs\proj-5.0.1_bin\bin\proj.dll"

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


[<DllImport(ProjDLL, EntryPoint="proj_info", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern PjInfo proj_info();

type PjDirection = 
    | PjFwd = 1
    | PjIdent = 0
    | PjInv = -1

[<DllImport(ProjDLL, EntryPoint="proj_torad", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_torad (double angle_in_degrees);

[<DllImport(ProjDLL, EntryPoint="proj_todeg", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern double proj_todeg (double angle_in_radians);


[<DllImport(ProjDLL, EntryPoint="proj_rtodms", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)>]
extern IntPtr proj_rtodms(StringBuilder s, double r, char pos, char neg);
