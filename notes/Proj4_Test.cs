using System;
using System.Runtime.InteropServices;


// Ctrl+F5 to run ...
namespace proj4_test
{

    [StructLayout(LayoutKind.Sequential)]
    public struct ProjUV
    {
        public double u;
        public double v;
    }

    public class Proj
    {
        public const double RAD_TO_DEG = 57.29577951308232;
        public const double DEG_TO_RAD = .0174532925199432958;

        [DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern ProjUV pj_fwd(ProjUV LP, IntPtr projPJ);

        [DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern ProjUV pj_inv(ProjUV XY, IntPtr projPJ);

        [DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int pj_transform(IntPtr src, IntPtr dst,
            int point_count, int point_offset,
            [InAttribute, OutAttribute] double[] x,
            [InAttribute, OutAttribute] double[] y,
            [InAttribute, OutAttribute] double[] z);

        [DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int pj_is_latlong(IntPtr projPJ);

        [DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void pj_free(IntPtr projPJ);

        [DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr pj_init(int argc,
                [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStr,
                     SizeParamIndex=1)] string[] argv);

        [DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr pj_init_plus(
                [MarshalAs(UnmanagedType.LPStr)] string pjstr);
        [DllImport(@"C:\Program Files\QGIS 3.0\bin\proj.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr pj_get_release();
    }

    public class Program
    {
        static bool Test_pj_get_release()
        {
            bool result = true;
            try
            {
                // Don't free result
                IntPtr pRelease = Proj.pj_get_release();
                string release = Marshal.PtrToStringAnsi(pRelease);
                Console.WriteLine(release);
            }
            catch { result = false; }
            return result;
        }

        // see:
        // https://github.com/OSGeo/proj.4/wiki/ProjAPI
        static bool Test_pj_transform()
        {
            double[] x = { -16 };
            double[] y = { 20.25 };
            bool ans = false;

            for (int i = 0; i < x.Length; i++)
            {
                Console.WriteLine("{0:F2}, {1:F2}", x[i], y[i]);
            }

            for (int i = 0; i < x.Length; i++)
            {
                x[i] *= Proj.DEG_TO_RAD;
                y[i] *= Proj.DEG_TO_RAD;
            }
            for (int i = 0; i < x.Length; i++)
            {
                Console.WriteLine("{0:F2}, {1:F2}", x[i], y[i]);
            }

            IntPtr src = Proj.pj_init_plus(@"+proj=latlong +ellps=clrk66");
            IntPtr dst = Proj.pj_init_plus("+proj=merc +ellps=clrk66 +lat_ts=33");
            int errno = Proj.pj_transform(src, dst, x.Length, 1, x, y, null);

            if (errno != 0)
            {
                Console.WriteLine("Error: pj_transform {0}", errno);
                ans = false;
            }
            else
            {
                for (int i = 0; i < x.Length; i++)
                {
                    Console.WriteLine("{0}, {1}", x[i], y[i]);
                }
                ans = true;
            }
            Proj.pj_free(src);
            Proj.pj_free(dst);
            return ans;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Testing pj_transform ...");
            if (Test_pj_transform())
            {
                Console.WriteLine("succeeded");
            }
            else
            {
                Console.WriteLine("failed");
            }

            Console.Write("Testing pj_get_release ...");
            if (Test_pj_get_release())
            {
                Console.WriteLine("succeeded");
            }
            else
            {
                Console.WriteLine("failed");
            }
        }
    }
}
