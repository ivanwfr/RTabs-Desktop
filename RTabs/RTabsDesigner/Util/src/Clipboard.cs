// using {{{
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

// }}}
namespace Util
{
    public class ClipboardAsync
    {
        // SET GET CLIPBOARD CONTENTS
        public static void SetText(string text)// {{{
        {
            //log("SetText(text):");

            ClipboardAsync instance = new ClipboardAsync();
            instance.text_to_set    = text;
            Thread thread           = new Thread(instance.instance_setText);

            thread.SetApartmentState( ApartmentState.STA );
            thread.Start();
            thread.Join();

        }
        // }}}
        public static string GetText()// {{{
        {
            //log("GetText():");

            return GetText( TextDataFormat.Text );
        }
        // }}}

        // THREADED INSTANCE
        public static string GetText(TextDataFormat format)// {{{
        {
            //log("GetText(format):");

            ClipboardAsync instance = new ClipboardAsync();

            Thread           thread = new Thread(instance.instance_getText);

            thread.SetApartmentState( ApartmentState.STA );
            thread.Start(format);
            thread.Join();

            return instance.text_to_get;
        }
        // }}}
        // private {{{
        private string text_to_get;
        private string text_to_set;

        private void instance_setText()
        {
            //log("instance_setText");
            Clipboard.Clear();
            Clipboard.SetText(text_to_set);
        }

        private void instance_getText(object format)
        {
            //log("instance_getText(format)");

            try {
                if(format == null)  text_to_get = Clipboard.GetText();
                else                text_to_get = Clipboard.GetText( (TextDataFormat)format );
            }
            catch(Exception ex) {
                log("instance_getText: "+ex.Message);
                text_to_get = ex.Message;
            }
        }

        // }}}

        private static void log(string msg)// {{{
        {
            Logger.Log(typeof(ClipboardAsync).Name, msg+"\n");
        }
        // }}}
    }
}
