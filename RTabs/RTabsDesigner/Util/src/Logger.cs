using System;
using System.Windows.Forms;
using Util;
public interface LoggerInterface// {{{
{
    Object Invoke(Delegate method);
    string  get_APP_HELP_TEXT();
    string  get_APP_NAME();
    string  get_APP_TITLE();
    void    log(string caller, string msg);
    void    callback(System.Object caller, string detail);
    void    notify(NotePane np, string detail);
    void    set_logging(bool state);
}
// }}}
public interface ClientServerInterface// {{{
{
    bool   ClientServerIsOnLine();
    int    ClientServerConnectionsCount();
    void   ClientServerGetOnLine();
    void   ClientServerGetOffLine();

    void   ClientServerButtons(RTabs.MainForm mainform);
    void   ClientServerDisconnect();
    void   ClientServerExit(string caller);
    void   ClientServerOnActivated(object sender, EventArgs e);
    void   ClientServerOnLoad();
    void   ClientServer_set_Firewall_Rule();

    void   ClientServer_callback(System.Object caller, string detail);

    string ClientServer_get_BOM();
    string ClientServerStatus();
    void   update_COMM_DASH();

    void   settings_Changed(string caller);
    void   cancel();

    void   dispatch_KEY_VAL();
}
// }}}
class Logger
{
    //{{{
    private static LoggerInterface AppLogger;
    public static void   SetAppLogger(LoggerInterface appLogger) {         AppLogger  = appLogger; }
    public static void   SetLogging(bool state)                  {      if(AppLogger != null)        AppLogger.set_logging( state );}
    public static string get_APP_NAME()                          { return (AppLogger == null) ? "" : AppLogger.get_APP_NAME();      }
    public static string get_APP_TITLE()                         { return (AppLogger == null) ? "" : AppLogger.get_APP_TITLE();     }
    public static string get_APP_HELP_TEXT()                     { return (AppLogger == null) ? "" : AppLogger.get_APP_HELP_TEXT(); }

    //}}}
    // CONST {{{
    public  const   string  LOG_CLEAR               = "CLEAR";
    public  const   int     LOG_TICK_PER_LINE       =  25;
    public  const   int     LOG_TICK_PER_PAGE       = 500;

    private const   string  UNDEFINED_APP_NAME      = "UNDEFINED_APP_NAME";
    private const   string  UNDEFINED_APP_TITLE     = "UNDEFINED_APP_TITLE";
    private const   string  UNDEFINED_APP_HELP_TEXT = "UNDEFINED_APP_HELP_TEXT";

    //}}}
    // LOG ================================================================={{{
    // ========================================================================
    // log log_left log_center log_arobas {{{
    public static void   Log(string caller, string msg)
    {
        if(AppLogger == null) return;

	bool has_linefeed = msg.EndsWith("\n");
        if( !has_linefeed ) {
            linefeed_required = true;
        }
        else if(linefeed_required)
        {
            msg += "\n";
	    has_linefeed = true;
            linefeed_required = false;
        }
        AppLogger.Invoke( (MethodInvoker)delegate() { AppLogger.log(caller, msg); });
    //  AppLogger.log(caller, msg);

/*// {{{
	if(log_autoscroll)
	{
	    scrolled_by_log_time    = System.currentTimeMillis();
	    log_container.fullScroll( View.FOCUS_DOWN );

	    update_log_container_scrolling();
	}
*/ // }}}
    }
    public static  void  Log_left  (string caller, string msg) { Log_box(caller, msg, L_JUSTIFY, SEP_LINE_LEFT  , CORNER_SYM); }
    public static  void  Log_center(string caller, string msg) { Log_box(caller, msg, C_JUSTIFY, SEP_LINE_AROBAS, CORNER_SYM); }
    public static  void  Log_right (string caller, string msg) { Log_box(caller, msg, R_JUSTIFY, SEP_LINE_RIGHT , CORNER_SYM); }

    //}}}
    // vars {{{
//  private static long    scrolled_by_log_time    = System.currentTimeMillis();
    private static bool    linefeed_required	    = false;

    //}}}
    // CONST {{{
    public   const string JUSTIFY_SFX	=    "_JUSTIFY";
    private  const string L_JUSTIFY	= "L"+ JUSTIFY_SFX;
    private  const string C_JUSTIFY	= "C"+ JUSTIFY_SFX;
    private  const string R_JUSTIFY	= "R"+ JUSTIFY_SFX;

    private const string SEP_LINE_LEFT   = "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<";
    private const string SEP_LINE_RIGHT  = ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
    private const string SEP_LINE_AROBAS = "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@";
    private const string SEP_LINE_SPACES = "                                                                                                    ";
    private const string CORNER_SYM	= " ";

    //}}}
    private static void Log_box(string caller, string s, string justify, string t, string o)// {{{
    {
        if(AppLogger == null) return;

	int s_length = 1+ s.Length +1;
	int t_length =    t.Length   ;

	if(s_length > t_length) s = s.Substring(0, t_length-5) +"...";
	else			t = t.Substring(0, s_length  );
	s = s.Replace("\n"," ");

	string l = "";
	int extra_space = 80-t.Length;
        if(extra_space < 0) extra_space = 0;
	switch(justify)
	{
	    case R_JUSTIFY: l = SEP_LINE_SPACES.Substring(0, extra_space  ); break;
	    case C_JUSTIFY: l = SEP_LINE_SPACES.Substring(0, extra_space/2); break;
	    case L_JUSTIFY: l = ""                                         ; break;
	}
        string msg  = "\n"+l + o + t +   o+"\n"
                    +      l +"| " + s + " |\n"
                    +      l + o + t +   o+"\n"
                    ;

        AppLogger.Invoke( (MethodInvoker)delegate() { AppLogger.log(caller, msg); });
    //  AppLogger.log(caller, msg);

    }
    // }}}
    // =====================================================================}}}
}

