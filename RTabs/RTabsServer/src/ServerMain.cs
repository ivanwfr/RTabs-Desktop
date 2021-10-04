using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using Util;
namespace RTabs
{
    static class Program
    {
        // APP RESOURCES {{{
        public  static  string      APP_NAME            = Settings.SERVER_APP_NAME;
        public  static  string      APP_TITLE           = Settings.SERVER_APP_TITLE;
        public  static  string      APP_INIT_TEXT       = APP_TITLE +" JUST-INSTALLED HELP:\n"
            +"Enter Escape to dismiss this help text\n"
            +"Double-click to toggle maximized panels\n"
            +"...\n"
            +"================================================================================\n"
            ;
        public  static  string      APP_HELP_TEXT       = APP_TITLE +" HELP:\n"
            +"WINDOWS FIREWALL:\n"
            +" TCP port  which will need to be allowed by windows firewall.\n"
            +" The \"Firewall\" button update this rule for this server program and the currently selected port number.\n"
            +" (...needs administrator privileges.)\n"
            +"\n"
            +"ROUTER FIREWALL:\n"
            +" Client connections through a router will require an additional NAT rule on the router's firewall as well for the selected port number to allow incoming TCP packets to reach the server.\n"
            +"\n"
            +"AUTOSTART:\n"
            +" The server can be started when user logs on to the computer by pressing the \"Auto Start\" button which will create a scheduled task to start the service.\n"
            +"\n"
            +"HIDE:\n"
            +" The \"Hide\" button removes the server window from the desktop.\n"
            +"\n"
            +"ADMINISTRATOR PRIVILEGED FEATURES:\n"
            +" Most windows operating system settings will work only if the server is started in elevetated privilege mode.\n"
            +"...\n"
            +"================================================================================\n"
            ;

        //}}}
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using(Mutex mutex = new Mutex(false, "Global\\ew"+APP_NAME))
            {
                if(!mutex.WaitOne(0, false)) {
                    MessageBox.Show(APP_NAME +" is already running", APP_NAME +" launch warning");
                    return;
                }
                Settings.APP_NAME       = APP_NAME;
                Settings.APP_TITLE      = APP_TITLE;
                Settings.APP_INIT_TEXT  = APP_INIT_TEXT;
                Settings.APP_HELP_TEXT  = APP_HELP_TEXT;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ClientServerInterface csi = new ServerForm();
                Application.Run( new MainForm(csi) );
            }
        }
    }
}
