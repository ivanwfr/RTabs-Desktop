using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public partial class TabTagForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.txt_input = new System.Windows.Forms.TextBox();
            this.txt_current = new System.Windows.Forms.TextBox();
            this.lbl_tab_name = new System.Windows.Forms.Label();
            this.btn_RUN = new System.Windows.Forms.Button();
            this.btn_SHELL = new System.Windows.Forms.Button();
            this.btn_SENDKEYS = new System.Windows.Forms.Button();
            this.combo_builtins = new System.Windows.Forms.ComboBox();
            this.combo_sk = new System.Windows.Forms.ComboBox();
            this.lbl_builtins = new System.Windows.Forms.Label();
            this.btn_PROFILE = new System.Windows.Forms.Button();
            this.btn_activate = new System.Windows.Forms.Button();
            this.sk_ENTER = new System.Windows.Forms.Button();
            this.sk_ESC = new System.Windows.Forms.Button();
            this.sk_TAB = new System.Windows.Forms.Button();
            this.sk_PGUP = new System.Windows.Forms.Button();
            this.sk_SUBTRACT = new System.Windows.Forms.Button();
            this.sk_HOME = new System.Windows.Forms.Button();
            this.sk_ADD = new System.Windows.Forms.Button();
            this.sk_F1 = new System.Windows.Forms.Button();
            this.sk_F2 = new System.Windows.Forms.Button();
            this.sk_F3 = new System.Windows.Forms.Button();
            this.sk_F4 = new System.Windows.Forms.Button();
            this.sk_F5 = new System.Windows.Forms.Button();
            this.sk_F6 = new System.Windows.Forms.Button();
            this.sk_F7 = new System.Windows.Forms.Button();
            this.sk_F8 = new System.Windows.Forms.Button();
            this.sk_F9 = new System.Windows.Forms.Button();
            this.sk_F10 = new System.Windows.Forms.Button();
            this.sk_F11 = new System.Windows.Forms.Button();
            this.sk_F12 = new System.Windows.Forms.Button();
            this.sk_F13 = new System.Windows.Forms.Button();
            this.sk_F14 = new System.Windows.Forms.Button();
            this.sk_F15 = new System.Windows.Forms.Button();
            this.sk_F16 = new System.Windows.Forms.Button();
            this.sk_END = new System.Windows.Forms.Button();
            this.sk_DIVIDE = new System.Windows.Forms.Button();
            this.sk_PGDN = new System.Windows.Forms.Button();
            this.sk_MULTIPLY = new System.Windows.Forms.Button();
            this.sk_UP = new System.Windows.Forms.Button();
            this.sk_RIGHT = new System.Windows.Forms.Button();
            this.sk_DOWN = new System.Windows.Forms.Button();
            this.sk_LEFT = new System.Windows.Forms.Button();
            this.sk_BACKSPACE = new System.Windows.Forms.Button();
            this.sk_BREAK = new System.Windows.Forms.Button();
            this.sk_DELETE = new System.Windows.Forms.Button();
            this.sk_INSERT = new System.Windows.Forms.Button();
            this.lbl_sel_count = new System.Windows.Forms.Label();
            this.btn_sel_clear = new System.Windows.Forms.Button();
            this.groupBox_colors = new System.Windows.Forms.GroupBox();
            this.radio_color00 = new System.Windows.Forms.RadioButton();
            this.radio_color01 = new System.Windows.Forms.RadioButton();
            this.radio_color02 = new System.Windows.Forms.RadioButton();
            this.radio_color03 = new System.Windows.Forms.RadioButton();
            this.radio_color04 = new System.Windows.Forms.RadioButton();
            this.radio_color05 = new System.Windows.Forms.RadioButton();
            this.radio_color06 = new System.Windows.Forms.RadioButton();
            this.radio_color07 = new System.Windows.Forms.RadioButton();
            this.radio_color08 = new System.Windows.Forms.RadioButton();
            this.radio_color09 = new System.Windows.Forms.RadioButton();
            this.radio_color10 = new System.Windows.Forms.RadioButton();
            this.radio_color11 = new System.Windows.Forms.RadioButton();
            this.lbl_INFO = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radio_shape_tile = new System.Windows.Forms.RadioButton();
            this.radio_shape_circle = new System.Windows.Forms.RadioButton();
            this.radio_shape_onedge = new System.Windows.Forms.RadioButton();
            this.radio_shape_padd_r = new System.Windows.Forms.RadioButton();
            this.radio_shape_square = new System.Windows.Forms.RadioButton();
            this.radio_shape_auto = new System.Windows.Forms.RadioButton();
            this.groupBox.SuspendLayout();
            this.groupBox_colors.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(176, 120);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 2;
            this.btn_ok.Text = "O&K";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            this.btn_ok.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // btn_cancel
            // 
            this.btn_cancel.BackColor = System.Drawing.Color.DimGray;
            this.btn_cancel.Location = new System.Drawing.Point(16, 120);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "C&ancel";
            this.btn_cancel.UseVisualStyleBackColor = false;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            this.btn_cancel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.BackColor = System.Drawing.Color.Black;
            this.groupBox.Controls.Add(this.txt_input);
            this.groupBox.Controls.Add(this.txt_current);
            this.groupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.groupBox.Location = new System.Drawing.Point(16, 40);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(981, 66);
            this.groupBox.TabIndex = 3;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Command";
            // 
            // txt_input
            // 
            this.txt_input.BackColor = System.Drawing.Color.Black;
            this.txt_input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_input.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txt_input.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_input.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.txt_input.Location = new System.Drawing.Point(3, 40);
            this.txt_input.Name = "txt_input";
            this.txt_input.Size = new System.Drawing.Size(975, 23);
            this.txt_input.TabIndex = 1;
            this.txt_input.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // txt_current
            // 
            this.txt_current.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txt_current.Dock = System.Windows.Forms.DockStyle.Top;
            this.txt_current.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_current.ForeColor = System.Drawing.Color.Silver;
            this.txt_current.Location = new System.Drawing.Point(3, 16);
            this.txt_current.Name = "txt_current";
            this.txt_current.ReadOnly = true;
            this.txt_current.Size = new System.Drawing.Size(975, 23);
            this.txt_current.TabIndex = 0;
            this.txt_current.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // lbl_tab_name
            // 
            this.lbl_tab_name.AutoSize = true;
            this.lbl_tab_name.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_tab_name.Location = new System.Drawing.Point(24, 16);
            this.lbl_tab_name.Name = "lbl_tab_name";
            this.lbl_tab_name.Size = new System.Drawing.Size(111, 19);
            this.lbl_tab_name.TabIndex = 4;
            this.lbl_tab_name.Text = "lbl_tab_name";
            // 
            // btn_RUN
            // 
            this.btn_RUN.BackColor = System.Drawing.Color.Black;
            this.btn_RUN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_RUN.Location = new System.Drawing.Point(16, 152);
            this.btn_RUN.Name = "btn_RUN";
            this.btn_RUN.Size = new System.Drawing.Size(75, 23);
            this.btn_RUN.TabIndex = 5;
            this.btn_RUN.Text = "RUN";
            this.btn_RUN.UseVisualStyleBackColor = false;
            this.btn_RUN.Click += new System.EventHandler(this.btn_builtin_Click);
            this.btn_RUN.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // btn_SHELL
            // 
            this.btn_SHELL.BackColor = System.Drawing.Color.Black;
            this.btn_SHELL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_SHELL.Location = new System.Drawing.Point(96, 152);
            this.btn_SHELL.Name = "btn_SHELL";
            this.btn_SHELL.Size = new System.Drawing.Size(75, 23);
            this.btn_SHELL.TabIndex = 6;
            this.btn_SHELL.Text = "SHELL";
            this.btn_SHELL.UseVisualStyleBackColor = false;
            this.btn_SHELL.Click += new System.EventHandler(this.btn_builtin_Click);
            this.btn_SHELL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // btn_SENDKEYS
            // 
            this.btn_SENDKEYS.BackColor = System.Drawing.Color.Black;
            this.btn_SENDKEYS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_SENDKEYS.Location = new System.Drawing.Point(504, 112);
            this.btn_SENDKEYS.Name = "btn_SENDKEYS";
            this.btn_SENDKEYS.Size = new System.Drawing.Size(80, 24);
            this.btn_SENDKEYS.TabIndex = 7;
            this.btn_SENDKEYS.Text = "SENDKEYS";
            this.btn_SENDKEYS.UseVisualStyleBackColor = false;
            this.btn_SENDKEYS.Click += new System.EventHandler(this.btn_builtin_Click);
            this.btn_SENDKEYS.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // combo_builtins
            // 
            this.combo_builtins.BackColor = System.Drawing.Color.Black;
            this.combo_builtins.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.combo_builtins.FormattingEnabled = true;
            this.combo_builtins.Location = new System.Drawing.Point(88, 184);
            this.combo_builtins.Name = "combo_builtins";
            this.combo_builtins.Size = new System.Drawing.Size(160, 22);
            this.combo_builtins.TabIndex = 8;
            this.combo_builtins.SelectedIndexChanged += new System.EventHandler(this.combo_builtins_SelectedIndexChanged);
            this.combo_builtins.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // combo_sk
            // 
            this.combo_sk.BackColor = System.Drawing.Color.Black;
            this.combo_sk.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_sk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.combo_sk.FormattingEnabled = true;
            this.combo_sk.Location = new System.Drawing.Point(520, 208);
            this.combo_sk.Name = "combo_sk";
            this.combo_sk.Size = new System.Drawing.Size(128, 20);
            this.combo_sk.TabIndex = 8;
            this.combo_sk.SelectedIndexChanged += new System.EventHandler(this.combo_sk_SelectedIndexChanged);
            this.combo_sk.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // lbl_builtins
            // 
            this.lbl_builtins.AutoSize = true;
            this.lbl_builtins.BackColor = System.Drawing.Color.Black;
            this.lbl_builtins.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_builtins.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbl_builtins.Location = new System.Drawing.Point(16, 184);
            this.lbl_builtins.Name = "lbl_builtins";
            this.lbl_builtins.Size = new System.Drawing.Size(52, 14);
            this.lbl_builtins.TabIndex = 9;
            this.lbl_builtins.Text = "Built-Ins";
            // 
            // btn_PROFILE
            // 
            this.btn_PROFILE.BackColor = System.Drawing.Color.Black;
            this.btn_PROFILE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_PROFILE.Location = new System.Drawing.Point(176, 152);
            this.btn_PROFILE.Name = "btn_PROFILE";
            this.btn_PROFILE.Size = new System.Drawing.Size(75, 23);
            this.btn_PROFILE.TabIndex = 7;
            this.btn_PROFILE.Text = "PROFILE";
            this.btn_PROFILE.UseVisualStyleBackColor = false;
            this.btn_PROFILE.Click += new System.EventHandler(this.btn_builtin_Click);
            this.btn_PROFILE.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // btn_activate
            // 
            this.btn_activate.Location = new System.Drawing.Point(96, 120);
            this.btn_activate.Name = "btn_activate";
            this.btn_activate.Size = new System.Drawing.Size(75, 23);
            this.btn_activate.TabIndex = 4;
            this.btn_activate.Text = "Ac&tivate";
            this.btn_activate.Click += new System.EventHandler(this.btn_activate_Click);
            this.btn_activate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_ENTER
            // 
            this.sk_ENTER.BackColor = System.Drawing.Color.Black;
            this.sk_ENTER.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_ENTER.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sk_ENTER.Location = new System.Drawing.Point(520, 136);
            this.sk_ENTER.Name = "sk_ENTER";
            this.sk_ENTER.Size = new System.Drawing.Size(64, 24);
            this.sk_ENTER.TabIndex = 12;
            this.sk_ENTER.Text = "{ENTER}";
            this.sk_ENTER.UseVisualStyleBackColor = false;
            this.sk_ENTER.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_ENTER.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_ESC
            // 
            this.sk_ESC.BackColor = System.Drawing.Color.Black;
            this.sk_ESC.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_ESC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sk_ESC.Location = new System.Drawing.Point(584, 136);
            this.sk_ESC.Name = "sk_ESC";
            this.sk_ESC.Size = new System.Drawing.Size(64, 24);
            this.sk_ESC.TabIndex = 13;
            this.sk_ESC.Text = "{ESC}";
            this.sk_ESC.UseVisualStyleBackColor = false;
            this.sk_ESC.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_ESC.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_TAB
            // 
            this.sk_TAB.BackColor = System.Drawing.Color.Black;
            this.sk_TAB.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_TAB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sk_TAB.Location = new System.Drawing.Point(520, 160);
            this.sk_TAB.Name = "sk_TAB";
            this.sk_TAB.Size = new System.Drawing.Size(64, 24);
            this.sk_TAB.TabIndex = 0;
            this.sk_TAB.Text = "{TAB}";
            this.sk_TAB.UseVisualStyleBackColor = false;
            this.sk_TAB.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_TAB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_PGUP
            // 
            this.sk_PGUP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(87)))), ((int)(((byte)(231)))));
            this.sk_PGUP.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_PGUP.Location = new System.Drawing.Point(760, 112);
            this.sk_PGUP.Name = "sk_PGUP";
            this.sk_PGUP.Size = new System.Drawing.Size(56, 24);
            this.sk_PGUP.TabIndex = 4;
            this.sk_PGUP.Text = "{PGUP}";
            this.sk_PGUP.UseVisualStyleBackColor = false;
            this.sk_PGUP.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_PGUP.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_SUBTRACT
            // 
            this.sk_SUBTRACT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(68)))));
            this.sk_SUBTRACT.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_SUBTRACT.Location = new System.Drawing.Point(896, 112);
            this.sk_SUBTRACT.Name = "sk_SUBTRACT";
            this.sk_SUBTRACT.Size = new System.Drawing.Size(88, 24);
            this.sk_SUBTRACT.TabIndex = 4;
            this.sk_SUBTRACT.Text = "{SUBTRACT}";
            this.sk_SUBTRACT.UseVisualStyleBackColor = false;
            this.sk_SUBTRACT.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_SUBTRACT.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_HOME
            // 
            this.sk_HOME.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(87)))), ((int)(((byte)(231)))));
            this.sk_HOME.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_HOME.Location = new System.Drawing.Point(760, 136);
            this.sk_HOME.Name = "sk_HOME";
            this.sk_HOME.Size = new System.Drawing.Size(56, 24);
            this.sk_HOME.TabIndex = 1;
            this.sk_HOME.Text = "{HOME}";
            this.sk_HOME.UseVisualStyleBackColor = false;
            this.sk_HOME.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_HOME.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_ADD
            // 
            this.sk_ADD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(68)))));
            this.sk_ADD.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_ADD.Location = new System.Drawing.Point(848, 112);
            this.sk_ADD.Name = "sk_ADD";
            this.sk_ADD.Size = new System.Drawing.Size(48, 24);
            this.sk_ADD.TabIndex = 1;
            this.sk_ADD.Text = "{ADD}";
            this.sk_ADD.UseVisualStyleBackColor = false;
            this.sk_ADD.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_ADD.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F1
            // 
            this.sk_F1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(45)))), ((int)(((byte)(32)))));
            this.sk_F1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F1.Location = new System.Drawing.Point(680, 184);
            this.sk_F1.Name = "sk_F1";
            this.sk_F1.Size = new System.Drawing.Size(32, 24);
            this.sk_F1.TabIndex = 1;
            this.sk_F1.Text = "{F1}";
            this.sk_F1.UseVisualStyleBackColor = false;
            this.sk_F1.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F2
            // 
            this.sk_F2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(45)))), ((int)(((byte)(32)))));
            this.sk_F2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F2.Location = new System.Drawing.Point(712, 184);
            this.sk_F2.Name = "sk_F2";
            this.sk_F2.Size = new System.Drawing.Size(32, 24);
            this.sk_F2.TabIndex = 1;
            this.sk_F2.Text = "{F2}";
            this.sk_F2.UseVisualStyleBackColor = false;
            this.sk_F2.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F3
            // 
            this.sk_F3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(45)))), ((int)(((byte)(32)))));
            this.sk_F3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F3.Location = new System.Drawing.Point(744, 184);
            this.sk_F3.Name = "sk_F3";
            this.sk_F3.Size = new System.Drawing.Size(32, 24);
            this.sk_F3.TabIndex = 1;
            this.sk_F3.Text = "{F3}";
            this.sk_F3.UseVisualStyleBackColor = false;
            this.sk_F3.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F4
            // 
            this.sk_F4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(45)))), ((int)(((byte)(32)))));
            this.sk_F4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F4.Location = new System.Drawing.Point(776, 184);
            this.sk_F4.Name = "sk_F4";
            this.sk_F4.Size = new System.Drawing.Size(32, 24);
            this.sk_F4.TabIndex = 1;
            this.sk_F4.Text = "{F4}";
            this.sk_F4.UseVisualStyleBackColor = false;
            this.sk_F4.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F5
            // 
            this.sk_F5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(0)))));
            this.sk_F5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F5.ForeColor = System.Drawing.Color.Black;
            this.sk_F5.Location = new System.Drawing.Point(680, 208);
            this.sk_F5.Name = "sk_F5";
            this.sk_F5.Size = new System.Drawing.Size(32, 24);
            this.sk_F5.TabIndex = 1;
            this.sk_F5.Text = "{F5}";
            this.sk_F5.UseVisualStyleBackColor = false;
            this.sk_F5.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F5.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F6
            // 
            this.sk_F6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(0)))));
            this.sk_F6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F6.ForeColor = System.Drawing.Color.Black;
            this.sk_F6.Location = new System.Drawing.Point(712, 208);
            this.sk_F6.Name = "sk_F6";
            this.sk_F6.Size = new System.Drawing.Size(32, 24);
            this.sk_F6.TabIndex = 1;
            this.sk_F6.Text = "{F6}";
            this.sk_F6.UseVisualStyleBackColor = false;
            this.sk_F6.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F6.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F7
            // 
            this.sk_F7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(0)))));
            this.sk_F7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F7.ForeColor = System.Drawing.Color.Black;
            this.sk_F7.Location = new System.Drawing.Point(744, 208);
            this.sk_F7.Name = "sk_F7";
            this.sk_F7.Size = new System.Drawing.Size(32, 24);
            this.sk_F7.TabIndex = 1;
            this.sk_F7.Text = "{F7}";
            this.sk_F7.UseVisualStyleBackColor = false;
            this.sk_F7.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F7.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F8
            // 
            this.sk_F8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(0)))));
            this.sk_F8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F8.ForeColor = System.Drawing.Color.Black;
            this.sk_F8.Location = new System.Drawing.Point(776, 208);
            this.sk_F8.Name = "sk_F8";
            this.sk_F8.Size = new System.Drawing.Size(32, 24);
            this.sk_F8.TabIndex = 1;
            this.sk_F8.Text = "{F8}";
            this.sk_F8.UseVisualStyleBackColor = false;
            this.sk_F8.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F8.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F9
            // 
            this.sk_F9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(45)))), ((int)(((byte)(32)))));
            this.sk_F9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F9.Location = new System.Drawing.Point(824, 184);
            this.sk_F9.Name = "sk_F9";
            this.sk_F9.Size = new System.Drawing.Size(40, 24);
            this.sk_F9.TabIndex = 1;
            this.sk_F9.Text = "{F9}";
            this.sk_F9.UseVisualStyleBackColor = false;
            this.sk_F9.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F9.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F10
            // 
            this.sk_F10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(45)))), ((int)(((byte)(32)))));
            this.sk_F10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F10.Location = new System.Drawing.Point(864, 184);
            this.sk_F10.Name = "sk_F10";
            this.sk_F10.Size = new System.Drawing.Size(40, 24);
            this.sk_F10.TabIndex = 1;
            this.sk_F10.Text = "{F10}";
            this.sk_F10.UseVisualStyleBackColor = false;
            this.sk_F10.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F10.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F11
            // 
            this.sk_F11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(45)))), ((int)(((byte)(32)))));
            this.sk_F11.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F11.Location = new System.Drawing.Point(904, 184);
            this.sk_F11.Name = "sk_F11";
            this.sk_F11.Size = new System.Drawing.Size(40, 24);
            this.sk_F11.TabIndex = 1;
            this.sk_F11.Text = "{F11}";
            this.sk_F11.UseVisualStyleBackColor = false;
            this.sk_F11.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F11.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F12
            // 
            this.sk_F12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(45)))), ((int)(((byte)(32)))));
            this.sk_F12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F12.Location = new System.Drawing.Point(944, 184);
            this.sk_F12.Name = "sk_F12";
            this.sk_F12.Size = new System.Drawing.Size(40, 24);
            this.sk_F12.TabIndex = 1;
            this.sk_F12.Text = "{F12}";
            this.sk_F12.UseVisualStyleBackColor = false;
            this.sk_F12.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F12.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F13
            // 
            this.sk_F13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(0)))));
            this.sk_F13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F13.ForeColor = System.Drawing.Color.Black;
            this.sk_F13.Location = new System.Drawing.Point(944, 208);
            this.sk_F13.Name = "sk_F13";
            this.sk_F13.Size = new System.Drawing.Size(40, 24);
            this.sk_F13.TabIndex = 1;
            this.sk_F13.Text = "{F13}";
            this.sk_F13.UseVisualStyleBackColor = false;
            this.sk_F13.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F13.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F14
            // 
            this.sk_F14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(0)))));
            this.sk_F14.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F14.ForeColor = System.Drawing.Color.Black;
            this.sk_F14.Location = new System.Drawing.Point(904, 208);
            this.sk_F14.Name = "sk_F14";
            this.sk_F14.Size = new System.Drawing.Size(40, 24);
            this.sk_F14.TabIndex = 1;
            this.sk_F14.Text = "{F14}";
            this.sk_F14.UseVisualStyleBackColor = false;
            this.sk_F14.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F14.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F15
            // 
            this.sk_F15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(0)))));
            this.sk_F15.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F15.ForeColor = System.Drawing.Color.Black;
            this.sk_F15.Location = new System.Drawing.Point(864, 208);
            this.sk_F15.Name = "sk_F15";
            this.sk_F15.Size = new System.Drawing.Size(40, 24);
            this.sk_F15.TabIndex = 1;
            this.sk_F15.Text = "{F15}";
            this.sk_F15.UseVisualStyleBackColor = false;
            this.sk_F15.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F15.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_F16
            // 
            this.sk_F16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(0)))));
            this.sk_F16.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_F16.ForeColor = System.Drawing.Color.Black;
            this.sk_F16.Location = new System.Drawing.Point(824, 208);
            this.sk_F16.Name = "sk_F16";
            this.sk_F16.Size = new System.Drawing.Size(40, 24);
            this.sk_F16.TabIndex = 1;
            this.sk_F16.Text = "{F16}";
            this.sk_F16.UseVisualStyleBackColor = false;
            this.sk_F16.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_F16.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_END
            // 
            this.sk_END.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(87)))), ((int)(((byte)(231)))));
            this.sk_END.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_END.Location = new System.Drawing.Point(816, 136);
            this.sk_END.Name = "sk_END";
            this.sk_END.Size = new System.Drawing.Size(56, 24);
            this.sk_END.TabIndex = 2;
            this.sk_END.Text = "{END}";
            this.sk_END.UseVisualStyleBackColor = false;
            this.sk_END.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_END.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_DIVIDE
            // 
            this.sk_DIVIDE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(68)))));
            this.sk_DIVIDE.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_DIVIDE.Location = new System.Drawing.Point(920, 160);
            this.sk_DIVIDE.Name = "sk_DIVIDE";
            this.sk_DIVIDE.Size = new System.Drawing.Size(64, 24);
            this.sk_DIVIDE.TabIndex = 2;
            this.sk_DIVIDE.Text = "{DIVIDE}";
            this.sk_DIVIDE.UseVisualStyleBackColor = false;
            this.sk_DIVIDE.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_DIVIDE.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_PGDN
            // 
            this.sk_PGDN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(87)))), ((int)(((byte)(231)))));
            this.sk_PGDN.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_PGDN.Location = new System.Drawing.Point(760, 160);
            this.sk_PGDN.Name = "sk_PGDN";
            this.sk_PGDN.Size = new System.Drawing.Size(56, 24);
            this.sk_PGDN.TabIndex = 3;
            this.sk_PGDN.Text = "{PGDN}";
            this.sk_PGDN.UseVisualStyleBackColor = false;
            this.sk_PGDN.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_PGDN.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_MULTIPLY
            // 
            this.sk_MULTIPLY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(68)))));
            this.sk_MULTIPLY.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_MULTIPLY.Location = new System.Drawing.Point(848, 160);
            this.sk_MULTIPLY.Name = "sk_MULTIPLY";
            this.sk_MULTIPLY.Size = new System.Drawing.Size(72, 24);
            this.sk_MULTIPLY.TabIndex = 3;
            this.sk_MULTIPLY.Text = "{MULTIPLY}";
            this.sk_MULTIPLY.UseVisualStyleBackColor = false;
            this.sk_MULTIPLY.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_MULTIPLY.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_UP
            // 
            this.sk_UP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(87)))), ((int)(((byte)(231)))));
            this.sk_UP.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_UP.Location = new System.Drawing.Point(712, 112);
            this.sk_UP.Name = "sk_UP";
            this.sk_UP.Size = new System.Drawing.Size(48, 24);
            this.sk_UP.TabIndex = 14;
            this.sk_UP.Text = "{UP}";
            this.sk_UP.UseVisualStyleBackColor = false;
            this.sk_UP.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_UP.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_RIGHT
            // 
            this.sk_RIGHT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(87)))), ((int)(((byte)(231)))));
            this.sk_RIGHT.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_RIGHT.Location = new System.Drawing.Point(712, 136);
            this.sk_RIGHT.Name = "sk_RIGHT";
            this.sk_RIGHT.Size = new System.Drawing.Size(48, 24);
            this.sk_RIGHT.TabIndex = 15;
            this.sk_RIGHT.Text = "{RIGHT}";
            this.sk_RIGHT.UseVisualStyleBackColor = false;
            this.sk_RIGHT.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_RIGHT.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_DOWN
            // 
            this.sk_DOWN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(87)))), ((int)(((byte)(231)))));
            this.sk_DOWN.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_DOWN.Location = new System.Drawing.Point(712, 160);
            this.sk_DOWN.Name = "sk_DOWN";
            this.sk_DOWN.Size = new System.Drawing.Size(48, 24);
            this.sk_DOWN.TabIndex = 16;
            this.sk_DOWN.Text = "{DOWN}";
            this.sk_DOWN.UseVisualStyleBackColor = false;
            this.sk_DOWN.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_DOWN.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_LEFT
            // 
            this.sk_LEFT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(87)))), ((int)(((byte)(231)))));
            this.sk_LEFT.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_LEFT.Location = new System.Drawing.Point(664, 136);
            this.sk_LEFT.Name = "sk_LEFT";
            this.sk_LEFT.Size = new System.Drawing.Size(48, 24);
            this.sk_LEFT.TabIndex = 17;
            this.sk_LEFT.Text = "{LEFT}";
            this.sk_LEFT.UseVisualStyleBackColor = false;
            this.sk_LEFT.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_LEFT.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_BACKSPACE
            // 
            this.sk_BACKSPACE.BackColor = System.Drawing.Color.Black;
            this.sk_BACKSPACE.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_BACKSPACE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sk_BACKSPACE.Location = new System.Drawing.Point(584, 112);
            this.sk_BACKSPACE.Name = "sk_BACKSPACE";
            this.sk_BACKSPACE.Size = new System.Drawing.Size(80, 24);
            this.sk_BACKSPACE.TabIndex = 0;
            this.sk_BACKSPACE.Text = "{BACKSPACE}";
            this.sk_BACKSPACE.UseVisualStyleBackColor = false;
            this.sk_BACKSPACE.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_BACKSPACE.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_BREAK
            // 
            this.sk_BREAK.BackColor = System.Drawing.Color.Black;
            this.sk_BREAK.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_BREAK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sk_BREAK.Location = new System.Drawing.Point(584, 160);
            this.sk_BREAK.Name = "sk_BREAK";
            this.sk_BREAK.Size = new System.Drawing.Size(64, 24);
            this.sk_BREAK.TabIndex = 0;
            this.sk_BREAK.Text = "{BREAK}";
            this.sk_BREAK.UseVisualStyleBackColor = false;
            this.sk_BREAK.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_BREAK.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_DELETE
            // 
            this.sk_DELETE.BackColor = System.Drawing.Color.Black;
            this.sk_DELETE.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_DELETE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sk_DELETE.Location = new System.Drawing.Point(584, 184);
            this.sk_DELETE.Name = "sk_DELETE";
            this.sk_DELETE.Size = new System.Drawing.Size(64, 24);
            this.sk_DELETE.TabIndex = 0;
            this.sk_DELETE.Text = "{DEL}";
            this.sk_DELETE.UseVisualStyleBackColor = false;
            this.sk_DELETE.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_DELETE.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // sk_INSERT
            // 
            this.sk_INSERT.BackColor = System.Drawing.Color.Black;
            this.sk_INSERT.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sk_INSERT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sk_INSERT.Location = new System.Drawing.Point(520, 184);
            this.sk_INSERT.Name = "sk_INSERT";
            this.sk_INSERT.Size = new System.Drawing.Size(64, 24);
            this.sk_INSERT.TabIndex = 0;
            this.sk_INSERT.Text = "{INSERT}";
            this.sk_INSERT.UseVisualStyleBackColor = false;
            this.sk_INSERT.Click += new System.EventHandler(this.btn_sk_Click);
            this.sk_INSERT.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // lbl_sel_count
            // 
            this.lbl_sel_count.AutoSize = true;
            this.lbl_sel_count.BackColor = System.Drawing.Color.Transparent;
            this.lbl_sel_count.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbl_sel_count.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_sel_count.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbl_sel_count.Location = new System.Drawing.Point(552, 0);
            this.lbl_sel_count.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_sel_count.Name = "lbl_sel_count";
            this.lbl_sel_count.Size = new System.Drawing.Size(35, 37);
            this.lbl_sel_count.TabIndex = 18;
            this.lbl_sel_count.Text = "0";
            this.lbl_sel_count.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_sel_clear
            // 
            this.btn_sel_clear.BackColor = System.Drawing.Color.Black;
            this.btn_sel_clear.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_sel_clear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_sel_clear.Location = new System.Drawing.Point(248, 0);
            this.btn_sel_clear.Name = "btn_sel_clear";
            this.btn_sel_clear.Size = new System.Drawing.Size(280, 40);
            this.btn_sel_clear.TabIndex = 19;
            this.btn_sel_clear.Text = "Clear selection";
            this.btn_sel_clear.UseVisualStyleBackColor = false;
            this.btn_sel_clear.Click += new System.EventHandler(this.btn_sel_clear_Click);
            this.btn_sel_clear.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // groupBox_colors
            // 
            this.groupBox_colors.Controls.Add(this.radio_color00);
            this.groupBox_colors.Controls.Add(this.radio_color01);
            this.groupBox_colors.Controls.Add(this.radio_color02);
            this.groupBox_colors.Controls.Add(this.radio_color03);
            this.groupBox_colors.Controls.Add(this.radio_color04);
            this.groupBox_colors.Controls.Add(this.radio_color05);
            this.groupBox_colors.Controls.Add(this.radio_color06);
            this.groupBox_colors.Controls.Add(this.radio_color07);
            this.groupBox_colors.Controls.Add(this.radio_color08);
            this.groupBox_colors.Controls.Add(this.radio_color09);
            this.groupBox_colors.Controls.Add(this.radio_color10);
            this.groupBox_colors.Controls.Add(this.radio_color11);
            this.groupBox_colors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox_colors.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox_colors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.groupBox_colors.Location = new System.Drawing.Point(512, 232);
            this.groupBox_colors.Name = "groupBox_colors";
            this.groupBox_colors.Size = new System.Drawing.Size(472, 40);
            this.groupBox_colors.TabIndex = 24;
            this.groupBox_colors.TabStop = false;
            this.groupBox_colors.Text = "color #";
            // 
            // radio_color00
            // 
            this.radio_color00.AutoSize = true;
            this.radio_color00.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color00.Location = new System.Drawing.Point(8, 16);
            this.radio_color00.Name = "radio_color00";
            this.radio_color00.Size = new System.Drawing.Size(31, 18);
            this.radio_color00.TabIndex = 11;
            this.radio_color00.TabStop = true;
            this.radio_color00.Text = "0";
            this.radio_color00.UseVisualStyleBackColor = true;
            this.radio_color00.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // radio_color01
            // 
            this.radio_color01.AutoSize = true;
            this.radio_color01.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color01.Location = new System.Drawing.Point(45, 16);
            this.radio_color01.Name = "radio_color01";
            this.radio_color01.Size = new System.Drawing.Size(31, 18);
            this.radio_color01.TabIndex = 12;
            this.radio_color01.TabStop = true;
            this.radio_color01.Text = "1";
            this.radio_color01.UseVisualStyleBackColor = true;
            this.radio_color01.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // radio_color02
            // 
            this.radio_color02.AutoSize = true;
            this.radio_color02.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color02.Location = new System.Drawing.Point(82, 16);
            this.radio_color02.Name = "radio_color02";
            this.radio_color02.Size = new System.Drawing.Size(31, 18);
            this.radio_color02.TabIndex = 26;
            this.radio_color02.TabStop = true;
            this.radio_color02.Text = "2";
            this.radio_color02.UseVisualStyleBackColor = true;
            this.radio_color02.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // radio_color03
            // 
            this.radio_color03.AutoSize = true;
            this.radio_color03.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color03.Location = new System.Drawing.Point(119, 16);
            this.radio_color03.Name = "radio_color03";
            this.radio_color03.Size = new System.Drawing.Size(31, 18);
            this.radio_color03.TabIndex = 27;
            this.radio_color03.TabStop = true;
            this.radio_color03.Text = "3";
            this.radio_color03.UseVisualStyleBackColor = true;
            this.radio_color03.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // radio_color04
            // 
            this.radio_color04.AutoSize = true;
            this.radio_color04.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color04.Location = new System.Drawing.Point(156, 16);
            this.radio_color04.Name = "radio_color04";
            this.radio_color04.Size = new System.Drawing.Size(31, 18);
            this.radio_color04.TabIndex = 29;
            this.radio_color04.TabStop = true;
            this.radio_color04.Text = "4";
            this.radio_color04.UseVisualStyleBackColor = true;
            this.radio_color04.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // radio_color05
            // 
            this.radio_color05.AutoSize = true;
            this.radio_color05.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color05.Location = new System.Drawing.Point(193, 16);
            this.radio_color05.Name = "radio_color05";
            this.radio_color05.Size = new System.Drawing.Size(31, 18);
            this.radio_color05.TabIndex = 30;
            this.radio_color05.TabStop = true;
            this.radio_color05.Text = "5";
            this.radio_color05.UseVisualStyleBackColor = true;
            this.radio_color05.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // radio_color06
            // 
            this.radio_color06.AutoSize = true;
            this.radio_color06.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color06.Location = new System.Drawing.Point(232, 16);
            this.radio_color06.Name = "radio_color06";
            this.radio_color06.Size = new System.Drawing.Size(31, 18);
            this.radio_color06.TabIndex = 34;
            this.radio_color06.TabStop = true;
            this.radio_color06.Text = "6";
            this.radio_color06.UseVisualStyleBackColor = true;
            this.radio_color06.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // radio_color07
            // 
            this.radio_color07.AutoSize = true;
            this.radio_color07.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color07.Location = new System.Drawing.Point(269, 16);
            this.radio_color07.Name = "radio_color07";
            this.radio_color07.Size = new System.Drawing.Size(31, 18);
            this.radio_color07.TabIndex = 31;
            this.radio_color07.TabStop = true;
            this.radio_color07.Text = "7";
            this.radio_color07.UseVisualStyleBackColor = true;
            this.radio_color07.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // radio_color08
            // 
            this.radio_color08.AutoSize = true;
            this.radio_color08.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color08.Location = new System.Drawing.Point(306, 16);
            this.radio_color08.Name = "radio_color08";
            this.radio_color08.Size = new System.Drawing.Size(31, 18);
            this.radio_color08.TabIndex = 32;
            this.radio_color08.TabStop = true;
            this.radio_color08.Text = "8";
            this.radio_color08.UseVisualStyleBackColor = true;
            this.radio_color08.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // radio_color09
            // 
            this.radio_color09.AutoSize = true;
            this.radio_color09.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color09.Location = new System.Drawing.Point(343, 16);
            this.radio_color09.Name = "radio_color09";
            this.radio_color09.Size = new System.Drawing.Size(31, 18);
            this.radio_color09.TabIndex = 33;
            this.radio_color09.TabStop = true;
            this.radio_color09.Text = "9";
            this.radio_color09.UseVisualStyleBackColor = true;
            this.radio_color09.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // radio_color10
            // 
            this.radio_color10.AutoSize = true;
            this.radio_color10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color10.Location = new System.Drawing.Point(380, 16);
            this.radio_color10.Name = "radio_color10";
            this.radio_color10.Size = new System.Drawing.Size(37, 18);
            this.radio_color10.TabIndex = 35;
            this.radio_color10.TabStop = true;
            this.radio_color10.Text = "10";
            this.radio_color10.UseVisualStyleBackColor = true;
            this.radio_color10.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // radio_color11
            // 
            this.radio_color11.AutoSize = true;
            this.radio_color11.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_color11.Location = new System.Drawing.Point(417, 16);
            this.radio_color11.Name = "radio_color11";
            this.radio_color11.Size = new System.Drawing.Size(36, 18);
            this.radio_color11.TabIndex = 36;
            this.radio_color11.TabStop = true;
            this.radio_color11.Text = "11";
            this.radio_color11.UseVisualStyleBackColor = true;
            this.radio_color11.CheckedChanged += new System.EventHandler(this.radio_color_CheckedChanged);
            // 
            // lbl_INFO
            // 
            this.lbl_INFO.AutoSize = true;
            this.lbl_INFO.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_INFO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbl_INFO.Location = new System.Drawing.Point(176, 0);
            this.lbl_INFO.Name = "lbl_INFO";
            this.lbl_INFO.Size = new System.Drawing.Size(37, 37);
            this.lbl_INFO.TabIndex = 25;
            this.lbl_INFO.Text = "?";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radio_shape_tile);
            this.groupBox1.Controls.Add(this.radio_shape_circle);
            this.groupBox1.Controls.Add(this.radio_shape_onedge);
            this.groupBox1.Controls.Add(this.radio_shape_padd_r);
            this.groupBox1.Controls.Add(this.radio_shape_square);
            this.groupBox1.Controls.Add(this.radio_shape_auto);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.groupBox1.Location = new System.Drawing.Point(16, 216);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 56);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "shape";
            // 
            // radio_shape_tile
            // 
            this.radio_shape_tile.AutoSize = true;
            this.radio_shape_tile.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_shape_tile.Location = new System.Drawing.Point(16, 32);
            this.radio_shape_tile.Name = "radio_shape_tile";
            this.radio_shape_tile.Size = new System.Drawing.Size(38, 18);
            this.radio_shape_tile.TabIndex = 30;
            this.radio_shape_tile.TabStop = true;
            this.radio_shape_tile.Text = "tile";
            this.radio_shape_tile.UseVisualStyleBackColor = true;
            this.radio_shape_tile.CheckedChanged += new System.EventHandler(this.radio_shape_CheckedChanged);
            // 
            // radio_shape_circle
            // 
            this.radio_shape_circle.AutoSize = true;
            this.radio_shape_circle.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_shape_circle.Location = new System.Drawing.Point(88, 16);
            this.radio_shape_circle.Name = "radio_shape_circle";
            this.radio_shape_circle.Size = new System.Drawing.Size(51, 18);
            this.radio_shape_circle.TabIndex = 12;
            this.radio_shape_circle.TabStop = true;
            this.radio_shape_circle.Text = "circle";
            this.radio_shape_circle.UseVisualStyleBackColor = true;
            this.radio_shape_circle.CheckedChanged += new System.EventHandler(this.radio_shape_CheckedChanged);
            // 
            // radio_shape_onedge
            // 
            this.radio_shape_onedge.AutoSize = true;
            this.radio_shape_onedge.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_shape_onedge.Location = new System.Drawing.Point(88, 32);
            this.radio_shape_onedge.Name = "radio_shape_onedge";
            this.radio_shape_onedge.Size = new System.Drawing.Size(61, 18);
            this.radio_shape_onedge.TabIndex = 26;
            this.radio_shape_onedge.TabStop = true;
            this.radio_shape_onedge.Text = "onedge";
            this.radio_shape_onedge.UseVisualStyleBackColor = true;
            this.radio_shape_onedge.CheckedChanged += new System.EventHandler(this.radio_shape_CheckedChanged);
            // 
            // radio_shape_padd_r
            // 
            this.radio_shape_padd_r.AutoSize = true;
            this.radio_shape_padd_r.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_shape_padd_r.Location = new System.Drawing.Point(160, 32);
            this.radio_shape_padd_r.Name = "radio_shape_padd_r";
            this.radio_shape_padd_r.Size = new System.Drawing.Size(59, 18);
            this.radio_shape_padd_r.TabIndex = 27;
            this.radio_shape_padd_r.TabStop = true;
            this.radio_shape_padd_r.Text = "padd_r";
            this.radio_shape_padd_r.UseVisualStyleBackColor = true;
            this.radio_shape_padd_r.CheckedChanged += new System.EventHandler(this.radio_shape_CheckedChanged);
            // 
            // radio_shape_square
            // 
            this.radio_shape_square.AutoSize = true;
            this.radio_shape_square.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_shape_square.Location = new System.Drawing.Point(160, 16);
            this.radio_shape_square.Name = "radio_shape_square";
            this.radio_shape_square.Size = new System.Drawing.Size(59, 18);
            this.radio_shape_square.TabIndex = 29;
            this.radio_shape_square.TabStop = true;
            this.radio_shape_square.Text = "square";
            this.radio_shape_square.UseVisualStyleBackColor = true;
            this.radio_shape_square.CheckedChanged += new System.EventHandler(this.radio_shape_CheckedChanged);
            // 
            // radio_shape_auto
            // 
            this.radio_shape_auto.AutoSize = true;
            this.radio_shape_auto.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_shape_auto.Location = new System.Drawing.Point(16, 16);
            this.radio_shape_auto.Name = "radio_shape_auto";
            this.radio_shape_auto.Size = new System.Drawing.Size(58, 18);
            this.radio_shape_auto.TabIndex = 30;
            this.radio_shape_auto.TabStop = true;
            this.radio_shape_auto.Text = "<auto>";
            this.radio_shape_auto.UseVisualStyleBackColor = true;
            this.radio_shape_auto.CheckedChanged += new System.EventHandler(this.radio_shape_CheckedChanged);
            // 
            // TabTagForm
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1000, 285);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbl_INFO);
            this.Controls.Add(this.groupBox_colors);
            this.Controls.Add(this.btn_sel_clear);
            this.Controls.Add(this.lbl_sel_count);
            this.Controls.Add(this.sk_TAB);
            this.Controls.Add(this.sk_HOME);
            this.Controls.Add(this.sk_ADD);
            this.Controls.Add(this.sk_F1);
            this.Controls.Add(this.sk_F2);
            this.Controls.Add(this.sk_F3);
            this.Controls.Add(this.sk_F4);
            this.Controls.Add(this.sk_F5);
            this.Controls.Add(this.sk_F6);
            this.Controls.Add(this.sk_F7);
            this.Controls.Add(this.sk_F8);
            this.Controls.Add(this.sk_F9);
            this.Controls.Add(this.sk_F10);
            this.Controls.Add(this.sk_F11);
            this.Controls.Add(this.sk_F12);
            this.Controls.Add(this.sk_F13);
            this.Controls.Add(this.sk_F14);
            this.Controls.Add(this.sk_F15);
            this.Controls.Add(this.sk_F16);
            this.Controls.Add(this.sk_END);
            this.Controls.Add(this.sk_DIVIDE);
            this.Controls.Add(this.sk_PGDN);
            this.Controls.Add(this.sk_MULTIPLY);
            this.Controls.Add(this.sk_PGUP);
            this.Controls.Add(this.sk_SUBTRACT);
            this.Controls.Add(this.sk_BACKSPACE);
            this.Controls.Add(this.sk_BREAK);
            this.Controls.Add(this.sk_DELETE);
            this.Controls.Add(this.sk_INSERT);
            this.Controls.Add(this.sk_ESC);
            this.Controls.Add(this.sk_ENTER);
            this.Controls.Add(this.sk_UP);
            this.Controls.Add(this.sk_RIGHT);
            this.Controls.Add(this.sk_DOWN);
            this.Controls.Add(this.sk_LEFT);
            this.Controls.Add(this.btn_activate);
            this.Controls.Add(this.btn_PROFILE);
            this.Controls.Add(this.lbl_builtins);
            this.Controls.Add(this.combo_builtins);
            this.Controls.Add(this.combo_sk);
            this.Controls.Add(this.btn_SENDKEYS);
            this.Controls.Add(this.btn_SHELL);
            this.Controls.Add(this.btn_RUN);
            this.Controls.Add(this.lbl_tab_name);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.LightGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1000, 285);
            this.Name = "TabTagForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "RTabs Input";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.DialogBox_Focus);
            this.Load += new System.EventHandler(this.DialogBox_Focus);
            this.Enter += new System.EventHandler(this.DialogBox_Focus);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.groupBox_colors.ResumeLayout(false);
            this.groupBox_colors.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.TextBox txt_input;
        private System.Windows.Forms.TextBox txt_current;
        private System.Windows.Forms.Label lbl_tab_name;
        private System.Windows.Forms.Button btn_RUN;
        private System.Windows.Forms.Button btn_SHELL;
        private System.Windows.Forms.Button btn_SENDKEYS;
        private System.Windows.Forms.ComboBox combo_builtins;
        private System.Windows.Forms.ComboBox combo_sk;
        private System.Windows.Forms.Label lbl_builtins;
        private System.Windows.Forms.Button btn_PROFILE;
        private System.Windows.Forms.Button btn_activate;
        private System.Windows.Forms.Button sk_ENTER;
        private System.Windows.Forms.Button sk_ESC;
        private System.Windows.Forms.Button sk_TAB;
        private System.Windows.Forms.Button sk_HOME;
        private System.Windows.Forms.Button sk_ADD;
        private System.Windows.Forms.Button sk_F1;
        private System.Windows.Forms.Button sk_F2;
        private System.Windows.Forms.Button sk_F3;
        private System.Windows.Forms.Button sk_F4;
        private System.Windows.Forms.Button sk_F5;
        private System.Windows.Forms.Button sk_F6;
        private System.Windows.Forms.Button sk_F7;
        private System.Windows.Forms.Button sk_F8;
        private System.Windows.Forms.Button sk_F9;
        private System.Windows.Forms.Button sk_F10;
        private System.Windows.Forms.Button sk_F11;
        private System.Windows.Forms.Button sk_F12;
        private System.Windows.Forms.Button sk_F13;
        private System.Windows.Forms.Button sk_F14;
        private System.Windows.Forms.Button sk_F15;
        private System.Windows.Forms.Button sk_F16;
        private System.Windows.Forms.Button sk_END;
        private System.Windows.Forms.Button sk_DIVIDE;
        private System.Windows.Forms.Button sk_PGDN;
        private System.Windows.Forms.Button sk_MULTIPLY;
        private System.Windows.Forms.Button sk_PGUP;
        private System.Windows.Forms.Button sk_SUBTRACT;
        private System.Windows.Forms.Button sk_UP;
        private System.Windows.Forms.Button sk_RIGHT;
        private System.Windows.Forms.Button sk_DOWN;
        private System.Windows.Forms.Button sk_LEFT;
        private System.Windows.Forms.Button sk_BACKSPACE;
        private System.Windows.Forms.Button sk_BREAK;
        private System.Windows.Forms.Button sk_DELETE;
        private System.Windows.Forms.Button sk_INSERT;
        private System.Windows.Forms.Label lbl_sel_count;
        private System.Windows.Forms.Button btn_sel_clear;
        private System.Windows.Forms.GroupBox groupBox_colors;
        private System.Windows.Forms.RadioButton radio_color00;
        private System.Windows.Forms.RadioButton radio_color01;
        private System.Windows.Forms.RadioButton radio_color02;
        private System.Windows.Forms.RadioButton radio_color03;
        private System.Windows.Forms.RadioButton radio_color04;
        private System.Windows.Forms.RadioButton radio_color05;
        private System.Windows.Forms.RadioButton radio_color06;
        private System.Windows.Forms.RadioButton radio_color07;
        private System.Windows.Forms.RadioButton radio_color08;
        private System.Windows.Forms.RadioButton radio_color09;
        private System.Windows.Forms.RadioButton radio_color10;
        private System.Windows.Forms.RadioButton radio_color11;
        private System.Windows.Forms.Label lbl_INFO;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radio_shape_auto;
        private System.Windows.Forms.RadioButton radio_shape_tile;
        private System.Windows.Forms.RadioButton radio_shape_circle;
        private System.Windows.Forms.RadioButton radio_shape_onedge;
        private System.Windows.Forms.RadioButton radio_shape_padd_r;
        private System.Windows.Forms.RadioButton radio_shape_square;
    }
}
