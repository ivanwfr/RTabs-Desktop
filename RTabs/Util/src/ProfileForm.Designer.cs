using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    partial class ProfileForm
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
            this.textBox_profile_path = new System.Windows.Forms.TextBox();
            this.btn1 = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_path = new System.Windows.Forms.Button();
            this.profile_name = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_profile_path
            // 
            this.textBox_profile_path.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox_profile_path.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.textBox_profile_path.Location = new System.Drawing.Point(120, 24);
            this.textBox_profile_path.Name = "textBox_profile_path";
            this.textBox_profile_path.Size = new System.Drawing.Size(368, 20);
            this.textBox_profile_path.TabIndex = 1;
            this.textBox_profile_path.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            // 
            // btn1
            // 
            this.btn1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn1.Location = new System.Drawing.Point(8, 64);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(104, 23);
            this.btn1.TabIndex = 15;
            this.btn1.Text = "C&ancel";
            this.btn1.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_ok.Location = new System.Drawing.Point(120, 64);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(104, 23);
            this.btn_ok.TabIndex = 16;
            this.btn_ok.Text = "O&K";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_path
            // 
            this.btn_path.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_path.Location = new System.Drawing.Point(8, 24);
            this.btn_path.Name = "btn_path";
            this.btn_path.Size = new System.Drawing.Size(104, 23);
            this.btn_path.TabIndex = 17;
            this.btn_path.Text = "Choose a file";
            this.btn_path.Click += new System.EventHandler(this.btn_path_Click);
            // 
            // profile_name
            // 
            this.profile_name.AutoSize = true;
            this.profile_name.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.profile_name.Location = new System.Drawing.Point(8, 0);
            this.profile_name.Name = "profile_name";
            this.profile_name.Size = new System.Drawing.Size(58, 19);
            this.profile_name.TabIndex = 18;
            this.profile_name.Text = "Profile";
            // 
            // ProfileForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(496, 90);
            this.Controls.Add(this.profile_name);
            this.Controls.Add(this.btn_path);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.textBox_profile_path);
            this.Font = new System.Drawing.Font("Arial", 8.25F);
            this.ForeColor = System.Drawing.Color.LightGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RTabs Profile";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DialogBox_Focus);
            this.Enter += new System.EventHandler(this.DialogBox_Focus);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.text_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox_profile_path;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_path;
        private System.Windows.Forms.Label profile_name;
    }
}
