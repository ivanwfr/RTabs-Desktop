// using(); {{{
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
using System.Globalization;
//}}}
namespace RTabs // DesignerForm_TAG (200730:15h:13)
{
    public partial class DesignerForm : Form, ClientServerInterface
    {
        // DESIGNER MEMBERS {{{

        private static RTabs.MainForm MainFormInstance;

        private NotePane control_hide;

        private NotePane control_start;
        private NotePane control_stop;
        private NotePane control_stop_server;
        private NotePane control_ADB;
/*
        private NotePane control_send_tabs;
        private NotePane control_get_tabs;
        private NotePane control_send_palettes;
        private NotePane control_get_palettes;
*/

        public  StreamWriter    tcpWriter;
        public  StreamReader    tcpReader;
        public  TcpClient       tcpClient;
        private Thread          cb_thread;

        private bool            connected      = false;
        private bool            server_checked = false;


        //}}}
        // CONSTRUCT {{{
        public DesignerForm()
        {
        }
        //}}}
        // DESIGNER LIFE-CYCLE {{{
        public void ClientServerButtons(MainForm mainForm) // {{{
        {
            MainFormInstance        = mainForm;


            control_start           = MainFormInstance.tabsCollection.update_control(NotePane.CONTROL_NAME_START, NotePane.CONTROL_LABEL_START);
            control_start.TT
                = "Open DESIGNER-SERVER connection"
                ;
            control_stop            = MainFormInstance.tabsCollection.update_control(NotePane.CONTROL_NAME_STOP , NotePane.CONTROL_LABEL_STOP );
            control_stop.TT
                = "Close DESIGNER-SERVER connection"
                ;
            control_hide            = MainFormInstance.tabsCollection.update_control("Hide"                     );
            control_hide.TT
                = "Minimize application window"
                ;
            control_stop_server     = MainFormInstance.tabsCollection.update_control("Stop Server"              );
            control_stop_server.TT
                = "Send the stop-service request to the server (not really useful btw)"
                ;
            control_ADB             = MainFormInstance.tabsCollection.update_control(NotePane.CONTROL_NAME_ADB, NotePane.CONTROL_LABEL_ADB);
            control_ADB.TT
                = NotePane.CONTROL_LABEL_ADB
                ;
/*
            control_send_tabs       = MainFormInstance.tabsCollection.update_control("SEND TABS"                );
            control_get_tabs        = MainFormInstance.tabsCollection.update_control("GET TABS"                 );
            control_send_palettes   = MainFormInstance.tabsCollection.update_control("SEND PALETTES"            );
            control_get_palettes    = MainFormInstance.tabsCollection.update_control("GET PALETTES"             );
*/
        }
        //}}}
        public void ClientServerOnLoad() // {{{
        {
/*
            if(!DesignerHelper.IsUserAdministrator())
            {
                string msg = "This program should be started with administrator"
                    +        " rights to allow control of WINDOWS SYSTEM SETTINGS.";
                MessageBox.Show(msg, Settings.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
*/
            if(Settings.Settings_saved) MainFormInstance.set_settings_state( false );
            else                        MainFormInstance.set_settings_state( true  );
        }
        //}}}
        public void ClientServerOnActivated(object sender, EventArgs e) // {{{
        {
        /*
            if (!connected) return;
            if (!Settings.Send_input_events) return;

            log("EVENTS", "Mirroring keyboard input to server...");
            Thread thread = new Thread(syncModifiers);
            thread.Start();
        */
        }
        //}}}
        public void ClientServerExit(string caller) // {{{
        {
            disconnect();
            Environment.Exit(0);
        }
        //}}}

        public void ClientServer_set_Firewall_Rule() //{{{
        {
            log("EVENTS", "Adding Firewall OUT-RULE on port " + Settings.Port + " for " + Settings.APP_NAME);

            if( DesignerHelper.IsUserAdministrator())
            {
                string result = Netsh.AllowThisProgram("OUT");
                if(result.StartsWith("***") )
                    MessageBox.Show(result, Settings.APP_NAME +" Firewall RULE", MessageBoxButtons.OK, MessageBoxIcon.Error      );
                else
                    MessageBox.Show(result, Settings.APP_NAME +" Firewall RULE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string msg = "AT iNTALL TIME ONLY:\n"
                    + "This program should be started with administrator "
                    + " rights in order to add a new FIREWALL RULE.";
                MessageBox.Show(msg, Settings.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //}}}
        //}}}
        // DESIGNER UI {{{
        public void cancel()// {{{
        {

        }
        // }}}
        public void ClientServer_callback(System.Object caller, string detail)// {{{
        {
            log("EVENTS", "DesignerForm.ClientServer_callback:\n.caller=[" + caller + "]\n.detail=["+ detail +"]");

            // UI BUTTONS (connect disconnect server_stop win_start_menu task_mgr)
            if     (caller == control_hide          ) control_hide_Click();
            else if(caller == control_start         ) control_start_Click();
            else if(caller == control_stop          ) control_stop_Click();
            else if(caller == control_stop_server   ) control_stop_server_Click();
            else if(caller == control_ADB           ) MainFormInstance.control_ADB_Click((NotePane)caller);

            // TYPE_SHORTCUT {{{
            else {
                NotePane np = (NotePane)caller;
                if(np.Type == NotePane.TYPE_SHORTCUT)
                {
                    try {
                        // parse
                        string  cmdLine = np.Tag.ToString();
                        cmdLine         = parse_cmdLine("ClientServer_callback", cmdLine);

                        // send
                        if(cmdLine != "")
                        {
                            // URL
                            if(    cmdLine.StartsWith("http:" )
                                || cmdLine.StartsWith("https:")
                                || cmdLine.StartsWith("file:" )
                            ) {
                                cmdLine = "SHELL "+cmdLine;
                              //cmdLine = cmdLine.Replace(Settings.PROFILES_DIR, Settings.PROFILES_DIR_PATH); // server responsibility
                                log("COMM", "Bare URL cmdLine set to ["+ cmdLine +"]");
                            }
                            if     ( Settings.IsABuiltinCmdLine(cmdLine) ) send_cmd(            cmdLine);
                            else if( Settings.can_parse_KEY_VAL(cmdLine) ) send_cmd("SendKeys", cmdLine);
                            else if(                    "" !=  (cmdLine) ) send_cmd("SendKeys", cmdLine);
                            else if( Settings.IsADashCmdLine   (cmdLine) ) send_cmd("SendDash", cmdLine +" "+ np.Text);
                        }

                    }
                    catch(Exception) { }
                }
            }
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
            connect();
        }
        //}}}
        private void control_stop_Click()// {{{
        {
            log("EVENTS", "control_stop_Click");
            disconnect();
        }
        //}}}
        private void control_stop_server_Click()// {{{
        {
            log("EVENTS", "control_stop_server_Click");

/*
            string msg
                = "!! WARNING !!\n\n"
                + "Stopping the server will leave you unable to reconnect\n"
                + "Are you sure you want to stop the server ?";
            DialogResult yesNo = MessageBox.Show(msg, Settings.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if(yesNo == DialogResult.Yes)
            {
                send_cmd("STOP");
            //  connected = false;
            //  disconnect();
            }
*/

            send_cmd("STOP");

            //disconnect();

        }
        //}}}

//        private void control_start_menu_Click()// {{{
//        {
//            send_cmd("START_MENU");
//        }
//        //}}}
//        private void control_task_mgr_Click()// {{{
//        {
//            send_cmd("CTR_ALT_ESC");
//        }
//        //}}}

        // CLIENT ROLE
        private string parse_cmdLine(string caller, string cmdLine) //{{{
        {
            Log("COMM", "parse_cmdLine(caller=["+ caller +"], cmdLine=["+ cmdLine +"])");
            Settings.ParseTime_Millisecond = Settings.GetUnixTimeMilliSeconds();

            Settings.CmdParser.parse(cmdLine);
            string cmd = Settings.CmdParser.cmd;

            // CMD_PROFILE
            // EVALUATE BUILTINS {{{
            if( Settings.IsABuiltinCmdLine(cmdLine) )
            {
                Logger.Log("COMM", "@@@ EVALUATING BUILTIN CMD @@@");

                // consume .. dispatch later via KEY_VAL
                if(cmd == Settings.CMD_PROFILE)
                {
                    string profile_name = Settings.CmdParser.arg1;
if(profile_name != Settings.CMD_PROFILES_TABLE) MainFormInstance.tabsCollection.tabs_container_SuspendLayout();
                    // dismiss profile UI
                    MainFormInstance.MainForm_cancel_STATE_PROFILE();




// XXX













                    if(profile_name == Settings.CMD_PROFILES_TABLE) {
                        MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.control_profiles_Click(); });
                        cmdLine = "";
                    }
                    else {
                        Settings.LoadProfile( profile_name );

                        // apply changes
                        MainFormInstance.profile_changed("parse_cmdLine("+ caller +")");
                        //  MainFormInstance.adjust_layout_dpi("parse_cmdLine(caller=["+ caller +"], cmdLine=["+ cmdLine +"]");

                        // report loaded profile local version
                        cmdLine += " PRODATE="+ Settings.PRODATE;

                    }
if(profile_name != Settings.CMD_PROFILES_TABLE) MainFormInstance.tabsCollection.tabs_container_ResumeLayout();
                    Log("COMM", "parse_cmdLine: ...forwarding cmdLine=["+ cmdLine +"])");
                    return cmdLine;
                }
            }
            //}}}
            // OR EVALUATE  CMD_KEY_VAL {{{
            else if(Settings.can_parse_KEY_VAL(cmdLine))
            {
                parse_KEY_VAL(caller, cmd, cmdLine);
            }
            //}}}

            return cmdLine;
        }
        //}}}

        // parse_KEY_VAL {{{
        private void parse_KEY_VAL(string caller, string cmd, string cmdLine)
        {
            Log("COMM", "parse_KEY_VAL(caller=["+ caller +"], cmdLine=["+ cmdLine +"])");

            // cmdLine=[POLL  SOURCE=RTabsServer PROFILE=ProfilesCmd PRODATE=1444148022])

            int    loc_prodate = Settings.PRODATE;
            string loc_profile = Settings.PROFILE;

            string rem_profile = Settings.CmdParser.getArgValue("PROFILE", "");
            string rem_source  = Settings.CmdParser.getArgValue("SOURCE" , "");

            Logger.Log("COMM", "rem_profile=["+ rem_profile +"]");

            if((Settings.PROFILE == rem_profile) && (rem_profile != ""))
                sync_PROFILE( rem_profile );

            // SHARE REMOTE PARAMS .. THEN RESTORE LOCALS
            string buf = Settings.set_KEY_VAL("Server.parse_KEY_VAL from "+ Settings.APP_NAME, cmd, cmdLine);
            Settings.PROFILE = loc_profile;
            Settings.PRODATE = loc_prodate;

            if(buf != "")
            {
                Logger.Log("COMM", "@@@ EVALUATING KEY_VAL CMD @@@");
                Logger.Log("COMM", "Settings.PROFILE=["+ Settings.PROFILE +"]");
                // ... RECEIVED FROM SERVER
                if(caller == "tcpReaderTask")
                {
                    if(rem_source != Settings.APP_NAME) // ignore those sent by this very process (should have been dealt with already)
                        MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.adjust_layout_dpi("tcpReaderTask("+ cmdLine +")"); });
                    else
                        Logger.Log("COMM", "@@@ NOT EVALUATING OWN ["+ rem_source +"] KEY_VAL @@@");
                }
                // ... SENT TO SERVER
                else {
                    MainFormInstance.adjust_layout_dpi("parse_KEY_VAL(caller=["+ caller +"], cmdLine=["+ cmdLine +"]");
                    update_COMM_DASH();
                }
            }
            else {
                Logger.Log("COMM", "@@@ KEY_VAL CMD -- NO CHANGE @@@");
            }
        }
        //}}}



        //}}}
        // CONNECT {{{
        private void   connect()// {{{
        {
            if (!MainForm.OnLoad_done) return;
            log("COMM", "connect:");

            // SERVER CONNECTION [tcpClient] f(ip, port, password) .. [LaunchServer] {{{
            string stage = "";
            tcpClient    = null;

            string ip       = get_ip();
            int    port     = get_port();
            string password = get_password();

            string msg = "";
            if((ip.Length == 0) || (password.Length == 0) || (port == 0))
            {
                stage = "ip port password";
                msg   = "Server IP, port and password have not YET been registered by "+ Settings.SERVER_APP_NAME +"\n"
                      + "A first-time Server execution will provide the necessary information";
            }
            else {
                stage = "tcpClient";
                try {
                    log("COMM", "...requesting connection [timeout="+Settings.CONNECT_TIMEOUT+"ms]:");
                    tcpClient = new TcpClient();
                    tcpClient.ConnectAsync(ip, port).Wait( Settings.CONNECT_TIMEOUT );
                }
                catch(Exception) {
                //  msg = "connect stage=[" + stage + "]:\n" + Settings.ExToString(ex);
                //  msg = "connect stage=[" + stage + "]:\n" + ex.Message;
                    msg = "Server is not responding\n";
                }

                if((tcpClient == null) || !tcpClient.Connected)
                {
                    tcpClient = null;
/*
                    msg += "o Could not connect to "+ Settings.APP_TITLE +":\n"
                        -1
                        90-

                        +  "o__ port "+ port             +"\n"
                        +  "o___ within a delay of "+ (int)(Settings.CONNECT_TIMEOUT/1000) +"s\n"
                        +  "o____ ( "+ Settings.SERVER_APP_NAME +" might not be running )\n"
                        +  "..."
                        ;
*/

                }

            }
            // }}}
            // SERVER CONNECTION FAILED - ?(LaunchServer) {{{
            if(tcpClient == null)
            {
                connected = false;

                if( !server_checked )
                {
                    stage = "LaunchServer";
                    //  msg  += "\no______ Do you need to start "+Settings.SERVER_APP_NAME +".exe ?\n";
                    msg  += "\nDo you want to start "+Settings.SERVER_APP_NAME +".exe ?\n";

                    DialogResult yesNo = MessageBox.Show(msg, Settings.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if(yesNo == DialogResult.Yes) {
                        Settings.LaunchServer();
                        Thread.Sleep( Settings.CONNECT_TIMEOUT );
                    }
                    server_checked = true;

                    MainFormInstance.update_COMM_DASH("Listen_Task started");
                }
                else {
                    msg
                    = "Connection attempt failed\n"
                    + "IP ["+Settings.IP+"] is not the right address for your PC:\n"
                    + "1. Use the SERVER [COMM Settings] button to make the correction\n"
                    + "2. then click the DESIGNER [COMM Settings] button when done"
                    ;
                    MessageBox.Show(msg, Settings.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MainFormInstance.update_COMM_DASH( msg );
                }
                return;
            }
            //}}}
            // SERVER IO [tcpWriter] [tcpReader] [tcpReaderTask] {{{
            connected = true;
            try {
                // [tcpWriter] [tcpReader] {{{
                Text    = ip +":"+ port;

                stage = "GetStream";
                Stream tcp_stream                   = tcpClient.GetStream();

                stage = "StreamWriter";
                tcpWriter                           = new StreamWriter(tcp_stream);

                stage = "StreamReader";
                tcpReader                           = new StreamReader(tcp_stream);
                tcpReader.BaseStream.ReadTimeout    = 2000; 

                //}}}
                // [PASSWORD] {{{
                //tcpWriter.Write("PASSWORD " + password + "\n");
                //tcpWriter.Flush();

                //}}}
                // [tcpReaderTask] {{{
                // o prepare logging from another thread {{{
                // o controls created by one thread
                // o cannot be parented to a control
                // o by a different thread }}}
                stage = "send_cmd SIGNIN";
                log("COMM", "SIGNIN");
                send_cmd("SIGNIN "+ Settings.APP_NAME +" "+ password);

                stage = "ThreadStart tcpReaderTask";
                Thread thread = new Thread(new ThreadStart( tcpReaderTask ));
                thread.Start();

                //}}}
            }
            catch(Exception ex) // {{{
            {
                msg = "connect stage=[" + stage + "]:\n" + Settings.ExToString(ex);
                log("COMM", msg);
                update_COMM_DASH();
                MessageBox.Show(msg, Settings.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // }}}
            //}}}
            update_COMM_DASH();
            MainFormInstance.update_COMM_DASH("Listen_Task started");
        }
        //}}}
        private string get_ip() //{{{
        {
            string ip = "";
            try {
            //  ip = DesignerHelper.GetIP( MainFormInstance.text_IP.Text );
                ip = Settings.LoadSetting(Settings.APP_NAME, "ip", Settings.IP);
                log("COMM", ".get_ip: IP=["+ ip +"]");
            }
            catch(Exception ex)
            {
                string msg = "get_ip:\n" + Settings.ExToString(ex);
                log("COMM", msg);
                MessageBox.Show(msg, Settings.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            update_COMM_DASH();
            return ip;
        }
        //}}}
        private int    get_port() //{{{
        {
            int    port = 0;
            try {
            //  port = int.Parse( MainFormInstance.text_port.Text );
                port = int.Parse( Settings.LoadSetting(Settings.APP_NAME, "port", Settings.Port.ToString()));
                log("COMM", ".get_port: IP=["+ port +"]");
            }
            catch(Exception ex)
            {
                string msg = "get_port:\n" + Settings.ExToString(ex);
                log("COMM", msg);
                MessageBox.Show(msg, Settings.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            update_COMM_DASH();
            return port;
        }
        //}}}
        private string get_password() //{{{
        {
            string password = "";
            try {
            //  password = MainFormInstance.text_password.Text;
                password = Settings.LoadSetting(Settings.APP_NAME, "password", Settings.Password);
                log("COMM", ".get_password: IP=["+ password +"]");
            }
            catch(Exception ex)
            {
                string msg = "get_password:\n" + Settings.ExToString(ex);
                log("COMM", msg);
                MessageBox.Show(msg, Settings.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            update_COMM_DASH();
            return password;
        }
        //}}}
        private void   disconnect()// {{{
        {
            log("COMM", "disconnect");

            MainFormInstance.closePopupHelpForm();

            if(!connected)
                return;

            // GRACEFULLY CLOSE CLIENT END
            if( tcpClient.Connected )
                send_cmd("CLOSE");

            connected = false;    // for the aborted thread to know where it comes from

            // Clipboard
            if(cb_thread != null && cb_thread.IsAlive)
                cb_thread.Abort();
            cb_thread = null;

            //this.tcpClient.Close();
            update_COMM_DASH();
            MainFormInstance.update_COMM_DASH("Listen_Task started");
        }
        //}}}
        //}}}
        // PARSE {{{
        private void tcpReaderTask()// {{{
        {
            Log("COMM", "tcpReaderTask");
            try {
                string cmdLine   = "";
                string p         = "";
                int repeat_count = 0;
                while(connected && tcpClient.Connected) {
                    // ReadLine {{{
                    cmdLine ="";
                    try {
                        cmdLine = tcpReader.ReadLine();
                    }
                    catch(System.IO.IOException) {
                        Logger.Log("COMM", "o");
                        Thread.Sleep( Settings.READLINE_FAILED_COOLDOWN );
                    }

                    //}}}
                    try {
                        // Log {{{
                        if(cmdLine != p) {
                            if(repeat_count > 0) {
                                Log("COMM", String.Format("{0,4}x [{1}]", (repeat_count+1), p));
                                repeat_count = 0;
                            }
                            else if(cmdLine !="") {
                                Log("COMM", String.Format("------ [{1}]", (repeat_count+1), cmdLine));
                            }
                            p = cmdLine;
                        }
                        else if(cmdLine != "") {
                            ++repeat_count;
                        }
                        //}}}
                        // parse_POLL - [CMD_PROFILE] [KEY_VAL] {{{
                        if(cmdLine != "") {
                            Settings.CmdParser.parse( cmdLine );
                            if(Settings.CmdParser.arg1 != "ACK")
                                parse_cmdLine("tcpReaderTask", cmdLine);
                        }
                        //}}}
                    } catch(Exception ex) { string msg = "tcpReader:\n" + Settings.ExToString(ex); Log("COMM", "tcpReader.ReadLine=["+ cmdLine +"] *** "+ msg +" ***"); }
                }
            } catch(Exception ex) { string msg = "tcpReader:\n" + Settings.ExToString(ex); Log("COMM", msg); }

            // SERVER-SIDE-DISCONNECT// {{{
            if( !tcpClient.Connected ) {
                Log("COMM", "*** Server connection closed from "+ Settings.SERVER_APP_NAME +" remote host ***");
                connected = false;
            }
            else if( !connected ) {
                Log("COMM", "ooo Server connection closed from "+ Settings.DESIGNER_APP_NAME +" local host ooo");
                connected = false;
            }
            // }}}
            update_COMM_DASH();
            Log("COMM", "tcpReaderTask ... done");
        }
        // }}}
        private string LastClipboardText = "";
        private void handleClipboard() //{{{
        {
            LastClipboardText = ClipboardAsync.GetText();
            cb_thread = new Thread(new ThreadStart(sendClipboard_loop));
            cb_thread.Start();
        }
        //}}}
        private void sendClipboard_loop()// {{{
        {
            //Runs on its own thread and keeps looping to read anything that the server sends us
            try {
                while(true) {
                    if(Settings.Send_input_events)
                        sendClipboard_tick();
                    Thread.Sleep(Settings.CLIPBOARD_INTERVAL);
                }
            }
            catch(Exception ex) {
                if( connected ) {
                    string msg = "sendClipboard_loop():\n" + Settings.ExToString(ex);
                    log("COMM", msg);
                    update_COMM_DASH();
                    MessageBox.Show(msg, Settings.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //}}}
        private void sendClipboard_tick()// {{{
        {
            string txt = ClipboardAsync.GetText();
            if (txt == null) return;
            if (txt.Length == 0) return;
            if (txt == LastClipboardText) return;
            LastClipboardText = txt;

            send_cmd("CLIPBOARD " + txt);
        }
        //}}}
        //}}}
        // DESIGNER REQUEST {{{
        private bool send_cmd(string cmd, string arg)// {{{
        {
            //log("COMM", "send_cmd(cmd=[" + cmd + "],arg=[" + arg + "])");

            return send_cmd(cmd + " " + arg);
        }
        //}}}
        private bool send_cmd(string cmdLine)// {{{
        {
            // log {{{

            if( !connected ) {
                log("COMM", "send_cmd("+cmdLine+"):\n*** NOT CONNECTED TO SERVER ***");
                return false;
            }

            // ...verbose commands have summarize their logs
            if(    !cmdLine.StartsWith(Settings.CMD_TABS_SETTINGS)
                && !cmdLine.StartsWith(Settings.CMD_PALETTES_SETTINGS)
              ) {
                string cmd = "";
                if(cmdLine.Length < 128) {
                    cmd = cmdLine;
                }
                else {
                    int idx = cmdLine.IndexOf(" ");
                    if(idx > 0) cmd = cmdLine.Substring(0, idx);
                    else        cmd = cmdLine.Substring(0,  24) + "...";
                }

                log("COMM", "["+ cmd +"]");
              }

            //}}}
            // send {{{
            try {
                tcpWriter.Write(cmdLine + "\n");
                tcpWriter.Flush();
                return true;
            }
            catch(Exception ex) {
                string msg = "send_cmd(" + cmdLine + "):\n" + Settings.ExToString(ex);
                log("COMM", msg);
                update_COMM_DASH();
                MessageBox.Show(msg, Settings.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
            //}}}
        }
        //}}}
        // send_ACK {{{
        private bool send_ACK(string cmd)
        {
            return send_cmd(cmd +" "+ Settings.ACK);
        }
        //}}}


        // KEY_VAL
        public void dispatch_KEY_VAL() //{{{
        {
            if( !ClientServerIsOnLine() ) { log("COMM", "dispatch_KEY_VAL: *** NOT CONNECTED ***"); return; }

            log("COMM", "DesignerForm.dispatch_KEY_VAL");

            Settings.KEY_VAL_HISTORY += "\n- "+Settings.APP_NAME +".dispatch_KEY_VAL()";

            string argLine = Settings.Get_APP_KEY_VAL();

            send_cmd(Settings.CMD_KEY_VAL, argLine);

            log("COMM", "dispatch_KEY_VAL sent: ["+ argLine.Replace(" ","\n") +"]");
        }
        //}}}

        public  void settings_Changed(string caller) //{{{
        {
            log("EVENTS", "settings_Changed ["+ caller +"]");
        }
        //}}}

        //}}}
        // DESIGNER STATE {{{
        public bool   ClientServerIsOnLine() // {{{
        {
            //log("COMM", "DesignerForm.ClientServerIsOnLine: connected="+ connected);
            return connected;
        }
        //}}}
        public void   ClientServerGetOnLine() // {{{
        {
            log("COMM", "DesignerForm.ClientServerGetOnLine");

            connect();

            // try again if server has just been successfully launched
            if(!connected && server_checked) {
                log("COMM", "DesignerForm.ClientServerGetOnLine [server has been launched] .. trying one more time");
                connect();
            }
            if(MainFormInstance != null) MainFormInstance.set_connection_state("DesignerForm.ClientServerGetOnLine");
            update_COMM_DASH();
            control_start.TextBox.ForeColor = control_start.ForeColor;
        }
        //}}}
        public void   ClientServerGetOffLine() // {{{
        {
            log("COMM", "DesignerForm.ClientServerGetOffLine");
        //  Server responsibility only
        //  if(MainFormInstance != null) MainFormInstance.set_connection_state("DesignerForm.ClientServerGetOffLine");
        //  update_COMM_DASH();
            //control_start.TextBox.ForeColor = Color.Red;
        }
        //}}}
        public int    ClientServerConnectionsCount() // {{{
        {
            log("COMM", "DesignerForm.ClientServerConnectionsCount connected="+ (connected ? 1 : 0));
            return connected ? 1 : 0;
        }
        //}}}
        public void   ClientServerDisconnect() // {{{
        {
            log("COMM", "DesignerForm.ClientServerDisconnect");
            disconnect();
            if(MainFormInstance != null) MainFormInstance.set_connection_state("DesignerForm.ClientServerDisconnect");
            update_COMM_DASH();
            control_start.TextBox.ForeColor = Color.Red;
        }
        //}}}
        public string ClientServerStatus() //{{{
        {
            string localEndpoint_status
                = (((tcpClient != null) && (tcpClient.Connected))
                ?          "connected to "+ get_ip() +":"+ get_port()
                :  "*** NOT connected to "+ get_ip() +":"+ get_port()
                ) + "\n...click for setup"
                ;
            return localEndpoint_status;
        }
        //}}}
        public void update_COMM_DASH() // {{{
        {
            string msg = ""
                + Settings.get_status()
                +          get_status()
// + TabsCollection.XXX    // TODO remove
                ;

            Log("COMM_DASH", msg);
        }
        //}}}
        // }}}
        // PROFILES - sync (save, send) {{{
        private void sync_PROFILE(string profile_name) //{{{
        {
            Log("COMM","@@@  sync_PROFILE("+ profile_name +"):");

            // PROFILE NAME AND VERSION
            int  remote_prodate = int.Parse( Settings.CmdParser.getArgValue("PRODATE", "0") );

            // LOAD its LOCAL VERSION (Designer and SERVER are using the same files)
            // Settings.LoadProfile(profile_name, remote_prodate);

            // XXX DESIGNER AND SERVER MAY SIMULATE A TIME DELTA FOR TESTING PURPOSE
            /* XXX */ //remote_prodate += 120;

            // COMPARE VERSIONS
            bool local_old = (remote_prodate > Settings.PRODATE);
            bool local_new = (remote_prodate < Settings.PRODATE);

            Log("COMM"   , " . PROFILE=["+ profile_name     +"] local_old=["+ local_old +"] local_new=["+ local_new +"]");
            Log("COMM"   , " . ["        + remote_prodate   +"]=[remote_prodate  ] ("+ Settings.Get_time_elapsed( remote_prodate   ) +")");
            Log("COMM"   , " . ["        + Settings.PRODATE +"]=[Settings.PRODATE] ("+ Settings.Get_time_elapsed( Settings.PRODATE ) +")");

            // IF BOTH VERSION ARE THE SAME .. nothing happens
            if(local_new || local_old) {
                string newer     = local_new ? "DESIGNER" : "SERVER";
                string time_diff = Settings.Get_time_diff(Settings.PRODATE, remote_prodate);
                Log("COMM"   ," . ["+ newer +"] has been updated "+ time_diff +" later");
            }
            else {
                Log("COMM"   ," . [SERVER] and [DESIGNER] profiles are the same version");
            }

            // IF NOT
            // this is why this code is executing right now
            // as server is currently expecting
            // a synchronization from this process


            // IF LOCAL OLDER .. [DOWNLOAD REMOTE PROFILE CONTENT]
            if( local_old ) {
                download_PROFILE( profile_name );
                send_ACK( Settings.CMD_POLL );
                // load updated version
                MainFormInstance.Invoke( (MethodInvoker)delegate() { Settings.LoadProfile( profile_name ); });
            }
            // IF LOCAL NEWER .. [UPLOAD LOCAL PROFILE CONTENT]
            if( local_new ) {
                upload_PROFILE( profile_name );
                send_ACK( Settings.CMD_POLL );
            }
            // ELSE, NOTHING TO UPLOAD OR DOWNLOAD
        }
        //}}}
        private void download_PROFILE(string profile_name) //{{{
        {
            Log("COMM"   ,"@@@ download_PROFILE("+ profile_name +"):");

            // READ REMOTE PROFILE LINES
            StringBuilder sb = new StringBuilder();
            string cmd_ACK = Settings.CMD_POLL+" ACK";
            int    line_count = 0;
            string line;

            tcpReader.BaseStream.ReadTimeout    = 2000; 
            try {
                do {
                    line = tcpReader.ReadLine();
                    if((line != null) && (line != cmd_ACK))
                    {
                        sb.Append(line); sb.Append(Environment.NewLine);
                        ++line_count;
                    }
                }
                while( (line != cmd_ACK)
                    && (line != null)
                    && (connected && tcpClient.Connected)
                    );
            }
            catch(Exception ex) {
                Log( "COMM"
                    ,"download_PROFILE:\n"
                    +"*** " + ex.Message
                //  +"***\n"+ ex.StackTrace
                   );
            }
            tcpReader.BaseStream.ReadTimeout    = Timeout.Infinite; 
            string profile_text = sb.ToString();
            Log("COMM"   ,"@@@ [SERVER] "+ line_count +" lines ("+profile_text.Length+" bytes)");

            // SAVE PROFILE INTO A LOCAL FILE
            if(line_count > 1) {
                if( !profile_name.Equals(Settings.CMD_PROFILES_TABLE) ) {
                    string file_path = Settings.ProfilesFolder + Path.DirectorySeparatorChar + profile_name +".txt";
                    Settings.SaveProfile(file_path, profile_text);
                    Log("COMM"   ,"@@@ [DESIGNER] download_PROFILE("+ profile_name +") ...done");
                }
                else {
                    Log("COMM"   ,"@@@ [DESIGNER] download_PROFILE("+ profile_name +" ...does no need saving .. it is rebuilt every time to show the list of saved profiles");
                }
            }
            else {
                Log("COMM"   ,"@@@ [DESIGNER] download_PROFILE("+ profile_name +") ...failed");
            }
        }
        //}}}
        public  void upload_PROFILE(string profile_name) //{{{
        {
            Log("COMM"   ,"@@@  upload_PROFILE("+ profile_name +"):");

            string filePath = Settings.GetProfilePath( profile_name );
            Log("COMM"   ,"@@@ filePath=["+filePath+"]");

            if( !System.IO.File.Exists( filePath ) ) {
                Log("COMM"   , "*** upload_PROFILE("+ profile_name +") file not found: ["+ filePath +"]");
                return;
            }

            string[] lines = File.ReadAllLines( filePath );
            Log("COMM"   ,"@@@ "+ lines.Length +" lines");


            for(int i=0; i< lines.Length; ++i)
                send_cmd( lines[i] );

            Log("COMM"   ,"@@@ "+ profile_name +" settings sent");
        }
        // }}}
        //}}}

        // LOG //{{{
        public string get_status() //{{{
        {
            string reader_status
                = ((connected)
                ? " === "+ Settings.APP_NAME +" IS CONNECTED"
                : " *** "+ Settings.APP_NAME +" NOT CONNECTED")
                +"\n";

            string timestamp = "missing timestamp";
            if(Settings.PRODATE > 0) {
                System.DateTime dt = new System.DateTime(1970, 1, 1, 0, 0, 0);
                dt = dt.AddSeconds( Settings.PRODATE ).ToLocalTime();
                timestamp = dt.ToString("yyyy-MMM-dd HH:mm:ss");
            }
            string profile          = " @@@ ["+ Settings.PROFILE +"] ["+ Settings.PRODATE +"] ["+ timestamp +"]\n";
            string tabs_status      = " === x"+ MainFormInstance.tabsCollection.tabs_Dictionary.Count +" Tabs\n" ;
            string palettes_status  = " === x"+ NotePane.GetColorPaletteCount()                       +" Color palettes\n" ;

            string msg = ""
                + reader_status
                + profile
                + tabs_status
                + palettes_status
                ;

            return msg;
        }
        // }}}
        public string ClientServer_get_BOM() //{{{
        {
            return NotePane.get_BOM();  // color palettes
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
        Logger.Log(caller, msg);
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

