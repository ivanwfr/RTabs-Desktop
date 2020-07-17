#region using {{{
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

#endregion //}}}
/* // {{{
:new ../DX1/DX1Utility/DX1Utility.cs

// SET FOCUS TO APPLICATION WINDOW
MemoryHandler.SwitchWindow(Process.GetProcessesByName("notepad").FirstOrDefault().MainWindowHandle);

*/ // }}}
namespace RTabs
{
        public class SendInput
        {
            // MOUSE EVENT {{{
/*
:!start explorer "https://github.com/CTurt/3DSController/blob/master/PC/source/keys.c"
:!start explorer "https://msdn.microsoft.com/en-us/library/windows/desktop/ms646260(v=vs.85).aspx"
*/
            private const uint MOUSEEVENTF_LEFTDOWN     = 0x0002; // The left   button is down
            private const uint MOUSEEVENTF_LEFTUP       = 0x0004; // The left   button is up
            private const uint MOUSEEVENTF_MIDDLEDOWN   = 0x0020; // The middle button is down
            private const uint MOUSEEVENTF_MIDDLEUP     = 0x0040; // The middle button is up
            private const uint MOUSEEVENTF_RIGHTDOWN    = 0x0008; // The right  button is down
            private const uint MOUSEEVENTF_RIGHTUP      = 0x0010; // The right  button is up
            //}}}
            // VIRTUAL-KEY CODES {{{
/*
:!start explorer "https://msdn.microsoft.com/en-us/library/ee504832.aspx"
:!start explorer "http://cherrytree.at/misc/vk.htm"
*/
            private const int VK_LBUTTON		= 0x01; // Left mouse button.
            private const int VK_RBUTTON		= 0x02; // Right mouse button.
            private const int VK_CANCEL			= 0x03; // Control-break processing.
            private const int VK_MBUTTON		= 0x04; // Middle mouse button on a three-button mouse.
            private const int VK_BACK			= 0x08; // BACKSPACE 
            private const int VK_TAB			= 0x09; // TAB 
            private const int VK_CLEAR			= 0x0C; // CLEAR 
            private const int VK_RETURN			= 0x0D; // ENTER 
            private const int VK_SHIFT			= 0x10; // SHIFT 
            private const int VK_CONTROL		= 0x11; // CTRL 
            private const int VK_MENU			= 0x12; // ALT 
            private const int VK_PAUSE			= 0x13; // PAUSE 
            private const int VK_CAPITAL		= 0x14; // CAPS LOCK 
            private const int VK_ESCAPE			= 0x1B; // ESC 
            private const int VK_SPACE			= 0x20; // SPACEBAR.
            private const int VK_PRIOR			= 0x21; // PAGE UP 
            private const int VK_NEXT			= 0x22; // PAGE DOWN 
            private const int VK_END			= 0x23; // END 
            private const int VK_HOME			= 0x24; // HOME 

            private const int VK_LEFT                   = 0x25; // ARROW LEFT
            private const int VK_UP                     = 0x26; // ARROW UP
            private const int VK_RIGHT                  = 0x27; // ARROW RIGHT
            private const int VK_DOWN                   = 0x28; // ARROW DOWN

            private const int VK_SELECT			= 0x29; // SELECT 
            private const int VK_EXECUTE		= 0x2B; // EXECUTE 
            private const int VK_SNAPSHOT		= 0x2C; // PRINT SCREEN 
            private const int VK_INSERT			= 0x2D; // INS 
            private const int VK_DELETE			= 0x2E; // DEL 
            private const int VK_HELP			= 0x2F; // HELP 

            private const int VK_0          		= 0x30; // D0
            private const int VK_1          		= 0x31; // D1
            private const int VK_2          		= 0x32; // D2
            private const int VK_3          		= 0x33; // D3
            private const int VK_4          		= 0x34; // D4
            private const int VK_5          		= 0x35; // D5
            private const int VK_6          		= 0x36; // D6
            private const int VK_7          		= 0x37; // D7
            private const int VK_8          		= 0x38; // D8
            private const int VK_9          		= 0x39; // D9

    /*
    30    48  0 key
    31    49  1 key
    32    50  2 key
    33    51  3 key
    34    52  4 key
    35    53  5 key
    36    54  6 key
    37    55  7 key
    38    56  8 key
    39    57  9 key
    3A    58 Undefined
    ..................
    40    64 Undefined
    41    65  A key
    42    66  B key
    43    67  C key
    44    68  D key
    45    69  E key
    46    70  F key
    47    71  G key
    48    72  H key
    49    73  I key
    4A    74  J key
    4B    75  K key
    4C    76  L key
    4D    77  M key
    4E    78  N key
    4F    79  O key
    50    80  P key
    51    81  Q key
    52    82  R key
    53    83  S key
    54    84  T key
    55    85  U key
    56    86  V key
    57    87  W key
    58    88  X key
    59    89  Y key
    5A    90  Z key

    */

            /* NUM {{{*/
            private const int VK_NUMPAD0		= 0x60; // Numeric keypad 0 
            private const int VK_NUMPAD1		= 0x61; // Numeric keypad 1 
            private const int VK_NUMPAD2		= 0x62; // Numeric keypad 2 
            private const int VK_NUMPAD3		= 0x63; // Numeric keypad 3 
            private const int VK_NUMPAD4		= 0x64; // Numeric keypad 4 
            private const int VK_NUMPAD5		= 0x65; // Numeric keypad 5 
            private const int VK_NUMPAD6		= 0x66; // Numeric keypad 6 
            private const int VK_NUMPAD7		= 0x67; // Numeric keypad 7 
            private const int VK_NUMPAD8		= 0x68; // Numeric keypad 8 
            private const int VK_NUMPAD9		= 0x69; // Numeric keypad 9 
            private const int VK_MULTIPLY		= 0x6A; // Multiply 
            private const int VK_ADD			= 0x6B; // Add 
            private const int VK_SEPARATOR		= 0x6C; // Separator 
            private const int VK_SUBSTRACT		= 0x6D; // Substract 
            private const int VK_DECIMAL		= 0x6E; // Decimal 
            private const int VK_DIVIDE			= 0x6F; // Divide 

            /*}}}*/
            /* F1..F24 {{{*/
            private const int VK_F1			= 0x70; // F1 
            private const int VK_F2			= 0x71; // F2 
            private const int VK_F3			= 0x72; // F3 
            private const int VK_F4			= 0x73; // F4 
            private const int VK_F5			= 0x74; // F5 
            private const int VK_F6			= 0x75; // F6 
            private const int VK_F7			= 0x76; // F7 
            private const int VK_F8			= 0x77; // F8 
            private const int VK_F9			= 0x78; // F9 
            private const int VK_F10			= 0x79; // F10 
            private const int VK_F11			= 0x7A; // F11 
            private const int VK_F12			= 0x7B; // F12 
            private const int VK_F13			= 0x7C; // F13 
            private const int VK_F14			= 0x7D; // F14 
            private const int VK_F15			= 0x7E; // F15 
            private const int VK_F16			= 0x7F; // F16 
            private const int VK_F17			= 0x80; // F17 
            private const int VK_F18			= 0x81; // F18 
            private const int VK_F19			= 0x82; // F19 
            private const int VK_F20			= 0x83; // F20 
            private const int VK_F21			= 0x84; // F21 
            private const int VK_F22			= 0x85; // F22 
            private const int VK_F23			= 0x86; // F23 
            private const int VK_F24			= 0x87; // F24 
            /*}}}*/

            private const int VK_NUMLOCK		= 0x90; // NUM LOCK 
            private const int VK_SCROLL			= 0x91; // SCROLL LOCK 

            /* MODIFIERS {{{*/
            private const int VK_LSHIFT                 = 0xA0; // Left Shift
            private const int VK_RSHIFT                 = 0xA1; // Right Shift
            private const int VK_LCONTROL               = 0xA2; // Left Ctrl
            private const int VK_RCONTROL               = 0xA3; // Right Ctrl
            private const int VK_LMENU                  = 0xA4; // Left Alt
            private const int VK_RMENU                  = 0xA5; // Right Alt
            private const int VK_LWIN                   = 0x5B; // Left Win
            private const int VK_RWIN                   = 0x5C; // Right Win
            private const int VK_APPS			= 0x5D; // Context Menu

            /*}}}*/

            private const int VK_OFF			= 0xDF; // Used to power the device on and off. No keyboard equivalent
            private const int VK_PACKET			= 0xE7; // Used to pass Unicode characters as if they were keystrokes
            private const int VK_ATTN			= 0xF6; // ATTN 
            private const int VK_CRSEL			= 0xF7; // CRSEL 
            private const int VK_EXSEL			= 0xF8; // EXSEL 
            private const int VK_EREOF			= 0xF9; // Erase EOF 
            private const int VK_PLAY			= 0xFA; // PLAY 
            private const int VK_ZOOM			= 0xFB; // ZOOM 
            private const int VK_NONAME			= 0xFC; // Reserved
            private const int VK_PA1			= 0xFD; // PA1 
            private const int VK_OEM_CLEAR		= 0xFE; // CLEAR 

        //  private const int VK_OEM_PERIOD 		= 0xBE; // PERIOD 
            private const int VK_OPEN_CURLY_BRACKET	= 0xDB; // [{ OEM_4 for US .. sc_shift
            private const int VK_CLOSE_CURLY_BRACKET    = 0xDD; // ]} OEM_6 for US .. sc_shift
            private const int VK_OPEN_PAREN  	        = 0x39; // 9( NUM-ROW      .. sc_shift
            private const int VK_CLOSE_PAREN  	        = 0x30; // 0) NUM-ROW      .. sc_shift

            //}}}
        // STRUCT {{{
        public const int    INPUT_MOUSE         = 0;
        public const int    INPUT_KEYBOARD      = 1;
        public const uint   KEYEVENTF_KEYUP     = 0x0002;
        public const uint   KEYEVENTF_SCANCODE  = 0x0008;

        [StructLayout(LayoutKind.Sequential)]
            public struct    MOUSEINPUT {
                public int    dx;
                public int    dy;
                public int    mouseData;
                public uint   dwFlags;
                public uint   time;
                public IntPtr dwExtraInfo;
            }

        [StructLayout(LayoutKind.Sequential)]
            public struct    KEYBDINPUT {
                public ushort wVk;
                public ushort wScan;
                public uint   dwFlags;
                public uint   time;
                public IntPtr dwExtraInfo;
            }

        [StructLayout(LayoutKind.Sequential)]
            public struct    HARDWAREINPUT {
                uint          uMsg;
                ushort        wParamL;
                ushort        wParamH;
            }

        [StructLayout(LayoutKind.Explicit)]
            public struct KMH_INPUT {                   // FIXME IWE (4 should be aligned to 8)
                [FieldOffset(0)] public int             type;
                [FieldOffset(8)] public KEYBDINPUT      ki;
                [FieldOffset(8)] public MOUSEINPUT      mi;
                [FieldOffset(8)]        HARDWAREINPUT   hi;
            }

        //}}}
        // TESTS {{{
        private static ushort sc_shift   = (ushort)NativeMethods.MapVirtualKey(VK_SHIFT  , 0);
        private static ushort sc_control = (ushort)NativeMethods.MapVirtualKey(VK_CONTROL, 0);
        private static ushort sc_alt     = (ushort)NativeMethods.MapVirtualKey(VK_MENU   , 0);
        private static ushort sc_numlock = (ushort)NativeMethods.MapVirtualKey(VK_NUMLOCK, 0);
        private static ushort sc_enter   = (ushort)NativeMethods.MapVirtualKey(VK_RETURN , 0);

        private static ushort sc_0       = (ushort)NativeMethods.MapVirtualKey(VK_0      , 0);
        private static ushort sc_1       = (ushort)NativeMethods.MapVirtualKey(VK_1      , 0);
        private static ushort sc_2       = (ushort)NativeMethods.MapVirtualKey(VK_2      , 0);
        private static ushort sc_3       = (ushort)NativeMethods.MapVirtualKey(VK_3      , 0);
        private static ushort sc_4       = (ushort)NativeMethods.MapVirtualKey(VK_4      , 0);
        private static ushort sc_5       = (ushort)NativeMethods.MapVirtualKey(VK_5      , 0);
        private static ushort sc_6       = (ushort)NativeMethods.MapVirtualKey(VK_6      , 0);
        private static ushort sc_7       = (ushort)NativeMethods.MapVirtualKey(VK_7      , 0);
        private static ushort sc_8       = (ushort)NativeMethods.MapVirtualKey(VK_8      , 0);
        private static ushort sc_9       = (ushort)NativeMethods.MapVirtualKey(VK_9      , 0);

        public static void test()
        {
            Thread.Sleep(1000);

            Keys   keys;     int  vk;          int modifiers;  ushort scanCode;                         string fmt = "[{0,1}] vk=[{1,3}] keys=[{2,3}] modifiers=[{3,3}], scanCode=[{4:X}]";
            keys = Keys.Shift   ; vk = VK_SHIFT  ; modifiers = vk>>8; scanCode = sc_shift  ; log(String.Format(fmt, "+", vk, keys, modifiers, scanCode));
            keys = Keys.Control ; vk = VK_CONTROL; modifiers = vk>>8; scanCode = sc_control; log(String.Format(fmt, "+", vk, keys, modifiers, scanCode));
            keys = Keys.Alt     ; vk = VK_MENU   ; modifiers = vk>>8; scanCode = sc_alt    ; log(String.Format(fmt, "+", vk, keys, modifiers, scanCode));
            keys = Keys.NumLock ; vk = VK_NUMLOCK; modifiers = vk>>8; scanCode = sc_numlock; log(String.Format(fmt, "+", vk, keys, modifiers, scanCode));
            keys = Keys.Enter   ; vk = VK_RETURN ; modifiers = vk>>8; scanCode = sc_enter  ; log(String.Format(fmt, "+", vk, keys, modifiers, scanCode));

            SendString(""
                + "test #3:{ENTER}"
                + "/accuse{ENTER}"
                + "/angry{ENTER}"
                + "/blush{ENTER}"
                + "UPPERCASE\n"
                );
        }
        //}}}

        // SendString {{{
        /* // {{{
            SHIFT             +
            CTRL              ^
            ALT               %

            SHIFT             {+}
            CTRL              {^}
            ALT               {%}

            BACKSPACE         {BACKSPACE}, {BS}, or {BKSP}
            BREAK             {BREAK}
            CAPS LOCK         {CAPSLOCK}
            DEL or DELETE     {DEL} or {DELETE}
            DOWN ARROW        {DOWN}
            END               {END}
            ENTER             {ENTER} or ~
            ESC               {ESC}
            HELP              {HELP}
            HOME              {HOME}
            INS or INSERT     {INSERT} or {INS}
            LEFT ARROW        {LEFT}
            NUM LOCK          {NUMLOCK}
            PAGE DOWN         {PGDN}
            PAGE UP           {PGUP}
            PRINT SCREEN      {PRTSC}
            RIGHT ARROW       {RIGHT}
            SCROLL LOCK       {SCROLLLOCK}
            TAB               {TAB}
            UP ARROW          {UP}
            F1                {F1}
            F2                {F2}
            F3                {F3}
            F4                {F4}
            F5                {F5}
            F6                {F6}
            F7                {F7}
            F8                {F8}
            F9                {F9}
            F10               {F10}
            F11               {F11}
            F12               {F12}
            F13               {F13}
            F14               {F14}
            F15               {F15}
            F16               {F16}
            Keypad add        {ADD}
            Keypad substract  {SUBSTRACT}
            Keypad multiply   {MULTIPLY}
            Keypad divide     {DIVIDE}

        */ // }}}

        private static Dictionary<string,int> ScanCode_Dictionary = new Dictionary<string,int>();

        // initialize_ScanCode_Dictionary {{{
        private static void initialize_ScanCode_Dictionary()
        {
            string sym;
            ushort sc;

            sym = "{SPACE}"      ; sc = (ushort)NativeMethods.MapVirtualKey(VK_SPACE      , 0); ScanCode_Dictionary.Add(sym, sc);
            /* XXX MOUSE */
            sym = "{LBUTTON}"    ; sc = (ushort)NativeMethods.MapVirtualKey(VK_LBUTTON    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{MBUTTON}"    ; sc = (ushort)NativeMethods.MapVirtualKey(VK_MBUTTON    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{RBUTTON}"    ; sc = (ushort)NativeMethods.MapVirtualKey(VK_RBUTTON    , 0); ScanCode_Dictionary.Add(sym, sc);
            /* XXX MOUSE */
            sym = "{BACKSPACE}"  ; sc = (ushort)NativeMethods.MapVirtualKey(VK_BACK       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{BS}"         ; sc = (ushort)NativeMethods.MapVirtualKey(VK_BACK       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{BKSP}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_BACK       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{BREAK}"      ; sc = (ushort)NativeMethods.MapVirtualKey(VK_DELETE     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{CAPSLOCK}"   ; sc = (ushort)NativeMethods.MapVirtualKey(VK_CAPITAL    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{DEL}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_DELETE     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{DELETE}"     ; sc = (ushort)NativeMethods.MapVirtualKey(VK_DELETE     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{DOWN}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_DOWN       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{END}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_END        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{ENTER}"      ; sc = (ushort)NativeMethods.MapVirtualKey(VK_RETURN     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{RETURN}"     ; sc = (ushort)NativeMethods.MapVirtualKey(VK_RETURN     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "~"            ; sc = (ushort)NativeMethods.MapVirtualKey(VK_RETURN     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{ESC}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_ESCAPE     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{ESCAPE}"     ; sc = (ushort)NativeMethods.MapVirtualKey(VK_ESCAPE     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{HELP}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_HELP       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{HOME}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_HOME       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{INSERT}"     ; sc = (ushort)NativeMethods.MapVirtualKey(VK_INSERT     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{INS}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_INSERT     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{LEFT}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_LEFT       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{NUMLOCK}"    ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMLOCK    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{PGDN}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NEXT       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{PAGEDOWN}"   ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NEXT       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{PGUP}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_PRIOR      , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{PAGEUP}"     ; sc = (ushort)NativeMethods.MapVirtualKey(VK_PRIOR      , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{PRTSC}"      ; sc = (ushort)NativeMethods.MapVirtualKey(VK_SNAPSHOT   , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{RIGHT}"      ; sc = (ushort)NativeMethods.MapVirtualKey(VK_RIGHT      , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{SCROLLLOCK}" ; sc = (ushort)NativeMethods.MapVirtualKey(VK_SCROLL     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{TAB}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_TAB        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{UP}"         ; sc = (ushort)NativeMethods.MapVirtualKey(VK_UP         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F1}"         ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F1         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F2}"         ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F2         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F3}"         ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F3         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F4}"         ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F4         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F5}"         ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F5         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F6}"         ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F6         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F7}"         ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F7         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F8}"         ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F8         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F9}"         ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F9         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F10}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F10        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F11}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F11        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F12}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F12        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F13}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F13        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F14}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F14        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F15}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F15        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{F16}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_F16        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{ADD}"        ; sc = (ushort)NativeMethods.MapVirtualKey(VK_ADD        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{SUBSTRACT}"  ; sc = (ushort)NativeMethods.MapVirtualKey(VK_SUBSTRACT  , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{MULTIPLY}"   ; sc = (ushort)NativeMethods.MapVirtualKey(VK_MULTIPLY   , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{DIVIDE}"     ; sc = (ushort)NativeMethods.MapVirtualKey(VK_DIVIDE     , 0); ScanCode_Dictionary.Add(sym, sc);

/* [0..9] {{{
            sym = "{0}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_INSERT     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{1}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_END        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{2}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_DOWN       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{3}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NEXT       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{4}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_LEFT       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{5}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMPAD5    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{6}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_RIGHT      , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{7}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_HOME       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{8}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_UP         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{9}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_PRIOR      , 0); ScanCode_Dictionary.Add(sym, sc);
}}}*/
            sym = "{0}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMPAD0    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{1}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMPAD1    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{2}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMPAD2    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{3}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMPAD3    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{4}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMPAD4    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{5}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMPAD5    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{6}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMPAD6    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{7}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMPAD7    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{8}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMPAD8    , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{9}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_NUMPAD9    , 0); ScanCode_Dictionary.Add(sym, sc);

// SHIFT CTRL ALT WIN {{{
// http://www.kbdedit.com/manual/low_level_vk_list.html

            sym =  "{SHIFT}"     ; sc = (ushort)NativeMethods.MapVirtualKey(VK_LSHIFT     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{LSHIFT}"     ; sc = (ushort)NativeMethods.MapVirtualKey(VK_LSHIFT     , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{RSHIFT}"     ; sc = (ushort)NativeMethods.MapVirtualKey(VK_RSHIFT     , 0); ScanCode_Dictionary.Add(sym, sc);

            sym =  "{CTRL}"      ; sc = (ushort)NativeMethods.MapVirtualKey(VK_LCONTROL   , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{LCTRL}"      ; sc = (ushort)NativeMethods.MapVirtualKey(VK_LCONTROL   , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{RCTRL}"      ; sc = (ushort)NativeMethods.MapVirtualKey(VK_RCONTROL   , 0); ScanCode_Dictionary.Add(sym, sc);

            sym = "{MENU}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_MENU       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym =  "{ALT}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_MENU       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{LALT}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_MENU       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{RALT}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_MENU       , 0); ScanCode_Dictionary.Add(sym, sc);

            sym =  "{WIN}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_LWIN       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{LWIN}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_LWIN       , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{RWIN}"       ; sc = (ushort)NativeMethods.MapVirtualKey(VK_RWIN       , 0); ScanCode_Dictionary.Add(sym, sc);

//}}}

// modifiers [# ^ % !] shift ctrl alt numlock {{{
// '+' modifiers & 1  sc_shift
// '^' modifiers & 2  sc_control
// '%' modifiers & 4  sc_alt
// '!' modifiers & 8  sc_numlock

            sym = "{+}"/*SHFT=*/ ; sc = (ushort)NativeMethods.MapVirtualKey(VK_ADD        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{^}"/*SHFT6*/ ; sc = (ushort)NativeMethods.MapVirtualKey( 54/*6 key ^*/, 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{%}"/*SHFT5*/ ; sc = (ushort)NativeMethods.MapVirtualKey( 53/*5 key %*/, 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{!}"/*SHFT5*/ ; sc = (ushort)NativeMethods.MapVirtualKey( 49/*5 key %*/, 0); ScanCode_Dictionary.Add(sym, sc);

//}}}

            sym = "{(}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_OPEN_PAREN         , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{)}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_CLOSE_PAREN        , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{{}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_OPEN_CURLY_BRACKET , 0); ScanCode_Dictionary.Add(sym, sc);
            sym = "{}}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_CLOSE_CURLY_BRACKET, 0); ScanCode_Dictionary.Add(sym, sc);
        //  sym = "{(}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_ADD        , 0); ScanCode_Dictionary.Add(sym, sc);
        //  sym = "{)}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_SUBSTRACT  , 0); ScanCode_Dictionary.Add(sym, sc);
        //  sym = "{{}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_MULTIPLY   , 0); ScanCode_Dictionary.Add(sym, sc);
        //  sym = "{}}"          ; sc = (ushort)NativeMethods.MapVirtualKey(VK_DIVIDE     , 0); ScanCode_Dictionary.Add(sym, sc);
        //  sym = "{{}"          ; sc = (ushort)NativeMethods.MapVirtualKey((ushort)(Keys)(NativeMethods.VkKeyScan('{')), 0); ScanCode_Dictionary.Add(sym, sc);
        //  sym = "{}}"          ; sc = (ushort)NativeMethods.MapVirtualKey((ushort)(Keys)(NativeMethods.VkKeyScan('}')), 0); ScanCode_Dictionary.Add(sym, sc);
        //  sym = "{(}"          ; sc = (ushort)NativeMethods.MapVirtualKey((ushort)(Keys)(NativeMethods.VkKeyScan('(')), 0); ScanCode_Dictionary.Add(sym, sc);
        //  sym = "{)}"          ; sc = (ushort)NativeMethods.MapVirtualKey((ushort)(Keys)(NativeMethods.VkKeyScan(')')), 0); ScanCode_Dictionary.Add(sym, sc);

        //  sym = "."            ; sc = (ushort)NativeMethods.MapVirtualKey(VK_OEM_PERIOD , 0); ScanCode_Dictionary.Add(sym, sc);
/*
:!start explorer "https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx"
*/
        }
        //}}}

            /* // {{{
SENDKEYS:
:!start explorer "https://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys.send(v=vs.110).aspx"

STRING LITERALS:
:!start explorer "https://msdn.microsoft.com/en-us/library/aa691090(v=vs.71).aspx"
A character that follows a backslash character (\)
in a regular-string-literal-character
must be one of the following characters:
'  "  \  0  a  b  f  n  r  t  u  U  x  v

ESCAPE SEQUENCES:
:!start explorer "https://msdn.microsoft.com/en-us/library/h21280bw.aspx"
\a      Bell (alert)
\b      Backspace
\f      Formfeed
\n      New line
\r      Carriage return
\t      Horizontal tab
\v      Vertical tab
\'      Single quotation mark
\"      Double quotation mark
\\      Backslash
\?      Literal question mark

\ooo    ASCII character in octal notation

\xhh    ASCII character in hexadecimal notation

\xhhhh  Unicode character in hexadecimal notation
if this escape sequence is used in a wide-character
            constant or a Unicode string literal.
                For example:
                WCHAR f = L'\x4e00'
                or
                WCHAR b[] = L
                "The Chinese character for one is \x4e00".

                */ // }}}

    // check_keyState {{{
    private static bool keyState_Alt;
    private static bool keyState_Control;
    private static bool keyState_Shift;
    private static bool keyState_NumLock;
    private static bool keyState_CapsLock;
    private static void check_keyState()
    {
        keyState_Alt        = MainForm.Is_Alt_down();
        keyState_Control    = MainForm.Is_Control_down();
        keyState_Shift      = MainForm.Is_Shift_down();
        keyState_NumLock    = MainForm.Is_NumLock_down();
        keyState_CapsLock   = MainForm.Is_CapsLock_down();

        log("KeyState(Alt Ctrl Shift NumLock CapsLock):" 
            +" ACS=["+ (keyState_Alt    ?"X":"-") + (keyState_Control ?"X":"-") + (keyState_Shift?"X":"-") +"]"
            + " NC=["+ (keyState_NumLock?"X":"-") + (keyState_CapsLock?"X":"-") +"]"
           );
    }
    //}}}


        private static int KEY_PRESS_DURATION = 20; // ms

        public static void SendString(string s)
        {
            // KEY PRESS DURATIONS .. EXTRA DELAY FOR IN-GAME-SLASH COMMANDS (START WITH AN INITIAL SLASH) {{{
            int key_press_duration = KEY_PRESS_DURATION;
            if(s[0] == '/') {
                key_press_duration = 2 * KEY_PRESS_DURATION;
                log("@@@ CHAT COMMAND DETECTED: key_press_duration set to ["+ key_press_duration +"]");
            }
            //}}}

            try {
                if(ScanCode_Dictionary.Count == 0)
                    initialize_ScanCode_Dictionary();

                int     count;
                int     idx;
                int     modifiers; int group_modifiers = 0;
                string  sym;
                ushort  scanCode;
                bool    keyState_sensed = false;

                while(s.Length > 0)
                {
                    // CONSUME ALL MODIFIER PREFIX // {{{
                    sym         = "";
                    scanCode    = 0;
                    modifiers   = 0;
                    count       = 1;

                    while((s.Length > 0) && ((s[0] == '+') || (s[0] == '^') || (s[0] == '%') || (s[0] == '!')))
                    {
                        if     (s[0] == '+') modifiers |= 1;
                        else if(s[0] == '^') modifiers |= 2;
                        else if(s[0] == '%') modifiers |= 4;
                        else if(s[0] == '!') modifiers |= 8;
                        s = s.Substring(1);
                    }
                    if(modifiers != 0) {
                        log("[1 +] .. [2 ^] .. [4 %] .. [8 !]");
                        log(String.Format(" modifiers=[{0,3}]", modifiers));
                    }

                    if(s.Length == 0) break;

                    // HOLD GROUP MODIFIERS {{{
                    if((s[0] == '(') && (modifiers != 0) && (group_modifiers == 0))
                    {
                        group_modifiers = modifiers; modifiers = 0;
                        log(String.Format(" HOLD GROUP MODIFIERS group_modifiers=[{0,3}]", group_modifiers));

                        if((group_modifiers & 8) != 0) {    // sc_numlock
                            if(!keyState_sensed) check_keyState();
                            if(!keyState_NumLock) { KeyDown( sc_numlock ); Thread.Sleep( KEY_PRESS_DURATION ); KeyUp  ( sc_numlock ); }
                        }
                        if((group_modifiers & 1) != 0) KeyDown( sc_shift   );
                        if((group_modifiers & 2) != 0) KeyDown( sc_control );
                        if((group_modifiers & 4) != 0) KeyDown( sc_alt     );

                        s = s.Substring(1);
                        if(s.Length == 0) break;
                    }
                    // }}}
                    // RELEASE GROUP MODIFIERS {{{
                    if((s[0] == ')') && (group_modifiers != 0))
                    {
                        log(String.Format(" RELEASE GROUP MODIFIERS group_modifiers=[{0,3}]", group_modifiers));

                        if((group_modifiers & 2) != 0) KeyUp  ( sc_control );
                        if((group_modifiers & 1) != 0) KeyUp  ( sc_shift   );
                        if((group_modifiers & 4) != 0) KeyUp  ( sc_alt     );
                        if((group_modifiers & 8) != 0) {    // sc_numlock
                            if(!keyState_NumLock) { KeyDown( sc_numlock ); Thread.Sleep( KEY_PRESS_DURATION ); KeyUp  ( sc_numlock ); }
                        }

                        group_modifiers = 0;

                        s = s.Substring(1);
                        if(s.Length == 0) break;

                        continue;
                    }
                    // }}}

                    // }}}
                    // SCANCODE FROM NAMED SYM {{{
                    if(s[0] == '{')
                    {
                        idx = s.IndexOf('}', 2);
                        if(idx > 0)
                        {
                            sym     = s.Substring(0,idx+1); // [zero-based start] , [number of char length]

                            // count {{{
                            try {
                                int sdx = sym.IndexOf(' ');
                                if( sdx > 0) {                                  // ..sdx>|...   = 6
                                    String sc = sym.Substring(sdx+1, idx-sdx-1);// {RIGHT 42}
                                    log("@@@ COUNT sc=["+ sc +"]");             // .....idx>|   = 9
                                    count = int.Parse( sc );                    // strip count
                                    sym = sym.Substring(0,sdx) +"}";            // {RIGHT}
                                    log("@@@ sym=["+ sym +"] ... count=["+ count +"]");

                                    if(sym == "{SLEEP}") {
                                        // CONSUME SYM.LENGTH + COUNT SPECIFIER
                                        s = s.Substring(idx + 1);
                                        log("...Thread.Sleep("+ count +")");
                                        Thread.Sleep( count );
                                        continue;
                                    }

                                }
                            } catch(Exception ex) {
                                log("SendString("+ sym +"):\n"+ex.Message);
                            }
                            //}}}

                            if( ScanCode_Dictionary.ContainsKey( sym ) )
                            {
                                // CONSUME SYM.LENGTH + COUNT SPECIFIER
                                s = s.Substring(idx + 1);

                                scanCode = (ushort)ScanCode_Dictionary[ sym ];
                                log(String.Format("SCANCODE DICTIONARY:..........scanCode=[{0:X}]: {1}", scanCode, sym));

                                // SHIFTED NUM ROW
                                if     ((sym == "{%}") || (sym == "{+}") || (sym == "{^}") || (sym == "{!}")) modifiers |= 1;

                                // SHIFTED SYMBOLS
                                if     ((sym == "{{}") || (sym == "{}}") || (sym == "{(}") || (sym == "{)}")) modifiers |= 1;
/*
                                else if((sym == "{0}") || (sym == "{1}") || (sym == "{2}")) modifiers |= 8;
                                else if((sym == "{3}") || (sym == "{4}") || (sym == "{5}")) modifiers |= 8;
                                else if((sym == "{6}") || (sym == "{7}") || (sym == "{8}")) modifiers |= 8;
                                else if((sym == "{9}")                                    ) modifiers |= 8;
*/
/*
                                else if((sym == "{7}") || (sym == "{9}")) modifiers |= 8;
                                else if((sym == "{1}") || (sym == "{3}")) modifiers |= 8;
*/

                            }
                            else {
                                // reinject single char count
                                if(count > 1) {
                                    log("@@@ REINJECT SINGLE CHAR sym=["+ sym[1] +"] count=["+ count +"]");
                                    s = sym[1] + s.Substring(idx + 1);
                                }
                                sym = "";
                                log(String.Format("SCANCODE DICTIONARY:..........scanCode=[{0:X}]: {1}",    "???", s[0]));
                            }
                        }

                    }
                    //}}}
                    // SCANCODE FROM CHAR SYM {{{
                    if(sym == "")
                    {
                        // VK // {{{
                        short vk   = NativeMethods.VkKeyScan( s[0] );
                        Keys  keys = (Keys)(vk & 0xff);
                        if(modifiers == 0) modifiers  = vk >> 8;

                        // }}}
                        // SCANCODE // {{{
                        scanCode = (ushort)NativeMethods.MapVirtualKey((ushort)keys, 0);

                        // VK_NUMLOCK: turn numbers to numpad
                        if(    ((group_modifiers & 8) != 0)
                            || ((      modifiers & 8) != 0)
                          ) {
                            /*
                               30    48  0 key
                               31    49  1 key
                               32    50  2 key
                               33    51  3 key
                               34    52  4 key
                               35    53  5 key
                               36    54  6 key
                               37    55  7 key
                               38    56  8 key
                               39    57  9 key
                               3A    58 Undefined
                             */
                            if     (scanCode == sc_0) scanCode = (ushort)ScanCode_Dictionary["{0}"];
                            else if(scanCode == sc_1) scanCode = (ushort)ScanCode_Dictionary["{1}"];
                            else if(scanCode == sc_2) scanCode = (ushort)ScanCode_Dictionary["{2}"];
                            else if(scanCode == sc_3) scanCode = (ushort)ScanCode_Dictionary["{3}"];
                            else if(scanCode == sc_4) scanCode = (ushort)ScanCode_Dictionary["{4}"];
                            else if(scanCode == sc_5) scanCode = (ushort)ScanCode_Dictionary["{5}"];
                            else if(scanCode == sc_6) scanCode = (ushort)ScanCode_Dictionary["{6}"];
                            else if(scanCode == sc_7) scanCode = (ushort)ScanCode_Dictionary["{7}"];
                            else if(scanCode == sc_8) scanCode = (ushort)ScanCode_Dictionary["{8}"];
                            else if(scanCode == sc_9) scanCode = (ushort)ScanCode_Dictionary["{9}"];
                        }

                        log(String.Format("[{0,1}] vk=[{1,3}] modifiers=[{2,3}], scanCode=[{3:X}]: {4}"
                                ,           s[0],  vk,        modifiers,         scanCode,         keys));
                        //}}}
                        // SLEEP PARAM {{{
                        if(s[0] == '/') {
                            // - SendInput [/] vk=[191] modifiers=[  0], scanCode=[ 53]: OemQuestion
                            int SLASH_CMD_DELAY = 500;
                            log("@@@ IN-GAME FOCUS TO CHAT EXTRA DELAY s[0]=["+ s[0] +"] ... Thread.Sleep(SLASH_CMD_DELAY="+ SLASH_CMD_DELAY +")");
                            Thread.Sleep( SLASH_CMD_DELAY );
                        }
                        //}}}
                        // CONSUME // {{{
                        s = s.Substring(1);
                        // }}}
                    }
                    //}}}

                    // HOLD MODIFIERS // {{{
                    if((modifiers & 1) != 0) KeyDown( sc_shift   );
                    if((modifiers & 2) != 0) KeyDown( sc_control );
                    if((modifiers & 4) != 0) KeyDown( sc_alt     );
                    if((modifiers & 8) != 0) KeyDown( sc_numlock );

                    // }}}

                for(int c = 0; c<count; ++c) {
                    //log("@@@ sym=["+ sym +"]");
                    // PRESS-DELAY: [ENTER] [.] {{{
                    if(    (sym == "{ENTER}")
                        || (sym == "{RETURN}")
                        || (sym == "~")
                    //  || (scanCode == 52) // VK_OEM_PERIOD
                      )
                        Thread.Sleep(500);

                    // }}}
                    // PRESS KEY {{{
                    if((sym == "{LBUTTON}") || (sym == "{MBUTTON}") || (sym == "{RBUTTON}"))
                        BtnDown( scanCode, sym);
                    else
                        KeyDown( scanCode );

                    // }}}
                    // HOLD KEY & MODIFIERS {{{
                    Thread.Sleep( key_press_duration );

                    // }}}
                    // BUTTON EXTRA DELAY {{{
                    if(    (sym == "{LBUTTON}")
                        || (sym == "{MBUTTON}")
                        || (sym == "{RBUTTON}")
                      )
                        Thread.Sleep(100);

                    // }}}
                    // SPACE EXTRA DELAY {{{
                    if(sym == "{SPACE}")
                        Thread.Sleep( 50);

                    // }}}
                    // ENTER EXTRA DELAY {{{
                    if(    (sym == "{ENTER}")
                        || (sym == "{RETURN}")
                        || (sym == "~")
                      )
                        Thread.Sleep(500);

                    // }}}
                    // CONTROL ALT EXTRA DELAY {{{
                    if(    ((modifiers & 2) != 0)   // control
                        || ((modifiers & 4) != 0)   // alt
                      )
                        Thread.Sleep(50);

                    // }}}
                    // BACKSPACE EXTRA DELAY {{{
                    if(    (sym == "{BACKSPACE}")
                      )
                        Thread.Sleep(200);

                    // }}}
                    // RELEASE KEY {{{
                    if((sym == "{LBUTTON}") || (sym == "{MBUTTON}") || (sym == "{RBUTTON}"))
                        BtnUp( scanCode, sym);
                    else
                        KeyUp  ( scanCode );

                    // }}}
                }
                    // RELEASE MODIFIERS {{{
                    if((modifiers & 8) != 0) KeyUp  ( sc_numlock );
                    if((modifiers & 4) != 0) KeyUp  ( sc_alt     );
                    if((modifiers & 2) != 0) KeyUp  ( sc_control );
                    if((modifiers & 1) != 0) KeyUp  ( sc_shift   );

                    // }}}

                    // TO NEXT KEY INTERVAL // {{{
                    Thread.Sleep(10);
                    // }}}
                }
            }
            catch(Exception ex) {
                log("SendString("+ s +"):\n"+ex.Message);
                throw ex;
            }

        }
        //}}}

        // KeyDown {{{
        private static KMH_INPUT[] inputs = new KMH_INPUT[1];

        public static void KeyDown(ushort scanCode)
        {
            inputs[0].type       = INPUT_KEYBOARD;     // needs to set eveytime .. why?
            inputs[0].ki.wScan   = (ushort)(scanCode & 0xff);
            inputs[0].ki.dwFlags = KEYEVENTF_SCANCODE;

            if(NativeMethods.SendInput(1, inputs, Marshal.SizeOf(inputs[0])) != 1)
                throw new Exception("Could not send key DOWN: "+ scanCode);
        }
        //}}}
        // KeyUp {{{
        public static void KeyUp(ushort scanCode)
        {
            inputs[0].type       = INPUT_KEYBOARD;     // needs to set eveytime .. why?
            inputs[0].ki.wScan   = (ushort)(scanCode & 0xff);
            inputs[0].ki.dwFlags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP;

            if(NativeMethods.SendInput(1, inputs, Marshal.SizeOf(inputs[0])) != 1)
                throw new Exception("Could not send key UP: " + scanCode);
        }
        //}}}
        // BtnDown {{{

        public static void BtnDown(ushort scanCode, string sym)
        {
log("BtnDown("+sym+")");
            inputs[0].type       = INPUT_MOUSE;
            inputs[0].mi.dwFlags = (sym == "{LBUTTON}") ? MOUSEEVENTF_LEFTDOWN
                :                  (sym == "{MBUTTON}") ? MOUSEEVENTF_MIDDLEDOWN
                :                                         MOUSEEVENTF_RIGHTDOWN;

            if(NativeMethods.SendInput(1, inputs, Marshal.SizeOf(inputs[0])) != 1)
                throw new Exception("Could not send Button DOWN: "+ sym);
        }
        //}}}
        // BtnUp {{{
        public static void BtnUp(ushort scanCode, string sym)
        {
log("BtnUp("+sym+")");
            inputs[0].type       = INPUT_MOUSE;
            inputs[0].mi.dwFlags = (sym == "{LBUTTON}") ? MOUSEEVENTF_LEFTUP
                :                  (sym == "{MBUTTON}") ? MOUSEEVENTF_MIDDLEUP
                :                                         MOUSEEVENTF_RIGHTUP;

            if(NativeMethods.SendInput(1, inputs, Marshal.SizeOf(inputs[0])) != 1)
                throw new Exception("Could not send Button UP: " + scanCode);
        }
        //}}}

        // NativeMethods {{{
        internal static class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
                public static extern uint SendInput(uint nInputs, KMH_INPUT[] pInputs, int cbSize);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
                public static extern short VkKeyScan(char ch);

            [DllImport("user32.dll")]
                public static extern int MapVirtualKey(int uCode, uint uMapType);
/*
            [DllImport("user32.dll")]
                public static KeyStates GetKeyStates(Key key);
            //  public static extern int GetKeyState(int uCode);
            [DllImport("user32.dll", SetLastError = true)]
                public static extern Int16 GetKeyState(UInt32 virtualKeyCode);
*/
            public static bool IsKeyDown(int keyCode) {
                Int16 result = GetKeyState((UInt16)keyCode);
                return (result < 0);
            }

            [DllImport("user32.dll", SetLastError = true)]
                private static extern Int16 GetKeyState(UInt32 virtualKeyCode);

        }
        // }}}

    // log {{{
    public static void log(string msg)
    {
        Logger.Log(typeof(SendInput).Name, msg+"\n");
        /*
           MessageBox.Show(typeof(SendInput).Name+":\n"
           + msg
           , "NotePane"
           , MessageBoxButtons.OK
           , MessageBoxIcon.Information
           );

         */

    }
    //}}}


    }
}

