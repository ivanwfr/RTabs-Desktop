// using {{{
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Util;
// }}}
namespace Util
{
    public partial class TabTagForm : Form
    {
        // INSTANCE {{{
        private DialogResult  result        = DialogResult.Cancel;
        private NotePane        np          = null;
        private NotePane        np_clone    = null;
        private Point           np_clone_L;
        private Size            np_clone_S;
        private float           np_clone_Z;

        // }}}
        // TabTagForm {{{
        public TabTagForm()
        {
            InitializeComponent();
            Text = Settings.APP_NAME;
            combo_builtins_init();
            combo_sk_init();
        }
        //}}}
        // DialogBox_Focus {{{
        private void DialogBox_Focus(object sender, System.EventArgs e)
        {
            this.txt_input.Focus();
        }
        //}}}
        // ShowDialog {{{
        private static TabTagForm ttf;
        public static DialogResult ShowDialog(NotePane np, string tag_old, out string tag_new)
        {
            // SINGLETON -  {{{
            int x, y, w, h;

            if(ttf == null)
            {
                ttf = new TabTagForm();

                // START POSITION .. parent middle
                x   = Settings.MainFormInstance.Location.X;
                y   = Settings.MainFormInstance.Location.Y
                    + Settings.MainFormInstance.ClientSize.Height / 2
                    - ttf                      .ClientSize.Height / 2
                    ;
                ttf.Location = new Point(x,y);

                // Start Dimension .. fit parent width
                w   = Settings.MainFormInstance.ClientSize.Width ;
                h   = ttf                      .ClientSize.Height;
                ttf.ClientSize = new Size(w,h);

                ToolTip toolTip  = new System.Windows.Forms.ToolTip();
                toolTip.BackColor = Settings.MainFormInstance.BackColor;
                toolTip.ForeColor = Settings.MainFormInstance.ForeColor;

                toolTip.SetToolTip(ttf.btn_ok       ,   "EDIT: apply changes");
                toolTip.SetToolTip(ttf.btn_activate ,   "EDIT: activate / desactivate");
                toolTip.SetToolTip(ttf.btn_cancel   ,   "EDIT: ignore changes");

                toolTip.SetToolTip(ttf.btn_RUN      , "SERVER: RUN EXECUTABLE\n ...as a new process");
                toolTip.SetToolTip(ttf.btn_SHELL    , "SERVER: OPEN FILE\n...with the default related application");
                toolTip.SetToolTip(ttf.btn_SENDKEYS , "SERVER: SEND KEYS TO THE CURRENT WINDOW\n ...as they would come from the KEYBOARD");
                toolTip.SetToolTip(ttf.btn_PROFILE  , "SERVER: LOAD PROFILE");

                toolTip.SetToolTip(ttf.lbl_INFO     , "Send SHIFT-ESCAPE to the TAB PANEL\nto reset RichText attributes\n(i.e. ZoomFactor, ...)");

            }
            //}}}

            // TAB NAME - (StartPosition - Manual) {{{
            ttf.Text                = Settings.APP_NAME;
            ttf.lbl_tab_name.Text   = np.Name;

            // }}}
            // DISPLAY CURRENT TAG COMMAND // {{{
            if(!string.IsNullOrEmpty( tag_old ))
            {
                ttf.txt_current.Text = tag_old;
                ttf.txt_input  .Text = tag_old;

            }
            // }}}
            // CLONE NotePane {{{
            ttf.np                  = np;
            ttf.np_clone            = NotePane.Clone(ttf.np);
            ttf.np_clone.Parent     = ttf;

            x = ttf.btn_ok.Location.X + ttf.btn_ok.ClientSize.Width + 8;
            y = ttf.btn_ok.Location.Y;
            ttf.np_clone_L          = new Point(x,y);
            ttf.np_clone.Location   = ttf.np_clone_L;

            w = ttf.btn_SENDKEYS.Location.X
            -   ttf.btn_ok      .Location.X
            -   ttf.btn_ok.ClientSize.Width
            -   24;
            h = ttf.MinimumSize .Height
            -   ttf.btn_ok      .Location.Y
            -   12;
            ttf.np_clone_S          = new Size(w,h);
            ttf.np_clone.ClientSize = ttf.np_clone_S;

            ttf.np_clone_Z          = ttf.np_clone._tb.ZoomFactor;

            ttf.np_clone.KeyUp     += new System.Windows.Forms.KeyEventHandler( ttf.text_KeyUp );
            ttf.np_clone._tb.KeyUp += new System.Windows.Forms.KeyEventHandler( ttf.text_KeyUp );

            ttf.btn_activate.Text = (np.Type != NotePane.TYPE_SHORTCUT) ? "Activate" : "De-Activate";

            // radio_color {{{
            // leave radio buttons unchecked when working with a collection .. (commit changes forced)
            int sel_count = TabsCollection.Get_Sel_Count_but_this_one( np );
            if(sel_count > 0) {
                ttf.radio_color01.Checked = false;
                ttf.radio_color02.Checked = false;
                ttf.radio_color03.Checked = false;
                ttf.radio_color04.Checked = false;
                ttf.radio_color05.Checked = false;
                ttf.radio_color06.Checked = false;
                ttf.radio_color07.Checked = false;
                ttf.radio_color08.Checked = false;
                ttf.radio_color09.Checked = false;
                ttf.radio_color10.Checked = false;
                ttf.radio_color11.Checked = false;
                ttf.radio_color00.Checked = false;
                set_radio_color_BackColor( Color.Blue );
            }
            else {
                switch( np.color ) {
                    case "01": ttf.radio_color01.Checked = true; break;
                    case "02": ttf.radio_color02.Checked = true; break;
                    case "03": ttf.radio_color03.Checked = true; break;
                    case "04": ttf.radio_color04.Checked = true; break;
                    case "05": ttf.radio_color05.Checked = true; break;
                    case "06": ttf.radio_color06.Checked = true; break;
                    case "07": ttf.radio_color07.Checked = true; break;
                    case "08": ttf.radio_color08.Checked = true; break;
                    case "09": ttf.radio_color09.Checked = true; break;
                    case "10": ttf.radio_color10.Checked = true; break;
                    case "11": ttf.radio_color11.Checked = true; break;
                    default  : ttf.radio_color00.Checked = true; break;
                }
                set_radio_color_BackColor(Settings.MainFormInstance.BackColor);
            }
            //}}}

            // radio_shape {{{
            if(sel_count > 0) {
                ttf.radio_shape_tile  .Checked = false;
                ttf.radio_shape_circle.Checked = false;
                ttf.radio_shape_onedge.Checked = false;
                ttf.radio_shape_padd_r.Checked = false;
                ttf.radio_shape_square.Checked = false;
                ttf.radio_shape_auto  .Checked = false;
                set_radio_shape_BackColor( Color.Blue );
            }
            else {
                switch( np.shape ) {
                    case   "tile": ttf.radio_shape_tile  .Checked = true; break;
                    case "circle": ttf.radio_shape_circle.Checked = true; break;
                    case "onedge": ttf.radio_shape_onedge.Checked = true; break;
                    case "padd_r": ttf.radio_shape_padd_r.Checked = true; break;
                    case "square": ttf.radio_shape_square.Checked = true; break;
                    default      : ttf.radio_shape_auto  .Checked = true; break;
                }
                set_radio_shape_BackColor( Settings.MainFormInstance.BackColor );
            }
            //}}}

            //}}}
            // FORM COLOR {{{
            ttf.BackColor           = Settings.MainFormInstance.BackColor;

            //}}}
            // FORM SIZE // {{{
/*
            w           = ttf.ClientSize.Width; // previous width
            int  wm     = ttf.np_clone.Location.X + ttf.np_clone.ClientSize.Width  + 16;
            if(w<wm) w  = wm;

            h           = ttf.MinimumSize.Height;
            int  hm     = ttf.  btn_ok.Location.Y + ttf.np_clone.ClientSize.Height + 16;
            if(h<hm) h  = hm;

            ttf.ClientSize = new Size(w, h);
*/
            // }}}
            // number of selected tabs to be changed along the current one {{{
            if(sel_count > 0) {
                string s = (sel_count > 1) ? "tabs" : "tab";
                ttf.lbl_sel_count.Text    = "...or also change the "+ sel_count.ToString() +" other selected "+ s;
                ttf.lbl_sel_count.Visible = true;
                ttf.btn_sel_clear.Visible = true;
            }
            else {
                ttf.lbl_sel_count.Visible = false;
                ttf.btn_sel_clear.Visible = false;
            }
            //}}}
            // EDIT .. (wait for input] {{{
            ttf.ShowDialog();

            if(ttf.result == DialogResult.OK)
                tag_new = ttf.txt_input.Text;
            else
                tag_new = tag_old;

        //  if(ttf != null) ttf.Dispose();
        //  if(ttf.components != null) ttf.components.Dispose();
            ttf.np_clone.Dispose();
            ttf.np_clone            = null;
            ttf.np                  = null;

            //}}}

            log("ShowDialog:\n"
            +"DialogResult.OK=["+ DialogResult.OK +"]\n"
            +"     ttf.result ["+ ttf.result      +"]\n"
            +"        tag_old ["+ tag_old         +"]"
            +"        tag_new ["+ tag_new         +"]"
            );
            return ttf.result;
        }
        //}}}
        // set_radio_shape_BackColor {{{
        private static void set_radio_shape_BackColor(Color color) 
        {
            ttf.radio_shape_tile  .BackColor = color;
            ttf.radio_shape_circle.BackColor = color;
            ttf.radio_shape_onedge.BackColor = color;
            ttf.radio_shape_padd_r.BackColor = color;
            ttf.radio_shape_square.BackColor = color;
            ttf.radio_shape_auto  .BackColor = color;
        }
        //}}}
        // set_radio_color_BackColor {{{
        private static void set_radio_color_BackColor(Color color) 
        {
            ttf.radio_color01     .BackColor = color;
            ttf.radio_color02     .BackColor = color;
            ttf.radio_color03     .BackColor = color;
            ttf.radio_color04     .BackColor = color;
            ttf.radio_color05     .BackColor = color;
            ttf.radio_color06     .BackColor = color;
            ttf.radio_color07     .BackColor = color;
            ttf.radio_color08     .BackColor = color;
            ttf.radio_color09     .BackColor = color;
            ttf.radio_color10     .BackColor = color;
            ttf.radio_color11     .BackColor = color;
            ttf.radio_color00     .BackColor = color;
        }
        //}}}
        // Click KeyUp {{{
        private void btn_builtin_Click     (object sender, EventArgs            e) { builtins_insert( ((Button)sender).Text );   }
        private void btn_sk_Click          (object sender, EventArgs            e) { sk_append(((Button)sender). Text); }
        private void btn_cancel_Click      (object sender, EventArgs            e) { result = DialogResult.Cancel; Close(); }
        private void btn_activate_Click    (object sender, EventArgs            e) { result = DialogResult.Yes   ; Close(); }
        private void btn_ok_Click          (object sender, EventArgs            e) { result = DialogResult.OK    ; Close(); }
        private void btn_sel_clear_Click   (object sender, EventArgs            e)
        {
            TabsCollection.Sel_Clear();
            ttf.lbl_sel_count.Visible = false;
            ttf.btn_sel_clear.Visible = false;
            set_radio_color_BackColor( Settings.MainFormInstance.BackColor );
            set_radio_shape_BackColor( Settings.MainFormInstance.BackColor );
        }
        private void text_KeyUp            (object sender, KeyEventArgs         e) {
            if(sender == np_clone._tb) {
                if((e.KeyCode == Keys.Escape) && (!e.Shift)) { result = DialogResult.Cancel; Close(); }
            }
            else {
                if     (e.KeyCode == Keys.Enter ) { result = DialogResult.OK    ; Close(); }
                else if(e.KeyCode == Keys.Escape) { result = DialogResult.Ignore; Close(); }
            }
        }
        // }}}
        // Close {{{
        new protected void Close()
        {
            // OK
            if(result == DialogResult.OK)
            {
                // ...activate shortcut
                np.Type = NotePane.TYPE_SHORTCUT;
                TabsCollection.Clone_selection_Type( np );

            // ...transfer changes made to the clone back to the the stem
            //  if(!np._tb.Rtf       .Equals( np_clone._tb.Rtf        )                                 ) { np._tb.Rtf        = np_clone._tb.Rtf;                                      TabsCollection.Clone_selection_Rtf       ( np ); }
                if(!np._tb.Text      .Equals( np_clone._tb.Text       )                                 ) { np._tb.Text       = np_clone._tb.Text.Replace(Environment.NewLine, @"\n"); TabsCollection.Clone_selection_Text      ( np ); }
                if(!np._tb.ZoomFactor.Equals( np_clone._tb.ZoomFactor )                                 ) { np._tb.ZoomFactor = np_clone._tb.ZoomFactor;                               TabsCollection.Clone_selection_ZoomFactor( np ); }
                if(!np.zoom          .Equals( np_clone.zoom           )                                 ) { np.zoom           = np_clone.zoom;                                         TabsCollection.Clone_selection_zoom      ( np ); }
                if(!np.color         .Equals( np_clone.color          ) || collection_color_has_been_set) { np.color          = np_clone.color;                                        TabsCollection.Clone_selection_color     ( np ); }
                if(!np.shape         .Equals( np_clone.shape          ) || collection_shape_has_been_set) { np.shape          = np_clone.shape;                                        TabsCollection.Clone_selection_shape     ( np ); }

            }
            // ESCAPE
            else if(result == DialogResult.Ignore)
            {
                // ...no change
            }
            // ACTIVATE
            else if(result == DialogResult.Yes)
            {
                // ...toggle shortcut
                if(np.Type == NotePane.TYPE_SHORTCUT)   { np.Type = NotePane.TYPE_RICHTEXT; }
                else                                    { np.Type = NotePane.TYPE_SHORTCUT; }

                // ...propagate to currently selected tabs
                TabsCollection.Clone_selection_Type( np );
            }
            // CANCEL
            else {
                // ...no change
            }
            base.Close();
        }
    // }}}
        // builtins Select insert {{{
        // combo_builtins_init {{{
        private  void combo_builtins_init()
        {
            if(combo_builtins.Items.Count < 1)
            {
                log("combo_builtins_init: Settings.BUILTINS.Length: "+Settings.BUILTINS.Length);
                combo_builtins.Items.Clear();

                for(int i=0; i < Settings.BUILTINS.Length; ++i)
                    combo_builtins.Items.Add( string.Format("{0,2:F}", Settings.BUILTINS[i]) );
            }
        }
        //}}}
        // combo_sk_init {{{
        private const string SENDKEY_SHIFT          = "+ SHIFT";
        private const string SENDKEY_CTRL           = "^ CTRL" ;
        private const string SENDKEY_ALT            = "% ALT"  ;

        private  void combo_sk_init()
        {
            if(combo_sk.Items.Count < 1)
            {
                combo_sk.Items.Clear();

                combo_sk.Items.Add( SENDKEY_SHIFT);
                combo_sk.Items.Add( SENDKEY_CTRL );
                combo_sk.Items.Add( SENDKEY_ALT  );

                combo_sk.Items.Add("{CAPSLOCK}");
                combo_sk.Items.Add("{HELP}");
                combo_sk.Items.Add("{NUMLOCK}");
                combo_sk.Items.Add("{PRTSC}");
                combo_sk.Items.Add("{SCROLLLOCK}");
            }
        }
        //}}}
        // combo_builtins_SelectedIndexChanged {{{

        private void combo_builtins_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = (string)combo_builtins.SelectedItem;

            if     (s == SENDKEY_SHIFT)    s = "+"; // strip helper text
            else if(s == SENDKEY_CTRL )    s = "^"; // strip helper text
            else if(s == SENDKEY_ALT  )    s = "%"; // strip helper text

            builtins_Select( s );
        }
        //}}}
        // builtins_Select {{{
        private void builtins_Select(string builtin)
        {
        //  txt_input.Text = builtin;
            builtins_insert( builtin );
        }
        //}}}
        // builtins_insert {{{
        private void builtins_insert(string builtin)
        {
            if(builtin == "") return;

            // cmd
            string cmdLine = txt_input.Text.Trim();
            string cmd     = cmdLine;

            // args
            string   argLine    = cmdLine;
            string[] args       = { cmdLine };  // individual words
            if(cmdLine.IndexOf(' ') >= 0)
            {
                args    = cmdLine.Split(' ');
                cmd     = args[0];
                argLine = cmdLine.Substring(cmd.Length+1).Trim();
            }

            // unchanged
            if(cmd == builtin) return;

/*
            // replace a first-word core-command
            if(    (cmd == Settings.BUILTINS[0])    // SHELL
                || (cmd == Settings.BUILTINS[1])    // RUN
                || (cmd == Settings.BUILTINS[2])    // SENDKEYS
                || (cmd == Settings.BUILTINS[3])    // IMPORT
              )
                txt_input.Text = builtin +" "+ argLine;
            // prepend other command to current text
            else
                txt_input.Text = builtin +" "+ cmdLine;
*/
            // exchange first word command with this one
            for(int i=0; i < Settings.BUILTINS.Length; ++i)
            {
                if(Settings.BUILTINS[i] == cmd)
                {
                    txt_input.Text  = builtin +" "+ argLine;
                    return;
                }
            }
            // could not find a replaceable command, insert this one
            txt_input.Text          = builtin +" "+ cmdLine;

        }
        //}}}
        //}}}
        // sk_keys {{{
        // Google, #008744,#0057e7,#d62d20,#ffa700,#ffffff
        // combo_sk_SelectedIndexChanged {{{
        private void combo_sk_SelectedIndexChanged(object sender, EventArgs e)
        {
            sk_Select( (string)combo_sk.SelectedItem );
        }
        //}}}
        // sk_Select {{{
        private void sk_Select(string sk_key)
        {
            sk_append( sk_key );
        }
        //}}}
        // sk_insert sk_append {{{
        private void sk_append(string sk_key)
        {
            if(sk_key == "") return;

            if     (sk_key == SENDKEY_SHIFT)    sk_key = "+";   // strip helper text
            else if(sk_key == SENDKEY_CTRL )    sk_key = "^";   // strip helper text
            else if(sk_key == SENDKEY_ALT  )    sk_key = "%";   // strip helper text

            string cmdLine  = txt_input.Text;
        //  if(cmdLine == "") return;

            // last character
            int     idx     = cmdLine.Length - 1;
            string  last_key= (idx>=0) ? cmdLine.Substring( idx ) : "";

            // last symbol
            if     (last_key == "+") idx     = cmdLine.Length-1;    // SENDKEY_SHIFT
            else if(last_key == "^") idx     = cmdLine.Length-1;    // SENDKEY_CTRL
            else if(last_key == "%") idx     = cmdLine.Length-1;    // SENDKEY_ALT
            else if(last_key == "}") {
                idx     = cmdLine.LastIndexOf("{");
                if(idx >= 0)  last_key = cmdLine.Substring(idx);
                else            return; // ...that was not a symbol
            }

            if(last_key == sk_key) cmdLine = cmdLine.Substring(0, idx); // remove sk_key
            else                   cmdLine = cmdLine + sk_key;          // ...add sk_key

            txt_input.Text = cmdLine;
        }
/* // {{{
j0"qy$
0nyyp:s//1/g


    {ENTER} {ESC} {TAB}
    {UP} {RIGHT} {DOWN} {LEFT}
    {BACKSPACE} {BREAK} {DEL} {INSERT}
    {HOME} {END} {PGDN} {PGUP}
    {ADD} {DIVIDE} {MULTIPLY} {SUBTRACT}
    {F1} {F2} {F3} {F4} {F5} {F6} {F7} {F8} {F9} {F10} {F11} {F12} {F13} {F14} {F15} {F16}

    {CAPSLOCK} {HELP} {NUMLOCK} {PRTSC} {SCROLLLOCK}
*/ // }}}
        //}}}
        //}}}
        // log {{{
        private static void log(string msg) {
            Logger.Log(typeof(TabTagForm).Name, msg+"\n");
        }
        //}}}
        // Form [move .. resize] {{{
        // variables {{{
        private const int   MAGNET_SIZE = 30;

        public  bool        locked      = false;

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
        protected override void OnMouseUp  (MouseEventArgs e)// {{{
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

                if(e.X > (Size.Width -GRIP_SIZE)) resize_R = true;
                if(e.X <              GRIP_SIZE ) resize_L = true;
                if(e.Y <              GRIP_SIZE ) resize_T = true;
                if(e.Y > (Size.Height-GRIP_SIZE)) resize_B = true;

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

                    if(!resize_T && !resize_L && !resize_B && !resize_R)
                    {
                        if     (Math.Abs((Screen.PrimaryScreen.Bounds.Width   ) - (L+ Width  )) < MAGNET_SIZE) L = Screen.PrimaryScreen.Bounds.Width  - Width ;
                        else if(Math.Abs((Screen.PrimaryScreen.Bounds.Width /2) -  L          ) < MAGNET_SIZE) L = Screen.PrimaryScreen.Bounds.Width /2;
                        else if(                                                   L            < MAGNET_SIZE) L = 0;

                        if     (Math.Abs((Screen.PrimaryScreen.Bounds.Height  ) - (T + Height)) < MAGNET_SIZE) T  = Screen.PrimaryScreen.Bounds.Height - Height;
                        else if(Math.Abs((Screen.PrimaryScreen.Bounds.Height/2) -  T          ) < MAGNET_SIZE) T  = Screen.PrimaryScreen.Bounds.Height/2;
                        else if(                                                   T            < MAGNET_SIZE) T  = 0;
                    }
                    else if(resize_B || resize_R)
                    {
                         int sw   = Screen.PrimaryScreen.Bounds.Width;
                         int sw875 = (int)(sw * 0.875);
                         int sw750 = (int)(sw * 0.750);
                         int sw625 = (int)(sw * 0.625);
                         int sw500 = (int)(sw * 0.500);
                         int sw375 = (int)(sw * 0.375);
                         int sw250 = (int)(sw * 0.250);
                         int sw125 = (int)(sw * 0.125);

                         int sh   = Screen.PrimaryScreen.Bounds.Height;
                         int sh875 = (int)(sh * 0.875);
                         int sh750 = (int)(sh * 0.750);
                         int sh625 = (int)(sh * 0.625);
                         int sh500 = (int)(sh * 0.500);
                         int sh375 = (int)(sh * 0.375);
                         int sh250 = (int)(sh * 0.250);
                         int sh125 = (int)(sh * 0.125);

                        if     (Math.Abs(sw    -  W) < MAGNET_SIZE) W = sw;
                        else if(Math.Abs(sw875 -  W) < MAGNET_SIZE) W = sw875;
                        else if(Math.Abs(sw750 -  W) < MAGNET_SIZE) W = sw750;
                        else if(Math.Abs(sw625 -  W) < MAGNET_SIZE) W = sw625;
                        else if(Math.Abs(sw500 -  W) < MAGNET_SIZE) W = sw500;
                        else if(Math.Abs(sw375 -  W) < MAGNET_SIZE) W = sw375;
                        else if(Math.Abs(sw250 -  W) < MAGNET_SIZE) W = sw250;
                        else if(Math.Abs(sw125 -  W) < MAGNET_SIZE) W = sw125;

                        if     (Math.Abs(sh    -  H) < MAGNET_SIZE) H = sh;
                        else if(Math.Abs(sh875 -  H) < MAGNET_SIZE) H = sh875;
                        else if(Math.Abs(sh750 -  H) < MAGNET_SIZE) H = sh750;
                        else if(Math.Abs(sh625 -  H) < MAGNET_SIZE) H = sh625;
                        else if(Math.Abs(sh500 -  H) < MAGNET_SIZE) H = sh500;
                        else if(Math.Abs(sh375 -  H) < MAGNET_SIZE) H = sh375;
                        else if(Math.Abs(sh250 -  H) < MAGNET_SIZE) H = sh250;
                        else if(Math.Abs(sh125 -  H) < MAGNET_SIZE) H = sh125;

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
                    if(e.X > (Size.Width -GRIP_SIZE)) resize_R = true;
                    if(e.X <              GRIP_SIZE ) resize_L = true;
                    if(e.Y <              GRIP_SIZE ) resize_T = true;
                    if(e.Y > (Size.Height-GRIP_SIZE)) resize_B = true;
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
        // OnResize {{{
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);

            if(ttf == null) return;
            if(ttf.np_clone == null) return;

            int w = ttf.ClientSize.Width;
            int h = ttf.ClientSize.Height;
            //log("OnResize: wh=["+ w +"x"+ h +"]");

            Size s = np_clone_S;
            if((h - ttf.MinimumSize.Height) > s.Height)
            {
                // remember last unresized ZoomFactor 
                if(ttf.np_clone.Location.Y < ttf.MinimumSize.Height)
                    ttf.np_clone_Z = ttf.np_clone._tb.ZoomFactor;

                int x = 16;
                int y = ttf.MinimumSize.Height;
                ttf.np_clone.Location   = new Point(x,y);

                w = ttf.ClientSize.Width  - 32;
                h = ttf.ClientSize.Height - ttf.MinimumSize.Height - 32;
                ttf.np_clone.ClientSize = new Size(w,h);

                float ratio     = (float)(ttf.np_clone.ClientSize.Height) / (float)(s.Height);
                float zf        = ttf.np_clone_Z * ratio;
                if((zf > 1.0F/64F) && (zf < 64.0F)) {
                    np_clone._tb.ZoomFactor = 1.0F;
                    np_clone._tb.ZoomFactor = zf;
                }
            }
            else {
                if(ttf.np_clone.Location.Y >= ttf.MinimumSize.Height)
                {
                    ttf.np_clone.Location   = ttf.np_clone_L;
                    ttf.np_clone.ClientSize = ttf.np_clone_S;

                    // restore last saved unresized ZoomFactor
                    np_clone._tb.ZoomFactor = 1.0F;
                    np_clone._tb.ZoomFactor = ttf.np_clone_Z;
                }
            }
        }
        //}}}
/*
        // OnResizeEnd {{{
        protected override void OnResizeEnd(System.EventArgs e)
        {
            int w = ttf.ClientSize.Width;
            int h = ttf.ClientSize.Height;
            log("OnResizeEnd: wh=["+ w +"x"+ h +"]");
        }
        //}}}
*/

        private bool collection_color_has_been_set = false;
        private void radio_color_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if      (rb == radio_color01) np_clone.color = "01";
            else if (rb == radio_color02) np_clone.color = "02";
            else if (rb == radio_color03) np_clone.color = "03";
            else if (rb == radio_color04) np_clone.color = "04";
            else if (rb == radio_color05) np_clone.color = "05";
            else if (rb == radio_color06) np_clone.color = "06";
            else if (rb == radio_color07) np_clone.color = "07";
            else if (rb == radio_color08) np_clone.color = "08";
            else if (rb == radio_color09) np_clone.color = "09";
            else if (rb == radio_color10) np_clone.color = "10";
            else if (rb == radio_color11) np_clone.color = "11";
            else if (rb == radio_color00) np_clone.color =   "";
            // no default for unchanged options
            int sel_count = TabsCollection.Get_Sel_Count_but_this_one( np );
            if(sel_count > 0)
            {
                collection_color_has_been_set = true;
                set_radio_color_BackColor( Settings.MainFormInstance.BackColor );
            }
        }

        private bool collection_shape_has_been_set = false;
        private void radio_shape_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if      (rb == radio_shape_circle) np_clone.shape = "circle";
            else if (rb == radio_shape_onedge) np_clone.shape = "onedge";
            else if (rb == radio_shape_padd_r) np_clone.shape = "padd_r";
            else if (rb == radio_shape_square) np_clone.shape = "square";
            else if (rb == radio_shape_tile  ) np_clone.shape =   "tile";
            else if (rb == radio_shape_auto  ) np_clone.shape =       "";
            // no default for unchanged options
            int sel_count = TabsCollection.Get_Sel_Count_but_this_one( np );
            if(sel_count > 0)
            {
                collection_shape_has_been_set = true;
                set_radio_shape_BackColor( Settings.MainFormInstance.BackColor );
            }
        }

        //}}}
        //}}}

    }
}

/*
:!start explorer "https://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys(v=vs.110).aspx"
*/

