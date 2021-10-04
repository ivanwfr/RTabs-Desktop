// using {{{
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Media;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Util;
//using WindowsInput;
// }}}
namespace RTabs
{
    public class Server : IDisposable
    {
        // CLASS {{{
        public  static MainForm     MainFormInstance    = null;

        // accept up to 3 connections
        private static Server       Server_1            = null;
        private static Server       Server_2            = null;
        private static Server       Server_3            = null;

        private static int          FAILURES_MAX        = 1;
        private static string       SERVERLOG           = "SERVERLOG";

        // listener thread
        private static TcpListener  Server_TcpListener  = null;
        private static Thread       Listener_Thread     = null;
        private static string       Listener_Status     = "";
        private static bool         Listener_Running    = false;
        private static bool         Server_exiting      = false;

        // }}}
        // INSTANCE {{{
        private bool                    Running             = false;

        private string                  queued_cmdLine      = "";

        private string                  last_sent_palettes  = "";
        private string                  last_sent_tabs      = "";
        private string                  last_sent_key_val   = "";

        private bool                    loggedIn            = false;
        private bool                    sleeping            = false;

        private        Socket           acceptedSocket      = null;
        private static Socket           AcceptedSocket_1    = null;
        private static Socket           AcceptedSocket_2    = null;
        private static Socket           AcceptedSocket_3    = null;

        private        string           server_ID           = "SERVER";

        private         DateTime        keepAwakeTime       = DateTime.Now;

        private         Stream          networkStream       = null;
        private         StreamReader    networkStreamReader = null;
        private         Thread          readerThread        = null;
        private         Thread          writerThread        = null;

        // }}}
        // LISTEN {{{
        public  static void Start()// {{{
        {
            // SERVER START STATE
            Server_exiting = false;
            if(Listener_Thread != null) {
                Log("* Server.Start: ...already listening *");
                return;
            }
            Listener_Running = true;

            // SERVER RESOURCES
            if(Settings.PalettesDict.Count < 1)  Load_DESIGNER_PALETTES();
            if(Settings.TabsDict    .Count < 1)  Load_DESIGNER_TABS();

            // LISTEN CONNECTION REQUESTS
            if(Listener_Thread == null) {
                Listener_Thread = new Thread(new ThreadStart( Listen_Task ));
                Listener_Status= "STARTING";
                Listener_Thread.Start();
            }
            else {
                Log("* Server.Start: ...already listening *");
            }

            Log(SERVERLOG, "== [STARTED]  =====================");
        }
        // }}}
        public  static void Stop_Listener_Thread()// {{{
        {
            Log("Stop_Listener_Thread:");

            // stop accepting
            if( Server_TcpListener != null)
            {
                Log("...Server_TcpListener Close:");
                Server_TcpListener.Server.Close();

                Log("...Server_TcpListener Stop:");
                Server_TcpListener.Stop();
            }

            try {
                if(Listener_Thread != null) {
                    MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.update_COMM_DASH("Stop_Listener_Thread ...Listener_Thread Interrupt"); });
                    Log("...Listener_Thread Interrupt:");
                    Listener_Status= "BEING INTERRUPTED";
                    Listener_Thread.Interrupt();
                    Listener_Thread     = null;
                }
            } catch(Exception) { }

            Log(SERVERLOG, "== [INTERRUPTED] ==================\n");
        }
        // }}}
        public  static void StopAndExit()// {{{
        {
            Log("StopAndExit:");
            Server_exiting = true;

            StopService();
        }
        // }}}
        public  static void StopService()// {{{
        {
            Log("StopService:");
            Listener_Running = false;

            bool Server_1_running = ((Server_1 != null) && Server_1.Running);
            bool Server_2_running = ((Server_2 != null) && Server_2.Running);
            bool Server_3_running = ((Server_3 != null) && Server_3.Running);

            if(!Server_1_running && !Server_2_running && !Server_3_running)
                Log("STOP: ...no accepted connection still running");

            Log("STOP: Activating Stop_server_task...");
            new Thread(new ThreadStart( Stop_server_task )).Start();
        }
        // }}}
        public  static bool is_listening()// {{{
        {
            return (Listener_Thread != null) && Listener_Running;
        }
        // }}}
        // private
        // Stop_server_task {{{
        private static void Stop_server_task()
        {
            Log("Stop_server_task:");

            Stop_Listener_Thread();

            // stop reading accepted connection requests
            if((Server_1 != null) && Server_1.Running) { Log("...Server_1.close_connection"); Server_1.close_connection(); }
            if((Server_2 != null) && Server_2.Running) { Log("...Server_2.close_connection"); Server_2.close_connection(); }
            if((Server_3 != null) && Server_3.Running) { Log("...Server_3.close_connection"); Server_3.close_connection(); }

            Log(SERVERLOG, "== [STOPPED]  =====================\n");
        }
        //}}}
        // Listen_Task {{{
        private static int  accept_Count    = 0;
        private static void Listen_Task()
        {
            Log("\n===");
            Log("Listen_Task start:");
            Log("...Settings.IP......=["+ Settings.IP       +"]");
            Log("...Settings.Port....=["+ Settings.Port     +"]");
            Log("...Settings.Password=["+ Settings.Password +"]");
            try {
                // IPAddress {{{

                string hostNameOrAddress = Settings.IP;

                IPHostEntry ipHostEntry = Dns.GetHostEntry( hostNameOrAddress );
                Log("...IPHostEntry.AddressList.Length: "+ ipHostEntry.AddressList.Length);

                IPAddress ipAddress = null;
                foreach(IPAddress ipa in ipHostEntry.AddressList)
                {
                    Log(" "+ ipa);
                    if(ipa.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) // i.e. IPV4
                    {
                        ipAddress = ipa;
                        Log("...resolved ipAddress=["+ ipAddress +"]");
                        break;
                    }
                }

                if(ipAddress == null) ipAddress = IPAddress.Parse( hostNameOrAddress );

                Log("...selected ipAddress=["+ ipAddress +"]");
                //}}}
                // TcpListener {{{
                Log(SERVERLOG, "== [Running Listen_Task on "+ipAddress +":"+Settings.Port +"]");

                Server_TcpListener  = new TcpListener(ipAddress, Settings.Port);

            //  Only one usage of each socket address (protocol/network address/port) is normally permitted
            //  :!start explorer "http://blogs.msdn.com/b/dgorti/archive/2005/09/18/470766.aspx"
            //  ... can last for 4mn
            //  HKLM\System\CurrentControlSet\Services\Tcpip\Parameters\TCPTimedWaitDelay

            //  Server_TcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            //  An attempt was made to access a socket in a way forbidden by its access permissions

                Log("...["+ Server_TcpListener +"]:\n===");


                // OPTIONS
                Server_TcpListener.AllowNatTraversal(true);

            //  Server_TcpListener.Server.SetIPProtectionLevel(IPProtectionLevel.EdgeRestricted );
                Server_TcpListener.Server.SetIPProtectionLevel(IPProtectionLevel.Unrestricted   );
            //  Server_TcpListener.Server.SetIPProtectionLevel(IPProtectionLevel.Restricted     );
            //  Server_TcpListener.Server.SetIPProtectionLevel(IPProtectionLevel.Unspecified    );

                Server_TcpListener.Start();

                //}}}
                // Listener_Thread loop : [LISTEN ACCEPT handleConnection] {{{
                MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.update_COMM_DASH("Listen_Task started"); });

                while(Listener_Running && (Listener_Thread != null))
                {
                    Listener_Status= "LISTENING";
                    // ACCEPT
                    Socket _acceptedSocket = Server_TcpListener.AcceptSocket();
                    Log(SERVERLOG, " = [Connection accepted: "+ _acceptedSocket.RemoteEndPoint.ToString()+"]");

                    int available_server_num;
                    if      (Server_1 == null || !Server_1.Running) available_server_num = 0;
                    else if (Server_2 == null || !Server_2.Running) available_server_num = 1;
                    else if (Server_3 == null || !Server_3.Running) available_server_num = 2;
                    else                                            available_server_num = 3;

//                    // REPLY {{{
//                    if(MainFormInstance != null)
//                    {
//                        string msg = "Connection request #"+(available_server_num+1)+" received by "+MainFormInstance.get_APP_NAME();
//                        Log(SERVERLOG, " = [Replying with \""+msg+"\"]");
//                        byte[] bytes = ASCIIEncoding.ASCII.GetBytes(msg + Environment.NewLine);
//                        _acceptedSocket.Send(bytes, bytes.Length, SocketFlags.None);
//                    }
//                    //}}}

                    // HANDLE
                    if      (available_server_num == 0) { Server_1 = new Server(); Server_1.handleConnection( _acceptedSocket ); ++accept_Count; }
                    else if (available_server_num == 1) { Server_2 = new Server(); Server_2.handleConnection( _acceptedSocket ); ++accept_Count; }
                    else if (available_server_num == 2) { Server_3 = new Server(); Server_3.handleConnection( _acceptedSocket ); ++accept_Count; }
                    else {
                        Log(SERVERLOG, "***[CANNOT ACCEPT MORE THAN "+available_server_num+" CLIENT CONNECTIONS]***");
                    }
                }
                //}}}
            }
            catch(Exception ex) // {{{
            {
                if(     ex.GetType() == typeof(ThreadInterruptedException)) {
                    Log("*** Listen_Task INTERRUPT: "+ ex.Message);
                    Listener_Status = "*** INTERRUPTED";

                }
                else if(ex.GetType() == typeof(ThreadAbortException)) {
                    Log("*** Listen_Task ABORTED: "+ ex.Message);
                    Listener_Status = "*** ABORTED";

                }
                else if(ex.GetType() == typeof(SocketException)) {
                    Log("*** Listen_Task SOCKET: "+ ex.Message);
                    Listener_Status = "*** "+ex.Message;

                }
                else {
                    Log("...ex:\n"+ex.ToString());
                    Listener_Status = "*** "+ex.Message;
                }
            }
            //}}}
            finally { //{{{
                Log("Listen_Task ...done\n===");
                Listener_Thread = null;
            }
            // }}}
        }
         // }}}
        //}}}
        // SERVICE {{{
        public static bool has_a_client()// {{{
        {
            if (Server_1 != null && Server_1.Running) return true;
            if (Server_2 != null && Server_1.Running) return true;
            if (Server_3 != null && Server_1.Running) return true;
            return false;
        }
        // }}}
        public static int get_client_count()// {{{
        {
            int client_count = 0;
            if(Server_1 != null && Server_1.Running) client_count += 1;
            if(Server_2 != null && Server_1.Running) client_count += 1;
            if(Server_3 != null && Server_1.Running) client_count += 1;
            return client_count;
        }
        // }}}
        // private
        private void handleConnection(Socket acceptedSocket)// {{{
        {
            Log( "handleConnection("+acceptedSocket+"):");

            // COMM
            this.acceptedSocket = acceptedSocket;
            networkStream       = new NetworkStream( acceptedSocket );

            // THREAD
            Running             = true;
            loggedIn            = false;
            sleeping            = false;
            readerThread        = new Thread(new ThreadStart( Server_Read_Task ));
            readerThread.Start();
        }
        // }}}
        private void Server_Read_Task()// {{{
        {
            Log("Server_Read_Task...\n"
                +  "===");

            // SERVER-ID ... TO BE LATER RENAMED AFTER CLIENT-ID
            if     (this == Server_1) { server_ID  = "SERVER-1"; AcceptedSocket_1 = acceptedSocket; }
            else if(this == Server_2) { server_ID  = "SERVER-2"; AcceptedSocket_2 = acceptedSocket; }
            else if(this == Server_3) { server_ID  = "SERVER-3"; AcceptedSocket_3 = acceptedSocket; }

            Log(SERVERLOG,       " = ["+server_ID+"] [READING REQUESTS] ===");


            int failures = 0;
            string cmdLine;
            networkStreamReader  = new StreamReader( networkStream );
            while(  Running            // ... interrupted
                && !Server_exiting     // ... scheduled stop
                && (failures < FAILURES_MAX)
                ) {

                if(sleeping) Thread.Sleep(500);

                try {
                    //  Log("Server_Read_Task Reading client:\n");
                    Log(">");
                    if(queued_cmdLine != "")
                    {
                        cmdLine         = queued_cmdLine;
                        queued_cmdLine  = "";
                    }
                    else {
                        cmdLine         = networkStreamReader.ReadLine();
                        keepAwakeTime   = DateTime.Now;
                    }

                    if(cmdLine != null)
                    {
                        Thread.BeginCriticalRegion();
                        parse_cmdLine( cmdLine.Trim() );
                        Thread.EndCriticalRegion();
                        MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.update_COMM_DASH("Server_Read_Task ...parse_cmdLine"); });
                    }
                    else {
                        ++failures;
                        Log("Server_Read_Task: (cmdLine == null) failures=["+failures+"] ***Thread.Sleep(500)****");
                        Thread.Sleep(500);
                    }
                }
                catch(ThreadInterruptedException ex)
                {
                    Log("*** Server_Read_Task Interrupted: "+ ex.Message +"\n");
                }
                catch(ThreadAbortException  ex)
                {
                    Log("*** Server_Read_Task Aborted "+ ex.Message +"\n");
                    Log("*** Canceling with ResetAbort\n");
                    Thread.ResetAbort();
                }
                catch(Exception ex)
                {
                    ++failures;
                    Log( "Server_Read_Task:\n" // failures=["+failures+"]
                        +"*** " + ex.Message
                        //+"***\n"+ ex.StackTrace
                       );

                    Thread.Sleep(500);
                }
            }

            close_connection();
            networkStreamReader.Close();
            networkStreamReader = null;

            Log("Server_Read_Task ...done");
            MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.update_COMM_DASH("Server_Read_Task ...done"); });
        }
        // }}}
        private void close_connection()// {{{
        {
            Log("close_connection ["+server_ID+"]:");

            Running = false;

            Thread.Sleep(200); // ? ever care to join ?

            if(acceptedSocket.IsBound) acceptedSocket.Close();

            if(networkStream != null)
            {
                networkStream.Close();
                networkStream.Dispose();
                networkStream = null;
            }
            // MUTEX RELEASE {{{
            if( Settings.UseMutex ) {
                if(Settings.ServiceMutexOwner == server_ID)
                {
                    Log("RELEASING OWNED MUTEX");
                    Settings.Log_SMutex_Release("Server", server_ID);
                    Settings.SM_releasedBy( server_ID );
                    Settings.ServiceMutex.ReleaseMutex();
                }
            }
            //}}}

            MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.update_COMM_DASH("close_connection"); });
            Log("# # # # # # # # # # # # # # # # # # # #\n\n");
            Log(SERVERLOG, "== ["+server_ID+"] [CLOSED] =============\n");


            if(readerThread != null) readerThread.Abort();
            if(writerThread != null) writerThread.Abort();

        }
        // }}}
        //}}}
        // PARSE {{{
        // private void parse_cmdLine(string cmdLine) {{{
        private string cmd_prev;
        private void parse_cmdLine(string cmdLine)
        {
            // CmdParser: cmd arg1 args[...] // {{{
            Settings.ParseTime_Millisecond = Settings.GetUnixTimeMilliSeconds();

            Settings.CmdParser.parse(cmdLine);
            string cmd      = Settings.CmdParser.cmd.ToUpper();
            string arg1     = Settings.CmdParser.arg1;
            string argLine  = Settings.CmdParser.argLine;

            string method_id = "@@@ "+server_ID+".parse_cmdLine";

            // }}}
            // MUTEX ACQUIRE {{{
            if( Settings.UseMutex ) {
                try {
                    Settings.Log_SMutex_Wait("Server", server_ID);
                    if( Settings.ServiceMutex.WaitOne( Settings.WAIT_SERVICE_MUTEX_TIMEOUT_MS ) )
                        Settings.SM_aquiredBy( server_ID );
                    else
                        Settings.Log_SMutex_WaitFailed("Server", server_ID);
                } catch(Exception ex) {
                    Log(SERVERLOG, "*** "+ server_ID +":\n"+ Settings.ExToString(ex)); 
                }
            }
            //}}}
                // command and arguments {{{
                if(cmd == Settings.CMD_KEY_VAL) Log(method_id+":\n"+".cmdLine=["+cmdLine.Replace(" ","\n")+"]");
                else                            Log(method_id+":\n"+".cmdLine=["+cmdLine+"]");

                //}}}
                // APPLICATION WINDOW RESHAPING [Up Right Down Left Full] {{{
                string corner = "";
                if(    cmd.StartsWith( Settings.CMD_RUN    )
                    || cmd.StartsWith( Settings.CMD_BROWSE )
                    || cmd.StartsWith( Settings.CMD_SHELL  )
                  ) {
                    if     (cmd.EndsWith("_UL")) corner = "UL";
                    else if(cmd.EndsWith("_UR")) corner = "UR";
                    else if(cmd.EndsWith("_DL")) corner = "DL";
                    else if(cmd.EndsWith("_DR")) corner = "DR";

                    else if(cmd.EndsWith("_L" )) corner =  "L";
                    else if(cmd.EndsWith("_C" )) corner =  "C";      // CENTER VERTICAL
                    else if(cmd.EndsWith("_R" )) corner =  "R";

                    else if(cmd.EndsWith("_U" )) corner =  "U";
                    else if(cmd.EndsWith("_M" )) corner =  "M";
                    else if(cmd.EndsWith("_D" )) corner =  "D";

                    else if(cmd.EndsWith("_F" )) corner =  "F";      // FULLSCREEN

                    else if(cmd.IndexOf ("_")>0) corner =  "CENTER"; // CENTER VERTICAL & HORIZONTAL

                    if     (cmd.StartsWith( Settings.CMD_BROWSE )) cmd = Settings.CMD_BROWSE;
                    else if(cmd.StartsWith( Settings.CMD_RUN    )) cmd = Settings.CMD_RUN;
                    else if(cmd.StartsWith( Settings.CMD_SHELL  )) cmd = Settings.CMD_SHELL;
                }

                //}}}
                // CONNECTION {{{
                if     (cmd ==     Settings.CMD_OK                  ) { /* FIXME then what ? */ }
                else if(cmd ==     Settings.CMD_PASSWORD            ) { parse_PASSWORD(cmd, arg1); }
                else if(cmd ==     Settings.CMD_CLOSE               ) { /* release mutex first... */ }

                // }}}
                // TABS {{{
                else if(cmd ==     Settings.CMD_KEY_VAL             ) { parse_KEY_VAL          (cmd, argLine); }
                else if(cmd ==     Settings.CMD_SIGNIN              ) { parse_SIGNIN           (cmd, argLine); }

                else if(cmd ==     Settings.CMD_TABS_LOAD           ) { load_DESIGNER_TABS         (cmd         ); } // load   on   client request
                else if(cmd ==     Settings.CMD_TABS_CLEAR          ) {         parse_TABS_CLEAR   (cmd         ); } // init   on designer request
                else if(cmd ==     Settings.CMD_TABS_SETTINGS       ) {         parse_TABS_SETTINGS(cmd, argLine); } // load from designer feed
                else if(cmd ==     Settings.CMD_TABS_GET            ) {          send_TABS_SETTINGS(cmd         ); } // reply  to   client request

                else if(cmd ==     Settings.CMD_PALETTES_CLEAR      ) {     parse_PALETTES_CLEAR   (cmd         ); } // init   on designer request
                else if(cmd ==     Settings.CMD_PALETTES_GET        ) {      send_PALETTES_SETTINGS(cmd         ); } // reply  to   client request
                else if(cmd ==     Settings.CMD_PALETTES_LOAD       ) { load_USER_PALETTES         (cmd         ); } // load   on   client request
                else if(cmd ==     Settings.CMD_PALETTES_SETTINGS   ) {     parse_PALETTES_SETTINGS(cmd, argLine); } // load from designer feed

                else if(cmd ==     Settings.CMD_PROFILE             ) {      sync_PROFILE          (cmd, arg1);    }
                else if(cmd ==     Settings.CMD_PROFILE_DOWNLOAD    ) {      upload_PROFILE        (cmd, arg1);    }


            //  else if(cmd ==     Settings.CMD_PALETTES_DISPATCH   ) {  dispatch_PALETTES_CHANGED();          } // push   on designer request
            //  else if(cmd ==     Settings.CMD_TABS_DISPATCH       ) {  dispatch_TABS_CHANGED();              } // push   on designer request

            //  else if(cmd ==     Settings.CMD_POLL                ) {  send_POLL_BUFFER( cmd ); }
                // }}}
                // PROCESS BROWSE RUN SHELL SENDKEYS SENDDASH {{{
                else if((cmd == Settings.CMD_BROWSE) // {{{ BROWSE_UL BROWSE_UR BROWSE_DL BROWSE_DR
                    ||  (cmd == Settings.CMD_RUN   ) // {{{ RUN_UL    RUN_UR    RUN_DL    RUN_DR
                ) {
                    Log(SERVERLOG, " . ["+server_ID+"] cmd=["+ cmd +"] corner=["+corner+"] argLine=["+argLine+"]");
                    if(argLine.IndexOf(" #") >= 0) {
                        argLine = new Regex(" *#.*").Replace(argLine, "");
                        Log(SERVERLOG, "===# removed .. argLine=["+argLine+"]");
                    }

                    try {
                        Process proc = Settings.ExecuteProcess( argLine );
                        if((proc != null) && (corner != ""))
                                Settings.SetWindowGeometry(corner, proc);
                        // *** Process has exited, so the requested information is not available.***
                    } catch(Exception ex) { Log( "\n***\n*** "+ ex.Message +"\n***"); }
                    send_ACK( cmd );
                }
                // }}}
                else if((cmd == Settings.CMD_SHELL ) // {{{ SHELL_UL  SHELL_UR  SHELL_DL  SHELL_DR
                ) {
                    Log(SERVERLOG, " . ["+server_ID+"] cmd=["+ cmd +"] corner=["+corner+"] argLine=["+argLine+"]");
                    if(argLine.IndexOf(" #") >= 0) {
                        argLine = new Regex(" *#.*").Replace(argLine, "");
                        Log(SERVERLOG, "===# removed .. argLine=["+argLine+"]");
                        if(argLine == "") {
                            Log("=== "+ cmd +" ignoring: ["+ cmdLine +"]");
                            send_ACK( cmd );
                            return;
                        }
                    }
                    try {
                        Process proc = Settings.ExecuteShell  ( argLine );
                    // NO PROCESS TO SET WINDOW GEOMETRY FOR
                    //  if((proc != null) && (corner != ""))
                    //        Settings.SetWindowGeometry(corner, proc);

                    } catch(Exception ex) { Log( "\n***\n*** "+ ex.Message +"\n***"); }
                    send_ACK( cmd );
                }
                // }}}
                else if((cmd == Settings.CMD_SENDKEYS     )  // SENDKEYS or SENDINPUT {{{
                    ||  (cmd == Settings.CMD_SENDINPUT    )
                    ||  (cmd == Settings.CMD_SENDKEYSTEXT )
                    ||  (cmd == Settings.CMD_SENDINPUTTEXT)
                    ) {
                    if(argLine.IndexOf(" #") >= 0) {
                        argLine = new Regex(" *#.*").Replace(argLine, "");
                        Log(SERVERLOG, "===# removed .. argLine=["+argLine+"]");
                        if(argLine == "") {
                            Log("=== "+ cmd +" ignoring: ["+ cmdLine +"]");
                            send_ACK( cmd );
                            return;
                        }
                    }

                    Log(SERVERLOG, " . ["+server_ID+"] "+cmd+" ["+argLine+"]");

                    if( parse_INTERNAL(cmd, argLine) ) {
                        Log(SERVERLOG, " . SENDKEYS HAS BEEN INTERPRETED AS AN INTERNAL COMMAND");
                        if(queued_cmdLine != "")
                            Log(SERVERLOG, " . queued_cmdLine=["+ queued_cmdLine +"]");
                    }
                    else if((argLine.ToUpper() == "^+{ESC}") || (argLine == "+^{ESC}")) {
                        ServerHelper.ShowTaskmanager();
                        send_ACK( cmd );
                    }
                    else if((argLine.ToUpper() ==  "^{ESC}")                          ) {
                        ServerHelper.ShowStartMenu();
                        send_ACK( cmd );
                    }
                    //}}}
                    // real SendKeys or SendInput {{{
                    else {
                        // EXTRA
                        argLine = argLine.Replace("{SPACE}", " ");
                        argLine = argLine.Replace("{space}", "  ");
                        // SendKeys or SendInput {{{
                        string send_err = "";

                        // XXX
                        // .argLine=[^Lhttps://thepiratebay.cr/search/Homeland{%}20S06E01{%}20x265/0/7/0{ENTER}{SLEEP 500}^R]
                        // XXX
                        Log(method_id+":\n"+".argLine=["+argLine +"]");
                        string s = argLine;
////{{{
///*
//:!start explorer "https://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys(v=vs.110).aspx"
//*/
//
//                    //  string pattern;
//                    //  string pattern =      @"([^{])?"  + @"([+^%~()])"   +  @"([^}])?";
//                    //  s        = Regex.Replace(" "+s+" ", pattern, @"$1{$2}$3").Trim();
//
//                        // UTF8
//                    //  pattern = @"((U\+)|(\\u))([0-9A-Za-z]{3+})";
//                        
//                    //  s = new Regex(@"(U\+)([0-9A-Za-z]{3+})").Replace(s, @"%!($2)"); // ALT+NUMLOCK
//                    //  s = new Regex(@"((U\+)|(\\u))([0-9A-Za-z]*)").Replace(s, @"1=[$1] 2=[$2] 3=[$3] 4=[$4]");
//                    //  s = new Regex(@"((U\+)|(\\u))([0-9A-Za-z]*)").Replace(s, @"%!($4)");
///*
//                        string hex = new Regex(@"((U\+)|(\\u))([0-9A-Za-z]*)").Replace(s, @"$4");
//                        if(hex != "") {
//                            int dec = Convert.ToInt32(hex, 16);
//                            s = @"%!("+ dec +")";
//                        }
//*/
////}}}

                        MatchCollection matches = Regex.Matches(s, @"(U\+|\\u|0x)[0-9A-Za-z]{3,4}");
                        string result = ""; //"matches.Count=["+ matches.Count +"]";
                        int    idx    =  0; // beginning of s
                        string hex;
                        int    dec;
                        foreach (Match match in matches)
                        {
                            foreach (Capture capture in match.Captures)
                            {
                                // head
                                if(capture.Index > idx)
                                    result += s.Substring(idx, capture.Index-idx);

                                // hex
                                hex = capture.Value.Substring(2);
                                dec = Convert.ToInt32(hex, 16);

                                // enc
                                result += @"%!("+ dec +")";
                            //  result += "\n["+ capture.Index +"==="+ capture.Value +"==="+ dec +"]\n";

                                // tail
                                idx = capture.Index + capture.Length;
                            }
                        }
                        // tail
                        if(idx < s.Length)   result += s.Substring(idx);

                        if(result != "") s = result;

                        // XXX
                        // ..................................................................0x265...........................
                        // ..................................................................vvvvv...........................
                        // .argLine=[^Lhttps://thepiratebay.cr/search/Homeland{%}20S06E01{%}20x265/0/7/0{ENTER}{SLEEP 500}^R]
                        // .......s=[^Lhttps://thepiratebay.cr/search/Homeland{%}20S06E01{%}2%!(613)/0/7/0{ENTER}{SLEEP 500}^R]
                        // ..................................................................^^^^^^^...........................
                        // XXX
                        if(s != argLine)
                            Log("\n"+".......s=["+s       +"]");

                        if(    (cmd == Settings.CMD_SENDKEYS    ) // {{{
                            || (cmd == Settings.CMD_SENDKEYSTEXT)
                          ) {
                            // DECODE CHARACTERS WITH A SPECIAL MEANING .. (SYNTHETIC KEYBOARD EVENTS)
                            s = s.Replace("\\n"            , "\n"    )
                                . Replace("{ENTER}"        , "\n"    )
                                . Replace("{ESC}"          , "\u001B")
                                ;
                        Log("\n... s=["+s +"]");

                            s = new Regex(@"{SLEEP *[0-9]*}").Replace(s,@"");
                        Log("\n... s=["+s +"]");

                            // TODO WHEN is this relevant ?
                            // s = new Regex(@"([\+^%~(){}])").Replace(s,@"{$1}");

                            try { SendKeys.SendWait( s ); } catch(Exception ex) { send_err = " "+ ex.Message; }

                            // KEYBOARD INPUT
                            //  Log("=== send_keybd_event()" );
                            //  Util.NativeMethods.send_keybd_event( argLine );

                        }
                        // }}}
                        else {
                            // SYNTHETIC KEYBOARD EVENTS
                            Log("=== SendInput.SendString:");
                            s = s.Replace("\\n","{ENTER}");

                            // DELAY TO ALLOW SWITCHING FOCUS FROM EMBEDED EMULATORS {{{
                            if(server_ID.IndexOf("t03g") >= 0) {
                                int delay = 2000;
                                Log("=== SLEEPING("+ delay +") for ["+ server_ID +"]");
                                if(sleeping) Thread.Sleep(delay);
                            }
                            //}}}

                            try { SendInput.SendString( s );    } catch(Exception ex) { send_err = " "+ ex.Message; }
                        }
                        //}}}
                        if(send_err != "") Log("=== send_err=["+ send_err +"]"); 
                        send_ACK(cmd + send_err);
                    }
                    //}}}
                }
                // }}}
                else if( cmd ==     Settings.CMD_SENDDASH     ) {// {{{
                    Log(SERVERLOG, " . ["+server_ID+"] cmd=["+cmd+"] argLine=["+argLine+"]");

                    Log(method_id +":\n"+".argLine=["+argLine +"]");
                    send_ACK( cmd );
                }
                // }}}
                //}}}
                // LOGGING HIDE CLEAR STOP {{{
                else if(cmd == Settings.CMD_LOGGING      ) { parse_LOGGING(cmd, arg1); }
                else if(cmd == Settings.CMD_HIDE         ) { parse_HIDE   (cmd, arg1); }
                else if(cmd == Settings.CMD_CLEAR        ) { parse_CLEAR  (cmd      ); }
                else if(cmd == Settings.CMD_STOP         ) { parse_STOP   (cmd      ); }
            //  else if(cmd == Settings.CMD_BEEP         ) { SystemSounds.Asterisk.Play(); }
                //}}}
            // TODO MOUSE CLICK {{{
            //  else if(cmd == "CLIPBOARD"    ) { ClipboardAsync.SetText( cmdLine.Substring(cmd.Length+1) ); }
            //  else if(cmd == "LCLICK"       ) { ServerHelper.mouse_event(ServerHelper.MOUSEEVENTF_LEFTDOWN  | ServerHelper.MOUSEEVENTF_LEFTUP , (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, UIntPtr.Zero); }
            //  else if(cmd == "LDOWN"        ) { ServerHelper.mouse_event(ServerHelper.MOUSEEVENTF_LEFTDOWN                                    , (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, UIntPtr.Zero); }
            //  else if(cmd == "LUP"          ) { ServerHelper.mouse_event(ServerHelper.MOUSEEVENTF_LEFTUP                                      , (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, UIntPtr.Zero); }
            //  else if(cmd == "RCLICK"       ) { ServerHelper.mouse_event(ServerHelper.MOUSEEVENTF_LEFTDOWN  | ServerHelper.MOUSEEVENTF_LEFTUP , (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, UIntPtr.Zero); }
            //  else if(cmd == "RCLICK"       ) { ServerHelper.mouse_event(ServerHelper.MOUSEEVENTF_RIGHTDOWN | ServerHelper.MOUSEEVENTF_LEFTUP , (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, UIntPtr.Zero); }
            //  else if(cmd == "RDOWN"        ) { ServerHelper.mouse_event(ServerHelper.MOUSEEVENTF_RIGHTDOWN                                   , (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, UIntPtr.Zero); }
            //  else if(cmd == "RUP"          ) { ServerHelper.mouse_event(ServerHelper.MOUSEEVENTF_RIGHTUP                                     , (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, UIntPtr.Zero); }

            //}}}
            // TODO MODIFIERS {{{
            //  else if(cmd == "SET_MODIFIERS") { ServerHelper.SetModifiers(arg1); }
            //  else if(cmd == "{CAPSLOCK}"   ) { ServerHelper.SetCapsLock (true); }
            //  else if(cmd == "{NUMLOCK}"    ) { ServerHelper.SetNumLock  (true); }

            //  else if(cmd == "METRO"        ) { ServerHelper.ShowMetro();       }
            //}}}
                /* TODO MOUSE POINTER {{{
                else if(cmd == "M") {
                    try {
                        int xPos        = int.Parse(cmdLine.Substring(1, cmdLine.IndexOf(' ')));
                        int yPos        = int.Parse(cmdLine.Substring(   cmdLine.IndexOf(' '), cmdLine.Length - cmdLine.IndexOf(' ')));
                        Cursor.Position = new Point(xPos, yPos);
                    }
                    catch(Exception ex) { Log("*** "+ method_id +": Error:\n"+ ex); }
                }
                */ //}}}
                // ...UNPARSED {{{
                else if(cmdLine.Length > 0)
                {
                    Log("***["+server_ID+"] [ACK] [UNPARSED COMMAND] ["+cmdLine+"]");
                    send_ACK( cmdLine );
                }
                // }}}
                // MUTEX RELEASE {{{
                cmd_prev = cmd;

                if( Settings.UseMutex ) {
                    if(    (Settings.ServiceMutexOwner == server_ID)
                        && (Settings.ServiceMutexCount > 0         )
                      ) {
                        Settings.Log_SMutex_Release("Server", server_ID);
                        Settings.SM_releasedBy( server_ID );
                        Settings.ServiceMutex.ReleaseMutex();
                    }
                }

                if(cmd == Settings.CMD_CLOSE) parse_CLOSE(cmd); // differed after mutex relase

                //}}}
        }
        // }}}

        private void parse_SIGNIN(string cmd, string argLine) // {{{
        {
            Log("parse_SIGNIN("+cmd+","+argLine+")");

            // argline=[device password]
            string msg;
            string[] args    = argLine.Split(' ');
            if(args.Length != 2)
            {
                msg = "*** SIGNIN("+cmd+","+argLine+"): expected argline=[device password]";
                Log(msg);
                send_ACK(cmd + msg);
                close_connection();
                return;
            }
            string device_ID = args[0];
            string password  = args[1];

            if( !check_PASSWORD( password ) )
            {
                msg = cmd +" "+ argLine +" WRONG PASSWORD";
                Log(msg);
                send_ACK( msg );            // SIGNIN SGP512 ******** WRONG PASSWORD ACK
                close_connection();
                return;
            }

            // handle session
            if     (this == Server_1) { server_ID = "S1_"+ device_ID; }
            else if(this == Server_2) { server_ID = "S2_"+ device_ID; }
            else if(this == Server_3) { server_ID = "S3_"+ device_ID; }

            // REPLY WITH SERVER_ID {{{
            string Server_ID = Environment.MachineName;
            string buf       = cmd +" "+ Server_ID +" "+ Settings.SUBNET +" "+ Settings.MAC;
            int    bytes     = sendLine( buf );

            bytes += send_ACK( cmd );       // SIGNIN SERVER-1 255.255.255.0 CB-4E-8D-91-BB ACK

            //}}}
            msg = "*** SIGNIN("+cmd+","+argLine+"): "+buf;
            Log(buf);
        }
        //}}}
        private void parse_PASSWORD(string cmd, string password)// {{{
        {
            Log("parse_PASSWORD("+cmd+","+password+")");

            if( check_PASSWORD( password ) ) {
                send_ACK( cmd );
            }
            else {
                send_ACK( cmd + "*** wrong password ***");
                close_connection();
            }
        }
        // }}}
        private bool check_PASSWORD(string password)// {{{
        {
            Log("check_PASSWORD("+password+")");

            if(password.Trim() == Settings.Password || Settings.Password.Length == 0) {
                loggedIn = true;
                Log(" . ["+server_ID+"] [PASSWORD OK] loggedIn=["+ loggedIn +"]");
                return true;
            }
            else {
                Log("*** ["+server_ID+"] WRONG PASSWORD password=["+password+"] expected=["+Settings.Password+"] ***");
                return false;
            }
        }
        // }}}
        private void parse_LOGGING(string cmd, string arg1)// {{{
        {
            bool state = (arg1.Length > 0)
                ?        (arg1.ToLower() == "true")
                :        !Settings.LOGGING;
            send_ACK( cmd );

            if(state) {
                SetLogging( state );
                Log(SERVERLOG, " . ["+server_ID+"] [LOGGING]=["+ Settings.LOGGING +"]");
            }
            else {
                Log(SERVERLOG, " . ["+server_ID+"] [LOGGING]=["+ Settings.LOGGING +"]");
                SetLogging( state );
            }
        }
        // }}}
        private void parse_HIDE(string cmd, string arg1)// {{{
        {
            Log(SERVERLOG, " . ["+server_ID+"] parse_HIDE(arg1=["+ arg1 +"])");

            if(MainFormInstance == null || Server_exiting)
                return;

            if(     arg1.ToLower() == "true" )
                MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.showUI  ("parse_HIDE"); });
            else if(arg1.ToLower() == "false")
                MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.hideUI  ("parse_HIDE"); });
            else
                MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.toggleUI("parse_HIDE"); });

            send_ACK( cmd );
        }
        // }}}
        private void parse_CLEAR(string cmd)// {{{
        {
            Log(SERVERLOG, " . ["+server_ID+"] ["+ cmd +"]");

            if(MainFormInstance == null || Server_exiting)
                return;

            MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.clear_app_panels_content(); });

            send_ACK( cmd );
        }
        // }}}
        private bool parse_INTERNAL(string cmd, string argLine)// {{{
        {
            Log(SERVERLOG, " . ["+server_ID+"] ["+ cmd +"] ["+ argLine +"]");

            if(MainFormInstance == null || Server_exiting)
                return false;

            // THOSE ARE SENT VIA CMD_SENDKEYS FROM ANDROID // {{{
            if(Settings.can_parse_KEY_VAL( argLine ))
            {
                parse_KEY_VAL(cmd, argLine);
                return true;
            }
            else {
                Log(SERVERLOG, "...Settings.can_parse_KEY_VAL(argLine) returned false");
            }
            // }}}
            // EMBEDED after cmd=[SENDKEYS] .. skip cmd then re-parse argLine {{{
            if(    (argLine          == Settings.CMD_TABS_GET         )
                || (argLine          == Settings.CMD_PALETTES_GET     )
                || (argLine.StartsWith( Settings.CMD_PROFILE    + " "))
              ) {
                queued_cmdLine = argLine;
                return true;
            }
            //}}}

            return false;
        }
        // }}}
        private void parse_STOP(string cmd)// {{{
        {
            Log(SERVERLOG, " . ["+server_ID+"] [STOP]");
            send_ACK( cmd );
        //  StopService();
        //  MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.stop("parse_STOP"); }); // don't remember why this ?
            StopService(); // restored this on (160725)
        }
        // }}}
        private void parse_CLOSE(string cmd)// {{{
        {
            Log(SERVERLOG, " . ["+server_ID+"] [CLOSE]");

            send_ACK( cmd );
            close_connection();
        }
        // }}}
        //}}}
        // PALETTES {{{
        public static void Load_DESIGNER_PALETTES() //{{{
        {
            Settings.ReopenSettings(Settings.DESIGNER_APP_NAME);

            Settings.PalettesDict.Clear();

            string name = "";
            try {
                for(int num=1; num < Settings.PALETTES_MAX; ++num)
                {
                    string key  = "PALETTE."+num;
                    string argLine = Settings.LoadSetting(Settings.DESIGNER_APP_NAME, key, "");
                    if(argLine == "") break;

                    name = argLine.Split(',')[0];
                    Settings.PalettesDict.Add(name, argLine);
                    NotePane.LoadColorPaletteLine( argLine );
                }
            }
            catch(Exception ex) { Log("*** Load_DESIGNER_PALETTES: name=["+ name +"]\n***\n"+ ex.Message +"\n***"); }

            Log("Load_DESIGNER_PALETTES: "+Settings.PalettesDict.Count +" Color Palettes");
            MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.profile_changed("Load_DESIGNER_PALETTES()"); });
        }
        // }}}
        public  void load_USER_PALETTES(string cmd) //{{{
        {
            Load_DESIGNER_PALETTES();
            send_ACK( cmd );
        }
        //}}}
        public  void parse_PALETTES_CLEAR(string cmd) // {{{
        {
            //Log(SERVERLOG, " . ["+server_ID+"] ["+ cmd +"]");

            Settings.PalettesDict.Clear();

            send_ACK( cmd );

            Log("parse_PALETTES_CLEAR (x"+Settings.PalettesDict.Count+")");
        }
        //}}}
        public  void parse_PALETTES_SETTINGS(string cmd, string argLine)// {{{
        {
            // argLine isarg {{{
            string name = argLine.Split(',')[0];
            // }}}
            Settings.PalettesDict.Add(name, argLine);

            send_ACK( cmd );
        }
        // }}}
        private void send_PALETTES_SETTINGS(string cmd) //{{{
        {
            if     ((this == Server_1) && (Server_1.Running)) send_PALETTES_SETTINGS(Server_1, cmd);
            else if((this == Server_2) && (Server_2.Running)) send_PALETTES_SETTINGS(Server_2, cmd);
            else if((this == Server_3) && (Server_3.Running)) send_PALETTES_SETTINGS(Server_3, cmd);
        }
        //}}}
        public  void send_PALETTES_SETTINGS(Server server, string cmd)// {{{
        {
            // SEND DATA {{{
            int palettes_count = 0;
            int bytes = 0;
            if(Settings.PalettesDict.Count > 0)
            {
                bytes += sendLine(server.networkStream, cmd );
                foreach(var item in Settings.PalettesDict) {
                    bytes += sendLine(server.networkStream, (string)Settings.PalettesDict[item.Key]);
                    ++palettes_count;
                }
            }
            bytes += send_ACK(server.networkStream, cmd);

            last_sent_palettes = "Color palettes: x"+ palettes_count;
            //}}}
            Log(SERVERLOG,"["+server.server_ID+"] send_PALETTES_SETTINGS (x"+  Settings.PalettesDict.Count +") "+last_sent_palettes);
        }
        // }}}
        //}}}
        // TABS {{{
        private static void Load_DESIGNER_TABS() //{{{
        {
            Log("Load_DESIGNER_TABS:");

            Settings.TabsDict.Clear();

            Settings.ReopenSettings(Settings.DESIGNER_APP_NAME);

            int id_max = int.Parse(  Settings.LoadSetting(Settings.DESIGNER_APP_NAME,"UserTabIDmax", "0") );
            if( id_max < 1) id_max = Settings.USER_TABS_MAX;

            Log("...id_max=["+id_max +"]");

            string type, tag, zoom, xy_wh, text, tt;
            StringBuilder sb = new StringBuilder();
            char          fs = NotePane.TABVALUE_SEPARATOR;

            for(int i= 1; i <= id_max; ++i) {
                // Load tabs settings from DESIGNER REGISTRY {{{
                string tab_name     = NotePane.PANEL_NAME_USR + i;
                string sub_key_name = "TAB."+ tab_name;
                string settings     = Settings.LoadSetting(Settings.DESIGNER_APP_NAME, sub_key_name, "");

                //}}}
                // Serialize tabs into Settings.TabsDict {{{
                if(settings != "")
                {
                    string[] a = settings.Split( NotePane.TABVALUE_SEPARATOR );
                    type   = a[0].Substring(5); // 0.5  type=
                    tag    = a[1].Substring(4); // 1.4   tag=
                    zoom   = a[2].Substring(5); // 2.5  zoom=
                    xy_wh  = a[3].Substring(6); // 3.6 xy_wh=
                    text   = a[4].Substring(5); // 4.5  text=
                    tt     = a[5].Substring(3); // 5.3    tt=

                    // SERIALIZE [tab_name type tag zoom xy_wh text] {{{
                    sb.Clear();
                    sb.Append("TAB."+ tab_name);     // no APP_NAME required at this point
                    //..............654321...value
                    sb.Append(fs +  "type="+ type ); // 0 type
                    sb.Append(fs +   "tag="+ tag  ); // 1 tag
                    sb.Append(fs +  "zoom="+ zoom ); // 2 zoom
                    sb.Append(fs + "xy_wh="+ xy_wh); // 3 xy_wh
                    sb.Append(fs +  "text="+ text ); // 4 text
                    sb.Append(fs +    "tt="+ tt   ); // 5 tt

                    //}}}
                    String txt = sb.ToString();
//Log("@@@ Load_DESIGNER_TABS: ("+ txt +")");
                    Settings.TabsDict.Add(tab_name, txt);
                }
                //}}}
            }
            Log("Load_DESIGNER_TABS: "+Settings.TabsDict.Count +" TABS");
            MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.profile_changed("Load_DESIGNER_TABS()"); });

// TAB SETTINGS LINE {{{
//
// FOUR SEPARATORS USED:  = | . ,
//
// ______________________TAB.NAME = VALUE______________________________________________________________________________________
// [APP_NAME]____________TAB.NAME =
//                                  0000000000000 1111111111111111111111111 22222222 33333333333333333 4444444444444 5555555555
// ___________________.tab_name__ = 12345________|1234_____________________|12345___|123456___,__,__,_|12345________|123_______
//                                = type=________|tag=_____________________|zoom=___|xy_wh=___ __ __ _|text=________|tt=_______
// [RTabsDesigner] TAB.panel_usr1 = type=SHORTCUT|tag=SHELL C:/TMP/file.txt|zoom=1.5|xy_wh=130,23,16,5|text=file.txt|tt=tooltip
//                 TAB.panel_usr1 = type=SHORTCUT|tag=SHELL C:/TMP/file.txt|zoom=1.5|xy_wh=130,23,16,5|text=file.txt|tt=tooltip
//                                  type=SHORTCUT|tag=SHELL C:/TMP/file.txt|zoom=1.5|xy_wh=130,23,16,5|text=file.txt|tt=tooltip
//                                                tag=SHELL C:/TMP/file.txt|zoom=1.5|xy_wh=130,23,16,5|text=file.txt|tt=tooltip
//                                                                          zoom=1.5|xy_wh=130,23,16,5|text=file.txt|tt=tooltip
//                                                                                   xy_wh=130,23,16,5|text=file.txt|tt=tooltip
//                                                                                                     text=file.txt|tt=tooltip
//                                                                                                                   tt=tooltip
//}}}
        }
        // }}}
        private  void load_DESIGNER_TABS(string cmd) //{{{
        {
            Load_DESIGNER_TABS();
            send_ACK( cmd );
        }
        //}}}

        public  void parse_TABS_CLEAR(string cmd) // {{{
        {
            //Log(SERVERLOG, " . ["+server_ID+"] ["+ cmd +"]");

            Settings.TabsDict.Clear();

            send_ACK( cmd );
        }
        //}}}
        public  void parse_TABS_SETTINGS(string cmd, string argLine)// {{{
        {
            // argLine parse args {{{
            // STRINGIFICATION TEMPLATE {{{

            // o-----------------------------------o
            // [../BAK/REGISTRY/RTabsDesigner.reg] |
            // |                                   |
            // | "TAB.Add.xy_wh"="490,10,100,80"   |
            // |                                   |
            // | "TAB.Add.type"="BUTTON"           |
            // |                                   |
            // | "TAB.Add.zoom"="1"                |
            // |                                   |
            // o-----------------------------------o

            // o----------TYPE,value------------XY_WH,value-------------------ZOOM,value--o
            // |                                                                          |
            // |  TAB.Add.type,BUTTON | TAB.Add.xy_wh,490,10,100,80 | TAB.Add.zoom,1      |
            // |                                                                          |
            // o--------------------------------------------------------------------------o

            //}}}

            // 0 type
            // 1 tag
            // 2 zoom
            // 3 xy_wh
            // 4 text

            string[] settings = argLine.Split( NotePane.TABVALUE_SEPARATOR );
            string   name, type, xy_wh, zoom, tag, text;// {{{
            name    = (settings.Length > 0) ? settings[0] : "";     // TAB.Add.----  , BUTTON
            type    = (settings.Length > 1) ? settings[1] : "";     // TAB.---.type  , BUTTON
            tag     = (settings.Length > 2) ? settings[2] : "";     // TAB.---.tag   , SHELL somecommand
            zoom    = (settings.Length > 3) ? settings[3] : "";     // TAB.---.zoom  , 1
            xy_wh   = (settings.Length > 4) ? settings[4] : "";     // TAB.---.xy_wh , 490,10,100,80
            text    = (settings.Length > 5) ? settings[5] : "";     // TAB.---.text  , Label
            // }}}
            string[] key_val;// {{{
            key_val =  name.Split('.'); try {  name = key_val[1];                                              } catch(Exception) {}
            key_val =  type.Split(','); try {  type = key_val[1];                                              } catch(Exception) {}
            key_val =   tag.Split(','); try {   tag = key_val[1];                                              } catch(Exception) {}
            key_val =  zoom.Split(','); try {  zoom = key_val[1];                                              } catch(Exception) {}
            key_val = xy_wh.Split(','); try { xy_wh = key_val[1]+","+key_val[2]+","+key_val[3]+","+key_val[4]; } catch(Exception) {}
            key_val =  text.Split(','); try {  text = key_val[1];                                              } catch(Exception) {}
            // }}}
            // }}}
            Settings.TabsDict.Add(name, argLine);
/*// {{{
            Log(SERVERLOG
                ,"["+server_ID+"]"
                + " "+ String.Format("[TABS_SETTINGS {0,2}]name=[{1,24}] type=[{2,8}] xy_wh=[{3,16}] zoom=[{4}] tag=[{5}] text=[{6}]"
                    ,                  Settings.TabsDict.Count    , name        , type       , xy_wh        , zoom     , tag       , text
                    ));
*/ // }}}
            send_ACK( cmd );
        }
        // }}}

        private void send_TABS_SETTINGS(string cmd) //{{{
        {
            if     ((this == Server_1) && (Server_1.Running)) send_TABS_SETTINGS(Server_1, cmd);
            else if((this == Server_2) && (Server_2.Running)) send_TABS_SETTINGS(Server_2, cmd);
            else if((this == Server_3) && (Server_3.Running)) send_TABS_SETTINGS(Server_3, cmd);
        }
        //}}}
        private static int  send_PROFACK_Count       = 0;
        private static int  send_TABS_Count          = 0;
        private static int  send_TABS_SETTINGS_Times = 0;
        public  void send_TABS_SETTINGS(Server server, string cmd)// {{{
        {
            // prepend current designer graphic parameters
            int bytes = 0;
            if(Settings.TabsDict.Count > 0)
            {
                ++send_TABS_SETTINGS_Times;

                string argLine      = Settings.Get_APP_KEY_VAL();
                bytes += sendLine(server.networkStream, cmd +" "+ argLine);
                last_sent_key_val   = argLine;

                int tabs_count = 0;
                foreach(var item in Settings.TabsDict)
                {
                    string s = (string)Settings.TabsDict[ item.Key ];

                    if(s.IndexOf("SHORTCUT") > 0) // FIXME - SHORTCUT only
                    {
                        bytes += sendLine(server.networkStream, (string)Settings.TabsDict[item.Key]);
                        ++tabs_count;
                    }
                }
                send_TABS_Count += tabs_count;
                last_sent_tabs   = "Tabs: x"+ tabs_count;
            }
            bytes += send_ACK(server.networkStream, cmd);

            Log(SERVERLOG,"@@@ ["+server.server_ID+"] send_TABS_SETTINGS: ["+ bytes +" bytes sent]:");
            Log(SERVERLOG,"@@@ (x"+ Settings.TabsDict.Count +") "+ last_sent_tabs);
            Log(SERVERLOG,"@@@ "+ last_sent_key_val.Replace(" ","\n"));
        }
        // }}}
         //}}}
        // POLL {{{
        private static string Server_1_POLL_BUFFER = "";
        private static string Server_2_POLL_BUFFER = "";
        private static string Server_3_POLL_BUFFER = "";
        public  void parse_KEY_VAL(string cmd, string argLine) // {{{
        {
            string buf = Settings.set_KEY_VAL("Server.parse_KEY_VAL from "+ server_ID, cmd, argLine);

            // Set SOURCE=SERVER (if not already signed by designer or a client)
            if(Settings.SOURCE == "") {
               Settings.SOURCE = server_ID;
               buf += " SOURCE="+Settings.SOURCE;
            }
            send_ACK( cmd );
            // DISPATCH TO DISIGNER AND CLIENTS
            Log("parse_KEY_VAL: DISPATCHING KEY_VAL POLL_BUFFER=["+ buf +"]");
            Server_1_POLL_BUFFER = ""+buf; // force cloning
            Server_2_POLL_BUFFER = ""+buf; // force cloning
            Server_3_POLL_BUFFER = ""+buf; // force cloning

            dispatch_POLL_BUFFERS( cmd );

            // SERVER - APPLY KEY_VAL
            if(Settings.SOURCE != Settings.APP_NAME)
                MainFormInstance.Invoke( (MethodInvoker)delegate() { MainFormInstance.profile_changed("parse_KEY_VAL("+ cmd +")"); });
            else
                Log("@@@ NOT EVALUATING OWN ["+ Settings.SOURCE +"] KEY_VAL @@@\n");
        }
        //}}}
        public  void dispatch_POLL_BUFFERS(string cmd)// {{{
        {
            Log(SERVERLOG,"...dispatch_POLL_BUFFERS("+ cmd +")");
            Log(SERVERLOG,"........server_ID=["+ server_ID      +"]");

            bool originator_too = (cmd.IndexOf(Settings.CMD_PROFILE) >= 0);
            Log(SERVERLOG,"...originator_too=["+ originator_too +"]");

            for(int i=1; i <= 3; ++i)
            {
                Server server = null;
                switch(i) {
                    case 1: server = Server_1; break;
                    case 2: server = Server_2; break;
                    case 3: server = Server_3; break;
                }
                // do not send back to originator
                if((server == this) && !originator_too) {
                    Log(SERVERLOG,"...dispatch_POLL_BUFFERS: skipping originator ["+ server.server_ID +"]");
                    continue;
                }

                if((server != null) && server.Running)
                    send_POLL_BUFFER(server, Settings.CMD_POLL);
            }
        }
        // }}}
        public static void send_POLL_BUFFER(Server server, string cmd)// {{{
        {
            Log(SERVERLOG,"...["+ server.server_ID +"] send_POLL_BUFFER("+ cmd +")");
            // SELECT SERVER BUFFER {{{
            string buf = "";
            if     (server == Server_1) { buf = Server_1_POLL_BUFFER; }
            else if(server == Server_2) { buf = Server_2_POLL_BUFFER; }
            else if(server == Server_3) { buf = Server_3_POLL_BUFFER; }

            ///}}}
            // SEND BUFFER {{{
            int bytes = 0;
            if(buf.Length > 0) {
                bytes  = sendLine(server.networkStream, cmd +" "+ buf); // reply starts with with cmd for client parser...
            //  bytes += send_ACK(server.networkStream, cmd);           // no ACK required with this one-liner that startsWith("POLL")
                server.last_sent_key_val = buf;
            }

            //}}}
            // CONSUME SERVER BUFFER {{{
            if     (server == Server_1) { Server_1_POLL_BUFFER = ""; }
            else if(server == Server_2) { Server_2_POLL_BUFFER = ""; }
            else if(server == Server_3) { Server_3_POLL_BUFFER = ""; }

            //}}}
            Log(SERVERLOG,"...["+server.server_ID+"] send_POLL_BUFFER("+ cmd +") dispatched ("+bytes+" bytes):\n@@@ ["+ buf.Replace("\n"," ") +"]");
        }
        // }}}
        public  static void Dispatch(string cmdLine) //{{{
        {
            Log(SERVERLOG, "== [DISPATCH] ("+ cmdLine +"):");

/*
            Server   server    = null;
            if     ((Server_1 != null) && Server_1.Running && (AcceptedSocket_1 != null) && AcceptedSocket_1.IsBound) { server = Server_1; }
            else if((Server_2 != null) && Server_2.Running && (AcceptedSocket_2 != null) && AcceptedSocket_2.IsBound) { server = Server_2; }
            else if((Server_3 != null) && Server_3.Running && (AcceptedSocket_3 != null) && AcceptedSocket_3.IsBound) { server = Server_3; }
            if(      server   != null)
            {
//:!start explorer "https://msdn.microsoft.com/en-us/library/system.threading.thread.interrupt.aspx"

                server.readerThread.Interrupt();
            //  server.readerThread.Abort();

            //  Thread worker = new Thread (delegate() { server.readerThread.Interrupt(  ); });
            //  Thread worker = new Thread (delegate() { server.readerThread.Abort(  ); });
            //  worker.Start(  );

            }
            else {
                Log(SERVERLOG, "** [DISPATCH] no accepted connection yet\n");
            }
*/
            string[] args  = cmdLine.Split(' ');
            string cmd     = args[0];
            string argLine = cmdLine.Substring(cmd.Length+1);

            // FORWARD TO ALL CONNECTED CLIENTS
            Server_1_POLL_BUFFER = ""+argLine; // force cloning
            Server_2_POLL_BUFFER = ""+argLine; // force cloning
            Server_3_POLL_BUFFER = ""+argLine; // force cloning

// fake client polling (it should have a thread blocked on reading this)

            for(int i=1; i <= 3; ++i)
            {
                Server server = null;
                switch(i) {
                    case 1: server = Server_1; break;
                    case 2: server = Server_2; break;
                    case 3: server = Server_3; break;
                }

                if((server != null) && server.Running && server.acceptedSocket.IsBound)
                    send_POLL_BUFFER(server, cmd);
            }

            Log("Dispatch: SETTING ["+ cmdLine +"]");
        }
        // }}}
        //}}}
        // PROFILES - sync (save, send) {{{
        private void sync_PROFILE(string cmd, string profile_name) //{{{
        {
            Log(SERVERLOG, " . sync_PROFILE("+ cmd +", "+ profile_name +"):");

            // LOAD LOCAL VERSION
            Settings.LoadProfile(profile_name);

            // XXX DESIGNER AND SERVER MAY SIMULATE A TIME DELTA FOR TESTING PURPOSE
            //Settings.PRODATE -= 120;

            // COMPARE VERSIONS
            int  remote_prodate = int.Parse( Settings.CmdParser.getArgValue("PRODATE", "0") );

            bool local_old      = (remote_prodate > Settings.PRODATE);
            bool local_new      = (remote_prodate < Settings.PRODATE);

            Log(SERVERLOG, " . PROFILE=["+ profile_name     +"] local_old=["+ local_old +"] local_new=["+ local_new +"]");
            Log(SERVERLOG, " . ["        + remote_prodate   +"]=[remote_prodate  ] ("+ Settings.Get_time_elapsed( remote_prodate   ) +")");
            Log(SERVERLOG, " . ["        + Settings.PRODATE +"]=[Settings.PRODATE] ("+ Settings.Get_time_elapsed( Settings.PRODATE ) +")");

            // IF BOTH VERSION ARE THE SAME .. DO NOTHING
            if(local_new || local_old) {
                string newer     = local_new ? "SERVER" : server_ID;
                string time_diff = Settings.Get_time_diff(Settings.PRODATE, remote_prodate);
                Log(SERVERLOG," . ["+ newer +"] has been updated "+ time_diff +" later");
            }
            else {
                if(Settings.PRODATE == 0)
                    Log(SERVERLOG," . PROFILE NOT FOUND");
                else
                    Log(SERVERLOG," . ["+server_ID+"] and [SERVER] PROFILES are the same version");
            }

            // IF NOT THE SAME .. DISPATCH [YET TO POSSIBLY BE UPDATED] LOCAL PROFILE
            if(local_new || local_old) {
                string argLine = "PROFILE="+ profile_name +" PRODATE="+ Settings.PRODATE +" SOURCE="+ server_ID;
                parse_KEY_VAL(cmd, argLine);
            }
            send_ACK( cmd );
            ++send_PROFACK_Count;

            // IF LOCAL OLDER .. [DOWNLOAD REMOTE PROFILE CONTENT]
            if( local_old ) {
                download_PROFILE( profile_name );
                Settings.LoadProfile( profile_name ); // load updated version
            }

            // IF LOCAL NEWER .. [UPLOAD LOCAL PROFILE CONTENT]
            if( local_new ) {
                if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
                    upload_PROFILE(Settings.CMD_POLL, profile_name );
                // android client will send CMD_PROFILE_DOWNLOAD
            }
            // ELSE, NOTHING TO UPLOAD OR DOWNLOAD
        }
        //}}}
        private static bool Download_in_progress = false;
        private void download_PROFILE(string profile_name) //{{{
        {
            Download_in_progress = true;
            Log(SERVERLOG,"@@@ ["+server_ID+"] download_PROFILE("+ profile_name +"):");
/*
            // REQUEST REMOTE PROFILE LINES
            Log(SERVERLOG, "=== Thread.Sleep(1000):");
            Thread.Sleep(1000);

            string buf = Settings.CMD_PROFILE_UPLOAD +" PROFILE="+profile_name;
            Log(SERVERLOG, "...sendLine("+buf+"):");
            sendLine( buf );
*/
            // READ REMOTE PROFILE LINES
            StringBuilder sb  = new StringBuilder();
            string    cmd_ACK = Settings.CMD_POLL+" ACK";
            int    line_count = 0;
            string       line = "";

            networkStreamReader.BaseStream.ReadTimeout    = 30000;//5000; 
            Log(SERVERLOG,"...ReadTimeout=["+ networkStreamReader.BaseStream.ReadTimeout +"]");
            int wait_start = Settings.GetUnixTimeSeconds();
            int wait_end   = 0;
            try {
                do {
                    line = networkStreamReader.ReadLine();
                    if(wait_end == 0) wait_end = Settings.GetUnixTimeSeconds();
                    if((line != null) && (line != cmd_ACK))
                    {
                        sb.Append(line); sb.Append(Environment.NewLine);
                        ++line_count;
                    }
                }
                while( (line != cmd_ACK)
                    && (line != null)
                    && Running
                    );
            }
            catch(Exception ex) {
                if(wait_end == 0) wait_end = Settings.GetUnixTimeSeconds();
                Log( SERVERLOG
                    ,"download_PROFILE:\n"
                    +"*** " + ex.Message
                //  +"***\n"+ ex.StackTrace
                   );
            }
            string time_diff = Settings.Get_time_diff(wait_end, wait_start);
            networkStreamReader.BaseStream.ReadTimeout    = Timeout.Infinite; 
            string profile_text = sb.ToString();
            Log(SERVERLOG,"@@@ Wait time=["+ time_diff +"] from ["+server_ID+"] "+ line_count +" lines ("+profile_text.Length+" bytes)");

            // SAVE PROFILE INTO A LOCAL FILE
            if(line == cmd_ACK) {
                if( !profile_name.Equals(Settings.CMD_PROFILES_TABLE) ) {
                    string file_path
                    = Settings.ProfilesFolder
                    + Path.DirectorySeparatorChar
                    + profile_name.Replace('/', Path.DirectorySeparatorChar)
                    +".txt"
                    ;
                    Settings.SaveProfile(file_path, profile_text);
                    Log(SERVERLOG,"@@@ ["+server_ID+"] download_PROFILE("+ profile_name +") ...done\n");
                }
                else {
                    Log(SERVERLOG,"@@@ ["+server_ID+"] download_PROFILE("+ profile_name +" ...does no need saving .. it is rebuilt every time to show the list of saved profiles");
                }
            }
            else {
                Log( SERVERLOG
                    ,"*** ["+server_ID+"] download_PROFILE("+ profile_name +") ...failed ***"
                    +"*** MISSING  LAST LINE=["+ cmd_ACK +"] ***"
                    +"*** RECEIVED LAST LINE=["+ line    +"] ***\n"
                   );
            }
            Download_in_progress = false;
        }
        //}}}
        private void upload_PROFILE(string cmd, string profile_name) //{{{
        {
            Log(SERVERLOG,"@@@ ["+server_ID+"] upload_PROFILE("+ cmd +", "+ profile_name +"):");

            // PROFILE UPDATED BY SOME CLIENT ... REQUESTS COMING FROM OTHER CLIENT MAY HAVE TO WAIT FOR SERVER TO GET IT
            int count = 0;
            while(Download_in_progress) {
                Log(SERVERLOG,"@@@ UPLOAD DELAYED BY DOWNLOAD IN PROGRESS ("+ ++count +")");
                Thread.Sleep(500);
            }

            // PROFILE
            string filePath = Settings.GetProfilePath( profile_name );
            if(filePath == "") {
                Log(SERVERLOG, "=== upload_PROFILE("+ profile_name +") NOT in current Profiles_Dict ... reloading profiles folder");
                Settings.Clear_Profiles_Dict();
                filePath = Settings.GetProfilePath( profile_name );
            }
            if(filePath == "")
                Log(SERVERLOG, "*** upload_PROFILE("+ profile_name +") PROFILE NOT FOUND");

            // PROFILE FILE
            Log(SERVERLOG,"@@@ filePath=["+filePath+"]");
            if( !System.IO.File.Exists( filePath ) ) {
                Log(SERVERLOG, "*** upload_PROFILE("+ profile_name +") FILE NOT FOUND: ["+ filePath +"]");
                filePath = "";
            }

            int bytes = 0;
            if(filePath != "")
            {
                // load file contents
                string[] lines = File.ReadAllLines( filePath );
                Log(SERVERLOG,"@@@ "+ lines.Length +" lines");

                // android StoredReply has to begin with sent cmd
                if(!(Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
                    bytes += sendLine(cmd +" PROFILE="+ profile_name);

                for(int i=0; i< lines.Length; ++i)
                    bytes += sendLine(networkStream, lines[i]);
            }
            bytes += send_ACK(cmd);

            Log(SERVERLOG,"@@@ UPLOADED PROFILE ["+ profile_name +"] ...("+ bytes +" bytes)");
        }
        // }}}
        //}}}
        // UTIL {{{
        // sendLine {{{
        private static int sendLineBytes = 0;
        private        int sendLine(string txt) { return sendLine(this.networkStream, txt); }
        private static int sendLine(Stream server_networkStream, string txt)
        {
//Log("@@@ sendLine("+ txt +")");
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(txt+"\n");

            server_networkStream.Flush();
            server_networkStream.Write(bytes, 0, bytes.Length);
            server_networkStream.Flush();

            sendLineBytes += bytes.Length;

            return bytes.Length;
        }
        // }}}
        // send_ACK {{{
        private int send_ACK(string cmd) { return send_ACK(this.networkStream, cmd); }
        private static int send_ACK(Stream server_networkStream, string cmd)
        {
            return sendLine(server_networkStream, cmd +" "+ Settings.ACK);
        }
        //}}}
        // Dispose {{{
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                Server_TcpListener = null;

                if(networkStream != null) {
                    networkStream.Close();
                    networkStream.Dispose();
                    networkStream = null;
                }

            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        //}}}
        //}}}
        // LOG {{{
        // Log {{{
        private static void Log(string msg) {
            Log("Server", msg);
        }
        public  static void Log(string caller, string msg)
        {
            if(MainFormInstance == null || Server_exiting)
                return;
/*
            if(caller != "Server") Logger.Log("Server", msg+"\n");
            Logger.Log(caller, msg+"\n");
*/
            Logger.Log("Server", msg+"\n");
        }
        //}}}
        // Log_center {{{
        public  static void Log_center(string msg)
        {
            if(MainFormInstance == null || Server_exiting)
                return;

            string caller = typeof(Server).Name;
        //  MainFormInstance.Invoke( (MethodInvoker)delegate() { Logger.Log_center(caller, msg); });
            Logger.Log_center(caller, msg);
        }
        //}}}
        // SetLogging {{{
        public  static void SetLogging(bool state)
        {
            if(MainFormInstance == null || Server_exiting)
                return;

        //  MainFormInstance.Invoke( (MethodInvoker)delegate() { Logger.SetLogging( state ); });
           Logger.SetLogging( state );
        }
        //}}}

        // get_server_status {{{
        private static string get_server_status(Server server)
        {
            if((server == null) || !server.Running)
                return "is waiting a connection request\n";

            else
                return "connection: ["+ server.server_ID +"]:\n"
                    +  " === "+ server.last_sent_key_val +"\n"
                    +  " === "+ server.last_sent_tabs    +"\n"
                    +  " === "+ server.last_sent_palettes+"\n"
                    ;

        }
        //}}}
        // get_localEndpoint_status {{{
        public static string get_localEndpoint_status()
        {
            string localEndpoint_status;
            if(Server_TcpListener != null)
                localEndpoint_status = "Local endpoint:\n"+ Server_TcpListener.LocalEndpoint.ToString();
            else
                localEndpoint_status =  "*** LOCAL ENDPOINT NOT SET ***";

            if( Listener_Status.StartsWith("***") )
                localEndpoint_status +=  "\n"+ Listener_Status;

            return localEndpoint_status;
        }
        //}}}
        // get_status {{{
        public static string get_status()
        {
            string SEP = " =====================================================================";

            // PROFILE TABS PALETTES COMM {{{

            // SERVER
            string localEndpoint_status
                = (Server_TcpListener != null)
                ?  " ["+ Server_TcpListener.LocalEndpoint.ToString() +"]"
                :  "";
            string listener_status
                = SEP +"= SERVER:\n"
                + " @@@ "+ Listener_Status + localEndpoint_status
                +"\n";
            string connexion_status    = " ===  "+ (accept_Count      ) .ToString("D").PadLeft(3) +" connexion"+((accept_Count > 1) ? "s":"")+"\n";
            string profile_ack_status  = " ===  "+ (send_PROFACK_Count) .ToString("D").PadLeft(3) +" PROFILE-ACK-COUNT\n";
            string send_status         = " ===  "+ (sendLineBytes/1024) .ToString("D").PadLeft(3) +" kb sent during the last "+ Settings.Get_time_elapsed(Settings.LAUNCH_DATE) +"\n";

            // CURRENT PROFILE
            string timestamp = "missing timestamp";
            if(Settings.PRODATE > 0) {
                System.DateTime dt = new System.DateTime(1970, 1, 1, 0, 0, 0);
                dt = dt.AddSeconds( Settings.PRODATE ).ToLocalTime();
                timestamp = dt.ToString("yyyy-MMM-dd HH:mm:ss");
            }
            string profile             = " @@@ ["+ Settings.PROFILE +"] ["+ Settings.PRODATE +"] ["+ timestamp +"]";
        //  string tabs_send_status    = " ===  "+ send_TABS_SETTINGS_Times    .ToString("D").PadLeft(3) +" TAB-COUNT="+ send_TABS_Count +" ("+ sendLineBytes/1024 +" kb) (over "+ Settings.Get_time_elapsed(Settings.LAUNCH_DATE) +")\n";
            string tabs_status         = " ("+ Settings.TabsDict.Count     .ToString("D") +" Tabs";
            string palettes_status     =  " "+ Settings.PalettesDict.Count .ToString("D") +" Color palettes)\n";

            // DASH TEXT {{{
            string msg
                = listener_status
                + connexion_status
                + profile_ack_status
                + send_status
                + profile
                + tabs_status
                + palettes_status
            //  + tabs_send_status
                + SEP +"=========\n"
                + " SERVER1 "+ get_server_status( Server_1 )
                + SEP +"=========\n"
                + " SERVER2 "+ get_server_status( Server_2 )
                + SEP +"=========\n"
                + " SERVER3 "+ get_server_status( Server_3 )
                + SEP +"=========\n"
                ;

            //}}}
            return msg;
        }
        //}}}
        //}}}
        // get_BOM {{{
        public static string get_BOM()
        {
            string result = "";

            if((Server_1 != null) && (Server_1.Running))    result += Server_1.server_ID +"\n";
            if((Server_2 != null) && (Server_2.Running))    result += Server_2.server_ID +"\n";
            if((Server_3 != null) && (Server_3.Running))    result += Server_3.server_ID +"\n";
            if(result    == "")                             result += "No connection\n";
            if(Settings.PROFILE != "")                      result += "Profile ["+ Settings.PROFILE +"]\n";

        //  result +=   "Tabs x"    + Settings.TabsDict.Count;
        //  result += "\nPalettes x"+ Settings.PalettesDict.Count;

            return result;
        }
        //}}}
        // }}}
    }
}

