// using {{{
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using RTabs;
using Util;
// }}}
namespace Util
{
    public class NoteRichTextBox : System.Windows.Forms.RichTextBox
    //{{{
    {
        private SolidBrush sbb = null;
        private SolidBrush sbf = null;
        public bool        _sel= false; public bool Selected { get { return _sel;    } set { _sel = value; sbb = null; } }

        public override Color BackColor { get { return base.BackColor; } set { base.BackColor = value; sbb = null; } }
        public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; sbf = null; } }

        //public void invalidate() { sbb = null; }

        protected override void Dispose(bool disposing)
        {
            if(disposing) {
                if(sbb != null) sbb.Dispose();
                if(sbf != null) sbf.Dispose();
            }
            base.Dispose( disposing );
        }
    //  public void Dispose() { Dispose(true); }

        protected override void OnEnabledChanged( EventArgs e)// {{{
        {
            base.OnEnabledChanged( e );

            if(!(this.Enabled)) this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            else                this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, false);
/*
            NotePane np = (NotePane)Parent;
            if(!(this.Enabled)) np.toolTip.SetToolTip(np, "DISABLED");
            else                np.update_tooltip();
*/
            //this.Invalidate();
        }
        // }}}
        protected override void OnMouseUp(MouseEventArgs e)// {{{
        {
            base.OnMouseUp( e );

            //NotePane.PendingTimerActionCount    = 0;
            //BackColor = Parent.BackColor;
            //Refresh();
        }
        // }}}
        protected override void OnPaint( System.Windows.Forms.PaintEventArgs e )// {{{
        {
            base.OnPaint( e );

            if(!this.Enabled)
            {
                //((NotePane)Parent).toolTip.SetToolTip(this,null);

                if(sbb == null) sbb = new SolidBrush( _sel ? Color.Black : BackColor );
                e.Graphics.FillRectangle(sbb, ClientRectangle);

                string s = (this.Text.Trim() != "") ? this.Text : ((NotePane)Parent).Name;

                if(sbf == null) sbf = new SolidBrush( ForeColor );
                e.Graphics.DrawString(s, this.Font, sbf, 1.0F, 1.0F );
            }
        }
        // }}}
/*
        protected override void OnMouseClick(MouseEventArgs e)// {{{
        {
            base.OnMouseClick( e );

            if(!this.Enabled) ((NotePane)Parent).tb_OnMouseClick(e);
        }
        // }}}
        protected override void OnMouseDown(MouseEventArgs e)// {{{
        {
            base.OnMouseDown( e );

        //  BackColor = Color.Red;
        //  Refresh();

            //NotePane.PendingTimerActionCount    = 1;

        }
        // }}}
        protected override void OnKeyDown(KeyEventArgs e)// {{{
        {
            base.OnKeyDown(e);
            //((NotePane)Parent)._tb_KeyDown(this, e);

            if(e.KeyCode == Keys.Escape) {
                this.AutoWordSelection = true;          // Enable users to select entire word when double clicked.
                this.Clear();                                   // Clear contents of control.
                this.RightMargin = 2;           // Set the right margin to restrict horizontal text.
                this.SelectedText = "Alpha Bravo Charlie Delta Echo Foxtrot"; // Set the text for the control.
                this.ZoomFactor = 2.0f;             // Zoom by 2 points.
            }

        }
        // }}}
        protected override void OnMouseEnter(EventArgs e)// {{{
        {
            NotePane np = (NotePane)Parent;
            if(np._locked)
                base.OnMouseEnter( e );
            else {
                //Enabled = false;
                np._tb_OnMouseEnter(e);
                Cursor.Current = Cursors.SizeAll;
            }
        }
// }}}
        protected override void OnMouseDown(MouseEventArgs e)// {{{
        {
            NotePane np = (NotePane)Parent;
            if(np._locked)
                base.OnMouseDown( e );
            else {
                //Enabled = false;
                np._tb_OnMouseDown(e);
                Cursor.Current = Cursors.SizeAll;
            }
        }
// }}}
        protected override void OnMouseMove(MouseEventArgs e)// {{{
        {
            NotePane np = (NotePane)Parent;
            if(np._locked)
                base.OnMouseMove( e );
            else {
                np._tb_OnMouseMove(e);
                Cursor.Current = Cursors.IBeam;
            }
                Enabled = true;
        }
// }}}
        protected override void OnMouseUp(MouseEventArgs e)// {{{
        {
            NotePane np = (NotePane)Parent;
            if(np._locked)
                base.OnMouseUp( e );
            else {
                np._tb_OnMouseUp(e);
                Cursor.Current = Cursors.IBeam;
            }
                Enabled = true;
        }
// }}}
        protected override void OnMouseLeave(EventArgs e)// {{{
        {
            NotePane np = (NotePane)Parent;
            if(np._locked)
                base.OnMouseLeave( e );
            else {
                Enabled = true;
                np._tb_OnMouseLeave(e);
                Cursor.Current = Cursors.IBeam;
            }
        }
// }}}
*/
    }
    // }}}

    public partial class NotePane : Panel, IComparable
    {

        // CLASS {{{
        // members {{{


        private static MainForm MainFormInstance;

        public static char      TABVALUE_SEPARATOR      = '|';

        public static string    TYPE_PANEL              = "PANEL";      // LARGE
        public static string    TYPE_DASH               = "DASH";       // LARGE
        public static string    TYPE_CONTROL            = "CONTROL";    // small
        public static string    TYPE_RICHTEXT           = "RICHTEXT";   // small
        public static string    TYPE_SHORTCUT           = "SHORTCUT";   // small

        public static string    DONE_LABEL              = "<";
        public static Font      DONE_FONT               = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


        private static bool     Inititialized           = false;

        private static uint     UI_state                = 0;

        public static Font      TAB_FONT                = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        public static int       CONTROL_NAME_FONT_SIZE  = 29;
        public static int       PANEL_TITLE_FONT_SIZE   = 20;
        public static int       TEXT_FONT_SIZE          = 22;

        // BUILT-INS CONTROLS {{{
        private static string   CONTROL_NAME_HEADER         = "control_";

        // PROFILE-IMPORT-EXPORT
        public static string    CONTROL_NAME_PROFILE        = CONTROL_NAME_HEADER+"profile";
        public static string    CONTROL_NAME_INDEX          = CONTROL_NAME_HEADER+"index";

        public static string    CONTROL_NAME_EXPORT         = CONTROL_NAME_HEADER+"export";
        public static string    CONTROL_NAME_EXPORT_PROFILE = CONTROL_NAME_HEADER+"export_profile";
        public static string    CONTROL_NAME_EXPORT_TO_FILE = CONTROL_NAME_HEADER+"export_to_file";
        public static string    CONTROL_NAME_EXPORT_CLIPBRD = CONTROL_NAME_HEADER+"export_clipbrd";

        public static string    CONTROL_NAME_IMPORT         = CONTROL_NAME_HEADER+"import";
        public static string    CONTROL_NAME_IMPORT_INSERT  = CONTROL_NAME_HEADER+"insert";
        public static string    CONTROL_NAME_IMPORT_OVERLAY = CONTROL_NAME_HEADER+"overlay";

        // STATE-TRANSITION
        public static string    CONTROL_NAME_LOGGING        = CONTROL_NAME_HEADER+"logging";
        public static string    CONTROL_NAME_PALETTES       = CONTROL_NAME_HEADER+"palettes";
        public static string    CONTROL_NAME_SAVE           = CONTROL_NAME_HEADER+"save";
        public static string    CONTROL_NAME_SETTINGS       = CONTROL_NAME_HEADER+"settings";
        public static string    CONTROL_NAME_START          = CONTROL_NAME_HEADER+"START";
        public static string    CONTROL_NAME_STOP           = CONTROL_NAME_HEADER+"STOP";
        public static string    CONTROL_NAME_ADB            = CONTROL_NAME_HEADER+"ADB_5555";

        // EDIT-LAYOUT
        public static string    CONTROL_NAME_LAYOUT         = CONTROL_NAME_HEADER+"layout";
        public static string    CONTROL_NAME_ADD            = CONTROL_NAME_HEADER+"add";
        public static string    CONTROL_NAME_ACTIVATE       = CONTROL_NAME_HEADER+"activate";
        public static string    CONTROL_NAME_UPDATE_PROFILE = CONTROL_NAME_HEADER+"update";
        public static string    CONTROL_NAME_DEL            = CONTROL_NAME_HEADER+"del";

        public static string    CONTROL_NAME_COLOR0         = CONTROL_NAME_HEADER+"color0";
        public static string    CONTROL_NAME_COLOR1         = CONTROL_NAME_HEADER+"color1";
        public static string    CONTROL_NAME_COLOR2         = CONTROL_NAME_HEADER+"color2";
        public static string    CONTROL_NAME_COLOR3         = CONTROL_NAME_HEADER+"color3";
        public static string    CONTROL_NAME_COLOR4         = CONTROL_NAME_HEADER+"color4";
        public static string    CONTROL_NAME_COLOR5         = CONTROL_NAME_HEADER+"color5";

        public static string    CONTROL_NAME_COLOR6         = CONTROL_NAME_HEADER+"color6";
        public static string    CONTROL_NAME_COLOR7         = CONTROL_NAME_HEADER+"color7";
        public static string    CONTROL_NAME_COLOR8         = CONTROL_NAME_HEADER+"color8";
        public static string    CONTROL_NAME_COLOR9         = CONTROL_NAME_HEADER+"color9";
        public static string    CONTROL_NAME_COLOR10        = CONTROL_NAME_HEADER+"color10";
        public static string    CONTROL_NAME_COLOR11        = CONTROL_NAME_HEADER+"color11";

        public static string    CONTROL_NAME_SHAPE0         = CONTROL_NAME_HEADER+"";
        public static string    CONTROL_NAME_SHAPE1         = CONTROL_NAME_HEADER+"shape1";
        public static string    CONTROL_NAME_SHAPE2         = CONTROL_NAME_HEADER+"shape2";
        public static string    CONTROL_NAME_SHAPE3         = CONTROL_NAME_HEADER+"shape3";
        public static string    CONTROL_NAME_SHAPE4         = CONTROL_NAME_HEADER+"shape4";
        public static string    CONTROL_NAME_SHAPE5         = CONTROL_NAME_HEADER+"shape5";

        // COMM
        public static string    CONTROL_LABEL_START         = "START";
        public static string    CONTROL_LABEL_STOP          = "STOP";
        public static string    CONTROL_LABEL_ADB           = "Check ADB-5555 status";

        // SETTINGS
        public static string    CONTROL_LABEL_SETTINGS      = "COMM Settings";
        public static string    CONTROL_LABEL_SAVE          = "Save";

        // MISC .. TODO categorize
        public static string    CONTROL_NAME_CLEAR          = CONTROL_NAME_HEADER+"clear";
        public static string    CONTROL_NAME_EDIT           = CONTROL_NAME_HEADER+"edit";
        public static string    CONTROL_NAME_EXIT           = CONTROL_NAME_HEADER+"exit";
        public static string    CONTROL_NAME_FIREWALL       = CONTROL_NAME_HEADER+"firewall";
        public static string    CONTROL_NAME_HELP           = CONTROL_NAME_HEADER+"help";
        public static string    CONTROL_LABEL_CLEAR         = "Clear";
        public static string    CONTROL_LABEL_EXIT          = "Exit";
        public static string    CONTROL_LABEL_FIREWALL      = "Firewall";
        public static string    CONTROL_LABEL_HELP          = "Help";
        public static string    CONTROL_LABEL_PALETTES      = "Palettes";

        // ...those may fire OnMouseClick callback even when not _locked
        public static string    CONTROL_LABEL_LAYOUT        = "Layout";
        public static string    CONTROL_LABEL_ADD           = "Add";
        public static string    CONTROL_LABEL_ACTIVATE      = "Activate\n(toggle)";
        public static string    CONTROL_LABEL_UPDATE_PROFILE= "UPDATE";
        public static string    CONTROL_LABEL_DEL           = "Delete\n(Mouse-Right)";

        public static string    CONTROL_LABEL_COLOR0        = "Color 0";
        public static string    CONTROL_LABEL_COLOR1        = "Color 1";
        public static string    CONTROL_LABEL_COLOR2        = "Color 2";
        public static string    CONTROL_LABEL_COLOR3        = "Color 3";
        public static string    CONTROL_LABEL_COLOR4        = "Color 4";
        public static string    CONTROL_LABEL_COLOR5        = "Color 5";

        public static string    CONTROL_LABEL_COLOR6        = "Color 6";
        public static string    CONTROL_LABEL_COLOR7        = "Color 7";
        public static string    CONTROL_LABEL_COLOR8        = "Color 8";
        public static string    CONTROL_LABEL_COLOR9        = "Color 9";
        public static string    CONTROL_LABEL_COLOR10       = "Color 10";
        public static string    CONTROL_LABEL_COLOR11       = "Color 11";

        public static string    CONTROL_LABEL_SHAPE_AUTO    =       "";
        public static string    CONTROL_LABEL_SHAPE_CIRCLE  = "circle";
        public static string    CONTROL_LABEL_SHAPE_ONEDGE  = "onedge";
        public static string    CONTROL_LABEL_SHAPE_PADD_R  = "padd_r";
        public static string    CONTROL_LABEL_SHAPE_SQUARE  = "square";
        public static string    CONTROL_LABEL_SHAPE_TILE    =   "tile";

    //  public static string    CONTROL_LABEL_EDIT          = @"Edit";
    //  public static string    CONTROL_LABEL_EDIT_TYPE     = @"Edit\line > \ul TYPE\ul0\line ..KEYS";
    //  public static string    CONTROL_LABEL_EDIT_KEYS     = @"Edit\line > \ul KEYS\ul0\line ..done";
        public static string    CONTROL_LABEL_EDIT          = @"EDIT";
    //  public static string    CONTROL_LABEL_EDIT_TYPE     = @"ON / OFF";
    //  public static string    CONTROL_LABEL_EDIT_KEYS     = @"PC COMMAND";

        // PROFILE-IMPORT-EXPORT

        //blic static string    CONTROL_LABEL_PROFILE       = @"\caps\ul\impr\scaps\shad PROFILE";
        public static string    CONTROL_LABEL_PROFILE       = @"\ul\b PROFILE";
        public static string    CONTROL_LABEL_INDEX         = @"\ul\b INDEX"  ;

        public static string    CONTROL_LABEL_EXPORT        = @"EXPORT";
        public static string    CONTROL_LABEL_EXPORT_PROFILE= @"UPDATE ";
        public static string    CONTROL_LABEL_EXPORT_TO_FILE= @"EXPORT TO FILE";
        public static string    CONTROL_LABEL_EXPORT_CLIPBRD= @"EXPORT TO CLIPBOARD";

        public static string    CONTROL_LABEL_IMPORT        = @"IMPORT";
        public static string    CONTROL_LABEL_IMPORT_INSERT = @"INSERT";
        public static string    CONTROL_LABEL_IMPORT_OVERLAY= @"OVERLAY";

        // MISC
        public static string    CONTROL_LABEL_LOGGING       = "Logging is ON";
        public static string    CONTROL_LABEL_LOGGING_OFF   = "Logging is OFF";
/*
:!start explorer "http://www.biblioscape.com/rtf15_spec.htm"
\vertalc    Text is centered vertically.
\ql Left-aligned (the default).
\qr Right-aligned.
\qj Justified.
\qc Centered.

*/
        //}}}
        // BUILT-IN PANELS {{{
        public static string    PANEL_NAME_HEADER        = "panel_";
        public static string    PANEL_NAME_AUTOSTART     = PANEL_NAME_HEADER+"AUTOSTART";
        public static string    PANEL_NAME_COMM          = PANEL_NAME_HEADER+"COMM_DASH";
        public static string    PANEL_NAME_LOG           = PANEL_NAME_HEADER+"LOG";
        public static string    PANEL_NAME_NETSH         = PANEL_NAME_HEADER+"NETSH";
        public static string    PANEL_NAME_PALETTES      = PANEL_NAME_HEADER+"PALETTES";
        public static string    PANEL_NAME_USR           = PANEL_NAME_HEADER+"usr";

        public static string    PANEL_NAME_PROFILE       = PANEL_NAME_HEADER+"profile";
        public static string    PANEL_NAME_XPORT         = PANEL_NAME_HEADER+"IMPORT-EXPORT";

        //}}}

        // Constructor directive i.e. [NOT A BUTTON WAITING FOR A]...[Label property]
        public static string    TXT_PLACEHOLDER = "...";

        private static string   CONTROL_LABEL_PREFIX      =     @"\fs"+ CONTROL_NAME_FONT_SIZE +@"\qc\bullet ";
        private static string  SHORTCUT_LABEL_PREFIX      =     @"\fs"+ CONTROL_NAME_FONT_SIZE           +@" ";
        private static string     PANEL_TITLE_PREFIX      =     @"\fs"+  PANEL_TITLE_FONT_SIZE           +@" ";
        private static string            TEXT_PREFIX      = @"\par\fs"+         TEXT_FONT_SIZE           +@" ";
        //blic static string    PANEL_PROFILE_PREFIX      =     @"\fs"+         TEXT_FONT_SIZE           +@"\ul\impr\shad ";
        public static string    PANEL_PROFILE_PREFIX      =     @"\fs"+         TEXT_FONT_SIZE           +@"\ul\b ";

        //}}}
        //public  static int      PendingTimerActionCount;
        //public  static string   PENDING_TIMER_ACTION_LAYOUT = "LAYOUT";
        public static void Timer_Tick(object sender, EventArgs e)// {{{
        {
/*
            if(UI_state != 0) return;

            if(PendingTimerActionCount > 0)
                ++PendingTimerActionCount;

            if(PendingTimerActionCount >= 2)
            {
                PendingTimerActionCount = 0;
                MainFormInstance.control_layout_Click();
            }
*/
        }
        //}}}
        static  public void Initialize(Object o)// {{{
        {
            MainFormInstance = (MainForm)o;

            LoadDesignerColorSettings();

            Initialize_ColorPaletteDict( MainFormInstance );
            Select_ActiveColorPalette( Settings.PALETTE );

            Initialize_Images( MainFormInstance );


            Inititialized = true;
        }
        // }}}
        // LoadDesignerColorSettings {{{
        static  private void LoadDesignerColorSettings()
        {
            string[] valueNames = Settings.GetValueNames();
            string     p_prefix = "PALETTE.";
        //  int         p_count = 0;
            foreach(string valueName in valueNames)
            {
                if(!valueName.StartsWith( p_prefix ))
                    continue;

            //  log(String.Format("{0,3} {1}", ++p_count, valueName));

                string line   = Settings.LoadSetting(valueName, "");
                if(line != "")
                    LoadColorPaletteLine( line );
            }
            Store_LastSavedColorPaletteDict();
            //log("LoadDesignerColorSettings: ColorPaletteDict.Count=["+ ColorPaletteDict.Count +"]");
        }
        // }}}
        static  public void SaveSettings() // {{{
        {
            if( !Inititialized ) return;

            //log("SaveSettings: ColorPaletteDict.Count=["+ ColorPaletteDict.Count +"]");
            int num = 1;
            foreach(var item in ColorPaletteDict)
            {
                string          name = (string)item.Key;
            //  ColorPalette    cp   = (ColorPalette)ColorPaletteDict[name];
                string          key  = "PALETTE."+num;
                Settings.SaveSetting(key, GetColorPaletteLine( name ));
                num += 1;
            }
            Store_LastSavedColorPaletteDict();

            // CLEANUP HIGHER OBSOLETE ENTRIES
            while(num < Settings.PALETTES_MAX)
            {
                string key  = "PALETTE."+num++;
                Settings.DeleteSetting( key );
            }
        }
        //}}}
        public  static void Set_UI_state(uint ui_state)// {{{
        {
            UI_state = ui_state;
        }
        // }}}
        private static void Store_LastSavedColorPaletteDict() // {{{
        {
            LastSavedColorPaletteDict   = new Dictionary<string, Object>();
            foreach(var item in ColorPaletteDict)
            {
                string          name = (string)item.Key;
                ColorPalette    cp   = (ColorPalette)ColorPaletteDict[name];
                LastSavedColorPaletteDict.Add(name, cp);
            }
            //log("Store_LastSavedColorPaletteDict: "+LastSavedColorPaletteDict.Count+" saved palettes");
        }
        //}}}
        //}}}
        // IMAGES {{{
        static Image ShadowDownRight;
        static Image ShadowDownLeft;
        static Image ShadowDown;
        static Image ShadowRight;
        static Image ShadowTopRight;

        static public void Initialize_Images(Object o)// {{{
        {
            try {
                // IMAGES - [EmbeddedResource from .csproj]
                Type type = o.GetType();
                ShadowDownRight = new Bitmap(type, "Util.Images.tshadowdownright.png");
                ShadowDownLeft  = new Bitmap(type, "Util.Images.tshadowdownleft.png");
                ShadowDown      = new Bitmap(type, "Util.Images.tshadowdown.png");
                ShadowRight     = new Bitmap(type, "Util.Images.tshadowright.png");
                ShadowTopRight  = new Bitmap(type, "Util.Images.tshadowtopright.png");

            }
            catch(Exception ex) {// {{{
                MessageBox.Show("Initialize:\n"
                    + Settings.ExToString(ex)
                    , "NotePane"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information
                    );
            }
            // }}}

        }
        // }}}
        // }}}
        // PALETTES {{{
        private static Dictionary<string, Object>   ColorPaletteDict         = new Dictionary<string, Object>();
        private static Dictionary<string, Object>   LastSavedColorPaletteDict =  null;
        public  static ColorPalette                 ActiveColorPalette;
        private static int                          ActiveColorCrntNum;
        static public  void Initialize_ColorPaletteDict(Object o)// {{{
        {
            //log("Initialize_ColorPaletteDict (BEFORE): ColorPaletteDict.Count=["+ ColorPaletteDict.Count +"]");

            if(ColorPaletteDict.Count == 0)
                ColorPalette.LoadBuiltIns( ColorPaletteDict );

            //log("Initialize_ColorPaletteDict (AFTER): ColorPaletteDict.Count=["+ ColorPaletteDict.Count +"]");
        }
        // }}}

        // SELECT
//        static public  void SelectNextPalette() // {{{
//        {
//
//            if(ActiveColorPalette != null) {
//                bool found_current = false;
//                foreach(var item in ColorPaletteDict) {
//                    string name = (string)item.Key;
//                    if(                                        found_current) { Select_ActiveColorPalette( name ); break; }
//                    else if(name == ActiveColorPalette.name) { found_current= true; }
//                }
//            }
//            else {
//                Select_ActiveColorPalette("");
//            }
//            //log("SelectNextPalette: ActiveColorPalette:\n"+ActiveColorPalette);
//        }
//        // }}}
        static public  void Select_ActiveColorPalette(string name)// {{{
        {
            //log("Select_ActiveColorPalette(name=["+ name +"])");

            // look for a palette with that name
            if(name != "")
            {
                if( ColorPaletteDict.ContainsKey( name ) )
                {
                    ActiveColorPalette = (ColorPalette)ColorPaletteDict[ name ];
                    ActiveColorCrntNum = 0;
                }
                else {
                    //log("*** found no palette named ["+ name +"] ***");
                }
            }

            // ...pick one anyway
            if(ActiveColorPalette == null)
            {
                foreach(var item in ColorPaletteDict) {
                    ActiveColorPalette = (ColorPalette)ColorPaletteDict[ item.Key ];
                    break; // pick the first
                }
            }

            //Settings.PALETTE = ActiveColorPalette.name;

            //log("Select_ActiveColorPalette: ActiveColorPalette:\n"+ActiveColorPalette);
        }
        // }}}

        // LIST
        static public string[] GetColorPaletteNames() //{{{
        {
            //log("GetColorPaletteNames ColorPaletteDict.Count=["+ ColorPaletteDict.Count +"]");

            string[] names = new string[ ColorPaletteDict.Count ];

            int count = 0;
            foreach(var item in ColorPaletteDict)
                names[count++] = item.Key;

            //log("GetColorPaletteNames names.Length=["+ names.Length +"]");
            return names;
        }
        //}}}

        // FROM-TO TEXT (name, hex (,hex ...)
/* //{{{

http://www.color-hex.com/color-palettes/popular.php
Caucasian   #ffe0bd #ffcd94 #eac086 #ffad60 #ffe39f
Google      #008744 #0057e7 #d62d20 #ffa700 #ffffff
Facebook    #3b5998 #8b9dc3 #dfe3ee #f7f7f7 #ffffff
Metro-UI    #d11141 #00b159 #00aedb #f37735 #ffc425
Metro-Style #00aedb #a200ff #f47835 #d41243 #8ec127
Cappuccino  #4b3832 #854442 #fff4e6 #3c2f2f #be9b7b
Lollipop    #009688 #35a79c #54b2a9 #65c3ba #83d0c9
Twitter     #326ada #d4d8d4 #433e90 #a19c9c #d2d2d2


*/ //}}}
        static public  int    GetColorPaletteCount() //{{{
        {
            return ColorPaletteDict.Count;
        }
        //}}}
        static public  string GetColorPaletteLines() //{{{
        {
            string lines = "";
            foreach(var item in ColorPaletteDict)
            {
                string       name = (string)item.Key;
            //  ColorPalette cp   = (ColorPalette)ColorPaletteDict[ name ];
                lines += GetColorPaletteLine( name ) + "\n";
            }
            return lines;
        }
        //}}}
        static public  string GetColorPaletteLine(string name) //{{{
        {
            string hex_str = "";

            ColorPalette cp = (ColorPalette)ColorPaletteDict[ name ];

            for(int c=0; c < cp.hex_array.Length; ++c)
            {
                if(hex_str != "") hex_str += ",";
                hex_str                   += cp.hex_array[c];
            }

            return  name +", "+ hex_str;
        }
        //}}}
        static public  void   LoadColorPaletteLines(NotePane np)// {{{
        {
            // empty current palettes collection
            ColorPaletteDict = new Dictionary<string, Object>();

            // parse panel content

            string line_fragments = "";

            string[] lines  = np.TextBox.Text.Split('\n');

            for(int l=0; l < lines.Length; ++l)
            {
                string line = lines[l].Trim();
                if(line.Length > 0) {
                    if( LoadColorPaletteLine(line) )    line_fragments = "";
                    else                                line_fragments += " "+line;
                }
            }

            // TRY TO MAKE SOMETHING WITH FRAGMENTS FOLLOWING LAST PARSED LINE
            if(line_fragments.Length > 0)
            {
                line_fragments = line_fragments.Replace("[\n", " ");
                line_fragments = line_fragments.Replace("[\t", " ");
                line_fragments = line_fragments.Trim();
                //  log("line_fragments=["+ line_fragments +"]");
                LoadColorPaletteLine( line_fragments );
            }

            if(ColorPaletteDict.Count > 2)
            {
                //log(ColorPaletteDict.Count +" Palettes defined");
            }
            else {
                ColorPaletteDict = LastSavedColorPaletteDict;
                //log("Last saved "+ ColorPaletteDict.Count +" palettes restored");
            }

            // try to re-select the current palette from the new set
            Select_ActiveColorPalette( Settings.PALETTE );

            // update UI
            np.logger.callback(np, "LoadColorPaletteLines");
        }
        // }}}
        static public bool   LoadColorPaletteLine(string line) // {{{
        {
            string[] words  = line.Split(new char[] {',', ' ', '\t'});

            string   name   = words[0];

            string hex_str = "";
            for(int w=1; w < words.Length; ++w) {
                if(hex_str != "")  hex_str  += ",";
                hex_str                     += words[w];
            }

            ColorPalette cp = new ColorPalette(name, hex_str);
            if(cp.hex_array.Length > 2) {
                if(! ColorPaletteDict.ContainsKey( name ) )
                    ColorPaletteDict.Add(name, cp);
                return true;
            }
            else {
                return false;
            }
        }
        //}}}

        // COLORS
        public static Regex Color_tag_regex = new Regex(@"#[\dA-Fa-f]{6,}", RegexOptions.IgnoreCase);

        public void pickNextPaletteColor() // {{{
        {
        //  log("pickNextPaletteColor: ActiveColorPalette=["+ActiveColorPalette+"]");

            int max = ActiveColorPalette.bc_array.Length;

            if     (Name.Equals(CONTROL_NAME_COLOR1)) { this.BackColor = ActiveColorPalette.bc_array[0      ]; this.ForeColor      = ActiveColorPalette.fc_array[0      ]; return; }
            else if(Name.Equals(CONTROL_NAME_COLOR2)) { this.BackColor = ActiveColorPalette.bc_array[1      ]; this.ForeColor      = ActiveColorPalette.fc_array[1      ]; return; }
            else if(Name.Equals(CONTROL_NAME_COLOR3)) { this.BackColor = ActiveColorPalette.bc_array[2 % max]; this.ForeColor      = ActiveColorPalette.fc_array[2 % max]; return; }
            else if(Name.Equals(CONTROL_NAME_COLOR4)) { this.BackColor = ActiveColorPalette.bc_array[3 % max]; this.ForeColor      = ActiveColorPalette.fc_array[3 % max]; return; }
            else if(Name.Equals(CONTROL_NAME_COLOR5)) { this.BackColor = ActiveColorPalette.bc_array[4 % max]; this.ForeColor      = ActiveColorPalette.fc_array[4 % max]; return; }

            int nextColorNum        = (ActiveColorCrntNum+1) % ActiveColorPalette.bc_array.Length;

            // USE TAG COLORS {{{
            string hex_color = "";
            if(this.Tag != null) {
                string tag   = (string)this.Tag;
                Match  match = Color_tag_regex.Match( tag );
                if( match.Success ) {
                    Group   g = match.Groups[0];
                    hex_color = g.Captures[0].ToString();
                }
            }
/* // SAMPLE .. COLORS {{{
j0"*y}
#000000
#003300
#006600
#009900
#00CC00
#00FF00
#000033

*/ // }}}

            if(hex_color != "")
            {
                // PARSE ITEM'S BACKGROUND COLOR
                this.BackColor = ColorPalette.GetHexColor( hex_color );

                // PICK FOREGROUND COLOR
                if(ColorPalette.GetBrightness( this.BackColor ) < 127)
                    this.ForeColor = Color.White;
                else
                    this.ForeColor = Color.Black;

                // SHOW RESULT IN TOOLTIP
                this.TT = "hex_color=["+ hex_color +"]=["+ this.BackColor.ToString() +"]";
            }
            //}}}
            // CURRENT PALETTE COLORS {{{
            else {
                this.BackColor      = ActiveColorPalette.bc_array[ActiveColorCrntNum];
                this.ForeColor      = ActiveColorPalette.fc_array[ActiveColorCrntNum];
            }
            //}}}

            if(this.ForeColor == this.BackColor) this.ForeColor = Color.Black;

            this.toolTip.BackColor  = this.BackColor;

            this.BorderColor        = this.BackColor;
            this.BorderStyle        = System.Windows.Forms.BorderStyle.None;

//          _tb.BorderColor        = _tb.BackColor;
            _tb.BorderStyle         = System.Windows.Forms.BorderStyle.None;

/* {{{
            if(this._type == TYPE_SHORTCUT)
                this.BorderColor    = ActiveColorPalette.bc_array[nextColorNum];
            else
                this.BorderColor    = this.BackColor;
*/ // }}}
//try { // ! Control does not support transparent background colors. //{{{
//} catch(Exception) {} //}}}

            // PADDING MAKES A BORDER
            try {
                if(_type == TYPE_SHORTCUT) {
                    _tb.BackColor = ColorPalette.GetColorDarker (this.BackColor, 0.4);
                    _tb.ForeColor = ColorPalette.GetColorLighter(this.ForeColor, 0.5);
                }
                else {
                    this._tb.BackColor  = this.BackColor;
                    this._tb.ForeColor  = this.ForeColor;
                }
            } catch(Exception) {}

            // COLOR USED
            ActiveColorCrntNum      = nextColorNum;
        }
        // }}}
        public void reserve_color() // {{{
        {
//log("reserve_color: _color=["+_color+"]");

            // RESPECT TAG COLOR {{{
            string hex_color = "";
            if(this.Tag != null) {
                string tag   = (string)this.Tag;
                Match  match = Color_tag_regex.Match( tag );
                if( match.Success ) {
                    Group   g = match.Groups[0];
                    hex_color = g.Captures[0].ToString();
                }
            }

            if(hex_color != "")
                return;

            //}}}

            if(this._color == "")
            {
                pickNextPaletteColor();
                return;
            }

            int color_index = -1;
            try {
                color_index = int.Parse( this._color ) - 1;
            } catch(Exception) {
                //log("reserve_color: _color=["+_color+"]: "+ex.Message);
                //MainFormInstance.exit("NotePane.reserve_color");
            }
            if(color_index < 0) {
                this.BackColor          = Color.Black;
                this.ForeColor          = Color.White;
            }
            else {
                this.BackColor          = ActiveColorPalette.bc_array[color_index % ActiveColorPalette.bc_array.Length];
                this.ForeColor          = (ColorPalette.GetBrightness( this.BackColor ) < 127) ? Color.White : Color.Black;
            }
            this.toolTip.BackColor  = this.BackColor;

            this.Padding = (color_index != 0)  ? new Padding(2,2,2,2) : new Padding(0,0,0,0);

        //  _tb.BackColor           = ColorPalette.GetColorDarker (this.BackColor, 0.2);
        //  _tb.ForeColor           = ColorPalette.GetColorLighter(this.ForeColor, 0.2);
            _tb.BackColor           = this.BackColor;
            _tb.ForeColor           = this.ForeColor;
            //_tb.invalidate();

//log("reserve_color: _color=["+ _color +"] Padding=["+ this.Padding +"]");
        }
        // }}}
        public static Color GetDarkestBackColor() // {{{
        {
            return ActiveColorPalette.GetDarkestBackColor();
        }
        // }}}
        public static Color GetLightestBackColor() // {{{
        {
            return ActiveColorPalette.GetLightestBackColor();
        }
        // }}}

        //}}}
        // PROPERTIES {{{

        // CHANGE - NO INVALIDATE - NO SYNC
        public Color       _borderColor=  Color.White; public Color           BorderColor   { get { return _borderColor; } set { _borderColor   = value; } }
        public string      _textPrefix =  TEXT_PREFIX; public string          TextPrefix    { get { return  _textPrefix; } set {  _textPrefix   = value; } }
        public string      _labelPrefix= "";           public string          LabelPrefix   { get { return _labelPrefix; } set { _labelPrefix   = value; } }

        // CHANGE + INVALIDATE
        public NoteRichTextBox _tb = new NoteRichTextBox(); public NoteRichTextBox TextBox  { get { return _tb;          } set { _tb       = value; invalidate("TextBox");  } }
    //  public   RichTextBox   _tb = new   RichTextBox();   public     RichTextBox TextBox  { get { return _tb;      } set { _tb       = value; invalidate("TextBox");  } }

        public bool        _readOnly   = false;        public bool            ReadOnly      { get { return _readOnly;} set { _readOnly = value; invalidate("ReadOnly"); } }
        public string      _label      =    "";        public string          Label         { get { return _label;   } set { _label    = value; invalidate("Label");    } }
/*      public string      _text       =    ""; */     public override string Text          { get { return _tb.Text; } set { _tb.Text  = value; invalidate("Text");     } }
        public string      _type       =    "";        public string          Type          { get { return _type;    } set { _type     = value; invalidate("Type");     } }
        public string      _tt         =    "";        public string          TT            { get { return _tt;      } set { _tt       = value; update_tooltip();       } }

        // CHANGE + SYNC
        private bool       _locked     = true;         public bool            Locked        { get { return _locked;  } set { _locked   = value; validate();             } }
        private string     _xy_wh      =   "";         public string          xy_wh         { get { return _xy_wh;   } set { _xy_wh    = value; invalidate("xy_wh");    } }
        private string     _zoom       =   "";         public string          zoom          { get { return _zoom;    } set { _zoom     = value; invalidate("zoom" );    } }
        private string     _color      =   "";         public string          color         { get { return _color;   } set { _color    = value; invalidate("color");    } }
        public string      _shape      =   "";         public string          shape         { get { return _shape;   } set { _shape    = value; invalidate("shape");    } }

        // registry properties parsing and caching
        private string     _tab_line       = "";//     private string         tab_line      { get { if(_tab_line=="") parse_tab_line(); return _tab_line;       } /*set{}*/}
        private string     _tab_line_type  = "";       private string         tab_line_type { get { if(_tab_line=="") parse_tab_line(); return _tab_line_type ; } /*set{}*/}
        private string     _tab_line_tag   = "";       private string         tab_line_tag  { get { if(_tab_line=="") parse_tab_line(); return _tab_line_tag  ; } /*set{}*/}
        private string     _tab_line_zoom  = "";       private string         tab_line_zoom { get { if(_tab_line=="") parse_tab_line(); return _tab_line_zoom ; } /*set{}*/}
        private string     _tab_line_xy_wh = "";       private string         tab_line_xy_wh{ get { if(_tab_line=="") parse_tab_line(); return _tab_line_xy_wh; } /*set{}*/}
        private string     _tab_line_text  = "";       private string         tab_line_text { get { if(_tab_line=="") parse_tab_line(); return _tab_line_text ; } /*set{}*/}
        private string     _tab_line_color = "";       private string         tab_line_color{ get { if(_tab_line=="") parse_tab_line(); return _tab_line_color; } /*set{}*/}
        private string     _tab_line_shape = "";       private string         tab_line_shape{ get { if(_tab_line=="") parse_tab_line(); return _tab_line_shape; } /*set{}*/}
        private string     _tab_line_tt    = "";       private string         tab_line_tt   { get { if(_tab_line=="") parse_tab_line(); return _tab_line_tt   ; } /*set{}*/}

        public bool is_empty() //{{{
        {
            bool  diag = true;
            try { diag = ((_tb.Text.Trim().Length == 0) && (_tb.Rtf.IndexOf(@"\pict") < 0)); } catch(Exception) {}
            return diag;
        }
        //}}}

        public override string ToString() // {{{
        {
            string text = (Text.Length  < 32  ) ? Text : "Length="+Text.Length.ToString();
            string  tag = (Tag         != null) ? Tag.ToString() : "";

            return typeof(NotePane).Name                    +":\n"
                +".......Name=["+ Name                      +"]\n"
                +".......Type=["+ Type                      +"]\n"
                +"........Tag=["+ tag                       +"]\n"
                +"..... _zoom=["+ _zoom                     +"]\n"
                +"....._xy_wh=["+ _xy_wh                    +"]\n"
                +".......text=["+ text.Replace("\n",@"\n")  +"]\n"
                +".ZoomFactor=["+ _tb.ZoomFactor            +"]\n"
                +"......Label=["+ Label                     +"]\n"
                +".....Locked=["+ Locked                    +"]\n"
                ;
        }
        // }}}

        //}}}
        // INSTANCE {{{
    //  public  MaskPanel       _mask           = null;
        private bool            is_validating   = false;
        private LoggerInterface logger;
        public ToolTip          toolTip         = null;
        protected override void Dispose(bool disposing)
        {
            if(disposing) {
                if(_tb != null) _tb.Dispose();
            }
            base.Dispose( disposing );
        }
    //  public void Dispose() { Dispose(true); }

        private static System.Windows.Forms.ToolTip Np_toolTip = new System.Windows.Forms.ToolTip();

        // NotePane {{{
        public NotePane(string type, string name, string text, string color)
        {
            initialize(type, name, text, color);
        }
        public void initialize(string type, string name, string text, string color)
        {
            string stage        = "";
            try {
                if(text == null) text = "";
                stage = "type name"; //{{{
                if(type == "") _type    = TYPE_PANEL;
                else           _type    = type;
                this.Name   = name;

                //}}}
                stage = "text title or label"; // panel texttitle {{{
                if((type != TYPE_PANEL) && (type != TYPE_DASH))
                {
                    if(text != "")  _tb.Text    = text;
                //  else            this._label = name;
                }
                else if(              name.StartsWith(PANEL_NAME_HEADER)) {
                    string title    = name.Substring (PANEL_NAME_HEADER.Length);
                    _textPrefix     = PANEL_TITLE_PREFIX + title + TEXT_PREFIX;
                }
                // }}}
                stage = "RichTextBox"; //{{{
                insert_tb();
                //}}}
                //stage = "insert_mask"; //{{{
                //insert_mask();

                //}}}
                stage = "Cursor"; //{{{

                if((_type == TYPE_CONTROL ) || (_type == TYPE_SHORTCUT))
                    Cursor.Current  = Cursors.Arrow;
                else
                    Cursor.Current  = Cursors.IBeam;

                //}}}
                stage = "Padding"; //{{{
                    this.Padding    = new Padding(0,0,0,0);

                //}}}
                stage = "Rtf"; //{{{
                if     (_type == TYPE_CONTROL ) this._labelPrefix = CONTROL_LABEL_PREFIX;
                else if(_type == TYPE_SHORTCUT) this._labelPrefix = SHORTCUT_LABEL_PREFIX;
                else                            this._labelPrefix = TEXT_PREFIX;
                //}}}
                stage = "ToolTip"; //{{{
                toolTip = Np_toolTip; //new System.Windows.Forms.ToolTip();
            //  toolTip.AutoPopDelay = 5000;
            //  toolTip.InitialDelay = 1000;
            //  toolTip.ReshowDelay  = 500;
            //  toolTip.ShowAlways   = true; // Force the ToolTip text to be displayed whether or not the form is active.
            //  toolTip.SetToolTip(this,     name);
            //  toolTip.SetToolTip(this._tb, name);
            //  toolTip.BackColor = this.BackColor;

                //}}}
                stage = "Colors"; //{{{
                _color = color;
                if(_color == "")    pickNextPaletteColor();
                else                reserve_color();
                 //}}}
                stage = "validate"; //{{{
                //_tb.Text = text;
                //_tb.Rtf  = "";

                if(text != TXT_PLACEHOLDER)
                    this.Text = text;

                //}}}
            }
            catch(Exception ex) {// {{{
                MessageBox.Show("NotePane: stage=["+ stage +"]:\n"
                    + this.ToString()
                    + Settings.ExToString(ex)
                    , "NotePane"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information
                    );
            } // }}}

            /* // {{{
            //if(name == "panel_log") {
            MessageBox.Show("NotePane:\n"
            +"name.=["+ name  +"]"+Environment.NewLine
            +"text.=["+ text  +"]"+Environment.NewLine
            +" _type.=["+ _type  +"]"+Environment.NewLine
            +" _label=["+ _label +"]"+Environment.NewLine
            +" from.=["+ from  +"]"+Environment.NewLine
            +" to.=["+ to  +"]"+Environment.NewLine
            +"  title=["+  title +"]"+Environment.NewLine
            +" _textPrefix.=["+ _textPrefix +"]"+Environment.NewLine
            , "NotePane"
            , MessageBoxButtons.OK
            , MessageBoxIcon.Information
            );

            //}
             */ //}}}

        }
        //}}}

        private void insert_tb() // {{{
        {
        //  _tb              = new System.Windows.Forms.RichTextBox();
            //_tb              = new                  NoteRichTextBox();
            _tb.ReadOnly     = _readOnly;
            _tb.Font         = TAB_FONT;
            _tb.Tag          = this;

        //  if((Type == TYPE_PANEL) || (Type == TYPE_DASH)) _tb.Document.PageWidth = 250;

            _tb.EnableAutoDragDrop = true;

            _tb.Dock         = System.Windows.Forms.DockStyle.Fill;


            this.Controls.Add( _tb );
        }
        // }}}
//        private void insert_mask() // {{{
//        {
//            _mask           = new MaskPanel();
//            _mask.Dock      = System.Windows.Forms.DockStyle.Fill;
//            this.Controls.Add( _mask );
//            _mask.SendToBack();
//        }
//        // }}}
        public bool is_a_control()       { return ( _type == TYPE_CONTROL ); }
        public bool is_a_dash()          { return ( _type == TYPE_DASH    ); }
        public bool is_a_panel()         { return ( _type == TYPE_PANEL   ); }
        public bool is_a_shortcut()      { return ( _type == TYPE_SHORTCUT); }
        public bool is_layoutSuspended() { return (bool)typeof(Control).GetProperty("IsLayoutSuspended", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this, null); }
        public void set_logger(LoggerInterface logger)// {{{
        {
            this.logger = logger;
        }
        // }}}
        // }}}
        // CompareTo {{{
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            NotePane np = obj as NotePane;
            if(np != null) {
                int result = String.Compare(this.Text, np.Text); // what's displayed!
                if(result == 0)
                    result = String.Compare(this.Name, np.Name); // what's not displayed!
                return result;
            }
            else
                throw new ArgumentException("CompareTo(not a NotePane)");
        }
        //}}}
        // VALIDATE  - - - - - - - - - - - - - - - - - - - - - - - - - - - -{{{
        // VALIDATE  - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // VALIDATE  - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private void invalidate(string changed_property) //{{{
        {
            //log("invalidate: changed_property=["+ changed_property +"]");
            _tab_line = "";

            if (changed_property == "Type") // {{{
            {
/*
                Padding padding;
                if(_type == TYPE_SHORTCUT)  padding = TYPE_SHORTCUT_PADDING;
                else                        padding =       DEFAULT_PADDING;

            //  if(this.Padding != padding)
*/

                // DEFAULT COLORS AND PADDING
                if(_type == TYPE_SHORTCUT)
                {
                    _tb.BackColor = ColorPalette.GetColorDarker (this.BackColor, 0.2);
                    _tb.ForeColor = ColorPalette.GetColorLighter(this.ForeColor, 0.5);

                    int p;
                    if(Settings.scaledGridSize > 10) p = Settings.scaledGridSize / 10;
                    else                             p = 1;
                    this.Padding = new Padding(p, p, p, p);
                }

                // SHORTCUT border and colors
                else {
                    this.Padding = new Padding(0,0,0,0);
                    _tb.ForeColor = this.BackColor;
                    _tb.BackColor = this.ForeColor;
                }

                // ToolTip
                update_tooltip();
            }
            // }}}
            if (changed_property == "Label") // {{{
            {
                _tb.Rtf = "";
            }
            // }}}
            if((changed_property == "xy_wh") || (changed_property == "zoom" )) //{{{
            {
                double ratio
                    = ((this.Type == TYPE_SHORTCUT) || (this.Type == TYPE_RICHTEXT))
                    ?  Settings.ratio
                    : 1.0;

                double grid_scaled = TabsCollection.TAB_GRID_S * ratio;

                string[] a = _xy_wh.Split(',');

                int x      = (int)(int.Parse( a[0] ) * grid_scaled);
                int y      = (int)(int.Parse( a[1] ) * grid_scaled);
                int w      = (int)(int.Parse( a[2] ) * grid_scaled);
                int h      = (int)(int.Parse( a[3] ) * grid_scaled);

                Size       = new Size (w, h);
                Location   = new Point(x, y);

                if(_zoom != "") {
                    float zf = 1F;
                    if((this.Type == TYPE_SHORTCUT) || (this.Type == TYPE_RICHTEXT))
                        zf = (float)(ratio * Settings.TXT_ZOOM);
                    _tb.Font   = new Font(TAB_FONT.FontFamily, TAB_FONT.Size * zf, TAB_FONT.Style, TAB_FONT.Unit);
                }

                if(_type == TYPE_SHORTCUT) {
                    int p;
                    if(Settings.scaledGridSize > 10) p = Settings.scaledGridSize / 10;
                    else                             p = 1;
                    this.Padding = new Padding(p, p, p, p);
                }

                //// XXX {{{
                //if((string)Tag == "TESTS_DASH")
                //    _tb.Text = String.Format("_xy_wh={0}\n._zoom={0}\n.Left={1}\n.Top={2}\n.Width={3}\n.Height={4}"
                //        , _xy_wh
                //        , _zoom
                //        , Left   / Settings.ratio
                //        , Top    / Settings.ratio
                //        , Width  / Settings.ratio
                //        , Height / Settings.ratio
                //        );
                ////}}}

            }
            // }}}
            if (changed_property == "color") // {{{
            {
                reserve_color();
                update_tooltip();
            }
            // }}}
            validate();
        }
        //}}}
        private void validate() //{{{
        {
            // MessageBox {{{
            /*
            MessageBox.Show("validate:"+ Environment.NewLine
                +"is_layoutSuspended=["+ is_layoutSuspended()   +"]"
                +".....is_validating=["+ is_validating()        +"]"
                +"..............Name=["+ Name                   +"]"+ Environment.NewLine
                +"............_label=["+ _label                 +"]"+ Environment.NewLine
                +"..............text=["+ Text                   +"]"+ Environment.NewLine
                +"......is_a_control=["+ is_a_control()         +"]"+ Environment.NewLine
                , "NotePane"
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information
                );

             */
            //}}}
            _tab_line = "";
            if( !is_validating )
            {
                is_validating = true;

                // RTF
                //{{{
                if(is_empty() || (_tb.Rtf == ""))
                    rebuild_rtf();

                if(    (this._type == TYPE_CONTROL )
                    || (this._type == TYPE_SHORTCUT)
                    || (this._type == TYPE_CONTROL )
                  ) {
                    if(this.Tag != null)
                    {
                    bool has_newline = (Text.IndexOf("\n") >= 0) || (Text.IndexOf( Environment.NewLine ) >= 0);
                    //  if( ((string)this.Tag).StartsWith("#") )    rtf_justify_left();
                        if( has_newline )   rtf_justify_left  ();
                        else                rtf_justify_center();
                    }
                    rtf_setNoScrollBars();
                }

                //}}}
                // CLEAR CUSTOM HANDLERS
                //{{{
                _tb.GotFocus    -= EH_tb_GotFocus;
                _tb.KeyDown     -= EH_tb_KeyDownS;
                _tb.KeyDown     -= EH_tb_KeyDown;
                _tb.Click       -= EH_tb_Click;
                _tb.DoubleClick -= EH_tb_DoubleClick;
                _tb.TextChanged -= EH_tb_TextChanged;
                _tb.MouseWheel  -= EH_tb_MouseWheel;

                //}}}
                // BUTTON == [CURSOR ARROW] [HIDECARET CALLBACK]
                //{{{
                if(is_a_control() || is_a_shortcut())
                {
                //  _tb.ReadOnly     = true;
                    _tb.Cursor       = Cursors.Arrow;


                    if(UI_state == 0) _tb.GotFocus  += EH_tb_GotFocus;
                    if(UI_state == 0) _tb.KeyDown   += EH_tb_KeyDownS;

                    _tb.Click                       += EH_tb_Click;

                }
                //}}}
                //{{{
                else if(is_a_panel() || is_a_dash()) {
                    _tb.Cursor       = Cursors.IBeam;
                    _tb.DoubleClick  += EH_tb_DoubleClick;
                }
                //}}}
                // PANEL  == [CURSOR IBEAM]
                //{{{
                else {
                //  _tb.ReadOnly     = _readOnly;
                    _tb.Cursor       = Cursors.IBeam;

                }

                //}}}
                // CUSTOM HANDLERS [ESCAPE==SHOW_OR_EVAL_RTF] [IS_EMPTY==RESTORE] [_LOCKED == ENABLED]
                //{{{
                _tb.KeyDown         += EH_tb_KeyDown;
                _tb.TextChanged     += EH_tb_TextChanged;
                _tb.MouseWheel      += EH_tb_MouseWheel;

                //}}}

                // TOOLTIP {{{

                update_tooltip();

                //}}}

                _tb_enable( _locked );
                is_validating = false;
            }
        }
        //}}}
        private void _tb_enable(bool state) //{{{
        {
            _tb.Enabled   =  state;
            if(!_tb.Enabled && (Type != TYPE_CONTROL)) toolTip.SetToolTip(_tb, null);
/*
            if((UI_state == Settings.STATE_LAYOUT)
                _tb.Visible = state;
            else
                _tb.Visible = true;
*/

/*
            if(state) {
                _mask.SendToBack();
                _mask.Enabled = false;
                _mask.Visible = false;
            }
            else {
                _mask.Enabled = false;
                _mask.Visible = true;
                _mask.BringToFront();
            }
            _mask.Refresh();

*/

        }
        //}}}
        // VALIDATE  - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // VALIDATE  - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // VALIDATE  - - - - - - - - - - - - - - - - - - - - - - - - - - - -}}}
        // SELECT {{{
        public void select()
        {
            _tb.Selected = true;
            _tb.Invalidate();
        }
        public void unselect()
        {
            _tb.Selected = false;
            _tb.Invalidate();
        }
        public bool isSelected()
        {
            return _tb.Selected;
        }
        //}}}
        // EDIT  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -{{{
        // EDIT  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // EDIT  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // edit_tag {{{
        public void edit_tag()
        {
            //log("edit_tag(): tag_old=["+ _type +"]");

            // CURRENT
            string tag_new = (string)this.Tag;

            // DISPLAY
            string tag_old = (tag_new != "") ? tag_new : "#";//this.Text;
            tag_old        = tag_old.Replace("[\n", " ");
            tag_old        = tag_old.Replace("[\t", " ");
            tag_old        = tag_old.Trim();

            // EDIT
            if(TabTagForm.ShowDialog(this, tag_old, out tag_new) == DialogResult.OK)
            {
                tag_new  = tag_new.Replace("[\n", " ");
                tag_new  = tag_new.Replace("[\t", " ");
                tag_new  = tag_new.Trim();
                if(!this.Tag.Equals(tag_new)) {
                    this.Tag = tag_new;
                    TabsCollection.Clone_selection_Tag( this );
                }
            }
        }
        //}}}
        // Clone {{{
        public static NotePane Clone(NotePane np)
        {
            NotePane np_clone       = new NotePane(np._type, np.Name, np.Text, np._color);
            np_clone.Tag            = np.Tag;
            np_clone._label         = np._label;
            np_clone._tb.BackColor  = np._tb.BackColor;
            np_clone._tb.Font       = np._tb.Font;
            np_clone._tb.ForeColor  = np._tb.ForeColor;
            np_clone._tb.ZoomFactor = np._tb.ZoomFactor;
            np_clone._tb.Enabled    = true; // so it can be edited
            np_clone._xy_wh         = np._xy_wh;
            np_clone._zoom          = np._zoom;
            np_clone._color         = np._color;
            np_clone._shape         = np._shape;
            np_clone._tt            = np._tt;
            np_clone.update_tooltip();

            // clone's event handlers
            np_clone._tb.GotFocus    -= EH_tb_GotFocus   ; // DO NOT HideCaret
            np_clone._tb.Click       -= EH_tb_Click      ; // DO NOT HideCaret HideSelection logger_callback
            np_clone._tb.DoubleClick -= EH_tb_DoubleClick; // DO NOT zoom-filter maximize_xy_wh-restore_xy_wh
            np_clone._tb.KeyDown     -= EH_tb_KeyDownS   ; // DO NOT SuppressKeyPress

            np_clone._tb.MouseWheel  += EH_tb_MouseWheel ; // DO allow zoom (adjusting ZoomFactor)
            np_clone._tb.TextChanged += EH_tb_TextChanged; // DO restore RTF content when cleared
            np_clone._tb.KeyDown     += EH_tb_KeyDown    ; // DO allow   RTF editing

            np_clone.logger          = np.logger;

            return np_clone;
        }
        //}}}
        // EDIT  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // EDIT  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // EDIT  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -}}}
        // COMPOSITION {{{
        public void update_tooltip() // {{{
        {
            string t = this._tt;    // explicit tooltip

            if(t == "") {
                string short_name = Name;
                int    idx        = Name.LastIndexOf("_"); if(idx > 0) short_name = Name.Substring(idx+1);
                t   =                      short_name     +"\n"
                    + " .. ["+             Type           +"]\n"
                    + " .. [     xy_wh: "+ xy_wh          +"]\n"
                    + " .. [     color: "+ color          +"]\n"
                    + " .. [     shape: "+ shape          +"]\n"
                    + " .. [      zoom: "+ zoom           +"]\n"
                    + " .. [ZoomFactor: "+ _tb.ZoomFactor +"]\n"
                    + " .. [ Font.Size: "+ _tb.Font.Size  +"]\n"
                    ;
            }

            if(this.Tag != null) {
                string            s = this.Tag.ToString();
                if(s.Length > 80) s = s.Substring(0, 80) +"...";
                t += "\n"+s;
            }

            t = t.Replace("[\n", " ");
            t = t.Replace("[\t", " ");
            t = t.Trim();

            this.toolTip.SetToolTip(this,     t);
            this.toolTip.SetToolTip(this._tb, t);
        }
        //}}}
        // RTF BUILD & RELOAD  - - - - - - - - - - - - - - - - - - - - - - -{{{
        // RTF BUILD & RELOAD  - - - - - - - - - - - - - - - - - - - - - - -
        // RTF BUILD & RELOAD  - - - - - - - - - - - - - - - - - - - - - - -
        public void   rebuild_rtf() // {{{
        {
            // RTF [Settings.rtf] {{{
            string rtf = "";

            if(   Name.Contains("panel_usr"  ) && (this.Tag != null) && (this.Tag.ToString() == "#"))
                rtf = @"{\rtf1\ansi "+ _labelPrefix + new Regex("panel_usr0*").Replace(Name, "") +@"}";
            else
                rtf = Settings.LoadSetting("TAB."+this.Name+".rtf", "");

            if(rtf != "") {
                _tb.Rtf = rtf.Trim();

            }
            // }}}
            else {
                string s = "";
                // RTF from [Name] {{{
                if(s == "") {
                    //s = "Name="+Name +"\n-NO Label\n-NO Tag";
                    if(    Name.Contains("panel_usr"  )
                        && (this.Tag != null)
                        && (this.Tag.ToString().StartsWith("PROFILE"))
                      )
                        s = new Regex("^PROFILE.*[ /]").Replace(this.Tag.ToString(), "");
                    //else s = new Regex("panel_usr0*").Replace(Name, "");
//s += " Name";
                }

                // }}}
                // RTF from [Label] {{{

            //  if(is_a_control() && (_label != ""))
                if((s=="") && (_label != ""))
                {
                    s = _label;
                    //  s = new Regex(@"([a-z0-9])([A-Z_]+)").Replace(s, @"$1\line $2").Replace("_",""); // split camelCase words
                    s = new Regex(@"([a-z0-9])([A-Z_]+)").Replace(s,      @"$1 $2").Replace("_",""); // split camelCase words

                }
                // }}}
                // RTF from [Tag] {{{
                if((s=="") && (this.Tag != null))
                {
                    s = GetTagLabel(this.Tag.ToString());
                    if(s != "")
                        s = s.Replace(@"\",@"\\").Replace(@"\n",@"\par");
//s += " Tag";
                }

                // }}}
                // FORMAT CONTROL {{{
                if( is_a_control() )
                {
                    _tb.Rtf  = @"{\rtf1\ansi "+ _labelPrefix + s.Replace("\n", @"\par\bullet") +@"}";

//_tb.Rtf += " is_a_control";
                }
                // }}}
                // FORMAT SHORTCUT {{{
                else if(this._type == TYPE_SHORTCUT)
                {
                    //s = GetTagLabel(s);
                    _tb.Rtf  = @"{\rtf1\ansi "+ _labelPrefix + s +@"}";
//_tb.Rtf += " TYPE_SHORTCUT";
                }
                // }}}
                // FORMAT PANEL & DASH {{{
                else if((this._type == TYPE_PANEL) || (this._type == TYPE_DASH))
                {
                    _tb.Rtf  = @"{\rtf1\ansi "+ _textPrefix      +@"\par}";
//_tb.Rtf += " TYPE_PANEL or TYPE_DASH";
                }
                // }}}
                // FORMAT RICHTEXT - missing [Settings.rtf] {{{
                else {
                    //_tb.Rtf  = @"{\rtf1\ansi "+ _textPrefix            +@"\par}"+ _tb.Text;
                    if(this.Tag != null)
                    {
                        s = GetTagLabel(this.Tag.ToString());
                        _tb.Rtf  = @"{\rtf1\ansi "+ _textPrefix +s           +@"\par}";
//_tb.Rtf += " else Tag";
                    }
                }
                // }}}
            }

            // INSERTION POINT AT END {{{
            if(!is_a_control() && !is_a_shortcut())
            {
                _tb.Select(_tb.Text.Length, 0);
            //  _tb.AppendText(Environment.NewLine);
            }
            // }}}

            //_tb.ZoomFactor = zoom;
            reload_saved_zoom(); // already called by TabsCollection.add_tab()
        }
        //}}}
        public void   reload_saved_tag() //{{{
        {
            string key          = "TAB."+this.Name+".tag";
            string saved_tag    = Settings.LoadSetting(key, "");
            if(saved_tag != "")
            {
                this.Tag = saved_tag;
            }
        }
        //}}}
        public void   reload_saved_zoom() //{{{
        {
            _tb.SelectAll();
            _tb.ZoomFactor = 1.0F; // https://social.msdn.microsoft.com/forums/windows/en-us/8b61eef0-b712-4b8b-9f5f-c9bbf75abb53/richtextbox-zoomfactor-problems
            try { _tb.ZoomFactor = float.Parse( get_saved_zoom() ); } catch(Exception) { }
            _tb.DeselectAll();
        }
        //}}}
        private static string GetTagLabel(string np_tag) //{{{
        {
            int idx;

            // REMOVE PATH
            idx = np_tag.LastIndexOf( " "   ); if(idx > 0) np_tag = np_tag.Substring(idx+1).Trim();
            idx = np_tag.LastIndexOf(@"\"   ); if(idx > 0) np_tag = np_tag.Substring(idx+1).Trim();
            idx = np_tag.LastIndexOf(@"/"   ); if(idx > 0) np_tag = np_tag.Substring(idx+1).Trim();
            idx = np_tag.LastIndexOf( "="   ); if(idx > 0) np_tag = np_tag.Substring(idx+1).Trim();

            // REMOVE EXT
            idx = np_tag.LastIndexOf(".exe" ); if(idx > 0) np_tag = np_tag.Substring(0,idx).Trim();
            idx = np_tag.LastIndexOf(".msc" ); if(idx > 0) np_tag = np_tag.Substring(0,idx).Trim();
            idx = np_tag.LastIndexOf(".bat" ); if(idx > 0) np_tag = np_tag.Substring(0,idx).Trim();
            idx = np_tag.LastIndexOf(".ps1" ); if(idx > 0) np_tag = np_tag.Substring(0,idx).Trim();

            return np_tag;
        }
        //}}}


        // TODO work with RichTextBox FontSize (Inherited from Control)
        // TODO CaretBrush Property (to hide caret) https://msdn.microsoft.com/en-us/library/system.windows.controls.primitives.textboxbase.caretbrush(v=vs.110).aspx
        // TODO Opacity Property https://msdn.microsoft.com/en-us/library/system.windows.uielement.opacity(v=vs.110).aspx

        // RTF PROPERTIES
        public  void   rtf_justify_left()// {{{
        {
            _tb.SelectAll();
            _tb.SelectionAlignment  = HorizontalAlignment.Left;
            _tb.DeselectAll();
        }
        // }}}
        public  void   rtf_justify_center()// {{{
        {
            _tb.SelectAll();
            _tb.SelectionAlignment  = HorizontalAlignment.Center;
            _tb.DeselectAll();
            /*
            _tb.Top                 = _tb.Parent.Height - _tb.Height / 2;
            */
            //_tb.Top                 = _tb.Parent.Height - _tb.Height / 2;
            // -----------------------    L,  T,  R,  B
        //  this.Padding = new Padding(   0,100,  0,  0);
        //  this.Padding.Top = this.Height/2 - _tb.Height/2;

        // _tb.Font.FontFamily.
        //int top          = (this.Height/2) - (_tb.Height/2);
        //  int top = _tb.FontHeight;
            // int top = 100;
            //int top = _tb.Font.FontFamily.GetLineSpacing(FontStyle.Bold);
            //int top = _tb.Font.Height;
            //this.Padding     = new Padding( 5,  top,  5,  5);
            //_tb.Text = " top=["+ top +"]";

        }
        // }}}
        public  void   rtf_Font_BIG()// {{{
        {
            _tb.Font = new System.Drawing.Font("Tahoma"        , 36F  , System.Drawing.FontStyle.Bold   , System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
        // }}}
        public  void   rtf_Font_console()// {{{
        {
            _tb.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
        // }}}
//        public  void   rtf_FontHeight(int fontHeight)// {{{
//        {
//            _tb.FontHeight = fontHeight;
//        }
//        // }}}
        public  void   rtf_ForeColor(Color foreColor)// {{{
        {
            _tb.ForeColor = foreColor;
        }
        // }}}
        public  void   rtf_setNoScrollBars()// {{{
        {
            _tb.ScrollBars = RichTextBoxScrollBars.None;
        }
        // }}}

        private string get_img_Rtf(NotePane np, string fileName) //{{{
        {
            string img_rtf  = null;
            try {
                img_rtf = RtfPict.embedImage(this, fileName);
            }
            catch(Exception ex) {// {{{
                MessageBox.Show("get_img_Rtf:\n"
                    +".Name=["+ Name +"]"+Environment.NewLine
                    +".fileName=["+fileName +"]"    + Environment.NewLine
                    + Settings.ExToString(ex)
                    , "NotePane"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information
                    );
            } // }}}
            return img_rtf;
        }
        //}}}
        private string get_img_fileName() //{{{
        {
            return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\WinBg.jpg";
            //  return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\bar.png";
            //  return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\RTabs.ico";
            //  return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\tagDark.png";
            //  return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\tagDark_flat.png";
            //  return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\tagLight.png";
            //  return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\tagLight_flat.png";
            //  return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\tshadowdown.png";
            //  return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\tshadowdownleft.png";
            //  return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\tshadowdownright.png";
            //  return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\tshadowright.png";
            //  return @"C:\LOCAL\STORE\DEV\QUADRANS\PROJECTS\RTabs\Util\Images\tshadowtopright.png";
        }
        //}}}
        private void show_or_eval_RTF() //{{{
        {
            if(_tb.Text.StartsWith(@"{\rtf"))
            {
                _tb.Rtf  = _tb.Text;
            }
            else {
                _tb.Text = _tb.Rtf;
                _tb.Focus();
            }

            //_tb.Enabled = false;
            _tb_enable( true );

            Refresh();
        }
        //}}}
        // RTF BUILD & RELOAD  - - - - - - - - - - - - - - - - - - - - - - -
        // RTF BUILD & RELOAD  - - - - - - - - - - - - - - - - - - - - - - -
        // RTF - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -}}}
        // Saved Settings {{{

        // - ORDER-DEPENDENT VERSION
        private const int FIELD_IDX_TYPE  = 0, FIELD_LEN_TYPE  = 5;
        private const int FIELD_IDX_TAG   = 1, FIELD_LEN_TAG   = 4;
        private const int FIELD_IDX_ZOOM  = 2, FIELD_LEN_ZOOM  = 5;
        private const int FIELD_IDX_XY_WH = 3, FIELD_LEN_XY_WH = 6;
        private const int FIELD_IDX_TEXT  = 4, FIELD_LEN_TEXT  = 5;
        private const int FIELD_IDX_COLOR = 5, FIELD_LEN_COLOR = 6;
        private const int FIELD_IDX_SHAPE = 6, FIELD_LEN_SHAPE = 6;
        private const int FIELD_IDX_TT    = 7, FIELD_LEN_TT    = 3;

        private void parse_tab_line()
        {
            _tab_line = Settings.LoadSetting("TAB."+ Name);

            string[] fields = _tab_line.Split(NotePane.TABVALUE_SEPARATOR);

/* STATIC // {{{
            _tab_line_type  = ""; try { _tab_line_type  = fields[ FIELD_IDX_TYPE  ].Substring( FIELD_LEN_TYPE  ); } catch(Exception) {}
            _tab_line_tag   = ""; try { _tab_line_tag   = fields[ FIELD_IDX_TAG   ].Substring( FIELD_LEN_TAG   ); } catch(Exception) {}
            _tab_line_zoom  = ""; try { _tab_line_zoom  = fields[ FIELD_IDX_ZOOM  ].Substring( FIELD_LEN_ZOOM  ); } catch(Exception) {}
            _tab_line_xy_wh = ""; try { _tab_line_xy_wh = fields[ FIELD_IDX_XY_WH ].Substring( FIELD_LEN_XY_WH ); } catch(Exception) {}
            _tab_line_text  = ""; try { _tab_line_text  = fields[ FIELD_IDX_TEXT  ].Substring( FIELD_LEN_TEXT  ); } catch(Exception) {}
            _tab_line_color = ""; try { _tab_line_color = fields[ FIELD_IDX_COLOR ].Substring( FIELD_LEN_COLOR ); } catch(Exception) {}
            _tab_line_shape = ""; try { _tab_line_shape = fields[ FIELD_IDX_SHAPE ].Substring( FIELD_LEN_SHAPE ); } catch(Exception) {}
            _tab_line_tt    = ""; try { _tab_line_tt    = fields[ FIELD_IDX_TT    ].Substring( FIELD_LEN_TT    ); } catch(Exception) {}
*/ // }}}

            // dynamically check the position of each field .. then pick its value
            int _tab_line_len = _tab_line.Length;
            int idx_sep;
            int idx_val;

            string k;
            k = "type="  ; try { idx_val = _tab_line.IndexOf(k) + k.Length;    idx_sep = _tab_line.IndexOf(NotePane.TABVALUE_SEPARATOR, idx_val); if(idx_sep<0) idx_sep = _tab_line_len;  _tab_line_type  = _tab_line.Substring(idx_val, idx_sep-idx_val); } catch(Exception) {}
            k = "tag="   ; try { idx_val = _tab_line.IndexOf(k) + k.Length;    idx_sep = _tab_line.IndexOf(NotePane.TABVALUE_SEPARATOR, idx_val); if(idx_sep<0) idx_sep = _tab_line_len;  _tab_line_tag   = _tab_line.Substring(idx_val, idx_sep-idx_val); } catch(Exception) {}
            k = "zoom="  ; try { idx_val = _tab_line.IndexOf(k) + k.Length;    idx_sep = _tab_line.IndexOf(NotePane.TABVALUE_SEPARATOR, idx_val); if(idx_sep<0) idx_sep = _tab_line_len;  _tab_line_zoom  = _tab_line.Substring(idx_val, idx_sep-idx_val); } catch(Exception) {}
            k = "xy_wh=" ; try { idx_val = _tab_line.IndexOf(k) + k.Length;    idx_sep = _tab_line.IndexOf(NotePane.TABVALUE_SEPARATOR, idx_val); if(idx_sep<0) idx_sep = _tab_line_len;  _tab_line_xy_wh = _tab_line.Substring(idx_val, idx_sep-idx_val); } catch(Exception) {}
            k = "text="  ; try { idx_val = _tab_line.IndexOf(k) + k.Length;    idx_sep = _tab_line.IndexOf(NotePane.TABVALUE_SEPARATOR, idx_val); if(idx_sep<0) idx_sep = _tab_line_len;  _tab_line_text  = _tab_line.Substring(idx_val, idx_sep-idx_val); } catch(Exception) {}
            k = "color=" ; try { idx_val = _tab_line.IndexOf(k) + k.Length;    idx_sep = _tab_line.IndexOf(NotePane.TABVALUE_SEPARATOR, idx_val); if(idx_sep<0) idx_sep = _tab_line_len;  _tab_line_color = _tab_line.Substring(idx_val, idx_sep-idx_val); } catch(Exception) {}
            k = "shape=" ; try { idx_val = _tab_line.IndexOf(k) + k.Length;    idx_sep = _tab_line.IndexOf(NotePane.TABVALUE_SEPARATOR, idx_val); if(idx_sep<0) idx_sep = _tab_line_len;  _tab_line_shape = _tab_line.Substring(idx_val, idx_sep-idx_val); } catch(Exception) {}
            k = "tt="    ; try { idx_val = _tab_line.IndexOf(k) + k.Length;    idx_sep = _tab_line.IndexOf(NotePane.TABVALUE_SEPARATOR, idx_val); if(idx_sep<0) idx_sep = _tab_line_len;  _tab_line_tt    = _tab_line.Substring(idx_val, idx_sep-idx_val); } catch(Exception) {}
/* DYNAMIC // {{{
*/ // }}}

            // POST LOAD NORMALIZATION
            int color_int = 0; try { color_int = int.Parse( _tab_line_color ); } catch(Exception) { } _tab_line_color = String.Format("{0:D2}", color_int);
            if(_tab_line_color == "00") _tab_line_color = "";

        }

        // PROPERTIES GET ACCESSOR TO TRIGGER PARSE_TAB_LINE .. (when _tab_line=="")
        public string get_saved_type () { return tab_line_type ; }
        public string get_saved_tag  () { return tab_line_tag  ; }
        public string get_saved_zoom () { return tab_line_zoom ; }
        public string get_saved_xy_wh() { return tab_line_xy_wh; }
        public string get_saved_text () { return tab_line_text ; }
        public string get_saved_color() { return tab_line_color; }
        public string get_saved_shape() { return tab_line_shape; }
        public string get_saved_tt   () { return tab_line_tt   ; }

/* // {{{
*/ // }}}

        // @see parse_TABS() in /LOCAL/DATA/ANDROID/PROJECTS/RTabs/app/src/main/java/ivanwfr/rtabs/RTabsClient.java

        //}}}
        // encode decode {{{
    //  private static string SYMBOL_BACKSLASH = "U+005C";
    //  private static string SYMBOL_EQUALS    = "U+003D";
        private static string SYMBOL_VBAR      = "U+007C";
        public  static string encode_text(string s)
        {
            return s
        //  .Replace(              "=" , SYMBOL_EQUALS   )
            .Replace(              "|" , SYMBOL_VBAR     )
        //  .Replace(             "\\" , SYMBOL_BACKSLASH)
            .Replace(             "\n" , "\\n"           )    // eval last (after lone backslaches)
            ;
        }

        public static string decode_text(string s)
        {
            return s
            .Replace(            "\\n" , "\n"            )   // eval first (before lone backslaches)
        //  .Replace(SYMBOL_BACKSLASH  , "\\"            )
        //  .Replace(SYMBOL_EQUALS     ,  "="            )
            .Replace(SYMBOL_VBAR       ,  "|"            )
            ;
        }
        //}}}
        //}}}
        // GEOMETRY {{{
        public  void dev_dpi_Changed()// {{{
        {
            if(is_validating) return;
            invalidate("xy_wh");
        }
        // }}}
        public  void move_back()// {{{
        {
            if(xy_wh_to_move_back_to != "") {
                _xy_wh = xy_wh_to_move_back_to;
                invalidate("xy_wh");
            }
            else {
                reload_saved_xy_wh();
            }
        }
        // }}}
        public  void reload_saved_xy_wh()// {{{
        {
            _xy_wh = get_saved_xy_wh();
            invalidate("xy_wh");
        }
        // }}}
        // private void maximize()// {{{
        private static float ZOOM_MIN = 1F/16F;
        private static float ZOOM_MAX =     5F;
        public void maximize_xy_wh()
        {
            //try{
            this.Tag
                = "xy_wh="+Left+","+Top+","+Width+","+Height
                + " zoom="+_tb.ZoomFactor;

//string tag = (string)(this.Tag);
//this.toolTip.SetToolTip( _tb, tag);
//this.toolTip.SetToolTip(this, tag);

        //  float ratio     = (float)(this.Parent.Width * this.Parent.Height) / (float)(this.Width * this.Height);
            float ratio     = (float)(this.Parent.Width                     ) / (float)(this.Width              );
            float zf        = _tb.ZoomFactor * ratio;

        //  this.Location     = new Point(TabsCollection.TAB_GRID_S, TabsCollection.TAB_GRID_S);
        //  this.Size         = new Size (Parent.Width - 2*TabsCollection.TAB_GRID_S, Parent.Height - 2*TabsCollection.TAB_GRID_S);
            this.Location     = new Point(1             , 1              );
            this.Size         = new Size (Parent.Width-2, Parent.Height-2);

            if((zf > ZOOM_MIN) && (zf < ZOOM_MAX))
            {
                this._tb.ZoomFactor = 1.0F; // magic back to default first ?
                this._tb.ZoomFactor = zf;
            }

            //} catch(Exception) {}

            // bring to front
            this.Parent.Controls.SetChildIndex(this, 0);
        }
        // }}}
        // private void restore_xy_wh()// {{{
        private void restore_xy_wh()
        {
            //try{
                string tag = (string)this.Tag;

                string[] tuple = tag.Split(' ');

                string[] a = tuple[0].Substring(6).Split(',');
                int   x    = int.Parse( a[0] );
                int   y    = int.Parse( a[1] );
                int   w    = int.Parse( a[2] );
                int   h    = int.Parse( a[3] );
                float zf   = float.Parse( tuple[1].Substring(5) );

//tag = (string)(this.Tag) +"\n" + "restore: ["+ x +"]["+ y +"]_["+ w +"]["+ h +"]_["+ zf +"]";
//this.toolTip.SetToolTip( _tb, tag);
//this.toolTip.SetToolTip(this, tag);

                _tb.ZoomFactor   = 1.0F;
                _tb.ZoomFactor   = zf;
                Location         = new Point(x,y);
                Size             = new Size (w,h);

                this.Tag         = get_saved_tag();

            //} catch(Exception) {}
        }
        // }}}
        // private bool is_maximized()// {{{
        public bool is_maximized()
        {
            string tag = (this.Tag != null) ? (string)this.Tag : "";
            return tag.StartsWith("xy_wh=");
        }
        // }}}

        //}}}
        // EVENTS .. RichTextBox {{{

/* Notes {{{
" MouseWheelEventHandler Delegate:
:!start explorer "https://msdn.microsoft.com/en-us/library/system.windows.input.mousewheeleventhandler(v=vs.110).aspx"

" Routed Events Overview:
:!start explorer "https://msdn.microsoft.com/en-us/library/vstudio/ms742806(v=vs.100).aspx"

*/ //}}}

        // _tb_GotFocus {{{
        private static           System.EventHandler
            EH_tb_GotFocus = new System.EventHandler( _tb_GotFocus );

        private static void _tb_GotFocus(object sender, EventArgs e)
        {
            RichTextBox _tb = (RichTextBox)sender;
            NotePane     np = (NotePane)_tb.Tag;
/*
            if(Name == PANEL_NAME_XPORT) {
                _tb.SelectAll();
                _tb.Focus();
            }
*/
            NativeMethods.HideCaret(sender, e);
        }
        //}}}
        // _tb_TextChanged {{{
        private static              System.EventHandler
            EH_tb_TextChanged = new System.EventHandler( _tb_TextChanged );

        private static void _tb_TextChanged(object sender, EventArgs e)
        {
            RichTextBox _tb = (RichTextBox)sender;
            NotePane     np = (NotePane)_tb.Tag;

            if(np.is_validating) return;
            if( np.is_empty() )
                np.invalidate("_tb_TextChanged Text");
        }
        //}}}

        // _tb_KeyDown_suppress {{{
        private static           System.Windows.Forms.KeyEventHandler
            EH_tb_KeyDownS = new System.Windows.Forms.KeyEventHandler( _tb_KeyDown_suppress );

        private static void _tb_KeyDown_suppress(object sender, KeyEventArgs e)
        {
            RichTextBox _tb = (RichTextBox)sender;
            NotePane     np = (NotePane)_tb.Tag;

            //log("_tb_KeyDown_suppress");
            e.SuppressKeyPress  = true;
            np._tb.HideSelection   = true;
            np._tb.SelectionLength = 0;
        }
        //}}}
        // _tb_KeyDown {{{
        private static          System.Windows.Forms.KeyEventHandler
            EH_tb_KeyDown = new System.Windows.Forms.KeyEventHandler( _tb_KeyDown );

        private static void _tb_KeyDown(object sender, KeyEventArgs e)
        {
            RichTextBox _tb = (RichTextBox)sender;
            NotePane     np = (NotePane)_tb.Tag;

            //log("_tb_KeyDown");

            if(e.KeyCode == Keys.Escape)
            {
                if( np.is_maximized() )
                    np.restore_xy_wh();

                else if(e.Shift                         // RTF EDITING enter with shift
                    || (np._tb.Text.StartsWith(@"{\rtf"))  // ...quit no matter shift
                    )
                    np.show_or_eval_RTF();

            }

        }
        //}}}

        // _tb_ButtonClick {{{
        private static        System.EventHandler
            EH_tb_Click = new System.EventHandler( _tb_ButtonClick );

        private static void _tb_ButtonClick(object sender, EventArgs e)
        {
            RichTextBox _tb = (RichTextBox)sender;
            NotePane     np = (NotePane)_tb.Tag;

            if(UI_state == 0) {
                HideCaret(np._tb.Handle);
                np._tb.HideSelection   = true;
                np._tb.SelectionLength = 0;
            }

            //if(_tb.SelectionLength == 0) {
                np.logger_callback("Click");
            //}
        }

        [DllImport("user32.dll", EntryPoint = "HideCaret")]
            public static extern long HideCaret(IntPtr hwnd);

        // }}}
        // _tb_DoubleClick {{{
/*
        private void _tb_DoubleClick(object sender, EventArgs e)
        {
            if(UI_state == 0)   MainFormInstance.control_layout_Click();
            else                MainFormInstance.MainForm_cancel();
        }
*/
        //}}}
        // _tb_MouseWheel {{{
        private static             MouseEventHandler
            EH_tb_MouseWheel = new MouseEventHandler( _tb_MouseWheel );

        private static void _tb_MouseWheel(object sender, EventArgs e)
        {
            RichTextBox _tb = (RichTextBox)sender;
            NotePane     np = (NotePane)_tb.Tag;

            if((np.Type == TYPE_SHORTCUT) || (np.Type == TYPE_RICHTEXT) || (np.Type == TYPE_CONTROL))
                np._zoom = string.Format("{0:#.##}", (np._tb.ZoomFactor / Settings.TXT_ZOOM));
            else
                np._zoom = string.Format("{0:#.##}", (np._tb.ZoomFactor                    ));
            //_tb.Text = "MW: _zoom=["+ _zoom +"] ZoomFactor=["+ _tb.ZoomFactor +"]\n";
            new Thread(new ThreadStart( np._tb_ZSYNC )).Start();
        }

        // _tb_ZSYNC {{{
        private void _tb_ZSYNC()
        {
            Thread.Sleep(10);
            //if(!_tb.InvokeRequired ) {
                if((this.Type == TYPE_SHORTCUT) || (this.Type == TYPE_RICHTEXT) || (this.Type == TYPE_CONTROL))
                    _zoom = string.Format("{0:#.##}", (_tb.ZoomFactor / Settings.TXT_ZOOM));
                else
                    _zoom = string.Format("{0:#.##}", (_tb.ZoomFactor                    ));
                update_tooltip();
            //}
        }
        //}}}
/*
        // _tb_MouseWheel {{{
        private static void _tb_MouseWheel(object sender, EventArgs e)
        {
            RichTextBox _tb = (RichTextBox)sender;
            NotePane     np = (NotePane)_tb.Tag;

            if((np.Type == TYPE_SHORTCUT) || (np.Type == TYPE_RICHTEXT))
                np.zoom = string.Format("{0,2:F}", (_tb.ZoomFactor / Settings.TXT_ZOOM));
            else
                np._zoom = string.Format("{0,2:F}",  _tb.ZoomFactor);

            update_tooltip();
        }
        //}}}
*/
        //}}}
        // _tb_PanelDoubleClick {{{
        private static              System.EventHandler
            EH_tb_DoubleClick = new System.EventHandler( _tb_PanelDoubleClick );

        private static void _tb_PanelDoubleClick(object sender, EventArgs e)
        {
            RichTextBox _tb = (RichTextBox)sender;
            NotePane     np = (NotePane)_tb.Tag;

            // MAY ZOOM ONLY WHEN TABS MANIPUTLATION IS NOT INVOLVED
            if(    (UI_state == Settings.STATE_EDIT  )
                || (UI_state == Settings.STATE_LAYOUT)
              ) {
                Settings.Beep("...may zoom only when layout is not involved");
            }
            else if( !IsKeyDown(VirtualKeyCode.CONTROL) )
            {
                if( np.is_maximized() ) np. restore_xy_wh();
                else                    np.maximize_xy_wh();
            }
            else {
                np._tb.Text = "_zoom=["+ np._zoom +"] ZoomFactor=["+ np._tb.ZoomFactor +"]";
            }

            if(np.Name == PANEL_NAME_XPORT) {
                if(!np.is_maximized() )     np.maximize_xy_wh();
                np._tb.SelectAll();
            //  Clipboard.SetText(_tb.Text);    // automatic copy may be overkill !?
            }
        }

        private static bool IsKeyDown(VirtualKeyCode keyCode)
        {
            Int16 result = GetKeyState((UInt16)keyCode);

            return (result < 0);
        }

        [DllImport("user32.dll", SetLastError = true)]
            static extern Int16 GetKeyState(UInt32 virtualKeyCode);

        // }}}

        // tb_OnMouseClick - BUBBLE UP TO PARENT WHEN DISABLED {{{
        public void tb_OnMouseClick(MouseEventArgs e)
        {
            OnMouseClick(e);
        }
        //}}}

        // maximize_zoom {{{
/*
        public void maximize_zoom()
        {
            float z_before = _tb.ZoomFactor;

            Graphics graphics = null;
            using(graphics = CreateGraphics()) {

                SizeF stringSize = graphics.MeasureString(this.Text, this.Font);

                while( ((stringSize.Width  * _tb.ZoomFactor) < Width)
                    && ((stringSize.Height * _tb.ZoomFactor) < Height)
                    )
                    _tb.ZoomFactor += 1.2f;

                while( ((stringSize.Width  * _tb.ZoomFactor) > Width)
                    || ((stringSize.Height * _tb.ZoomFactor) > Height)
                    )
                    _tb.ZoomFactor /= 1.2f;

            }
            //// XXX {{{
            //if((string)Tag == "TESTS_DASH")
            //    _tb.Text = String.Format("adjust_zoom: from {0}\n to={1}"
            //        , z_before
            //        , _tb.ZoomFactor
            //        );
            ////}}}
            invalidate("zoom");
        }
*/
        //}}}

        // }}}
        // EVENTS .. NotePane {{{
        // MOVE AND RESIZE
        // variables {{{
        //  public  bool        _locked      = true;

        private bool        dragging    = false;
        private bool        resized     = false;
        private string      xy_wh_to_move_back_to = "";
        private int         gridSize    = TabsCollection.TAB_GRID_S;
        private bool        moving      = false;

        private bool        mouseDown   = false;
        private bool        mouseInside = false;

        private bool        resize_B    = false;
        private bool        resize_L    = false;
        private bool        resize_R    = false;
        private bool        resize_T    = false;

        private Point       dragPoint   = new Point();
        private Rectangle   origin      = new Rectangle();
        // }}}

        // OnMouseUp {{{
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            mouseDown   = false;
            moving      = false;
            if(dragging) {
                dragging    = false;

                if( isSelected() )
                    TabsCollection.Selection_grid_layout( false );
                else 
                    grid_layout( false );

                if(UI_state == Settings.STATE_LAYOUT)
                {
                    Point p = PointToScreen(e.Location);
                    int  dx = p.X - dragPoint.X;
                    int  dy = p.Y - dragPoint.Y;
                    if((dx != 0) || (dy != 0))
                    {
                        if(logger != null)
                            logger.notify(this, "OnMouseUp");   // ...moved
                    }

                    if( !resized ) {
                        if(this.Parent != null) { // i.e. not deleted by Mouse right-click .. del_tabs_named
                            if(this.Parent.Controls.GetChildIndex(this) != 0) this.Parent.Controls.SetChildIndex(this, 0);
                            else                                              this.Parent.Controls.SetChildIndex(this, this.Parent.Controls.Count);
                        }
                    }

                }

                resized     = false;
            }

/*
if(dragging) {
_mask.SendToBack();
//_mask.Visible = false;
//_tb.Text= _tb.Text +" ";
_tb.Enabled = true;
_tb.Refresh();
_tb.Enabled = false;
//Refresh();
//_mask.Visible = true;
_mask.BringToFront();
_mask.Refresh();
}
*/

        }
        //}}}
        // OnMouseDown {{{
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            mouseDown = true;
            if(!_locked && (UI_state == Settings.STATE_LAYOUT))
            {
                if(dragging) return; // already on it
                //_tb.Enabled = false;

                // Grid too small for POSITIONING {{{
                if(_type == TYPE_SHORTCUT)
                {
                    if(Settings.scaledGridSize < TabsCollection.TAB_GRID_MIN)
                    {
                        MainFormInstance.warn_Announce("Grid is too small for POSITIONING!\n.. "+ TabsCollection.TAB_GRID_MIN +"x"+ TabsCollection.TAB_GRID_MIN +" minimum gridsize required");
                        return;
                    }
                }

                //}}}
                // dragPoint  {{{

                if( isSelected() )
                    TabsCollection.Selection_grid_layout( true );
                else 
                    grid_layout( true );

                set_dragPoint(e);

                //}}}
                update_grip( e );

                moving      = (!resize_T && !resize_B && !resize_L && !resize_R);
                dragging    = true;
                resized     = false;

            }
            //_tb.Enabled = true;
            Refresh();
        }
         //}}}
        // set_dragPoint {{{
        private void set_dragPoint(MouseEventArgs e)
        {
            dragPoint       = PointToScreen(e.Location);
            origin.X        = Location.X;
            origin.Y        = Location.Y;
            origin.Width    = Size.Width;
            origin.Height   = Size.Height;
        }
        //}}}
        // OnMouseMove {{{
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if(_locked && (UI_state != Settings.STATE_LAYOUT))
                return;
            // DRAG {{{
            if(dragging) {
                // dx dy w h {{{

                Point p = PointToScreen(e.Location);

                int                         dx = p.X - dragPoint.X;
                dx = gridSize * (int)Math.Floor(((double)dx/(double)gridSize) + 0.5);

                int                         w  = Size  .Width;
                if(     resize_R)           w  = origin.Width  + dx;
                else if(resize_L)           w  = origin.Width  - dx;
                if(w <     MinimumSize.Width) {
                    dx -= (MinimumSize.Width - w);
                    w   =  MinimumSize.Width;
                }

                int                         dy = p.Y - dragPoint.Y;
                dy = gridSize * (int)Math.Floor(((double)dy/(double)gridSize) + 0.5);

                int                         h  = Size  .Height;
                if(     resize_B)           h  = origin.Height + dy;
                else if(resize_T)           h  = origin.Height - dy;
                if(h <     MinimumSize.Height) {
                    dy -= (MinimumSize.Height - h);
                    h   =  MinimumSize.Height;
                }

                int new_Top     = origin.Y+dy;
                int new_Left    = origin.X+dx;

                //}}}

                bool alt_down   = ((Form.ModifierKeys & Keys.Alt    ) != 0);
                bool ctr_down   = ((Form.ModifierKeys & Keys.Control) != 0);

                // FORCE SQUARE SHAPES
                if(ctr_down && ((dx!=0) || (dy!=0))) {
                    if(w > h) h = w;
                    else      w = h;
                }

                // 1/2 SINGLE ITEM {{{
                if(!_tb.Selected) {
                    // TOP-LEFT MOVING
                    if(moving || resize_T) this.Top  = new_Top ;
                    if(moving || resize_L) this.Left = new_Left;

                    // WIDTH-HEIGHT RESIZING
                    this.Width                       = w;
                    this.Height                      = h;
                }
                // }}}
                // 2/2 SELECTION GEOMETRY (...this item being part of) {{{
                else {
                    // TOP LEFT {{{
                    if(! alt_down )
                    {
                        if(moving || resize_T)
                            TabsCollection.Change_selection_Y(this, new_Top  - this.Top );
                        if(moving || resize_L)
                            TabsCollection.Change_selection_X(this, new_Left - this.Left);
                    }

                    //}}}
                    if(alt_down) {
                        Point m = new Point(e.X - Width/2, e.Y - Height/2);
                        Point o = PointToScreen(  Location);
                        // NWSE {{{
                        int w1 = Width /3; int w2 = 2 * Width /3;
                        int h1 = Height/3; int h2 = 2 * Height/3;

                        bool m_N  = (e.X > w1) && (e.Y < h1) && (e.X < w2);
                        bool m_NW = (e.X < w1) && (e.Y < h1)              ;
                        bool m_NE =               (e.Y < h1) && (e.X > w2);

                        bool m_S  = (e.X > w1) && (e.Y > h2) && (e.X < w2);
                        bool m_SW = (e.X < w1) && (e.Y > h2)              ;
                        bool m_SE =               (e.Y > h2) && (e.X > w2);

                        bool m_W  = (e.X < w1) && (e.Y > h1) && (e.Y < h2);
                        bool m_E  = (e.X > w2) && (e.Y > h1) && (e.Y < h2);

                        //}}}
                        // to NWSE {{{
                        bool to_N = (dy < 0) && m_NW || m_NE;
                        bool to_S = (dy > 0) && m_SW || m_SE;

                        bool to_W = (dx < 0) && m_NE || m_NW;
                        bool to_E = (dx > 0) && m_SE || m_SW;

                        //}}}
                        // OFF TLBR {{{
                        bool off_L = (e.X < 0);
                        bool off_R = (e.X > Width);
                        bool off_T = (e.Y < 0);
                        bool off_B = (e.Y > Height);
                        bool off   = (off_T || off_L || off_B || off_R);
                        //}}}
                        if( off ) {

                            // CHAIN {{{
                            if     (off_T && !off_R && (m_NW || m_NE)) {
                                TabsCollection                .Change_selection_Chain_Up     (this);
                                if     (off_L)  TabsCollection.Change_selection_Chain_Left   (this);
                                else if(m_NW )  TabsCollection.Change_selection_Align_Left   (this);
                                else if(m_NE )  TabsCollection.Change_selection_Align_Right  (this);
                            }
                            else if(off_R && !off_B && (m_NE || m_SE)) {
                                TabsCollection                .Change_selection_Chain_Right  (this);
                                if     (off_T)  TabsCollection.Change_selection_Chain_Up     (this);
                                else if(m_NE )  TabsCollection.Change_selection_Align_Top    (this);
                                else if(m_SE )  TabsCollection.Change_selection_Align_Bottom (this);
                            }
                            else if(off_B && !off_L && (m_SE || m_SW)) {
                                TabsCollection                .Change_selection_Chain_Down   (this);
                                if     (off_R)  TabsCollection.Change_selection_Chain_Right  (this);
                                else if(m_SE )  TabsCollection.Change_selection_Align_Right  (this);
                                else if(m_SW )  TabsCollection.Change_selection_Align_Left   (this);
                            }

                            else if(off_L && !off_T && (m_SW || m_NW)) {
                                TabsCollection                .Change_selection_Chain_Left   (this);
                                if     (off_B)  TabsCollection.Change_selection_Chain_Down   (this);
                                else if(m_SW )  TabsCollection.Change_selection_Align_Bottom (this);
                                else if(m_NW )  TabsCollection.Change_selection_Align_Top    (this);
                            }

                            //}}}
                        }

                        //                            // TEXT logging {{{
                        //                            string inout = off ? "X" : "o";
                        //                            _tb.Text = ""
                        //                                + string.Format("d {0,4} x {1,4}\n", dragPoint.X, dragPoint.Y)
                        //                                + string.Format("p {0,4} x {1,4}\n",         p.X,         p.Y)
                        //                                + string.Format("e {0,4} x {1,4}\n",         e.X,         e.Y)
                        //                                + string.Format("m {0,4} x {1,4}\n",         m.X,         m.Y)
                        //                                +"\n"
                        //                                + "w1=["+ w1 +"] w2=["+ w2 +"] Width=["+ Width  +"]\n"
                        //                                + "h1=["+ h1 +"] h2=["+ h2 +"] Width=["+ Height +"]\n"
                        //                                +"\n"
                        //                                + string.Format("{0,1} {1,1} {2,1}\n", (m_NW ? inout : "-"), (m_N ? inout : "-"), (m_NE ? inout : "-"))
                        //                                + string.Format("{0,1} {1,1} {2,1}\n", (m_W  ? inout : "-"), (              " "), (m_E  ? inout : "-"))
                        //                                + string.Format("{0,1} {1,1} {2,1}\n", (m_SW ? inout : "-"), (m_S ? inout : "-"), (m_SE ? inout : "-"))
                        //                                +"\n"
                        //                                ;
                        //                            //}}}

                    }
                    // WIDTH HEIGHT {{{
                    else if( !moving )
                    {
                        if(this.Width  != w) TabsCollection.Change_selection_Width (this, w);
                        if(this.Height != h) TabsCollection.Change_selection_Height(this, h);
                    }
                    //}}}

                }
                // }}}
            //  resized         = true;
                resized         = (dx!=0) || (dy!=0);
                this.Parent.Invalidate();
            }

            // MODE AND CURSOR
            if(UI_state == Settings.STATE_LAYOUT)
                update_grip( e );
            //}}}
        }
        //}}}
        // update_grip {{{
        private void update_grip(MouseEventArgs e)
        {
            // pick a BORDER or a CORNER to resize .. pick CENTER to MOVE {{{
            if( !dragging )
            {
                int grip_size = (Size.Height < Size.Width) ?  Size.Height / 4 :  Size.Width  / 4;
                if(grip_size > 30) grip_size = 30; // TODO find the right place for this constant

                resize_T = (e.Y <              grip_size );
                resize_B = (e.Y > (Size.Height-grip_size));

                resize_L = (e.X <              grip_size );
                resize_R = (e.X > (Size.Width -grip_size));
            }
            //}}}
/*
            // CHANGE SIDE {{{
            bool alt_down   = ((Form.ModifierKeys & Keys.Alt    ) != 0);
            if(alt_down)
            {
                Point o = PointToScreen(  Location);
                Point p = PointToScreen(e.Location);

                //bool  drag_origin_changed = false;
            //  if(p.Y > (o.Y + Height/2)) { resize_T = false; resize_B = true; }//drag_origin_changed = true; }
            //  if(p.Y < (o.Y + Height/2)) { resize_B = false; resize_T = true; }//drag_origin_changed = true; }
            //  if(p.X > (o.X + Width /2)) { resize_L = false; resize_R = true; }//drag_origin_changed = true; }
            //  if(p.X < (o.X + Width /2)) { resize_R = false; resize_L = true; }//drag_origin_changed = true; }
                //if(drag_origin_changed) set_dragPoint(e);

                bool off_L = (p.X < (o.X       ));
                bool off_R = (p.X > (o.X+Width ));
                bool off_T = (p.Y < (o.Y       ));
                bool off_B = (p.Y > (o.Y+Height));


                resize_T = (!off_L && !off_R) && (p.Y < (o.Y + Height/2));
                resize_B = (!off_L && !off_R) && (p.Y > (o.Y + Height/2));

                resize_L = (!off_T && !off_B) && (p.X < (o.X + Width /2));
                resize_R = (!off_T && !off_B) && (p.X > (o.X + Width /2));

            }
            //}}}
*/
            // CURSOR {{{
            if         (resize_T) {
                if     (resize_L)           Cursor.Current = Cursors.PanNW;
                else if(resize_R)           Cursor.Current = Cursors.PanNE;
                else                        Cursor.Current = Cursors.PanNorth;
            }
            else if(    resize_B) {
                if     (resize_L)           Cursor.Current = Cursors.PanSW;
                else if(resize_R)           Cursor.Current = Cursors.PanSE;
                else                        Cursor.Current = Cursors.PanSouth;
            }
            else if(resize_L            )   Cursor.Current = Cursors.PanWest;
            else if(            resize_R)   Cursor.Current = Cursors.PanEast;
            else                            Cursor.Current = Cursors.SizeAll;

            //}}}
        }
        //}}}
        // grid_layout {{{
        public void grid_layout(bool start_dragging)
        {
/*
/Left\|Top\|Width\|Height\|MinimumSize
*/
            // ABOUT TO BE MOVED OR RESIZED .. grid-alignment & MinimumSize {{{
            if(start_dragging) {
                if(toolTip != null) toolTip.Active = false;

                // remember current position where we start moving from
                xy_wh_to_move_back_to = _xy_wh;

                // get scaled grid size
                gridSize = TabsCollection.TAB_GRID_S;
                if((this.Type == TYPE_SHORTCUT) || (this.Type == TYPE_RICHTEXT))
                    gridSize = Settings.scaledGridSize;

                Left = gridSize * (int)Math.Floor(((double)Left/(double)gridSize) + 0.5);
                Top  = gridSize * (int)Math.Floor(((double)Top /(double)gridSize) + 0.5);

                // update scaled minimum grid size
                if((Type == NotePane.TYPE_PANEL) || (Type == NotePane.TYPE_DASH))
                    MinimumSize = new Size(TabsCollection.TAB_MIN_TXT_W * gridSize, TabsCollection.TAB_MIN_TXT_H * gridSize);
                else
                    MinimumSize = new Size(TabsCollection.TAB_MIN_BTN_W * gridSize, TabsCollection.TAB_MIN_BTN_H * gridSize);

                //Logger.Log("EVENTS", "gridSize=["+ gridSize +"]");
/* // Announce {{{
            MainForm.Announce.Add("NotePane"
                , MainFormInstance
                , "gridSize="+ gridSize
                , 0
                , 0
                , MainForm.Announce.DEFAULT_ANNOUNCE_ERASE_DELAY
                );
*/ // }}}
            }
            //}}}
            // HAS JUST BEEN MOVED OR RESIZED .. [commit xy_wh] {{{
            else {
                if(toolTip != null) toolTip.Active = true;

                int x      = Left;
                int y      = Top;
                int w      = Width;
                int h      = Height;

                if(x < 0) x = 0;
                if(y < 0) y = 0;

                if(w < MinimumSize.Width ) w = MinimumSize.Width;
                if(h < MinimumSize.Height) h = MinimumSize.Height;

                MinimumSize = new Size(1,1);    // used only here, while dragging

                double dots_per_grid_cell
                    = ((this.Type == NotePane.TYPE_SHORTCUT) || (this.Type == NotePane.TYPE_RICHTEXT))
                    ?  TabsCollection.TAB_GRID_S * Settings.ratio
                    :  TabsCollection.TAB_GRID_S;

                x    = (int)Math.Floor(0.5 + ((double)x / dots_per_grid_cell));
                y    = (int)Math.Floor(0.5 + ((double)y / dots_per_grid_cell));
                w    = (int)Math.Floor(0.5 + ((double)w / dots_per_grid_cell));
                h    = (int)Math.Floor(0.5 + ((double)h / dots_per_grid_cell));

                _xy_wh  = String.Format("{0},{1},{2},{3}", x, y, w, h);
            }
            //}}}
        }
        //}}}

        // OnMouseEnter {{{
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            mouseInside = true;

            // announce priority: control.tt .. layout/edit .. np.tt
            if((Type == TYPE_CONTROL) && (_tt != ""))
                MainFormInstance.info_Announce(_tt);
            else if((UI_state == Settings.STATE_LAYOUT) || (UI_state == Settings.STATE_EDIT))
                MainFormInstance.sel_Announce();
            else if(_tt != "")
                MainFormInstance.info_Announce(_tt);

/*
            if( mouseInside )
            {
                if(    (UI_state == Settings.STATE_EDIT  )
                    || (UI_state == Settings.STATE_LAYOUT)
                ) {
                    _tb_enable( false );
                    Cursor.Current = Cursors.Arrow; //Hand;
                }
                this.Refresh();
            }
*/
        }
        //}}}
        // OnMouseLeave {{{
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            bool state = (ClientRectangle.Contains(PointToClient(Control.MousePosition)));
            if(mouseInside != state) {
                mouseInside = state; // this.Refresh();

                // announce priority: control.tt .. layout/edit .. np.tt
                if((UI_state == Settings.STATE_LAYOUT) || (UI_state == Settings.STATE_EDIT))
                    MainFormInstance.sel_Announce();
/*
                if(    !mouseInside
                    && (UI_state == Settings.STATE_EDIT)
                  ) {
                    if(!this._tb.Enabled) {
                        _tb_enable( _locked );
                        Cursor.Current = Cursors.Arrow;
                    }
                }
*/
            }
        }
        //}}}
/*
        // is_a_ui_state_control {{{
        private bool is_a_ui_state_control()
        {
            return (UI_state == Settings.STATE_LAYOUT)
                && (   (Name == CONTROL_NAME_ACTIVATE      )
                    || (Name == CONTROL_NAME_ADD           )
                    || (Name == CONTROL_NAME_EDIT          )
                    || (Name == CONTROL_NAME_LAYOUT        )
                    || (Name == CONTROL_NAME_UPDATE_PROFILE)
                   );
        }
        //}}}
*/
        // OnMouseClick {{{
        protected override void OnMouseClick(MouseEventArgs e)
        {
            // DRAGGING DELTA {{{
            int delta = 0;
            if(dragging) {
                Point p = PointToScreen(e.Location);
                delta = Math.Abs((p.X - dragPoint.X) + (p.Y - dragPoint.Y));

            }
            if(delta > 5)
                return;
            //}}}
            if(logger != null) {
            //log("OnMouseClick");
                // EDIT CLICKED TAG {{{
                if(UI_state == Settings.STATE_EDIT) {
                    logger.notify(this, "OnMouseClick");

                    return;
                }
                //}}}
                // LAYOUT CLICKED TAG {{{
                else if((UI_state == Settings.STATE_LAYOUT) || (UI_state == Settings.STATE_EDIT))
                {
                    // CONTROL TYPE
                    if(    (Name == CONTROL_NAME_ACTIVATE      )
                        || (Name == CONTROL_NAME_ADD           )
                        || (Name == CONTROL_NAME_EDIT          )
                        || (Name == CONTROL_NAME_LAYOUT        )
                        || (Name == CONTROL_NAME_UPDATE_PROFILE)
                      ) {
                        logger_callback("OnMouseClick");
                    }

                    // OTHER TYPE
                    else {
                        if     (e.Button == MouseButtons.Left ) logger.notify(this, "OnMouseClick"      );
                        else if(e.Button == MouseButtons.Right) logger.notify(this, "OnMouseClick.Right");
                    }

                    return;
                }
                //}}}
                // CONTROL RIGHT CLICK {{{
                else if(UI_state == 0)
                {
                    // CONTROL TYPE
                    if(Type == TYPE_CONTROL)
                    {
                        if     (e.Button == MouseButtons.Left ) logger_callback("OnMouseClick"      );
                        else if(e.Button == MouseButtons.Right) logger_callback("OnMouseClick.Right");

                        return;
                    }
                }
                //}}}
            }

            // right click - EDIT RTF {{{
            if(delta < 5) {
                if(e.Button == MouseButtons.Right) {
                    show_or_eval_RTF();
                    Refresh();
                }
            }
            //}}}
        }
        //}}}
        // OnClick {{{
/*
        protected override void OnClick(EventArgs e)
        {
            if(!dragging) {
                base.OnClick(e);
                logger_callback("OnClick");
            }
        }
*/
        //}}}
        // OnResize {{{
        protected override void OnResize(EventArgs e)
        {
            //base.Invalidate();
            base.OnResize(e);
        }
        //}}}
        // OnPaint {{{
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if(this._color != "") return; // Use Padding instead of borders

            // skip it all when shadow images are missing {{{
            if(ShadowDownRight == null)
                return;

            //}}}

            // shadow geometry {{{
            int                     shadowSize    = 3; int  shadowMargin  = 2;
            /*
               if     (!Enabled    ) { shadowSize    = 5;      shadowMargin  = 2; }
               else
             */
            bool this_is_a_button = is_a_control() || is_a_shortcut();

            if( this_is_a_button ) {
                if(mouseInside)   {
                    shadowSize                    = 5;      shadowMargin  = 2;
                    if(mouseDown) { shadowSize    = 3;      shadowMargin  = 2; }
                }
            }

            //log("...OnPaint mouseInside=["+ mouseInside +"] mouseDown["+ mouseDown +"] ... ["+ shadowSize +"."+ shadowMargin +"]");
            // }}}

            // Create tiled brushes for the shadow on the right and at the bottom.
            TextureBrush shadowRightBrush = new TextureBrush(ShadowRight, WrapMode.Tile);
            TextureBrush shadowDownBrush  = new TextureBrush(ShadowDown , WrapMode.Tile);

            // Translate (move) the brushes so the top or left of the image matches the top or left of the
            // area where it's drawed. If you don't understand why this is necessary, comment it out.
            // Hint: The tiling would start at 0,0 of the control, so the shadows will be offset a little.
            shadowDownBrush .TranslateTransform(               0, Height-shadowSize);
            shadowRightBrush.TranslateTransform(Width-shadowSize,                 0);

            // Define the rectangles that will be filled with the brush.
            // (where the shadow is drawn)
            Rectangle ShadowDownRectangle = new Rectangle(
                shadowSize + shadowMargin,                      // X
                Height     -  shadowSize,                       // Y
                Width      - (shadowSize * 2 + shadowMargin),   // width (stretches)
                shadowSize                                      // height
                );

            Rectangle ShadowRightRectangle = new Rectangle(
                Width      -  shadowSize,                       // X
                shadowSize +  shadowMargin,                     // Y
                shadowSize,                                     // width
                Height     - (shadowSize * 2 + shadowMargin)    // height (stretches)
                );

            // And draw the shadow on the right and at the bottom.
            Graphics g = e.Graphics;
            g.FillRectangle(shadowDownBrush , ShadowDownRectangle);
            g.FillRectangle(shadowRightBrush, ShadowRightRectangle);

            // Now for the corners, draw the 3 5x5 pixel images.
            g.DrawImage(ShadowTopRight , new Rectangle(Width - shadowSize, shadowMargin       , shadowSize, shadowSize));
            g.DrawImage(ShadowDownRight, new Rectangle(Width - shadowSize, Height - shadowSize, shadowSize, shadowSize));
            g.DrawImage(ShadowDownLeft , new Rectangle(shadowMargin      , Height - shadowSize, shadowSize, shadowSize));

            // shadow geometry {{{

            // Fill the area inside with the color in the BackColor property.
            // 1 pixel is added to everything to make the rectangle smaller.
            // This is because the 1 pixel border is actually drawn outside the rectangle.

            // INNER-RECTANGLE  {{{
            Color fColor = BackColor;
            Color bColor = BorderColor;
            int      x   = 0;
            int      y   = 0;

            //}}}
            // MOUSE DOWN BUTTON SHADOW
            if(mouseDown) {
                if(this_is_a_button) {
                    x           += 1;
                    y           += 1;
                    shadowSize  += 2;
                }
            }
            //}}}
            // paint inner rectangle {{{
            if((fColor != null) || (bColor != null))
            {
                Rectangle fullRectangle = new Rectangle(
                    x,
                    y,
                    Width  - shadowSize,
                    Height - shadowSize
                    );

                if(fColor != null) { SolidBrush bgBrush   = new SolidBrush(fColor); g.FillRectangle(bgBrush  , fullRectangle); }
                if(bColor != null) { Pen        borderPen = new        Pen(bColor); g.DrawRectangle(borderPen, fullRectangle); }

            }
            //}}}

            shadowDownBrush .Dispose(); shadowDownBrush  = null;
            shadowRightBrush.Dispose(); shadowRightBrush = null;

            if(_tb != null)
            {
                if( _tb.Enabled) {
                    _tb.Visible = true;
                    _tb.Refresh();
                }
/*
                if(UI_state == Settings.STATE_LAYOUT) && (Name == CONTROL_NAME_LAYOUT)
                {
                    _tb.Visible = false;
                    g.DrawString(DONE_LABEL, DONE_FONT, Brushes.Black, 2, 2);
                    g.DrawString(DONE_LABEL, DONE_FONT, Brushes.Red  , 0, 0);
                }
*/
            }
        }
        //}}}

         //}}}
        // LOG {{{
        private static void log(string msg)// {{{
        {
            //Logger.Log(typeof(NotePane).Name, msg+"\n");
            MessageBox.Show(typeof(NotePane).Name+":\n"
                + msg
                , "NotePane"
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information
                );
/*
*/
        }
        // }}}
        public static string get_BOM() //{{{
        {
            return "Palettes x"+ ColorPaletteDict.Count;
        }
        //}}}
        private void logger_callback(string detail) //{{{
        {
            if(logger == null)
                return;

            // control-related-to-current-UI_state callback
            if(     (                                          UI_state == 0                          )

                ||  ((Name == CONTROL_NAME_ACTIVATE      ) && (UI_state == Settings.STATE_LAYOUT     ))
                ||  ((Name == CONTROL_NAME_ADD           ) && (UI_state == Settings.STATE_LAYOUT     ))

                ||  ((Name == CONTROL_NAME_EDIT          ) && (UI_state == Settings.STATE_EDIT       ))
                ||  ((Name == CONTROL_NAME_EDIT          ) && (UI_state == Settings.STATE_LAYOUT     ))
                ||  ((Name == CONTROL_NAME_LAYOUT        ) && (UI_state == Settings.STATE_EDIT       ))
                ||  ((Name == CONTROL_NAME_LAYOUT        ) && (UI_state == Settings.STATE_LAYOUT     ))

                ||  ((Name == CONTROL_NAME_UPDATE_PROFILE) && (UI_state == Settings.STATE_LAYOUT     ))

                ||  ((Name == CONTROL_NAME_SETTINGS      ) && (UI_state == Settings.STATE_SETTINGS   ))

                ||  (Name.StartsWith(PANEL_NAME_PROFILE  ) && (UI_state == Settings.STATE_PROFILE    ))
                ||  ((Name == CONTROL_NAME_PROFILE       ) && (UI_state == Settings.STATE_PROFILE    ))

                ||  ((Name == CONTROL_NAME_EXPORT        ) && (UI_state == Settings.STATE_EXPORT     ))
                ||  ((Name == CONTROL_NAME_EXPORT_PROFILE) && (UI_state == Settings.STATE_EXPORT     ))
                ||  ((Name == CONTROL_NAME_EXPORT_TO_FILE) && (UI_state == Settings.STATE_EXPORT     ))
                ||  ((Name == CONTROL_NAME_EXPORT_CLIPBRD) && (UI_state == Settings.STATE_EXPORT     ))
                ||  ((Name == CONTROL_NAME_UPDATE_PROFILE) && (UI_state == Settings.STATE_EXPORT     ))

                ||  ((Name == CONTROL_NAME_IMPORT        ) && (UI_state == Settings.STATE_IMPORT     ))
                ||  ((Name == CONTROL_NAME_IMPORT_INSERT ) && (UI_state == Settings.STATE_IMPORT     ))
                ||  ((Name == CONTROL_NAME_IMPORT_OVERLAY) && (UI_state == Settings.STATE_IMPORT     ))

              )
                logger.callback(this, detail);
        }
        //}}}
        //}}}

    }
}

/* // Notes {{{
/\<Left\>\|\<Top\>\|\<Width\>\|\<Height\>
*/ //}}}

