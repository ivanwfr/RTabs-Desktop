// using {{{
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Util;
//}}}
namespace RTabs
{
    public partial class   ServerForm : Form, ClientServerInterface
    {
        // SERVER MEMBERS {{{
        private static RTabs.MainForm MainFormInstance;

        private NotePane control_hide;

        private NotePane control_start;
        private NotePane control_stop;
        private NotePane control_ADB;

        private NotePane control_autostart;
        private NotePane panel_autostart = null;




        //}}}
        // CONSTRUCT {{{
        public ServerForm()
        {
        }
        //}}}
        // SERVER LIFE-CYCLE {{{
        public void ClientServerButtons(MainForm mainForm) // {{{
        {
            MainFormInstance        = mainForm;
            Server.MainFormInstance = MainFormInstance;

            control_start        = MainFormInstance.tabsCollection.update_control(NotePane.CONTROL_NAME_START, NotePane.CONTROL_LABEL_START);
            control_start.TT
                = "Open server connection"
                ;
            control_stop         = MainFormInstance.tabsCollection.update_control(NotePane.CONTROL_NAME_STOP , NotePane.CONTROL_LABEL_STOP );
            control_stop.TT
                = "Close server connection"
                ;
            control_hide         = MainFormInstance.tabsCollection.update_control("Hide"                     );
            control_hide.TT
                = "Minimize application Window into the System Tray"
                ;
            control_autostart    = MainFormInstance.tabsCollection.update_control("Auto-Start"               );
            control_autostart.TT = "Sends the START WITH WINDOWS\nrequest to the Task Scheduler";


            control_ADB         = MainFormInstance.tabsCollection.update_control(NotePane.CONTROL_NAME_ADB, NotePane.CONTROL_LABEL_ADB);
            control_ADB.TT
                = NotePane.CONTROL_LABEL_ADB
                ;





        }
        //}}}
        public void ClientServerOnLoad() // {{{
        {
            if(!ServerHelper  .IsUserAdministrator())
            {
                string msg = "AT INTALL TIME ONLY:\n"
                    + "This program should be started with administrator"
                    + " rights to allow control of WINDOWS SYSTEM SETTINGS"
                    + " such as:\n"
                    + "- Opening Firewall access to the server from android devices\n"
                    + "- Starting the server at Windows startup"
                    ;
                MessageBox.Show(msg, Settings.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if(Settings.Settings_saved) MainFormInstance.set_settings_state( false );
            else                        MainFormInstance.set_settings_state( true  );
        }
        //}}}
        public void ClientServerOnActivated(object sender, EventArgs e) // {{{
        {








        }
        //}}}
        public void ClientServerExit(string caller) // {{{
        {
            Server.StopAndExit();
            Environment.Exit(0);
        }
        //}}}

        public void ClientServer_set_Firewall_Rule() //{{{
        {
            log("EVENTS", "Adding Firewall  IN-RULE on port " + Settings.Port + " for " + Settings.APP_NAME);


            if( ServerHelper.IsUserAdministrator())
            {
                string result = Netsh.AllowThisProgram("IN");
                if(result.StartsWith("***") )
                    MessageBox.Show(result, Settings.APP_NAME +" Firewall RULE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else {
                    MessageBox.Show(result, Settings.APP_NAME +" Firewall RULE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(""
                    + "This program should be started with administrator "
                    + " rights in order to add a new FIREWALL RULE."
                    , Settings.APP_TITLE
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error
                    );

            }
        }
        //}}}
        //}}}
        // SERVER UI {{{
        public void cancel()// {{{
        {
            if( panel_autostart_isVisible() )  control_autostart_Click();
        }

        private bool panel_autostart_isVisible()
        {
            return ((panel_autostart != null) && panel_autostart.Visible);
        }
        // }}}
        public void ClientServer_callback(System.Object caller, string detail)// {{{
        {
            log("EVENTS", "ServerForm.ClientServer_callback:\n.caller=[" + caller + "]\n.detail=["+ detail +"]");

            // UI BUTTONS (connect disconnect server_stop win_start_menu task_mgr)
            if     (caller == control_hide          ) control_hide_Click();
            else if(caller == control_start         ) control_start_Click();
            else if(caller == control_stop          ) control_stop_Click();
            else if(caller == control_autostart     ) control_autostart_Click();
            else if(caller == control_ADB           ) control_ADB_Click();

            // TYPE_SHORTCUT {{{
            //}}}

        }
        //}}}

        private void control_hide_Click()// {{{
        {
            log("EVENTS", "control_hide_Click");
            MainFormInstance.toggleUI("control_hide_Click");
        }
        //}}}

        private void control_start_Click()// {{{
        {
            log("EVENTS", "control_start_Click");
            ClientServerGetOnLine();
        }
        //}}}
        private void control_stop_Click()// {{{
        {
            log("EVENTS", "control_stop_Click");
            ClientServerDisconnect();
        }
        //}}}

        private void control_ADB_Click()// {{{
        {
            log("EVENTS", "control_ADB_Click");

            string ip = "192.168.1.18";
            int  port = 5555;
            control_ADB.Label = "checking ADB on "+ip+":"+port+" ...";

            control_ADB.Label
            ="\n"+ connect_ADB(ip, port++)
            +"\n"+ connect_ADB(ip, port++)
            +"\n"+ connect_ADB(ip, port++)
            ;
        }
        //}}}

        private void control_autostart_Click()// {{{
        {
            log("EVENTS", "control_autostart_Click");

            // DONE ALREADY
            panel_autostart = MainFormInstance.tabsCollection.get_tab_NP( NotePane.PANEL_NAME_AUTOSTART );
            if((panel_autostart != null) && panel_autostart.Visible)
            {
                panel_autostart.Hide();
                return;
            }

            // PERFORM TASK
            if (ServerHelper.IsUserAdministrator())
            {

                // CLEAR PREVIOUS LOG
                if(panel_autostart != null) panel_autostart.Text = "";

                if( ScheduleTask.AddNewTask(Settings.APP_NAME, Settings.APP_NAME + " auto-start") )
                    MessageBox.Show(
                        "Schedule task added for " + Settings.APP_NAME + " auto-start"
                        , Settings.APP_NAME
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Information
                        );

                // SHOW LAST LOG
                panel_autostart = MainFormInstance.tabsCollection.get_tab_NP( NotePane.PANEL_NAME_AUTOSTART );
                if((panel_autostart != null) && !panel_autostart.Visible)
                    panel_autostart.Show();
            }
            else
            {
                MessageBox.Show(
                    "*** "+ Settings.APP_TITLE +" must be \"Run as Administrator\" in order to add a Schedule task"
                    , Settings.APP_NAME
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error
                    );
            }

        }
        //}}}


        //}}}
        //    CONNECT @see Server {{{





        private string connect_ADB(string ip, int port)// {{{
        {
            if (!MainForm.OnLoad_done) return "!MainForm.OnLoad_done";

            log("COMM", "connect_ADB:");


            string          msg = "";
            TcpClient adbClient = null;
            try {
                log("COMM", "...requesting connection [timeout="+Settings.CONNECT_TIMEOUT+"ms]:");
                adbClient = new TcpClient();
                adbClient.ConnectAsync(ip, port).Wait( Settings.CONNECT_TIMEOUT );
            }
            catch(Exception/*ex*/) {
                msg = ip+":"+port+" .. ADB not listening"/*+":\n"+ex.Message*/;
            }

            if( adbClient.Connected )
            {
                adbClient.Close();
                adbClient = null;
                msg = ip+":"+port+" .. ADB IS! LISTENING";
            }

            update_COMM_DASH();
            MainFormInstance.update_COMM_DASH( msg );
            return msg;
        }
        //}}}
        //}}}
        //    PARSE   @see Server {{{
        //}}}
        // SERVER   REQUEST {{{
        private bool send_cmd(string cmd, string arg)// {{{
        {
            //log("EVENTS", "COMM", "send_cmd(cmd=[" + cmd + "],arg=[" + arg + "])");

            return send_cmd(cmd + " " + arg);
        }
        //}}}
        private bool send_cmd(string cmdLine)// {{{
        {
            log("COMM", "send_cmd(cmdLine=[" + cmdLine + "])");

            // TODO ...forward to connected clients
            // see Server parse_DPI
            Server.Dispatch( cmdLine );

            return false;
        }

        //}}}
        public void  read_ACK() //{{{
        {
            log("COMM", "read_ACK()");

            // TODO ...forward to connected clients
        }
        //}}}

        // KEY_VAL
        public void dispatch_KEY_VAL() //{{{
        {
            if( !ClientServerIsOnLine() ) { log("COMM", "dispatch_KEY_VAL: *** NOT CONNECTED ***"); return; }

            log("COMM",   "ServerForm.dispatch_KEY_VAL");

            Settings.KEY_VAL_HISTORY += "\n- "+Settings.APP_NAME +".dispatch_KEY_VAL()";

            string argLine = Settings.Get_APP_KEY_VAL();

            send_cmd(Settings.CMD_KEY_VAL, argLine);

            log("COMM", "dispatch_KEY_VAL: ["+ argLine +"] sent");
        }
        //}}}

        public  void settings_Changed(string caller) //{{{
        {
            log("EVENTS", "settings_Changed ["+ caller +"]");
        }
        //}}}

        //}}}
        // SERVER   STATE {{{
        public bool   ClientServerIsOnLine() // {{{
        {
            //log("COMM", "ServerForm.ClientServerIsOnLine: Server.is_listening="+ Server.is_listening());
            return Server.is_listening(); // || Server.has_a_client();
        }
        //}}}
        public void   ClientServerGetOnLine() // {{{
        {
            log("COMM", "ServerForm.ClientServerGetOnLine");
            Server.Start();
            if(MainFormInstance != null) MainFormInstance.set_connection_state("ServerForm.ClientServerGetOnLine");
            update_COMM_DASH();
            control_start.TextBox.ForeColor = control_start.ForeColor;
        }
        //}}}
        public void   ClientServerGetOffLine() // {{{
        {
            log("COMM", "ServerForm.ClientServerGetOffLine");
            Server.Stop_Listener_Thread();
            if(MainFormInstance != null) MainFormInstance.set_connection_state("ServerForm.ClientServerGetOffLine");
            update_COMM_DASH();
            control_start.TextBox.ForeColor = Color.Red;
        }
        //}}}
        public int    ClientServerConnectionsCount() // {{{
        {
            log("COMM", "ServerForm.ClientServerConnectionsCount Server.get_client_count()="+ Server.get_client_count());
            return Server.get_client_count();
        }
        //}}}
        public void   ClientServerDisconnect() // {{{
        {
            log("COMM", "ServerForm.ClientServerDisconnect");
            Server.StopService();
            if(MainFormInstance != null) MainFormInstance.set_connection_state("ServerForm.ClientServerDisconnect");
            update_COMM_DASH();
            control_start.TextBox.ForeColor = Color.Red;
        }
        //}}}
        public string ClientServerStatus() //{{{
        {
            return Server.get_localEndpoint_status() +"\n...click for setup";
        }
        //}}}
        public void update_COMM_DASH() // {{{
        {
            string msg = ""
                + Settings.get_status()
                + Server  .get_status()
//+ TabsCollection.XXX    // TODO remove
                ;

            Log("COMM_DASH", msg);
        }
        //}}}
        //}}}


        // LOG //{{{
        public string ClientServer_get_BOM() //{{{
        {
            return Server.get_BOM();
        }
        //}}}
        public void log(string caller, string msg)// {{{
        {
            if(MainFormInstance != null)
                MainFormInstance.log(caller, msg);
        }
        //}}}
//        public void set_logging(bool state)// {{{
//        {
//            MainFormInstance.set_logging( state );
//        }
//        //}}}
        public  static void Log(string caller, string msg)// {{{
        {
        //  if(MainFormInstance == null) return;
        //  MainFormInstance.Invoke( (MethodInvoker)delegate() { Logger.Log(caller, msg+"\n"); });
            Logger.Log(caller, msg+"\n");
        }

        // }}}
        public  static void Log_left(string caller, string msg)// {{{
        {
        //  if(MainFormInstance == null) return;
        //  MainFormInstance.Invoke( (MethodInvoker)delegate() { Logger.Log_left(caller, msg); });
            Logger.Log_left(caller, msg);
        }

        // }}}
        public  static void Log_right(string caller, string msg)// {{{
        {
        //  if(MainFormInstance == null) return;
        //  MainFormInstance.Invoke( (MethodInvoker)delegate() { Logger.Log_right(caller, msg); });
            Logger.Log_right(caller, msg);
        }

        // }}}
        //}}}

    }
}

