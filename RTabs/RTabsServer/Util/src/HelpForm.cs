// {{{
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Util;
// }}}
namespace Util
{
    public partial class HelpForm : Form, LoggerInterface
    {
        // HELP MEMBERS {{{ 

        private string  title;
        public  bool    closed;

        private Util.TabsCollection tabsCollection;
        private NotePane         control_close;
        private NotePane         panel_help;

        // }}}
        // CONSTRUCT {{{
        public HelpForm(string title)// {{{
        {
            InitializeComponent();
            tabsCollection  = new Util.TabsCollection(this, tabs_container);
            closed = false;
            this.title = title;

            // BUILTIN BUTTONS
            control_close = tabsCollection.update_control("Close");
            //tton_close        .MouseClick += new System.Windows.Forms.MouseEventHandler(this.control_close_Click);

            control_close.Left     = 0;
            control_close.Top      = 0;
            control_close.Width    = Width - 10;
            control_close.Height   = 80;
            control_close.Anchor   = ((System.Windows.Forms.AnchorStyles)
            (((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            control_close.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
        //  control_close.Rtf = @"{\rtf1\ansi\fs32\b Close Help\b0}";
            control_close.LabelPrefix = @"\fs32\b";

            // BUILTIN PANELS
            panel_help          = tabsCollection.update_panel("panel_help", "panel_help");
            panel_help.Left     = 0;
            panel_help.Top      = control_close.Height+10;
            panel_help.Width    = Width - 10;
            panel_help.Height   = panel_help.Parent.Height - control_close.Height - 10;;
            panel_help.Anchor   = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            panel_help.Location = new System.Drawing.Point(0, control_close.Height+5);
            panel_help.Padding = new System.Windows.Forms.Padding(10, 10, 50, 10);

            panel_help.BackColor    = Color.Yellow;
            panel_help.TextBox.Font = new System.Drawing.Font("Comic sans ms", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            panel_help.Width        = Width - 10;

        //  panel_help.Rtf          = @"{\rtf1\ansi\fs20\b " + panel_help.Name +@"\b0:\line}";
            control_close.LabelPrefix = @"\fs20\b";

        }

        // }}}
        //}}}
        // LOG {{{

        // has to implement ClientServerInterface ... but it is not really a good logger
        public string   get_APP_HELP_TEXT()             { return ""; }
        public string   get_APP_NAME()                  { return ""; }
        public string   get_APP_TITLE()                 { return ""; }
        public void     log(string caller, string msg)  {            }
        public void     set_logging(bool state)         {            }

        // ...just a callback receiver
        public void callback(System.Object caller, string detail)// {{{
        {
            if( this.Visible ) this.Hide();
        }
        // }}}
        public void notify(NotePane np, string detail)// {{{
        {
        }
        // }}}
        //}}}
        // EVENTS {{{
        private void control_close_Click(object sender, EventArgs e)// {{{
        {
            this.Hide();
        }
        // }}}
        private void OnLoad(object sender, EventArgs e)// {{{
        {
        }

        // }}}
        private void OnFormClosing(object sender, FormClosingEventArgs e)// {{{
        {
            closed = true;
        }

        // }}}
        //}}}
        // SETTINGS {{{
        public void setHelp(string title, string text)// {{{
        {
        //  panel_help.Rtf = @"{\rtf1\ansi\fs28\b "+ title +@"\b0:\line\line\fs24"+ text  +@"\line}";
            panel_help.LabelPrefix = @"\fs28\b";
            panel_help. TextPrefix = @"\fs24\i";
            panel_help.Label = title;
            panel_help.Text  = text.Replace("\n", @"\line");

        }
        // }}}
        //}}}
        // MOVE AND RESIZE Form {{{
        // variables {{{
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
        // }}}
        protected override void OnMouseUp(MouseEventArgs e)// {{{
        {
            base.OnMouseUp(e);
        //  mouseDown   = false;
            dragging    = false; 
            Refresh();
        //  ResumeLayout(false);
        }
        // }}}
        protected override void OnMouseDown(MouseEventArgs e)// {{{
        {
            base.OnMouseDown(e);
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

                Refresh();
            }

        }
        // }}}
        protected override void OnMouseMove(MouseEventArgs e)// {{{
        {
            base.OnMouseMove(e);
            if(!locked) {
                // update {{{
                if(dragging) {
                    Point p = PointToScreen(e.Location);

                    int                         dx = p.X - dragPoint.X;
                    int                         w  = Size.Width;
                    if(     resize_R)           w  = origin.Width  + dx;
                    else if(resize_L)           w  = origin.Width  - dx;
                    if(w < MinimumSize.Width) {
                        dx -= (MinimumSize.Width - w);
                        w   =  MinimumSize.Width;
                    }

                    int                         dy = p.Y - dragPoint.Y;     
                    int                         h  = Size.Height;
                    if(     resize_B)           h  = origin.Height + dy;
                    else if(resize_T)           h  = origin.Height - dy;
                    if(h < MinimumSize.Height) {
                        dy -= (MinimumSize.Height - h);
                        h   =  MinimumSize.Height;
                    }

                    if(moving || resize_T) Top  = origin.Y+dy;
                    if(moving || resize_L) Left = origin.X+dx;
                    Width                       = w;
                    Height                      = h;
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

        // }}}
        //}}}
    }
}
