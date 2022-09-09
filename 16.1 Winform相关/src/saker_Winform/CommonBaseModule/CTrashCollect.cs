using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace saker_Winform.CommonBaseModule
{
    class CTrashCollect
    {
        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        /// <summary>
        /// 即时释放内存
        /// </summary>
        public static void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
        }


    }
}
