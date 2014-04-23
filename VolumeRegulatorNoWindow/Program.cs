using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace VolumeRegulatorNoWindow
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static IntPtr thisHandle;

        [STAThread]
        static void Main(string[] args)
        {
            thisHandle = FindWindow(null, Console.Title);
            //ShowWindow(thisHandle, SW_HIDE);

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


        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        static private void Mute()//Выключение-включение звука
        {
            SendMessageW(thisHandle, WM_APPCOMMAND, thisHandle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }
        static private void DecreaseVol()//Убавление громкости
        {
            SendMessageW(thisHandle, WM_APPCOMMAND, thisHandle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        static private void IncreaseVol()//Прибавление звука
        {
            SendMessageW(thisHandle, WM_APPCOMMAND, thisHandle, (IntPtr)APPCOMMAND_VOLUME_UP);
        }

    }
}
