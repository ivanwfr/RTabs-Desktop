// using {{{
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// }}}
namespace Util
{
    public partial class ProfileForm : Form
    {
        private DialogResult  result        = DialogResult.Cancel;

        public ProfileForm() // {{{
        {
            InitializeComponent();
            Text = Settings.APP_NAME;
        }
        // }}}
        private void DialogBox_Focus(object sender, System.EventArgs e) // {{{
        {
            this.textBox_profile_path.Focus();
        }
        // }}}
        public static DialogResult ShowDialog(string profile_name, out string new_profile_name)// {{{
        {
            ProfileForm dialog                  = new ProfileForm();
            dialog.Text                         = Settings.APP_NAME;
            dialog.textBox_profile_path.Text    = profile_name;
            dialog.ShowDialog();

            if(dialog.result == DialogResult.OK)
                new_profile_name = dialog.textBox_profile_path.Text;
            else
                new_profile_name = profile_name;
            if(dialog != null) dialog.Dispose();

            log("ShowDialog:\n"
            +"  DialogResult.OK ["+ DialogResult.OK  +"]\n"
            +"    dialog.result ["+ dialog.result    +"]\n"
            +"     profile_name ["+ profile_name     +"]"
            +" new_profile_name ["+ new_profile_name +"]"
            );
            return dialog.result;
        }

        private void btn_cancel_Click      (object sender, EventArgs            e) { result = DialogResult.Cancel; Close(); }
        private void btn_ok_Click          (object sender, EventArgs            e) { result = DialogResult.OK    ; Close(); }
    //  private void ProfileForm_FormClosing(object sender, FormClosingEventArgs e) { }
        private void text_KeyUp            (object sender, KeyEventArgs         e)
        {
            if     (e.KeyCode == Keys.Enter ) { result = DialogResult.OK    ; Close(); }
            else if(e.KeyCode == Keys.Escape) { result = DialogResult.Cancel; Close(); }
        }

        // }}}

        // SUGGEST CURRENT PROFILE FILE PATH
        // selectProfile {{{
        public void   selectProfile(string profile_path)
        {
            textBox_profile_path.Text         = profile_path;
        }
        //}}}
        
        // RETRIEVE USER-SELECTED PROFILE FILE PATH
        // getProfile_path {{{
        public string getProfile_path()
        {
            return textBox_profile_path.Text;
        }
        //}}}

        // POPUP A PROFILE FILE CHOOSER DIALOG
        // btn_path_Click {{{
        private void btn_path_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = Settings.ProfilesFolder;
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter           = "Profile Text Files(*.txt)|*.txt";
            fileDialog.DereferenceLinks = true;
            fileDialog.CheckFileExists  = false; // exporting to an existing file ?

            if(fileDialog.ShowDialog() == DialogResult.OK)
                textBox_profile_path.Text = fileDialog.FileName;
        }
        //}}}

        // SELECT PROFILE FILE TO LOAD

        // LOG
        // log {{{
        private static void log(string msg)
        {
            Logger.Log(typeof(ProfileForm).Name, msg+"\n");
        }
        //}}}

    }
}
