module Proj4

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

[<System.Runtime.InteropServices.DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll",EntryPoint="pj_run_selftests")>]
extern int pj_run_selftests(int verbosity);


//[<System.Runtime.InteropServices.DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll",EntryPoint="pj_release")>]
//extern string pj_release();
    
