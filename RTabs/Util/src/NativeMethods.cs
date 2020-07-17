// using {{{
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// }}}
namespace Util
{
    public static class NativeMethods
    {
        // reject focus - HideCaret {{{
        [DllImport("user32.dll")]
            static extern bool HideCaret(IntPtr hWnd);

        public static void HideCaret(object sender, EventArgs e)
        {
            HideCaret( ((RichTextBox)sender).Handle );
        }

        //}}}

        public enum EmfToWmfBitsFlags// {{{
        {
            EmfToWmfBitsFlagsDefault = 0x00000000,
            EmfToWmfBitsFlagsEmbedEmf = 0x00000001,
            EmfToWmfBitsFlagsIncludePlaceable = 0x00000002,
            EmfToWmfBitsFlagsNoXORClip = 0x00000004
        };
        // }}}

        public struct RtfFontFamilyDef// {{{
        {
            public const string Unknown = @"\fnil";
            public const string Roman = @"\froman";
            public const string Swiss = @"\fswiss";
            public const string Modern = @"\fmodern";
            public const string Script = @"\fscript";
            public const string Decor = @"\fdecor";
            public const string Technical = @"\ftech";
            public const string BiDirect = @"\fbidi";
        }
        // }}}

        // GdipEmfToWmfBits {{{
        [DllImport("gdiplus.dll")]
            public static extern uint GdipEmfToWmfBits(
                IntPtr                  _hEmf,
                uint                    _bufferSize,
                byte[]                  _buffer,
                int                     _mappingMode,
                EmfToWmfBitsFlags       _flag
                );

        //}}}

        // [keybd_event] Synthesizes keystroke (virtual-key code, hardware scan code {{{
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, UIntPtr dwExtraInfo);  

        // }}}
        // [mouse_event] synthesizes mouse motion and button click {{{
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);

        // }}}

    }

}

