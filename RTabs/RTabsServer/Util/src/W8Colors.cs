    public static class ColorHelper
    {
        // Windows 8 Charms background colors
        // http://www.creepyed.com/2012/09/windows-8-colors-hex-code/</remarks>
        public static Windows.UI.Color[] MetroCharmsBackgroundColors
        {
            get
            {
                return new Windows.UI.Color[]
                {
                    ColorFromUInt(0xFF2E1700),	
                    ColorFromUInt(0xFF4E0000),	
                    ColorFromUInt(0xFF4E0038),	
                    ColorFromUInt(0xFF2D004E),	
                    ColorFromUInt(0xFF1F0068),	
                    ColorFromUInt(0xFF001E4E),	
                    ColorFromUInt(0xFF004D60),	
                    ColorFromUInt(0xFF004A00),	
                    ColorFromUInt(0xFF15992A),	
                    ColorFromUInt(0xFFE56C19),	
                    ColorFromUInt(0xFFB81B1B),	
                    ColorFromUInt(0xFFB81B6C),	
                    ColorFromUInt(0xFF691BB8),	
                    ColorFromUInt(0xFF1B58B8),	
                    ColorFromUInt(0xFF569CE3),	
                    ColorFromUInt(0xFF00AAAA),	
                    ColorFromUInt(0xFF83BA1F),	
                    ColorFromUInt(0xFFD39D09),	
                    ColorFromUInt(0xFFE064B7)
                };
            }
        }
 
        /// Contains Windows 8 Charms tile colors
        public static Windows.UI.Color[] MetroCharmsTileColors
        {
            get
            {
                return new Windows.UI.Color[]
                {
                    ColorFromUInt(0xFFF3B200),	
                    ColorFromUInt(0xFF77B900),	
                    ColorFromUInt(0xFF2572EB),	
                    ColorFromUInt(0xFFAD103C),	
                    ColorFromUInt(0xFF632F00),	
                    ColorFromUInt(0xFFB01E00),	
                    ColorFromUInt(0xFFC1004F),	
                    ColorFromUInt(0xFF7200AC),	
                    ColorFromUInt(0xFF4617B4),	
                    ColorFromUInt(0xFF006AC1),	
                    ColorFromUInt(0xFF008287),	
                    ColorFromUInt(0xFF199900),	
                    ColorFromUInt(0xFF00C13F),	
                    ColorFromUInt(0xFFFF981D),	
                    ColorFromUInt(0xFFFF2E12),	
                    ColorFromUInt(0xFFFF1D77),	
                    ColorFromUInt(0xFFAA40FF),	
                    ColorFromUInt(0xFF1FAEFF),	
                    ColorFromUInt(0xFF56C5FF),	
                    ColorFromUInt(0xFF00D8CC),	
                    ColorFromUInt(0xFF91D100),	
                    ColorFromUInt(0xFFE1B700),	
                    ColorFromUInt(0xFFFF76BC),	
                    ColorFromUInt(0xFF00A3A3),	
                    ColorFromUInt(0xFFFE7C22)
                };
            }
        }
 
        /// Contains Windows 8 Start screen background colors
        public static Windows.UI.Color[] MetroStartScreenBackgroundColors
        {
            get
            {
                return new Windows.UI.Color[]
                {
                    ColorFromUInt(0xFF261300),
                    ColorFromUInt(0xFF380000),
                    ColorFromUInt(0xFF40002E),
                    ColorFromUInt(0xFF250040),
                    ColorFromUInt(0xFF180052),
                    ColorFromUInt(0xFF001940),
                    ColorFromUInt(0xFF004050),
                    ColorFromUInt(0xFF003E00),
                    ColorFromUInt(0xFF128425),
                    ColorFromUInt(0xFFC35D15),
                    ColorFromUInt(0xFF9E1716),
                    ColorFromUInt(0xFF9E165B),
                    ColorFromUInt(0xFF57169A),
                    ColorFromUInt(0xFF16499A),
                    ColorFromUInt(0xFF4294DE),
                    ColorFromUInt(0xFF008E8E),
                    ColorFromUInt(0xFF7BAD18),
                    ColorFromUInt(0xFFC69408),
                    ColorFromUInt(0xFFDE4AAD)
                };
            }
        }
 
        /// Contains Windows 8 Start screen tile colors
        public static Windows.UI.Color[] MetroStartScreenTileColors
        {
            get
            {
                return new Windows.UI.Color[]
                {
                    ColorFromUInt(0xFF543A24),
                    ColorFromUInt(0xFF61292B),
                    ColorFromUInt(0xFF662C58),
                    ColorFromUInt(0xFF4C2C66),
                    ColorFromUInt(0xFF423173),
                    ColorFromUInt(0xFF2C4566),
                    ColorFromUInt(0xFF306772),
                    ColorFromUInt(0xFF2D652B),
                    ColorFromUInt(0xFF3A9548),
                    ColorFromUInt(0xFFC27D4F),
                    ColorFromUInt(0xFFAA4344),
                    ColorFromUInt(0xFFAA4379),
                    ColorFromUInt(0xFF7F6E94),
                    ColorFromUInt(0xFF6E7E94),
                    ColorFromUInt(0xFF6BA5E7),
                    ColorFromUInt(0xFF439D9A),
                    ColorFromUInt(0xFF94BD4A),
                    ColorFromUInt(0xFFCEA539),
                    ColorFromUInt(0xFFE773BD)
                };
            }
        }
 
        /// Converts uint to color.
        /// http://stackoverflow.com/questions/2109756/how-to-get-color-from-hex-color-code-using-net</remarks>
        public static Windows.UI.Color ColorFromUInt(uint argb)
        {
            return Windows.UI.Color.FromArgb((byte)((argb & -16777216) >> 0x18),
                                             (byte)((argb & 0xff0000 ) >> 0x10),
                                             (byte)((argb & 0xff00   ) >>    8),
                                             (byte)((argb & 0xff              ))
                                             ;
        }
    }
