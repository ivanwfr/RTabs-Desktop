using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Util;
namespace RTabs
{
    static class Program
    {
        // APP RESOURCES {{{
        public  static  string      APP_NAME            = Settings.DESIGNER_APP_NAME;
        public  static  string      APP_TITLE           = Settings.DESIGNER_APP_TITLE;
        public  static  string      APP_INIT_TEXT       = APP_TITLE +" JUST-INSTALLED HELP:\n"
            +"Enter Escape to dismiss this help text\n"
            +"Double-click to toggle maximized panels\n"
            +"...\n"
            +"================================================================================\n"
            ;
        public  static  string      APP_HELP_TEXT       = APP_TITLE +" HELP:\n"
            +"IP-Address\n"
            +"Password\n"
            +"Connect\n"
            +"Firewall rule with administrators privileges\n"
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
            Settings.APP_NAME       = APP_NAME;
            Settings.APP_TITLE      = APP_TITLE;
            Settings.APP_INIT_TEXT  = APP_INIT_TEXT;
            Settings.APP_HELP_TEXT  = APP_HELP_TEXT;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ClientServerInterface csi = new DesignerForm();
            Application.Run( new MainForm(csi) );
        }
    }
}
