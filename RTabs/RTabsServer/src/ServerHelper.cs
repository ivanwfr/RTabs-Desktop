// {{{
using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.Security.Principal;

using Util;
using System.Net;
// }}}
namespace RTabs
{
    public static class ServerHelper
    {
        // APP CONSTANTS {{{

        public const int  KEYEVENTF_EXTENDEDKEY     = 0x0001;
        public const int  KEYEVENTF_KEYUP           = 0x0002;

        public const byte VK_CAPITAL                = 0x14;
        public const byte VK_NUMLOCK                = 0x90;
        public const byte VK_SCROLL                 = 0x91;
        public const int  VK_CONTROL                = 0x11;
        public const int  VK_ESCAPE                 = 0x1B;

        public const uint MOUSEEVENTF_ABSOLUTE      = 0x8000;
        public const uint MOUSEEVENTF_HWHEEL        = 0x01000;  // wheel button tilted
        public const uint MOUSEEVENTF_LEFTDOWN      = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP        = 0x0004;
        public const uint MOUSEEVENTF_MIDDLEDOWN    = 0x0020;
        public const uint MOUSEEVENTF_MIDDLEUP      = 0x0040;
        public const uint MOUSEEVENTF_MOVE          = 0x0001;
        public const uint MOUSEEVENTF_RIGHTDOWN     = 0x0008;
        public const uint MOUSEEVENTF_RIGHTUP       = 0x0010;
        public const uint MOUSEEVENTF_WHEEL         = 0x0800;
        public const uint MOUSEEVENTF_XDOWN         = 0x0080;
        public const uint MOUSEEVENTF_XUP           = 0x0100;

        // }}}
        // KEYBOARD {{{

        // TODO: use SendInput instead of keybd_event:
        // @See ../../PROJECTS/DX1Utility_141/Key/SpecialKeyPlayer.cs

        public static void SetModifiers(string args)// {{{
        {
            Log("SetModifiers("+ args +"):");

            bool cl = (args.IndexOf(Settings.KEYS_CAPSLOCK  ) >= 0);
            bool nl = (args.IndexOf(Settings.KEYS_NUMLOCK   ) >= 0);
            bool sl = (args.IndexOf(Settings.KEYS_SCROLLLOCK) >= 0);
            Log("cl=["+ cl +"] nl=["+ nl +"] sl=["+ sl +"]");

            if(Control.IsKeyLocked( Keys.CapsLock ) != cl) ServerHelper.SetCapsLock  (cl);
            if(Control.IsKeyLocked( Keys.NumLock  ) != nl) ServerHelper.SetNumLock   (nl);
            if(Control.IsKeyLocked( Keys.Scroll   ) != sl) ServerHelper.SetScrollLock(sl);
        }
        // }}}

        public static void SetCapsLock(bool down)// {{{
        {
            int dwFlags = KEYEVENTF_EXTENDEDKEY | (down ? 0 : ServerHelper.KEYEVENTF_KEYUP);
            NativeMethods.keybd_event(VK_CAPITAL, 0x00, dwFlags, IntPtr.Zero);
        }
        // }}}
        public static void SetNumLock(bool down)// {{{
        {
            int dwFlags = KEYEVENTF_EXTENDEDKEY | (down ? 0 : ServerHelper.KEYEVENTF_KEYUP);
            NativeMethods.keybd_event(VK_NUMLOCK, 0x00, dwFlags, IntPtr.Zero);
        }
        // }}}
        public static void SetScrollLock(bool down)// {{{
        {
            int dwFlags = KEYEVENTF_EXTENDEDKEY | (down ? 0 : ServerHelper.KEYEVENTF_KEYUP);
            NativeMethods.keybd_event(VK_SCROLL , 0x00, dwFlags, IntPtr.Zero);
        }
        // }}}

        public static void ShowStartMenu()// {{{ ^{Esc} or LWin
        {
            Log("ShowStartMenu");
            // shows the windows start button
            NativeMethods.keybd_event((byte)Keys.LWin, 0, ServerHelper.KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            NativeMethods.keybd_event((byte)Keys.LWin, 0, ServerHelper.KEYEVENTF_EXTENDEDKEY | ServerHelper.KEYEVENTF_KEYUP, IntPtr.Zero);
        }
        // }}}
        public static void ShowTaskmanager()// {{{ ^+{Esc}
        {
            Log("ShowTaskmanager");
            try { System.Diagnostics.Process.Start(@"C:\Windows\system32\taskmgr.exe"); }catch { ;}
        }
        // }}}
        public static void ShowMetro()// {{{
        {
            // METRO SIDEBAR

            NativeMethods.keybd_event((byte)Keys.LWin, 0, ServerHelper.KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            NativeMethods.keybd_event((byte)Keys.C   , 0, ServerHelper.KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            NativeMethods.keybd_event((byte)Keys.C   , 0, ServerHelper.KEYEVENTF_EXTENDEDKEY | ServerHelper.KEYEVENTF_KEYUP, IntPtr.Zero);
            NativeMethods.keybd_event((byte)Keys.LWin, 0, ServerHelper.KEYEVENTF_EXTENDEDKEY | ServerHelper.KEYEVENTF_KEYUP, IntPtr.Zero);

            Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width - 30, Screen.PrimaryScreen.Bounds.Height /2);
            Thread.Sleep(20);
        }
        // }}}
        //}}}
        // MOUSE {{{

        public static void ScrollHorizontal(uint Amount)// {{{
        {
            NativeMethods.mouse_event(ServerHelper.MOUSEEVENTF_HWHEEL, 0, 0, Amount, UIntPtr.Zero);
        }
        // }}}
        public static void ScrollVertical(uint Amount)// {{{
        {
            NativeMethods.mouse_event(ServerHelper.MOUSEEVENTF_HWHEEL, 0, 0, Amount, UIntPtr.Zero);
        }
        // }}}
        //}}}
        // HOST {{{
        public static bool AddDesktopShortcut()// {{{
        {
            Log("AddDesktopShortcut");

            // exe -> desktop lnk path {{{
            FileInfo FInfo      = new FileInfo( Application.ExecutablePath );

            string FileNameLnk  = @"C:\Users\"
                + Environment.UserName
                + @"\Desktop\"
                + FInfo.Name.ToLower().Replace(".exe", "") + ".lnk";

            if(File.Exists(FileNameLnk)) {
                Log("...FileNameLnk ["+ FileNameLnk +"] is already there");
                return true;
            }

            //}}}
            // shell-create [desktop shorcut] {{{
            string powershell_command = ""
                +"$WshShell                  = New-Object -comObject WScript.Shell"          + Environment.NewLine
                +"$Shortcut                  = $WshShell.CreateShortcut('"+ FileNameLnk +"')"+ Environment.NewLine
                +"$Shortcut.TargetPath       = '"+           Application.ExecutablePath +"';"+ Environment.NewLine
                +"$Shortcut.Description      = 'Runs the program with admin rights';"        + Environment.NewLine
                +"$Shortcut.WorkingDirectory = '"+              Application.StartupPath +"';"+ Environment.NewLine
                +"$Shortcut.WindowStyle      = 1;"                                           + Environment.NewLine
                +"$Shortcut.Save()"                                                          + Environment.NewLine
                ;
            string ScriptResults = Settings.ExecuteCommandAsAdmin( powershell_command );

            //}}}
            // set administrator rights + return diagnostic {{{
            bool diag = false;

            if(    (ScriptResults.Length == 0)
                && File.Exists(FileNameLnk)
              ) {
                using (FileStream fs = new FileStream(FileNameLnk, FileMode.Open, FileAccess.ReadWrite))
                {
                    fs.Seek(21, SeekOrigin.Begin);
                    fs.WriteByte(0x22);
                }
                diag = true;

            }
            //}}}
            Log("...diag=["+ diag +"]");
            return diag;
        }
        // }}}
        public static bool IsUserAdministrator()// {{{
        {
            // If the server is not run with admin rights
            // then windows will not allow mouse clicks
            // on some windows pages
            // like the task-manager

            try {
                WindowsIdentity  user      = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch(Exception ex) {
                Log("IsUserAdministrator(): "+ex.Message);
            }
            return false;
        }
        // }}}
        //}}}
        // LOG {{{
        private static void Log(string Msg)// {{{
        {
            Server.Log("COMM", Msg);
        }
        // }}}
        //}}}

    }
    static class NativeMethods
    {
        // [keybd_event] Synthesizes keystroke (virtual-key code, hardware scan code {{{
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, IntPtr dwExtraInfo);  

        // }}}
        // [mouse_event] synthesizes mouse motion and button click {{{
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);

        // }}}
    }
}
