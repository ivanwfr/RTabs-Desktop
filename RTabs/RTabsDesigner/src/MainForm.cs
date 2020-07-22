// using {{{
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

using Util;
//}}}
namespace RTabs
{
    public partial class MainForm   : Form, LoggerInterface
    {
        // CLASS {{{
        private const  string   MainForm_TAG = "MainForm (200720:00h:35)";

        public  static  uint        UI_state            = 0;

        public  static  HelpForm    PopupHelpForm;

        //}}}
        // INSTANCE {{{
        public static bool          OnLoad_done         = false;
        public static bool          OnActivated_done    = false;

        private bool                HideMinimizing      = false;
        private bool                UI_state_canceling  = false;

        private NotifyIcon          notifyIcon;

        private int                 logCount;
        private int                 tickCount;

        public Util.TabsCollection tabsCollection;

    //  private NotePane            control_clear;
        private NotePane            control_edit;
        private NotePane            control_exit;
        private NotePane            control_firewall;
        private NotePane            control_help;
        private NotePane            control_logging;
        private NotePane            control_palettes;
        private NotePane            control_profiles;
        private NotePane            control_index;
    //  private NotePane            control_save;
        private NotePane            control_settings;

        private NotePane            control_export;
        private NotePane            control_export_profile;
        private NotePane            control_export_to_file;
        private NotePane            control_export_clipboard;
        private NotePane            control_import;
        private NotePane            control_import_insert;
        private NotePane            control_import_overlay;

        private NotePane            control_layout;
        private NotePane            control_add;
        private NotePane            control_activate;
        private NotePane            control_update_profile;
        private NotePane            control_del;

        private NotePane            control_color0;
        private NotePane            control_color1;
        private NotePane            control_color2;
        private NotePane            control_color3;
        private NotePane            control_color4;
        private NotePane            control_color5;
        private NotePane            control_color6;
        private NotePane            control_color7;
        private NotePane            control_color8;
        private NotePane            control_color9;
        private NotePane            control_color10;
        private NotePane            control_color11;

        private NotePane            panel_help      = null;
        private NotePane            panel_log       = null;
        private NotePane            panel_palettes  = null;
        private NotePane            panel_XPort     = null;

        private Color               mainForm_BackColor;

        private ClientServerInterface   clientServerForm;

        private Image               saved_tabs_container_BackgroundImage= null;
        private Image               scaled_BackgroundImage              = null;

        //}}}
        // CONSTRUCT {{{
        public MainForm(ClientServerInterface _clientServerForm)
        {
            Settings.ParseTime_Millisecond = Settings.GetUnixTimeMilliSeconds();

            // UI IDE COMPONENTS
            this.clientServerForm = _clientServerForm;
            InitializeComponent();

            if((Settings.APP_NAME.ToUpper().IndexOf("SERVER") >= 0))
                this.ShowInTaskbar  = false;
            else
                this.ShowInTaskbar  = true;

            // SAVE FORM BACKGOUND IMAGE FOR LAYOUT SETTINGS
            mainForm_BackColor      = this.BackColor;

        //  saved_tabs_container_BackgroundImage  = tabs_container.BackgroundImage;
            hide_layout_grid();

            // APPLICATION FORM
            string timestamp        = Settings.RetrieveLinkerTimestamp();
          //this.Text               = Settings.APP_TITLE+" "+timestamp;
            this.Text               = Settings.PROJECT_NAME+" "+timestamp; // (190117)

            // GETS KEY INPUT - SEE FORM_KEYDOWN()
            this.KeyPreview         = true;

            // LOAD APPLICATION SETTINGS SAVED IN REGISTRY
            loadSettings();

            // Notepane - [Initialize_ColorPaletteDict] [Initialize_Images]
            NotePane.Initialize(this);
            palettes_Changed("MainForm");

            Settings.MainFormInstance   = this;

            // TABS - hook tabs factory to application form
            tabsCollection = new Util.TabsCollection(this, tabs_container, controls_container, panels_container);
            tabsCollection.set_logger( this ); // as a callback handler
            TabsCollection.ResetAutoPlace();

            // COMM DASH FIRST (TOP OF THE PAGE)
            tabsCollection.update_dash(NotePane.PANEL_NAME_COMM, NotePane.TXT_PLACEHOLDER);

            // LOG SINK
            panel_log            = tabsCollection.update_panel(NotePane.PANEL_NAME_LOG              , NotePane.TXT_PLACEHOLDER);

            // SERVER-DESIGNER
            // exit comm-settings layout {{{

            control_exit         = tabsCollection.update_control(NotePane.CONTROL_NAME_EXIT         , NotePane.CONTROL_LABEL_EXIT     );
            control_settings     = tabsCollection.update_control(NotePane.CONTROL_NAME_SETTINGS     , NotePane.CONTROL_LABEL_SETTINGS );
            control_layout       = tabsCollection.update_control(NotePane.CONTROL_NAME_LAYOUT       , NotePane.CONTROL_LABEL_LAYOUT   );

            // DESIGNER'S EDIT CONTROL MUST BE NEAR LAYOUT
            if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
                control_edit             = tabsCollection.update_control(NotePane.CONTROL_NAME_EDIT          , NotePane.CONTROL_LABEL_EDIT          );

            control_logging      = tabsCollection.update_control(NotePane.CONTROL_NAME_LOGGING      , NotePane.CONTROL_LABEL_LOGGING  );

            //  control_save         = tabsCollection.update_control(NotePane.CONTROL_NAME_SAVE         , NotePane.CONTROL_LABEL_SAVE     );
            // }}}

            // DESIGNER
            // profile add activate del color {{{
            if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
            {
                // profile export // {{{
                control_profiles         = tabsCollection.update_control(NotePane.CONTROL_NAME_PROFILE       , NotePane.CONTROL_LABEL_PROFILE       );
                control_index            = tabsCollection.update_control(NotePane.CONTROL_NAME_INDEX         , NotePane.CONTROL_LABEL_INDEX         );
                control_update_profile   = tabsCollection.update_control(NotePane.CONTROL_NAME_UPDATE_PROFILE, NotePane.CONTROL_LABEL_UPDATE_PROFILE);
                control_export_profile   = tabsCollection.update_control(NotePane.CONTROL_NAME_EXPORT_PROFILE, NotePane.CONTROL_LABEL_EXPORT_PROFILE);
                control_export_to_file   = tabsCollection.update_control(NotePane.CONTROL_NAME_EXPORT_TO_FILE, NotePane.CONTROL_LABEL_EXPORT_TO_FILE);
                control_export_clipboard = tabsCollection.update_control(NotePane.CONTROL_NAME_EXPORT_CLIPBRD, NotePane.CONTROL_LABEL_EXPORT_CLIPBRD);
                control_export           = tabsCollection.update_control(NotePane.CONTROL_NAME_EXPORT        , NotePane.CONTROL_LABEL_EXPORT        );

                // }}}
                // add del activate {{{
                control_add              = tabsCollection.update_control(NotePane.CONTROL_NAME_ADD           , NotePane.CONTROL_LABEL_ADD           );
                control_del              = tabsCollection.update_control(NotePane.CONTROL_NAME_DEL           , NotePane.CONTROL_LABEL_DEL           );
                control_activate         = tabsCollection.update_control(NotePane.CONTROL_NAME_ACTIVATE      , NotePane.CONTROL_LABEL_ACTIVATE      );
                control_edit             = tabsCollection.update_control(NotePane.CONTROL_NAME_EDIT          , NotePane.CONTROL_LABEL_EDIT          );

                // }}}
                // color filters {{{
                control_color0           = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR0        , NotePane.CONTROL_LABEL_COLOR0  , "0" );
                control_color1           = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR1        , NotePane.CONTROL_LABEL_COLOR1  , "1" );
                control_color2           = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR2        , NotePane.CONTROL_LABEL_COLOR2  , "2" );
                control_color3           = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR3        , NotePane.CONTROL_LABEL_COLOR3  , "3" );
                control_color4           = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR4        , NotePane.CONTROL_LABEL_COLOR4  , "4" );
                control_color5           = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR5        , NotePane.CONTROL_LABEL_COLOR5  , "5" );
                control_color6           = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR6        , NotePane.CONTROL_LABEL_COLOR6  , "6" );
                control_color7           = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR7        , NotePane.CONTROL_LABEL_COLOR7  , "7" );
                control_color8           = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR8        , NotePane.CONTROL_LABEL_COLOR8  , "8" );
                control_color9           = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR9        , NotePane.CONTROL_LABEL_COLOR9  , "9" );
                control_color10          = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR10       , NotePane.CONTROL_LABEL_COLOR10 , "10" );
                control_color11          = tabsCollection.update_control(NotePane.CONTROL_NAME_COLOR11       , NotePane.CONTROL_LABEL_COLOR11 , "11" );

                // }}}

                // SAVE-LOAD
                control_palettes         = tabsCollection.update_control(NotePane.CONTROL_NAME_PALETTES      , NotePane.CONTROL_LABEL_PALETTES      );
                control_palettes.TT
                    = "Export current color palette into an editable text tab\n"
                    + "where you can add your own, one per line [somePaletteName #xxxxxx #xxxxxx ...]\n"
                    + "before you click this button again to have them imported."
                    ;


                // IMPORT
                control_import           = tabsCollection.update_control(NotePane.CONTROL_NAME_IMPORT        , NotePane.CONTROL_LABEL_IMPORT        );
                control_import.TT
                = "Stuff your own PALETTES and TABS lines into a writable Text Tab.\n"
                + "You can then save them into the CUSTOM PROFILE folder."
                ;
                control_import_insert    = tabsCollection.update_control(NotePane.CONTROL_NAME_IMPORT_INSERT , NotePane.CONTROL_LABEL_IMPORT_INSERT );
                control_import_overlay   = tabsCollection.update_control(NotePane.CONTROL_NAME_IMPORT_OVERLAY, NotePane.CONTROL_LABEL_IMPORT_OVERLAY);

                // TOOLTIPS {{{

                control_layout.TT
                    = "Adjust Tabs position and size\n"
                    + "...click again when done"
                    ;

                control_logging.TT
                    = "Activate on-demand module log tabs (toggle)"
                    ;

                control_profiles.TT
                    = "ACCESS SAVED PROFILES"
                    ;

                control_index.TT
                    = "OPEN TOP INDEX PROFILE"
                    ;

                control_export.TT
                    = "EXPORT CURRENT TABS\n"
                    + "copy to CLIPBOARD\n"
                    + "or save as a PROFILE"
                    ;

                control_add.TT
                    = "ADD A NEW TAB"
                    ;

                control_activate.TT
                    = "ACTIVATE/DEACTIVATE TAB"
                    ;

                control_update_profile.TT
                    = "UPDATE PROFILE"
                    ;

                control_del.TT = "RIGHT-CLICK ON A TAB TO DELETE IT";

                control_edit.TT
                    = "EDIT TABS (Activation, Text, PC-Command)\n"
                    + "...click again when done"
                    ;

                //control_edit.update_tooltip();

                //}}}
            }
            //}}}

            // SERVER
            // firewall {{{
            else {
                control_firewall     = tabsCollection.update_control(NotePane.CONTROL_NAME_FIREWALL     , NotePane.CONTROL_LABEL_FIREWALL );
                control_firewall.TT
                    = "Open Firewall acces to the application\n"
                    + "to the selected IP and Port (toggle)"
                    ;
            }
            //}}}


            control_exit.TT
                = "Terminate application"
                ;

        //  control_clear        = tabsCollection.update_control(NotePane.CONTROL_NAME_CLEAR        , NotePane.CONTROL_LABEL_CLEAR    );
            control_help         = tabsCollection.update_control(NotePane.CONTROL_NAME_HELP         , NotePane.CONTROL_LABEL_HELP     );
            control_help.TT
                = "Help panel (toggle)"
                ;

            // DESIGNER OR SERVER UI CONTORLS
            clientServerForm.ClientServerButtons(this);

            // UI - current profile state
            clear_profile_needs_saving("MainForm");

            if(Settings.HIDE)
                hideUI("MainForm");
        }
        //}}}
        // LOG {{{
        public string   get_APP_HELP_TEXT() { return Settings.APP_HELP_TEXT; }
        public string   get_APP_NAME()      { return Settings.APP_NAME;      }
        public string   get_APP_TITLE()     { return Settings.APP_TITLE;     }

        public  void log(string caller, string msg)// {{{
        {
            //  if( !Settings.LOGGING ) return;

            // AT CREATION TIME, UI MAY NOT BE READY {{{
            if(panel_log == null)
                return;

            //}}}
            // [caller] [tab_name] {{{
            string tab_name;

            if(caller.StartsWith( NotePane.PANEL_NAME_HEADER ))
            {
                tab_name = caller;
                caller   = caller.Replace(NotePane.PANEL_NAME_HEADER, "");
            }
            else {
                tab_name = NotePane.PANEL_NAME_HEADER+caller;
            }

            //}}}
            // CLEAR {{{
            if(msg == Logger.LOG_CLEAR) {
                if( tabsCollection.has_tab     (tab_name) ) {
                    tabsCollection.update_panel(tab_name, msg);
                    return;
                }
            }
            // }}}
            // ALREADY FORMATTED {{{
            string text;

            // dashboard message
            if(caller.EndsWith("DASH")) {
                text     = msg;
            }
            // justified message
            else if(msg.StartsWith("\n")) {
                text     = msg;
            }
            // progress message
            else if(msg.Length == 1) {
                text    = msg;
            }

            //}}}
            // LOG FORMAT [text] = [line_count] [caller] [msg] {{{
            else {
                string optional_linefeed = msg.EndsWith("\n") ? "" : "\n";
            //  text = string.Format("{0,3} - {1} {2}{3}", ++logCount, caller, msg, optional_linefeed);
	        long ts = Settings.GetUnixTimeMilliSeconds() - Settings.ParseTime_Millisecond;
                text    = string.Format("{0,9}-{1,3} - {2} {3}{4}", ts, ++logCount, caller, msg, optional_linefeed);
            }

            // }}}
            // CREATE CALLER'S PANEL {{{
            NotePane np = null;
            if( !tabsCollection.has_tab(tab_name) )
            {
                if(tabs_container.InvokeRequired ) {
                    panel_log.TextBox.AppendText("\n*IRQ* "+text+"\n");
                }
                else {
                    if( tab_name.EndsWith("DASH") )
                        np = tabsCollection.update_dash (tab_name, text);  // title +text
                    else if( Settings.LOGGING )
                        np = tabsCollection.update_panel(tab_name, text);  // title +text
                }
            }
            //}}}
            // UPDATE CALLER'S PANEL {{{
            else {
                if( tab_name.EndsWith("DASH") )
                    np = tabsCollection.update_dash (tab_name, text);  // ..... +text
                else if( Settings.LOGGING )
                    np = tabsCollection.update_panel(tab_name, text);  // ..... +text
            }
            // }}}

            if(np != null)
                np.Show();
        }
        //}}}
        public  void set_logging(bool state)// {{{
        {
            // TOGGLE LOGGING
            if(state) {
                Settings.LOGGING = true;
                log("EVENTS", "set_logging(true):");

                control_logging.Label = NotePane.CONTROL_LABEL_LOGGING;
                add_highlight( control_logging  );
                //add_DropShadow( control_logging.TextBox );
                //control_logging.TextBox.Refresh();
            }
            else {
                log("EVENTS", "set_logging(false):");
                Settings.LOGGING = false;
                clear_app_panels_content();

                control_logging.Label = NotePane.CONTROL_LABEL_LOGGING_OFF;
                del_highlight( control_logging );
                //del_DropShadow( control_logging.TextBox );
                //control_logging.TextBox.Refresh();
            }
            // SHOW-HIDE NotePane.PANEL_NAME_HEADER
            tabsCollection.show_UI_state_targets_only( UI_state );
            update_MainForm("set_logging("+ state +")");
        }
        //}}}

        private void timer_Tick(object sender, EventArgs e)// {{{
        {
            if( !OnLoad_done ) return;
            combo_Highlight_off();
            NotePane.Timer_Tick(sender, e);

            // DRAW AND ERASE ANNOUNCES
            Announce.Tic( tickCount );

            RichTextBox _tb = panel_log.TextBox;

            if((tickCount % Logger.LOG_TICK_PER_PAGE) == 0)
                panel_log.rebuild_rtf();

            if((tickCount % Logger.LOG_TICK_PER_LINE) == 0) {
                _tb.AppendText(String.Format("\n{0,4} ", 1+tickCount));
                if(    clientServerForm.ClientServerIsOnLine()
                    && (UI_state != Settings.STATE_SETTINGS)
                  )
                    update_COMM_DASH("timer_Tick");
            }

            ++tickCount;

            _tb.AppendText(".");

            try {
                _tb.SelectionStart = _tb.Text.Length;
                _tb.ScrollToCaret();
            } catch(Exception) {}
        }
        //}}}
        // combo_Highlight_off {{{
        private void combo_Highlight_off()
        {
            combo_dev_dpi  .SelectionStart  = 0; combo_dev_dpi  .SelectionLength = 0;
            combo_dev_wh   .SelectionStart  = 0; combo_dev_wh   .SelectionLength = 0;
            combo_mon_scale.SelectionStart  = 0; combo_mon_scale.SelectionLength = 0;
            combo_palettes .SelectionStart  = 0; combo_palettes .SelectionLength = 0;
        }
        //}}}

        private string get_BOM() //{{{
        {
            string s
                = Settings.APP_NAME +"\n"
                + clientServerForm.ClientServer_get_BOM()
                ;

             s = Settings.ellipsis(s, 60).Replace("\\n","\n");

            return s;
        }
        //}}}
        //}}}
        // LIFE-CYCLE {{{
/*
:!start explorer "https://msdn.microsoft.com/en-us/library/system.windows.forms.form(v=vs.110).aspx"
:!start explorer "https://msdn.microsoft.com/en-us/library/ms754221(v=vs.100).aspx"
:!start explorer "https://social.msdn.microsoft.com/Forums/vstudio/en-US/fc9a0fa4-eebc-45d3-9899-a0753143a1bc/wpf-lifecycle?forum=wpf"
*/
        // OnLoad (virtual .. no override) {{{
        protected void OnLoad(object sender, EventArgs e)
        {
            // UI IS READY
            Logger.SetAppLogger( this ); // ...that's only where we can start logging

            // LOAD USER TABS
            tabsCollection.load_usr_tabs();

            log("EVENTS", "OnLoad");

            // RESTORE application window geometry {{{
            apply_Settings_APP_XYWH();

            //}}}
            // Tray menu {{{
            if((Settings.APP_NAME.ToUpper().IndexOf("SERVER") >= 0))
                add_notifyIcon();

            //}}}

            OnLoad_done = true;
            clientServerForm.ClientServerOnLoad();

            try {
                if     ((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0)) v_splitter.SplitterDistance = v_splitter.Width; // make tabs fully visible
                else if((Settings.APP_NAME.ToUpper().IndexOf("SERVER"  ) >= 0)) v_splitter.SplitterDistance = 0;               // make panels fully visible
            } catch(Exception) {} // may have invalid geometry!

        }
        //}}}
        // OnActivated (virtual .. no override) {{{
        protected void OnActivated(object sender, EventArgs e)
        {
            log("EVENTS", "OnActivated");

            clientServerForm.ClientServerOnActivated(sender, e);

            OnActivated_done = true;
        }
        //}}}
/*
        // OnResize {{{
        protected override void OnResize(System.EventArgs e)
        {
            log("EVENTS", "OnResize");
            if(     WindowState == FormWindowState.Minimized & HideMinimizing) {
                //Hide();
                //Settings.HIDE = true;
            }
            else if(WindowState == FormWindowState.Normal) {
                //HideMinimizing = false;
                //Settings.HIDE  = false;
            }
        } //}}}
        // OnResizeEnd (virtual .. no override) {{{
        protected void OnResizeEnd(System.EventArgs e)
        {
            log("EVENTS", "OnResizeEnd");
            if(     WindowState == FormWindowState.Minimized & HideMinimizing) {
                //Hide();
                //Settings.HIDE = true;
            }
            else if(WindowState == FormWindowState.Normal) {
                //HideMinimizing = false;
                //Settings.HIDE  = false;
            }
        } //}}}
*/
        // OnLayout {{{
        protected override void OnLayout(LayoutEventArgs e)
        {
            log("EVENTS", "OnLayout("+ e.AffectedProperty +"): H=["+this.Size.Height+"] minH=["+this.MinimumSize.Height+"]");
            base.OnLayout(e);

            if(this.Size.Height < this.MinimumSize.Height)
                Settings.HIDE       = true;

        } //}}}
        // OnShown {{{
        protected override void OnShown(EventArgs e)
        {
            log("EVENTS", "OnShown");
            base.OnShown(e);
            // KEY_VAL {{{
MainForm_SuspendLayout();
            palettes_Changed ("OnActivated");
            txt_zoom_Changed ("OnActivated");
            dev_zoom_Changed ("OnActivated");
            dev_dpi_Changed  ("OnActivated");
            dev_wh_Changed   ("OnActivated");
            mon_scale_Changed("OnActivated");
MainForm_ResumeLayout();
            adjust_layout_dpi("OnShown");

            //}}}

        }
        //}}}
        // OnFormClosing (virtual .. no override) {{{
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            log("EVENTS", "OnFormClosing("+ e.CloseReason +"): H=["+this.Size.Height+"] minH=["+this.MinimumSize.Height+"]");

            if(this.Size.Height < this.MinimumSize.Height)
                Settings.HIDE       = true;

            if( HideMinimizing ) {
                e.Cancel            = true;
                this.WindowState    = FormWindowState.Minimized;
//this.Hide();
            }
            else {
                exit("OnFormClosing");
            }
        }
        //}}}
        //}}}
        // RESTORE application window geometry {{{
        public void apply_Settings_APP_XYWH()
        {
            log("EVENTS", "apply_Settings_APP_XYWH():");
            if( Settings.APP_XYWH == "") {
                Settings.APP_XYWH
                    = String.Format("{0},{1},{2},{3}"
                        , ((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
                        ? 0 : Screen.PrimaryScreen.Bounds.Width / 2
                        , 0
                        , Screen.PrimaryScreen.Bounds.Width / 2
                        , Screen.PrimaryScreen.Bounds.Height
                        );
            }
            string[] xy_wh  = Settings.APP_XYWH.Split(',');
            int x           = int.Parse( xy_wh[0] );
            int y           = int.Parse( xy_wh[1] );
            int w           = int.Parse( xy_wh[2] );
            int h           = int.Parse( xy_wh[3] );
            this.Location   = new System.Drawing.Point(x, y);
            this.ClientSize = new System.Drawing.Size (w, h);
            /*
            // RESTORE SAVED POSITION
            if(    (Settings.SplitX >= v_splitter.Panel1MinSize)
            && (Settings.SplitX <= v_splitter.Panel2MinSize))
            v_splitter.SplitterDistance = Settings.SplitX;
             */

            // DESIGNER TABS
            if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
                v_splitter_show( tabs_container );

            // SERVER PANELS
            if((Settings.APP_NAME.ToUpper().IndexOf("SERVER") >= 0))
                v_splitter_show( panels_container );
        }
        //}}}
        // HIDE-SHOW {{{
        // Tray Icon {{{
        private void add_notifyIcon()
        {
            // TRAY ICON
            log("EVENTS", "add_notifyIcon");

            ContextMenu menu             = new ContextMenu();
            menu.MenuItems.Add("Open "+Settings.APP_NAME, new EventHandler( toggleUI_CB ));
            menu.MenuItems.Add("Exit "+Settings.APP_NAME, new EventHandler( exitAppCB ));

            notifyIcon                   = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Icon              = this.Icon;
            notifyIcon.Text              = Settings.APP_NAME;
            notifyIcon.MouseDoubleClick += new MouseEventHandler( toggleUI_CB );
            notifyIcon.ContextMenu       = menu;
            notifyIcon.Visible           = true;

        }

        private void toggleUI_CB(object sender, EventArgs e)
        {
            toggleUI("toggleUI_CB");
        }

        public  void toggleUI(string caller)
        {
            if(WindowState == FormWindowState.Minimized)
                showUI(caller);
            else
                hideUI(caller);

        }

        //}}}
        // hideUI {{{
        public  void hideUI(string caller)
        {
            log("EVENTS", "hideUI("+ caller +")");

            Settings.HIDE       = true;
            HideMinimizing      = true;
            this.WindowState    = FormWindowState.Minimized;
//this.Hide();

            update_MainForm("hideUI("+ caller +")");
        }
        //}}}
        // showUI {{{
        public  void showUI(string caller)
        {
            log("EVENTS", "showUI("+ caller +")");

            Settings.HIDE       = false;
            HideMinimizing      = false;
            this.WindowState    = FormWindowState.Normal;

            if(    (Settings.SplitX >= v_splitter.Panel1MinSize)
                && (Settings.SplitX <= v_splitter.Panel2MinSize)
              )
                v_splitter.SplitterDistance = Settings.SplitX;

            Activate();
            BringToFront();
            Focus();

            update_MainForm("showUI("+ caller +")");
        }
        //}}}
        // disconnect {{{
        public  void disconnect(string caller)
        {
            log("EVENTS", "disconnect("+ caller +")");
            clientServerForm.ClientServerDisconnect();
        }
        //}}}
        // set_input_ip_visibility {{{
        private void set_input_ip_visibility(bool state)
        {
            panel_ip      .Visible = state;
            text_netsh    .Visible = state;
            label_netsh   .Visible = state;
        }
        //}}}
        // set_input_dpi_visibility {{{
        private void set_input_dpi_visibility(bool state)
        {
            panel_dpi     .Visible = state;
        }
        //}}}
        // Highlight {{{
        // add_highlight {{{
        private void add_highlight(NotePane np)
        {
            if(np != null) {
                //  if(np.BackColor != Color.Red) np.TextBox.ForeColor = Color.Red;
                //  else                          np.TextBox.ForeColor = Color.Yellow;
                np.TextBox.ForeColor = Color.Red;
                np.TextBox.BackColor = Color.Black;
            }
        }
        //}}}
        // del_highlight {{{
        private void del_highlight(NotePane np)
        {
            if(np != null) {
                np.TextBox.ForeColor = np.ForeColor;
                np.TextBox.BackColor = np.BackColor;
            }
        }
        //}}}

        // add_server_highlight {{{
        private void add_ClientServer_highlight(NotePane np)
        {
            if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
                np.TextBox.ForeColor = Color.BlueViolet;
            else
                np.TextBox.ForeColor = Color.CadetBlue;
        }
        //}}}
        /*
        // ApplyDropShadow() {{{
        DropShadowEffect myDropShadowEffect = null;
        private void add_DropShadow(Control control)
        {
        if(myDropShadowEffect == null) // {{{
        {
        // Initialize a new DropShadowEffect that will be applied // to the Button.
        myDropShadowEffect = new DropShadowEffect();

        // Set the color of the shadow to Black.
        Color myShadowColor = new Color();
        myShadowColor.A = 255; // Note that the alpha value is ignored by Color property. 

        // The Opacity property is used to control the alpha.
        myShadowColor.B = 50;
        myShadowColor.G = 50;
        myShadowColor.R = 50;
        myDropShadowEffect.Color = myShadowColor;

        // Set the direction of where the shadow is cast to 320 degrees.
        myDropShadowEffect.Direction = 320;

        // Set the depth of the shadow being cast.
        myDropShadowEffect.ShadowDepth = 25;

        // Set the shadow softness to the maximum (range of 0-1).
        myDropShadowEffect.BlurRadius = 6;

        // Set the shadow opacity to half opaque or in other words - half transparent.
        // The range is 0-1.
        myDropShadowEffect.Opacity = 0.5;

    }
    // }}}
    // Apply the effect to the Button.
    control.Effect = myDropShadowEffect;
    }
    private void del_DropShadow(Control control)
    {
        control.Effect = null;
    }
    //}}}
*/
        // }}}
        //}}}
        // EXIT {{{
        private void exitAppCB(object sender, EventArgs e) // {{{
        {
            exit("exitAppCB");
        } //}}}
        public void exit(string caller)// {{{
        {
            if(notifyIcon != null)
                notifyIcon.Visible      = false;

            log("EVENTS", "exit(caller=["+ caller +"])");

            // STOP LOGGING
            Settings.MainFormInstance   = null;
            Logger.SetAppLogger( null );

            saveSettings("exit");

            clientServerForm.ClientServerExit( caller );
        }
        //}}}
        //}}}

        public void saveSettings(string caller)// {{{
        {
            log("EVENTS", "saveSettings(caller=["+ caller +"])");
            // SAVED BY BOTH SERVER AND DESIGNER {{{

            // COLORS
            Settings.SaveSetting("PALETTE", Settings.PALETTE);
            Settings.SaveSetting("OPACITY", Settings.OPACITY.ToString());
            Settings.SaveSetting("PROFILE", Settings.PROFILE);
            Settings.SaveSetting("PRODATE", Settings.PRODATE.ToString());

            if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
                NotePane.SaveSettings();

            // UI
            if(WindowState == FormWindowState.Normal)
            {
                if(    (this.Width  != Screen.PrimaryScreen.Bounds.Width )
                    || (this.Height != Screen.PrimaryScreen.Bounds.Height)
                  )
                    Settings.APP_XYWH = String.Format("{0},{1},{2},{3}", this.Left, this.Top, this.Width, this.Height);

                Settings.SaveSetting("APP_XYWH"  , Settings.APP_XYWH);

            }
        //  Settings.SplitX = v_splitter.SplitterDistance; // (!) just save the last [user-set-position]
            Settings.SaveSetting("HIDE"    , Settings.HIDE.ToString());
            Settings.SaveSetting("LOGGING" , Settings.LOGGING.ToString());
            Settings.SaveSetting("SplitX"  , Settings.SplitX.ToString());

            // TABS
            log("EVENTS", "...User tabs");
            tabsCollection.saveSettings();

            //}}}
            // SAVED BY SERVER {{{
            if((Settings.APP_NAME.ToUpper().IndexOf("SERVER") >= 0))
            {
                Settings.SaveSetting("IP"      , Settings.IP             );
                Settings.SaveSetting("Port"    , Settings.Port.ToString());
                Settings.SaveSetting("Password", Settings.Password       );
                Settings.SaveSetting("MAC"     , Settings.MAC            );
                Settings.SaveSetting("SUBNET"  , Settings.SUBNET         );
                Settings.SaveSetting("HIDE"    , Settings.HIDE.ToString());

                // have the server rescan profiles folder .. when user ask to save settings
                Settings.Clear_Profiles_Dict();
            }
            // }}}
            // SAVED BY/FOR BOTH SERVER AND DESIGNER {{{
            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "DEV_DPI"     , Settings.DEV_DPI     .ToString());
            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "DEV_H"       , Settings.DEV_H       .ToString());
            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "DEV_W"       , Settings.DEV_W       .ToString());
            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "DEV_ZOOM"    , Settings.DEV_ZOOM    .ToString());
            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "MON_SCALE"   , Settings.MON_SCALE   .ToString());
            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "TXT_ZOOM"    , Settings.TXT_ZOOM    .ToString());

            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "DEV_DPI_P"   , Settings.DEV_DPI_P   .ToString());
            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "DEV_H_P"     , Settings.DEV_H_P     .ToString());
            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "DEV_W_P"     , Settings.DEV_W_P     .ToString());
            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "DEV_ZOOM_P"  , Settings.DEV_ZOOM_P  .ToString());
            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "MON_SCALE_P" , Settings.MON_SCALE_P .ToString());
            Settings.SaveSetting(Settings.DESIGNER_APP_NAME, "TXT_ZOOM_P"  , Settings.TXT_ZOOM_P  .ToString());
            //}}}
            Settings.Flush(Settings.DESIGNER_APP_NAME);
            Settings.Flush();
            Settings.Settings_saved = true;

            clientServerForm.settings_Changed("saveSettings"); // TODO - optimize - spot what has changed ONLY

            info_Announce("Settings have been saved by ["+ caller +"]");
        }
        // }}}
        public void loadSettings() // {{{
        {
            // ...too early for any logs

            Settings.Settings_saved = false; // will be set by an effective load of once saved value
            // USED BY BOTH SERVER AND DESIGNER {{{

            // PER-APP logging
            Settings.LOGGING    =   bool.Parse( Settings.LoadSetting("LOGGING" , Settings.LOGGING.ToString()));

            // PER-APP UI
            Settings.APP_XYWH   =              Settings.LoadSetting("APP_XYWH"   , Settings.APP_XYWH);
            Settings.SplitX     =    int.Parse(Settings.LoadSetting("SplitX"     , Settings.SplitX.ToString()));
            Settings.PALETTE    =              Settings.LoadSetting("PALETTE"    , Settings.PALETTE);
            Settings.OPACITY    =    int.Parse(Settings.LoadSetting("OPACITY"    , Settings.OPACITY.ToString()));
            Settings.PROFILE    =              Settings.LoadSetting("PROFILE"    , Settings.PROFILE);
            Settings.PRODATE    =    int.Parse(Settings.LoadSetting("PRODATE"    , Settings.PRODATE.ToString()));


            // CONNECTION SETTINGS
            Settings.IP         =             Settings.LoadSetting(Settings.SERVER_APP_NAME, "IP"         , Settings.IP              );
            Settings.Port       =  int.Parse( Settings.LoadSetting(Settings.SERVER_APP_NAME, "Port"       , Settings.Port.ToString()));
            Settings.Password   =             Settings.LoadSetting(Settings.SERVER_APP_NAME, "Password"   , Settings.Password        );
            Settings.MAC        =             Settings.LoadSetting(Settings.SERVER_APP_NAME, "MAC"         , Settings.MAC              );
            Settings.SUBNET     =             Settings.LoadSetting(Settings.SERVER_APP_NAME, "SUBNET"         , Settings.SUBNET              );

            if(Settings.IP     == "") Settings.IP     = Netsh.GetIP();
            if(Settings.MAC    == "") Settings.MAC    = Netsh.GetMAC( Netsh.GetIPAddress( Settings.IP ) );
            if(Settings.SUBNET == "") Settings.SUBNET = Netsh.GetSUBNET();

            text_IP      .Text  = Settings.IP;
            text_port    .Text  = Settings.Port.ToString();
            text_password.Text  = Settings.Password;
            text_mac     .Text  = Settings.MAC;
            text_subnet  .Text  = Settings.SUBNET;

            // LOAD DESIGNER SETTINGS (...TO BE SENT BY SERVER TO CLIENTS)
            Settings.DEV_DPI    =    int.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "DEV_DPI"     , Settings.DEV_DPI            .ToString()));
            Settings.DEV_H      =    int.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "DEV_H"       , Settings.DEV_H              .ToString()));
            Settings.DEV_W      =    int.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "DEV_W"       , Settings.DEV_W              .ToString()));
            Settings.DEV_ZOOM   = double.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "DEV_ZOOM"    , Settings.DEV_ZOOM           .ToString()));
            Settings.MON_SCALE  = double.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "MON_SCALE"   , Settings.MON_SCALE          .ToString()));
            Settings.TXT_ZOOM   = double.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "TXT_ZOOM"    , Settings.TXT_ZOOM           .ToString()));

            Settings.DEV_DPI_P  =    int.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "DEV_DPI_P"   , Settings.DEV_DPI_P          .ToString()));
            Settings.DEV_H_P    =    int.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "DEV_H_P"     , Settings.DEV_H_P            .ToString()));
            Settings.DEV_W_P    =    int.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "DEV_W_P"     , Settings.DEV_W_P            .ToString()));
            Settings.DEV_ZOOM_P = double.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "DEV_ZOOM_P"  , Settings.DEV_ZOOM_P         .ToString()));
            Settings.MON_SCALE_P= double.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "MON_SCALE_P" , Settings.MON_SCALE_P        .ToString()));
            Settings.TXT_ZOOM_P = double.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "TXT_ZOOM_P"  , Settings.TXT_ZOOM_P         .ToString()));

            Settings.MON_DPI    =    int.Parse(Settings.LoadSetting(Settings.DESIGNER_APP_NAME, "MON_DPI"   , this.CreateGraphics().DpiY.ToString()));

            // DPI ........................................................... smaller=96 medium=120 larger=144

            // digest the whole chain ratio to optimize rendering
            // ...to be maintained everytime a member should change

            // ...................[SMALLER ON SCREEN FOR WITH HIGH DEVICE DPI......] . [COMPENSATED BY SCALE]
            Settings.ratio      = (double)Settings.MON_DPI/ (double)Settings.DEV_DPI *  Settings.MON_SCALE  ;

            //}}}
            // USED BY SERVER ONLY {{{
            if((Settings.APP_NAME.ToUpper().IndexOf("SERVER") >= 0))
            {
                Settings.HIDE         = bool.Parse( Settings.LoadSetting("HIDE"        , Settings.HIDE.ToString()));
            //  Settings.PollInterval =  int.Parse( Settings.LoadSetting("PollInterval", Settings.PollInterval.ToString()));
            }
            // }}}
            // UPDATED BY SERVER ONLY {{{
            if((Settings.APP_NAME.ToUpper().IndexOf("SERVER") < 0))
            {
                // input fields - (editable in SERVER - readonly in DESIGNER)
                text_IP      .Enabled = false;
                text_password.Enabled = false;
                text_port    .Enabled = false;
            }
            text_mac         .Enabled = false;
            text_subnet      .Enabled = false;
            //}}}
            Settings.Settings_saved = true; // Got something! .. it means that it's not the first run
        }
        // }}}
        public void loadFreshCommSettings() // {{{
        {
            // FRESH LOAD OF CONNECTION SETTINGS
            Settings.ReopenSettings(Settings.SERVER_APP_NAME);

            // IP PORT PASSWORD input fields
            // ...both DESIGNER and SERVER will use the same SERVER'S REGISTRY SETTINGS

            Settings.IP         =               Settings.LoadSetting(Settings.SERVER_APP_NAME, "IP"         , Settings.IP              );
            Settings.Port       =  int.Parse(   Settings.LoadSetting(Settings.SERVER_APP_NAME, "Port"       , Settings.Port.ToString()));
            Settings.Password   =               Settings.LoadSetting(Settings.SERVER_APP_NAME, "Password"   , Settings.Password        );
            Settings.MAC        = Netsh.GetMAC( Netsh.GetIPAddress(  Settings.IP ) );
            Settings.SUBNET     = Netsh.GetSUBNET();
        }
        // }}}

        // SETTINGS {{{
        // set_settings_state {{{
        private string method_id = "set_settings_state";
        private void control_settings_Click()// {{{
        {
            // do not interfere
            if((UI_state != 0) && (UI_state != Settings.STATE_SETTINGS))
                return;

            bool settings_state = ((UI_state & Settings.STATE_SETTINGS) != 0);
            log("EVENTS", "control_settings_Click: settings_state=["+settings_state+"]");

            set_settings_state( !settings_state );

        }
        //}}}
        public void set_settings_state(bool settings_state)// {{{
        {
            log("EVENTS", "set_settings_state("+settings_state+"):");

            if(settings_state)      UI_state |=  Settings.STATE_SETTINGS;    // bit set
            else                    UI_state &= ~Settings.STATE_SETTINGS;    // bit clear
            NotePane.Set_UI_state(  UI_state );

            if(settings_state)
            {
                // MUTEX ACQUIRE {{{
                if( Settings.UseMutex ) {
                    try {
                        Settings.Log_SMutex_Wait("EVENTS", method_id);
                        if( Settings.ServiceMutex.WaitOne( Settings.WAIT_SERVICE_MUTEX_TIMEOUT_MS ))
                            Settings.SM_aquiredBy( method_id );
                        else
                            Settings.Log_SMutex_WaitFailed("EVENTS", method_id);
                    } catch(Exception ex) {
                        log("EVENTS", "*** "+ method_id +":\n"+ Settings.ExToString(ex)); 
                    }
                }
                //}}}

                clientServerForm.ClientServerGetOffLine();

                // TOOLS
                set_input_ip_visibility      ( true  );
                set_input_dpi_visibility     ( false );

                // BUTTON

                update_COMM_DASH("settings_state("+ settings_state +")");

                add_highlight( control_settings );
                control_settings.TextBox.Refresh();

                text_IP      .Text  = Settings.IP;
                text_port    .Text  = Settings.Port.ToString();
                text_password.Text  = Settings.Password;
                text_mac     .Text  = Settings.MAC;
                text_subnet  .Text  = Settings.SUBNET;

                tabsCollection.list_usr_tabs();

                log_netsh();
            }
            else {
                // READ AND HIDE USER SETTINGS INPUT FIELDS {{{
                //{{{
                bool changed_ip;
                bool changed_port;
                bool changed_password;
                bool nothing_changed;
                int  port;

                //}}}
                if(OnLoad_done) {
                    // SERVER LISTEN ...WITH CURRENT IP PORT AND PASSWORD {{{
                    if((Settings.APP_NAME.ToUpper().IndexOf("SERVER") >= 0)) {
                        // WHAT HAS CHANGED {{{
                        port                                    = Settings.Port;
                        try               {  port               = int.Parse(text_port.Text); }
                        catch(Exception)  {  text_port.Text     = Settings.Port.ToString();  }

                        changed_ip        = (Settings.IP       != text_IP.Text      );
                        changed_port      = (Settings.Port     != port              );
                        changed_password  = (Settings.Password != text_password.Text);
                        nothing_changed   = !changed_ip && !changed_port && !changed_password;

                        log("EVENTS","@ changed_ip......: ["+ changed_ip       +"] ["+ Settings.IP       +"]..["+ text_IP.Text       +"]\n");
                        log("EVENTS","@ changed_port....: ["+ changed_port     +"] ["+ Settings.Port     +"]..["+ port               +"]\n");
                        log("EVENTS","@ changed_password: ["+ changed_password +"] ["+ Settings.Password +"]..["+ text_password.Text +"]\n");
                        log("EVENTS","@ nothing_changed.: ["+ nothing_changed  +"]\n");

                        //}}}
                        // SAVE CHANGES OR CANCEL {{{
                        // SERVER-RESTART FOR IP AND PORT CHANGES {{{
                        int connectionsCount = clientServerForm.ClientServerConnectionsCount();
                        if((changed_ip || changed_port) && (connectionsCount > 0))
                        {
                            DialogResult Answer
                                = MessageBox.Show("Current "+ connectionsCount
                                    + " client connection(s) have to be closed to make those change"
                                    , "Change Settings"
                                    , MessageBoxButtons.OKCancel
                                    , MessageBoxIcon.Information
                                    );
                            if(Answer == DialogResult.Cancel)
                                nothing_changed = true;
                        }
                        //}}}
                        // CANCEL: RESET UI INPUT FIELDS VALUES {{{
                        if(nothing_changed)
                        {
                            text_IP      .Text  = Settings.IP;
                            text_port    .Text  = Settings.Port.ToString();
                            text_password.Text  = Settings.Password;
                        }
                        //}}}
                        // SERVER=[STOP..SAVE..RESTART] .. DESIGNER=[DISCONNECT..RECONNECT] {{{
                        else {
                            // [OLD IP-PORT] stop / disconnect {{{
                            if(changed_ip || changed_port) {
                                log("EVENTS", "...STOPING THE SERVER");
                                clientServerForm.ClientServerDisconnect();
                            }

                            //}}}
                            // SAVE CURRENT UI INPUT FIELDS VALUES {{{
                            Settings.IP         = text_IP.Text;

                            // validate IP .. see text_IP_KeyDown
/*
                            Regex regex = new Regex(@"((\d{1,3}\.){3}(\d{1,3}\.))");
                            Match match = regex.Match( Settings.IP );
                            if( !match.Success ) {
                                Settings.IP = Netsh.GetIP();
                                text_IP.Text  = Settings.IP;
                            }
*/

                            Settings.Port       = port;
                            Settings.Password   = text_password.Text;
                            Settings.MAC        = Netsh.GetMAC( Netsh.GetIPAddress( Settings.IP ) );
                            Settings.SUBNET     = Netsh.GetSUBNET();
                            //}}}
                            // [NEW IP & PORT] .. SERVER-LISTEN .. DESIGNER-CONNECT {{{
                            if(changed_ip || changed_port)
                            {
                                log("EVENTS", "...STARTING THE SERVER");
                                clientServerForm.ClientServerGetOnLine();
                            }
                            // PASSWORD CHANGE ONLY MATTERS FOR NEW CONNECTION REQUESTS
                            // ...TODO send new password to currently connected clients

                            //}}}
                        }
                        //}}}
                        //}}}
                    }
                    //}}}
                    // DESIGNER CONNECT WITH CURRENT IP PORT AND PASSWORD {{{
                    if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
                    {
                        loadFreshCommSettings();

                        changed_ip        = (Settings.IP              != text_IP.Text      );
                        changed_port      = (Settings.Port.ToString() != text_port.Text    );
                        changed_password  = (Settings.Password        != text_password.Text);

                        log("EVENTS","@ changed_ip......: ["+ changed_ip       +"] ["+ text_IP.Text       +"]..["+ Settings.IP       +"]\n");
                        log("EVENTS","@ changed_port....: ["+ changed_port     +"] ["+ text_port.Text     +"]..["+ Settings.Port     +"]\n");
                        log("EVENTS","@ changed_password: ["+ changed_password +"] ["+ text_password.Text +"]..["+ Settings.Password +"]\n");

                        if(changed_ip || changed_port)
                        {
                            log("EVENTS", "...reconnecting");
                            clientServerForm.ClientServerDisconnect();
                            clientServerForm.ClientServerGetOnLine();
                        }
                    }
                    //}}}
                    saveSettings( NotePane.CONTROL_LABEL_SETTINGS );
                }
                //}}}

                // MUTEX RELEASE {{{
                if( Settings.UseMutex ) {
                    if(    (Settings.ServiceMutexOwner == method_id)
                        && (Settings.ServiceMutexCount > 0         )
                      ) {
                        Settings.Log_SMutex_Release("EVENTS", method_id);
                        Settings.SM_releasedBy( method_id );
                        Settings.ServiceMutex.ReleaseMutex();
                    }
                }
                //}}}
            }

            // TRY TO ESTABLISH DESIGNER-SERVER CONNECTION {{{
            if( !settings_state )
            {
                if(!clientServerForm.ClientServerIsOnLine())
                    clientServerForm.ClientServerGetOnLine();

                // IF IT SUCCEEDS, HIDE SETTINGS UI CONTROLS
                if( clientServerForm.ClientServerIsOnLine() )
                {
                    log("EVENTS", Settings.APP_NAME +" IS ONLINE: hiding settings ui");
                    set_input_ip_visibility ( false );
                    set_input_dpi_visibility( true  );

                    update_control_settings_Label();
                    del_highlight( control_settings );
                    control_settings.TextBox.Refresh();
                }
                // IF IT FAILS, KEEP THEM VISIBLE AND REESTABLISH SETTINGS_STATE
                else {
                    log("EVENTS", Settings.APP_NAME +" *** NOT ONLINE: keeping settings_state=true");
                    UI_state |=  Settings.STATE_SETTINGS;    // bit set
                    NotePane.Set_UI_state( UI_state );

                    set_input_ip_visibility ( true  );
                    set_input_dpi_visibility( false );
                }
            }
            //}}}

            tabsCollection.show_UI_state_targets_only( UI_state );
            update_MainForm("set_settings_state("+ settings_state +")");
        }
        //}}}
        //}}}
        private void text_IP_KeyDown        (object sender, KeyEventArgs e)// {{{
        {
            // {ENTER} .. cascade from IP to PORT
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                // EMPTY
                if(text_IP.Text == "")
                {
                    text_IP.Text = Netsh.GetIP();
                    text_IP.SelectAll();
                    info_Announce("First adapter IP: "+ text_IP.Text);
                    return;
                }

                // CHECK IPHOSTENTRY
                string ip = Netsh.GetIPAddress( text_IP.Text );
                if(ip != "") {
                    text_mac   .Text = Netsh.GetMAC(ip);
                    text_subnet.Text = Netsh.GetSUBNET();
                    info_Announce("Resolved IP: ["+ text_IP.Text+"]=["+ip+"]");
                }
                else {
                    warn_Announce("Could not resolve: "+ text_IP.Text);
                    // CHECK IP FORMAT
                    Regex regex = new Regex(@"(\d{1,3}\.){3}(\d{1,3})");
                    Match match = regex.Match( text_IP.Text );
                    if( !match.Success ) {
                        warn_Announce("Invalid IP format: "+ text_IP.Text);
                        text_IP.Text = Netsh.GetIP();
                        text_IP.SelectAll();
                        return;
                    }
                }

                // IP CHECK PASSED
                text_port.Focus();
            }
        }
        //}}}
        private void text_port_KeyDown      (object sender, KeyEventArgs e)// {{{
        {
            // {ENTER} .. cascade from PORT TO PASSWORD
            if(e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true;    text_password.Focus(); }
        }
        //}}}
        private void text_password_KeyDown  (object sender, KeyEventArgs e)// {{{
        {
            // {ENTER} .. cascade from PASSWORD to ending SETTINGS STATE
            if(e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true;    set_settings_state(false); }
        }
        private void text_netsh_KeyDown     (object sender, KeyEventArgs e)// {{{
        {
            if(e.KeyCode == Keys.Enter) {
                e.SuppressKeyPress = true;
                log_netsh();
            }
        }
        //}}}
        private void log_netsh()// {{{
        {
            // netsh
            string ip     = Netsh.GetIP();
            string mac    = Netsh.GetMAC( Netsh.GetIPAddress(ip) );
            string subnet = Netsh.GetSUBNET();

            string filter = text_netsh.Text;
            string cmd    = "ipconfig /all";
            log("COMM_DASH"
                , "=== INTERFACE IPV4 CONFIG:\n"
                + "First IP=["+ ip     +"]\n"
                + ".....MAC=["+ mac    +"]\n"
                + "..Subnet=["+ subnet +"]\n"
                + "\n"
                + "=== ipconfig (capture) === "+ filter +":\n"
                + Netsh.CommandFilter(cmd, filter, false)
                + "\n"
                + "=== ipconfig (filter ) === "+ filter +":\n"
                + Netsh.CommandFilter(cmd, filter, true )
               );
        }
        //}}}
        //}}}

        //}}}
        // UI_state [layout .. edit .. logging .. export .. import .. profile] {{{
        // --------------------------------------------------------------------
        // --------------------------------------------------------------------
        // SETTINGS IMPORT EXPORT {{{
        // click @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        // USAGE {{{
        public const string IMPORT_USAGE_TEXT
            = "# COPY-PASTE YOUR IMPORT SETTINGS HERE\n"
            + "# AND CLICK ONE OF THE IMPORT OPTIONS\n"
            + "# ... you can add some directives to customize\n"
            + "# - load another profile:\n"
            + "#     TAG_CMD=PROFILE\n"
            + "#     TAG_SFX=\"\"\n"
            + "# - send text:\n"
            + "#     TAG_CMD=SENDKEYS\n"
            + "# - send keyboard input:\n"
            + "#     TAG_CMD=SENDINPUT\n"
            + "# - append text:\n"
            + "#     TAG_SFX={ENTER}\n"
            + "# - layout on 2 cols:\n"
            + "#     TAG_LYO=2\n"
            ;

        // }}}
        // control_import_Click {{{
        public void control_import_Click() { control_import_Click(null); }
        public void control_import_Click(Object caller)
        {
            log("EVENTS", "control_import_Click()");
            // do not interfere {{{
            if((UI_state != 0) && (UI_state != Settings.STATE_IMPORT))
                return;

            bool import_state = ((UI_state & Settings.STATE_IMPORT) != 0);
            log("EVENTS", "...import_state=["+import_state+"]");

            if(panel_XPort == null) panel_XPort_build();
            //}}}
            // IMPORT START {{{
            if(!import_state)
            {
                // show import panel
                v_splitter_show( panel_XPort );
                this.Refresh(); // FIXME is it necessary ?

                // import initial empty contents
                panel_XPort.Show();
                panel_XPort.TextBox.Text = IMPORT_USAGE_TEXT;
                panel_XPort.TextBox.SelectAll();
                panel_XPort.Tag = panel_XPort.TextBox.Text; // SET MARKED CURRENT SETTINGS-DATA
                panel_XPort.Focus();

                // start import state
                set_import_state(true);
            }
            // }}}
            // IMPORT DONE {{{
            else {
                // [IMPORT] .. [OFFSET X@Y] [hide] [show] [exit-import-state] {{{
                if( !UI_state_canceling )
                {
                    // [OFFSET 0@0] for [IMPORT_MODE_REPLACE] or [IMPORT_MODE_OVERLAY] {{{
                    if     (caller == control_import        )   Settings.ImportMode = Settings.IMPORT_MODE_REPLACE;
                    else if(caller == control_import_insert )   Settings.ImportMode = Settings.IMPORT_MODE_INSERT ;
                    else /*(caller == control_import_overlay)*/ Settings.ImportMode = Settings.IMPORT_MODE_OVERLAY;

                    if(    (Settings.ImportMode == Settings.IMPORT_MODE_REPLACE)
                        || (Settings.ImportMode == Settings.IMPORT_MODE_OVERLAY)
                    ) {
                        Settings.ImportOffsetX = 0;
                        Settings.ImportOffsetY = 0;
                    }
                    // }}}
                    // [OFFSET freeX@freeY] [IMPORT_MODE_INSERT] {{{
                    // ... FIXME: find a way to fill up free zones
                    else {
                        Point  location = tabsCollection.get_free_grid_xy_near_top_left();
                        Settings.ImportOffsetX = location.X;
                        Settings.ImportOffsetY = location.Y;
                        log("EVENTS", "control_import_Click: import offset=["+ location.X +" @ "+ location.Y +"]");
                    }
                    //}}}
                    // [hide] [show] [import] [exit-import-state] {{{

                    // hide import panel
                    if(!UI_state_canceling)
                        panel_XPort.Hide();

                    // show tabs
                    v_splitter_show( tabs_container ); this.Refresh(); // FIXME is it necessary ?

                    // import settings
                    if(!UI_state_canceling)
                        panel_XPort_import();

                    // exit import state
                    set_import_state(false);

                    //}}}
                }
                //}}}
                // [CANCEL] hide show  exit-import-state {{{
                else
                {
                    // hide import panel
                    panel_XPort.Hide();

                    // show tabs
                    v_splitter_show( tabs_container ); this.Refresh(); // FIXME is it necessary ?

                    // exit import state
                    set_import_state(false);
                }
                //}}}
                //}}}
            }
            //}}}
        }
        //}}}
        // control_export_Click {{{
        public void control_export_Click() { control_export_Click(null); }
        public void control_export_Click(Object caller)
        {
            log("EVENTS", "control_export_Click()");
            // do not interfere {{{
            if((UI_state != 0) && (UI_state != Settings.STATE_EXPORT))
                return;

            bool export_state = ((UI_state & Settings.STATE_EXPORT) != 0);
            log("EVENTS", "...export_state=["+export_state+"]");

            if(panel_XPort == null) panel_XPort_build();
            //}}}
            // EXPORT START {{{
            if(!export_state)
            {
                // show import panel
                v_splitter_show( panel_XPort );
                this.Refresh(); // FIXME is it necessary ?

                // export current settings contents
                panel_XPort.Show();
                panel_XPort_export();
                panel_XPort.Focus();

                // start export state
                set_export_state(true);
            }
            //}}}
            // EXPORT DONE (or CANCELED) {{{
            else {
                // TO FILE {{{
                if(    (caller == control_export_profile)
                    || (caller == control_export_to_file)
                  ) {
                    // CANCEL OR CUSTOMIZE FILE PATH {{{
                    string file_path
                        = Settings.ProfilesFolder
                        + Path.DirectorySeparatorChar
                        + ((Settings.PROFILE != "") ? Settings.PROFILE : Settings.PROFILE_DRAFT)
                        +".txt";

                    if(caller == control_export_to_file) {
                        if(ProfileForm.ShowDialog(file_path, out file_path) != DialogResult.OK)
                            file_path = "";
                    }

                    if(file_path != "")
                        export_profile( file_path );

                    //}}}
                }
                // }}}
                // CLIPBOARD {{{
                else if(caller == control_export_clipboard)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("# PROFILE=CLIPBOARD"                            +"\n");
                    sb.Append("# PRODATE="+ Settings.PRODATE+" "+ DateTime.Now +"\n");
                    sb.Append( panel_XPort.Text );

                    Clipboard.SetText( sb.ToString().Replace("\n", Environment.NewLine) );
                    log("EVENTS", "panel_XPort.Text has been copied to the Clipboard");
                    MessageBox.Show("Export: Tabs are in the clipboard:\n"
                        + tabsCollection.ToString()
                        , Settings.PROFILE
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Information
                        );
                }
                //}}}
                // hide export panel
                if((!UI_state_canceling) && (panel_XPort != null))
                    panel_XPort.Hide();

                // show tabs
                v_splitter_show( tabs_container ); this.Refresh(); // FIXME is it necessary ?

                // exit export state
                set_export_state( false );
            }
            //}}}
        }
        //}}}
        // export_profile {{{
        private void export_profile(string file_path)
        {
            log("EVENTS", "export_profile("+ file_path +"):");
            // ENFORCE EXTENSION (.txt) // {{{
            file_path = file_path.Trim();
            if( !file_path.EndsWith(".txt") )
                file_path += ".txt";

            // }}}
            // PROFILE NAME .. TO BE KEPT REDUNDANT WITH FILE NAME // {{{
        //  String[] pathComponents = file_path.Split( Path.DirectorySeparatorChar );
        //  pathComponents          = pathComponents.Last().Split('.');
        //  Settings.PROFILE        = pathComponents.First().Replace(" ","_");
            String file_tail        = file_path.Substring(Settings.ProfilesFolder.Length + 1);  // remove profile folder
            String file_root        = file_tail.Split('.')[0];                                  // remove extension
            Settings.PROFILE        = file_root.Replace(Path.DirectorySeparatorChar, '/');   // unix separator
            log("EVENTS", "...Settings.PROFILE=["+ Settings.PROFILE +"]");

            // }}}
            // PROFILE TIMESTAMP // {{{
            Settings.PRODATE        = Settings.GetUnixTimeSeconds();
            log("EVENTS", "...Settings.PRODATE=["+ Settings.PRODATE +"]");

            // }}}
            // PROFILE TEXT // {{{
            StringBuilder sb = new StringBuilder();
            sb.Append("# PROFILE="+ Settings.PROFILE                   +"\n");
            sb.Append("# PRODATE="+ Settings.PRODATE+" "+ DateTime.Now +" (by RTabsDesigner)" +"\n");
            // DO NOT STORE CANVAS PALETTE .. SHOULD COME FROM LAST COLORS VISIT
            if( file_root.StartsWith("CANVAS") ) {
                sb.Append( panel_XPort.Text.Replace("# PALETTE=", "# PALETTE-") );
            }
            else {
                sb.Append( panel_XPort.Text );
            }

            // }}}
            // SAVE TO FILE // {{{
            if( !Settings.PROFILE.Equals(Settings.CMD_PROFILES_TABLE) )
            {
                Settings.SaveProfile(file_path, sb.ToString().Replace("\n",Environment.NewLine));
                log("EVENTS", "PROFILE ["+ Settings.PROFILE +"] TABS and PALETTES have been saved into ["+ file_path +"]");
            }
            else {
                log("EVENTS", "control_export_Click("+ Settings.PROFILE +" ...does no need saving .. it is rebuilt every time to show the list of saved profiles");
            }
            // }}}
            // DISPATCH // {{{
            Settings.SOURCE = "";
            adjust_layout_dpi("control_export_Click");
            update_COMM_DASH ("control_export_Click");

            // UI - current profile state
            clear_profile_needs_saving("export_profile");
            // }}}
        }
        //}}}
        // state @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        private void set_import_state(bool import_state)// {{{
        {
            log("EVENTS", "set_import_state("+ import_state +"):");

            if(import_state)        UI_state = Settings.STATE_IMPORT;
            else                    UI_state = 0;
            NotePane.Set_UI_state(  UI_state );

            if(import_state) {
                set_input_ip_visibility      ( false );
                set_input_dpi_visibility     ( false );
                add_highlight( control_import         );
                add_highlight( control_import_insert  );
                add_highlight( control_import_overlay );
            }
            else {
                set_input_ip_visibility      ( false );
                set_input_dpi_visibility     ( true  );
                del_highlight( control_import         );
                del_highlight( control_import_insert  );
                del_highlight( control_import_overlay );
            }
            tabsCollection.show_UI_state_targets_only( UI_state );
            update_MainForm("set_import_state("+ import_state +")");
        }
        //}}}
        private void set_export_state(bool export_state)// {{{
        {
            log("EVENTS", "set_export_state("+ export_state +"):");

            if(export_state)        UI_state = Settings.STATE_EXPORT;
            else                    UI_state = 0;
            NotePane.Set_UI_state(  UI_state );

            // show toggled current export_state
            if(export_state) {
                // TOOLS
                set_input_ip_visibility      ( false );
                set_input_dpi_visibility     ( false );
                // UI
                this.Refresh();
                // BUTTON
                add_highlight( control_export );
                if(control_export_profile != null) control_export_profile.TT   = Settings.PROFILE;
            }
            else {
                // TOOLS
                set_input_ip_visibility      ( false );
                set_input_dpi_visibility     ( true  );
                // UI
                this.Refresh();
                // BUTTON
            //  if(control_export         != null) control_export.Label= NotePane.CONTROL_LABEL_EXPORT;
                if(control_export_profile != null) control_export.TextBox.Refresh();
                del_highlight( control_export );
            }
            if(control_export_profile != null)
                control_export_profile.Label
                    = (Settings.PROFILE != "")
                    ?  NotePane.CONTROL_LABEL_EXPORT_PROFILE +" "+ Settings.PROFILE
                    :  "...";

            tabsCollection.show_UI_state_targets_only( UI_state );
            update_MainForm("set_export_state("+ export_state +")");
        }
        //}}}
        // data @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        // panel_XPort_import {{{
        private void panel_XPort_import()
        {
            log("EVENTS", "panel_XPort_import()");

            // IGNORE UNCHANGED SETTINGS
            if(panel_XPort.TextBox.Text == (string)panel_XPort.Tag) // CHECK MARKED CURRENT SETTINGS-DATA
            {
                log("EVENTS", "@@@ Settings unchanged");
                return;
            }

            // UPDATE SETTINGS
            Settings.LoadProfileFromText( panel_XPort.TextBox.Text );

            // update UI
            callback(panel_XPort, "panel_XPort_import");

            // APPLY SETTINGS
            //apply_Settings_APP_XYWHH();
            string caller =   "importDone";
MainForm_SuspendLayout();
            palettes_Changed ( caller );
            dev_zoom_Changed ( caller );
            dev_dpi_Changed  ( caller );
            dev_wh_Changed   ( caller );
            txt_zoom_Changed ( caller );
            mon_scale_Changed( caller );
MainForm_ResumeLayout();

            // SAVE SETTINGS
            saveSettings("IMPORT");
        }
        //}}}
        // panel_XPort_export {{{
        private void panel_XPort_export()
        {
            log("EVENTS", "panel_XPort_export()");

            if(panel_XPort == null) panel_XPort_build();

            panel_XPort.TextBox.Text
            = "# PALETTE="   + Settings.PALETTE   +"\n"
            + "# OPACITY="   + Settings.OPACITY   +"\n"
            + "# DEV_W="     + Settings.DEV_W     +"\n"
            + "# DEV_H="     + Settings.DEV_H     +"\n"
            + "# MON_SCALE=" + Settings.MON_SCALE +"\n"
            + "# DEV_DPI="   + Settings.DEV_DPI   +"\n"
            ;

            tabsCollection.update_panel(panel_XPort.Name, Settings.GetProfileText());

            panel_XPort.TextBox.SelectAll();

            // == REMEMBER LAST LOADED SETTINGS
            panel_XPort.Tag = panel_XPort.TextBox.Text; // SET MARKED CURRENT SETTINGS-DATA
        }
        //}}}
        // panel_XPort_build {{{
        private void panel_XPort_build()
        {
            log("EVENTS", "panel_XPort_build");

            // panel_XPort
            if( panel_XPort == null) {
                panel_XPort                    = tabsCollection.update_panel(NotePane.PANEL_NAME_XPORT, NotePane.TXT_PLACEHOLDER);
                panel_XPort.SuspendLayout();
                panel_XPort        .BackColor  = this.BackColor;
                panel_XPort.TextBox.BackColor  = this.BackColor;
                panel_XPort        .ForeColor  = this.ForeColor;
                panel_XPort.TextBox.ForeColor  = this.ForeColor;
                panel_XPort.LabelPrefix        = @"\fs28\b ";
                panel_XPort. TextPrefix        = @"\b0\fs16 ";
                panel_XPort.Visible            = false;
                panel_XPort.ResumeLayout();
            }
        }
        //}}}
        // panel_XPort_isVisible {{{
        private bool panel_XPort_isVisible()
        {
            return ((panel_XPort != null) && panel_XPort.Visible);
        }
        //}}}
        //}}}
        // PROFILE {{{

        // set_profile_needs_saving {{{
        public void set_profile_needs_saving(string caller)
        {
            log("EVENTS", "set_profile_needs_saving("+ caller +") ######################################################");

            if(control_profiles         != null) control_profiles      .Label = NotePane.CONTROL_LABEL_PROFILE        +@"\line\fs22 (current: \b *"+ Settings.PROFILE +")";
            if(control_update_profile   != null) control_update_profile.Label = NotePane.CONTROL_LABEL_UPDATE_PROFILE +@"\line\fs22\b *"           + Settings.PROFILE;
        //  if(control_export_profile   != null) control_export_profile.Label = NotePane.CONTROL_LABEL_EXPORT_PROFILE +@"\line\fs16\b *"           + Settings.PROFILE;

            add_highlight( control_profiles       );
            add_highlight( control_update_profile );
        //  add_highlight( control_export_profile );
        }
        //}}}
        // clear_profile_needs_saving {{{
        public void clear_profile_needs_saving(string caller)
        {
            log("EVENTS", "clear_profile_needs_saving("+ caller +")-["+Settings.PROFILE+"] ##############################################");

            if(control_profiles         != null) control_profiles      .Label = NotePane.CONTROL_LABEL_PROFILE        +@"\line\fs22 (current: \b " + Settings.PROFILE +")";
            if(control_update_profile   != null) control_update_profile.Label = NotePane.CONTROL_LABEL_UPDATE_PROFILE +@"\line\fs22\b "            + Settings.PROFILE;
        //  if(control_export_profile   != null) control_export_profile.Label = NotePane.CONTROL_LABEL_EXPORT_PROFILE +@"\line\fs16\b "            + Settings.PROFILE;

            del_highlight( control_profiles       );
            del_highlight( control_update_profile );
        //  del_highlight( control_export_profile );
        }
        //}}}
        // profile_changed {{{
        public void profile_changed(String caller)
        {
            try {
                log("EVENTS", "profile_changed("+ caller +")-["+Settings.PROFILE+"]");

                MainForm_SuspendLayout();
                palettes_Changed ("profile_changed");
                txt_zoom_Changed ("profile_changed");
                dev_zoom_Changed ("profile_changed");
                dev_dpi_Changed  ("profile_changed");
                dev_wh_Changed   ("profile_changed");
                mon_scale_Changed("profile_changed");
                MainForm_ResumeLayout();
                adjust_layout_dpi("profile_changed");

                clear_profile_needs_saving("profile_changed("+ caller +")");

                tabsCollection.scroll_containers_top_left();
            }
            catch(Exception ex)
            {
                log("EVENTS", "*** profile_changed("+ caller +")\n"+ Settings.ExToString(ex)); 
            }
        }
        //}}}

        // click @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        // control_profiles_Click {{{
        public void control_profiles_Click()
        {
            log("EVENTS", "control_profiles_Click()");
            // do not interfere {{{
            if((UI_state != 0) && (UI_state != Settings.STATE_PROFILE))
                return;

            bool profiles_state = ((UI_state & Settings.STATE_PROFILE) != 0);
            log("EVENTS", "...profiles_state=["+profiles_state+"]");

            //}}}
            // PROFILE SELECTION START {{{
            if(!profiles_state)
            {
                Settings.ImportMode = Settings.IMPORT_MODE_REPLACE; // TODO consider composition mode
                set_profiles_state(true); // enter profiles state
            }
            // }}}
            // PROFILE SELECTION END {{{
            else {
                // no unsaved changes (just loaded)
                clear_profile_needs_saving("control_profiles_Click");
                if( profiles_state )
                    set_profiles_state(false);
            }
            //}}}
        }
        //}}}
        // control_index_Click {{{
        public void control_index_Click()
        {
            Settings.LoadProfile("/index");
        }
        //}}}
        // state @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        // current working values .. so we can restore them when done
        private int    saved_DEV_DPI   = Settings.DEV_DPI;
        private int    saved_DEV_H     = Settings.DEV_H;
        private int    saved_DEV_W     = Settings.DEV_W;
        private double saved_DEV_ZOOM  = Settings.DEV_ZOOM;
        private double saved_MON_SCALE = Settings.MON_SCALE;
        private double saved_TXT_ZOOM  = Settings.TXT_ZOOM;

        private void set_profiles_state(bool profiles_state)// {{{
        {
            log("EVENTS", "set_profiles_state("+ profiles_state +"):");

            if(profiles_state)      UI_state = Settings.STATE_PROFILE;
            else                    UI_state = 0;
            NotePane.Set_UI_state(  UI_state );

            if(profiles_state) {
                // RESET CURRENT PROFILE
                Settings.PROFILE    = Settings.PROFILE_DRAFT;
                // SAVE WORKING PARAMETERS
                saved_DEV_DPI       = Settings.DEV_DPI;
                saved_DEV_H         = Settings.DEV_H;
                saved_DEV_W         = Settings.DEV_W;
                saved_DEV_ZOOM      = Settings.DEV_ZOOM;
                saved_MON_SCALE     = Settings.MON_SCALE;
                saved_TXT_ZOOM      = Settings.TXT_ZOOM;
                // SWITCH TO PROFILES WINDOWS PARAMETERS {{{
                MainForm_SuspendLayout();
                Settings.DEV_DPI   = Settings.DEV_DPI_P;   dev_dpi_Changed  ("set_profiles_state");
                Settings.DEV_H     = Settings.DEV_H_P;     dev_wh_Changed   ("set_profiles_state");
                Settings.DEV_W     = Settings.DEV_W_P;     dev_wh_Changed   ("set_profiles_state");
                Settings.DEV_ZOOM  = Settings.DEV_ZOOM_P;  dev_zoom_Changed ("set_profiles_state");
                Settings.MON_SCALE = Settings.MON_SCALE_P; mon_scale_Changed("set_profiles_state");
                Settings.TXT_ZOOM  = Settings.TXT_ZOOM_P;  txt_zoom_Changed ("set_profiles_state");
                MainForm_ResumeLayout();
                adjust_layout_dpi("set_profiles_state");
                // }}}
                // ADD PROFILES TABS
                tabsCollection.del_tabs_named( NotePane.PANEL_NAME_USR );
                tabsCollection.hide_all_tabs();
                add_profile_tabs();
                // UI VISIBILITY
                v_splitter_show( tabs_container );
                set_input_ip_visibility      ( false );
                set_input_dpi_visibility     ( false );
                // UI STATUS
                add_highlight( control_profiles );
                //if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0)) tabsCollection.tabs_container_SuspendLayout();
                tabsCollection.set_tabs_locked_state( true );
            }
            else {
                // CLEAR CURRENT PROFILE SELECTION
                if( !tabsCollection.has_tabs_named( NotePane.PANEL_NAME_USR ) )
                    Settings.PROFILE = Settings.PROFILE_DRAFT;
                // SAVE PROFILES WINDOWS PARAMETERS
                Settings.DEV_DPI_P   = Settings.DEV_DPI;
                Settings.DEV_H_P     = Settings.DEV_H;
                Settings.DEV_W_P     = Settings.DEV_W;
                Settings.DEV_ZOOM_P  = Settings.DEV_ZOOM;
                Settings.MON_SCALE_P = Settings.MON_SCALE;
                Settings.TXT_ZOOM_P  = Settings.TXT_ZOOM;
                // RESTORE WORKING PARAMETERS
                Settings.DEV_DPI     = saved_DEV_DPI;
                Settings.DEV_H       = saved_DEV_H;
                Settings.DEV_W       = saved_DEV_W;
                Settings.DEV_ZOOM    = saved_DEV_ZOOM;
                Settings.MON_SCALE   = saved_MON_SCALE;
                Settings.TXT_ZOOM    = saved_TXT_ZOOM;
                // DEL PROFILES TABS
                tabsCollection.del_tabs_named( NotePane.PANEL_NAME_PROFILE );
                // UI VISIBILITY
                set_input_ip_visibility      ( false );
                set_input_dpi_visibility     ( true  );
                // UI STATUS
                del_highlight( control_profiles ); //control_profiles.TextBox.ForeColor  = control_profiles.ForeColor;
                //if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0)) tabsCollection.tabs_container_ResumeLayout();
                saveSettings("set_profiles_state("+ profiles_state +")");
            }
            tabsCollection.show_UI_state_targets_only( UI_state );
            update_MainForm("set_profiles_state("+ profiles_state +")");
        }
        //}}}
        // data @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        private void add_profile_tabs() //{{{
        {
            // UI NORMALIZE

            Settings.ImportMode    = Settings.IMPORT_MODE_REPLACE; // do not add
            Settings.ImportOffsetX = 0;
            Settings.ImportOffsetY = 0;

            Settings.Clear_Profiles_Dict();

            add_profile_load_tabs();

            //add_profile_display_tabs();

            // PROFILE USAGE {{{
            string      tab_name;
            string      tab_value;
            NotePane    np;

            Point xy_max= tabsCollection.get_Visible_xy_max( NotePane.TYPE_SHORTCUT );

            tab_name         = NotePane.PANEL_NAME_PROFILE  +"_USAGE";
            // .................0000000000000 1111 2222222 333333 4444444444444444444 5555555 66666666666666666
            tab_value        = "type=RICHTEXT|tag=|zoom=1.2|xy_wh=|text="+ tab_name +"|color=1|tt=Profiles usage";
            np               = tabsCollection.import_tab_line(tab_name, tab_value);
            np._tb.Text = "";
            tabsCollection.update_panel(tab_name, TabsCollection.TAB_USAGE_TEXT);
            xy_max           = tabsCollection.get_Visible_xy_max( NotePane.TYPE_SHORTCUT );
        //  np        .xy_wh =             "1,"+ (xy_max.Y+1) +",30,15";
            np        .xy_wh = (xy_max.X+1)+","+           1  +",50,30";

            log("EVENTS", "@@@ add_profile_tabs:\n"+np.ToString());
            //}}}

        }
        //}}}
        private void add_profile_load_tabs() //{{{
        {
            // PROFILE FOLDER {{{
            string      tab_name;
            string      tab_tag;
            string      tab_text;
            string      tab_value;
            NotePane    np;
            Point       xy_max;


            tab_name    = NotePane.PANEL_NAME_PROFILE  +"_FOLDER";
            tab_tag     = "SHELL "+ Settings.ProfilesFolder + Path.DirectorySeparatorChar;
            tab_text    = "\nPROFILES FOLDER";
            // ............0000000000000 11111111111111111 222222 333333 4444444444444444444 5555555 66666666666666666
            tab_value   = "type=SHORTCUT|tag="+ tab_tag +"|zoom=1|xy_wh=|text="+ tab_text +"|color=1|tt=OPEN PROFILES FOLDER";
            np          = tabsCollection.import_tab_line(tab_name, tab_value);
            np.Label    = NotePane.PANEL_PROFILE_PREFIX+ tab_text.Replace("\n", @"\line ");
            np.xy_wh    = "1,1,50,4";

            log("EVENTS", "@@@ add_profile_tabs:\n"+np.ToString());
            //}}}

            Dictionary<string, string> profiles_Dict               = Settings.Get_Profiles_Dict();
            Dictionary<string, string>.KeyCollection profile_names = profiles_Dict.Keys;

            // TOP LEFT & #COLS
            xy_max       = tabsCollection.get_Visible_xy_max( NotePane.TYPE_SHORTCUT );
            int        x = 1           ;
            int        y = 6; 
            int     cols = 1+ (int)Math.Sqrt( (double)profiles_Dict.Count );
            TabsCollection.ResetAutoPlace_x_y_cols(x, y, cols);

            // TODO put index in front of other profiles in each sub folder XXX
            // SELECT PROFILES (i.e. replace current tabs)
            int    idx;
            string profile_dir;
            string profile_base;
            string current_dir = "";
            foreach(string profile_name in profile_names)
            {
                profile_base     = profile_name;
                // PROFILE SUB-FOLDER {{{
                profile_dir      = "";
                idx              = profile_name.LastIndexOf("/");
                if(idx > 0) {
                    profile_dir  = profile_name.Substring(0, idx);
                    profile_base = profile_name.Substring(   idx+1);
                }
                if(profile_dir  != current_dir)
                {
                    current_dir  = profile_dir;

                    xy_max       = tabsCollection.get_Visible_xy_max( NotePane.TYPE_SHORTCUT );
                    TabsCollection.ResetAutoPlace_x_y_cols(1, xy_max.Y+1, cols);

                    tab_name     = NotePane.PANEL_NAME_PROFILE  +"_SUB_FOLDER_"+current_dir;
                    tab_tag      = "SHELL "+ Settings.ProfilesFolder + Path.DirectorySeparatorChar+ current_dir;
                    tab_text     = current_dir;
                    tab_value    = "type=SHORTCUT|tag="+ tab_tag +"|zoom=.8|xy_wh=|text="+ tab_text +"|color=2|tt=OPEN PROFILES SUB-FOLDER";
                    np           = tabsCollection.import_tab_line(tab_name, tab_value);
                    np.Label     = NotePane.PANEL_PROFILE_PREFIX+ tab_text;

                    xy_max       = tabsCollection.get_Visible_xy_max( NotePane.TYPE_SHORTCUT );
                    TabsCollection.ResetAutoPlace_x_y_cols(2, xy_max.Y+0, cols);
                }
                //}}}
                tab_name     = NotePane.PANEL_NAME_PROFILE +"_IMPORT_"+ profile_name;
                tab_tag      =                              "PROFILE "+ profile_name;
                tab_text     =                                          profile_base;
                // ..........0000000000000 11111111111111111 222222 333333 4444444444444444444 5555555 66666666666666666666
                tab_value    = "type=SHORTCUT|tag="+ tab_tag +"|zoom=.8|xy_wh=|text="+ tab_text +"|color=|tt=Load this profile";
                np           = tabsCollection.import_tab_line(tab_name, tab_value);
                np.Label     = NotePane.PANEL_PROFILE_PREFIX + tab_text;
            //  np.rebuild_rtf();   // FIXME .. is that required or not ?
                log("EVENTS", "@@@ add_profile_load_tabs:\n"+ np.ToString());
            }

        }
        //}}}
        private void add_profile_display_tabs() //{{{
        {
            // PROFILE LIST {{{
            string      tab_name;
            string      tab_tag;
            string      tab_text;
            string      tab_value;
            NotePane    np;

            Point xy_max= tabsCollection.get_Visible_xy_max( NotePane.TYPE_SHORTCUT );
            int left    =  xy_max.X+2;

            tab_name    = NotePane.PANEL_NAME_PROFILE  +"_LIST";
            tab_tag     = "";
            tab_text    = "\nEdit txt files";
                // ........0000000000000 11111111111111111 222222 333333 4444444444444444444 5555555 66666666666666666
            tab_value   = "type=SHORTCUT|tag="+ tab_tag +"|zoom=2|xy_wh=|text="+ tab_text +"|color=1|tt=click below...";
            np          = tabsCollection.import_tab_line(tab_name, tab_value);
            np.Label    = NotePane.PANEL_PROFILE_PREFIX+ tab_text.Replace("\n", @"\line ");
            xy_max      = tabsCollection.get_Visible_xy_max( NotePane.TYPE_SHORTCUT );
            np.xy_wh    = left+",1,30,4";

            log("EVENTS", "@@@ add_profile_tabs:\n"+np.ToString());
            //}}}

            Dictionary<string, string> profiles_Dict               = Settings.Get_Profiles_Dict();
            Dictionary<string, string>.KeyCollection profile_names = profiles_Dict.Keys;

            // TOP LEFT & #ROWS
            xy_max       = tabsCollection.get_Visible_xy_max( NotePane.TYPE_SHORTCUT );
            int        x = left;
            int        y = 6;
            int     cols = 1+ (int)Math.Sqrt( (double)profiles_Dict.Count );
            TabsCollection.ResetAutoPlace_x_y_cols(x, y, cols);

            // SHELL PROFILES (i.e. display profile tabs)
            foreach(string profile_name in profile_names)
            {
                tab_name  = NotePane.  PANEL_NAME_PROFILE +"_"    + profile_name;
                tab_tag   = "SHELL "+ profiles_Dict[profile_name];
                tab_text  = profile_name
                    .Replace("_"," ")
                    //.Replace("PROFILES","")
                    //.Replace("PROFILE" ,"")
                    //.Replace("Profiles","")
                    //.Replace("Profile" ,"")
                    //.Replace("profiles","")
                    //.Replace("profile" ,"")
                    .Trim();

                // ..........0000000000000 11111111111111111 222222 333333 4444444444444444444 5555555 666666666666666666666666
                tab_value = "type=SHORTCUT|tag="+ tab_tag +"|zoom=1|xy_wh=|text="+ tab_text +"|color=|tt=Display this profile:";
                np        = tabsCollection.import_tab_line(tab_name, tab_value);
                np.Label  = tab_text;
            //  np.rebuild_rtf();   // FIXME .. is that required or not ?
                log("EVENTS", "@@@ add_profile_tabs:\n"+ np.ToString());
            }

        }
        //}}}
        //}}}

        private void control_add_Click()// {{{
        {
            log("EVENTS", "control_add_Click");

            if(Settings.scaledGridSize >= TabsCollection.TAB_GRID_MIN) {
                NotePane np = tabsCollection.update_richtext();
                np.Locked = false;
            }
            else
                warn_Announce("Grid is too small for INSERTING!\n.. "+ TabsCollection.TAB_GRID_MIN +"x"+ TabsCollection.TAB_GRID_MIN +" minimum gridsize required");
        }
        //}}}
        private void control_activate_Click()// {{{
        {
            log("EVENTS", "control_activate_Click");

            if(TabsCollection.Sel_tab_count() > 0)
                tabsCollection. sel_tab_activate_toggle ();
            else
                warn_Announce("You have to select some tabs to activate/deactivate (Layout->left click)!");
        }
        //}}}
        private void control_update_profile_Click()// {{{
        {
            log("EVENTS", "control_update_profile_Click");

            // export current settings contents
            if(panel_XPort == null) panel_XPort_build();
            panel_XPort.Show();
            panel_XPort_export();

            string file_path
                = Settings.ProfilesFolder
                + Path.DirectorySeparatorChar
                + ((Settings.PROFILE != "") ? Settings.PROFILE : Settings.PROFILE_DRAFT)
                +".txt";

            export_profile( file_path );
        }
        //}}}

        public void control_layout_Click()// {{{
        {
            log("EVENTS", "control_layout_Click");

            // friend state toggle
//            if(UI_state == Settings.STATE_EDIT) set_edit_state( false );

            // do not interfere 
            if((UI_state != 0) && (UI_state != Settings.STATE_LAYOUT) && (UI_state != Settings.STATE_EDIT))
                return;

            bool layout_state = ((UI_state & Settings.STATE_LAYOUT) != 0);
            log("EVENTS", "...layout_state=["+layout_state+"]");

            set_layout_state( !layout_state );
        }
        //}}}
        private void set_layout_state(bool layout_state)// {{{
        {
            log("EVENTS", "set_layout_state("+ layout_state +"):");

bool was_in_edit_state = (UI_state == Settings.STATE_EDIT);
        //  if(layout_state)        UI_state |=  Settings.STATE_LAYOUT;    // bit set
        //  else                    UI_state &= ~Settings.STATE_LAYOUT;    // bit clear
            if(layout_state)        UI_state = Settings.STATE_LAYOUT;
            else                    UI_state = 0;
            NotePane.Set_UI_state(  UI_state );

            // show toggled current layout_state
            if(layout_state) {
if(!was_in_edit_state) {
                // FUNC
                tabsCollection.set_tabs_locked_state( false );
                // TOOLS
                set_input_ip_visibility      ( false );
                set_input_dpi_visibility     ( false );
                // UI
                show_layout_grid();
                this.Refresh();
}
else if(control_edit != null) { control_edit.Label = NotePane.CONTROL_LABEL_EDIT; del_highlight( control_edit ); }
                // BUTTON
                if(control_edit           != null) control_edit.Label = NotePane.CONTROL_LABEL_EDIT;
                if(control_update_profile != null) control_update_profile.TT = Settings.PROFILE;
                control_layout.Label = NotePane.CONTROL_LABEL_LAYOUT +"\n done";
                control_layout.TextBox.Refresh();

                // allow-deny event handlers {{{
                if((Settings.APP_NAME.ToUpper().IndexOf("SERVER") >= 0)) {
                    v_splitter_show  ( tabs_container   );
                    v_splitter_Toggle( panels_container );
                }
                else {
                    v_splitter_show  ( panels_container );
                    v_splitter_Toggle( tabs_container   );
                }
                //}}}
                add_highlight( control_layout        );
            }
            else {
                // FUNC
                tabsCollection.set_tabs_locked_state( true );
                // TOOLS
                set_input_ip_visibility      ( false );
                set_input_dpi_visibility     ( true  );
                // UI
                hide_layout_grid();
                this.Refresh();
                // BUTTON
                if(control_edit != null) control_edit.Label = NotePane.CONTROL_LABEL_EDIT;
                control_layout.Label                        = NotePane.CONTROL_LABEL_LAYOUT;
                control_layout.TextBox.Refresh();
                del_highlight( control_layout        ); //control_layout.TextBox.ForeColor = control_layout.ForeColor;

                // allow-deny event handlers {{{
                if((Settings.APP_NAME.ToUpper().IndexOf("SERVER") >= 0)) {
                    v_splitter_show  ( tabs_container   );
                    v_splitter_Toggle( panels_container );
                }
                else {
                    v_splitter_show  ( panels_container );
                    v_splitter_Toggle( tabs_container   );
                }
                //}}}
            }

            update_MainForm("set_layout_state("+ layout_state +")");
            sel_Announce();
        }
        //}}}
        // show_layout_image {{{
        private void show_layout_image()
        {
            if(scaled_BackgroundImage != null)
                tabs_container.BackgroundImage    = scaled_BackgroundImage;

            else if(saved_tabs_container_BackgroundImage != null)
                tabs_container.BackgroundImage    = saved_tabs_container_BackgroundImage;

            this.Refresh();
        }
        //}}}

        public void control_edit_Click()// {{{
        {
            log("EVENTS", "control_edit_Click");

            // friend state toggle
//            if(UI_state == Settings.STATE_LAYOUT) set_layout_state( false );

            // do not interfere
            if((UI_state != 0) && (UI_state != Settings.STATE_EDIT) && (UI_state != Settings.STATE_LAYOUT))
                return;

            bool edit_state = ((UI_state & Settings.STATE_EDIT) != 0);
            log("EVENTS", "...edit_state=["+edit_state+"]");

            set_edit_state( !edit_state );
        }
        //}}}
        private void set_edit_state(bool edit_state)// {{{
        {
            log("EVENTS", "set_edit_state("+ edit_state +"):");

bool was_in_layout_state = (UI_state == Settings.STATE_LAYOUT);
            if(edit_state)          UI_state = Settings.STATE_EDIT;
            else                    UI_state = 0;
            NotePane.Set_UI_state(  UI_state );

            // show toggled current edit_state
            if(edit_state) {
if(!was_in_layout_state) {
                // FUNC
                tabsCollection.set_tabs_locked_state( false );
                // TOOLS
                set_input_ip_visibility      ( false );
                set_input_dpi_visibility     ( false );
                // UI
                this.Refresh();
}
else if(control_layout != null) { control_layout.Label = NotePane.CONTROL_LABEL_EDIT; del_highlight( control_layout ); }

                // BUTTON

                control_layout.Label = NotePane.CONTROL_LABEL_LAYOUT;
                if(control_edit != null) control_edit.Label = NotePane.CONTROL_LABEL_EDIT +"\n done";
                if(control_edit != null) control_edit.TextBox.Refresh();

                tabsCollection.edit_sel_tab();
                add_highlight( control_edit );
            }
            else {
                // FUNC
                tabsCollection.set_tabs_locked_state( true );
                // TOOLS
                set_input_ip_visibility      ( false );
                set_input_dpi_visibility     ( true  );
                // UI
                this.Refresh();
                // BUTTON

                del_highlight( control_edit           ); //control_edit.TextBox.ForeColor  = control_edit.ForeColor;
            //  del_highlight( control_update_profile );

                if(control_edit != null) control_edit.Label = NotePane.CONTROL_LABEL_EDIT;
                if(control_edit != null) control_edit.TextBox.Refresh();
                control_layout.Label = NotePane.CONTROL_LABEL_LAYOUT;
            }
            tabsCollection.show_UI_state_targets_only( UI_state );
            update_MainForm("set_edit_state("+ edit_state +")");
            sel_Announce();
        }
        //}}}

        // update_MainForm {{{
        // updates timer {{{
        private System.Timers.Timer update_Timer;
        private const int           UPDATE_TIMER_DELAY = 100;
        private string              update_MainForm_callers;
        // update_MainForm {{{
        private void update_MainForm(string caller)
        {
            //log("EVENTS", "update_MainForm("+ caller +")");

            if(update_Timer == null) {
                update_Timer = new System.Timers.Timer(UPDATE_TIMER_DELAY);
                update_Timer.AutoReset = false;
                update_Timer.Elapsed += update_MainForm_Timer_Elapsed;
            }
            else update_Timer.Stop();

            update_MainForm_callers += caller+"\n";

            // retrigger a delayed post
        //  update_Timer.Change(UPDATE_TIMER_DELAY, 0); // delay, period
            update_Timer.Start(); // re-start ?
/*
:!start explorer "https://msdn.microsoft.com/en-us/library/393k7sb1(v=vs.110).aspx"
:!start explorer "https://msdn.microsoft.com/en-us/library/system.timers.timer(v=vs.110).aspx"
:!start explorer "https://msdn.microsoft.com/en-us/library/yz1c7148.aspx"
:!start explorer "http://stackoverflow.com/questions/1042312/how-to-reset-a-timer-in-c"
*/
        }
        //}}}
        // update_MainForm_Timer_Elapsed {{{
        private void update_MainForm_Timer_Elapsed(Object source, ElapsedEventArgs e)
        {
            //log("EVENTS", "update_MainForm_Timer_Elapsed("+ e.SignalTime +")");
        //  do_update_MainForm( update_MainForm_callers );
            Invoke( (MethodInvoker)delegate() { do_update_MainForm( update_MainForm_callers ); });
        }
        //}}}
        //}}}
        private void do_update_MainForm(string callers)
        {
            if(Settings.MainFormInstance != this) return;   // UI-build early stage

            log("EVENTS", "do_update_MainForm:\n"+ callers +"\n");
            update_MainForm_callers = ""; // consume reported callers

            // APPLICATION - FORM BACKGROUND COLOR f(UI_state) {{{

            Color darkestColor          = NotePane.GetDarkestBackColor();
            Color lightestColor         = NotePane.GetLightestBackColor();

            mainForm_BackColor          = Util.ColorPalette.GetColorLightnessTo(darkestColor,  5);
            panel_dpi         .BackColor= mainForm_BackColor;
            panel_ip          .BackColor= mainForm_BackColor;
            controls_container.BackColor= mainForm_BackColor;
            v_splitter.BackColor        = mainForm_BackColor;

            tabs_view         .BackColor= Util.ColorPalette.GetColorLightnessTo(darkestColor , 20);
            panels_container  .BackColor= Util.ColorPalette.GetColorLightnessTo(lightestColor, 40);
            controls_container.BackColor= Util.ColorPalette.GetColorLightnessTo(lightestColor, 15);

            switch(UI_state)
            {
            //  case Settings.STATE_EDIT     :  this    .BackColor = Util.ColorPalette.GetColorLightnessTo(lightestColor,30);   break;
            //  case Settings.STATE_LAYOUT   :  this    .BackColor = Util.ColorPalette.GetColorLightnessTo(darkestColor, 20);   break;
                case Settings.STATE_EDIT     :  this    .BackColor = Color.DarkSlateGray; break;
                case Settings.STATE_LAYOUT   :  this    .BackColor = Color.Navy;    break;

                case Settings.STATE_SETTINGS :  this    .BackColor = Util.ColorPalette.GetColorLightnessTo(darkestColor, 50);   break;
                case Settings.STATE_IMPORT   :
                case Settings.STATE_EXPORT   :
                case Settings.STATE_PROFILE  :  this    .BackColor = Color.Red;                                                 break;
                default:
                                                if(!clientServerForm.ClientServerIsOnLine())
                                                    this.BackColor = Color.DarkSlateGray;
                                                else
                                                    this.BackColor = mainForm_BackColor;
                                                break;
            }
            //}}}
            // control_logging Label {{{
            if( Settings.LOGGING ) {
                control_logging.Label = NotePane.CONTROL_LABEL_LOGGING;
                add_highlight( control_logging  );
            }
            else {
                control_logging.Label = NotePane.CONTROL_LABEL_LOGGING_OFF;
            }

            add_ClientServer_highlight( control_exit );

            //}}}

            // EARLY POST-INSTALLED EXECUTION
            if( !initial_help_check_done_once )
            {
                initial_help_check_done_once = true;
                Dictionary<string, string> profiles_Dict = Settings.Get_Profiles_Dict();
                if(profiles_Dict.Count == 0)
                    control_help_Click(true);
            }
        }
        private static bool initial_help_check_done_once = false;
        //}}}
        // set_connection_state {{{
        public void set_connection_state(string caller)
        {
            log("EVENTS", "set_connection_state("+ caller +"):");

            bool disconnected = !clientServerForm.ClientServerIsOnLine();
            if( !disconnected ) {
                set_input_ip_visibility      ( false );
                set_input_dpi_visibility     ( true  );
            }
            else {
                set_input_ip_visibility      ( true  );
                set_input_dpi_visibility     ( false );
            }

            if(disconnected)
                tabsCollection.show_UI_state_targets_only_disconnected( UI_state );
            else
                tabsCollection.show_UI_state_targets_only( UI_state );

            update_MainForm("set_connection_state("+ disconnected +")");
        }
        //}}}

        // update_COMM_DASH {{{
        public void update_COMM_DASH(string caller)
        {
        //  log("EVENTS", "update_COMM_DASH("+ caller +"):");
            // APPLICATION - EXIT BUTTON TIMESTAMP // {{{
            string timestamp         = Settings.RetrieveLinkerTimestamp();
            this.Text                = Settings.APP_TITLE+" "+timestamp;

            string procName = ((Settings.APP_NAME.ToUpper().IndexOf("SERVER") >= 0)) ? "Server" : "Designer";
            control_exit.LabelPrefix = @"\fs40 ";
            control_exit.Label
                = @"Exit "      + Settings.PROJECT_NAME +@"\line "+ procName
                + @"\line\fs20 "+ timestamp;

            // }}}

            // APPLICATION - SYSTEM-TRAY // {{{
            if(notifyIcon != null)
                notifyIcon.Text = get_BOM();

            //}}}

            // Client-Server current state
            update_control_settings_Label();

            clientServerForm.update_COMM_DASH();
        }
        //}}}
        private void update_control_settings_Label() //{{{
        {
            // Client-Server current state
            control_settings.TT    = clientServerForm.ClientServerStatus();
            control_settings.Label
            = NotePane.CONTROL_LABEL_SETTINGS
            + @"\fs20 "
            + "\n IP "     + Settings.IP
            + "\n port "   + Settings.Port
            + "\n MAC "    + Settings.MAC
            + "\n SUBNET " + Settings.SUBNET
            ;
        }
        //}}}
        // adjust_layout_dpi {{{

        private bool                            MainForm_LayoutSuspended = false;
        private void MainForm_SuspendLayout() { MainForm_LayoutSuspended = true ; }
        private void MainForm_ResumeLayout () { MainForm_LayoutSuspended = false; }

        public void adjust_layout_dpi(string caller)
        {
            if(MainForm_LayoutSuspended) {
                log("EVENTS","\n");
                log("EVENTS","@@@ caller=["+ caller +"]\n");
                log("EVENTS","@ MainForm_LayoutSuspended\n");
                log("EVENTS","@@@\n");
                return;
            }

            Settings.KEY_VAL_HISTORY += "\n- adjust_layout_dpi("+ caller +")";

            // SYNCHRONZE SETTINGS UI CONTROLS {{{
            log("EVENTS","\n");
            log("EVENTS","@@@ caller=["+ caller +"]\n");
            log("EVENTS","@                 SOURCE: ["+ Settings.SOURCE        +"]\n");

        MainForm_SuspendLayout();
            combo_palettes .SelectedItem = Settings.PALETTE;
            combo_txt_zoom .SelectedItem = Settings.TXT_ZOOM.ToString();
            combo_dev_zoom .SelectedItem = Settings.DEV_ZOOM.ToString();
            combo_dev_dpi  .SelectedItem = Settings.DEV_DPI.ToString();
            combo_dev_wh   .SelectedItem = Settings.DEV_W.ToString()+"x"+Settings.DEV_H.ToString();
            combo_mon_scale.SelectedItem = string.Format("{0,2:F}", Settings.MON_SCALE);
        MainForm_ResumeLayout ();

            //}}}

            // PROPAGATE SETTINGS {{{
            // After conforming to current Settings
            // ...share them with all but originating instances
            bool this_is_the_server_app = (Settings.APP_NAME.ToUpper().IndexOf("SERVER"  ) >= 0);
            bool this_is_the_design_app = (Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0);
            bool source_identified      = (Settings.SOURCE != "");
            bool sent_by_this_app
                =  this_is_the_design_app && (Settings.SOURCE == "DESIGN")
                || this_is_the_server_app && (Settings.SOURCE == "SERVER")
                ;
            bool to_be_dispatched
                =  !source_identified                       // really comes from this UI control
                && (UI_state != Settings.STATE_PROFILE)     // don't share profile PROFILES_TABLE handling
            //  .. (!sent_by_this_app && source_identified && this_is_the_server_app) //... that's the Server's job
                ;

            log("EVENTS","@ this_is_the_server_app: ["+ this_is_the_server_app +"]\n");
            log("EVENTS","@ this_is_the_design_app: ["+ this_is_the_design_app +"]\n");
            log("EVENTS","@ ......sent_by_this_app: ["+ sent_by_this_app       +"]\n");
            log("EVENTS","@ .....source_identified: ["+ source_identified      +"]\n");
            log("EVENTS","@ ......to_be_dispatched: ["+ to_be_dispatched       +"]\n");
            log("EVENTS","@@@\n");

            if( to_be_dispatched )
                clientServerForm.dispatch_KEY_VAL();

            // CONSUME THIS SETTINGS CHANGE SOURCE SIGNATURE
            Settings.SOURCE = "";

            //}}}

            // GEOMETRY {{{
            log("EVENTS","@         MON_SCALE = [x"+ Settings.MON_SCALE +"]\n");
            log("EVENTS","@   DEV_W x DEV_H   = ["+  Settings.DEV_W     +" x "+ Settings.DEV_H   +"]\n");
            log("EVENTS","@ MON_DPI / DEV_DPI = ["+  Settings.MON_DPI   +" / "+ Settings.DEV_DPI +"]\n");

            Settings.ratio          = Settings.MON_SCALE * (double)Settings.MON_DPI / (double)Settings.DEV_DPI;
            Settings.scaledGridSize = (int)(TabsCollection.TAB_GRID_S * Settings.ratio);

            int w = (int)(Settings.DEV_W * Settings.ratio);
            int h = (int)(Settings.DEV_H * Settings.ratio);

            tabs_container.Width  = w;
            tabs_container.Height = h;

            //}}}

            // BACKGROUND {{{
            //  double dark_factor   = (double)w / 3840; // between 0.1 and 0.9
            double dark_factor   = (double)w / (2*Screen.PrimaryScreen.Bounds.Width); // lighter when too big
            tabs_container.BackColor = Util.ColorPalette.GetColorDarker(this.BackColor, dark_factor);

            log("EVENTS","@ tabs_container WxH=["+w+"x"+h+"] dark_factor=["+ dark_factor +"]\n");

            // RESCALE BACKGROUND IMAGE TO REFLECT [DESIGNER / DEVICE] PIXEL RATIO. {{{
            if(saved_tabs_container_BackgroundImage != null)
            {
                int new_w = (int)(saved_tabs_container_BackgroundImage.Width  * Settings.MON_SCALE);
                int new_h = (int)(saved_tabs_container_BackgroundImage.Height * Settings.MON_SCALE);
                scaled_BackgroundImage = new Bitmap(saved_tabs_container_BackgroundImage, new Size(new_w, new_h));
            }
            //}}}

            //}}}

            // TABS {{{
            if(tabsCollection != null)
                tabsCollection.dev_dpi_Changed();

            //}}}

            log("EVENTS","@@@\n\n");
            update_COMM_DASH("adjust_layout_dpi("+ caller +")");
            dpi_Announce();

            if(notifyIcon != null)
                notifyIcon.Text = get_BOM();
        }
        //}}}

        // hide_layout_grid {{{
        private void hide_layout_grid()
        {
            tabs_container.BackgroundImage            = null;
        }
        //}}}
        // show_layout_grid {{{
        private void show_layout_grid()
        {
            show_layout_image();

        }
        //}}}

        private void control_exit_Click()// {{{
        {
            log("EVENTS", "control_exit_Click");

            exit("control_exit_Click");  // ... see OnFormClosing
        }
        //}}}
        private void control_logging_Click()// {{{
        {
            log("EVENTS", "control_logging_Click");

            set_logging( !Settings.LOGGING );
        }
        //}}}
        private void control_clear_Click()// {{{
        {
            tabsCollection.clear_app_panels_content();
        }
        //}}}
        public  void clear_app_panels_content()// {{{
        {
            tabsCollection.clear_app_panels_content();
        }
        //}}}

        // FIREWALL {{{
        private void control_firewall_Click()// {{{
        {
            log("EVENTS", "control_firewall_Click");

            // DONE ALREADY
            NotePane np = tabsCollection.get_tab_NP( NotePane.PANEL_NAME_NETSH );
            if((np != null) && np.Visible) {
                np.Hide();
                return;
            }

            // CLEAR PREVIOUS LOG
            if(np != null) np.Text = "";

            // PERFORM TASK
            clientServerForm.ClientServer_set_Firewall_Rule();

            // SHOW LAST LOG
            np = tabsCollection.get_tab_NP( NotePane.PANEL_NAME_NETSH );
            if((np != null) && !np.Visible) {
                np = tabsCollection.get_tab_NP( NotePane.PANEL_NAME_NETSH );
                np.Show();
            }

        }
        //}}}
        private bool panel_firewall_isVisible()// {{{
        {
            NotePane np = tabsCollection.get_tab_NP( NotePane.PANEL_NAME_NETSH );
            return ((np != null) && np.Visible);
        }
        //}}}
        //}}}
        // HELP {{{
        private void control_help_Click() { control_help_Click(false); }
        private void control_help_Click(bool found_no_user_profile)// {{{
        {
            log("EVENTS", "control_help_Click");

            if((panel_help != null) && panel_help.Visible)
            {
                panel_help.Hide();
                return;
            }

            if( panel_help == null)
            {
                panel_help                      = tabsCollection.update_panel(Settings.APP_TITLE, NotePane.TXT_PLACEHOLDER);
                panel_help.SuspendLayout();

                panel_help        .BackColor    = Color.DarkRed;
                panel_help.TextBox.BackColor    = Color.DarkRed;
                panel_help        .ForeColor    = Color.White;
                panel_help.TextBox.ForeColor    = Color.White;
                panel_help.ReadOnly             = true;
                panel_help.LabelPrefix          = @"\fs28\b ";
                panel_help. TextPrefix          = @"\b0\fs20 ";
                panel_help.ResumeLayout();

                panel_help.Text =
                    (found_no_user_profile ? Settings.APP_INIT_TEXT : "")
                    + Settings.APP_HELP_TEXT
                    ;
            }

            v_splitter_ShowParent( panels_container );
            panel_help.maximize_xy_wh();
            panel_help.Show();
        }
        //}}}
        private bool panel_help_isVisible()// {{{
        {
            return ((panel_help != null) && panel_help.Visible);
        }
        //}}}
        private void showPopupHelpForm()// {{{
        {
            if(PopupHelpForm == null)
            {
                PopupHelpForm = new Util.HelpForm(Settings.APP_NAME+" help");
                PopupHelpForm.setHelp(Settings.APP_TITLE, Settings.APP_HELP_TEXT);
            }
            PopupHelpForm.Show();
        }
        //}}}
        public void closePopupHelpForm()// {{{
        {
            if(PopupHelpForm != null) {
                PopupHelpForm.Close();
                PopupHelpForm = null;
            }
        }
        //}}}
        //}}}
        // EVENTS [callback] [notify] [cancel] {{{
        // callback {{{
        public void callback(Object caller, string detail)
        {
            // UI CONTROLS - [EXIT] [ADD] [CLEAR] [HELP] [LAYOUT] [LOGGING] {{{

            //MessageBox.Show("callback caller=["+ caller +"]" , "callback detail=["+ detail +"]" , MessageBoxButtons.OKCancel , MessageBoxIcon.Information);

            bool from_a_common_control =
                (   caller == control_exit            )
            //  || (caller == control_clear           )
                || (caller == control_firewall        )
                || (caller == control_help            )
                || (caller == control_logging         )
                // (caller == control_save            )
                || (caller == control_settings        )

                || (caller == control_add             )
                || (caller == control_activate        )
                || (caller == control_edit            )
                || (caller == control_layout          )

                || (caller == control_profiles        )
                || (caller == control_index           )
                || (caller == control_update_profile  )
                || (caller == control_export          )
                || (caller == control_export_profile  )
                || (caller == control_export_to_file  )
                || (caller == control_export_clipboard)
                || (caller == control_import          )
                || (caller == control_import_insert   )
                || (caller == control_import_overlay  )

                || (caller == control_palettes        )
                || (caller == panel_palettes          )
                ;

            //}}}
            // MAINFORM HANDLERS {{{
            if(from_a_common_control)
            {
                log("EVENTS", "callback:\n.detail=["+ detail +"]\n.caller=["+ caller +"]\n");

                // SESSION
                if     (caller == control_exit              ) control_exit_Click();
                else if(caller == control_settings          ) control_settings_Click();
            //  else if(caller == control_save              ) control_save_Click();

                // TABS
                else if(caller == control_layout            ) control_layout_Click();
                else if(caller == control_add               ) control_add_Click();
                else if(caller == control_activate          ) control_activate_Click();

                else if(caller == control_edit              ) control_edit_Click();

                // COLOR PALETTES
                else if(caller == control_palettes          ) control_palettes_Click();
                else if(caller == panel_palettes            ) palettes_Changed(detail);

                else if(caller == control_help              ) control_help_Click();
                else if(caller == control_firewall          ) control_firewall_Click();
                else if(caller == control_logging           ) control_logging_Click();
            //  else if(caller == control_clear             ) control_clear_Click();

                // PROFILE-IMPORT-EXPORT
                else if(caller == control_profiles          ) control_profiles_Click();
                else if(caller == control_index             ) control_index_Click();
                else if(caller == control_update_profile    ) control_update_profile_Click();

                else if(caller == control_import            ) control_import_Click(caller);
                else if(caller == control_import_insert     ) control_import_Click(caller);
                else if(caller == control_import_overlay    ) control_import_Click(caller);

                else if(caller == control_export            ) control_export_Click(caller);
                else if(caller == control_export_profile    ) control_export_Click(caller);
                else if(caller == control_export_to_file    ) control_export_Click(caller);
                else if(caller == control_export_clipboard  ) control_export_Click(caller);

            }
            //}}}
            // DESIGNER OR SERVER HANDLERS {{{
            else {
                clientServerForm.ClientServer_callback(caller, detail);
            }
            //}}}
        }
        //}}}
        // notify {{{
        public void notify(NotePane np, string detail)
        {
            log("EVENTS", "notify:\n.detail=["+ detail +"]\n.np=["+ np +"]");
            // LAYOUT (MOVE) {{{
            if(      detail == "OnMouseUp")
            {
/*
                bool too_small_for_tabs = (Settings.scaledGridSize < TabsCollection.TAB_GRID_MIN);
                if((np.Parent == tabs_container) && too_small_for_tabs)
                {
                    warn_Announce("Grid is too small for POSITIONING!\n.. "+ TabsCollection.TAB_GRID_MIN +"x"+ TabsCollection.TAB_GRID_MIN +" minimum gridsize required");
                    np.move_back();
                }
*/
                //else {
                    log("EVENTS", "move_tab ["+ np.Name +"]");
                    tabsCollection.move_tab( np );

                    if(     (np.Type == NotePane.TYPE_RICHTEXT)
                        ||  (np.Type == NotePane.TYPE_SHORTCUT)
                        || ((np.Type == NotePane.TYPE_DASH    ) && (np.Parent == tabs_container))
                      )
                        set_profile_needs_saving("notify move_tab");
                //}
            }
            // }}}
            // LAYOUT (DELETE) {{{
            else if(detail == "OnMouseClick.Right")
            {
                if(UI_state == Settings.STATE_LAYOUT)
                {
                    if(     (np.Type == NotePane.TYPE_RICHTEXT)
                        ||  (np.Type == NotePane.TYPE_SHORTCUT)
                        || ((np.Type == NotePane.TYPE_DASH    ) && (np.Parent == tabs_container  ))
                        || ((np.Type == NotePane.TYPE_PANEL   ) && (np.Parent == panels_container))
                      ) {
                        log("EVENTS", "del_tab ["+ np.Name +"]");
                        tabsCollection.del_tab( np );
                        set_profile_needs_saving("notify del_tab");
                    }
                }
            }
            // }}}
            // LAYOUT (ADD) - EDIT {{{
            else if(detail == "OnMouseClick")
            {
                if(UI_state == Settings.STATE_LAYOUT)
                {
                    if     (np == control_layout        ) control_layout_Click();         // exit state
                    else if(np == control_edit          ) control_edit_Click();           // friend state toggle
                    else if(np == control_update_profile) control_update_profile_Click(); // embeded function

                    else if(np == control_color0 ) tabsCollection.sel_tab_color( "");
                    else if(np == control_color1 ) tabsCollection.sel_tab_color("1");
                    else if(np == control_color2 ) tabsCollection.sel_tab_color("2");
                    else if(np == control_color3 ) tabsCollection.sel_tab_color("3");
                    else if(np == control_color4 ) tabsCollection.sel_tab_color("4");
                    else if(np == control_color5 ) tabsCollection.sel_tab_color("5");
                    else if(np == control_color6 ) tabsCollection.sel_tab_color("6");
                    else if(np == control_color7 ) tabsCollection.sel_tab_color("7");
                    else if(np == control_color8 ) tabsCollection.sel_tab_color("8");
                    else if(np == control_color9 ) tabsCollection.sel_tab_color("9");
                    else if(np == control_color10) tabsCollection.sel_tab_color("10");
                    else if(np == control_color11) tabsCollection.sel_tab_color("11");

                    else if((np.Type == NotePane.TYPE_RICHTEXT)
                        ||  (np.Type == NotePane.TYPE_SHORTCUT)
                        || ((np.Type == NotePane.TYPE_DASH    ) && (np.Parent == tabs_container))
                    ) {
                        bool control = ((Form.ModifierKeys & Keys.Control) != 0);
                        bool shift   = ((Form.ModifierKeys & Keys.Shift  ) != 0);
                        bool alt     = ((Form.ModifierKeys & Keys.Alt    ) != 0);

                        log("EVENTS", "OnMouseClick: control=["+ control +"] shift=["+ shift +"] alt=["+ alt +"]");

                        if     ( control ) tabsCollection.sel_tab_toggle( np );
                        else if(   shift ) tabsCollection.sel_tab_add   ( np );
                        else if(     alt ) tabsCollection.sel_tab_extend( np );
                        else               tabsCollection.sel_tab       ( np );
                    }
                }
                else if(UI_state == Settings.STATE_EDIT)
                {
                    if     (np == control_edit          ) control_edit_Click();           // exit state
                    else if(np == control_layout        ) control_layout_Click();         // friend state toggle
                    else if(np == control_update_profile) control_update_profile_Click(); // embeded function

                    else if((np.Type == NotePane.TYPE_RICHTEXT)
                        ||  (np.Type == NotePane.TYPE_SHORTCUT)
                        || ((np.Type == NotePane.TYPE_DASH    ) && (np.Parent == tabs_container))
                    ) {
                        log("EVENTS", "edit_tab ["+ np.Name +"]");
                        tabsCollection.edit_tab( np );
                        set_profile_needs_saving("notify edit_tab");
                    }
                }
            }
            // }}}
        }
        //}}}
        // MainForm_cancel {{{
        public void MainForm_cancel_STATE_PROFILE() {
            if(UI_state == Settings.STATE_PROFILE) control_profiles_Click();
        }
        public void MainForm_cancel()
        {
            if(v_splitter.Width == 0) return;    // UI not built yet .. Yes! that can happen, I saw it when hidden

            UI_state_canceling = true;

            if     (UI_state == Settings.STATE_EDIT        ) control_edit_Click();
            else if(UI_state == Settings.STATE_LAYOUT      ) control_layout_Click();

            else if(panel_firewall_isVisible()             ) control_firewall_Click();
            else if(panel_help_isVisible()                 ) control_help_Click();

            // batched (to be canceled) commits
            else if(UI_state == Settings.STATE_EXPORT      ) control_export_Click();
            else if(UI_state == Settings.STATE_IMPORT      ) control_import_Click();
            else if(UI_state == Settings.STATE_PROFILE     ) control_profiles_Click();
            else if(UI_state == Settings.STATE_SETTINGS    ) control_settings_Click();

            else if(panel_palettes_isVisible()             ) control_palettes_Click();

            // ...may not have been hidden by import-export when canceled (as intended .. to keep it visible)
            else if(panel_XPort_isVisible()                ) panel_XPort.Hide();

            else if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0)) {
                // ... and >>alternate between user-set-split and center-split
                if(v_splitter.SplitterDistance != Settings.SplitX)   v_splitter.SplitterDistance  = Settings.SplitX;
                else                                                v_splitter.SplitterDistance  = v_splitter.Width / 2;
            }

            update_COMM_DASH("MainForm_cancel");
            dpi_Announce();

        //  clear_app_panels_content();

            clientServerForm.cancel();

            UI_state_canceling = false;
        }
        //}}}

        //}}}
        // SIDE [palettes txt_zoom dev_zoom dev_dpi dev_wh mon_scale] {{{

        // PALETTES {{{
        // palettes_Changed {{{
        private void palettes_Changed(string caller)
        {
log("EVENTS", "palettes_Changed("+ caller +")");

            Settings.KEY_VAL_HISTORY += "\n- palettes_Changed("+ caller +")";

            // UPDATE COMBO {{{
            combo_palettes.Items.Clear();
            string[] names = NotePane.GetColorPaletteNames();

            log("EVENTS", "palettes_Changed: names.Length["+names.Length+"]");

            for(int i=0; i<names.Length; ++i)
                combo_palettes.Items.Add( names[i] );

            //}}}

            // display current selection
            //combo_palettes.SelectedItem = Settings.PALETTE;

        // Colored items: https://msdn.microsoft.com/en-us/library/system.windows.forms.drawitemeventargs.drawfocusrectangle(v=vs.110).aspx
        }
        //}}}
        // combo_palettes_SelectedIndexChanged {{{
        private void combo_palettes_SelectedIndexChanged(object sender, EventArgs e)
        {
log("EVENTS", "combo_palettes_SelectedIndexChanged("+combo_palettes.SelectedIndex+")");
            Settings.KEY_VAL_HISTORY += "\n- combo_palettes_SelectedIndexChanged";

            combo_palettes_Select( combo_palettes.SelectedIndex );
        }
        //}}}
        // combo_palettes_Select {{{
        private void combo_palettes_Select(int index)
        {
log("EVENTS", "combo_palettes_Select("+index+")");

            string[] names = NotePane.GetColorPaletteNames();

log("EVENTS", "names["+ index +"]=["+names[index]+"]");
log("EVENTS", "Settings.PALETTE=["+ Settings.PALETTE +"]");


            //if(names[index] != Settings.PALETTE) {
                Settings.PALETTE = names[index];
                log("EVENTS", "combo_palettes_Select: Settings PALETTE #"+ (index+1) +" ["+Settings.PALETTE+"]");

                // READYING CURRENT PALETTE SELECTION
                NotePane.Select_ActiveColorPalette( Settings.PALETTE );

                update_MainForm("combo_palettes_Select("+index+")");
                tabsCollection.palette_Changed();
                adjust_layout_dpi("combo_palettes_Select");
            //}
        }
        //}}}
        // user_palette_Select {{{
        private void user_palette_Select(int choice, int offset)
        {
log("EVENTS", "user_palette_Select(choice=["+ choice +"], offset=["+ offset +"])");
            Settings.KEY_VAL_HISTORY += "\n- user_palette_Select(choice=["+ choice +"], offset=["+ offset +"])";

            int index;
            if(offset != 0) index = combo_palettes.SelectedIndex + offset;
            else            index = choice-1;

            if(index >= combo_palettes.Items.Count) index = combo_palettes.Items.Count -1;
            if(index <  0)                          index = 0;

            combo_palettes.SelectedIndex = index;
        }
        //}}}
        private void check_Settings_PALETTE() //{{{
        {
log("EVENTS", "check_Settings_PALETTE:");
log("EVENTS", "..............Settings.PALETTE=["+ Settings.PALETTE            +"]");
log("EVENTS", "...combo_palettes.SelectedItem=["+ combo_palettes.SelectedItem +"]");

            // XXX Settings.PALETTE changed through POLL KEY_VAL
            // should not be embeded into dpi handling (currently handling KEY_VAL input)
            if((string)combo_palettes.SelectedItem != Settings.PALETTE)
            {
                string[] names = NotePane.GetColorPaletteNames();
                for(int i=0; i < names.Length; ++i) {
                    if(names[i] == Settings.PALETTE) {
                        if(combo_palettes.SelectedIndex != i)
                            combo_palettes.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        //}}}
        //...
        // control_palettes_Click {{{
        private void control_palettes_Click()
        {
            log("EVENTS", "control_palettes_Click");

            // build {{{
            if( panel_palettes == null)
            {
                panel_palettes                   = tabsCollection.update_panel(NotePane.PANEL_NAME_PALETTES, NotePane.TXT_PLACEHOLDER);
                panel_palettes.SuspendLayout();
                panel_palettes        .BackColor = this.BackColor;
                panel_palettes.TextBox.BackColor = this.BackColor;
                panel_palettes        .ForeColor = this.ForeColor;
                panel_palettes.TextBox.ForeColor = this.ForeColor;
                panel_palettes.LabelPrefix       = @"\fs28\b ";
                panel_palettes. TextPrefix       = @"\b0\fs16 ";
                panel_palettes.Visible = false;
                panel_palettes.ResumeLayout();
            }
            //}}}
            // PALETTES START .. (populate with current ColorPaletteDict) {{{
            if( !panel_palettes.Visible ) {
                // get
                panel_palettes.Text = "";
                tabsCollection.update_panel(panel_palettes.Name, NotePane.GetColorPaletteLines());
                panel_palettes.Tag = panel_palettes.TextBox.Text; // SET MARKED PALETTES-DATA
                // show
                panel_palettes.Show();
                v_splitter_show( panel_palettes );
            }
            //}}}
            // PALETTES END {{{
            else {
                // apply changes
                if( !UI_state_canceling )
                {
                    if(panel_palettes.TextBox.Text != (string)panel_palettes.Tag)    // CHECK MARKED PALETTES-DATA
                    {
                        NotePane.LoadColorPaletteLines( panel_palettes );
                        NotePane.SaveSettings();
                        palettes_Changed("control_palettes_Click");
                    }
                }
                // hide
                panel_palettes.Hide();
                v_splitter_show( tabs_container );
            }
            //}}}

        }
        //}}}
        // panel_palettes_isVisible {{{
        private bool panel_palettes_isVisible()
        {
            return ((panel_palettes != null) && panel_palettes.Visible);
        }
        //}}}
        //}}}
        // TXT_ZOOM {{{
        // txt_zoom_Changed {{{
        public  void txt_zoom_Changed(string caller)
        {
            log("EVENTS", "txt_zoom_Changed("+ caller +")");

            Settings.KEY_VAL_HISTORY += "\n- txt_zoom_Changed("+ caller +")";

            // INITIALIZE COMBO {{{
            if(combo_txt_zoom.Items.Count < 1)
            {
                log("EVENTS", "txt_zoom_Changed: Settings.TXT_ZOOMS.Length: "+Settings.TXT_ZOOMS.Length);
                combo_txt_zoom.Items.Clear();

                for(int i=0; i < Settings.TXT_ZOOMS.Length; ++i)
                    combo_txt_zoom.Items.Add(  Settings.TXT_ZOOMS[i].ToString() );
            }
            // }}}

            // display current selection
            combo_txt_zoom.SelectedItem = Settings.TXT_ZOOM.ToString();

        }
        //}}}
        // combo_txt_zoom_SelectedIndexChanged {{{
        private void combo_txt_zoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.KEY_VAL_HISTORY += "\n- combo_txt_zoom_SelectedIndexChanged("+ combo_txt_zoom.SelectedIndex +")";
            combo_txt_zoom_Select( combo_txt_zoom.SelectedIndex );
        }
        //}}}
        // combo_txt_zoom_Select {{{
        private void combo_txt_zoom_Select(int index)
        {
            log("EVENTS", "combo_txt_zoom_Select("+ index +")");
            double txt_zoom = Settings.TXT_ZOOM;
            switch(index+1) {
                case  1: txt_zoom = Settings.TXT_ZOOM_1; break;
                case  2: txt_zoom = Settings.TXT_ZOOM_2; break;
                case  3: txt_zoom = Settings.TXT_ZOOM_3; break;
                case  4: txt_zoom = Settings.TXT_ZOOM_4; break;
                case  5: txt_zoom = Settings.TXT_ZOOM_5; break;
                case  6: txt_zoom = Settings.TXT_ZOOM_6; break;
                case  7: txt_zoom = Settings.TXT_ZOOM_7; break;
                case  8: txt_zoom = Settings.TXT_ZOOM_8; break;
                case  9: txt_zoom = Settings.TXT_ZOOM_9; break;
                case 10: txt_zoom = Settings.TXT_ZOOM_0; break;
            }
            if((txt_zoom != Settings.TXT_ZOOM) || !OnLoad_done) {
                Settings.TXT_ZOOM = txt_zoom;
                log("EVENTS", "combo_txt_zoom_Select: Settings TXT_ZOOM #"+ (index+1) +" ["+ Settings.TXT_ZOOM +"]");

            //  tabsCollection.adjust_zoom( Settings.TXT_ZOOM );    // NotePane ZoomFactor
                tabsCollection.set_zoom( Settings.TXT_ZOOM );       // NotePane ZoomFactor
                adjust_layout_dpi("combo_txt_zoom_Select");         // rendering only
            }
        }
        //}}}
        // user_txt_zoom_Select {{{
        private void user_txt_zoom_Select(int choice, int offset)
        {
            Settings.KEY_VAL_HISTORY += "\n- user_txt_zoom_Select(choice=["+ choice +"], offset=["+ offset +"])";

            int index;
            if(offset != 0) index = combo_txt_zoom.SelectedIndex + offset;
            else            index = choice-1;

            if(index >= combo_txt_zoom.Items.Count) index = combo_txt_zoom.Items.Count -1;
            if(index <  0)                          index = 0;

            combo_txt_zoom.SelectedIndex = index;
        }
        //}}}
        //}}}
        // DEV_ZOOM {{{
        // dev_zoom_Changed {{{
        public  void dev_zoom_Changed(string caller)
        {
            Settings.KEY_VAL_HISTORY += "\n- dev_zoom_Changed("+ caller +")";

            // INITIALIZE COMBO {{{
            if(combo_dev_zoom.Items.Count < 1)
            {
                log("EVENTS", "dev_zoom_Changed: Settings.DEV_ZOOMS.Length: "+Settings.DEV_ZOOMS.Length);
                combo_dev_zoom.Items.Clear();

                for(int i=0; i < Settings.DEV_ZOOMS.Length; ++i)
                    combo_dev_zoom.Items.Add( Settings.DEV_ZOOMS[i].ToString() );
            }
            // }}}

            // display current selection
            combo_dev_zoom.SelectedItem = Settings.DEV_ZOOM.ToString();

        }
        //}}}
        // combo_dev_zoom_SelectedIndexChanged {{{
        private void combo_dev_zoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.KEY_VAL_HISTORY += "\n- combo_dev_zoom_SelectedIndexChanged("+ combo_dev_zoom.SelectedIndex +")";
            combo_dev_zoom_Select( combo_dev_zoom.SelectedIndex );
        }
        //}}}
        // combo_dev_zoom_Select {{{
        private void combo_dev_zoom_Select(int index)
        {
            Settings.KEY_VAL_HISTORY += "\n- combo_dev_zoom_Select("+ index +")";

            double dev_zoom = Settings.DEV_ZOOM;
            switch(index+1) {
                case  1: dev_zoom = Settings.DEV_ZOOM_1; break;
                case  2: dev_zoom = Settings.DEV_ZOOM_2; break;
                case  3: dev_zoom = Settings.DEV_ZOOM_3; break;
                case  4: dev_zoom = Settings.DEV_ZOOM_4; break;
                case  5: dev_zoom = Settings.DEV_ZOOM_5; break;
                case  6: dev_zoom = Settings.DEV_ZOOM_6; break;
                case  7: dev_zoom = Settings.DEV_ZOOM_7; break;
                case  8: dev_zoom = Settings.DEV_ZOOM_8; break;
                case  9: dev_zoom = Settings.DEV_ZOOM_9; break;
                case 10: dev_zoom = Settings.DEV_ZOOM_0; break;
            }
            if((dev_zoom != Settings.DEV_ZOOM) || !OnLoad_done) {
                Settings.DEV_ZOOM = dev_zoom;
                log("EVENTS", "combo_dev_zoom_Select: Settings DEV_ZOOM #"+ (index+1) +" ["+ Settings.DEV_ZOOM +"]");

            //  tabsCollection.adjust_adjust( Settings.DEV_ZOOM );  // NotePane ZoomFactor
                adjust_layout_dpi("combo_dev_zoom_Select");         // rendering only
            }

        }
        //}}}
        //...no user_dev_zoom_Select
        //}}}
        // DEV_DPI {{{
        // dev_dpi_Changed {{{
        public  void dev_dpi_Changed(string caller)
        {
        log("EVENTS", "dev_dpi_Changed("+ caller +")");
            Settings.KEY_VAL_HISTORY += "\n- dev_dpi_Changed("+ caller +")";

            // INITIALIZE COMBO {{{
            if(combo_dev_dpi.Items.Count < 1)
            {
                log("EVENTS", "dev_dpi_Changed: Settings.DEV_DPI_NAMES.Length: "+Settings.DEV_DPI_NAMES.Length);
                combo_dev_dpi.Items.Clear();

                for(int i=0; i < Settings.DEV_DPI_NAMES.Length; ++i)
                    combo_dev_dpi.Items.Add(  Settings.DEV_DPI_NAMES[i] );
            }
            // }}}

            // display current selection
            combo_dev_dpi.SelectedItem = Settings.DEV_DPI.ToString();

            check_Settings_PALETTE();
        }
        //}}}
        // combo_dpi_SelectedIndexChanged {{{
        private void combo_dpi_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.KEY_VAL_HISTORY += "\n- combo_dpi_SelectedIndexChanged("+ combo_dev_dpi.SelectedIndex +")";
            combo_dev_dpi_Select( combo_dev_dpi.SelectedIndex );
        }
        //}}}
        // combo_dev_dpi_Select {{{
        private void combo_dev_dpi_Select(int index)
        {
        //  log("EVENTS", "combo_dev_dpi_Select("+ index +")");
            int dev_dpi = Settings.DEV_DPI;
            switch(index+1) {
                case 1: dev_dpi = Settings.DEV_DPI_1; break; // 96dpi
                case 2: dev_dpi = Settings.DEV_DPI_2; break;
                case 3: dev_dpi = Settings.DEV_DPI_3; break;
                case 4: dev_dpi = Settings.DEV_DPI_4; break;
                case 5: dev_dpi = Settings.DEV_DPI_5; break;
                case 6: dev_dpi = Settings.DEV_DPI_6; break;
                case 7: dev_dpi = Settings.DEV_DPI_7; break;
                case 8: dev_dpi = Settings.DEV_DPI_8; break; // 640dpi
            }
            if((dev_dpi != Settings.DEV_DPI) || !OnLoad_done) {
                Settings.DEV_DPI = dev_dpi;
                log("EVENTS", "combo_dev_dpi_Select: Settings DEV_DPI #"+ (index+1) +" ["+ Settings.DEV_DPI +"]");


                adjust_layout_dpi("combo_dev_dpi_Select");
            }

        }
        //}}}
        // user_dev_dpi_Select {{{
        private void user_dev_dpi_Select(int choice, int offset)
        {
            Settings.KEY_VAL_HISTORY += "\n- user_dev_dpi_Select(choice=["+ choice +"], offset=["+ offset +"])";

            int index;
            if(offset != 0) index = combo_dev_dpi.SelectedIndex + offset;
            else            index = choice-1;

            if(index >= combo_dev_dpi.Items.Count) index = combo_dev_dpi.Items.Count -1;
            if(index <  0)                         index = 0;

            combo_dev_dpi.SelectedIndex = index;
        }
        //}}}
        //}}}
        // DEV_WH {{{
        // dev_wh_Changed {{{
        private  void dev_wh_Changed(string caller)
        {
        log("EVENTS", "dev_wh_Changed("+ caller +")");
            Settings.KEY_VAL_HISTORY += "\n- dev_wh_Changed("+ caller +")";

            // INITIALIZE COMBO {{{
            if(combo_dev_wh.Items.Count < 1)
            {
                log("EVENTS", "dev_wh_Changed: Settings.DEV_WIDTHS.Length: "+Settings.DEV_WIDTHS.Length);
                combo_dev_wh.Items.Clear();

                for(int i=0; i < Settings.DEV_WIDTHS.Length; ++i)
                    combo_dev_wh.Items.Add(  Settings.DEV_WIDTHS[i]+"x"+Settings.DEV_HEIGHTS[i] );
            }
            // }}}

            // display current selection
            combo_dev_wh.SelectedItem = Settings.DEV_W.ToString()+"x"+Settings.DEV_H.ToString();
        }
        //}}}
        // combo_dev_wh_SelectedIndexChanged {{{
        private void combo_dev_wh_SelectedIndexChanged(object sender, EventArgs e)
        {
        //log("EVENTS", "combo_dev_wh_SelectedIndexChanged("+combo_dev_wh.SelectedIndex+")");
            Settings.KEY_VAL_HISTORY += "\n- combo_dev_wh_SelectedIndexChanged("+  combo_dev_wh.SelectedIndex +")";
            combo_dev_wh_Select( combo_dev_wh.SelectedIndex );
        }
        //}}}
        // combo_dev_wh_Select {{{
        private void combo_dev_wh_Select(int index)
        {
        log("EVENTS", "combo_dev_wh_Select("+index+")");
            int dev_w = Settings.DEV_W;
            int dev_h = Settings.DEV_W;
            switch(index+1) {
                case  1: dev_w = Settings.DEV_W_1 ; dev_h = Settings.DEV_H_1 ; break;
                case  2: dev_w = Settings.DEV_W_2 ; dev_h = Settings.DEV_H_2 ; break;
                case  3: dev_w = Settings.DEV_W_3 ; dev_h = Settings.DEV_H_3 ; break;
                case  4: dev_w = Settings.DEV_W_4 ; dev_h = Settings.DEV_H_4 ; break;
                case  5: dev_w = Settings.DEV_W_5 ; dev_h = Settings.DEV_H_5 ; break;
                case  6: dev_w = Settings.DEV_W_6 ; dev_h = Settings.DEV_H_6 ; break;
                case  7: dev_w = Settings.DEV_W_7 ; dev_h = Settings.DEV_H_7 ; break;
                case  8: dev_w = Settings.DEV_W_8 ; dev_h = Settings.DEV_H_8 ; break;
                case  9: dev_w = Settings.DEV_W_9 ; dev_h = Settings.DEV_H_9 ; break;
                case 10: dev_w = Settings.DEV_W_10; dev_h = Settings.DEV_H_10; break;
                case 11: dev_w = Settings.DEV_W_11; dev_h = Settings.DEV_H_11; break;
                case 12: dev_w = Settings.DEV_W_12; dev_h = Settings.DEV_H_12; break;
                case 13: dev_w = Settings.DEV_W_13; dev_h = Settings.DEV_H_13; break;
                case 14: dev_w = Settings.DEV_W_14; dev_h = Settings.DEV_H_14; break;
                case 15: dev_w = Settings.DEV_W_15; dev_h = Settings.DEV_H_15; break;
            }
            if((dev_w != Settings.DEV_W) || (dev_h != Settings.DEV_H) || !OnLoad_done) {
                Settings.DEV_W = dev_w;
                Settings.DEV_H = dev_h;
                log("EVENTS", "combo_dev_wh_Select: Settings DEV_WH #"+ (index+1) +" ["+ Settings.DEV_W+"x"+Settings.DEV_H +"]");

                adjust_layout_dpi("combo_dev_wh_Select");
            }

        }
        //}}}
        // user_dev_wh_Select {{{
        private void user_dev_wh_Select(int choice, int offset)
        {
        //  log("EVENTS", "user_dev_wh_Select(choice=["+choice+"], offset=["+ offset +"])");
            Settings.KEY_VAL_HISTORY += "\n- user_dev_wh_Select(choice=["+ choice +"], offset=["+ offset +"])";

            int dev_w = Settings.DEV_W;
            int dev_h = Settings.DEV_H;
            // current choice with an offset {{{
            if(offset != 0) {
                for(choice=1; choice <= Settings.DEV_WIDTHS.Length; ++choice) {
                    if(    (   dev_w == Settings.DEV_WIDTHS [choice-1])
                        && (   dev_h == Settings.DEV_HEIGHTS[choice-1])
                      ) {
                        choice += offset;
                        break;
                    }
                }
            }
            //}}}
            // cap in range
            if     (choice < 1                         ) choice = 1;
            else if(choice > Settings.DEV_WIDTHS.Length) choice = Settings.DEV_WIDTHS.Length;

            combo_dev_wh.SelectedIndex = choice-1;
        }
        //}}}
        //}}}
        // MON_SCALE {{{
        // mon_scale_Changed {{{
        public  void mon_scale_Changed(string caller)
        {
            Settings.KEY_VAL_HISTORY += "\n- mon_scale_Changed("+ caller +")";

            // INITIALIZE COMBO {{{
            if(combo_mon_scale.Items.Count < 1)
            {
                log("EVENTS", "mon_scale_Changed: Settings.MON_SCALES.Length: "+Settings.MON_SCALES.Length);
                combo_mon_scale.Items.Clear();

                for(int i=0; i < Settings.MON_SCALES.Length; ++i)
                    combo_mon_scale.Items.Add( string.Format("{0,2:F}", Settings.MON_SCALES[i]) );
            }
            // }}}

            // display current selection
            combo_mon_scale.SelectedItem = string.Format("{0,2:F}", Settings.MON_SCALE);

        }
        //}}}
        // combo_mon_scale_SelectedIndexChanged {{{
        private void combo_mon_scale_SelectedIndexChanged(object sender, EventArgs e)
        {
        //  log("EVENTS", "combo_mon_scale_SelectedIndexChanged("+ combo_mon_scale.SelectedIndex +")");
            Settings.KEY_VAL_HISTORY += "\n- combo_mon_scale_SelectedIndexChanged("+ combo_mon_scale.SelectedIndex +")";
            combo_mon_scale_Select( combo_mon_scale.SelectedIndex );
        }
        //}}}
        // combo_mon_scale_Select {{{
        private void combo_mon_scale_Select(int index)
        {
        log("EVENTS", "combo_mon_scale_Select("+ index +")");
            double mon_scale = Settings.MON_SCALE;
            switch(index+1) {
                case  1: mon_scale = Settings.MON_SCALE_1; break; // 0.10
                case  2: mon_scale = Settings.MON_SCALE_2; break;
                case  3: mon_scale = Settings.MON_SCALE_3; break;
                case  4: mon_scale = Settings.MON_SCALE_4; break;
                case  5: mon_scale = Settings.MON_SCALE_5; break;
                case  6: mon_scale = Settings.MON_SCALE_6; break;
                case  7: mon_scale = Settings.MON_SCALE_7; break;
                case  8: mon_scale = Settings.MON_SCALE_8; break;
                case  9: mon_scale = Settings.MON_SCALE_9; break;
                case  0: mon_scale = Settings.MON_SCALE_0; break; // 4.00
            }
            if((mon_scale != Settings.MON_SCALE) || !OnLoad_done) {
                Settings.MON_SCALE = mon_scale;
                log("EVENTS", "combo_mon_scale_Select: Settings MON_SCALE #"+ (index+1) +" ["+ Settings.MON_SCALE +"]");

                adjust_layout_dpi("combo_mon_scale_Select");
            }

        }
        //}}}
        // user_mon_scale_Select {{{
        private void user_mon_scale_Select(int choice, int offset)
        {
        //log("EVENTS", "user_mon_scale_Select(choice=["+ choice +"], offset=["+ offset +"])");
            Settings.KEY_VAL_HISTORY += "\n- user_mon_scale_Select(choice=["+ choice +"], offset=["+ offset +"])";
            double  mon_scale = Settings.MON_SCALE;
            // current choice with an offset {{{
            if(offset != 0) {
                for(choice=1; choice <= Settings.MON_SCALES.Length; ++choice) {
                    if(mon_scale == Settings.MON_SCALES[choice-1]) {
                        choice += offset;
                        break;
                    }
                }
            }
            //}}}
            // cap in range
            if     (choice < 1                         ) choice = 1;
            else if(choice > Settings.MON_SCALES.Length) choice = Settings.MON_SCALES.Length;

            combo_mon_scale.SelectedIndex = choice-1;
        }
        //}}}
        //}}}

        //}}}
        // Form [KEYS .. WHEEL .. CLICK] {{{
        // Form_KeyDown {{{
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            Settings.KEY_VAL_HISTORY = " Form_KeyDown("+e.KeyCode.ToString() +")";
            // {{{
            //:!start explorer "https://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx"
            //:!start explorer "https://msdn.microsoft.com/en-us/library/ms171537(v=vs.110).aspx"
            //}}}

            // ESCAPE CANCEL {{{
            if(e.KeyCode == Keys.Escape)
            {
                if( e.Shift ) // used by NotePane to expand its Rtf
                    {
                    return;
                }

                if((UI_state == Settings.STATE_LAYOUT) && (TabsCollection.Sel_tab_count() > 0))
                {
                    tabsCollection.sel_tab_clear();
                    return;
                }

                NotePane np = tabsCollection.GetUsrNotePaneWaitingForEscape();
                if(np == null) {
                    e.SuppressKeyPress = true;
                    MainForm_cancel();
                    clientServerForm.cancel();
                }
                else {
                    //log("EVENTS", np.Name +" has focus");
                    //np.Flash();
                }

                return;
            }
            //}}}
            // CTRL A COPY {{{
            if(e.Control && (e.KeyCode == Keys.A)) {
                //log("EVENTS", "Form_KeyDown: SELECT ALL");
                if(UI_state == Settings.STATE_LAYOUT) {
                    e.SuppressKeyPress = true;
                    tabsCollection.sel_tab_add_all();
                }
                return;
            }
            //}}}
            // CTRL X COPY {{{
            if(e.Control && (e.KeyCode == Keys.X)) {
                //log("EVENTS", "Form_KeyDown: DELETE SELECTION");
                if(UI_state == Settings.STATE_LAYOUT) {
                    e.SuppressKeyPress = true;
                    tabsCollection.del_sel();
                }
                return;
            }
            //}}}
            // CTRL C COPY {{{
            if(e.Control && (e.KeyCode == Keys.C)) {
                //log("EVENTS", "Form_KeyDown: COPY");
                if(UI_state == Settings.STATE_LAYOUT) {
                    e.SuppressKeyPress = true;
                    tabsCollection.sel_tab_copy();
                }
                return;
            }
            //}}}
            // CTRL V PASTE {{{
            if(e.Control && (e.KeyCode == Keys.V)) {
                //log("EVENTS", "Form_KeyDown: PASTE");
                if(UI_state == Settings.STATE_LAYOUT) {
                    e.SuppressKeyPress = true;
                    tabsCollection.sel_tab_paste();
                }
                return;
            }
            //}}}
            // F4     EXIT {{{
            if(e.KeyCode == Keys.F4) {
                log("EVENTS", "Form_KeyDown: EXIT");
                e.SuppressKeyPress = true;
                control_exit_Click();
                return;
            }
            //}}}
            // F5     REFRESH - (just re-apply current values) {{{
            if(e.KeyCode == Keys.F5) {
                e.SuppressKeyPress = true;
                log("EVENTS", "Form_KeyDown: REFRESH");

                adjust_layout_dpi("Form_KeyDown");
                return;
            }
            //}}}
            // DEL    CLEAR {{{
            if(e.KeyCode == Keys.Delete) {
                log("EVENTS", "Form_KeyDown: CLEAR");
                e.SuppressKeyPress = true;
                clear_app_panels_content();
                return;
            }
            //}}}

            // [INDEXING] OR [OFFSETTING] SOME ATTRIBUTE {{{
            // from this point on: F6 or F7 or some uncommon modifier combo is rerquired
            if(    !e.Alt
                && !e.Control
            //  && !e.Shift
                && (e.KeyCode != Keys.F6) && (e.KeyCode != Keys.F7)
              )
                return;

            //log("EVENTS", "Form_KeyDown e.KeyCode=[e."+ e.KeyCode +"]");
            int choice = -1;
            int offset =  0;

            // SELECTION INDEX {{{
            switch(e.KeyCode)
            {
                case Keys.D1    : choice =  1; break;
                case Keys.D2    : choice =  2; break;
                case Keys.D3    : choice =  3; break;
                case Keys.D4    : choice =  4; break;
                case Keys.D5    : choice =  5; break;
                case Keys.D6    : choice =  6; break;
                case Keys.D7    : choice =  7; break;
                case Keys.D8    : choice =  8; break;
                case Keys.D9    : choice =  9; break;
                case Keys.D0    : choice = 10; break;
            }
            // }}}

            // SELECTION OFFSET [--F6 F7++] {{{
            switch(e.KeyCode)
            {
                case Keys.F6        : choice =  0; offset = -1; break;
                case Keys.F7        : choice =  0; offset = +1; break;
            }
            // }}}

            if((choice < 0) && (offset == 0)) return;
            //log("EVENTS", " ..choice=["+ choice +"]");
            //log("EVENTS", " ..offset=["+ offset +"]");
            e.SuppressKeyPress = true;

            //}}}
            // ......CONTROL+ALT.. PALETTE {{{
            if(!e.Shift &&  e.Control &&  e.Alt) {
                user_palette_Select(choice, offset);
                return;
            }
            //}}}
            // SHIFT+CONTROL...... TXT_ZOOM {{{
            if( e.Shift &&  e.Control && !e.Alt) {
                user_txt_zoom_Select(choice, offset);
                return;
            }
            //}}}
            // ..............ALT.. MON_SCALE {{{
            if(!e.Shift && !e.Control &&  e.Alt) {
                user_mon_scale_Select(choice, offset);
                return;
            }
            // }}}
            // SHIFT.........ALT.. DEV_DPI {{{
            if( e.Shift && !e.Control &&  e.Alt) {
                user_dev_dpi_Select(choice, offset);
                return;
            }
            //}}}
            // ......CONTROL...... DEV_W DEV_H {{{
            if(!e.Shift &&  e.Control && !e.Alt) {
                user_dev_wh_Select(choice, offset);
                return;
            }
            //}}}
        }
        //}}}
        // Form_MouseWheel {{{
        private void Form_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Settings.KEY_VAL_HISTORY = " Form_MouseWheel()";

            int lines = SystemInformation.MouseWheelScrollLines;
            int delta = SystemInformation.MouseWheelScrollLines / 120;

            info_Announce("lines=["+ lines +"] delta=["+ delta +"]");

            /*
               if(numberOfPixelsToMove != 0)
               {
               System.Drawing.Drawing2D.Matrix translateMatrix = new  System.Drawing.Drawing2D.Matrix();

               translateMatrix.Translate(0, numberOfPixelsToMove);

               mousePath.Transform(translateMatrix);
               }
               panel1.Invalidate();
             */
        }
        //}}}
        // Form_DoubleClick {{{
        private void Form_DoubleClick(object sender, EventArgs e)
        {
            // ENTER LAYOUT state
            if(UI_state == 0) {
                control_layout_Click();
            }
            // EXIT LAYOUT STATE
            else if(UI_state == Settings.STATE_LAYOUT) {
                if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
                {
                    // DESIGNER TOGGLES BETWEEN LAYOUT AND EDIT
                //  set_layout_state( false );
                    set_edit_state  ( true  );
                }
                else {
                    // ... WHILE SERVER EXITS LAYOUT STATE
                    MainForm_cancel();
                    clientServerForm.cancel();
                }
            }
            // EXIT EDIT STATE
            else if(UI_state == Settings.STATE_EDIT) {
            //  set_edit_state  ( false );
                set_layout_state( true  );
            }
            // use escape to get out of this loop

        }
        //}}}
        //}}}
        // Form [Is key down] {{{
        public static bool Is_Control_down()  { return ((Form.ModifierKeys & Keys.Control ) != 0); }
        public static bool Is_Shift_down()    { return ((Form.ModifierKeys & Keys.Shift   ) != 0); }
        public static bool Is_Alt_down()      { return ((Form.ModifierKeys & Keys.Alt     ) != 0); }

        public static bool Is_NumLock_down()  { return Form.IsKeyLocked( Keys.NumLock  ); }
        public static bool Is_CapsLock_down() { return Form.IsKeyLocked( Keys.CapsLock ); }

        //}}}
        // Form [MOVE .. RESIZE] {{{
        // variables {{{
        private const int   MAGNET_SIZE = 30;
        public  bool        locked      = false;
        private bool        RESIZABLE   =  true;

        private const int   GRIP_SIZE   = 30;

        private bool        dragging    = false;
        private bool        moving      = false;

    //  private bool        mouseDown   = false;
    //  private bool        mouseOver   = false;

        private bool        resize_B    = false;
        private bool        resize_L    = false;
        private bool        resize_R    = false;
        private bool        resize_T    = false;

        private Point       dragPoint   = new Point();
        private Rectangle   origin      = new Rectangle();
        //}}}
        protected override void OnMouseUp(MouseEventArgs e)// {{{
        {
            //base.OnMouseUp(e);
        //  mouseDown   = false;
            dragging    = false;
        //  Refresh();
        //  ResumeLayout(false);
        }
        //}}}
        protected override void OnMouseDown(MouseEventArgs e)// {{{
        {
            //base.OnMouseDown(e);
        //  mouseDown = true;

            if(!locked) {
                //SuspendLayout();

                if(dragging) return; // already on it

                dragging        = true;
                dragPoint       = PointToScreen(e.Location);
                origin.X        = Location.X;
                origin.Y        = Location.Y;
                origin.Width    = Size.Width;
                origin.Height   = Size.Height;

                if( RESIZABLE ) {
                    if(e.X > (Size.Width -GRIP_SIZE)) resize_R = true;
                    if(e.X <              GRIP_SIZE ) resize_L = true;
                    if(e.Y <              GRIP_SIZE ) resize_T = true;
                    if(e.Y > (Size.Height-GRIP_SIZE)) resize_B = true;
                }

                moving = (!resize_T && !resize_B && !resize_L && !resize_R);

                if         (resize_T) {
                    if     (resize_L)           Cursor.Current = Cursors.SizeNWSE;
                    else if(resize_R)           Cursor.Current = Cursors.SizeNESW;
                    else                        Cursor.Current = Cursors.SizeNS;
                }
                else if(    resize_B) {
                    if     (resize_L)           Cursor.Current = Cursors.SizeNESW;
                    else if(resize_R)           Cursor.Current = Cursors.SizeNWSE;
                    else                        Cursor.Current = Cursors.SizeNS;
                }
                else if(resize_L || resize_R)   Cursor.Current = Cursors.SizeWE;
                else                            Cursor.Current = Cursors.SizeAll;

                //Refresh();
            }

        }
        //}}}
        protected override void OnMouseMove(MouseEventArgs e)// {{{
        {
            //base.OnMouseMove(e);
            if(!locked) {
                // update {{{
                if(dragging) {
                    Point p = PointToScreen(e.Location);

                    int                         dx = p.X - dragPoint.X;
                    int                         W  =   Size.Width;
                    if(     resize_R)           W  = origin.Width  + dx;
                    else if(resize_L)           W  = origin.Width  - dx;
                    if(  W  <  MinimumSize.Width)
                    {
                        dx -= (MinimumSize.Width - W);
                        W   =  MinimumSize.Width;
                    }

                    int                         dy = p.Y - dragPoint.Y;
                    int                         H  =   Size.Height;
                    if(     resize_B)           H  = origin.Height + dy;
                    else if(resize_T)           H  = origin.Height - dy;
                    if(  H  < MinimumSize.Height)
                    {
                        dy -= (MinimumSize.Height - H);
                        H   =  MinimumSize.Height;
                    }

                    int L = Left;
                    int T = Top;

                    if(moving || resize_T) T  = origin.Y+dy;
                    if(moving || resize_L) L = origin.X+dx;

                    int s_w = Screen.PrimaryScreen.Bounds.Width;
                    int s_h = Screen.PrimaryScreen.Bounds.Height;

                    if(!resize_T && !resize_L && !resize_B && !resize_R)
                    {
                        if     (Math.Abs((s_w  ) - (L+ Width  )) < MAGNET_SIZE) L = s_w   -  Width; // [FRAME_RIGHT]  to [SCREEN_RIGHT ]
                        else if(Math.Abs((s_w/2) - (L+ Width  )) < MAGNET_SIZE) L = s_w/2 -  Width; // [FRAME_RIGHT]  to [SCREEN_CENTER]
                        else if(Math.Abs((s_w/2) -  L          ) < MAGNET_SIZE) L = s_w/2         ; // [FRAME_LEFT ]  to [SCREEN_CENTER]
                        else if(                    L            < MAGNET_SIZE) L = 0             ; // [FRAME_LEFT ]  to [SCREEN_LEFT  ]

                        if     (Math.Abs((s_h  ) - (T + Height)) < MAGNET_SIZE) T = s_h   - Height; // [FRAME_BOTTOM] to [SCREEN_BOTTOM]
                        else if(Math.Abs((s_h/2) - (T + Height)) < MAGNET_SIZE) T = s_h/2 - Height; // [FRAME_RIGHT]  to [SCREEN_CENTER]
                        else if(Math.Abs((s_h/2) -  T          ) < MAGNET_SIZE) T = s_h/2         ; // [FRAME_TOP   ] to [SCREEN_MIDDLE]
                        else if(                    T            < MAGNET_SIZE) T = 0             ; // [FRAME_TOP   ] to [SCREEN_TOP   ]
                    }
                    else if(resize_B || resize_R)
                    {
                         int sw875 = (int)(s_w * 0.875);
                         int sw750 = (int)(s_w * 0.750);
                         int sw625 = (int)(s_w * 0.625);
                         int sw500 = (int)(s_w * 0.500);
                         int sw375 = (int)(s_w * 0.375);
                         int sw250 = (int)(s_w * 0.250);
                         int sw125 = (int)(s_w * 0.125);

                         int sh875 = (int)(s_h * 0.875);
                         int sh750 = (int)(s_h * 0.750);
                         int sh625 = (int)(s_h * 0.625);
                         int sh500 = (int)(s_h * 0.500);
                         int sh375 = (int)(s_h * 0.375);
                         int sh250 = (int)(s_h * 0.250);
                         int sh125 = (int)(s_h * 0.125);

                         if( RESIZABLE ) {
                             if     (Math.Abs(s_w   -  W) < MAGNET_SIZE) W = s_w;
                             else if(Math.Abs(sw875 -  W) < MAGNET_SIZE) W = sw875;
                             else if(Math.Abs(sw750 -  W) < MAGNET_SIZE) W = sw750;
                             else if(Math.Abs(sw625 -  W) < MAGNET_SIZE) W = sw625;
                             else if(Math.Abs(sw500 -  W) < MAGNET_SIZE) W = sw500;
                             else if(Math.Abs(sw375 -  W) < MAGNET_SIZE) W = sw375;
                             else if(Math.Abs(sw250 -  W) < MAGNET_SIZE) W = sw250;
                             else if(Math.Abs(sw125 -  W) < MAGNET_SIZE) W = sw125;

                             if     (Math.Abs(s_h   -  H) < MAGNET_SIZE) H = s_h;
                             else if(Math.Abs(sh875 -  H) < MAGNET_SIZE) H = sh875;
                             else if(Math.Abs(sh750 -  H) < MAGNET_SIZE) H = sh750;
                             else if(Math.Abs(sh625 -  H) < MAGNET_SIZE) H = sh625;
                             else if(Math.Abs(sh500 -  H) < MAGNET_SIZE) H = sh500;
                             else if(Math.Abs(sh375 -  H) < MAGNET_SIZE) H = sh375;
                             else if(Math.Abs(sh250 -  H) < MAGNET_SIZE) H = sh250;
                             else if(Math.Abs(sh125 -  H) < MAGNET_SIZE) H = sh125;
                         }

                    }

                    Width  = W;
                    Height = H;
                    Top    = T;
                    Left   = L;

                }
                //}}}
                // cursor {{{
                else {
                    // handle {{{
                    resize_B = false;
                    resize_L = false;
                    resize_R = false;
                    resize_T = false;
                    if( RESIZABLE ) {
                        if(e.X > (Size.Width -GRIP_SIZE)) resize_R = true;
                        if(e.X <              GRIP_SIZE ) resize_L = true;
                        if(e.Y <              GRIP_SIZE ) resize_T = true;
                        if(e.Y > (Size.Height-GRIP_SIZE)) resize_B = true;
                    }
                    //}}}
                    if         (resize_T) {
                        if     (resize_L)           Cursor.Current = Cursors.SizeNWSE;
                        else if(resize_R)           Cursor.Current = Cursors.SizeNESW;
                        else                        Cursor.Current = Cursors.SizeNS;
                    }
                    else if(    resize_B) {
                        if     (resize_L)           Cursor.Current = Cursors.SizeNESW;
                        else if(resize_R)           Cursor.Current = Cursors.SizeNWSE;
                        else                        Cursor.Current = Cursors.SizeNS;
                    }
                    else if(resize_L || resize_R)   Cursor.Current = Cursors.SizeWE;
                    else                            Cursor.Current = Cursors.SizeAll;
                }
                //}}}
            }
        }

        //}}}
        //}}}
        // UTILITIES {{{
        // --------------------------------------------------------------------
        // --------------------------------------------------------------------
        // Announce {{{
        // dpi_Announce {{{
        public void dpi_Announce()
        {
            int w = (int)(Settings.DEV_W * Settings.ratio);
            int h = (int)(Settings.DEV_H * Settings.ratio);

            Announce.Add("MainForm"
                /* type */ , Announce.INFO
                /* Form */ , this
                /* text */ ,"Device @"+ Settings.DEV_DPI +"dpi\n"
                +           "= "+       Settings.DEV_W +"x"+ Settings.DEV_H +"\n"
                +           "GRID="+    Settings.scaledGridSize +"x"+ Settings.scaledGridSize +"\n"
                +           "\n"
                +           "Monitor @"+ Settings.MON_DPI+"dpi\n"
                +           "x "+ Settings.MON_SCALE+"\n"
                +           "= "+ w +"x"+ h
                +           "\n"
                +           "\n"
                +           "F5 Refresh\n"
                +           "F6 --\n"
                +           "F7 ++\n"
                +           "\n"
                +           "\n"
                +           "SHIFT+ALT\n"
                +           "...DEV Dpi\n"
                +           "\n"
                +           "CTRL\n"
                +           "...DEV W*H\n"
                +           "\n"
                +           "ALT\n"
                +           "...SCALE\n"
                +           "\n"
                +           "SHIFT+CTRL\n"
                +           "...ZOOM-TEXT\n"
                /* x    */ , 10
                /* y    */ , panel_dpi.Bottom
                /* time */ , Announce.DEFAULT_ANNOUNCE_ERASE_DELAY
                );
////{{{
//            Announce.Add("tabs_container"
//                /* Form */ , Announce.INFO
//                    /* Form */ , tabs_view
//                    /* text */ , Settings.DEV_W +"x"+ Settings.DEV_H +" @"+ Settings.DEV_DPI +"dpi\n"
//                    +                         w +"x"+              h +" x"+ Settings.MON_SCALE
//                    /* x    */ , tabs_container.Width / 2
//                    /* y    */ , tabs_container.Height
//                    /* time */ , Announce.DEFAULT_ANNOUNCE_ERASE_DELAY
//                );
////}}}
            Announce.Add("tabs_container"
                /* Form */ , Announce.INFO
                    /* Form */ , tabs_view
                    /* text */
                    , string.Format("{0,4}x{1,4}\t@ {2} DPI\n", Settings.DEV_W, Settings.DEV_H, Settings.DEV_DPI  )
                    + string.Format("{0,4}x{1,4}\tx {2} scaled", w             , h             , Settings.MON_SCALE)
                    +"\n\n= DEV_W x DEV_H\n* MON_SCALE\n@ DEV_DPI"
                    /* x    */ , tabs_container.Width / 2
                    /* y    */ , tabs_container.Height
                    /* time */ , Announce.DEFAULT_ANNOUNCE_ERASE_DELAY
                );

        }
        //}}}
        // sel_Announce {{{
        public void sel_Announce()
        {
            string action = "";
            if     (UI_state  == Settings.STATE_LAYOUT)   action = "Laying out ";
            else if(UI_state  == Settings.STATE_EDIT  )   action = "Editing ";
            else                                          action = "Using ";
            //else return;

            int count = TabsCollection.Sel_tab_count();
            switch( count )
            {
                case  0: action +=                   "tabs"; break;
                case  1: action += count + " selected tab" ; break;
                default: action += count + " selected tabs"; break;
            }

            if(count > 0) warn_Announce( action );
            else          info_Announce( action );

        }
        //}}}
        // info_Announce {{{
        public void info_Announce(string msg)
        {
            // control, msg, x, y, tic#
            Announce.Add("status_line"
                /* type */ , Announce.INFO
                /* Form */ , this
                /* text */ , msg.Replace("\n"," ")
                /* x    */ , 16
                /* y    */ , (int)(this.ClientSize.Height - Announce.BigFont.GetHeight() -4)
                /* time */ , 5);
        }
        //}}}
        // warn_Announce {{{
        public void warn_Announce(string msg)
        {
            Announce.Add("status_line"
                /* type */ , Announce.WARN
                /* Form */ , this
                /* text */ , msg.Replace("\n"," ")
                /* x    */ , (int)(this.ClientSize.Width/2)
                /* y    */ , (int)(this.ClientSize.Height - Announce.BigFont.GetHeight() -4)
                /* time */ , 5);
        }
        //}}}

        public class Announce
        {
            // CLASS {{{
            public const  int   INFO = 0;
            public const  int   WARN = 1;
            public static int   DEFAULT_ANNOUNCE_ERASE_DELAY = 30;
        //  public static Font  BigFont = new Font("Comic sans ms", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            public static Font  BigFont = new Font("Tahoma"       , 11F, System.Drawing.FontStyle.Bold   , System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            private static Dictionary<string, Object> Announce_Dictionary = new Dictionary<string, Object>();
            //}}}
            // INSTANCE {{{
            private Control     control;
            private Rectangle   announce_rect;
            private bool        has_been_drawn = false;
            private int         tics_to_live;
            private int         type;
            private int         x;
            private int         y;
            private string      announce_msg;

            // }}}
            // Add {{{
            public static void Add(string name, int type, Control control, string msg, int x, int y, int tics_to_live)
            {
                Announce an;
                if( Announce_Dictionary.ContainsKey(name) )
                {
                    an = (Announce)Announce_Dictionary[name];
                    if(an.control != null)
                        an.erase( name );
                    Announce_Dictionary.Remove( name );
                }

                an = new Announce(control, type, msg, x, y, tics_to_live);
                an.draw( name );

                // schedule a pending erase
                if(an.control != null)
                    Announce_Dictionary.Add(name, an);
            }
            //}}}
            // TIC [postponed] [erase] {{{
            public static void Tic(int tickCount)
            {
                List<string> consumed = new List<string>();

                //if(Announce_Dictionary.Count > 0) Logger.Log("EVENTS", "Announce pending=["+Announce_Dictionary.Count +"]");

                foreach(var item in Announce_Dictionary)
                {
                    string      name = (string)item.Key;
                    Announce    an   = (Announce)Announce_Dictionary[name];

                    if(an.control == null) {
                        consumed.Add(name);
                    }
                    else {
//Logger.Log("EVENTS", "an=["+ an.announce_msg +"]");
                        if     (--an.tics_to_live < 0  ) { an.erase( name ); consumed.Add(name); }
                        else if(  an.announce_msg != "") { an.draw ( name );                     }
                    }
                }

                //Logger.Log("EVENTS", "Announce consumed=["+  consumed.Count +"]");
                foreach(string item in consumed)
                {
//Logger.Log("EVENTS", "Announce.Tic: comsumed=["+ item +"]");
                    Announce_Dictionary.Remove(item);
                }

                //Logger.Log("EVENTS", "Announce remaining=["+Announce_Dictionary.Count +"]");

            }
            //}}}
            // CONSTRUCTOR {{{
            private Announce(Control control, int type, string msg, int x, int y, int tics_to_live)
            {
                this.announce_msg   = msg;
                this.control        = control;
                this.tics_to_live   = tics_to_live;
                this.type           = type;
                this.x              = x;
                this.y              = y;
            }
            //}}}
            // draw {{{
            private void draw(string name)
            {
//Logger.Log("EVENTS", "Announce.draw("+ name +")");
                // RESCHEDULE ONE TIC IF UI NOT READY {{{
                if(!OnLoad_done) {
                    Settings.KEY_VAL_HISTORY += "\n- [Announce]";
                    return;
                }
                //}}}
                //Settings.KEY_VAL_HISTORY += "\n- [A]";
                Graphics graphics  = null;
                using(graphics     = control.CreateGraphics())
                {
                    // CLEANUP {{{
                //  SizeF stringSize = new SizeF();
                //  SizeF stringSize = graphics.MeasureString(announce_msg, BigFont);

                    //  SolidBrush  b_brush     = new SolidBrush( control.BackColor );
                    //  graphics.FillRectangle(b_brush, x, y, stringSize.Width, stringSize.Height);

                    //  }}}
                    // DRAW MESSAGE & SCHEDULE ERASE {{{
                    if(announce_msg != "")
                    {
                        // DRAW MESSAGE // {{{

                        // BORDER // {{{
                    //  Pen b_pen = new Pen(Color.Red, 2);
                    //  graphics.DrawRectangle(b_pen, x, y, stringSize.Width, stringSize.Height);

                        // }}}
                        // STRING // {{{
                        Color f_color;
                        if(type == WARN) {
                            f_color = Color.Red;
                        }
                        else {
                            if(name == "status_line") f_color = Color.Green;
                            else                      f_color = Util.ColorPalette.GetColorLighter(Settings.MainFormInstance.BackColor, 0.8);
                        }
                        SolidBrush  f_brush     = new SolidBrush( f_color );
                        graphics.DrawString(    announce_msg, BigFont, f_brush, x, y);

                        // }}}
                        // }}}
                        // SCHEDULE AN ERASE JOB // {{{
                        if( !has_been_drawn ) {
                            SizeF stringSize = graphics.MeasureString(announce_msg, BigFont);
                            announce_rect    = new Rectangle(x, y, (int)stringSize.Width, (int)stringSize.Height);
                            has_been_drawn = true;
                        }
                        // }}}
                    }
                    // }}}
                }
            }
            //}}}
            // erase {{{
            private void erase(string name)
            {
//Logger.Log("EVENTS", "Announce.erase("+ name +")");
                if( !has_been_drawn )
                    return;
                Graphics graphics;
                using(graphics = control.CreateGraphics())
                {
                    SolidBrush  b_brush = new SolidBrush(control.BackColor);
                    graphics.FillRectangle(b_brush, announce_rect);
                    control = null;
                }
            }
            //}}}
        }
        //}}}
        // combo_DrawItem {{{
        private void combo_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox    combo   = sender as ComboBox;

            int         index   = e.Index >= 0 ? e.Index : 0;

            SolidBrush  back_brush;
            SolidBrush  fore_brush;

            if((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                back_brush = new SolidBrush( combo.ForeColor );
                fore_brush = new SolidBrush( combo.BackColor );
            }
            else {
                back_brush = new SolidBrush( combo.BackColor );
                fore_brush = new SolidBrush( combo.ForeColor );
            }

            e.Graphics.FillRectangle(back_brush, e.Bounds);

            string item_string =  combo.Items[index].ToString();

            e.Graphics.DrawString(item_string, e.Font, fore_brush, e.Bounds, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }
        //}}}
        // --------------------------------------------------------------------
        // --------------------------------------------------------------------
        //}}}
        // VS EVENT HANDLERS {{{
        // panel_scroll_DoubleClick {{{
        private void panel_scroll_DoubleClick(object sender, EventArgs e)
        {
            v_splitter_Toggle(sender);
        }
        //}}}
        // tabs_container_DoubleClick {{{
        private void tabs_container_DoubleClick(object sender, EventArgs e)
        {
            v_splitter_Toggle(sender);
        }
        //}}}
        // panel_panels_DoubleClick {{{
        private void panel_panels_DoubleClick(object sender, EventArgs e)
        {
            v_splitter_Toggle(sender);
        }
        //}}}
        // panel_controls_DoubleClick {{{
        private void panel_controls_DoubleClick(object sender, EventArgs e)
        {
            v_splitter_Toggle(sender);
        }
        //}}}

        // v_splitter_Toggle {{{
        private void v_splitter_Toggle(object panel_to_hide)
        {
            // ONLY WHEN DOCKED .. (i.e. user did not choose manually to show both sides)
            bool docked_L = (v_splitter.SplitterDistance < (                  v_splitter.SplitterIncrement));
            bool docked_R = (v_splitter.SplitterDistance > (v_splitter.Width - v_splitter.SplitterIncrement));

            if(     (panel_to_hide == tabs_view) || (panel_to_hide == tabs_container)) {
                if(docked_R) v_splitter.SplitterDistance = 0;                // make RIGHT fully visible .. when this LEFT one is
                else         v_splitter.SplitterDistance = v_splitter.Width;  // make this  fully visible
            }
            else if(panel_to_hide == panels_container) {
                if(docked_L) v_splitter.SplitterDistance = v_splitter.Width;  // make LEFT  fully visible .. when this right one is
                else         v_splitter.SplitterDistance = 0;                // make this  fully visible
            }
            else /*                  controls_container */ {
                if(docked_R) v_splitter.SplitterDistance = 0;                // make RIGHT fully visible
                else         v_splitter.SplitterDistance = v_splitter.Width;  // make LEFT  fully visible
            }

            // take the color of the hidden sibling
            v_splitter_changeColor();
            v_splitter.Focus();

            // CONTROLS LAYOUT {{{
            if(UI_state == Settings.STATE_LAYOUT)
            {
                docked_L = (v_splitter.SplitterDistance < (v_splitter.SplitterIncrement));
                docked_R = (v_splitter.SplitterDistance > (v_splitter.Width - v_splitter.SplitterIncrement));

                // LAYING OUT USER OR CONTROL TABS {{{
                if((docked_R) && ((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))) {
                    // only tabs_container visible
                    tabsCollection.show_UI_state_targets_only( UI_state );  
                    info_Announce("LAYING OUT USER TABS");
                }
                else {
                    // panels_container also visible
                    tabsCollection.show_UI_state_targets_only_withControls( UI_state );
                    warn_Announce("LAYING OUT APPLICATION TABS");
                }
                //}}}
            }
            //}}}
            else {
                tabsCollection.show_UI_state_targets_only_withControls( UI_state );
            }
        }
        //}}}
        // v_splitter_show {{{
        private void v_splitter_show(Panel panel_to_show)
        {
            // ONLY WHEN DOCKED .. (i.e. user did not choose manually to show both sides)
            bool docked_L = (v_splitter.SplitterDistance < (                  v_splitter.SplitterIncrement));
            bool docked_R = (v_splitter.SplitterDistance > (v_splitter.Width - v_splitter.SplitterIncrement));

            // do nothing if  already visible
            if(!docked_L && !docked_R)
                return;

            v_splitter_ShowParent( panel_to_show );
        }
        //}}}
        // v_splitter_ShowParent {{{
        private void v_splitter_ShowParent(Panel panel_to_show)
        {
            try {
                if     ((panel_to_show == panels_container) || (panel_to_show.Parent == panels_container)) v_splitter.SplitterDistance = 0;
                else if((panel_to_show == tabs_container  ) || (panel_to_show.Parent == tabs_container  )) v_splitter.SplitterDistance = v_splitter.Width;
            }
            catch(Exception) { }

            // take the color of the hidden sibling
            v_splitter_changeColor();
            v_splitter.Focus();
        }
        //}}}

        // v_splitter_DoubleClick {{{
        private void v_splitter_DoubleClick(object sender, EventArgs e)
        {
            bool docked_L = (v_splitter.SplitterDistance < (                  v_splitter.SplitterIncrement));
            bool docked_R = (v_splitter.SplitterDistance > (v_splitter.Width - v_splitter.SplitterIncrement));
            bool user_set = (v_splitter.SplitterDistance == Settings.SplitX);

            // One is fully visible .. show the other one
            if     (docked_R) v_splitter.SplitterDistance = Settings.SplitX;
            else if(docked_L) v_splitter.SplitterDistance = Settings.SplitX;

            // or switch betwee center-split and user-set-split
            else if(user_set) v_splitter.SplitterDistance = v_splitter.Width / 2;
            else              v_splitter.SplitterDistance = Settings.SplitX;     // happens when already at center-split

            // take the color of the hidden sibling
            v_splitter_changeColor();
        }
        //}}}
        // v_splitter_SplitterMoved {{{
        private void v_splitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //if( !OnLoad_done ) return;
        }
        //}}}
        // v_splitter_SplitterMoving {{{
        private void v_splitter_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            int incr    = v_splitter.SplitterIncrement;
            int center  = v_splitter.Width / 2;

            // remember the last user-set-position (...discard meaningless values)
            if(    (e.MouseCursorX >=   (                   incr)) // not docked left
                && (e.MouseCursorX <=   (v_splitter.Width - incr)) // not docked right
                && (Math.Abs(e.SplitX - center         ) >= incr ) // not at center-split
                && (Math.Abs(e.SplitX - Settings.SplitX) >= incr ) // not a transition
              ) {
                Settings.SplitX = e.SplitX;
                v_splitter.Focus();
              }
        }
        //}}}
        // v_splitter_changeColor {{{
        private void v_splitter_changeColor()
        {
            // TAKE THE COLOR OF THE HIDDEN SIBLING
            if(v_splitter.SplitterDistance < v_splitter.SplitterIncrement)
                v_splitter.BackColor = tabs_view.BackColor;

            else if(v_splitter.SplitterDistance > (v_splitter.Width - v_splitter.SplitterIncrement))
                v_splitter.BackColor = panels_container.BackColor;

            // ...use main form color when not docked
            else
                v_splitter.BackColor = Settings.MainFormInstance.BackColor;
        }
        //}}}
        // Minimize_MouseEnter {{{
        private void Minimize_MouseEnter(object sender, EventArgs e)
        {
            Label    label   = sender as Label;
            label.BackColor = Color.FromArgb(80,   0, 255,   0);
        }
        //}}}
        // Close_MouseEnter {{{
        private void Close_MouseEnter(object sender, EventArgs e)
        {
            Label    label   = sender as Label;
            label.BackColor = Color.FromArgb(80, 255,   0,   0);
        }
        //}}}
        // Minimize_MouseLeave {{{
        private void Minimize_MouseLeave(object sender, EventArgs e)
        {
            Label    label   = sender as Label;

            label.BackColor = Color.Transparent;
        }
        //}}}
        // Close_MouseLeave {{{
        private void Close_MouseLeave(object sender, EventArgs e)
        {
            Label    label   = sender as Label;

            label.BackColor = Color.Transparent;
        }
        //}}}
        // }}}

    }
}

/* // {{{

" STANDARD NUMERIC FORMAT STRINGS
:!start explorer "https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx"

*/ // }}}

