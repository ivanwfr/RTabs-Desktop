// using {{{
using System;
using System.Collections.Generic;
using System.Net ;
using System.Text;
using System.Security.Principal;
using System.IO;
using System.Windows.Forms;

using Util;
// }}}
namespace RTabs
{
    public static class DesignerHelper
    {
        public static bool IsIP4Address(string hostName)// {{{
        {//123.123.123.123
            if( hostName.Split('.').Length             !=  4
                ||  hostName.Length                     > 23
                ||  hostName.ToLower().IndexOf(".com")  >  1
              )
                return false;

            if(hostName.Length > 15)
                return false;

            IPAddress IP;
            return IPAddress.TryParse(hostName, out IP);
        }
        // }}}
        public static string GetIP(string hostNameOrAddress)// {{{
        {
            Log("GetIP("+ hostNameOrAddress +")");
            try {
                // IPAddress {{{

                IPHostEntry ipHostEntry = Dns.GetHostEntry( hostNameOrAddress );
                Log("...IPHostEntry.AddressList.Length: "+ ipHostEntry.AddressList.Length);

                IPAddress ipAddress = null;
                foreach(IPAddress ipa in ipHostEntry.AddressList)
                {
                    Log(" "+ ipa);
                    if(ipa.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) // i.e. IPV4
                    {
                        ipAddress = ipa;
                        Log("...resolved ipAddress=["+ ipa +"]");
                    }
                }

                if(ipAddress != null) return ipAddress.ToString();
                else                  return hostNameOrAddress;

                //}}}
            }
            catch(Exception ex) {// {{{
                Log("GetHostEntry(hostNameOrAddress=["+ hostNameOrAddress +"] Exception:\n"+ex.Message);
            } // }}}
            return "";
        }
        // }}}
        public static bool IsUserAdministrator()// {{{
        {
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception ex)
            {
                MessageBox.Show("DesignerHelper.IsUserAdmistrator: "+ex.Message);
            }
            return false;
        }
        // }}}
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
            string powershell_command
                ="$WshShell                  = New-Object -comObject WScript.Shell"          + Environment.NewLine
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

        private static void Log(string msg)// {{{
        {
            Logger.Log("COMM", ".."+typeof(DesignerHelper).Name +": "+ msg+"\n");
        }
        // }}}
    }
}
