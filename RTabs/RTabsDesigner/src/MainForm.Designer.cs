namespace RTabs
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                scaled_BackgroundImage.Dispose();
            }
            base.Dispose(disposing);
        }
    //  public void Dispose() { Dispose(true); }

        public System.Windows.Forms.Label label_IP;
        public System.Windows.Forms.Label label_password;
        public System.Windows.Forms.Label label_port;
        public System.Windows.Forms.Panel tabs_container;
        public System.Windows.Forms.Timer timer;
        public System.Windows.Forms.TextBox text_IP;
        public System.Windows.Forms.TextBox text_password;
        public System.Windows.Forms.TextBox text_port;
        public System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.Label label_palettes;
        public System.Windows.Forms.ComboBox combo_palettes;
        private System.Windows.Forms.Panel panel_ip;
        public System.Windows.Forms.Panel controls_container;
        public System.Windows.Forms.Panel panels_container;
        private System.Windows.Forms.Panel tabs_view;
        private System.Windows.Forms.Panel panel_dpi;
        public System.Windows.Forms.Label label_dev_dpi;
        public System.Windows.Forms.ComboBox combo_dev_dpi;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label_palettes = new System.Windows.Forms.Label();
            this.combo_palettes = new System.Windows.Forms.ComboBox();
            this.controls_container = new System.Windows.Forms.Panel();
            this.panel_dpi = new System.Windows.Forms.Panel();
            this.label_zoom = new System.Windows.Forms.Label();
            this.combo_txt_zoom = new System.Windows.Forms.ComboBox();
            this.combo_dev_zoom = new System.Windows.Forms.ComboBox();
            this.label_mon_scale = new System.Windows.Forms.Label();
            this.combo_mon_scale = new System.Windows.Forms.ComboBox();
            this.label_dev_wh = new System.Windows.Forms.Label();
            this.combo_dev_wh = new System.Windows.Forms.ComboBox();
            this.label_dev_dpi = new System.Windows.Forms.Label();
            this.combo_dev_dpi = new System.Windows.Forms.ComboBox();
            this.panel_ip = new System.Windows.Forms.Panel();
            this.text_mac = new System.Windows.Forms.TextBox();
            this.label_mac = new System.Windows.Forms.Label();
            this.label_subnet = new System.Windows.Forms.Label();
            this.text_subnet = new System.Windows.Forms.TextBox();
            this.label_IP = new System.Windows.Forms.Label();
            this.text_IP = new System.Windows.Forms.TextBox();
            this.label_port = new System.Windows.Forms.Label();
            this.text_port = new System.Windows.Forms.TextBox();
            this.label_password = new System.Windows.Forms.Label();
            this.text_password = new System.Windows.Forms.TextBox();
            this.panels_container = new System.Windows.Forms.Panel();
            this.tabs_view = new System.Windows.Forms.Panel();
            this.tabs_container = new System.Windows.Forms.Panel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.text_netsh = new System.Windows.Forms.TextBox();
            this.v_splitter = new System.Windows.Forms.SplitContainer();
            this.label_minimize = new System.Windows.Forms.Label();
            this.label_close = new System.Windows.Forms.Label();
            this.label_netsh = new System.Windows.Forms.Label();
            this.panel_dpi.SuspendLayout();
            this.panel_ip.SuspendLayout();
            this.tabs_view.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.v_splitter)).BeginInit();
            this.v_splitter.Panel1.SuspendLayout();
            this.v_splitter.Panel2.SuspendLayout();
            this.v_splitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_palettes
            // 
            this.label_palettes.AutoSize = true;
            this.label_palettes.BackColor = System.Drawing.Color.Transparent;
            this.label_palettes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_palettes.Location = new System.Drawing.Point(19, 10);
            this.label_palettes.Name = "label_palettes";
            this.label_palettes.Size = new System.Drawing.Size(91, 17);
            this.label_palettes.TabIndex = 15;
            this.label_palettes.Text = "[ca]  Colors";
            // 
            // combo_palettes
            // 
            this.combo_palettes.BackColor = System.Drawing.Color.Black;
            this.combo_palettes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combo_palettes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.combo_palettes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_palettes.ForeColor = System.Drawing.Color.DarkOrange;
            this.combo_palettes.FormattingEnabled = true;
            this.combo_palettes.Location = new System.Drawing.Point(6, 30);
            this.combo_palettes.MaxDropDownItems = 48;
            this.combo_palettes.Name = "combo_palettes";
            this.combo_palettes.Size = new System.Drawing.Size(104, 27);
            this.combo_palettes.TabIndex = 0;
            this.combo_palettes.TabStop = false;
            this.toolTip1.SetToolTip(this.combo_palettes, "Control+Alt F6-F7  or  1-0");
            this.combo_palettes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.combo_DrawItem);
            this.combo_palettes.SelectedIndexChanged += new System.EventHandler(this.combo_palettes_SelectedIndexChanged);
            // 
            // controls_container
            // 
            this.controls_container.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controls_container.AutoScroll = true;
            this.controls_container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.controls_container.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.controls_container.Location = new System.Drawing.Point(149, 557);
            this.controls_container.Margin = new System.Windows.Forms.Padding(8);
            this.controls_container.MinimumSize = new System.Drawing.Size(50, 50);
            this.controls_container.Name = "controls_container";
            this.controls_container.Size = new System.Drawing.Size(640, 220);
            this.controls_container.TabIndex = 1;
            this.controls_container.DoubleClick += new System.EventHandler(this.panel_controls_DoubleClick);
            // 
            // panel_dpi
            // 
            this.panel_dpi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panel_dpi.Controls.Add(this.label_zoom);
            this.panel_dpi.Controls.Add(this.combo_txt_zoom);
            this.panel_dpi.Controls.Add(this.combo_dev_zoom);
            this.panel_dpi.Controls.Add(this.label_palettes);
            this.panel_dpi.Controls.Add(this.combo_palettes);
            this.panel_dpi.Controls.Add(this.label_mon_scale);
            this.panel_dpi.Controls.Add(this.combo_mon_scale);
            this.panel_dpi.Controls.Add(this.label_dev_wh);
            this.panel_dpi.Controls.Add(this.combo_dev_wh);
            this.panel_dpi.Controls.Add(this.label_dev_dpi);
            this.panel_dpi.Controls.Add(this.combo_dev_dpi);
            this.panel_dpi.Location = new System.Drawing.Point(12, 49);
            this.panel_dpi.Name = "panel_dpi";
            this.panel_dpi.Size = new System.Drawing.Size(120, 347);
            this.panel_dpi.TabIndex = 30;
            // 
            // label_zoom
            // 
            this.label_zoom.AutoSize = true;
            this.label_zoom.BackColor = System.Drawing.Color.Transparent;
            this.label_zoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_zoom.Location = new System.Drawing.Point(4, 72);
            this.label_zoom.Name = "label_zoom";
            this.label_zoom.Size = new System.Drawing.Size(106, 17);
            this.label_zoom.TabIndex = 21;
            this.label_zoom.Text = "[cs] Txt Zoom";
            this.toolTip1.SetToolTip(this.label_zoom, "Global Text Size");
            // 
            // combo_txt_zoom
            // 
            this.combo_txt_zoom.BackColor = System.Drawing.Color.Black;
            this.combo_txt_zoom.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combo_txt_zoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.combo_txt_zoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_txt_zoom.ForeColor = System.Drawing.Color.DarkOrange;
            this.combo_txt_zoom.FormattingEnabled = true;
            this.combo_txt_zoom.Location = new System.Drawing.Point(6, 90);
            this.combo_txt_zoom.MaxDropDownItems = 48;
            this.combo_txt_zoom.Name = "combo_txt_zoom";
            this.combo_txt_zoom.Size = new System.Drawing.Size(104, 27);
            this.combo_txt_zoom.TabIndex = 20;
            this.combo_txt_zoom.TabStop = false;
            this.toolTip1.SetToolTip(this.combo_txt_zoom, "Control+Shift F6-F7  or Contro+Shift 1-8");
            this.combo_txt_zoom.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.combo_DrawItem);
            this.combo_txt_zoom.SelectedIndexChanged += new System.EventHandler(this.combo_txt_zoom_SelectedIndexChanged);
            // 
            // combo_dev_zoom
            // 
            this.combo_dev_zoom.BackColor = System.Drawing.Color.Black;
            this.combo_dev_zoom.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combo_dev_zoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.combo_dev_zoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_dev_zoom.ForeColor = System.Drawing.Color.DarkOrange;
            this.combo_dev_zoom.FormattingEnabled = true;
            this.combo_dev_zoom.Location = new System.Drawing.Point(46, 120);
            this.combo_dev_zoom.MaxDropDownItems = 48;
            this.combo_dev_zoom.Name = "combo_dev_zoom";
            this.combo_dev_zoom.Size = new System.Drawing.Size(64, 27);
            this.combo_dev_zoom.TabIndex = 22;
            this.combo_dev_zoom.TabStop = false;
            this.toolTip1.SetToolTip(this.combo_dev_zoom, "Device Font Adjustment factor\r\n- Font size = 12\r\n- with Zoom Text = 1\r\n- with Mon" +
        "itor Scale = 1\r\n- with DPI = Monitor DPI");
            this.combo_dev_zoom.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.combo_DrawItem);
            this.combo_dev_zoom.SelectedIndexChanged += new System.EventHandler(this.combo_dev_zoom_SelectedIndexChanged);
            // 
            // label_mon_scale
            // 
            this.label_mon_scale.AutoSize = true;
            this.label_mon_scale.BackColor = System.Drawing.Color.Transparent;
            this.label_mon_scale.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_mon_scale.Location = new System.Drawing.Point(24, 280);
            this.label_mon_scale.Name = "label_mon_scale";
            this.label_mon_scale.Size = new System.Drawing.Size(86, 17);
            this.label_mon_scale.TabIndex = 19;
            this.label_mon_scale.Text = "[a] Monitor";
            // 
            // combo_mon_scale
            // 
            this.combo_mon_scale.BackColor = System.Drawing.Color.Black;
            this.combo_mon_scale.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combo_mon_scale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.combo_mon_scale.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_mon_scale.ForeColor = System.Drawing.Color.DarkOrange;
            this.combo_mon_scale.FormattingEnabled = true;
            this.combo_mon_scale.Location = new System.Drawing.Point(6, 300);
            this.combo_mon_scale.MaxDropDownItems = 48;
            this.combo_mon_scale.Name = "combo_mon_scale";
            this.combo_mon_scale.Size = new System.Drawing.Size(104, 27);
            this.combo_mon_scale.TabIndex = 18;
            this.combo_mon_scale.TabStop = false;
            this.toolTip1.SetToolTip(this.combo_mon_scale, "Alt F6-F7  or  Alt 1-8");
            this.combo_mon_scale.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.combo_DrawItem);
            this.combo_mon_scale.SelectedIndexChanged += new System.EventHandler(this.combo_mon_scale_SelectedIndexChanged);
            // 
            // label_dev_wh
            // 
            this.label_dev_wh.AutoSize = true;
            this.label_dev_wh.BackColor = System.Drawing.Color.Transparent;
            this.label_dev_wh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_dev_wh.Location = new System.Drawing.Point(10, 220);
            this.label_dev_wh.Name = "label_dev_wh";
            this.label_dev_wh.Size = new System.Drawing.Size(96, 17);
            this.label_dev_wh.TabIndex = 17;
            this.label_dev_wh.Text = "[c] DEV_WH";
            // 
            // combo_dev_wh
            // 
            this.combo_dev_wh.BackColor = System.Drawing.Color.Black;
            this.combo_dev_wh.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combo_dev_wh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.combo_dev_wh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_dev_wh.ForeColor = System.Drawing.Color.DarkOrange;
            this.combo_dev_wh.FormattingEnabled = true;
            this.combo_dev_wh.Location = new System.Drawing.Point(6, 240);
            this.combo_dev_wh.MaxDropDownItems = 48;
            this.combo_dev_wh.Name = "combo_dev_wh";
            this.combo_dev_wh.Size = new System.Drawing.Size(104, 27);
            this.combo_dev_wh.TabIndex = 16;
            this.combo_dev_wh.TabStop = false;
            this.toolTip1.SetToolTip(this.combo_dev_wh, "Control F6-F7  or  Control 1-0");
            this.combo_dev_wh.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.combo_DrawItem);
            this.combo_dev_wh.SelectedIndexChanged += new System.EventHandler(this.combo_dev_wh_SelectedIndexChanged);
            // 
            // label_dev_dpi
            // 
            this.label_dev_dpi.AutoSize = true;
            this.label_dev_dpi.BackColor = System.Drawing.Color.Transparent;
            this.label_dev_dpi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_dev_dpi.Location = new System.Drawing.Point(54, 160);
            this.label_dev_dpi.Name = "label_dev_dpi";
            this.label_dev_dpi.Size = new System.Drawing.Size(65, 17);
            this.label_dev_dpi.TabIndex = 15;
            this.label_dev_dpi.Text = "[as] DPI";
            // 
            // combo_dev_dpi
            // 
            this.combo_dev_dpi.BackColor = System.Drawing.Color.Black;
            this.combo_dev_dpi.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combo_dev_dpi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.combo_dev_dpi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_dev_dpi.ForeColor = System.Drawing.Color.DarkOrange;
            this.combo_dev_dpi.FormattingEnabled = true;
            this.combo_dev_dpi.Location = new System.Drawing.Point(6, 180);
            this.combo_dev_dpi.MaxDropDownItems = 48;
            this.combo_dev_dpi.Name = "combo_dev_dpi";
            this.combo_dev_dpi.Size = new System.Drawing.Size(104, 27);
            this.combo_dev_dpi.TabIndex = 0;
            this.combo_dev_dpi.TabStop = false;
            this.toolTip1.SetToolTip(this.combo_dev_dpi, "Shift F6-F7  or  Shift 1-8");
            this.combo_dev_dpi.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.combo_DrawItem);
            this.combo_dev_dpi.SelectedIndexChanged += new System.EventHandler(this.combo_dpi_SelectedIndexChanged);
            // 
            // panel_ip
            // 
            this.panel_ip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panel_ip.Controls.Add(this.text_mac);
            this.panel_ip.Controls.Add(this.label_mac);
            this.panel_ip.Controls.Add(this.label_subnet);
            this.panel_ip.Controls.Add(this.text_subnet);
            this.panel_ip.Controls.Add(this.label_IP);
            this.panel_ip.Controls.Add(this.text_IP);
            this.panel_ip.Controls.Add(this.label_port);
            this.panel_ip.Controls.Add(this.text_port);
            this.panel_ip.Controls.Add(this.label_password);
            this.panel_ip.Controls.Add(this.text_password);
            this.panel_ip.Location = new System.Drawing.Point(2, 411);
            this.panel_ip.Name = "panel_ip";
            this.panel_ip.Size = new System.Drawing.Size(144, 240);
            this.panel_ip.TabIndex = 11;
            // 
            // text_mac
            // 
            this.text_mac.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.text_mac.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_mac.ForeColor = System.Drawing.Color.DarkOrange;
            this.text_mac.Location = new System.Drawing.Point(8, 160);
            this.text_mac.Name = "text_mac";
            this.text_mac.Size = new System.Drawing.Size(124, 22);
            this.text_mac.TabIndex = 10;
            this.toolTip1.SetToolTip(this.text_mac, "Server Login password");
            // 
            // label_mac
            // 
            this.label_mac.AutoSize = true;
            this.label_mac.BackColor = System.Drawing.Color.Transparent;
            this.label_mac.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_mac.Location = new System.Drawing.Point(92, 140);
            this.label_mac.Name = "label_mac";
            this.label_mac.Size = new System.Drawing.Size(40, 16);
            this.label_mac.TabIndex = 11;
            this.label_mac.Text = "MAC:";
            // 
            // label_subnet
            // 
            this.label_subnet.AutoSize = true;
            this.label_subnet.BackColor = System.Drawing.Color.Transparent;
            this.label_subnet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_subnet.Location = new System.Drawing.Point(79, 190);
            this.label_subnet.Name = "label_subnet";
            this.label_subnet.Size = new System.Drawing.Size(53, 16);
            this.label_subnet.TabIndex = 35;
            this.label_subnet.Text = "Subnet:";
            // 
            // text_subnet
            // 
            this.text_subnet.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.text_subnet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_subnet.ForeColor = System.Drawing.Color.DarkOrange;
            this.text_subnet.Location = new System.Drawing.Point(8, 206);
            this.text_subnet.Name = "text_subnet";
            this.text_subnet.Size = new System.Drawing.Size(124, 22);
            this.text_subnet.TabIndex = 34;
            this.toolTip1.SetToolTip(this.text_subnet, "Server Login password");
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.BackColor = System.Drawing.Color.Transparent;
            this.label_IP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_IP.Location = new System.Drawing.Point(109, 8);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(23, 16);
            this.label_IP.TabIndex = 8;
            this.label_IP.Text = "IP:";
            // 
            // text_IP
            // 
            this.text_IP.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.text_IP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_IP.ForeColor = System.Drawing.Color.DarkOrange;
            this.text_IP.Location = new System.Drawing.Point(8, 24);
            this.text_IP.Name = "text_IP";
            this.text_IP.Size = new System.Drawing.Size(124, 22);
            this.text_IP.TabIndex = 1;
            this.toolTip1.SetToolTip(this.text_IP, "Server IP-Address");
            this.text_IP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.text_IP_KeyDown);
            // 
            // label_port
            // 
            this.label_port.AutoSize = true;
            this.label_port.BackColor = System.Drawing.Color.Transparent;
            this.label_port.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_port.Location = new System.Drawing.Point(97, 48);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(35, 16);
            this.label_port.TabIndex = 9;
            this.label_port.Text = "Port:";
            // 
            // text_port
            // 
            this.text_port.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.text_port.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_port.ForeColor = System.Drawing.Color.DarkOrange;
            this.text_port.Location = new System.Drawing.Point(8, 64);
            this.text_port.Name = "text_port";
            this.text_port.Size = new System.Drawing.Size(124, 22);
            this.text_port.TabIndex = 2;
            this.toolTip1.SetToolTip(this.text_port, "Server Port Number");
            this.text_port.KeyDown += new System.Windows.Forms.KeyEventHandler(this.text_port_KeyDown);
            // 
            // label_password
            // 
            this.label_password.AutoSize = true;
            this.label_password.BackColor = System.Drawing.Color.Transparent;
            this.label_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_password.Location = new System.Drawing.Point(61, 96);
            this.label_password.Name = "label_password";
            this.label_password.Size = new System.Drawing.Size(71, 16);
            this.label_password.TabIndex = 6;
            this.label_password.Text = "Password:";
            // 
            // text_password
            // 
            this.text_password.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.text_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_password.ForeColor = System.Drawing.Color.DarkOrange;
            this.text_password.Location = new System.Drawing.Point(8, 112);
            this.text_password.Name = "text_password";
            this.text_password.Size = new System.Drawing.Size(124, 22);
            this.text_password.TabIndex = 3;
            this.toolTip1.SetToolTip(this.text_password, "Server Login password");
            this.text_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.text_password_KeyDown);
            // 
            // panels_container
            // 
            this.panels_container.AutoScroll = true;
            this.panels_container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panels_container.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panels_container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panels_container.Location = new System.Drawing.Point(0, 0);
            this.panels_container.Margin = new System.Windows.Forms.Padding(8);
            this.panels_container.Name = "panels_container";
            this.panels_container.Size = new System.Drawing.Size(140, 480);
            this.panels_container.TabIndex = 1;
            this.panels_container.DoubleClick += new System.EventHandler(this.panel_panels_DoubleClick);
            // 
            // tabs_view
            // 
            this.tabs_view.AutoScroll = true;
            this.tabs_view.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.tabs_view.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabs_view.Controls.Add(this.tabs_container);
            this.tabs_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs_view.Location = new System.Drawing.Point(0, 0);
            this.tabs_view.Name = "tabs_view";
            this.tabs_view.Size = new System.Drawing.Size(480, 480);
            this.tabs_view.TabIndex = 29;
            this.tabs_view.DoubleClick += new System.EventHandler(this.panel_scroll_DoubleClick);
            // 
            // tabs_container
            // 
            this.tabs_container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.tabs_container.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabs_container.Location = new System.Drawing.Point(0, 0);
            this.tabs_container.Margin = new System.Windows.Forms.Padding(8);
            this.tabs_container.Name = "tabs_container";
            this.tabs_container.Size = new System.Drawing.Size(480, 477);
            this.tabs_container.TabIndex = 0;
            this.tabs_container.DoubleClick += new System.EventHandler(this.tabs_container_DoubleClick);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // text_netsh
            // 
            this.text_netsh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.text_netsh.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.text_netsh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_netsh.ForeColor = System.Drawing.Color.DarkOrange;
            this.text_netsh.Location = new System.Drawing.Point(308, 530);
            this.text_netsh.Name = "text_netsh";
            this.text_netsh.Size = new System.Drawing.Size(481, 22);
            this.text_netsh.TabIndex = 10;
            this.text_netsh.Text = "(Description.*)";
            this.toolTip1.SetToolTip(this.text_netsh, "Server Login password");
            this.text_netsh.KeyDown += new System.Windows.Forms.KeyEventHandler(this.text_netsh_KeyDown);
            // 
            // v_splitter
            // 
            this.v_splitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.v_splitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.v_splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.v_splitter.Location = new System.Drawing.Point(149, 49);
            this.v_splitter.Margin = new System.Windows.Forms.Padding(0);
            this.v_splitter.Name = "v_splitter";
            // 
            // v_splitter.Panel1
            // 
            this.v_splitter.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.v_splitter.Panel1.Controls.Add(this.tabs_view);
            this.v_splitter.Panel1MinSize = 0;
            // 
            // v_splitter.Panel2
            // 
            this.v_splitter.Panel2.Controls.Add(this.panels_container);
            this.v_splitter.Panel2MinSize = 0;
            this.v_splitter.Size = new System.Drawing.Size(640, 480);
            this.v_splitter.SplitterDistance = 480;
            this.v_splitter.SplitterIncrement = 40;
            this.v_splitter.SplitterWidth = 20;
            this.v_splitter.TabIndex = 32;
            this.v_splitter.TabStop = false;
            this.v_splitter.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.v_splitter_SplitterMoving);
            this.v_splitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.v_splitter_SplitterMoved);
            this.v_splitter.DoubleClick += new System.EventHandler(this.v_splitter_DoubleClick);
            // 
            // label_minimize
            // 
            this.label_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_minimize.AutoSize = true;
            this.label_minimize.BackColor = System.Drawing.Color.Transparent;
            this.label_minimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_minimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_minimize.Location = new System.Drawing.Point(685, 4);
            this.label_minimize.Name = "label_minimize";
            this.label_minimize.Size = new System.Drawing.Size(55, 42);
            this.label_minimize.TabIndex = 23;
            this.label_minimize.Text = "➷";
            this.label_minimize.Click += new System.EventHandler(this.toggleUI_CB);
            this.label_minimize.MouseEnter += new System.EventHandler(this.Minimize_MouseEnter);
            this.label_minimize.MouseLeave += new System.EventHandler(this.Minimize_MouseLeave);
            // 
            // label_close
            // 
            this.label_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_close.AutoSize = true;
            this.label_close.BackColor = System.Drawing.Color.Transparent;
            this.label_close.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_close.Location = new System.Drawing.Point(746, 4);
            this.label_close.Name = "label_close";
            this.label_close.Size = new System.Drawing.Size(43, 42);
            this.label_close.TabIndex = 33;
            this.label_close.Text = "X";
            this.label_close.Click += new System.EventHandler(this.exitAppCB);
            this.label_close.MouseEnter += new System.EventHandler(this.Close_MouseEnter);
            this.label_close.MouseLeave += new System.EventHandler(this.Close_MouseLeave);
            // 
            // label_netsh
            // 
            this.label_netsh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_netsh.AutoSize = true;
            this.label_netsh.BackColor = System.Drawing.Color.Transparent;
            this.label_netsh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_netsh.Location = new System.Drawing.Point(155, 533);
            this.label_netsh.Name = "label_netsh";
            this.label_netsh.Size = new System.Drawing.Size(144, 16);
            this.label_netsh.TabIndex = 11;
            this.label_netsh.Text = "ipconfig capture-regex:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(800, 800);
            this.Controls.Add(this.label_netsh);
            this.Controls.Add(this.label_close);
            this.Controls.Add(this.text_netsh);
            this.Controls.Add(this.label_minimize);
            this.Controls.Add(this.v_splitter);
            this.Controls.Add(this.panel_dpi);
            this.Controls.Add(this.controls_container);
            this.Controls.Add(this.panel_ip);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Gray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "MainForm";
            this.Text = "RTabsServer";
            this.Activated += new System.EventHandler(this.OnActivated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.DoubleClick += new System.EventHandler(this.Form_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form_MouseWheel);
            this.panel_dpi.ResumeLayout(false);
            this.panel_dpi.PerformLayout();
            this.panel_ip.ResumeLayout(false);
            this.panel_ip.PerformLayout();
            this.tabs_view.ResumeLayout(false);
            this.v_splitter.Panel1.ResumeLayout(false);
            this.v_splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.v_splitter)).EndInit();
            this.v_splitter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer v_splitter;
        public System.Windows.Forms.Label label_mon_scale;
        public System.Windows.Forms.ComboBox combo_mon_scale;
        public System.Windows.Forms.Label label_dev_wh;
        public System.Windows.Forms.ComboBox combo_dev_wh;
        public System.Windows.Forms.Label label_zoom;
        public System.Windows.Forms.ComboBox combo_txt_zoom;
        public System.Windows.Forms.ComboBox combo_dev_zoom;
        public System.Windows.Forms.Label label_minimize;
        public System.Windows.Forms.Label label_close;
        public System.Windows.Forms.Label label_netsh;
        public System.Windows.Forms.TextBox text_netsh;
        public System.Windows.Forms.Label label_mac;
        public System.Windows.Forms.TextBox text_mac;
        public System.Windows.Forms.Label label_subnet;
        public System.Windows.Forms.TextBox text_subnet;
    }
}
