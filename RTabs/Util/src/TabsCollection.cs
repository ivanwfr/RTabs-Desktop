// using {{{
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Forms;

// }}}
/** COMMENTS {{{
:new /LOCAL/DATA/ANDROID/PROJECTS/RTabs/app/src/main/java/ivanwfr/rtabs/RTabsClient.java

/name\|type\|tag\|zoom\|xy_wh\|text\|rtf

  0 type
  1 tag
  2 zoom
  3 xy_wh
  4 text

TAB.Name = [type=___][tag=___][zoom=___][xy_wh=___][text=___]

" OBSOLTE SUBKEYS:
/\(name\|type\|tag\|zoom\|xy_wh\|text\|rtf\)"="

 */
// }}}
namespace Util
{
    public class TabsCollection
    {
        // USAGE {{{
        public const string TAB_USAGE_TEXT
            = "TABS DEFINITION-LINE SYNTAX:\n"
            + "\n"
            + "Four separators are used: = | . ,\n"
            + "\n"
            + "A tab definition line looks like this:\n"
            + "\n"
            + "[APP_NAME] TAB.name = type=___|tag=___|zoom=___|xy_wh=_,_,_,_|text=___|color=_|shape=___|tt=___\n"
            + "...\n"
            + "[APP_NAME] is ignored and optional (...reserved usage)\n"
            + "TAB.  ... (in TAB.name) i.e. \"this is a TAB kind of line (i.e. not a PALETTE)\"\n"
            + "name  ... tab identifier\n"
            + "type  ... one of: PANEL, DASH, CONTROL RICHTEXT or SHORTCUT\n"
            + "zoom  ... text size (1 is the default)\n"
            + "xy_wh ... location and size\n"
            + "text  ... to be displayed\n"
            + "tag   ... command (sent to .. executed by) the server (PC side)\n"
            + "color ... optional reserved palette color index (stands-out from other tabs)\n"
            + "shape ... [circle]\n"
            + "tt    ... tooltip (shown by the Designer, not yet in android apk)\n"
            ;

        // }}}
        // GEOMETRY CONSTANTS {{{
        public static int   TAB_GRID_MIN    =  8;
        public static int   TAB_GRID_S      = 20;
        public static int   TAB_MIN_TXT_W   = 10;
        public static int   TAB_MIN_TXT_H   =  5;
        public static int   TAB_MIN_BTN_W   =  3;
        public static int   TAB_MIN_BTN_H   =  1;

        // }}}
        // CLASS {{{
        private  static RTabs.MainForm MainFormInstance    = null;

        //}}}
        // INSTANCE {{{
        public         Dictionary<string, Object> tabs_Dictionary = new Dictionary<string, Object>();
        private        Dictionary<string, Object> del_Dictionary  = new Dictionary<string, Object>();
        private        Stack<NotePane>            pool_Stack      = new Stack<NotePane>();

        private static SortedSet<NotePane>        Sel_SortedSet   = new SortedSet<NotePane>();
        private static HashSet<NotePane>          Copy_HashSet    = new HashSet<NotePane>();

        private Panel           tabs_container;
        private Panel           controls_container;
        private Panel           panels_container;
        private LoggerInterface logger;
        private StringBuilder   sb = new StringBuilder();

        //}}}
        // CONSTRUCTORS {{{
        public TabsCollection(Form mainform, Panel tabs_container)
            : this(mainform, tabs_container, tabs_container, tabs_container)
        {
        }

        public TabsCollection(Form mainform, Panel tabs_container, Panel controls_container, Panel panels_container)
        {
            MainFormInstance        = (RTabs.MainForm)mainform;
            this.    tabs_container =     tabs_container;
            this.controls_container = controls_container;
            this.  panels_container =   panels_container;
        }

        public void set_logger(LoggerInterface logger)
        {
            this.logger = logger;
        }

        // }}}
        // DISPLAY - - - - - - - - - - - - - - - - - - - - - - - - - - - - {{{
        // hide_all_tabs {{{
        public void hide_all_tabs()
        {
            foreach(var item in tabs_Dictionary) {
                NotePane np = get_tab_NP( item.Key );
                np.Visible    = false;
            }
            tabs_container.Refresh();
        }
        // }}}
        public void show_UI_state_targets_only(uint ui_state) // show relevant controls only {{{
        {
            show_UI_state_targets_only(ui_state, false, false); // not-disconnected, no-controls
        }
        // }}}
        public void show_UI_state_targets_only_withControls(uint ui_state) // show relevant controls only {{{
        {
            show_UI_state_targets_only(ui_state, false, true); // not-disconnected, with-controls
        }
        // }}}
        public void show_UI_state_targets_only_disconnected(uint ui_state) // show relevant controls only {{{
        {
            show_UI_state_targets_only(ui_state, true, false); // disconnected, no-controls
        }
        // }}}
        public void show_UI_state_targets_only(uint ui_state, bool disconnected, bool withControls) // can edit user-added tabs only {{{
        {
            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                // STATELESS {{{
                if(ui_state == 0)
                {
                    // Show all but transient tabs
                    if(    (np.Name  != Settings.APP_TITLE)
                        && (np.Name  != NotePane.PANEL_NAME_PALETTES)
                        && (np.Name  != NotePane.PANEL_NAME_XPORT   )
                      )
                        np.Visible    = true;

                    // hide utility panels (not handled elsewhere)
                    if(    !Settings.LOGGING
                        && (np.Name.StartsWith(NotePane.PANEL_NAME_HEADER))
                        && (np.Type == NotePane.TYPE_PANEL)
                        && (np.Name  != NotePane.PANEL_NAME_PALETTES)
                        && (np.Name  != NotePane.PANEL_NAME_XPORT)
                      )
                        np.Visible    = false;

                    // hide everything when not connected .. (i.e. working on connection settings IP, PORT, PASSWORD)
                    if(    disconnected
                        && (np.Type ==        NotePane.TYPE_CONTROL)
                        && (np.Name  !=        NotePane.CONTROL_NAME_START)
                        // (np.Name  !=        NotePane.CONTROL_NAME_STOP )
                      )
                        np.Visible    = false;

                    // hide TRANSITORY CONTROLS .. (i.e. only relevant in some sub-mode)
                    if(    (np.Name  ==        NotePane.CONTROL_NAME_ADD           )    // LAYOUT ACTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_DEL           )    // LAYOUT ACTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_ACTIVATE      )    // LAYOUT ACTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_UPDATE_PROFILE)    // LAYOUT ACTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR0        )    // LAYOUT SELECTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR1        )    // LAYOUT SELECTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR2        )    // LAYOUT SELECTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR3        )    // LAYOUT SELECTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR4        )    // LAYOUT SELECTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR5        )    // LAYOUT SELECTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR6        )    // LAYOUT SELECTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR7        )    // LAYOUT SELECTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR8        )    // LAYOUT SELECTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR9        )    // LAYOUT SELECTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR10       )    // LAYOUT SELECTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_COLOR11       )    // LAYOUT SELECTION

                        || (np.Name  ==        NotePane.CONTROL_NAME_EXPORT_CLIPBRD)    // EXPORT ACTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_EXPORT_PROFILE)    // EXPORT ACTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_EXPORT_TO_FILE)    // EXPORT ACTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_IMPORT_INSERT )    // IMPORT ACTION
                        || (np.Name  ==        NotePane.CONTROL_NAME_IMPORT_OVERLAY)    // IMPORT ACTION
                      )
                        np.Visible    = false;

                }
                // }}}
                // STATE_LAYOUT .. (shows everything) {{{
                else if(ui_state == Settings.STATE_LAYOUT)
                {
                    if(     withControls
                        &&  (np.Name  != NotePane.CONTROL_NAME_ACTIVATE      )
                        &&  (np.Name  != NotePane.CONTROL_NAME_ADD           )
                        &&  (np.Name  != NotePane.CONTROL_NAME_DEL           )
                        &&  (np.Name  != NotePane.CONTROL_NAME_UPDATE_PROFILE)
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR0        )
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR1        )
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR2        )
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR3        )
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR4        )
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR5        )
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR6        )
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR7        )
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR8        )
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR9        )
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR10       )
                        &&  (np.Name  != NotePane.CONTROL_NAME_COLOR11       )
                      ) {
                        np.Visible    = true;
                    }
                    else {
                        if(     (np.Name == NotePane.CONTROL_NAME_LAYOUT        )
                            ||  (np.Name == NotePane.CONTROL_NAME_ACTIVATE      )
                            ||  (np.Name == NotePane.CONTROL_NAME_ADD           )
                            ||  (np.Name == NotePane.CONTROL_NAME_DEL           )
                            ||  (np.Name == NotePane.CONTROL_NAME_EDIT          )   // EDIT-LAYOUT TOGGLE
                            ||  (np.Name == NotePane.CONTROL_NAME_UPDATE_PROFILE)
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR0        )
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR1        )
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR2        )
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR3        )
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR4        )
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR5        )
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR6        )
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR7        )
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR8        )
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR9        )
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR10       )
                            ||  (np.Name == NotePane.CONTROL_NAME_COLOR11       )
                            ||  (np.Type == NotePane.TYPE_RICHTEXT)
                            ||  (np.Type == NotePane.TYPE_SHORTCUT)
                            || ((np.Type == NotePane.TYPE_DASH    ) && (np.Parent == tabs_container))
                          )
                            np.Visible      =  true;

                        else
                            np.Visible      =  false;
                    }

                }
                // }}}
                // STATE_EDIT {{{
                else if(ui_state == Settings.STATE_EDIT)
                {
                    if(    (np.Name == NotePane.CONTROL_NAME_EDIT  )
                        || (np.Name == NotePane.CONTROL_NAME_LAYOUT)        // EDIT-LAYOUT TOGGLE
                        || (np.Name == NotePane.CONTROL_NAME_UPDATE_PROFILE)
                        || (np.Type == NotePane.TYPE_RICHTEXT)
                        || (np.Type == NotePane.TYPE_SHORTCUT)
                        || (np.Type == NotePane.TYPE_DASH)
                        ||((np.Type == NotePane.TYPE_PANEL) && Settings.LOGGING)
                      ) {
                        np.Visible      =  true;
                    }
                    else {
                        np.Visible      = false;
                    }
                }
                // }}}
                // STATE_IMPORT {{{
                else if( (ui_state == Settings.STATE_IMPORT)
                    )
                {
                    if(    (np.Name == NotePane.CONTROL_NAME_IMPORT)
                        || (np.Name == NotePane.CONTROL_NAME_IMPORT_INSERT)
                        || (np.Name == NotePane.CONTROL_NAME_IMPORT_OVERLAY)
                        || (np.Name == NotePane.PANEL_NAME_XPORT)
                        || (np.Type == NotePane.TYPE_RICHTEXT)
                        || (np.Type == NotePane.TYPE_SHORTCUT)
                        ||((np.Type == NotePane.TYPE_PANEL) && Settings.LOGGING)
                        || (np.Type == NotePane.TYPE_DASH)
                      ) {
                        np.Visible      =  true;
                    }
                    else {
                        np.Visible      = false;
                    }
                }
                // }}}
                // STATE_EXPORT {{{
                else if( (ui_state == Settings.STATE_EXPORT)
                    )
                {
                    if(    (np.Name == NotePane.CONTROL_NAME_EXPORT)
                        || (np.Name == NotePane.CONTROL_NAME_EXPORT_PROFILE)
                        || (np.Name == NotePane.CONTROL_NAME_EXPORT_TO_FILE)
                        || (np.Name == NotePane.CONTROL_NAME_EXPORT_CLIPBRD)
                        || (np.Name == NotePane.PANEL_NAME_XPORT)
                        || (np.Type == NotePane.TYPE_RICHTEXT)
                        || (np.Type == NotePane.TYPE_SHORTCUT)
                        ||((np.Type == NotePane.TYPE_PANEL) && Settings.LOGGING)
                        || (np.Type == NotePane.TYPE_DASH)
                      ) {
                        np.Visible      =  true;
                    }
                    else {
                        np.Visible      = false;
                    }
                }
                // }}}
                // STATE_PROFILE {{{
                else if(ui_state == Settings.STATE_PROFILE)
                {
                    if(   ((np.Type == NotePane.TYPE_CONTROL ) && (np.Name         == NotePane.CONTROL_NAME_PROFILE ))
                        ||((np.Type == NotePane.TYPE_SHORTCUT) && (np.Name.StartsWith(NotePane.  PANEL_NAME_PROFILE)))
                        ||((np.Type == NotePane.TYPE_RICHTEXT) && (np.Name.StartsWith(NotePane.  PANEL_NAME_PROFILE)))
                        ||((np.Type == NotePane.TYPE_PANEL   ) && Settings.LOGGING)
                        ||((np.Type == NotePane.TYPE_DASH    ) && (np.Parent != tabs_container))
                      ) {
                        np.Visible      =  true;
                    }
                    else {
                        np.Visible      = false;
                    }
                }
                // }}}
                // STATE_SETTINGS {{{
                else if(ui_state == Settings.STATE_SETTINGS)
                {
                    if(    (np.Name  == NotePane.CONTROL_NAME_SETTINGS)
                        ||((np.Type == NotePane.TYPE_PANEL) && Settings.LOGGING)
                        || (np.Type == NotePane.TYPE_DASH)
                      ) {
                        np.Visible      =  true;
                    }
                    else {
                        np.Visible      = false;
                    }
                }
                // }}}
            }
        }
        // }}}
        public void palette_Changed() // {{{
        {
            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                if(np.color == "")  np.pickNextPaletteColor();
                else                np.reserve_color();
            }
        }
        // }}}
        public void dev_dpi_Changed() // {{{
        {
            ResetAutoPlace();
            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                if((np.Type == NotePane.TYPE_SHORTCUT) || (np.Type == NotePane.TYPE_RICHTEXT))
                    np.dev_dpi_Changed();
            }
        }
        // }}}
        public void change_zoom(int offset) // {{{
        {
            float ratio = (offset > 0) ? 1.1F : 0.9F;

            ResetAutoPlace();
            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                if((np.Type == NotePane.TYPE_SHORTCUT) || (np.Type == NotePane.TYPE_RICHTEXT))
                {
                    float factor = np.TextBox.ZoomFactor * ratio;
                    if((factor > 1.0F/64) && (factor < 64.0F)) {
                        np.TextBox.ZoomFactor = 1.0f;
                        np.TextBox.ZoomFactor = factor;
                    }
                }
            }
            tabs_container.Invalidate();
        }
        // }}}
        public void adjust_zoom(double mon_dev_zoom_ratio) // {{{
        {
            ResetAutoPlace();
            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                if((np.Type == NotePane.TYPE_SHORTCUT) || (np.Type == NotePane.TYPE_RICHTEXT))
                {
                    float factor = np.TextBox.ZoomFactor * (float)mon_dev_zoom_ratio;
                    if((factor > 1.0F/64) && (factor < 64.0F)) {
                        np.TextBox.ZoomFactor = 1.0f;
                        np.TextBox.ZoomFactor = factor;
                    }
                }
            }
            tabs_container.Invalidate();
        }
        // }}}
        public void set_zoom(double zoomRatio) // {{{
        {
            ResetAutoPlace();
            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                if((np.Type == NotePane.TYPE_SHORTCUT) || (np.Type == NotePane.TYPE_RICHTEXT))
                {
                    //float current          = np.TextBox.ZoomFactor;
                    //np.TextBox.ZoomFactor  = 1.0f;
                    //np.TextBox.ZoomFactor *= current * (float)zoomRatio;
                    np.dev_dpi_Changed();
                }
            }
            tabs_container.Invalidate();
        }
        // }}}
        public void clear_app_panels_content() // {{{
        {
            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                if(     !np.is_a_control()
                    &&  !np.is_a_shortcut()
                    &&  !(np.Type == NotePane.TYPE_DASH)
                    &&  !(np.Type == NotePane.TYPE_RICHTEXT)
                    &&  !(np.Name == NotePane.PANEL_NAME_PALETTES)
                    &&  !(np.Name == NotePane.PANEL_NAME_LOG)
                    &&  ! np.ReadOnly
                  ) 
                    np._tb.Text = "";

            }
            System.GC.Collect();
        }
        // }}}
        // DISPLAY - - - - - - - - - - - - - - - - - - - - - - - - - - - - }}}
        // LAYOUT  - - - - - - - - - - - - - - - - - - - - - - - - - - - - {{{
        // set_tabs_locked_state - (i.e. not moving or resizing) {{{
        public  void set_tabs_locked_state(bool state)
        {
        
            container_Visible( false );
            container_SuspendLayout();

            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                np.Locked   = state;
            }
            container_ResumeLayout();
            container_Visible( true );
        }
        //}}}
        // move_tab {{{
        public  void move_tab(NotePane np)
        {
            ResetAutoPlace();
            //container_BringToFront( np ); // done already by NotePane before notification
            container_ScrollControlIntoView( np );
            if( np.isSelected() ) {
                foreach(NotePane item in  Sel_SortedSet)
                {
                    layout_tab_on_grid( item );
                    item.update_tooltip();
                }
            }
            else {
                layout_tab_on_grid( np );
                np.update_tooltip();
            }

        }
        //}}}
        // edit_sel_tab {{{
        public  void edit_sel_tab()
        {
            if(Sel_SortedSet.Count > 0)
                edit_tab( Sel_SortedSet.First() );
        }
        //}}}
        // edit_tab {{{
        public  void edit_tab(NotePane np)
        {
            //container_BringToFront( np ); // done already by NotePane before notification
            container_ScrollControlIntoView( np );
            np.edit_tag();
        }
        //}}}
        // layout_all_tabs_on_grid {{{
        public  void layout_all_tabs_on_grid()
        {

            scroll_containers_top_left();

            bool too_small_for_tabs         = (Settings.scaledGridSize < TAB_GRID_MIN);
            bool too_small_for_tabs_warn    = false;
            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );

                if((np.Parent == tabs_container) && too_small_for_tabs)
                    too_small_for_tabs_warn = true;
                else
                    layout_tab_on_grid( np );
            }

            if( too_small_for_tabs_warn )
                MainFormInstance.warn_Announce("Grid is too small for SCALING!");

        }
        //}}}
    // tabs_container_SuspendLayout {{{
    public void tabs_container_SuspendLayout()
    {
        log("@@@ tabs_container_SuspendLayout @@@");
        tabs_container.Visible = false;
        tabs_container.SuspendLayout();
    }
    //}}}
    // tabs_container_ResumeLayout {{{
    public void tabs_container_ResumeLayout()
    {
        log("@@@ tabs_container_ResumeLayout @@@");
        tabs_container.ResumeLayout();
        tabs_container.Visible = true;
    }
    //}}}
        //}}}
        // AutoPlace {{{

        // {{{
        private const  int   AUTOPLACE_ROWS_DEFAULT = 4;
        private const  int   AUTOPLACE_COLS_DEFAULT = 3;
        private static Point AutoPlacePoint         = new Point(0,0);
        private static Point FreePlacePoint         = new Point(0,0);
        private static int   AutoPlace_ROWS         = AUTOPLACE_ROWS_DEFAULT;
        private static int   AutoPlace_COLS         = 0;
        private static  int  AutoPlace_WRAP_X       = 0;
        private static  int  AutoPlace_WRAP_Y       = 0;
        private static  int  AutoPlace_PANELY       = 0;

        //}}}

        // ResetAutoPlace {{{
        public static void ResetAutoPlace()
        {
            ResetAutoPlace_x_y_rows(0, 0, AUTOPLACE_ROWS_DEFAULT);
        }
        //}}}
        // ResetAutoPlace_x_y_rows {{{
        public static void ResetAutoPlace_x_y_rows(int x, int y, int rows)
        {
            //AutoPlace_PANELY = 0; // session wide ?

            AutoPlace_WRAP_X = x;
            AutoPlace_WRAP_Y = y;
            AutoPlacePoint.X = x;
            AutoPlacePoint.Y = y;

            AutoPlace_ROWS   = (rows>=1) ? rows : 1;
            AutoPlace_COLS   = 0;
        }
        //}}}
        // ResetAutoPlace_x_y_cols {{{
        public static void ResetAutoPlace_x_y_cols(int x, int y, int cols)
        {
            AutoPlace_WRAP_X = x;
            AutoPlace_WRAP_Y = y;
            AutoPlacePoint.X = x;
            AutoPlacePoint.Y = y;

            AutoPlace_ROWS   = 0;
            AutoPlace_COLS   = (cols>=1) ? cols : 1;
        }
        //}}}

        // get_free_grid_xy {{{
        private Point get_free_grid_xy(string caller, NotePane np)
        {

            // STEP {{{
            Size   default_size     = get_default_size(np);
            Size   item_size        = get_item_size   (np);

            if(    (np.Type == NotePane.TYPE_CONTROL )
                || (np.Type == NotePane.TYPE_RICHTEXT)
                || (np.Type == NotePane.TYPE_SHORTCUT)
            ) {
                FreePlacePoint.X     = AutoPlacePoint.X;
                FreePlacePoint.Y     = AutoPlacePoint.Y;
                if(AutoPlace_ROWS > 0) AutoPlacePoint.Y += item_size.Height;
                else                   AutoPlacePoint.X += item_size.Width;
            }
            else {  // TYPE_PANEL .. TYPE_DASH
                FreePlacePoint.X  = 0;
                FreePlacePoint.Y  = AutoPlace_PANELY;
                AutoPlace_PANELY += item_size.Height + 1;
            }

            //}}}
            // WRAP {{{
            if(AutoPlace_ROWS > 0) {
                if( AutoPlacePoint.Y >= (AutoPlace_WRAP_Y + default_size.Height*AutoPlace_ROWS))
                {
                    AutoPlacePoint.Y  = AutoPlace_WRAP_Y;           // FIRST ROW LOCATION
                    AutoPlace_WRAP_X += item_size.Width ;// + 1;    // next column .. with spacing ?
                    AutoPlacePoint.X  = AutoPlace_WRAP_X;
                }
            }
            else {
                if( AutoPlacePoint.X >= (AutoPlace_WRAP_X + default_size.Width*AutoPlace_COLS))
                {
                    AutoPlacePoint.X  = AutoPlace_WRAP_X;           // FIRST COLUMN LOCATION
                    AutoPlace_WRAP_Y += item_size.Height;// + 1;    // next row .. with spacing ?
                    AutoPlacePoint.Y  = AutoPlace_WRAP_Y;
                }
            }

            //}}}

//log("get_free_grid_xy("+ caller +", "+ np +"): ...return "+ freeAutoPlacePoint.ToString());
//XXX += "\nget_free_grid_xy("+ caller +", "+ np.Name +"): ...return "+ FreePlacePoint.ToString();

            return  FreePlacePoint;
        }
        //}}}
//public static string XXX = "";

        // get_free_xy_wh {{{
        private string get_free_xy_wh(string caller, NotePane np)
        {
            Point  location = get_free_grid_xy(caller, np);
            Size   item_size= get_item_size(np);
        //  string xy_wh    = location.X+","+location.Y+"," + item_size.Width+","+item_size.Height;
            string xy_wh    = String.Format("{0:D2},{1:D2},{2:D2},{3:D2}", location.X, location.Y,  item_size.Width, item_size.Height);
            return xy_wh;
        }
        //}}}
        // get_item_size {{{
        private Size get_item_size(NotePane np)
        {
            if(     (np.Name.Equals(NotePane.CONTROL_NAME_EXIT    ))
                ||  (np.Name.Equals(NotePane.CONTROL_NAME_SETTINGS))

                ||  (np.Name.Equals(NotePane.CONTROL_NAME_PROFILE))
                ||  (np.Name.Equals(NotePane.CONTROL_NAME_UPDATE_PROFILE))
                ||  (np.Name.Equals(NotePane.CONTROL_NAME_EXPORT_PROFILE))
                ||  (np.Name.Equals(NotePane.CONTROL_NAME_EXPORT_TO_FILE))

              ) {
                return new Size(3*TAB_MIN_BTN_W+1, 4*TAB_MIN_BTN_H);
            }
/*
// NOTE: when loadTabSettings calls, np has no Tag yet!

            else if( (np.Tag != null)
                &&   ((string)(np.Tag)).StartsWith("PROFILE")
            ) {
                return new Size(4*TAB_MIN_BTN_W  , 5*TAB_MIN_BTN_H);
            }
*/
            else if((np.Name.Equals(NotePane.PANEL_NAME_COMM      ))
              ) {
                return new Size(4*TAB_MIN_TXT_W, 5*TAB_MIN_TXT_H);
            }
            else {
                return get_default_size(np);
            }
        }
        //}}}
         // get_default_size {{{
        private Size get_default_size(NotePane np)
        {
            if(     (np.Type == NotePane.TYPE_PANEL)
                ||  (np.Type == NotePane.TYPE_DASH )
              ) {
                return new Size(4*TAB_MIN_TXT_W,   TAB_MIN_TXT_H+1);
            }
            else if((np.Type == NotePane.TYPE_RICHTEXT)
              ) {
                return new Size(4*TAB_MIN_BTN_W,   TAB_MIN_BTN_W+2);
            }
            else {
                return new Size(2*TAB_MIN_BTN_W,   TAB_MIN_BTN_H+1);
            //  string text = np.Text.Trim();
            //  text = text.Trim(' ');
            //  text = text.Trim('\n');
            //  if(text.IndexOf ('\n') > 0) return new Size(2*TAB_MIN_BTN_W,   TAB_MIN_BTN_H+2);
            //  if(text.IndexOf ( ' ') > 0) return new Size(3*TAB_MIN_BTN_W,   TAB_MIN_BTN_H+1);
            }
        }
        //}}}
        // get_min_size {{{
        private Size get_min_size(NotePane np)
        {
            if(    (np.Type == NotePane.TYPE_PANEL)
                || (np.Type == NotePane.TYPE_DASH )
              ) {
                return new Size(  TAB_MIN_TXT_W,   TAB_MIN_TXT_H);
            }
            else
                return new Size(  TAB_MIN_BTN_W,   TAB_MIN_BTN_H);
        }
        //}}}
        // get_Visible_xy_max {{{
        public Point get_Visible_xy_max(string type)
        {
            int x,y,w,h;
            int x_max = 0;
            int y_max = 0;
            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );

                if( ! np.Visible      ) continue;
                if( !(np.Type == type)) continue;

                string[] a  = np.xy_wh.Split(',');
                x           = int.Parse( a[0] );
                y           = int.Parse( a[1] );
                w           = int.Parse( a[2] );
                h           = int.Parse( a[3] );

                x += w;
                y += h;

                if(y > y_max) y_max = y;
                if(x > x_max) x_max = x;
            }
            return new Point(x_max, y_max);
        }
        //}}}
        // get_free_grid_xy_near_top_left {{{
        public Point get_free_grid_xy_near_top_left()
        {
            // . new col if there is room ... next row if not
            // . always bellow previous one
            int x,y,w,h;
            int x_max = 0;
            int y_max = 0;
            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                if(np.Parent != tabs_container)
                    continue;

                string[] a  = np.xy_wh.Split(',');
                x           = int.Parse( a[0] );
                y           = int.Parse( a[1] );
                w           = int.Parse( a[2] );
                h           = int.Parse( a[3] );

                x += w;
                y += h;

                if(y > y_max) y_max = y;
                if(x > x_max) x_max = x;
            }
            // . locate new np as close to top-left as possible
            if(x_max > y_max) { x =     0; y = y_max; }
            else              { x = x_max; y =     0; }

            return new Point(x, y);
        }
        //}}}

        // AutoPlace - - - - - - - - - - - - - - - - - - - - - - - - - - - }}}
        // UPDATE- - - - - - - - - - - - - - - - - - - - - - - - - - - - - {{{
        // update_panel(name,text) {{{
        public NotePane update_panel(string tab_name, string text)
        {
            NotePane np = get_create_or_rebuild(tab_name, NotePane.TYPE_PANEL, text);
            return update_tab_text(np, text);
        }

        //}}}
        // update_dash(name,text) {{{
        public NotePane update_dash(string tab_name, string text)
        {
            NotePane np = get_create_or_rebuild(tab_name, NotePane.TYPE_DASH, text);
            return update_tab_text(np, text);
        }

        //}}}

        // update_control(name) {{{
        public NotePane update_control(string tab_name)
        {
            string label = tab_name;
            return update_control(tab_name, label);
        }
        //}}}
        // update_control(name,label) {{{
        public NotePane update_control(string tab_name, string label)
        {
            NotePane np = get_create_or_rebuild(tab_name, NotePane.TYPE_CONTROL, "");
            np.Label    = label;
            return np;
        }
        //}}}
        // update_control(name,label, color) {{{
        public NotePane update_control(string tab_name, string label, string color)
        {
            int color_int = 0; try { color_int = int.Parse( color ); } catch(Exception) { } color = String.Format("{0:D2}", color_int);
            if(color == "00") color = "";

            NotePane np = get_create_or_rebuild(tab_name, NotePane.TYPE_CONTROL, "");
            np.Label    = label;
            np.color    = color;
            return np;
        }
        //}}}

        // update_richtext() {{{
        public NotePane update_richtext()
        {
            string tab_name = get_free_tab_name();
            string text     = tab_name;
            return update_richtext(tab_name, text);
        }

        //}}}
        // update_richtext(name,text) {{{
        public NotePane update_richtext(string tab_name, string text)
        {
            NotePane np = get_create_or_rebuild(tab_name, NotePane.TYPE_RICHTEXT, text);
            return update_tab_text(np, text);
        }

        //}}}

        // update_tab_text(np, name,text) {{{
        public NotePane update_tab_text(NotePane np, string text)
        {
            // CLEAR CONTENT {{{
            if(text == Logger.LOG_CLEAR) {
                np.Text = "";
            }
            //}}}
            // REPLACE DASHBOARD CONTENT {{{
            else if(np.Type == NotePane.TYPE_DASH)
            {
                np.TextBox.Text = text;
            }
            //}}}
            // APPEND PANEL TEXT {{{
            else {
                if(np.Text == "\n")
                    np.Text = text;
                else
                    np.TextBox.AppendText(text);
            }
            //}}}
            try {
                np.TextBox.SelectionStart = np.Text.Length;
                np.TextBox.ScrollToCaret();
            } catch(Exception) {}

            return np;
        }

        //}}}

        // get_create_or_rebuild(tab_name, type, text) {{{
        private NotePane get_create_or_rebuild(string tab_name, string type, string text)
        {
            return get_create_or_rebuild(tab_name, type, text, "");
        }
        private NotePane get_create_or_rebuild(string tab_name, string type, string text, string color)
        {
            NotePane np = get_tab_NP( tab_name );
            if(np != null) return np;

            text = NotePane.decode_text( text );
            Object o = create_tab(tab_name, type, text, color);
            if(o != null) {
                System.Tuple<string, NotePane> tuple = (System.Tuple<string, NotePane>)o;
                np = tuple.Item2;
            }
            return np;
        }

        //}}}
        // UPDATE  - - - - - - - - - - - - - - - - - - - - - - - - - - - - }}}
        // ADD - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - {{{
        // create_tab(tab_name, type, text) {{{
        private NotePane pop_NotePane() //{{{
        {
            //return null;
            //log("pop_NotePane(): pool_Stack.Count=["+pool_Stack.Count+"]");
            if(pool_Stack.Count > 0) return pool_Stack.Pop();
            else return null;
        }
        // }}}
        private Object create_tab(string tab_name, string type, string text)
        {
            return  create_tab(tab_name, type, text, "");
        }
        private Object create_tab(string tab_name, string type, string text, string color)
        {
            string stage = "pop_NotePane";

            // pick from the pool of deleted tabs
            NotePane np  = pop_NotePane();
            if(np == null) np = new NotePane(type, tab_name, "", color);
            else           np.initialize(    type, tab_name, "", color);

            try {
                stage = "new NotePane"; // {{{

                //}}}
                stage = "set_logger"; // ...callback receiver {{{
                if(logger != null)
                    np.set_logger( logger );

                //}}}
                stage = "loadTabSettings"; // {{{

                loadTabSettings( np );

                //}}}
                stage = "np.rebuild_rtf"; // {{{
            //  np.reload_saved_tag();
            //  np.rebuild_rtf();
            //  np.reload_saved_zoom();
                layout_tab_on_grid( np );

                //}}}
                stage = "container.Add"; // {{{
                container_Add(np);

                //}}}
                // Show {{{
                stage = "Show";

                np.Show();
                //}}}
                return _add_tab_tuple(tab_name, np);
            }
            catch(Exception ex) { displayNotePane("create_tab: stage=["+ stage +"]", np, ex); }
            return null;
        }
        // }}}

        //  _add_tab_tuple(tab_name, np) //{{{
        private Object _add_tab_tuple(string tab_name, NotePane np)
        {
            /** Store (and return) new Tuple<tab_name, np>. */
            System.Tuple<string, NotePane> tuple = new System.Tuple<string, NotePane>(tab_name, np);

            if(tabs_Dictionary.ContainsKey( tab_name ) ) {
                log("_add_tab_tuple: tabs_Dictionary.ContainsKey("+ tab_name +") ... calling get_free_tab_name:");
                tab_name = get_free_tab_name();
            }
            tabs_Dictionary.Add(tab_name, tuple);

            return tuple;
        }
        // }}}
        // _get_tab_tuple(tab_name) {{{
        private Object _get_tab_tuple(string tab_name)
        {
            Object o = null;
            try {  o = tabs_Dictionary[tab_name]; } catch(Exception) { }
            return o;
        }
        // }}}
        // ADD - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - }}}
        // SELECT  - - - - - - - - - - - - - - - - - - - - - - - - - - - - {{{
        public void sel_tab_color(string color) //{{{
        {
        log("sel_tab_color("+ color +"):");
            // selecting/deselecting all tabs with this color
            // ...deselect if the first found is already selected

            bool found_first    = false;
            bool selecting      = true;
            int color_int       = 0; try { color_int = int.Parse( color ); } catch(Exception) { }

            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                if((np.Type != NotePane.TYPE_SHORTCUT) && (np.Type != NotePane.TYPE_RICHTEXT))
                    continue;

                int np_color_int = 0; try { np_color_int = int.Parse( np.color ); } catch(Exception) { }
                if(np_color_int == color_int)
                {
                    if( !found_first )
                        selecting = !np.isSelected();

                    if( selecting ) {
                        if(!Sel_SortedSet.Contains( np ) )
                            Sel_SortedSet.Add( np );
                        np.select();
                        log("...selecting ["+ np.Name +"]");
                    }
                    else {
                        if( Sel_SortedSet.Contains( np ) )
                            Sel_SortedSet.Remove( np );
                        np.unselect();
                        log("...unselecting ["+ np.Name +"]");
                    }
                }
            }
            MainFormInstance.sel_Announce();
        }
        // }}}
        public void sel_tab(NotePane np) //{{{
        {
            // unselect all but np
            foreach(NotePane item in Sel_SortedSet)
            {
                if(item != np)
                    item.unselect();
            }

            // clear current selection
            Sel_SortedSet.Clear();

            // select np (if not already a single selection
            if(np != null)
            {
                // clear the single selection
                if( np.isSelected() )
                {
                    np.unselect();
                }
                // make a new single selection
                else {
                    Sel_SortedSet.Add( np );
                    np.select();
                }
            }
            MainFormInstance.sel_Announce();
        }
        // }}}
        public void sel_tab_add(NotePane np) //{{{
        {
            if(!Sel_SortedSet.Contains( np ) )
                Sel_SortedSet.Add     ( np );
            np.select();
            MainFormInstance.sel_Announce();
        }
        // }}}
        public void sel_tab_add_all() //{{{
        {
            Sel_SortedSet.Clear();

            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                if((np.Type == NotePane.TYPE_SHORTCUT) || (np.Type == NotePane.TYPE_RICHTEXT))
                {
                    Sel_SortedSet.Add( np );
                    np.select();
                }
            }
            MainFormInstance.sel_Announce();
        }
        // }}}
        public void sel_tab_toggle(NotePane np) //{{{
        {
            if( Sel_SortedSet.Contains( np ) ) {
                Sel_SortedSet.Remove  ( np );
                np.unselect();
            }
            else  {
                Sel_SortedSet.Add( np );
                np.select();
            }
            MainFormInstance.sel_Announce();
        }
        // }}}
        public void sel_tab_extend(NotePane np) //{{{
        {
            // this np is already selected {{{
            if( Sel_SortedSet.Contains( np ) )
                return;

            //}}}
            // rectangle Surrounding this np and those already selected {{{
            int t = np.Top;
            int l = np.Left;
            int b = np.Bottom;
            int r = np.Right;
            foreach(NotePane item in Sel_SortedSet)
            {
                if(t > item.Top   ) t = item.Top;
                if(l > item.Left  ) l = item.Left;
                if(r < item.Right ) r = item.Right;
                if(b < item.Bottom) b = item.Bottom;
            }

            //}}}
            // select this np {{{
            Sel_SortedSet.Add( np );
            np.select();
            np.Invalidate();

            //}}}
            // outline selection area {{{
            Rectangle selR = new Rectangle(l, t, r-l, b-t);
            log("sel_tab_extend("+ np.Name +") ... selR=["+ selR +"]");

            Graphics graphics  = null;
            using(graphics     = tabs_container.CreateGraphics())
            {
                //SolidBrush  b_brush     = new SolidBrush( np.BackColor );
                //graphics.FillRectangle(b_brush, selR.Left, selR.Top, selR.Width, selR.Height);
                Pen b_pen = new Pen(Color.Red, 2);
                graphics.DrawRectangle  (b_pen, selR.Left, selR.Top, selR.Width, selR.Height);
            }
            //}}}
            // collect intersected tags {{{
            foreach(var item in tabs_Dictionary)
            {
                np = get_tab_NP( item.Key );

                if((np.Type != NotePane.TYPE_SHORTCUT) && (np.Type != NotePane.TYPE_RICHTEXT))
                    continue;

                if( Sel_SortedSet.Contains( np ) ) continue;

                Rectangle npR = new Rectangle(np.Left, np.Top, np.Right - np.Left, np.Bottom - np.Top);

                if( npR.IntersectsWith( selR ) ) {
                    log("...selecting ["+ np.Name +"]");
                    Sel_SortedSet.Add( np );
                    np.select();
                    np.Invalidate();
                }
            }
            //}}}
            MainFormInstance.sel_Announce();
        }
        // }}}
        public void sel_tab_copy() //{{{
        {
            Copy_HashSet.Clear();
/*
// XXX trying to reclaim memory {{{
// ...changes nothing apparently...
if(Sel_SortedSet.Count == 0)
{
    del_Dictionary  = new Dictionary<string, Object>();
    pool_Stack      = new Stack<NotePane>();
    Sel_SortedSet     = new HashSet<NotePane>();
    Copy_HashSet    = new HashSet<NotePane>();
    sb              = new StringBuilder();
} // XXX }}}
*/
            System.GC.Collect();

            foreach(NotePane item in Sel_SortedSet)
                Copy_HashSet.Add( NotePane.Clone( item ) );
        }
        // }}}
        public void sel_tab_paste() //{{{
        {

            // HAVE SOME TABS COPY SOURCE {{{
            if(Copy_HashSet.Count > 0)
            {
                // clear selection
                sel_tab(null);

                // duplicate source tags
                foreach(NotePane item in Copy_HashSet)
                {
                    // clone the clone
                    NotePane np_clone = NotePane.Clone( item );
                    np_clone.Name     = get_free_tab_name();

                    // DO NOT DUPLICATE DEFAULT TEXT
//log("sel_tab_paste: ...DO NOT DUPLICATE DEFAULT TEXT: item.Text=["+item.Text+"]");
                    if(    item.Text.StartsWith(      NotePane.PANEL_NAME_USR )
                        || item.Text.StartsWith( "\n"+NotePane.PANEL_NAME_USR )
                      )
                        np_clone.Text = np_clone.Name;

                    _add_tab_tuple(np_clone.Name, np_clone);

                    // display the clone
                    layout_tab_on_grid( np_clone );
                    np_clone.Locked   = false;
                    tabs_container.Controls.Add( np_clone );
                    tabs_container.Controls.SetChildIndex(np_clone, 0);

                    // rebuild current selection
                    sel_tab_add( np_clone );
                }
            }
            //}}}
            // COPY CLIPBOARD TEXT INTO SELECTION {{{
            else if(Sel_SortedSet.Count > 0)
            {
                string cb_text = ClipboardAsync.GetText();
                if(cb_text != "") {
                    foreach(NotePane item in Sel_SortedSet)
                        item.Text = cb_text;
                }
            }
            //}}}

        }
        // }}}
        public void sel_tab_clear() //{{{
        {
            foreach(NotePane item in  Sel_SortedSet)
                item.unselect();
            Sel_SortedSet.Clear();
            MainFormInstance.sel_Announce();
        }
        // }}}
        public static int Sel_tab_count() //{{{
        {
            return Sel_SortedSet.Count;
        }
        // }}}
        public static int Get_Sel_Count_but_this_one(NotePane np) //{{{
        {
            int sel_count = Sel_SortedSet.Count;

            sel_count -= Sel_SortedSet.Contains(np) ? 1 : 0;

            return sel_count;
        }
        // }}}
        public static void Sel_Clear() //{{{
        {
            MainFormInstance.tabsCollection.sel_tab( null );
            MainFormInstance.sel_Announce();
        }
        // }}}
        public void sel_tab_activate_toggle() //{{{
        {
            if(Sel_SortedSet.Count < 1)
                return;

            bool activate = (Sel_SortedSet.First().Type != NotePane.TYPE_SHORTCUT);

            foreach(NotePane item in Sel_SortedSet)
                item.Type = activate ? NotePane.TYPE_SHORTCUT : NotePane.TYPE_RICHTEXT;
        }
        // }}}
        // CHANGE SELECTION LAYOUT [X Y W H]
        // {{{
        // ---------------------------------------------[user_item_]--[value]------------------------------------------------------------------------------[LOOP_ON_ALL_SELECTED_ITEMS_]-----------------[_CHANGE_VALUE__]------
        public static void Change_selection_X           (NotePane np, int dx) {                                                                       foreach(NotePane item in Sel_SortedSet)                { item.Left  += dx; } } // X
        public static void Change_selection_Y           (NotePane np, int dy) {                                                                       foreach(NotePane item in Sel_SortedSet)                { item.Top   += dy; } } // Y
        public static void Change_selection_Width       (NotePane np, int  w) {                                                                       foreach(NotePane item in Sel_SortedSet)                { item.Width  =  w; } } // Width
        public static void Change_selection_Height      (NotePane np, int  h) {                                                                       foreach(NotePane item in Sel_SortedSet)                { item.Height =  h; } } // Height

        public static void Change_selection_Align_Left  (NotePane np        ) { if(Sel_SortedSet.Count == 1)    return; int l = np.Left;              foreach(NotePane item in Sel_SortedSet) if(item != np) { item.Left   =  l;             } }    // Align Left
        public static void Change_selection_Align_Right (NotePane np        ) { if(Sel_SortedSet.Count == 1)    return; int r = np.Right;             foreach(NotePane item in Sel_SortedSet) if(item != np) { item.Left   =  r-item.Width;  } }    // Align Right
        public static void Change_selection_Align_Top   (NotePane np        ) { if(Sel_SortedSet.Count == 1)    return; int t = np.Top;               foreach(NotePane item in Sel_SortedSet) if(item != np) { item.Top    =  t;             } }    // Align Top
        public static void Change_selection_Align_Bottom(NotePane np        ) { if(Sel_SortedSet.Count == 1)    return; int b = np.Bottom;            foreach(NotePane item in Sel_SortedSet) if(item != np) { item.Top    =  b-item.Height; } }    // Align Bottom

        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        // ---------------------------------------------[user_item_]--[value]-[_NOTHING_TO_DO_WITH_SINGLE_SELECTION_]----[PROPAGATED_ATTRIBUTE___]---------[LOOP_ON_ALL_SELECTED_ITEMS_]-----------------[_CHANGE_VALUE________________]-[NEXT_VALUE_____]----
        public static void Change_selection_Chain_Right (NotePane np        ) { if(Sel_SortedSet.Count == 1)    return; int l = np.Left +  np.Width;  foreach(NotePane item in Sel_SortedSet) if(item != np) { item.Left   =  l            ; l += item.Width ; } } // Chain Right
        public static void Change_selection_Chain_Down  (NotePane np        ) { if(Sel_SortedSet.Count == 1)    return; int t = np.Top  +  np.Height; foreach(NotePane item in Sel_SortedSet) if(item != np) { item.Top    =  t            ; t += item.Height; } } // Chain Down
        public static void Change_selection_Chain_Left  (NotePane np        ) { if(Sel_SortedSet.Count == 1)    return; int r = np.Left;              foreach(NotePane item in Sel_SortedSet) if(item != np) { item.Left   =  r-item.Width ; r -= item.Width ; } } // Chain Left
        public static void Change_selection_Chain_Up    (NotePane np        ) { if(Sel_SortedSet.Count == 1)    return; int b = np.Top;               foreach(NotePane item in Sel_SortedSet) if(item != np) { item.Top    =  b-item.Height; b -= item.Height; } } // Chain Up
        // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        // }}}
        // GRID
        public static void Selection_grid_layout(bool start_dragging) //{{{
        {
            foreach(NotePane item in Sel_SortedSet)
                item.grid_layout( start_dragging );
        }
        // }}}
        // TYPE TAG TEXT ZOOM COLOR
        // {{{
        public static void Clone_selection_Text      (NotePane np) { foreach(NotePane item in Sel_SortedSet) if(item != np) item._tb.Text       = np._tb.Text;       }
        public static void Clone_selection_Type      (NotePane np) { foreach(NotePane item in Sel_SortedSet) if(item != np) item.Type           = np.Type;           }
        public static void Clone_selection_ZoomFactor(NotePane np) { foreach(NotePane item in Sel_SortedSet) if(item != np) item._tb.ZoomFactor = np._tb.ZoomFactor; }
        public static void Clone_selection_zoom      (NotePane np) { foreach(NotePane item in Sel_SortedSet) if(item != np) item.zoom           = np.zoom;           }
        public static void Clone_selection_color     (NotePane np) { foreach(NotePane item in Sel_SortedSet) if(item != np) item.color          = np.color;          }
        public static void Clone_selection_shape     (NotePane np) { foreach(NotePane item in Sel_SortedSet) if(item != np) item.shape          = np.shape;          }
        public static void Clone_selection_Tag       (NotePane np) { foreach(NotePane item in Sel_SortedSet) if(item != np) item.Tag            = np.Tag;            }
    //  public static void Clone_selection_Rtf       (NotePane np) { foreach(NotePane item in Sel_SortedSet) if(item != np) item._tb.Rtf        = np._tb.Rtf;        }

        //}}}
        // SELECT- - - - - - - - - - - - - - - - - - - - - - - - - - - - - }}}
        // DELETE  - - - - - - - - - - - - - - - - - - - - - - - - - - - - {{{
        // del_tab {{{
        public void del_tab(NotePane np)
        {
            if(np == null) return;

            if( !Sel_SortedSet.Contains( np ) ) {
                del_np( np );
                tabs_Dictionary.Remove( np.Name );
            //  tabs_container.Invalidate();
            }
            else {
                del_sel();
            }
        }
        //}}}
        // del_sel {{{
        public void del_sel()
        {
            sel_tab_copy();              // put deleted item into copy buffer

            del_Dictionary.Clear();    // collect to be removed from tabs_Dictionary

            foreach(NotePane item in  Sel_SortedSet)
                del_np( item );
            Sel_SortedSet.Clear();

            foreach(var item in del_Dictionary)
                tabs_Dictionary.Remove( item.Key ); // remove collected from tabs_Dictionary
            del_Dictionary.Clear();

            System.GC.Collect();

            tabs_container.Invalidate();

            MainFormInstance.sel_Announce();
        }
        //}}}
        // del_tabs_named {{{
        public void del_tabs_named(string name_StartsWith)
        {
            log("del_tabs_named("+ name_StartsWith +"): BEFORE: tabs_Dictionary.Count=["+ tabs_Dictionary.Count         +"]");
            log(".......................................tabs_container.Controls.Count=["+ tabs_container.Controls.Count +"]");

            del_Dictionary.Clear(); // collect to be removed from tabs_Dictionary

            foreach(var item in tabs_Dictionary) {
                string tab_name = item.Key;
                if( tab_name.StartsWith( name_StartsWith ) )
                    del_np( get_tab_NP( tab_name ) );
            }

            foreach(var item in  del_Dictionary)
                tabs_Dictionary.Remove( item.Key ); // remove collected from tabs_Dictionary
            del_Dictionary.Clear();

            Sel_SortedSet.Clear();
            System.GC.Collect();

            log(".......................................tabs_container.Controls.Count=["+ tabs_container.Controls.Count +"]");
            log("del_tabs_named("+ name_StartsWith +"): .AFTER: tabs_Dictionary.Count=["+ tabs_Dictionary.Count         +"]");
            MainFormInstance.sel_Announce();
        }
        //}}}
        // del_item {{{
        private void del_np(NotePane np)
        {
            if( Sel_SortedSet.Contains( np ) )
                np.unselect();

            np.Label    = "";
            np.Locked   = true;
            np.TT       = "";
            np.Tag      = "";
            np.Text     = "";
            np.zoom     = "";

            container_Remove   ( np         );
            if( !del_Dictionary.ContainsKey( np.Name ) )
            {
                del_Dictionary.Add ( np.Name, np);  // collect to be removed from tabs_Dictionary
                pool_Stack    .Push( np         );
                del_tab_settings   ( np.Name    );
            }
        }
        //}}}
        // has_tabs_named {{{
        public bool has_tabs_named(string name_StartsWith)
        {
            log("has_tabs_named("+ name_StartsWith +"):");

            bool diag = false;

            foreach(var item in tabs_Dictionary) {
                string tab_name = item.Key;
                if( tab_name.StartsWith( name_StartsWith ) ) {
                    diag = true;
                    break;
                }
            }

            log("has_tabs_named("+ name_StartsWith +") ...return "+ diag);
            return diag;
        }
        //}}}
        // delete_usr_tabs {{{
        public void delete_usr_tabs()
        {
            log("delete_usr_tabs()");

            del_tabs_named( NotePane.PANEL_NAME_USR );

            //tabs_container.Controls.Clear();
        }
        //}}}
        // DELETE- - - - - - - - - - - - - - - - - - - - - - - - - - - - - }}}
        // ACCESS  - - - - - - - - - - - - - - - - - - - - - - - - - - - - {{{
        // get_tab_NP {{{
        public NotePane get_tab_NP(string tab_name)
        {
            NotePane np = null;
            Object o = _get_tab_tuple(tab_name);
            if(o != null) {
                System.Tuple<string, NotePane> tuple = (System.Tuple<string, NotePane>)o;
                np = tuple.Item2;
            }

            return np;
        }
        //}}}

        // has_tab {{{
        public bool has_tab(string tab_name)
        {
            return _get_tab_tuple(tab_name) != null;
        }
        //}}}

        // GetUsrNotePaneWaitingForEscape {{{
        public NotePane GetUsrNotePaneWaitingForEscape()
        {
            foreach(var item in tabs_Dictionary) {
                NotePane  np = get_tab_NP( item.Key );
                Point p = np.PointToClient(Control.MousePosition);
                if(     np.Visible
                    && (np.Type != NotePane.TYPE_CONTROL)
                    &&  np.is_maximized()
                    &&  np.ClientRectangle.Contains( p )
                  )
                  return np;
            }
            return null;
        }
        //}}}

        // ACCESS  - - - - - - - - - - - - - - - - - - - - - - - - - - - - }}}
        // STORE - - - - - - - - - - - - - - - - - - - - - - - - - - - - - {{{
        // get_free_tab_name {{{
        public string get_free_tab_name()
        {
            string tab_name;
            for(int i=1; i <= tabs_Dictionary.Count; ++i)
            {
                tab_name = NotePane.PANEL_NAME_USR + i.ToString("D3");
                if( !tabs_Dictionary.ContainsKey(tab_name) )
                    return tab_name;
            }
            tab_name = NotePane.PANEL_NAME_USR + (tabs_Dictionary.Count+1).ToString("D3");
            return tab_name;
        }
        //}}}


        // STORE - - - - - - - - - - - - - - - - - - - - - - - - - - - - - }}}
        // CONTAINERS {{{
        // scroll_containers_top_left {{{
        public void scroll_containers_top_left()
        {
            log("scroll_containers_top_left()");

            containers_VerticalScroll  ( 0 );
            containers_HorizontalScroll( 0 );

            // scrollbars are not willing to update!
            tabs_container.Parent.PerformLayout();
        }
        //}}}
        // containers_VerticalScroll {{{
        private void containers_VerticalScroll(int value)
        {
            Panel p = (Panel)(tabs_container.Parent);
            p                       .VerticalScroll.Value   = value;
            panels_container        .VerticalScroll.Value   = value;
            controls_container      .VerticalScroll.Value   = value;
        }
        //}}}
        // containers_HorizontalScroll {{{
        private void containers_HorizontalScroll(int value)
        {
            Panel p = (Panel)(tabs_container.Parent);
            p                       .HorizontalScroll.Value = value;
            panels_container        .HorizontalScroll.Value = value;
            controls_container      .HorizontalScroll.Value = value;
        }
        //}}}
        // container_ScrollControlIntoView {{{
        private void container_ScrollControlIntoView(NotePane np)
        {
            if     (np.Type == NotePane.TYPE_PANEL   )   panels_container.ScrollControlIntoView(np);
            else if(np.Type == NotePane.TYPE_DASH    )   panels_container.ScrollControlIntoView(np);
            else if(np.Type == NotePane.TYPE_CONTROL ) controls_container.ScrollControlIntoView(np);
            else if(np.Type == NotePane.TYPE_RICHTEXT)     tabs_container.ScrollControlIntoView(np);
            else if(np.Type == NotePane.TYPE_SHORTCUT)     tabs_container.ScrollControlIntoView(np);
        }
        //}}}
        // container_BringToFront {{{
        private void container_BringToFront(NotePane np)
        {
            if     (np.Type == NotePane.TYPE_PANEL   )   panels_container.Controls.SetChildIndex(np, 0);
            else if(np.Type == NotePane.TYPE_DASH    )   panels_container.Controls.SetChildIndex(np, 0);
            else if(np.Type == NotePane.TYPE_CONTROL ) controls_container.Controls.SetChildIndex(np, 0);
            else if(np.Type == NotePane.TYPE_RICHTEXT)     tabs_container.Controls.SetChildIndex(np, 0);
            else if(np.Type == NotePane.TYPE_SHORTCUT)     tabs_container.Controls.SetChildIndex(np, 0);
        }
        //}}}
        // container_PushToBack {{{
        private void container_PushToBack(NotePane np)
        {
            if     (np.Type == NotePane.TYPE_PANEL   )   panels_container.Controls.SetChildIndex(np,   panels_container.Controls.Count);
            else if(np.Type == NotePane.TYPE_DASH    )   panels_container.Controls.SetChildIndex(np,   panels_container.Controls.Count);
            else if(np.Type == NotePane.TYPE_CONTROL ) controls_container.Controls.SetChildIndex(np, controls_container.Controls.Count);
            else if(np.Type == NotePane.TYPE_RICHTEXT)     tabs_container.Controls.SetChildIndex(np,     tabs_container.Controls.Count);
            else if(np.Type == NotePane.TYPE_SHORTCUT)     tabs_container.Controls.SetChildIndex(np,     tabs_container.Controls.Count);
        }
        //}}}
        // container_Add {{{
        private void container_Add(NotePane np)
        {
            Panel container;

            if(     (np.Type == NotePane.TYPE_RICHTEXT)
                ||  (np.Type == NotePane.TYPE_SHORTCUT)
                || ((np.Type == NotePane.TYPE_DASH    ) && np.Name.StartsWith(NotePane.PANEL_NAME_USR))
              )
                container    =     tabs_container;

            else if(np.Type == NotePane.TYPE_CONTROL )
                container    = controls_container;

            else
                container    = panels_container;

            container.Controls.Add( np );
            container.Controls.SetChildIndex(np, 0); // bring to front
        }
        //}}}
        // container_Remove {{{
        private void container_Remove(NotePane np)
        {
            Panel container;

            if(     (np.Type == NotePane.TYPE_RICHTEXT)
                ||  (np.Type == NotePane.TYPE_SHORTCUT)
                || ((np.Type == NotePane.TYPE_DASH    ) && np.Name.StartsWith(NotePane.PANEL_NAME_USR))
              )
                container    =     tabs_container;

            else if(np.Type == NotePane.TYPE_CONTROL )
                container    = controls_container;

            else
                container    = panels_container;

            container.Controls.Remove( np );
        }
        //}}}
        // container_Visible {{{
        private void container_Visible(bool state)
        {
            tabs_container    .Visible = state;
            panels_container  .Visible = state;
            controls_container.Visible = state;
        }
        //}}}
        // container_SuspendLayout {{{
        private void container_SuspendLayout()
        {
            tabs_container    .SuspendLayout();
            panels_container  .SuspendLayout();
            controls_container.SuspendLayout();
        }
        //}}}
        // container_ResumeLayout {{{
        private void container_ResumeLayout()
        {
            tabs_container    .ResumeLayout();
            panels_container  .ResumeLayout();
            controls_container.ResumeLayout();
        }
        //}}}
        //}}}
        // SETTINGS  - - - - - - - - - - - - - - - - - - - - - - - - - - - {{{

        // LoadSetting
        // load_usr_tabs {{{
        public  void load_usr_tabs()
        {
            // LOAD tabs_Dictionary TABS

            string[] valueNames = Settings.GetValueNames(); // filter registry names
            string     t_prefix = "TAB."+NotePane.PANEL_NAME_USR;
            int         t_count = 0;
            foreach(string valueName in valueNames)
            {
                if(!valueName.StartsWith( t_prefix ) || valueName.EndsWith(".rtf"))
                    continue;

                log(String.Format("{0,3} {1}", ++t_count, valueName));
                string tab_name = valueName.Substring(4); // TAB.

                NotePane     np = get_tab_NP( tab_name );

                // adjust existing np
                if(np != null) {
                    loadTabSettings( np );
                }
                // create new np from saved settings
                else {
                    string settings       = Settings.LoadSetting("TAB."+ tab_name, "");
                    if(settings   != "") {
                        try {
                            string[]    a    = settings.Split( NotePane.TABVALUE_SEPARATOR );
                            string      type = a[0].Substring(5);
                            string      text = a[4].Substring(5);
                            text = NotePane.decode_text( text );
                            if(type != "")     create_tab(tab_name, type, text);
                        }
                        catch(Exception) { }
                    }
                }

            }

        }
        // }}}
        // list_usr_tabs {{{
        public  void list_usr_tabs()
        {
            log("list_usr_tabs("+ Settings.APP_NAME +"):");

            int    t_count = 0; string t_prefix = "TAB."+NotePane.PANEL_NAME_USR;
            int    p_count = 0; string p_prefix = "PALETTE.";

            string[] valueNames = Settings.GetValueNames(); // filter registry names
            foreach(string valueName in valueNames)
            {
                if(     valueName.StartsWith( t_prefix ) && !valueName.EndsWith(".rtf")) log(String.Format("{0,3} {1}", ++t_count, valueName));
                else if(valueName.StartsWith( p_prefix )                               ) log(String.Format("{0,3} {1}", ++p_count, valueName));
                else                                                                     log(String.Format("{0,3} {1}", ""       , valueName));
            }
        }
        // }}}
        // loadTabSettings(np) {{{
        private void loadTabSettings(NotePane np)
        {
            string stage = "settings";
            try {
                // type {{{
                stage = "type";

                //string type  = np.get_saved_type();

                if((np.Type == NotePane.TYPE_CONTROL) || (np.Type == NotePane.TYPE_SHORTCUT)) {
                    np.HorizontalScroll.Enabled = false;    // disable only
                    np.HorizontalScroll.Visible = false;    // disable only
                }
                //}}}
                // xy_wh {{{
                stage = "xy_wh";
                string   xy_wh                 = np.get_saved_xy_wh();
                if(      xy_wh == "")    xy_wh = get_free_xy_wh("loadTabSettings", np);
                if(      xy_wh != "") np.xy_wh = xy_wh;

                //}}}
                // zoom {{{
                stage = "zoom";

                //np.TextBox.ZoomFactor = 1.0F;                               // https://social.msdn.microsoft.com/forums/windows/en-us/8b61eef0-b712-4b8b-9f5f-c9bbf75abb53/richtextbox-zoomfactor-problems
                //if(zoom != "") np.TextBox.ZoomFactor = float.Parse( zoom ) * Settings.TXT_ZOOM;
                np.zoom = np.get_saved_zoom();

                //}}}
                // color {{{
                stage = "color";

                np.color = np.get_saved_color();

                //}}}
                // shape {{{
                stage = "shape";

                np.shape = np.get_saved_shape();

                //}}}
                // text {{{
                np.Text = NotePane.decode_text( np.get_saved_text() );

                //}}}
                // tag and toolTip {{{
                stage = "tag";

                np.Tag  = NotePane.decode_text( np.get_saved_tag() );
                np._tt  = NotePane.decode_text( np.get_saved_tt () );

                np.update_tooltip();
                //}}}
            }
            catch(Exception ex) { displayNotePane("loadTabSettings: stage=["+ stage +"]", np, ex); }

//if(np.Name == NotePane.CONTROL_NAME_LOGGING) displayNotePane("zoom=["+zoom+"]\nloadTabSettings("+ np.Name +")", np, null);

        }
        //  }}}

        // import_tab_line {{{
        public NotePane import_tab_line(string tab_name, string tab_value)
        {

            // create new np from tab_value
            try {
                // sample lines {{{
                //  tab_name=[TAB.panel_usr10]
                // tab_value=[type=SHORTCUT|tag=0164914961|zoom=1|xy_wh=87,22,9,3|text=0164914961|shape=circle|tt=Tél. Perso]
                //            0000000000000 11111111111111 222222 333333333333333 .....4444444444 555555555555 6666666666666
                //
                //  tab_name=[TAB.panel_LOG]
                // tab_value=[type=PANEL|tag=|zoom=.57|xy_wh=23,43,11,11|text=|tt=]
                //        0000000000 1111 22222222 33333333333333333 44444 555
                //
                //  tab_name=[TAB.control_add]
                // tab_value=[type=CONTROL|tag=|zoom=1.2|xy_wh=16,4,4,3|text=|tt=]
                //        000000000000 1111 22222222 33333333333333 44444 555
                //
                //}}}

                // TODO see Settings.set_KEY_VAL


                // KEY=VAL pairs {{{
                // VARS {{{
                string   name;

                string   type;
                string   tag;
                float    z;
                string   xy_wh;

                string   text;
                string   color;
                string   shape;
                string   tt;

                //}}}

                string[] args  = tab_value.Split('|');
                log("import_tab_line: "+ args.Length +" tab_value=["+ tab_value.Replace("|","\n") +"]:");

                type=""; tag=""; z=0; xy_wh=""; text=""; color=""; shape=""; tt="";

                for(int i=0; i < args.Length; ++i)
                {
                    // KEY=VAL
                    string[] kv  = args[i].Split('=');

                    if(kv.Length < 2)
                    {
                        log("*** args["+ i +"]=["+ args[i] +"]: key has no value");
                        continue;
                    }

                    if(kv.Length != 2)
                    {
                        log("*** args["+ i +"]=["+ args[i] +"]: key has more than one value");

                        // .................=....... .....=.... .....=....|
                        // MUST SUPPORT |tag=KEY_VAL DEV_W=1920 DEV_H=1200|
                        // .............|tag KEY_VAL DEV_W 1920 DEV_H 1200|
                        // .............|^^^ ^^^^^^^^^^^^^ ^^^^^^^^^^ ^^^^|
                        // .............|vvv vvvvvvvvvvvvvvvvvvvvvvvvvvvvv|
                        // ............ |tag=KEY_VAL DEV_W=1920 DEV_H=1200|

                        for(int j=2; j < kv.Length; ++j) kv[1] += "="+kv[j];

                        log("*** kv[1]=["+ kv[1] +"]");
                    }

                    //if(D) log("@@@ args["+ i +"]=["+ args[i] +"]");

                    try{ if(kv[0].ToUpper().Equals(   "TAB" )) {  name =             kv[1] ; log( string.Format("ooo {0,16} = {1}\n",  "name" , name )); } } catch(Exception ex) { log( string.Format("*** {0,16} : {1}\n", args[i] +"]: ", ex.Message+"\n")); }
                    try{ if(kv[0].ToLower().Equals(  "type" )) {  type =             kv[1] ; log( string.Format("ooo {0,16} = {1}\n",  "type" , type )); } } catch(Exception ex) { log( string.Format("*** {0,16} : {1}\n", args[i] +"]: ", ex.Message+"\n")); }
                    try{ if(kv[0].ToLower().Equals(   "tag" )) {   tag =             kv[1] ; log( string.Format("ooo {0,16} = {1}\n",   "tag" , tag  )); } } catch(Exception ex) { log( string.Format("*** {0,16} : {1}\n", args[i] +"]: ", ex.Message+"\n")); }
                    try{ if(kv[0].ToLower().Equals(     "z" )) {     z = float.Parse(kv[1]); log( string.Format("ooo {0,16} = {1}\n",     "z" , z    )); } } catch(Exception ex) { log( string.Format("*** {0,16} : {1}\n", args[i] +"]: ", ex.Message+"\n")); }
                    try{ if(kv[0].ToLower().Equals( "xy_wh" )) { xy_wh =             kv[1] ; log( string.Format("ooo {0,16} = {1}\n", "xy_wh" , xy_wh)); } } catch(Exception ex) { log( string.Format("*** {0,16} : {1}\n", args[i] +"]: ", ex.Message+"\n")); }
                    try{ if(kv[0].ToLower().Equals(  "text" )) {  text =             kv[1] ; log( string.Format("ooo {0,16} = {1}\n",  "text" , text )); } } catch(Exception ex) { log( string.Format("*** {0,16} : {1}\n", args[i] +"]: ", ex.Message+"\n")); }
                    try{ if(kv[0].ToLower().Equals( "color" )) { color =             kv[1] ; log( string.Format("ooo {0,16} = {1}\n", "color" , color)); } } catch(Exception ex) { log( string.Format("*** {0,16} : {1}\n", args[i] +"]: ", ex.Message+"\n")); }
                    try{ if(kv[0].ToLower().Equals( "shape" )) { shape =             kv[1] ; log( string.Format("ooo {0,16} = {1}\n", "shape" , shape)); } } catch(Exception ex) { log( string.Format("*** {0,16} : {1}\n", args[i] +"]: ", ex.Message+"\n")); }
                    try{ if(kv[0].ToLower().Equals(    "tt" )) {    tt =             kv[1] ; log( string.Format("ooo {0,16} = {1}\n",    "tt" , tt   )); } } catch(Exception ex) { log( string.Format("*** {0,16} : {1}\n", args[i] +"]: ", ex.Message+"\n")); }

                }
                // }}}


                // [tab_name] .. get a new tab_name when not replacing // {{{
                if(Settings.ImportMode != Settings.IMPORT_MODE_REPLACE)
                {
                    string old_tab_name = tab_name;
                    tab_name            = get_free_tab_name();
                    log("@@@ import_tab_line: ["+ old_tab_name +"] renamed ["+ tab_name +"]");
                }

                // }}}
                // [tab_name] .. or simply override any existing tab with that tab_name with imported settings // {{{
                Settings.SaveSetting("TAB."+tab_name, tab_value);

                // }}}
                // adjust existing np settings {{{
                NotePane np = get_tab_NP( tab_name );
                if(np != null)
                {
                    loadTabSettings( np );
                    //if(np.Parent == null) log("@@@ import_tab_line: ["+ tab_name +"] has no parent"); // *** TRACING: tabs_container <sync> tabs_Dictionary
                }
                //}}}
                // ...or create new tab .. [WITH OPTIONAL IMPORT OFFSET] {{{
                else if((type == NotePane.TYPE_SHORTCUT)
                    ||  (type == NotePane.TYPE_RICHTEXT)
                    ||  (type == NotePane.TYPE_PANEL   )
                    ||  (type == NotePane.TYPE_DASH    )
                    //  (type == NotePane.TYPE_CONTROL )
                    ) {
                    text = NotePane.decode_text( text );
                    Object  o  = create_tab(tab_name, type, text);
                    if(o != null)
                    {
                        // geometry {{{
                        System.Tuple<string, NotePane> tuple = (System.Tuple<string, NotePane>)o;
                        np = tuple.Item2;
                        /*
                        // create_tab calls get_free_xy_wh
                        string   xy_wh                 = np.get_saved_xy_wh();
                        if(      xy_wh == "")    xy_wh = get_free_xy_wh("import_tab_line", np);
                        if(      xy_wh != "") np.xy_wh = xy_wh;
                         */

                        //}}}
                        // OPTIONAL RELOCATING OFFSET {{{
                        if((Settings.ImportOffsetX != 0) || (Settings.ImportOffsetY != 0))
                        {
                            string old_xy_wh =  np.xy_wh;
                            int x=0, y=0, w=0, h=0;
                            try {
                                string[] a = np.xy_wh.Split(',');
                                x = (int)(int.Parse( a[0] ) + Settings.ImportOffsetX);
                                y = (int)(int.Parse( a[1] ) + Settings.ImportOffsetY);
                                w = (int)(int.Parse( a[2] )                         );
                                h = (int)(int.Parse( a[3] )                         );
                            } catch(Exception) { }
                            np.xy_wh= String.Format("{0:D2},{1:D2},{2:D2},{3:D2}", x, y, w, h);

                            log("@@@ import_tab_line: relocating ["+ tab_name +"] from ["+ old_xy_wh +"] to ["+ np.xy_wh +"]");

                        }
                        //}}}
                    }
                }
                //}}}

                return np;
            }
            catch(Exception ex) {
                log("*** import_tab_line [NOT PARSED]: tab_name=["+ tab_name +"] tab_value=["+ tab_value +"]\n"+ ex.Message);
                //throw ex;
                return null;
            }
        }
        // }}}
        // import_data_line {{{
        public NotePane import_data_line(string tab_name, string tab_value)
        {
            string type, text;

            try {
                // o---------------------------------------------------o {{{
                // |tab_name=[TAB.panel_usr10]                         |
                // |---------------------------------------------------|
                // |tab_value                                          |
                // |---------------------------------------------------|
                // | =[type=SHORTCUT]                                  |
                // | = [tag=0164914961]                                |
                // | =  [zoom=1]                                       |
                // | =   [xy_wh=87,22,9,3]                             |
                // | =    [text=0164914961|shape=circle|tt=Tél. Perso] |
                // o---------------------------------------------------o }}}
                type        = NotePane.TYPE_SHORTCUT;
                // avoid creating invisible tabs
                if(tab_value.Trim() == "") {
                    log("*** import_data_line [NOT PARSED]: tab_name=["+ tab_name +"] tab_value=[EMPTY]");
                    return null;
                }
                text        = NotePane.decode_text( tab_value );
            //  NotePane np = get_create_or_rebuild(tab_name, type, text);
                Object  o   = create_tab(tab_name, type, text);
                System.Tuple<string, NotePane> tuple = (System.Tuple<string, NotePane>)o;
                NotePane np = tuple.Item2;

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

                // tag
                if( !text.StartsWith("#") )
                {
                    if( !Settings.TAG_CMD.Equals("") ) {
                        np.Tag = Settings.TAG_CMD +" "+ text;
                        if(Settings.TAG_SFX != "")
                            np.Tag += Settings.TAG_SFX;
                    }
                    else {
                        np.Tag = "# "+ text;
                    }
                }
                else
                    np.Tag =       text;

                // text
                np.Text    =       text;

                // xy_wh
                //np.xy_wh = get_free_xy_wh("import_data_line", np);

                np.update_tooltip();

                // ProfileLines .. profile file content to save
                Settings.SaveSetting("TAB."+tab_name, text);

                return np;
            }
            catch(Exception) {
                log("*** import_data_line [NOT PARSED]: tab_name=["+ tab_name +"] tab_value=["+ tab_value +"]");
                return null;
            }
        }
        // }}}

        // SaveSetting
        public  void saveSettings() // {{{
        {
            log("saveSettings()");

            // SAVE tabs_Dictionary TABS
            int id_max = 0;
            foreach(var item in tabs_Dictionary)
            {
                // store into registry
                string tab_name  = item.Key;
                if( !tab_name.StartsWith(NotePane.PANEL_NAME_PROFILE) )
                    saveTabSettings( tab_name );

                // track usr panels id_max
                //{{{
                if(tab_name.StartsWith(NotePane.PANEL_NAME_USR))
                {
                    string id_str = "";
                    int    id     =  0;
                    try {
                        id_str = tab_name.Substring(NotePane.PANEL_NAME_USR.Length);
                        if(id_str != "") {
                            try { id = int.Parse( id_str ); } catch(Exception) { }
                            if(id > id_max) id_max = id;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("TabsCollection.saveSettings:\n"
                            + Settings.ExToString(ex) +"\n"
                            +"tab_name=["+ tab_name +"]\n\n"
                            +"  id_str=["+ id_str +"]\n"
                            +"  id    =["+ id     +"]\n"
                            +"  id_max=["+ id_max +"]"
                            , "TabsCollection"
                            , MessageBoxButtons.OK
                            , MessageBoxIcon.Information);
                    }
                }
                //}}}
            }

            // STORE ESTIMATED USR PANELS ID_MAX (to be used as a helper by next cleanup)
            Settings.SaveSetting("UserTabIDmax", id_max.ToString());

            // CLEANUP HIGHER OBSOLETE ENTRIES
            delete_extra_id(id_max+1);

        }
        // }}}
        private void saveTabSettings(string tab_name) // {{{
        {
            NotePane np  = get_tab_NP( tab_name );
//log("saveTabSettings("+ tab_name +") ["+ np.Type +"]");
            // may have been removed from tabs_container by delete_usr_tabs ... and not from tabs_Dictionary !!!
            if(np.Parent == null)
            {
                log("saveTabSettings("+ tab_name +") (np.Parent == null)");
                del_tab_settings( tab_name );
                return;
            }

            string type  = get_np_type ( np );
            string tag   = get_np_tag  ( np );
            string zoom  = get_np_zoom ( np );
            string xy_wh = get_np_xy_wh( np );
            string text  = get_np_text ( np );
            string color = get_np_color( np );
            string shape = get_np_shape( np );
            string tt    = get_np_tt   ( np );

/* //{{{
            Settings.SaveSetting(
                "TAB."+np.Name
                ,  "type=" + type  + NotePane.TABVALUE_SEPARATOR
                +   "tag=" + tag   + NotePane.TABVALUE_SEPARATOR
                +  "zoom=" + zoom  + NotePane.TABVALUE_SEPARATOR
                + "xy_wh=" + xy_wh + NotePane.TABVALUE_SEPARATOR
                +  "text=" + text  + NotePane.TABVALUE_SEPARATOR
                + "color=" + color + NotePane.TABVALUE_SEPARATOR
                +  "tt="   + tt
                );
*/ //}}}

            sb.Clear();

/* (160704 order changed)
            sb.Append(  "type="+ type  + NotePane.TABVALUE_SEPARATOR); // 0 type
            sb.Append(   "tag="+ tag   + NotePane.TABVALUE_SEPARATOR); // 1 tag
            sb.Append(  "zoom="+ zoom  + NotePane.TABVALUE_SEPARATOR); // 2 zoom
            sb.Append( "xy_wh="+ xy_wh + NotePane.TABVALUE_SEPARATOR); // 3 xy_wh
            sb.Append(  "text="+ text  + NotePane.TABVALUE_SEPARATOR); // 4 text
            sb.Append( "color="+ color + NotePane.TABVALUE_SEPARATOR); // 5 color
            sb.Append( "shape="+ shape + NotePane.TABVALUE_SEPARATOR); // 6 shape
            sb.Append(    "tt="+ tt                                 ); // 7 tt
*/
            sb.Append(  "type="+ type  + NotePane.TABVALUE_SEPARATOR); // 0 type
            sb.Append( "xy_wh="+ xy_wh + NotePane.TABVALUE_SEPARATOR); // 1 xy_wh
            sb.Append( "shape="+ shape + NotePane.TABVALUE_SEPARATOR); // 2 shape
            sb.Append( "color="+ color + NotePane.TABVALUE_SEPARATOR); // 3 color
            sb.Append(  "zoom="+ zoom  + NotePane.TABVALUE_SEPARATOR); // 4 zoom
            sb.Append(   "tag="+ tag   + NotePane.TABVALUE_SEPARATOR); // 5 tag
            sb.Append(  "text="+ text  + NotePane.TABVALUE_SEPARATOR); // 6 text
            sb.Append(    "tt="+ tt                                 ); // 7 tt

            Settings.SaveSetting("TAB."+np.Name, sb.ToString());
        //  Settings.SaveSetting("TAB."+tab_name, sb.ToString());

            string rtf   = get_np_rtf  ( np );
            if(rtf != "")
                Settings.SaveSetting("TAB."+np.Name+".rtf", rtf.Replace("\r\n",""));


        }
        // }}}
        public  string export_user_tabs() // {{{
        {
        //  string tab_key_val_lines = "";
            sb.Clear();

            foreach(var item in tabs_Dictionary)
            {
                string tab_name = item.Key;
                NotePane np     = get_tab_NP(tab_name);
                if(    !np.is_empty()
                    && (np.Type != NotePane.TYPE_CONTROL)
                    && (np.Type != NotePane.TYPE_DASH)
                    && (np.Type != NotePane.TYPE_PANEL)
                    // (np.Type != NotePane.TYPE_RICHTEXT)
                    // (np.Type != NotePane.TYPE_SHORTCUT)
                  ) {
                //  tab_key_val_lines += get_tab_key_val_line( np )+"\n";
                    sb.Append(           get_tab_key_val_line( np )+"\n");
                }
            }
        //  return tab_key_val_lines;
            return sb.ToString();
        }
        // }}}
        private string get_tab_key_val_line(NotePane np) // {{{
        {
            // @see parse_TABS() in /LOCAL/DATA/ANDROID/PROJECTS/RTabs/app/src/main/java/ivanwfr/rtabs/RTabsClient.java

/* // {{{
//....NNNNNNNNNNNN=YYYYYYYYYYYYY TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT ZZZZZZ XXXXXXXXXXXXXXX TTTTTTTTTTTTTTTTT CCCCCC SSSSSSSSSSSS TT
//TAB.panel_usr011=type=SHORTCUT|tag=http://remotetabs.com/trulyergonomic/Profile_java.html|zoom=1|xy_wh=14,3,12,2|text=Profile_java|color=|shape=square|tt=
            sb.Clear();
            sb.Append("TAB."+np.Name+".type" +","+get_np_type ( np ) + NotePane.TABVALUE_SEPARATOR); // 0 type
            sb.Append("TAB."+np.Name+".tag"  +","+get_np_tag  ( np ) + NotePane.TABVALUE_SEPARATOR); // 1 tag
            sb.Append("TAB."+np.Name+".zoom" +","+get_np_zoom ( np ) + NotePane.TABVALUE_SEPARATOR); // 2 zoom
            sb.Append("TAB."+np.Name+".xy_wh"+","+get_np_xy_wh( np ) + NotePane.TABVALUE_SEPARATOR); // 3 xy_wh
            sb.Append("TAB."+np.Name+".text" +","+get_np_text ( np ) + NotePane.TABVALUE_SEPARATOR); // 4 text
            sb.Append("TAB."+np.Name+".color"+","+get_np_color( np ) + NotePane.TABVALUE_SEPARATOR); // 5 color
            sb.Append("TAB."+np.Name+".shape"+","+get_np_shape( np ) + NotePane.TABVALUE_SEPARATOR); // 6 shape
            sb.Append("TAB."+np.Name+".tt"   +","+get_np_tt   ( np )                              ); // 7 tt
*/ // }}}

// (160704) 
//TAB.panel_usr011=type=SHORTCUT|xy_wh=14,3,12,2|shape=square|color=|zoom=1|tag=http://remotetabs.com/trulyergonomic/Profile_java.html|text=Profile_java|tt=
//....NNNNNNNNNNNN=YYYYYYYYYYYYY XXXXXXXXXXXXXXX SSSSSSSSSSSS CCCCCC ZZZZZZ TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT TTTTTTTTTTTTTTTTT TT
            sb.Append("TAB."+np.Name+".type" +","+get_np_type ( np ) + NotePane.TABVALUE_SEPARATOR); // 0 type
            sb.Append("TAB."+np.Name+".xy_wh"+","+get_np_xy_wh( np ) + NotePane.TABVALUE_SEPARATOR); // 1 xy_wh
            sb.Append("TAB."+np.Name+".shape"+","+get_np_shape( np ) + NotePane.TABVALUE_SEPARATOR); // 2 shape
            sb.Append("TAB."+np.Name+".color"+","+get_np_color( np ) + NotePane.TABVALUE_SEPARATOR); // 3 color
            sb.Append("TAB."+np.Name+".zoom" +","+get_np_zoom ( np ) + NotePane.TABVALUE_SEPARATOR); // 4 zoom
            sb.Append("TAB."+np.Name+".tag"  +","+get_np_tag  ( np ) + NotePane.TABVALUE_SEPARATOR); // 5 tag
            sb.Append("TAB."+np.Name+".text" +","+get_np_text ( np ) + NotePane.TABVALUE_SEPARATOR); // 6 text
            sb.Append("TAB."+np.Name+".tt"   +","+get_np_tt   ( np )                              ); // 7 tt
/* // {{{
*/ // }}}
            return sb.ToString();
        }
        // }}}

        // NotePane properties
        private string get_np_type (NotePane np)// 0 {{{
        {
            string type = np.Type;

            return type;
        } // }}}
        private string get_np_tag  (NotePane np)// 1 {{{
        {
            string tag  = "";

            if(    (np.Tag  != null)
                && (np.Type != NotePane.TYPE_CONTROL)
                && (np.Type != NotePane.TYPE_DASH   )
                && (np.Type != NotePane.TYPE_PANEL  )
              )
                tag += NotePane.encode_text( np.Tag.ToString() );

            return tag;
        } // }}}
        private string get_np_zoom (NotePane np)// 2 {{{
        {
/*
        //  string zoom =  np.TextBox.ZoomFactor.ToString();
            string zoom = (np.TextBox.ZoomFactor / Settings.TXT_ZOOM).ToString();

            return zoom;
*/
            return np.zoom;
        } // }}}
        private string get_np_xy_wh(NotePane np)// 4 {{{
        {
            return np.xy_wh;
        }
        // }}}
        private string get_np_rtf  (NotePane np)// 5 {{{
        {
            string rtf  = "";

            if(    !np.is_empty()
                && (np.Type != NotePane.TYPE_CONTROL)
                && (np.Type != NotePane.TYPE_DASH)
                && (np.Type != NotePane.TYPE_PANEL)
                && (np.TextBox.Rtf != "")
              )
                rtf += np.TextBox.Rtf;

            return rtf;
        } // }}}
        private string get_np_text (NotePane np)// 3 {{{
        {
            string text  = "";

            if(    !np.is_empty()
                && (np.Type != NotePane.TYPE_CONTROL)
                && (np.Type != NotePane.TYPE_DASH   )
                && (np.Type != NotePane.TYPE_PANEL  )
                && (np.Text != "")
              )
                text += NotePane.encode_text( np.Text );

            return text;
        } // }}}
        private string get_np_color(NotePane np)// 2 {{{
        {
            return ""+np.color;
        } // }}}
        private string get_np_shape(NotePane np)// 2 {{{
        {
            return    np.shape;
        } // }}}
        private string get_np_tt   (NotePane np)// 3 {{{
        {
            string tt    = "";

            if(    !np.is_empty()
                && (np.Type != NotePane.TYPE_CONTROL)
                && (np.Type != NotePane.TYPE_DASH   )
                && (np.Type != NotePane.TYPE_PANEL  )
                && (np.Text != "")
              )
                tt += NotePane.encode_text( np._tt );

            return tt;
        } // }}}

        // UTIL
/*
        private void layout_tab_on_grid(NotePane np)// {{{
        {
            try{
                double dots_per_grid_cell
                    = ((  np.Type == NotePane.TYPE_SHORTCUT) || (  np.Type == NotePane.TYPE_RICHTEXT))
                    ?  TabsCollection.TAB_GRID_S * Settings.ratio
                    :  TabsCollection.TAB_GRID_S;

                string[] a = np.xy_wh.Split(',');
                int x      = (int)(int.Parse( a[0] ) * dots_per_grid_cell);
                int y      = (int)(int.Parse( a[1] ) * dots_per_grid_cell);
                int w      = (int)(int.Parse( a[2] ) * dots_per_grid_cell);
                int h      = (int)(int.Parse( a[3] ) * dots_per_grid_cell);

                if(x < 0) x = 0;
                if(y < 0) y = 0;

                Size   size = get_min_size( np );
                if(w < size.Width ) w = size.Width;
                if(h < size.Height) h = size.Height;

                x    = (int)Math.Floor(0.5 + ((double)x / dots_per_grid_cell));
                y    = (int)Math.Floor(0.5 + ((double)y / dots_per_grid_cell));
                w    = (int)Math.Floor(0.5 + ((double)w / dots_per_grid_cell));
                h    = (int)Math.Floor(0.5 + ((double)h / dots_per_grid_cell));

                np.xy_wh= String.Format("{0:D2},{1:D2},{2:D2},{3:D2}", x, y, w, h);
            }
            catch(Exception ex) { displayNotePane("layout_tab_on_grid np.xy_wh=["+ np.xy_wh +"]:", np, ex); }

        }
        // }}}
*/
        private void layout_tab_on_grid(NotePane np)// {{{
        {
            try {
                string[] a = np.xy_wh.Split(',');
                int x      = int.Parse( a[0] );
                int y      = int.Parse( a[1] );
                int w      = int.Parse( a[2] );
                int h      = int.Parse( a[3] );

                //if((x < 0) || (y < 0))
                //{
                    if(x < 0) x = 0;
                    if(y < 0) y = 0;
                    np.xy_wh= String.Format("{0:D2},{1:D2},{2:D2},{3:D2}", x, y, w, h);
                //}
            }
            catch(Exception ex) { displayNotePane("layout_tab_on_grid np.xy_wh=["+ np.xy_wh +"]:", np, ex); }

        }
        // }}}

        private void delete_extra_id(int first_obsolete_id) // {{{
        {
            // CLEANUP HIGHER OBSOLETE ENTRIES
            for(int i = first_obsolete_id; i < Settings.USER_TABS_MAX; ++i) {
                del_tab_settings(NotePane.PANEL_NAME_USR + i);
            }

        }
        // }}}
        private void del_tab_settings(string tab_name) //{{{
        {
            Settings.DeleteSetting("TAB."+tab_name       );
            Settings.DeleteSetting("TAB."+tab_name+".rtf");
/*
            Settings.DeleteSetting("TAB."+tab_name+".type" );
            Settings.DeleteSetting("TAB."+tab_name+".zoom" );
            Settings.DeleteSetting("TAB."+tab_name+".xy_wh");
            Settings.DeleteSetting("TAB."+tab_name+".tag"  );
            Settings.DeleteSetting("TAB."+tab_name+".text" );
            Settings.DeleteSetting("TAB."+tab_name+".color");
            Settings.DeleteSetting("TAB."+tab_name+".tt"   );
*/
        }
        // }}}

        // SETTINGS  - - - - - - - - - - - - - - - - - - - - - - - - - - - }}}
        // LOG {{{
        public override string ToString() //{{{
        {
            int c = 0, d = 0, p = 0, r = 0, s = 0;
            foreach(var item in tabs_Dictionary)
            {
                NotePane np = get_tab_NP( item.Key );
                if     (np.Type == NotePane.TYPE_CONTROL  ) c += 1;
                else if(np.Type == NotePane.TYPE_DASH     ) d += 1;
                else if(np.Type == NotePane.TYPE_PANEL    ) p += 1;
                else if(np.Type == NotePane.TYPE_RICHTEXT ) r += 1;
                else if(np.Type == NotePane.TYPE_SHORTCUT ) s += 1;
            }

            sb.Clear();
            sb.Append("Tabs x"+ tabs_Dictionary.Count +":\n");

            if(c>0) sb.Append(String.Format("{0,3} Controls\n", c));
            if(d>0) sb.Append(String.Format("{0,3} Dash\n"    , d));
            if(p>0) sb.Append(String.Format("{0,3} Panel\n"   , p));
            if(r>0) sb.Append(String.Format("{0,3} RichText\n", r));
            if(s>0) sb.Append(String.Format("{0,3} Shortcut\n", s));

            return sb.ToString();
        }
        //}}}
        private static void log(string msg)// {{{
        {
            Logger.Log(typeof(TabsCollection).Name, msg+"\n");
/*
            MessageBox.Show(typeof(NotePane).Name+":\n"
                + msg
                , "NotePane"
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information
                );

*/

        }
        // }}}
        private void displayNotePane(string msg, NotePane np, Exception ex) //{{{
        {
            string error = (ex == null) ? "" : Settings.ExToString(ex);

            MessageBox.Show(msg +"\n"
                + np.ToString() +"\n"
                + "get_saved_zoom()=["+ np.get_saved_zoom() +"]\n"
                + error
                , "TabsCollection"
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information
                );
        }
        //}}}
        //}}}
    }

}

