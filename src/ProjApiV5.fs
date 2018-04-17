module ProjApiV5

open System
open System.Runtime.InteropServices


// The API is now defined in <proj.h>

// Target 5.0.1
[<Literal>]
let ProjDLL = @"E:\coding\libs\proj-5.0.1_bin\bin\proj.dll"

let RAD_TO_DEG : double = 57.295779513082321
let DEG_TO_RAD : double = 0.017453292519943296

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