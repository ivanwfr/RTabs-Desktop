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

using System.ComponentModel;
// }}}
namespace Util
{
    public class ColorPalette {
        // PALETTES BUILT-IN {{{
        public static string PALETTE_NAMED_COLORS               = "W8"; // {{{

        // http://www.windowscentral.com/microsoft-confirming-custom-accent-colors-hubs-and-native-photosynth-windows-phone-8

        private static string AMBER              = "#F0A30A";
        private static string BROWN              = "#82592C";
        private static string COBALT             = "#0050EF";
        private static string CRIMSON            = "#A20025";
        private static string CYAN               = "#1BA1E2";

        private static string MAGENTA            = "#D80073";
        private static string LIME               = "#A3C300";
        private static string INDIGO             = "#6A00FF";
        private static string GREEN              = "#5FA917";
        private static string EMERALD            = "#008A00";

        private static string MAUVE              = "#765F89";
        private static string OLIVE              = "#6C8764";
        private static string ORANGE             = "#FA6800";
        private static string PINK               = "#F471D0";
        private static string RED                = "#E51300";

        private static string SIENNA             = "#7A3B3F";
        private static string STEEL              = "#647687";
        private static string TEAL               = "#00ABA9";
        private static string VIOLET             = "#AA00FF";
        private static string YELLOW             = "#D7C000";

        private static string[] HEX_NAMED_COLORS = {
            AMBER   , BROWN , COBALT , CRIMSON , CYAN    ,
            MAGENTA , LIME  , INDIGO , GREEN   , EMERALD ,
            MAUVE   , OLIVE , ORANGE , PINK    , RED     ,
            SIENNA  , STEEL , TEAL   , VIOLET  , YELLOW
        };
        // }}}
        // http://www.creepyed.com/2012/09/windows-8-colors-hex-code
        public static string PALETTE_CHARMS_BACKCOLORS = "Back"; // {{{
        private static string[]  HEX_CHARMS_BACKCOLORS = {

            "#2E1700" //  1
                , "#4E0000" //  2
                , "#4E0038" //  3
                , "#2D004E" //  4
                , "#1F0068" //  5

                , "#001E4E" //  6
                , "#004D60" //  7
                , "#004A00" //  8
                , "#15992A" //  9
                , "#E56C19" // 10

                , "#B81B1B" // 11
                , "#B81B6C" // 12
                , "#691BB8" // 13
                , "#1B58B8" // 14
                , "#569CE3" // 15

                , "#00AAAA" // 16
                , "#83BA1F" // 17
                , "#D39D09" // 18
                , "#E064B7" // 19
        };
        //}}}
        public static string PALETTE_CHARMS_TILECOLORS = "Tile"; // {{{
        private static string[]  HEX_CHARMS_TILECOLORS = {
            "#F3B200"    //  1
                , "#77B900"    //  2
                , "#2572EB"    //  3
                , "#AD103C"    //  4
                , "#632F00"    //  5

                , "#B01E00"    //  6
                , "#C1004F"    //  7
                , "#7200AC"    //  8
                , "#4617B4"    //  9
                , "#006AC1"    // 10

                , "#008287"    // 11
                , "#199900"    // 12
                , "#00C13F"    // 13
                , "#FF981D"    // 14
                , "#FF2E12"    // 15

                , "#FF1D77"    // 16
                , "#AA40FF"    // 17
                , "#1FAEFF"    // 18
                , "#56C5FF"    // 19
                , "#00D8CC"    // 20

                , "#91D100"    // 21
                , "#E1B700"    // 22
                , "#FF76BC"    // 23
                , "#00A3A3"    // 24
                , "#FE7C22"    // 25
        };
        //}}}
        public static string PALETTE_CHARMS_SCREEN_BACKCOLORS = "Screen"; // //{{{
        private static string[]  HEX_CHARMS_SCREEN_BACKCOLORS = {
            "#261300" //  1
                , "#380000" //  2
                , "#40002E" //  3
                , "#250040" //  4
                , "#180052" //  5

                , "#001940" //  6
                , "#004050" //  7
                , "#003E00" //  8
                , "#128425" //  9
                , "#C35D15" //  10

                , "#9E1716" //  11
                , "#9E165B" //  12
                , "#57169A" //  13
                , "#16499A" //  14
                , "#4294DE" //  15

                , "#008E8E" //  16
                , "#7BAD18" //  17
                , "#C69408" //  18
                , "#DE4AAD" //  19
        };

        //}}}
        public static string PALETTE_CHARMS_SCREEN_TILECOLORS = "Screen-Tile"; // //{{{
        private static string[]  HEX_CHARMS_SCREEN_TILECOLORS = {
            "#543A24" //  1
                , "#61292B" //  2
                , "#662C58" //  3
                , "#4C2C66" //  4
                , "#423173" //  5

                , "#2C4566" //  6
                , "#306772" //  7
                , "#2D652B" //  8
                , "#3A9548" //  9
                , "#C27D4F" //  10

                , "#AA4344" //  11
                , "#AA4379" //  12
                , "#7F6E94" //  13
                , "#6E7E94" //  14
                , "#6BA5E7" //  15

                , "#439D9A" //  16
                , "#94BD4A" //  17
                , "#CEA539" //  18
                , "#E773BD" //  19
        };


    //  private static Color[, ] Palette_NAMED_COLORS             = new Color[            HEX_NAMED_COLORS.Length,             HEX_NAMED_COLORS.Length];
    //  private static Color[, ] Palette_CHARMS_BACKCOLORS        = new Color[       HEX_CHARMS_BACKCOLORS.Length,        HEX_CHARMS_BACKCOLORS.Length];
    //  private static Color[, ] Palette_CHARMS_TILECOLORS        = new Color[       HEX_CHARMS_TILECOLORS.Length,        HEX_CHARMS_TILECOLORS.Length];
    //  private static Color[, ] Palette_CHARMS_SCREEN_BACKCOLORS = new Color[HEX_CHARMS_SCREEN_BACKCOLORS.Length, HEX_CHARMS_SCREEN_BACKCOLORS.Length];
    //  private static Color[, ] Palette_CHARMS_SCREEN_TILECOLORS = new Color[HEX_CHARMS_SCREEN_TILECOLORS.Length, HEX_CHARMS_SCREEN_TILECOLORS.Length];

        //}}}
        public static string PALETTE_CHARMS_ECC   = "ECC"; // //{{{
        private static string[]  HEX_CHARMS_ECC   = {
                  "#964B00" //  1 brown
                , "#FF0000" //  2 red
                , "#FFA500" //  3 Orange
                , "#FFFF00" //  4 Yellow
                , "#9ACD32" //  5 Green

                , "#6495ED" //  6 Blue
                , "#EE82EE" //  7 Violet
                , "#A0A0A0" //  8 Gray
                , "#FFFFFF" //  9 White
                , "#CFB53B" //  10 Gold

                , "#C0C0C0" //  11 Silver
        };

        // https://en.wikipedia.org/wiki/Electronic_color_code
        //}}}
        //}}}
        public static void LoadBuiltIns(Dictionary<string, Object> dict)// {{{
        {
            // return built-in (name + hex_array) palettes
            string   name      = "";
            string[] hex_array = null;
            try {
                name= PALETTE_NAMED_COLORS             ; hex_array= HEX_NAMED_COLORS             ; dict.Add(name, new ColorPalette(name, hex_array));
                name= PALETTE_CHARMS_BACKCOLORS        ; hex_array= HEX_CHARMS_BACKCOLORS        ; dict.Add(name, new ColorPalette(name, hex_array));
                name= PALETTE_CHARMS_TILECOLORS        ; hex_array= HEX_CHARMS_TILECOLORS        ; dict.Add(name, new ColorPalette(name, hex_array));
                name= PALETTE_CHARMS_SCREEN_BACKCOLORS ; hex_array= HEX_CHARMS_SCREEN_BACKCOLORS ; dict.Add(name, new ColorPalette(name, hex_array));
                name= PALETTE_CHARMS_SCREEN_TILECOLORS ; hex_array= HEX_CHARMS_SCREEN_TILECOLORS ; dict.Add(name, new ColorPalette(name, hex_array));
                name= PALETTE_CHARMS_ECC               ; hex_array= HEX_CHARMS_ECC               ; dict.Add(name, new ColorPalette(name, hex_array));

            }
            catch(Exception ex) {
                MessageBox.Show("LoadBuiltIns:\n"
                    +".............name["+ name             +"]"+ Environment.NewLine
                    +".hex_array.Length["+ hex_array.Length +"]"+ Environment.NewLine
                    + Settings.ExToString(ex)
                    , "ColorPalette"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information
                    );
            }

        }
        // }}}
        // members {{{
        public string   name;
        public string[] hex_array;
        public Color[]  bc_array;
        public Color[]  fc_array;

        //}}}
        // CONSTRUCTORS
        public  ColorPalette(string name, string hex_str)// {{{
        {
            this.name       = name;
            this.hex_array  = Split_hex_str( hex_str );
            this.convert();
        }

        private ColorPalette(string name, string[] hex_array)
        {
            this.name       = name;
            this.hex_array  = hex_array;
            this.convert();
        }
        // }}}
        private static string[] Split_hex_str(string hex_str) //{{{
        {
            hex_str = hex_str.Trim();
            if(hex_str == "")   return new string[0];
            else                return hex_str.Split(new Char[] {',', ' ', '\t'});
        //  if     (hex_str.IndexOf(",") > 0)       hex_array = hex_str.Split(',');
        //  else if(hex_str.IndexOf(" ") > 0)       hex_array = hex_str.Split(' ');
        }
        //}}}
        private void convert() //{{{
        {
            try_convert_hex();
            discard_rejected();

            log("convert done:"+ this.ToString());
        }
        //}}}
        private void try_convert_hex()// {{{
        {
            bc_array    = new Color[ hex_array.Length ];
            fc_array    = new Color[ hex_array.Length ];

            // try to convert hex_array values
            TypeConverter cc = TypeDescriptor.GetConverter( typeof(Color) );
            Color bc, fc;
            int converted_count = 0;
            for(int i=0; i < hex_array.Length; ++i)
            {
                try {
                    bc = (Color)cc.ConvertFromString( hex_array[i] );
bc = Color.FromArgb(255, bc); // *** Control does not support transparent background colors. ***
// http://stackoverflow.com/questions/17753043/how-to-change-transparency-of-a-color-in-c-sharp
                //  fc = (bc.GetBrightness() > 0.3) ? Color.Black : Color.White;
                    fc = (GetBrightness(bc)  > 127) ? Color.Black : Color.White;

                    bc_array[converted_count] = bc;
                    fc_array[converted_count] = fc;
                    converted_count += 1;

                }
                catch (Exception) {
                    hex_array[i] = "";  // invalidate this one
                }
            }
        }
        //}}}


        private void discard_rejected() //{{{
        {
            string hex_str = "";
            for(int w=0; w < hex_array.Length; ++w)
            {
                if(hex_array[w].StartsWith("#"))
                {
                    if(hex_str != "")   hex_str += ",";
                    hex_str                     += hex_array[w];
                }
            }
            hex_array = Split_hex_str( hex_str );
        }
        //}}}

    // BRIGHTNESS

        public Color GetDarkestBackColor()// {{{
        {
            Color color = Color.White;

            for(int i=0; i < bc_array.Length; ++i)
            {
                if(GetBrightness(bc_array[i]) < GetBrightness(color))
                    color = bc_array[i];
            }

            return color;
        }
        //}}}
        public Color GetLightestBackColor()// {{{
        {
            Color color = Color.White;

            for(int i=0; i < bc_array.Length; ++i)
            {
                if(GetBrightness(bc_array[i]) > GetBrightness(color))
                    color = bc_array[i];
            }

            return color;
        }
        //}}}

        public static Color GetHexColor(string hex_str)// {{{
        {
            TypeConverter cc = TypeDescriptor.GetConverter( typeof(Color) );

            Color bc = Color.White;

            try { bc = (Color)cc.ConvertFromString( hex_str ); } catch (Exception) { }

            return bc;
        }
        //}}}

        public static Color GetColorDarker(Color color, double factor)// {{{
        {
            if((factor < 0) || (factor > 1)) return color;

            int r = (int)(factor * color.R);
            int g = (int)(factor * color.G);
            int b = (int)(factor * color.B);
            return Color.FromArgb(r, g, b);
        }
        // }}}
        public static Color GetColorLighter(Color color, double factor)// {{{
        {
            if((factor < 0) || (factor > 1)) return color;

            int r = (int)(factor * color.R + (1 - factor) * 255);
            int g = (int)(factor * color.G + (1 - factor) * 255);
            int b = (int)(factor * color.B + (1 - factor) * 255);
            return Color.FromArgb(r, g, b);
        }
        // }}}

        public static int GetBrightness(Color c) //{{{
        {
            // http://www.nbdtech.com/Blog/archive/2008/04/27/Calculating-the-Perceived-Brightness-of-a-Color.aspx
            return (int)Math.Sqrt(
                c.R * c.R * .241 + 
                c.G * c.G * .691 + 
                c.B * c.B * .068);
        }
        //}}}

        public static Color GetColorLightnessTo(Color color, int to)// {{{
        {
            color = Color.FromArgb(color.R, color.G, color.B);
            int brightness = GetBrightness( color );
            if(brightness > to) {
                while(brightness > to) {
                    color      = GetColorDarker(color, 0.95);
                    brightness = GetBrightness( color );
                }
            }
            else {
                while(brightness < to) {
                    color      = GetColorLighter(color, 0.95);
                    brightness = GetBrightness( color );
                }
            }
            return color;
        }
        // }}}

        // LOG
        private static void log(string msg)// {{{
        {
            Logger.Log(typeof(ColorPalette).Name, msg+"\n");

/*
            MessageBox.Show(typeof(ColorPalette).Name+":\n"
                + msg
                , "ColorPalette"
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information
                );
*/
        }
        // }}}
        public override string ToString() {// {{{
            string hex_str = "";
            int count;
            for(count= 0; count < hex_array.Length; ++count)
            {
                if((count == 0)     )
                    hex_str = hex_array[count];
                else {
                    hex_str += " "+ hex_array[count];
                    if((count % 5) == 0) hex_str += Environment.NewLine;
                }
            }

            return typeof(ColorPalette).Name +" ["+name+"] has "+ count +" colors:\n["+hex_str+"]";
        }
        // }}}
    }

}
