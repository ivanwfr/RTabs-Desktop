// using {{{
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

// }}}
namespace Util
{
    public class RtfPict {
        // http://stackoverflow.com/questions/18017044/insert-image-at-cursor-position-in-rich-text-box {{{

        // public methods
        public static string embedImage(NotePane np, string fileName)// {{{
        {
            var img = Image.FromFile( fileName );

            var sb = new StringBuilder();

            sb.Append(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033");    // header
            sb.Append( getFontTable(np.Font));                          // font table
            sb.Append( @"\fs"+ NotePane.CONTROL_NAME_FONT_SIZE +@"\b \bullet  "+ np.Name +@"\b0  ("+ fileName.Substring(fileName.LastIndexOf("\\") + 1) +@")\line ");
            sb.Append( getImagePrefix(img, np));                        // image control
            sb.Append( getRtfImage(img, np));                           // metafile bytes in HEX format
            sb.Append(@"}");                                            // close image control

            return sb.ToString();
        }
        // }}}

        // private methods
        private static string getFontTable(Font font)// {{{
        {
            var fontTable = new StringBuilder();
            // Append table control string
            fontTable.Append(@"{\fonttbl{\f0");
            fontTable.Append(@"\");
            var rtfFontFamily = new HybridDictionary();
            rtfFontFamily.Add(FontFamily.GenericMonospace.Name, NativeMethods.RtfFontFamilyDef.Modern);
            rtfFontFamily.Add(FontFamily.GenericSansSerif, NativeMethods.RtfFontFamilyDef.Swiss);
            rtfFontFamily.Add(FontFamily.GenericSerif, NativeMethods.RtfFontFamilyDef.Roman);
            rtfFontFamily.Add("UNKNOWN", NativeMethods.RtfFontFamilyDef.Unknown);

            // If the font's family corresponds to an RTF family, append the
            // RTF family name, else, append the RTF for unknown font family.
            fontTable.Append(rtfFontFamily.Contains(font.FontFamily.Name) ? rtfFontFamily[font.FontFamily.Name] : rtfFontFamily["UNKNOWN"]);
            // \fcharset specifies the character set of a font in the font table.
            // 0 is for ANSI.
            fontTable.Append(@"\fcharset0 ");
            // Append the name of the font
            fontTable.Append(font.Name);
            // Close control string
            fontTable.Append(@";}}");
            return fontTable.ToString();
        }
        // }}}
        private static string getImagePrefix(Image _image, Control control)// {{{
        {
            float xDpi, yDpi;
            var sb = new StringBuilder();
            using (Graphics graphics = control.CreateGraphics())
            {
                xDpi = graphics.DpiX;
                yDpi = graphics.DpiY;
            }
            // Calculate the current width of the image in (0.01)mm
            var picw = (int)Math.Round((_image.Width / xDpi) * 2540);
            // Calculate the current height of the image in (0.01)mm
            var pich = (int)Math.Round((_image.Height / yDpi) * 2540);
            // Calculate the target width of the image in twips
            var picwgoal = (int)Math.Round((_image.Width / xDpi) * 1440);
            // Calculate the target height of the image in twips
            var pichgoal = (int)Math.Round((_image.Height / yDpi) * 1440);
            // Append values to RTF string
            sb.Append(@"{\pict\wmetafile8");
            sb.Append(@"\picw");
            sb.Append(picw);
            sb.Append(@"\pich");
            sb.Append(pich);
            sb.Append(@"\picwgoal");
            sb.Append(picwgoal);
            sb.Append(@"\pichgoal");
            sb.Append(pichgoal);
            sb.Append(" ");

            return sb.ToString();
        }
        // }}}

        //}}}
        private static string getRtfImage(Image image, Control control)// {{{
        {
            // Used to store the enhanced metafile
            MemoryStream stream = null;

            // Used to create the metafile and draw the image
            Graphics graphics = null;

            // The enhanced metafile
            Metafile metaFile = null;
            try {
                var sb = new StringBuilder();
                stream = new MemoryStream();

                // Get a graphics context from the RichTextBox
                using (graphics = control.CreateGraphics())
                {
                    // Get the device context from the graphics context
                    IntPtr hdc = graphics.GetHdc();
                    // Create a new Enhanced Metafile from the device context
                    metaFile = new Metafile(stream, hdc);
                    // Release the device context
                    graphics.ReleaseHdc(hdc);
                }

                // Get a graphics context from the Enhanced Metafile
                using (graphics = Graphics.FromImage(metaFile))
                {
                    // Draw the image on the Enhanced Metafile
                    graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
                }

                // Get the handle of the Enhanced Metafile
                IntPtr hEmf = metaFile.GetHenhmetafile();

                // A call to EmfToWmfBits with a null buffer return the size of the
                // buffer need to store the WMF bits.  Use this to get the buffer
                // size.
                uint bufferSize = NativeMethods.GdipEmfToWmfBits(hEmf, 0, null, 8, NativeMethods.EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);

                // Create an array to hold the bits
                var buffer = new byte[bufferSize];

                // A call to EmfToWmfBits with a valid buffer copies the bits into the
                // buffer an returns the number of bits in the WMF.  
                uint _convertedSize = NativeMethods.GdipEmfToWmfBits(hEmf, bufferSize, buffer, 8, NativeMethods.EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);

                // Append the bits to the RTF string
                foreach (byte t in buffer) {
                    sb.Append(String.Format("{0:X2}", t));
                }
                return sb.ToString();
            }
            finally {
                if(metaFile != null) metaFile.Dispose();
            }
        }
        // }}}
    }

}

