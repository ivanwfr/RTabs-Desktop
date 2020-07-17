namespace Util
{
    partial class HelpForm
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
            this.tabs_container = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // tabs_container
            // 
            this.tabs_container.AutoScroll = true;
            this.tabs_container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs_container.Location = new System.Drawing.Point(50, 10);
            this.tabs_container.Margin = new System.Windows.Forms.Padding(100);
            this.tabs_container.MinimumSize = new System.Drawing.Size(50, 50);
            this.tabs_container.Name = "tabs_container";
            this.tabs_container.Padding = new System.Windows.Forms.Padding(100);
            this.tabs_container.Size = new System.Drawing.Size(580, 460);
            this.tabs_container.TabIndex = 27;
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.tabs_container);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(250, 250);
            this.Name = "HelpForm";
            this.Padding = new System.Windows.Forms.Padding(50, 10, 10, 10);
            this.ShowInTaskbar = false;
            this.Text = "HelpForm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel tabs_container;
    }
}
