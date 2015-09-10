using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace autosearch
{
    class Program
    {
        private static int wordStack = 0;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static String kz = "";
        public static Results frmResults = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);

            _hookID = SetHook(_proc);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmResults = new Results();
            Application.Run(frmResults);
            UnhookWindowsHookEx(_hookID);

        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                //kz += (Keys)vkCode;
                if (vkCode == 13 || vkCode == 32)
                    wordStack++;
                if (wordStack > 8) {
                    wordStack--;
                    kz = String.Join(" ", kz.Split(char.Parse(" ")).Skip(1).ToArray());
                }
                if ((vkCode >= 32 && vkCode <= 126) || (vkCode >= 192 && vkCode <= 255))
                    kz += (char)vkCode;
                frmResults.setSearch(kz);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        //These Dll's will handle the hooks. Yaaar mateys!

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // The two dll imports below will handle the window hiding.

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
    }
}
