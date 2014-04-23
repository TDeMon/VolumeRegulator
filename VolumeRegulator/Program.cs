using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Diagnostics;

namespace VolumeRegulator
{
    class Program
    {
        static IntPtr thisHandle;


        static void Main(string[] args)
        {
            thisHandle = FindWindow(null, Console.Title);
            //ShowWindow(thisHandle, Constants.SW_HIDE);
            SetWindowPos((IntPtr)thisHandle, (IntPtr)Constants.HWND_BOTTOM, 0, 0, 0, 0, Constants.SWP_NOMOVE | Constants.SWP_NOSIZE /*| Constants.SWP_HIDEWINDOW*/);

            //ActivateInternet();
            //Mute();

            if (args.Length > 0)
            {
                if (args[0] == "-mute")
                {
                    Mute();
                }
                if (args[0] == "-up")
                {
                    IncreaseVol();
                }
                if (args[0] == "-down")
                {
                    DecreaseVol();
                }
                if (args[0] == "-internet")
                {
                    ActivateInternet();
                }
            }

        }


        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, String sClassName, String sAppName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hWnd, UInt32 wCmd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
                                               int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        //<summary>
        //Выключение-включение звука
        //</summary>
        static private void Mute()
        {
            SendMessageW(thisHandle, WM_APPCOMMAND, thisHandle,  (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        //<summary>
        //Убавление громкости
        //</summary>
        static private void DecreaseVol()
        {
            SendMessageW(thisHandle, WM_APPCOMMAND, thisHandle,(IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        //<summary>
        //Прибавление звука
        //</summary>
        static private void IncreaseVol()
        {
            SendMessageW(thisHandle, WM_APPCOMMAND, thisHandle,(IntPtr)APPCOMMAND_VOLUME_UP);
        }

        public class Constants
        {
            public const int GW_HWNDFIRST = 0;
            public const int GW_HWNDNEXT = 2;
            public const int GW_HWNDPREV = 3;
            public const int GW_CHILD = 5;

            public const int SW_HIDE = 0;
            public const int SW_SHOWNORMAL = 1;
            public const int SW_NORMAL = 1;
            public const int SW_SHOWMINIMIZED = 2;
            public const int SW_SHOWMAXIMIZED = 3;
            public const int SW_MAXIMIZE = 3;
            public const int SW_SHOWNOACTIVATE = 4;
            public const int SW_SHOW = 5;
            public const int SW_MINIMIZE = 6;
            public const int SW_SHOWMINNOACTIVE = 7;
            public const int SW_SHOWNA = 8;
            public const int SW_RESTORE = 9;
            public const int SW_SHOWDEFAULT = 10;
            public const int SW_FORCEMINIMIZE = 11;
            public const int SW_MAX = 11;

            public const int SWP_FRAMECHANGED = 0x0020;
            public const int SWP_NOMOVE = 0x0002;
            public const int SWP_NOSIZE = 0x0001;
            public const int SWP_NOZORDER = 0x0004;
            public const int SWP_SHOWWINDOW = 0x0040;
            public const int HWND_TOPMOST = -1;
            public const int HWND_NOTOPMOST = -2;
            public const int HWND_BOTTOM = 1;

            public const int WM_SETFOCUS = 0x0007;
        }

        /// <summary>
        /// Contains information about the placement of a window on the screen.
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPLACEMENT
        {
            /// <summary>
            /// The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof(WINDOWPLACEMENT).
            /// <para>
            /// GetWindowPlacement and SetWindowPlacement fail if this member is not set correctly.
            /// </para>
            /// </summary>
            public int Length;

            /// <summary>
            /// Specifies flags that control the position of the minimized window and the method by which the window is restored.
            /// </summary>
            public int Flags;

            /// <summary>
            /// The current show state of the window.
            /// </summary>
            public int ShowCmd;

            /// <summary>
            /// The coordinates of the window's upper-left corner when the window is minimized.
            /// </summary>
            public System.Drawing.Point MinPosition;

            /// <summary>
            /// The coordinates of the window's upper-left corner when the window is maximized.
            /// </summary>
            public System.Drawing.Point MaxPosition;

            /// <summary>
            /// The window's coordinates when the window is in the restored position.
            /// </summary>
            public System.Drawing.Rectangle NormalPosition;

            /// <summary>
            /// Gets the default (empty) value.
            /// </summary>
            public static WINDOWPLACEMENT Default
            {
                get
                {
                    WINDOWPLACEMENT result = new WINDOWPLACEMENT();
                    result.Length = Marshal.SizeOf(result);
                    return result;
                }
            }
        }


        /// <summary>
        /// Переключается в браузер:
        /// </summary>
        static private void ActivateInternet()
        {
            //System.Threading.Thread.Sleep(3000);
            IntPtr handle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Chrome_WidgetWin_1", null);
            //IntPtr hide_handle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Chrome_WidgetWin_1", null);
            //System.Threading.Thread.Sleep(5000);

            if (handle != IntPtr.Zero)
            {

                //System.Threading.Thread.Sleep(1000);
                IntPtr current_active = GetForegroundWindow();
                handle = FindWindowEx(IntPtr.Zero, handle, "Chrome_WidgetWin_1", null);

                WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
                placement.Length = Marshal.SizeOf(placement);
                GetWindowPlacement((IntPtr)handle, out placement);
                if (placement.ShowCmd == 3 || placement.ShowCmd == 1)//maximized || normal
                {
                    ShowWindow((IntPtr)handle, Constants.SW_MINIMIZE);
                    //UpdateWindow((IntPtr)handle);
                    return;
                }
                //if (current_active != handle)
                else if (placement.ShowCmd == 2)//minimized
                {
                    //handle = FindWindowEx(IntPtr.Zero, handle, "Chrome_WidgetWin_1", null);
                    ShowWindow((IntPtr)handle, Constants.SW_SHOW);
                    ShowWindow((IntPtr)handle, Constants.SW_MAXIMIZE);
                    UpdateWindow((IntPtr)handle);
                    SetActiveWindow((IntPtr)handle);
                    SetFocus((IntPtr)handle);
                    //SetWindowPos((IntPtr)handle, (IntPtr)Constants.HWND_TOP, 0, 0, 0, 0, Constants.SWP_NOMOVE | Constants.SWP_NOSIZE | Constants.SWP_FRAMECHANGED);
                    SetForegroundWindow(handle);
                    return;
                }
                else if (false)//normal
                {
                    SetForegroundWindow(handle);
                    //handle = FindWindowEx(IntPtr.Zero, handle, "Chrome_WidgetWin_1", null);
                    ShowWindow((IntPtr)handle, Constants.SW_MINIMIZE);
                    //ShowWindow((IntPtr)handle, Constants.SW_HIDE);
                    //ShowWindow((IntPtr)handle, Constants.SW_RESTORE);
                    //UpdateWindow((IntPtr)handle);
                    return;
                }
            }
            else
            {
                Process.Start("chrome.exe", "www.yandex.ru");
                System.Threading.Thread.Sleep(1000);
            }
            //System.Threading.Thread.Sleep(1000);

        }

    }

}
