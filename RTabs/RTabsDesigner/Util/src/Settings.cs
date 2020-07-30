// {{{
using Microsoft.Win32;
using RTabs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
// }}}
namespace Util
{
    public class Settings
    {
        // SETTINGS MEMBERS {{{
        private const  string   Settings_TAG                    = "Settings (200727:02h:48)";

        public static bool      UseMutex                        = false;

        public static long      ParseTime_Millisecond           = 0;
        private static string   FOLD_OPEN                       = "{{{";

        public static int       USER_TABS_MAX                   = 256;
        public static int       PALETTES_MAX                    = 256;

        public static DateTime  LAUNCH_DATE                     = DateTime.Now;

        public static string    PROFILE_DRAFT                   = "draft";
        public static string    MUTEX_DASH                      = "MUTEX_DASH";
        public static Mutex     ServiceMutex                    = new Mutex();
        public static int       WAIT_SERVICE_MUTEX_TIMEOUT_MS   = 5000;
        public static int       ServiceMutexCount               = 0;
        public static string    ServiceMutexOwner               = "";

        private static string   FOLD_CLOSE                      = "}}}";

        //}}}
        // CLIENT-SERVER INTERNAL COMMANDS {{{

        // -----------------------------------  file://PROFILES_DIR/DEV/AZ-ABS-00-0100-010011_rev00.html
        public static string PROFILES_DIR           = "PROFILES_DIR";
        public static string PROFILES_DIR_PATH      =                                        "RTabs_Profiles";
        // ----------------g PROFILES_DIR_PATH      = "C:/LOCAL/STORE/DEV/PROJECTS/RTabs/Util/RTabs_Profiles";
        // -----------------------------------  file://C:/LOCAL/STORE/DEV/PROJECTS/RTabs/Util/RTabs_Profiles/DEV/index.html

        // DEVICE
        public static string ADB_DEVICE_IP          = "192.168.1.18";   // default fallback value
        public static int    ADB_DEVICE_PORT        =  5555;            // when environment has none

        // PROCESS
        public static string CMD_BROWSE             = "BROWSE";
        public static string CMD_RUN                = "RUN";
        public static string CMD_SHELL              = "SHELL";

        public static string CMD_BEEP               = "BEEP";
        public static string CMD_CLEAR              = "CLEAR";
        public static string CMD_CLOSE              = "CLOSE";
        public static string CMD_CONTROLS_TABLE     = "CONTROLS_TABLE";
        public static string CMD_HIDE               = "HIDE";
        public static string CMD_KEY_VAL            = "KEY_VAL";
        public static string CMD_LOGGING            = "LOGGING";
        public static string CMD_OK                 = "OK";
        public static string CMD_PALETTES_CLEAR     = "PALETTES_CLEAR";
        public static string CMD_PALETTES_GET       = "PALETTES_GET";
        public static string CMD_PALETTES_LOAD      = "PALETTES_LOAD";
        public static string CMD_PALETTES_SETTINGS  = "PALETTES_SETTINGS";
        public static string CMD_PASSWORD           = "PASSWORD";
        public static string CMD_POLL               = "POLL";
        public static string CMD_PROFILE            = "PROFILE";
        public static string CMD_PROFILES_TABLE     = "PROFILES_TABLE";
        public static string CMD_PROFILE_DOWNLOAD   = "PROFILE_DOWNLOAD";
        public static string CMD_RELOAD             = "RELOAD";

        public static string CMD_SENDINPUT          = "SENDINPUT";
        public static string CMD_SENDINPUTTEXT      = "SENDINPUTTEXT";
        public static string CMD_SENDKEYS           = "SENDKEYS";
        public static string CMD_SENDKEYSTEXT       = "SENDKEYSTEXT";

        public static string CMD_SIGNIN             = "SIGNIN";
        public static string CMD_STOP               = "STOP";
        public static string CMD_TABS_CLEAR         = "TABS_CLEAR";
        public static string CMD_TABS_GET           = "TABS_GET";
        public static string CMD_TABS_LOAD          = "TABS_LOAD";
        public static string CMD_TABS_SETTINGS      = "TABS_SETTINGS";

        // DEVICE-SIDE
        public static string CMD_PROFILES           = "PROFILES";
        public static string CMD_PROFILE_UPLOAD     = "PROFILE_UPLOAD";
        public static string CMD_DELETE_PROFILE     = "DELETE_PROFILE";
        public static string CMD_FIT_W              = "FIT_W";
        public static string CMD_FIT_H              = "FIT_H";
        public static string CMD_BGNEXT             = "BGNEXT";
        public static string CMD_LOG                = "LOG";
        public static string CMD_FREEZE             = "FREEZE";
        public static string CMD_FINISH             = "FINISH";
        public static string CMD_RESCALE            = "RESCALE";
        public static string CMD_SERVER             = "SERVER";
        public static string CMD_STATUS             = "STATUS";
        public static string CMD_INVENTORY          = "INVENTORY";

        //}}}
        // CLIENT-SERVER POLL KEY_VAL PARAMS {{{
        private static string KEY_VAL_DEV_DPI   = "DEV_DPI";
        private static string KEY_VAL_DEV_H     = "DEV_H";
        private static string KEY_VAL_DEV_W     = "DEV_W";

        private static string KEY_VAL_DEV_ZOOM  = "DEV_ZOOM";
        private static string KEY_VAL_MON_DPI   = "MON_DPI";
        private static string KEY_VAL_MON_SCALE = "MON_SCALE";
        private static string KEY_VAL_TXT_ZOOM  = "TXT_ZOOM";

        private static string KEY_VAL_MAXCOLORS = "MAXCOLORS";
        private static string KEY_VAL_OPACITY   = "OPACITY";
        private static string KEY_VAL_PALETTE   = "PALETTE";
        private static string KEY_VAL_PROFILE   = "PROFILE";

    //  private static string KEY_VAL_TAG_CMD   = "TAG_CMD";
    //  private static string KEY_VAL_TAG_LYO   = "TAG_LYO";
    //  private static string KEY_VAL_TAG_SFX   = "TAG_SFX";

        // @see DesignerForm ServerForm dispatch_KEY_VAL

        //}}}
        // BUILTIN COMMANDS AND PARAMS {{{

        public static string[] BUILTINS = {
            "#"
                , CMD_BROWSE
                , CMD_RUN
                , CMD_SHELL

                , CMD_PROFILE
                , CMD_SENDINPUT
                , CMD_SENDINPUTTEXT
                , CMD_SENDKEYS
                , CMD_SENDKEYSTEXT
                , "#"
                // PROC_CMD_MAX  = 3
                , CMD_CLEAR
                , CMD_CLOSE
                , CMD_HIDE
                , CMD_LOGGING
            //  , CMD_PALETTES_GET
            //  , CMD_PALETTES_LOAD
                , CMD_PROFILES_TABLE
                , CMD_SIGNIN
                , CMD_STOP
            //  , CMD_TABS_GET
            //  , CMD_TABS_LOAD
                , "#"

                // DEVICE-SIDE
            //  , CMD_BGNEXT
                , CMD_DELETE_PROFILE
                , CMD_FINISH
                , CMD_FIT_H
                , CMD_FIT_W
                , CMD_FREEZE
                , CMD_INVENTORY
                , CMD_LOG
                , CMD_PROFILES
            //  , CMD_RESCALE
                , CMD_SERVER
                , CMD_STATUS
                , "#"

                , CMD_BROWSE +"_UL"
                , CMD_BROWSE +"_UR"
                , CMD_BROWSE +"_DL"
                , CMD_BROWSE +"_DR"
                , CMD_BROWSE +"_L" 
                , CMD_BROWSE +"_C"       // CENTER (HORIZONTAL)
                , CMD_BROWSE +"_R" 
                , CMD_BROWSE +"_U" 
                , CMD_BROWSE +"_M"       // MIDDLE (VERTICAL)
                , CMD_BROWSE +"_D" 
                , CMD_BROWSE +"_F"       // FULLSCREEN
                , CMD_BROWSE +"_CENTER"  // HORIZONTAL & VERTICAL
                , "#"

                , CMD_RUN    +"_UL"
                , CMD_RUN    +"_UR"
                , CMD_RUN    +"_DL"
                , CMD_RUN    +"_DR"
                , CMD_RUN    +"_L" 
                , CMD_RUN    +"_C"       // CENTER (HORIZONTAL)
                , CMD_RUN    +"_R" 
                , CMD_RUN    +"_U" 
                , CMD_RUN    +"_M"       // MIDDLE (VERTICAL)
                , CMD_RUN    +"_D" 
                , CMD_RUN    +"_F"       // FULLSCREEN
                , CMD_RUN    +"_CENTER"  // HORIZONTAL & VERTICAL
                , "#"

                , CMD_SHELL  +"_UL"
                , CMD_SHELL  +"_UR"
                , CMD_SHELL  +"_DL"
                , CMD_SHELL  +"_DR"
                , CMD_SHELL  +"_L" 
                , CMD_SHELL  +"_C"       // CENTER (HORIZONTAL)
                , CMD_SHELL  +"_R" 
                , CMD_SHELL  +"_U" 
                , CMD_SHELL  +"_M"       // MIDDLE (VERTICAL)
                , CMD_SHELL  +"_D" 
                , CMD_SHELL  +"_F"       // FULLSCREEN
                , CMD_SHELL  +"_CENTER"  // HORIZONTAL & VERTICAL
                , "#"

                , KEY_VAL_PROFILE
                , KEY_VAL_DEV_H
                , KEY_VAL_DEV_W
                , KEY_VAL_PALETTE
                , KEY_VAL_OPACITY
                , KEY_VAL_MAXCOLORS

        };

        // https://en.wikipedia.org/wiki/Pixel_density#Calculation_of_monitor_PPI
        //}}}

        // DASH REPORT {{{
        public static string CMD_SENDDASH           = "SENDDASH";
        public static string TESTS_DASH             = "TESTS_DASH";

        //}}}
        // [DELAYS] [TOOLTIPS] [UI] [COMM] {{{

        public const int        CLIPBOARD_INTERVAL          = 1000;
        public const int        CONNECT_TIMEOUT             = 2000;
        public const int        READLINE_FAILED_COOLDOWN    =  500;
        public const int        LOG_TICK_PER_LINE           =   25;
        public const int        SETWINDOWGEOMETRY_DELAY     =  500; //1000;
        public const int        SLEEP_INTERVAL              = 1000;

        public const string     ACK                         = "ACK";
        public const string     KEYS_CAPSLOCK               = "CL";
        public const string     KEYS_NUMLOCK                = "NL";
        public const string     KEYS_SCROLLLOCK             = "SL";

        //}}}
        // PRECESS and UI STATES {{{
    //  public const uint       STATE_DELETE        = 0x0001;

        public const uint       STATE_LAYOUT        = 0x0001;
        public const uint       STATE_EDIT          = 0x0002;

        public const uint       STATE_SETTINGS      = 0x0004;

        public const uint       STATE_EXPORT        = 0x0008;
        public const uint       STATE_IMPORT        = 0x0010;
        public const uint       STATE_PROFILE       = 0x0020;

        public const int        IMPORT_MODE_INSERT  = 0;
        public const int        IMPORT_MODE_OVERLAY = 1;
        public const int        IMPORT_MODE_REPLACE = 2;

        // }}}

        // PROFILE {{{
        // [RTabs_Profiles] [MyDocuments.RTabs] [Profiles] {{{

        public static string RTabs_Profiles
            ="RTabs_Profiles"
            ;

        public static string MyDocumemntsFolder
            = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments )
            + Path.DirectorySeparatorChar +"RTabs"
            ;

        public static string ProfilesFolder
            = MyDocumemntsFolder
            + Path.DirectorySeparatorChar
            +"Profiles"
            ;

        private static Dictionary<string, string> Profiles_Dict  = new Dictionary<string, string>();
        public  static Dictionary<string, string> PalettesDict   = new Dictionary<string, string>();
        public  static Dictionary<string, string> TabsDict       = new Dictionary<string, string>();
/*
:!start explorer "https://msdn.microsoft.com/en-us/library/xfhwa508(v=vs.110).aspx"
:new /LOCAL/STORE/DEV/PROJECTS/DX1Utility_141/Macro/Macro.cs
// read file contents //{{{
                   System.IO.FileStream stream = new System.IO.FileStream(file_path, System.IO.FileMode.Open);
                   string profile_settings = Macro.Read(stream);
                   stream.Close();

//}}}
*/
        //}}}
        // Get_Profiles_Dict .. [profile_mame] [file_path] {{{
        public static void Clear_Profiles_Dict() //{{{
        {
            Profiles_Dict.Clear();
        }
        //}}}
        public static Dictionary<string, string> Get_Profiles_Dict()
        {
            // PROFILE DICTIONARY .. [<profile_mame> <file_path>] {{{
            if(Profiles_Dict.Count < 1)
            {
                Log("Get_Profiles_Dict:");

                // CREATE AND POPULATE PROFILE FOLDER
                //{{{
                if(!System.IO.Directory.Exists(Settings.ProfilesFolder) )
                {
                    Log("...CreateDirectory ProfilesFolder=["+Settings.ProfilesFolder+"]");
                    System.IO.Directory.CreateDirectory( Settings.ProfilesFolder );

                    Log("...COPY DEFAULT PROFILES FROM ["+Settings.RTabs_Profiles+"] to ["+Settings.ProfilesFolder+"]");

                    string profile_to_load = "";
                    string[] fileList = Directory.GetFiles(Settings.RTabs_Profiles, "*");

                    foreach (string f in fileList)
                    {
                        try {
                            string fName = f.Substring(Settings.RTabs_Profiles.Length + 1);
                            Log("COPY ["+fName+"]");

                            File.Copy(Path.Combine(Settings.RTabs_Profiles, fName), Path.Combine(Settings.ProfilesFolder, fName));
                            if((fName == "index.txt") || (profile_to_load == ""))

                                profile_to_load = fName.Replace(".txt", "");
                        }
                        catch(IOException ex) { Log( ex.Message ); }
                    }
                    // LOAD FIRST PROFILE
                    if(profile_to_load != "")
                    {
                            Log("profile_to_load=["+profile_to_load+"]");
                        LoadProfile( profile_to_load );
                    }
                }
                //}}}

                // FOLDER FILES
            //  String[] file_paths = System.IO.Directory.GetFiles(Settings.ProfilesFolder, "*.txt");
                String[] file_paths = System.IO.Directory.GetFiles(Settings.ProfilesFolder, "*.txt", SearchOption.AllDirectories);
                Log( "...file_paths.Length=["+ file_paths.Length +"]");

                //Array.Sort( file_paths );

                string exclude_BAK = Path.DirectorySeparatorChar+"BAK"+Path.DirectorySeparatorChar;
                Log("...exclude_BAK=["+ exclude_BAK +"]");

                int idx_tail = Settings.ProfilesFolder.Length+1;
            //  Log("...idx_tail=["+ idx_tail +"]");

                int count = 0;
                foreach(String file_path in file_paths)
                {
                //  Log("...file_path=["+ file_path +"]");

                    // PROFILE NAMES .. f(file head)
                //  String[] pathComponents = file_path.Split( Path.DirectorySeparatorChar );
                //  pathComponents          = pathComponents.Last().Split('.');
                //  string profile_tail     = pathComponents.First();

                    if( file_path.IndexOf(exclude_BAK) < 0)
                    {
                        string profile_tail     = file_path.Substring( idx_tail );
                    //  Log("...profile_tail=["+ profile_tail +"]");

                    //  String[] pathComponents = profile_tail.Split( Path.DirectorySeparatorChar );
                    //  pathComponents          = pathComponents.Last().Split('.');
                    //  string profile_name     = pathComponents.First();

                        string profile_name = profile_tail.Substring(0, profile_tail.Length-4).Replace(Path.DirectorySeparatorChar.ToString(),"/");
                        Log(String.Format("...{0,3} - profile_name=[{1}]", ++count, profile_name));

                        try {
                            Profiles_Dict.Add(profile_name, file_path);
                        }
                        catch(Exception) { }
                    }
                }
            }
            //}}}
            return Profiles_Dict;
        }
        //}}}

    // }}}

        // IMPORT-EXPORT {{{
        // SaveProfile {{{
        public static void SaveProfile(string file_path, string file_text)
        {
            Log("SaveProfile("+ file_path +", file_text (Length="+ file_text.Length+")");
            try {
                int idx = file_path.LastIndexOf( Path.DirectorySeparatorChar );
                if(idx > 0)
                {
                    // create missing sub_dirs
                    string dir_path = file_path.Substring(0, idx);
                    Log("...checking dir_path=["+ dir_path +"]:");
                    if( !Directory.Exists(dir_path) )
                    {
                        DirectoryInfo di = Directory.CreateDirectory(dir_path);
                        Log("...Directory.CreateDirectory("+ dir_path +") ...done at {0}.", Directory.GetCreationTime( dir_path ).ToString());
                    }
                }
                File.WriteAllText(file_path, file_text);
                Log("...File.WriteAllText() ...done");
            }
            catch(ApplicationException ex)
            {
                MessageBox.Show("SaveProfile("+ file_path +"):\n"
                    + Settings.ExToString(ex)
                    , Settings.PROFILE
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information
                    );
            }
            catch(Exception ex) {
                Log("*** SaveProfile("+file_path+") Exception:\n"+ ex);
            }
        }
        //}}}
        // GetProfilePath {{{
        public static string GetProfilePath(string profile_name)
        {
            Log("GetProfilePath("+ profile_name +")");

            Get_Profiles_Dict();

            string filePath = "";

            // TODO (200719) prioritize relative current profile first
            // i.e.:
            // LOADING [index]
            // ...FROM [samples/sample1]
            // .....TO [samples/index] .. instead of [/index]

            if( Profiles_Dict.ContainsKey( profile_name ) )
                filePath = Profiles_Dict[ profile_name ];
            else
                Log("*** GetProfilePath("+ profile_name +") ...not found in Profiles_Dict");

            Log("GetProfilePath("+ profile_name +") ...return["+ filePath +"]");
            return filePath;
        }
        //}}}
        // GetProfileText {{{
        //{{{
        static private bool     Siphoning_ProfileLines = false;
        static private string   ProfileLines          = "";
        static public  int      ImportMode             = IMPORT_MODE_REPLACE;
        static public  int      ImportOffsetX          = 0;
        static public  int      ImportOffsetY          = 0;

        //}}}
        static public  string   GetProfileText()
        {
            Log("GetProfileText():");

            if(MainFormInstance == null) return "";

            ProfileLines           = "";

            Siphoning_ProfileLines = true;
            MainFormInstance.saveSettings("GetProfileText");
            Siphoning_ProfileLines = false;

            Log("...ProfileLines "+(ProfileLines.Length / 1000)+"kb");
            return ProfileLines;
        }
        //}}}

        // ListProfiles {{{
        public static string ListProfiles()
        {
            StringBuilder sb = new StringBuilder();

            Dictionary<string, string>.KeyCollection profile_names = Profiles_Dict.Keys;

            foreach(string profile_name in profile_names)
                sb.Append( String.Format("{0,32} = {1}\n", profile_name, Profiles_Dict[profile_name]) );

            return sb.ToString();
        }
        //}}}

        // LoadProfile {{{
        public static void LoadProfile(string profile_name)
        {
            Log("LoadProfile("+ profile_name +")");

            // FIRST LOAD ERROR WILL REFRESH PROFILES LIST
            for(int trial=0; trial < 2; ++trial)
            {
                Get_Profiles_Dict();

                try {
                    // INIT SETTINGS KEY_VAL .. (TO BE SET BY PROFILE'S IMPORTED DATA) {{{
                    Settings.PRODATE           =  0;

                    Settings.TAG_CMD           = "";
                    Settings.TAG_LYO           = "";
                    Settings.TAG_SFX           = "";

                    //}}}
                    // PROFILE QUALIFIER {{{
                    Log("...Settings.PROFILE=["+ Settings.PROFILE +"]");

                    string current_dirName
                        = (Settings.PROFILE == "")
                        ?  ""
                        :  Path.GetDirectoryName( Settings.PROFILE ).Replace("\\","/");

                    string profile_dirName
                        =   Path.GetDirectoryName( profile_name     ).Replace("\\","/");

                    profile_name
                        =  profile_name.Replace("\\","/");

                    bool  fully_qualified
                        =  (profile_name[0] == '/')
                        || (profile_dirName == current_dirName);

                    Log(".......profile_name=["+ profile_name     +"]");
                    Log("....profile_dirName=["+ profile_dirName  +"]");
                    Log("....current_dirName=["+ current_dirName  +"]");
                    Log("....fully_qualified=["+ fully_qualified  +"]");

                    //}}}
                    // 1/2 - TRY RELATIVE TO CURRENT_DIRNAME .. RESCAN FOLDER ONCE TO UPDATE CACHED LIST {{{
                    bool profile_loaded = false;
                    if(!fully_qualified && (current_dirName != ""))
                    {
                        string relative_name   = current_dirName+"/"+profile_name;
                        Log(".....relative_name=["+ relative_name   +"]");

                        if( Profiles_Dict.ContainsKey( relative_name ) )
                        {
                            string        filePath = Profiles_Dict[ relative_name ];
                            Log("...LOADING RELATIVE ["+ filePath +"]");

                            profile_loaded         = LoadProfileFromFilePath( filePath );

                            if( profile_loaded )
                                Settings.PROFILE   = relative_name;
                        }
                        else if(trial == 0)
                        {
                            Log("...MISSING RELATIVE ["+ relative_name +"] .. REFRESHING [Profiles_Dict]");
                            Clear_Profiles_Dict();
                            continue;
                        }
                    }
                    //}}}
                    // 2/2 - TRY ABSOLUTE {{{
                    if(!profile_loaded)
                    {
                        if(profile_name[0] == '/')
                            profile_name       = profile_name.Substring(1);
                        string  filePath       = Profiles_Dict[ profile_name ];
                        Log("...LOADING ABSOLUTE filePath=["+ filePath +"]");

                        profile_loaded         = LoadProfileFromFilePath( filePath );

                        if( profile_loaded )
                            Settings.PROFILE   = profile_name;
                    }
                    //}}}
                    // ... - SKIP SECOND TRIAL WHEN FIRST SUCCEEDS {{{

                    Log("........profile_loaded: "+ Settings.PROFILE    +"]");
                    Log("...Profiles_Dict.Count: "+ Profiles_Dict.Count +"]");

                    if(profile_loaded || (Profiles_Dict.Count > 0)) // ...no exception cleanup occured
                        break;                                      // ...proceed to second trial otherwise
                    //}}}
                }
                catch(Exception ex)
                {
                    // RESCAN FOLDER ONCE TO UPDATE CACHED LIST {{{
                    if(trial == 0) {
                        Log( "*** LoadProfile("+ profile_name +") CLEARING [Profiles_Dict] ***\n");
                        Clear_Profiles_Dict();
                    }
                    else {
                        Log( "*** LoadProfile("+ profile_name +") (folder scan updated) ***\n"
                            + ex.Message +"\n"
                          //+"@@@\n"
                          //+ ListProfiles()
                          //+"@@@\n"
                           );
                    }
                    //}}}
                }
            }
        }
        //}}}
        // LoadProfileFromFilePath {{{
        private static bool LoadProfileFromFilePath(string filePath)
        {
            Log("LoadProfileFromFilePath(filePath=["+ filePath +"]):");

            // *** requires a panel content to parse from
            if(MainFormInstance == null) return false;
            // ACCESS FILE // {{{
            Log("...filePath=["+filePath+"]");
            if( !System.IO.File.Exists( filePath ) ) {
                Log("*** file not found: ["+ filePath +"]");
                return false;

            }
            // }}}
            // INITIALIZE SETTINGS {{{
            if(ImportMode == IMPORT_MODE_REPLACE) {
                Log("...REPLACING current Settings:");
                MainFormInstance.tabsCollection.delete_usr_tabs();
            }
            else if(ImportMode == IMPORT_MODE_INSERT) {
                Log("...ADDING-TO current Settings:");
            }
            else {  // IMPORT_MODE_OVERLAY
                Log("...MERGING-OVER current Settings:");
            }

            TabsDict.Clear();
            //}}}
            //if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0)) MainFormInstance.tabsCollection.tabs_container_SuspendLayout();
            // LOAD FILE LINES {{{
            System.IO.StreamReader reader = null;
            int count = 0;
            try {
                reader = new System.IO.StreamReader( filePath );
                string line = reader.ReadLine();
                do {
                    if(line == null) break;
                    ++count;
                    if(line.Length > 0)
                        LoadProfileLine( line );
                    line = reader.ReadLine();
                }
                while(line != null);
            }
            catch(Exception ex) {
                Log("*** LoadProfileFromFilePath("+filePath+") Exception:\n"+ ex);
            }
            finally {
                if(reader != null)
                    reader.Close();
            }
            //}}}
            //if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0)) MainFormInstance.tabsCollection.tabs_container_ResumeLayout();
            Log("LoadProfileFromFilePath(filePath=["+ filePath +"]) ..."+ count +" lines loaded");

            return true;
        }
        //}}}
        // LoadProfileFromText {{{
        static public  void   LoadProfileFromText(string text)
        {
            // parse content {{{
            string[] lines  = text.Split('\n');
            Log("LoadProfileFromText: importing "+ lines.Length +" line"+((lines.Length > 1) ? "s" : "")+":");

            //}}}
            // INITIALIZE SETTINGS {{{
            if(ImportMode == IMPORT_MODE_REPLACE) {
                Log("...REPLACING current Settings:");
                MainFormInstance.tabsCollection.delete_usr_tabs();
            }
            else if(ImportMode == IMPORT_MODE_INSERT) {
                Log("...ADDING-TO current Settings:");
            }
            else {  // IMPORT_MODE_OVERLAY
                Log("...MERGING-OVER current Settings:");
            }

            // KEY_VAL IS TO BE SET BY IMPORTED DATA
            Settings.TAG_CMD = "";
            Settings.TAG_LYO = "";
            Settings.TAG_SFX = "";

            // DEFAULT TOP LEFT & #COLS .. until it's adjuste by a TAG_LYO option
            int cols = 1+ (int)Math.Sqrt( (double)lines.Length );
            TabsCollection.ResetAutoPlace_x_y_cols(Settings.ImportOffsetX, Settings.ImportOffsetY, cols);

            TabsDict.Clear();
            //}}}
            //if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0)) MainFormInstance.tabsCollection.tabs_container_SuspendLayout();
            // LOAD TEXT LINES {{{
            for(int l=0; l < lines.Length; ++l)
            {
                string line = lines[l].Trim();
                if(line.Length > 0)
                    LoadProfileLine( line );

            }
            //}}}
            //if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0)) MainFormInstance.tabsCollection.tabs_container_ResumeLayout();
        }
        // }}}

        // LoadProfileLine {{{
        static private void   LoadProfileLine(string argLine)
        {
            Log("LoadProfileLine("+ argLine +"):");  // VERBOSE!

            // # COMMENT LINE
            // KEY_VAL {{{
            if(argLine.StartsWith("#"))
            {
                // # PROFILE=ProfileName
                // # POSIX_TIME=GetUnixTimeSeconds
                if( set_KEY_VAL("LoadProfileLine", "#", argLine) != "")
                {
                    // ADJUST AUTO PLACE LAYOUT #COLS
                    if(Settings.TAG_LYO != "")
                    {
                        try {
                            int cols = int.Parse(Settings.TAG_LYO);
                            TabsCollection.ResetAutoPlace_x_y_cols(Settings.ImportOffsetX, Settings.ImportOffsetY, cols);
                        }
                        catch(Exception ex) {
                            Log("LoadProfileLine: TAG_LYO=["+ TAG_LYO +"]: "+ ex.Message);
                            MessageBox.Show("Setings.LoadProfileLine(TAG_LYO=["+ TAG_LYO +"]):\n"
                                + Settings.ExToString(ex) +"\n"
                                , "LoadProfileLine"
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Information
                                );
                        }

                    }
                    return;
                }
                // not an hex color TODO: regex [#\d{6,}]
                else if(argLine.StartsWith("# ")) {
                    return;
                }
            }
            //}}}

            // [APP_NAME] NAME = VALUE
            //{{{
            // ...[RTabsDesigner] APP_XYWH=1280,0,1280,1400

            // FIRST '=' AS NAME = VALUE SEPARATOR
            string name  = "";
            string value = "";

            int idx = argLine.IndexOf("=");
            if(idx > 0) {
                name  = argLine.Substring(0,idx  ).Trim('"').Trim();
                value = argLine.Substring(  idx+1).Trim('"').Trim();

            }
            // OPTIONAL APPLICATION LINE HEADER
            if(name.StartsWith("[")) {
                idx = name.IndexOf("]");
                if(idx > 0)
                    name = name.Substring(idx+1).Trim();

            }

            //  Log("LoadProfileLine("+ argLine            +"):\n"
            //      +                 "     name=["+ name  +"]\n"
            //      +                 "    value=["+ value +"]");

            //}}}

            // RUNTIME UI SETTINGS
            // (IGNORED) {{{

            // ...APP_XYWH = 1280,0,1280,1400
            // .....SplitX = 790
            // .......HIDE = False
            // ....DEV_DPI = 240

            //  if(name.StartsWith("APP_XYWH" ))  { Settings.APP_XYWH     = value;      MainFormInstance.apply_Settings_APP_XYWH(); }
            //  if(name.StartsWith("SplitX"      )) try{ Settings.SplitX  =  int.Parse(value); } catch(Exception ex) { Log("*** ["+name+"]=["+value+"]: "+ ex.Message+"\n"); }
            //  if(name.StartsWith("HIDE"        )) try{ Settings.HIDE    = bool.Parse(value); } catch(Exception ex) { Log("*** ["+name+"]=["+value+"]: "+ ex.Message+"\n"); }
            //  if(name.StartsWith("LOGGING"     )) try{ Settings.LOGGING = bool.Parse(value); } catch(Exception ex) { Log("*** ["+name+"]=["+value+"]: "+ ex.Message+"\n"); }
            //  if(name         == "DEV_DPI"      ) set_KEY_VAL("LoadProfileLine", "IMPORT", argLine);
            //  if(name         == "DEV_H"        ) set_KEY_VAL("LoadProfileLine", "IMPORT", argLine);
            //  if(name         == "DEV_W"        ) set_KEY_VAL("LoadProfileLine", "IMPORT", argLine);
            //  if(name         == "DEV_ZOOM"     ) set_KEY_VAL("LoadProfileLine", "IMPORT", argLine);
            //  if(name         == "MON_DPI"      ) set_KEY_VAL("LoadProfileLine", "IMPORT", argLine);
            //  if(name         == "MON_SCALE"    ) set_KEY_VAL("LoadProfileLine", "IMPORT", argLine);
            //  if(name         == "TXT_ZOOM"     ) set_KEY_VAL("LoadProfileLine", "IMPORT", argLine);
            //  if(name         == "OPACITY"      ) set_KEY_VAL("LoadProfileLine", "IMPORT", argLine);
            //  if(name         == "PALETTE"      ) set_KEY_VAL("LoadProfileLine", "IMPORT", argLine);

            //}}}

            // COLOR PALETTES
            //{{{

            // ...PALETTE.10 = PALETTE_NAME, #COLOR_HEX,#COLOR_HEX,#COLOR_HEX,#COLOR_HEX,#COLOR_HEX

            if(name.StartsWith("PALETTE."))
            {
                if( !NotePane.LoadColorPaletteLine( value ) )
                    Log("** ["+name+"]=["+value+"]: Palette ["+ name +"] already defined\n");
            }

            //}}}

            // TABS [DESIGNER] [SERVER] .. [STANDARD PROFILE FORMAT]
            //{{{
            // ...TAB.panel_usr10 = [type=SHORTCUT|tag=0164914961|zoom=1|xy_wh=87,22,9,3|text=0164914961|tt=Tél. Perso]
            else if(name.StartsWith("TAB."))
            {
/*
                string tab_name = name.Substring(4);
                // RENAME DUPLICATE [tab_name]
                if( TabsDict.ContainsKey( tab_name ) ) {
                    string free_tab_name = MainFormInstance.tabsCollection.get_free_tab_name();
                        Log("* RENAMING DUPLICATE from ["+ tab_name +"] to ["+ free_tab_name +"]");
                        Log("* ["+tab_name+"]=["+value+"]\n");
                        tab_name = free_tab_name;
                }
*/
                string tab_name = NotePane.PANEL_NAME_USR + (TabsDict.Count+1).ToString("D3");

                // [DESIGNER] .. import
                if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
                {
                    if(MainFormInstance.tabsCollection.import_tab_line(tab_name, value) == null)
                        Log("** ["+tab_name+"]=["+value+"]: Tab ["+ tab_name +"] not imported\n");
                }

                // [SERVER] .. insert into current server collection .. FIXME ? usefulness for [DESIGNER]
                try {
                    //Log("@@@ LoadColorPaletteLine: TabsDict.Add(tab_name=["+tab_name+"], value=["+value+"])");
                    TabsDict.Add(tab_name, name + NotePane.TABVALUE_SEPARATOR + value);
                }
                catch(Exception ex) {
                    Log("* TabsDict.Add("+ tab_name +") *** "+ ex.Message +" ***");
/*
                    MessageBox.Show("Setings.LoadProfileLine(tab_name=["+ tab_name +"]):\n"
                        + Settings.ExToString(ex) +"\n"
                        + value.Replace("|","\n")
                        , Settings.PROFILE
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Information
                        );
*/
                }

            }

            // }}}
            // TABS - [DESIGNER] .. [PARSE NOT FORMATED DATA]
            // {{{
/* // {{{
:new Util/RTabs_Profiles

# TAG_CMD=SENDKEYS TAG_SFX={ENTER}
/angry
# TAG_CMD=PROFILE TAG_SFX=""
ESO
dash
ADB
# TAG_CMD=SENDKEYS TAG_SFX={ENTER}
/approve

*/ // }}}
            else if((Settings.APP_NAME.ToUpper().IndexOf("DESIGNER") >= 0))
            {
            string tab_name = MainFormInstance.tabsCollection.get_free_tab_name();
                //string tab_name = NotePane.PANEL_NAME_USR + (TabsDict.Count+1).ToString("D3");
                value = argLine;

                Log("* ["+tab_name+"]=["+value+"]:");

                NotePane np = MainFormInstance.tabsCollection.import_data_line(tab_name, value);
                if(np != null)
                    Log("import_data_line(tab_name["+ tab_name +"], value=["+ value +"] returned:\n"+ np.ToString());
                else
                    Log("** ["+tab_name+"]=["+value+"]: Tab ["+ tab_name +"] not imported\n");
            }
            // }}}

        }
        //}}}

        // IsASettingsLine {{{
        static private bool   IsASettingsLine(string argLine)
        {
            bool    diag    = false;
            string  s       = argLine;
            if(!diag && s.StartsWith("#"       )) diag = true;
            if(!diag && s.StartsWith("["       )) { int idx = s.IndexOf("]"); if(idx > 0) s = s.Substring(idx+1).Trim(); }
            if(!diag && s.StartsWith("PALETTE.")) diag = true;
            if(!diag && s.StartsWith("TAB."    )) diag = true;
            Log("IsASettingsLine("+ argLine +") ...return "+ diag +"");
            return diag;
        }
        //}}}
            // TODO: IMPORT-TEMPLATE (text = tag = tt) {{{
/* // {{{
==> Util/Settings/Excel.txt <==
#:!start explorer "https://support.office.com/en-us/article/Excel-shortcut-and-function-keys-1798d9d5-842a-42b8-9c99-9b7213f0040f" 
#text           = tag         = tt 
CTRL+PgUp       = ^{PGUP}     = Switches between worksheet tabs, from left-to-right
CTRL+PgDn       = ^{PGDN}     = Switches between worksheet tabs, from right-to-left
CTRL+SHIFT+(    = ^+(         = Unhides any hidden rows within the selection
CTRL+SHIFT+)    = ^+)         = Unhides any hidden columns within the selection
CTRL+SHIFT+&    = ^+&         = Applies the outline border to the selected cells
CTRL+SHIFT_     = ^+_         = Removes the outline border from the selected cells
CTRL+SHIFT+~    = ^+~         = Applies the General number format
CTRL+SHIFT+$    = ^+$         = Applies the Currency format with two decimal places (negative numbers in parentheses)

==> Util/Settings/SendKeys.txt <==
#:!start explorer "https://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys(v=vs.110).aspx"
#text            = tag
BACKSPACE        = {BACKSPACE}, {BS}, or {BKSP}
#text            = tag
BREAK            = {BREAK}
CAPS LOCK        = {CAPSLOCK
DEL or DELETE    = {DEL} or {DELETE}
DOWN ARROW       = {DOWN}
END              = {END}
ENTER            = {ENTER}or ~
ESC              = {ESC}
HELP             = {HELP}

*/ // }}}
/* // {{{
    {BACKSPACE}
    {BREAK}
    {CAPSLOCK}
    {DEL}
    {DOWN}
    {END}
    {ENTER}
    {ESC}
    {HELP}
    {HOME}
    {INSERT}
    {LEFT}
    {NUMLOCK}
    {PGDN}
    {PGUP}
    {PRTSC}
    {RIGHT}
    {SCROLLLOCK}
    {TAB}
    {UP}
    {F1}
    {F2}
    {F3}
    {F4}
    {F5}
    {F6}
    {F7}
    {F8}
    {F9}
    {F10}
    {F11}
    {F12}
    {F13}
    {F14}
    {F15}
    {F16}
    {ADD}
    {SUBTRACT}
    {MULTIPLY}
    {DIVIDE}
*/ // }}}
            // }}}
    // }}}

        // TODO - USEFUL TOOLTIPS
        // TOOLTIPS {{{
/*

        public const string     CONTROL_LAYOUT_TT =
            "Click to display content";

        public const string     CONTROL_EDIT_TT =
            "Double-click to edit\n"
            +"Left -click to disable button action\n"
            +"Right-click to decode Rtf\n"
            +"Midle-click to render Rtf\n"
            +"Delete all to restore original content\n"
            +"...Escape when done"
            ;

*/
        //}}}
        public static void Beep(string caller) // {{{
        {
            Log( caller );
            SystemSounds.Beep.Play();
        }
        // }}}

        // DESIGNER AND SERVER REGISTRY SETTINGS
        // CLASS VARIABLES {{{
        public  static MainForm MainFormInstance  = null;

        // DEV_ZOOM .. NotePane.RichTextBox.ZoomFactor adapter {{{
        public static double DEV_ZOOM      =  1.0; // CURRENT SELECTION
        public static double DEV_ZOOM_P    =  1.0; // PROFILES WINDOW

        public static double DEV_ZOOM_1    =  0.65;
        public static double DEV_ZOOM_2    =  0.70;
        public static double DEV_ZOOM_3    =  0.75;
        public static double DEV_ZOOM_4    =  0.80;
        public static double DEV_ZOOM_5    =  0.85;
        public static double DEV_ZOOM_6    =  0.90;
        public static double DEV_ZOOM_7    =  1.00;
        public static double DEV_ZOOM_8    =  1.05;
        public static double DEV_ZOOM_9    =  1.10;
        public static double DEV_ZOOM_0    =  1.15;

        public static double[] DEV_ZOOMS = {
            DEV_ZOOM_1,
            DEV_ZOOM_2,
            DEV_ZOOM_3,
            DEV_ZOOM_4,
            DEV_ZOOM_5,
            DEV_ZOOM_6,
            DEV_ZOOM_7,
            DEV_ZOOM_8,
            DEV_ZOOM_9,
            DEV_ZOOM_0
        };

        // https://en.wikipedia.org/wiki/Pixel_density#Calculation_of_monitor_PPI
        //}}}
        // TXT_ZOOM .. NotePane.RichTextBox.ZoomFactor {{{
        public static double TXT_ZOOM      =  1.0; // CURRENT SELECTION
        public static double TXT_ZOOM_P    =  1.0; // PROFILES WINDOW

        // https://msdn.microsoft.com/en-us/library/system.windows.forms.richtextbox.zoomfactor(v=vs.110).aspx
        // The value of this property can be between 1/64 (0.015625) and 64.0, not inclusive.
        // A value of 1.0 indicates that no zoom is applied Math.Round(to the control. 

        public static double TXT_ZOOM_1    =  1.00; //0.60;
        public static double TXT_ZOOM_2    =  1.20; //0.70;
        public static double TXT_ZOOM_3    =  1.30; //0.80;
        public static double TXT_ZOOM_4    =  1.40; //0.90;
        public static double TXT_ZOOM_5    =  1.50; //1.00; // NO ZOOM
        public static double TXT_ZOOM_6    =  1.60; //1.10;
        public static double TXT_ZOOM_7    =  1.70; //1.20;
        public static double TXT_ZOOM_8    =  1.80; //1.30;
        public static double TXT_ZOOM_9    =  1.90; //1.40;
        public static double TXT_ZOOM_0    =  2.00; //1.50;

        public static double[] TXT_ZOOMS   = {
            TXT_ZOOM_1,
            TXT_ZOOM_2,
            TXT_ZOOM_3,
            TXT_ZOOM_4,
            TXT_ZOOM_5,
            TXT_ZOOM_6,
            TXT_ZOOM_7,
            TXT_ZOOM_8,
            TXT_ZOOM_9,
            TXT_ZOOM_0
        };

        // https://en.wikipedia.org/wiki/Pixel_density#Calculation_of_monitor_PPI
        //}}}
        // RESOLUTION - MON_DPI DEV_DPI .. (Dot per inch) {{{
        public static string SOURCE         = ""; // last KEY_VAL-SOURCE

        public static int    MON_DPI        =  96; // CURRENT SELECTION
        public static int    DEV_DPI        = 240; // CURRENT SELECTION
        public static int    DEV_DPI_P      = 240; // PROFILES WINDOW

        public static int    DEV_DPI_1      =  96; // Desktop
        public static int    DEV_DPI_2      = 120; //    LDPI
        public static int    DEV_DPI_3      = 160; //    MDPI
        public static int    DEV_DPI_4      = 210; //   TVDPI
        public static int    DEV_DPI_5      = 240; //    HDPI
        public static int    DEV_DPI_6      = 320; //   XHDPI
        public static int    DEV_DPI_7      = 480; //  XXHDPI
        public static int    DEV_DPI_8      = 640; // XXXHDPI

        public static double ratio          = 1; // = (double)Settings.MON_DPI/ (double)Settings.DEV_DPI *  Settings.MON_SCALE;
        public static int    scaledGridSize =  TabsCollection.TAB_GRID_S;

        public static string[] DEV_DPI_NAMES  = {
            ""+DEV_DPI_1,
            ""+DEV_DPI_2,
            ""+DEV_DPI_3,
            ""+DEV_DPI_4,
            ""+DEV_DPI_5,
            ""+DEV_DPI_6,
            ""+DEV_DPI_7,
            ""+DEV_DPI_8
        };

        // https://en.wikipedia.org/wiki/Pixel_density#Calculation_of_monitor_PPI
        //}}}
        // RESOLUTION - DEV_W DEV_H .. Width x Height (pixels) {{{

        // -----------------------------------------------------------------
        // VGA (Video Graphics Array) ----------------  NAME     X:Y   X×Y -
        // -----------------------------------------------------------------
        public static int DEV_W    = 1920, DEV_H    =  1200; // CURRENT SELECTION (Xperia Z2 Tablet)
        public static int DEV_W_P  = 1920, DEV_H_P  =  1200; // PROFILES WINDOW

        public static int DEV_W_1  =  640, DEV_H_1  =   480; // VGA     4:3   0.307
        public static int DEV_W_2  =  768, DEV_H_2  =   480; // WVGA   16:10  0.368
        public static int DEV_W_3  =  800, DEV_H_3  =   600; // SVGA    4:3   0.480
        public static int DEV_W_4  = 1024, DEV_H_4  =   576; // WSVGA  16:9   0.590
        public static int DEV_W_5  = 1280, DEV_H_5  =   720; // HD     16:9   0.922
        public static int DEV_W_6  = 1600, DEV_H_6  =   900; // HD+    16:9   1.440
        public static int DEV_W_7  = 1920, DEV_H_7  =  1080; // FHD    16:9   2.074
        public static int DEV_W_8  = 1920, DEV_H_8  =  1200; // WUXGA  16:10  2.304
        public static int DEV_W_9  = 2560, DEV_H_9  =  1440; // WQHD   16:9   3.686
        public static int DEV_W_10 = 2560, DEV_H_10 =  1600; // WQXGA  16:10  4.096
        public static int DEV_W_11 = 3200, DEV_H_11 =  1800; // WQXGA+ 16:9   5.760
        public static int DEV_W_12 = 3840, DEV_H_12 =  2160; // UHD    16:9   8.294
        public static int DEV_W_13 = 5120, DEV_H_13 =  2880; // UHD+   16:9  14.746
        public static int DEV_W_14 = 7680, DEV_H_14 =  4320; // FUHD   16:9  33.178
        public static int DEV_W_15 =15360, DEV_H_15 =  8640; // QUHD   16:9 132.71

        public static double[] DEV_WIDTHS   = { DEV_W_1, DEV_W_2, DEV_W_3, DEV_W_4, DEV_W_5, DEV_W_6, DEV_W_7, DEV_W_8, DEV_W_9, DEV_W_10 , DEV_W_11, DEV_W_12, DEV_W_13, DEV_W_14, DEV_W_15 };
        public static double[] DEV_HEIGHTS  = { DEV_H_1, DEV_H_2, DEV_H_3, DEV_H_4, DEV_H_5, DEV_H_6, DEV_H_7, DEV_H_8, DEV_H_9, DEV_H_10 , DEV_H_11, DEV_H_12, DEV_H_13, DEV_H_14, DEV_H_15 };

//      public static int DEV_W_1  =  480, DEV_H_1  =   320; // HVGA    3:2   0.154
//      public static int DEV_W_2  =  640, DEV_H_2  =   360; // nHD    16:9   0.230
//      public static int DEV_W_6  =  960, DEV_H_6  =   540; // qHD    16:9   0.518
//      public static int DEV_W_7  =  960, DEV_H_7  =   640; // DVGA    3:2   0.614


// :!start explorer "https://en.wikipedia.org/wiki/Graphics_display_resolution\#Video_Graphics_Array"

        // }}}
        // MON_SCALE .. Monitor rendering accounting for Device Dpi & WxH (pixel) {{{
        public static double MON_SCALE      =  1.0; // CURRENT SELECTION
        public static double MON_SCALE_P    =  1.0; // PROFILES WINDOW

        public static double MON_SCALE_1    = 0.10; // i.e. DEV_DPI=96  @ 15360x8640  .. scale 1/10 rendering == 1536x864
        public static double MON_SCALE_2    = 0.20; // i.e. DEV_DPI=640 @ 2560x1600   .. scale 1/20 rendering ==  768x480
        public static double MON_SCALE_3    = 0.30;
        public static double MON_SCALE_4    = 0.50;
        public static double MON_SCALE_5    = 0.75;
        public static double MON_SCALE_6    = 1.00;
        public static double MON_SCALE_7    = 1.25;
        public static double MON_SCALE_8    = 1.50;
        public static double MON_SCALE_9    = 2.00;
        public static double MON_SCALE_0    = 4.00; // i.e. DEV_DPI=96  @ 480x320     ..  scale  x4 rendering == 1920x1280

        public static double[] MON_SCALES   = {
            MON_SCALE_1,
            MON_SCALE_2,
            MON_SCALE_3,
            MON_SCALE_4,
            MON_SCALE_5,
            MON_SCALE_6,
            MON_SCALE_7,
            MON_SCALE_8,
            MON_SCALE_9,
            MON_SCALE_0
        };

        // https://en.wikipedia.org/wiki/Pixel_density#Calculation_of_monitor_PPI
        //}}}

        // COMM {{{
        public  static string    IP                      = "";//"192.168.1.2";
        public  static int       Port                    =  4001;
        public  static string    Password                = "pass";
        public  static string    MAC                     = "";
        public  static string    SUBNET                  = "";

        // }}}
        // numbers {{{
        //  public  static int      PollInterval            = 2000;

        // }}}
        // strings {{{
        public  static string    APP_XYWH                =  "";
        public  static string    PALETTE                 =  "";
        public  static string    TAG_LYO                 =  "";
        public  static string    TAG_CMD                 =  "";
        public  static string    TAG_SFX                 =  "";
    //  public  static string    OOB_CMD                 =  "";
        public  static string    PROFILE                 =  "";
        public  static int       PRODATE                 =   0;
        public  static int       MAXCOLORS               =   0;
        public  static int       OPACITY                 = 100;
        public  static int       SplitX                  = 100;
        public  static string    KEY_VAL_HISTORY         =  "";

        // }}}
        // bool {{{
        public  static bool      LOGGING                 = false;
        public  static bool      HIDE                    = false;

    //  public  static bool      Connected               = false;
        public  static bool      Send_input_events       = false;
        public  static bool      Sleep                   = false;

        // }}}
        // }}}
        // THIS APPLICATION PROPERTIES {{{

        public  static  string      PROJECT_NAME        = "RTabs";

        public  static  string      DESIGNER_APP_NAME   = PROJECT_NAME +  "Designer";
        public  static  string      DESIGNER_APP_TITLE  = PROJECT_NAME + " Designer";

        public  static  string      SERVER_APP_NAME     = PROJECT_NAME +  "Server";
        public  static  string      SERVER_APP_TITLE    = PROJECT_NAME + " Server";


        static public string _APP_HELP_TEXT = ""; public static string APP_HELP_TEXT { get { return _APP_HELP_TEXT; } set { _APP_HELP_TEXT  = value; } }
        static public string _APP_INIT_TEXT = ""; public static string APP_INIT_TEXT { get { return _APP_INIT_TEXT; } set { _APP_INIT_TEXT  = value; } }
        static public string _APP_TITLE     = ""; public static string APP_TITLE     { get { return _APP_TITLE;     } set { _APP_TITLE      = value; } }
        static public string _APP_NAME      = ""; public static string APP_NAME      { get { return _APP_NAME;      } set { _APP_NAME       = value; Create_App_SubKey( _APP_NAME ); } }

        //}}}
        // REGISTRY SUBKEY {{{
        private static Dictionary<string, RegistryKey> App_SubKey_Dict = new Dictionary<string, RegistryKey>();
        private static bool     This_App_SubKey_Warn_Acknowledged = false;

        private static void Create_App_SubKey(string app_name)// {{{
        {
            try {
                // Creates a new subkey or opens an existing subkey for write access
                App_SubKey_Dict.Add(app_name, Registry.CurrentUser.CreateSubKey( app_name ));
            }
            catch(Exception ex) {
                Log("LoadSettings(): "+ex.Message);
            }
        }
        // }}}
        private static bool Got_This_App_SubKey(string app_name)// {{{
        {
            if(App_SubKey_Dict.ContainsKey( app_name ) )
                return true;

            Create_App_SubKey( app_name );

            if(App_SubKey_Dict.ContainsKey( app_name ) )
                return true;

            // STILL MISSING ... WARN JUST ONCE
            if(    !App_SubKey_Dict.ContainsKey( app_name )
                && !This_App_SubKey_Warn_Acknowledged
              ) {
                MessageBox.Show("Settings:"  +  Environment.NewLine
                    + "Settings(app_name) should have been called first" +  Environment.NewLine
                    + "app_name=["+ app_name +"]"
                    , "Settings"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error
                    );

                This_App_SubKey_Warn_Acknowledged  = true;
            }
            return false;
        }
        // }}}
        private static RegistryKey get_App_SubKey(string app_name) //{{{
        {
            return App_SubKey_Dict[app_name];
        }
        //}}}
        public  static void ReopenSettings(string app_name)// {{{
        {
            if( !Got_This_App_SubKey(app_name) ) return;

            RegistryKey app_SubKey = App_SubKey_Dict[ app_name ];

            if(app_SubKey == null) return;

            App_SubKey_Dict.Remove(app_name);

            Log("ReopenSettings: App_SubKey_Dict.Remove("+ app_name +") done");
        }
        // }}}
        //}}}
        // PER-APPLICATION REGISTRY SETTING IO {{{
        public  static string[] GetValueNames(string app_name)// {{{
        {
            if( !Got_This_App_SubKey(app_name) ) return new string[0];
            RegistryKey app_SubKey = get_App_SubKey( app_name );
            return app_SubKey.GetValueNames();
        }
        // }}}
        // LoadSetting {{{

        public  static string LoadSetting(                 string name                      ) { return LoadSetting(_APP_NAME, name, ""           ); }
        public  static string LoadSetting(                 string name, string default_value) { return LoadSetting(_APP_NAME, name, default_value); }
        public  static string LoadSetting(string app_name, string name, string default_value)
        {
            if( !Got_This_App_SubKey(app_name) ) return default_value;

            RegistryKey app_SubKey = get_App_SubKey( app_name );
            if(app_SubKey == null) return default_value;

            try   { return app_SubKey.GetValue(name).ToString(); }
            catch { return default_value; }

        }
        // }}}
        public  static   void SaveSetting(string app_name, string name, string value)// {{{
        {
            if( !Got_This_App_SubKey(app_name) ) return;

// Log("SaveSetting("+ name +","+ value +")");  // VERBOSE!

            RegistryKey app_SubKey = get_App_SubKey( app_name );
            if(app_SubKey == null) return;

            if(Siphoning_ProfileLines) {
                if(     (name.StartsWith("TAB.panel_usr") && !name.EndsWith(".rtf"))
                    ||   name.StartsWith("PALETTE.")
                //  ||   name.StartsWith("OPACITY")
                //  ||   name.StartsWith("DEV_DPI")
                //  ||   name.StartsWith("DEV_H")
                //  ||   name.StartsWith("DEV_W")
                //  ||   name.StartsWith("DEV_ZOOM")
                //  ||   name.StartsWith("MON_SCALE")
                //  ||   name.StartsWith("TXT_ZOOM")
                  )
                    ProfileLines += name+"="+ value +"\n";
            }
            //  profileLines += "["+ app_name +"] "+ name+"="+ value +"\n";

            try              { app_SubKey.SetValue(name, value); }
            catch(Exception) { /* Log("SaveSetting(): "+ex.Message); */ }

        }
        // }}}
        public  static   void Flush(string app_name)// {{{
        {
            if( !Got_This_App_SubKey(app_name) ) return;
            RegistryKey app_SubKey = get_App_SubKey( app_name );
            if(app_SubKey == null) return;
            app_SubKey.Flush();
        }
        // }}}
        public  static   void Flush()// {{{
        {
            Flush(_APP_NAME);
        }
        // }}}
        public  static   void DeleteSetting(string app_name, string name)// {{{
        {
            if( !Got_This_App_SubKey(app_name) ) return;

            //Log("DeleteSetting("+ name +")");

            RegistryKey  app_SubKey = get_App_SubKey( app_name );
            if(app_SubKey == null) return;

            try              { app_SubKey.DeleteValue(name); }
            catch(Exception) { /* Log("DeleteSetting(): "+ ex.Message); */ }
        }
        // }}}
        //}}}
        // THIS-APPLICATION REGISTRY SETTING IO {{{
        public  static bool      Settings_saved = false;

        public  static string[] GetValueNames()// {{{
        {
            return GetValueNames(_APP_NAME);
        }
        // }}}
        public  static   void SaveSetting(string name, string Value)// {{{
        {
            SaveSetting(_APP_NAME, name, Value);
        }
        // }}}
        public  static   void DeleteSetting(string name)// {{{
        {
            DeleteSetting(_APP_NAME, name);
        }
        // }}}
        //}}}

        // BUILTINS COMMANDS & POLL KEY=VALUE
        //{{{
        // can_parse_KEY_VAL {{{

        public static bool can_parse_KEY_VAL(string argLine)
        {
            bool diag
                = !IsASettingsLine( argLine )
                && (   (argLine.IndexOf(KEY_VAL_DEV_DPI   +"="  ) >= 0)
                    || (argLine.IndexOf(KEY_VAL_DEV_H     +"="  ) >= 0)
                    || (argLine.IndexOf(KEY_VAL_DEV_W     +"="  ) >= 0)

                    || (argLine.IndexOf(KEY_VAL_DEV_ZOOM  +"="  ) >= 0)
                    || (argLine.IndexOf(KEY_VAL_MON_DPI   +"="  ) >= 0)
                    || (argLine.IndexOf(KEY_VAL_MON_SCALE +"="  ) >= 0)

                    || (argLine.IndexOf(KEY_VAL_TXT_ZOOM  +"="  ) >= 0)
                    || (argLine.IndexOf(KEY_VAL_PALETTE   +"="  ) >= 0)
                    || (argLine.IndexOf(KEY_VAL_MAXCOLORS +"="  ) >= 0)
                    || (argLine.IndexOf(KEY_VAL_OPACITY   +"="  ) >= 0)
                    || (argLine.IndexOf(KEY_VAL_PROFILE   +"="  ) >= 0)
                  )
                ;

            Log("can_parse_KEY_VAL("+ argLine +") ...return "+ diag);
            return diag;
        }
        //}}}
        // set_KEY_VAL {{{
        public static string set_KEY_VAL(string caller, string cmd, string argLine)
        {
/* // {{{
:vnew /LOCAL/DATA/ANDROID/PROJECTS/RTabs/app/src/main/java/ivanwfr/rtabs/Settings.java
:vnew /LOCAL/STORE/DEV/PROJECTS/RTabs/Util/src/Settings.cs
*/ // }}}
            // RESET DISPATCHING-APPLICATION SIGNATURE
            // RESET DISPATCHING-APPLICATION SIGNATURE
            // before sharing these changes,
            // a dispatching source will add ITS SOURCE=ID KEY_VAL-SIGNATURE
            // to recognize one of its bouncing-back messages
            Settings.SOURCE = "";

            string[] args  = argLine.Split(' ');
            Log("set_KEY_VAL("+cmd+", ("+ args.Length +" split) argLine=["+ argLine.Replace(" ","\n") +"]:");

        //  string oob_cmd  = "";
            string source   = "";
            string profile  = "";
            int    prodate  = 0;

            double dev_zoom = 0;
            int    dev_dpi  = 0;
            int    dev_h    = 0;
            int    dev_w    = 0;

            double mon_scale= 0;
            double txt_zoom = 0;
            int    mon_dpi  = 0;

            int    opacity  = 0;
            int    maxcolors= 0;
            string palette  = "";

            string tag_cmd  = "";
            string tag_lyo  = "";
            string tag_sfx  = "";

            for(int i=0; i < args.Length; ++i)
            {
                string[] kv = args[i].Split('=');
                if(kv.Length < 2) continue;
                if(kv[1].Equals("\"\"")) kv[1]="";

                try{ if(kv[0].ToUpper() == "SOURCE"   ) { source    =               kv[1]  ; } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
            //  try{ if(kv[0].ToUpper() == "OOB_CMD"  ) { oob_cmd   =               kv[1]  ; } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
                try{ if(kv[0].ToUpper() == "PROFILE"  ) { profile   =               kv[1]  ; } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
                try{ if(kv[0].ToUpper() == "PRODATE"  ) { prodate   =    int.Parse( kv[1] ); } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }

                try{ if(kv[0].ToUpper() == "DEV_DPI"  ) { dev_dpi   =    int.Parse( kv[1] ); } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
                try{ if(kv[0].ToUpper() == "DEV_H"    ) { dev_h     =    int.Parse( kv[1] ); } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
                try{ if(kv[0].ToUpper() == "DEV_W"    ) { dev_w     =    int.Parse( kv[1] ); } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
                try{ if(kv[0].ToUpper() == "DEV_ZOOM" ) { dev_zoom  = double.Parse( kv[1] ); } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }

                try{ if(kv[0].ToUpper() == "MON_DPI"  ) { mon_dpi   =    int.Parse( kv[1] ); } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
                try{ if(kv[0].ToUpper() == "MON_SCALE") { mon_scale = double.Parse( kv[1] ); } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
                try{ if(kv[0].ToUpper() == "TXT_ZOOM" ) { txt_zoom  = double.Parse( kv[1] ); } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }

                try{ if(kv[0].ToUpper() == "MAXCOLORS") { maxcolors =    int.Parse( kv[1] ); } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
                try{ if(kv[0].ToUpper() == "OPACITY"  ) { opacity   =    int.Parse( kv[1] ); } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
                try{ if(kv[0].ToUpper() == "PALETTE"  ) { palette   =               kv[1]  ; } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }

                try{ if(kv[0].ToUpper() == "TAG_CMD"  ) { tag_cmd   =               kv[1]  ; } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
                try{ if(kv[0].ToUpper() == "TAG_LYO"  ) { tag_lyo   =               kv[1]  ; } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }
                try{ if(kv[0].ToUpper() == "TAG_SFX"  ) { tag_sfx   =               kv[1]  ; } } catch(Exception ex) { Log("*** ["+ args[i] +"]: "+ ex.Message+"\n"); }

            }

            // post-format
            profile = profile.Replace(" ", "_");

            // FORWARD CHANGES TO ALL POLLING CLIENTS
            string buf    = "";

        //  if(profile   != "") oob_cmd = Settings.CMD_PROFILE;

        //  if(oob_cmd   != "") { Settings.OOB_CMD   = oob_cmd  ; buf += " OOB_CMD="  +Settings.OOB_CMD  ; }
            if(source    != "") { Settings.SOURCE    = source   ; buf += " SOURCE="   +Settings.SOURCE   ; }
            if(profile   != "") { Settings.PROFILE   = profile  ; buf += " PROFILE="  +Settings.PROFILE  ; }
            if(prodate   !=  0) { Settings.PRODATE   = prodate  ; buf += " PRODATE="  +Settings.PRODATE  ; }

            if(dev_dpi   !=  0) { Settings.DEV_DPI   = dev_dpi  ; buf += " DEV_DPI="  +Settings.DEV_DPI  ; }
            if(dev_h     !=  0) { Settings.DEV_H     = dev_h    ; buf += " DEV_H="    +Settings.DEV_H    ; }
            if(dev_w     !=  0) { Settings.DEV_W     = dev_w    ; buf += " DEV_W="    +Settings.DEV_W    ; }
            if(dev_zoom  !=  0) { Settings.DEV_ZOOM  = dev_zoom ; buf += " DEV_ZOOM=" +Settings.DEV_ZOOM ; }

            if(mon_dpi   !=  0) { Settings.MON_DPI   = mon_dpi  ; buf += " MON_DPI="  +Settings.MON_DPI  ; }
            if(mon_scale !=  0) { Settings.MON_SCALE = mon_scale; buf += " MON_SCALE="+Settings.MON_SCALE; }
            if(txt_zoom  !=  0) { Settings.TXT_ZOOM  = txt_zoom ; buf += " TXT_ZOOM=" +Settings.TXT_ZOOM ; }

            if(opacity   !=  0) { Settings.MAXCOLORS = maxcolors; buf += " MAXCOLORS="+Settings.MAXCOLORS; }
            if(opacity   !=  0) { Settings.OPACITY   = opacity  ; buf += " OPACITY="  +Settings.OPACITY  ; }
            if(palette   != "") { Settings.PALETTE   = palette  ; buf += " PALETTE="  +Settings.PALETTE  ; }

            if(tag_cmd   != "") { Settings.TAG_CMD   = tag_cmd  ; buf += " TAG_CMD="  +Settings.TAG_CMD  ; }
            if(tag_lyo   != "") { Settings.TAG_LYO   = tag_lyo  ; buf += " TAG_LYO="  +Settings.TAG_LYO  ; }
            if(tag_sfx   != "") { Settings.TAG_SFX   = tag_sfx  ; buf += " TAG_SFX="  +Settings.TAG_SFX  ; }

            Log("set_KEY_VAL("+ cmd +"):\n"+buf.Replace(" ", "\n"));

            return buf.Trim();
        }
        //}}}
        // IsABuiltinCmdLine {{{
        public static bool IsABuiltinCmdLine(string cmdLine)
        {
            bool diag = false;
            CultureInfo ci = new CultureInfo("en-US");

            // COMM
            if     (cmdLine.Equals    (CMD_OK                          )) diag = true;
            else if(cmdLine.StartsWith(CMD_PASSWORD+" "      , true, ci)) diag = true;
            else if(cmdLine.StartsWith(CMD_SIGNIN+" "        , true, ci)) diag = true;


            // PALETTES
            else if(cmdLine.Equals    (CMD_PALETTES_CLEAR              )) diag = true;
            else if(cmdLine.Equals    (CMD_PALETTES_GET                )) diag = true;
            else if(cmdLine.Equals    (CMD_PALETTES_LOAD               )) diag = true;
            else if(cmdLine.StartsWith(CMD_PALETTES_SETTINGS , true, ci)) diag = true;

            // TABS
            else if(cmdLine.Equals    (CMD_TABS_CLEAR                  )) diag = true;
            else if(cmdLine.Equals    (CMD_TABS_GET                    )) diag = true;
            else if(cmdLine.Equals    (CMD_TABS_LOAD                   )) diag = true;
            else if(cmdLine.StartsWith(CMD_TABS_SETTINGS     , true, ci)) diag = true;

            // PROFILE
            else if(cmdLine.Equals    (CMD_RELOAD                      )) diag = true;
            else if(cmdLine.StartsWith(CMD_PROFILE+" "       , true, ci)) diag = true;

            // DESKTOP
            else if(cmdLine.StartsWith(CMD_SENDINPUT         , true, ci)) diag = true;
            else if(cmdLine.StartsWith(CMD_SENDKEYS          , true, ci)) diag = true;
            else if(cmdLine.StartsWith(CMD_BROWSE +" "       , true, ci)) diag = true;
            else if(cmdLine.StartsWith(CMD_BROWSE +"_"       , true, ci)) diag = true;
            else if(cmdLine.StartsWith(CMD_RUN    +" "       , true, ci)) diag = true;
            else if(cmdLine.StartsWith(CMD_RUN    +"_"       , true, ci)) diag = true;
            else if(cmdLine.StartsWith(CMD_SHELL  +" "       , true, ci)) diag = true;
            else if(cmdLine.StartsWith(CMD_SHELL  +"_"       , true, ci)) diag = true;

            // STATES
            else if(cmdLine.Equals    (CMD_BEEP                        )) diag = true;
            else if(cmdLine.Equals    (CMD_CLEAR                       )) diag = true;
            else if(cmdLine.Equals    (CMD_CLOSE                       )) diag = true;
            else if(cmdLine.Equals    (CMD_HIDE                        )) diag = true;
            else if(cmdLine.Equals    (CMD_STOP                        )) diag = true;
            else if(cmdLine.StartsWith(CMD_LOGGING           , true, ci)) diag = true;

            Log("IsABuiltinCmdLine("+ cmdLine +") ...diag=["+ diag +"]");
            return diag;
        }
        //}}}
        // IsADashCmdLine {{{
        public static bool IsADashCmdLine(string cmdLine)
        {
            bool diag = false;
            CultureInfo ci = new CultureInfo("en-US");
            if     (cmdLine.StartsWith(TESTS_DASH            , true, ci)) diag = true;

            return diag;
        }
        //}}}

        public static string Get_APP_KEY_VAL() //{{{
        {
            StringBuilder sb = new StringBuilder();

            // PROFILE
            sb.Append(    " PROFILE="+ Settings.PROFILE   );
            sb.Append(    " PRODATE="+ Settings.PRODATE   );
            sb.Append(     " SOURCE="+ Settings.APP_NAME  );
            sb.Append(    " PALETTE="+ Settings.PALETTE   );
            sb.Append(    " OPACITY="+ Settings.OPACITY   );
            sb.Append(  " MAXCOLORS="+ Settings.MAXCOLORS );

/* ...yet not used by android apk
            sb.Append(  " TAG_CMD="+ Settings.TAG_CMD   );
            sb.Append(  " TAG_LYO="+ Settings.TAG_LYO   );
            sb.Append(  " TAG_SFX="+ Settings.TAG_SFX   );
*/

            // GEOMETRY
            sb.Append(  " DEV_DPI="+ Settings.DEV_DPI   );
            sb.Append( " DEV_ZOOM="+ Settings.DEV_ZOOM  );
            sb.Append( " TXT_ZOOM="+ Settings.TXT_ZOOM  );
            sb.Append(    " DEV_W="+ Settings.DEV_W     );
            sb.Append(    " DEV_H="+ Settings.DEV_H     );
        //  sb.Append(" DEV_SCALE="+ Settings.DEV_SCALE );  // DEV-SIDE
        //  sb.Append(    " DEV_X="+ Settings.DEV_X     );  // DEV-SIDE
        //  sb.Append(    " DEV_Y="+ Settings.DEV_Y     );  // DEV-SIDE
            sb.Append(  " MON_DPI="+ Settings.MON_DPI   );  // MON-SIDE
            sb.Append(" MON_SCALE="+ Settings.MON_SCALE );  // MON-SIDE
            sb.Append(  " LOGGING="+ Settings.LOGGING   );

            return sb.ToString();
        }
        //}}}
        //}}}

        // POWERSHELL
        public  static string ExecuteCommandAsAdmin(string command)// {{{
        {
            Log("ExecuteCommandAsAdmin("+command+")");

            ProcessStartInfo psinfo         = new ProcessStartInfo();
            psinfo.FileName                 = "powershell.exe";
            psinfo.Arguments                = command;
            psinfo.CreateNoWindow           = true;
            psinfo.RedirectStandardError    = true;
            psinfo.RedirectStandardOutput   = true;
            psinfo.UseShellExecute          = false;
        //  psinfo.WindowStyle              = ProcessWindowStyle.Normal;    // Hidden Maximized Minimized Normal

            using (Process proc = new Process())
            {
                proc.StartInfo  = psinfo;
                proc.Start();
                string stdout   = proc.StandardOutput.ReadToEnd();
                string stderr   = proc.StandardError .ReadToEnd();
                return (string.IsNullOrEmpty( stderr ))
                    ?                         stdout
                    :                         stdout +"\n *** "+ stderr +" ***"
                    ;
            }
        }
        // }}}

        // PROCESS
        public  static Process ExecuteProcess(string cmdLine)// {{{
        {
            Log("ExecuteProcess("+cmdLine+")");
            // [cmd_file] [cmd_args] {{{
            Settings.CmdParser.parse(cmdLine);
            string cmd_file = Settings.CmdParser.cmd;
            string cmd_args = Settings.CmdParser.argLine;

            //}}}

            // 1/2 BROWSER COMMAND .. [cmd_file] [cmd_args] (substitute the default browser for an url arg) {{{
            Regex regex = new Regex(URL_FILTER);
            Match match = regex.Match( cmdLine );
            if( match.Success )
            {
                Log("BROWSER: ...MATCHED REGEX: URL_FILTER=["+ URL_FILTER +"]");
                Log("BROWSER: ...USING DEFAULT BROWSER");

                cmd_file = GetSystemDefaultBrowser();
                cmd_args = "\""+ cmdLine +"\"";
            //  if( cmd_args.StartsWith("about:") ) cmd_args = cmd_args.Replace("about:","-a chrome:");
            }
            //}}}
            // 2/2 EXECUTABLE COMMAND {{{
            else {
                Log("EXECUTABLE:");
                // NORMALIZE [cmd_file] AGAINST [Path.DirectorySeparatorChar] {{{
                cmd_file = cmd_file.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

                //}}}
                // SERVER [current directory] {{{
                Log( String.Format("...{0,32} =[{1}]", "Directory.GetCurrentDirectory"  , Directory.GetCurrentDirectory()    )); 

                //}}}
                // SERVER [exePath] {{{
                string exePath = System.Reflection.Assembly.GetCallingAssembly().Location;
                Log( String.Format("...{0,32} =[{1}]", "exePath", exePath));

                // (see Settings.LaunchServer)
                //}}}
                // SERVER [exeDir] {{{
                string  exeDir= Path.GetDirectoryName( exePath );
                Log( String.Format("...{0,32} =[{1}]", "exeDir", exeDir));

                //}}}
                // [cmd_file] .. (RELATIVE TO SERVER PATH) {{{
                if(cmd_file.IndexOf(".."+Path.DirectorySeparatorChar) == 0)
                {
                    // [cmd_file] .. f(tail) {{{
                    cmd_file = cmd_file.Substring(3);

                    Log( String.Format("...{0,32} =[{1}]", "cmd_file", cmd_file));
                    //}}}
                    // [cmd_file] f(exeParentDir) {{{
                    int idx = exeDir.LastIndexOf(Path.DirectorySeparatorChar);
                    if(idx > 0) {
                        string exeParentDir = exeDir.Substring(0, idx);

                        Log( String.Format("...{0,32} =[{1}]", "exeParentDir", exeParentDir));
                        cmd_file = exeParentDir + Path.DirectorySeparatorChar + cmd_file;
                    }
                    //}}}
                }
                //}}}
            }
            //}}}

            Log( String.Format("...{0,32} =[{1}]", "cmd_file", cmd_file));
            Log( String.Format("...{0,32} =[{1}]", "cmd_args", cmd_args));

            ProcessStartInfo psinfo         = new ProcessStartInfo();
            psinfo.FileName                 = cmd_file;
            psinfo.Arguments                = cmd_args;
            psinfo.CreateNoWindow           = true;
        //  psinfo.RedirectStandardError    = true;
        //  psinfo.RedirectStandardOutput   = true;
            psinfo.UseShellExecute          = false;
        //  psinfo.WindowStyle              = ProcessWindowStyle.Normal;    // Hidden Maximized Minimized Normal

        //  using (Process proc = new Process()) {
            try {
                Process proc    = new Process();
                proc.StartInfo  = psinfo;
                proc.Start();
                proc.WaitForInputIdle(2000);
                SetForegroundWindow((IntPtr)(proc.MainWindowHandle) );
                BringWindowToTop   ((IntPtr)(proc.MainWindowHandle) );
                return proc;
            }
            catch(Exception ex) { Log( "\n***\n*** "+ ex.Message +"\n***"); }
        //  }
            return null;
        }
        // }}}

        // SHELL
        // ExecuteShell {{{
    //  private static string URL_FILTER = "(http[^+%!^{]+)|(about:.+)|(chrome:.+)";
        private static string URL_FILTER = "(https?|file|ftp)://([^+%!^{]+)|(about:.+)|(chrome:.+)";

        public  static Process ExecuteShell(string cmdLine)
        {
            Log("ExecuteShell("+cmdLine+")");

            Settings.CmdParser.parse(cmdLine);

            string cmd_file                 = Settings.CmdParser.cmd;
            string cmd_args                 = Settings.CmdParser.argLine;

            // LOCALLY EXPAND BUILTIN DIRS
            if(    cmdLine.StartsWith("http:" )
                || cmdLine.StartsWith("https:")
                || cmdLine.StartsWith("file:" )
              ) {
                cmd_file = cmd_file.Replace(PROFILES_DIR, PROFILES_DIR_PATH);
            }

            Log("..cmd_file=["+ cmd_file +"]");
            Log("..cmd_args=["+ cmd_args +"]");

            ProcessStartInfo psinfo         = new ProcessStartInfo();
            psinfo.FileName                 = cmd_file;
            psinfo.Arguments                = cmd_args;
            psinfo.CreateNoWindow           = true;
        //  psinfo.RedirectStandardError    = true;
        //  psinfo.RedirectStandardOutput   = true;
            psinfo.UseShellExecute          = true;
        //  psinfo.WindowStyle              = ProcessWindowStyle.Normal;    // Hidden Maximized Minimized Normal

            //using (Process proc = new Process()) {
                Process proc    = new Process();
                proc.StartInfo  = psinfo;
                proc.Start();
                try {
                    proc.WaitForInputIdle(2000);
                    SetForegroundWindow((IntPtr)(proc.MainWindowHandle) );
                //  BringWindowToTop   ((IntPtr)(proc.MainWindowHandle) );
                } catch(Exception) { }
                return proc;
            //}
        }
        //}}}

        // BROWSER
            // {{{
            private  const  string  DEFAULT_BROWSER_COMMAND = "IExplore.exe";
            private  static string      SystemDefaultBrowser = string.Empty;
            private  static bool       Using_default_browser = true;

            // }}}
            // GetSystemDefaultBrowser {{{
            private  static string GetSystemDefaultBrowser()
            {
                if(SystemDefaultBrowser != string.Empty) return SystemDefaultBrowser; // KEEP FIRST FOUND

                string progName = Get_UrlAssociations_http_command();
            //  string progName = Get_Software_http_shell_open_command();

                if( Using_default_browser )
                {
                    Log("...SystemDefaultBrowser STILL UNDEFINED: ...returning DEFAULT_BROWSER_COMMAND=["+ progName +"]");
                    return progName;
                }
                else {
                    SystemDefaultBrowser = progName;
                    Log("...SystemDefaultBrowser HAS BEEN SET TO: ["+ SystemDefaultBrowser +"]");
                    return SystemDefaultBrowser;
                }

            }
            //}}}
            // Get_UrlAssociations_http_command {{{
            private  static string Get_UrlAssociations_http_command()
            {
                // UrlAssociations http [Progid] {{{
                string command = string.Empty;

                using (RegistryKey userChoiceKey
                    = Registry.CurrentUser.OpenSubKey(
                        @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"
                        )
                    )
                {
                    if(userChoiceKey != null)
                    {
                        try {
                            object progIdValue = userChoiceKey.GetValue("ProgID");
                            if(progIdValue != null)
                            {
                                // path from ProgID {{{
                                //  string dir = GetDirectoryFromProgID(progIdValue.ToString()                  );
                                //  Log("........GetDirectoryFromProgID(progIdValue=["+progIdValue.ToString()+"]): dir=["+dir+"]");
                                //}}}
                                // command {{{
                                string  pid_s = progIdValue.ToString().ToLower();
                                Log("...RegistryKey[HKCU_Sw_Ms_W_S_Ass_UrlAsso_Http_UserChoice]: pid_s=["+pid_s+"]");

                                //XXX*/ if     (pid_s.Contains("chrome" )) command = @"B:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                                //XXX*/ else if(pid_s.Contains("ie"     )) command = MY_IE_PATH;

                                if     (pid_s.Contains("chrome" )) command =   "ChromeHTML";
                                else if(pid_s.Contains("firefox")) command =  "firefox.exe";
                                else if(pid_s.Contains("opera"  )) command =    "opera.exe";
                                else if(pid_s.Contains("safari" )) command =   "safari.exe";
                                else if(pid_s.Contains("ie"     )) command = "IExplore.exe";

                                //}}}
                            }
                        }
                        catch (Exception ex) {
                            Log( string.Format("*** {0} in method {1}", ex.GetType(), ex.TargetSite) );
                        }
                    }
                }
                //}}}
                // GetEnvironmentVariable( PATH ) {{{
                //Log("GetEnvironmentVariable( PATH ):\n=== "+ Environment.GetEnvironmentVariable("PATH").Replace(";","\n=== ") +"\n===\n");

                //}}}
                // DEFAULT_BROWSER_COMMAND FALLBACK {{{
                if(command == string.Empty)
                {
                    Log("...command still undefined: ...using DEFAULT_BROWSER_COMMAND=["+ DEFAULT_BROWSER_COMMAND +"]");

                    command = DEFAULT_BROWSER_COMMAND;
                    Using_default_browser = true;
                }
                else {
                    Using_default_browser = false;
                }
                //}}}
                // BROWSER COMMAND PATH {{{
                command = GetPathFromProgName( command );

                //}}}
                Log("...directory=["+ Path.GetDirectoryName(command) +"]");
                Log("...return: command=["+command+"]");
                return command;
            }
            //}}}
            // GetPathFromProgName {{{
            public static string GetPathFromProgName(string progName)
            {
                Log("GetPathFromProgName("+progName+")");
                string command = string.Empty;
                try {
                    string regKey;
                    // ClassesRoot Applications progName shell open command

                  //            Applications\ChromeHTML\shell\open\command
                  //regKey  = @"Applications\"+ progName +@"\shell\open\command";

                  //               ChromeHTML\shell\open\command
                    regKey  =    progName +@"\shell\open\command";

                    command = GetDefaultRegistryValue(Registry.ClassesRoot, regKey);

                    Log("....regKey=["+regKey +"]");
                    Log("...command=["+command+"]");

                    if( string.IsNullOrEmpty( command ) )
                        return string.Empty;

                    Regex regex = new Regex(@"^""*([^""]*)""*", RegexOptions.IgnoreCase);
                    Match match = regex.Match( command );
                    if( match.Success ) {
                        Group g = match.Groups[1];
                        command =   g.Captures[0].ToString();
                    }
                    Log("...command=["+command+"]");
                }
                catch (Exception ex) {
                    Log( string.Format("*** {0} in method {1}", ex.GetType(), ex.TargetSite) );
                }
                return command;
            }
            //}}}
            // Get_Software_http_shell_open_command {{{
            public static string Get_Software_http_shell_open_command()
            {
                Log("Get_Software_http_shell_open_command()");
                string command = string.Empty;
                try {
                    string regKey;
                    // CurrentUser Software http
                    regKey  = @"Software\Classes\http\shell\open\command";
                    command = GetDefaultRegistryValue(Registry.CurrentUser, regKey);

                    Log("....regKey=["+regKey +"]");
                    Log("...command=["+command+"]");

                    if( string.IsNullOrEmpty( command ) )
                        return string.Empty;

                    Regex regex = new Regex(@"^""*([^""]*)""*", RegexOptions.IgnoreCase);
                    Match match = regex.Match( command );
                    if( match.Success ) {
                        Group g = match.Groups[1];
                        command =   g.Captures[0].ToString();
                    }
                    Log("...command=["+command+"]");
                }
                catch (Exception ex) {
                    Log( string.Format("*** {0} in method {1}", ex.GetType(), ex.TargetSite) );
                }
                // DEFAULT_BROWSER_COMMAND FALLBACK {{{
                if(command == string.Empty)
                {
                    Log("...command still undefined: ...using DEFAULT_BROWSER_COMMAND=["+ DEFAULT_BROWSER_COMMAND +"]");

                    command = DEFAULT_BROWSER_COMMAND;
                    Using_default_browser = true;
                }
                else {
                    Using_default_browser = false;
                }
                //}}}
                return command;
            }
            //}}}
            // GetDefaultRegistryValue {{{
            private static string GetDefaultRegistryValue(RegistryKey rootKey, string regPath)
            {
                try {
                    //var regPermission = new RegistryPermission(RegistryPermissionAccess.Read, @"HKEY_CLASSES_ROOT\" + regPath);
                    //regPermission.Demand();
                    using (var regKey = rootKey.OpenSubKey(regPath))
                    {
                        if (regKey != null)
                            return (string) regKey.GetValue("");
                    }
                }catch(Exception ex) {
                    Log("GetDefaultRegistryValue(): "+ ex.Message);
                }
                return "";
            }
            //}}}
    //        // GetDirectoryFromProgID {{{
    //        public static string GetDirectoryFromProgID(string progID)
    //        {
    //            var  classID = GetDefaultRegistryValue(Registry.ClassesRoot, progID +@"\CLSID\"                               );
    //            var fileName = GetDefaultRegistryValue(Registry.ClassesRoot,         @"\CLSID\"+ classID + @"\InProcServer32\");
    //            if(            !string.IsNullOrEmpty( fileName )) return Path.GetDirectoryName( fileName );
    //            else                                              return string.Empty;
    //        }
    //        //}}}

            // WINDOW
            // SetWindowGeometry {{{
            public  static  void SetWindowGeometry(string corner, Process proc)
            {
                Log("SetWindowGeometry(corner=["+corner+"], proc=["+proc+"])");

                // DEFAULT TO CENTER
                int w = Screen.PrimaryScreen.Bounds.Width  / 2;
                int h = Screen.PrimaryScreen.Bounds.Height / 2;
                int x = w/2;
                int y = h/2;

                if     (corner == "UL") { x = 0; y = 0; }
                else if(corner == "UR") { x = w; y = 0; }
                else if(corner == "DL") { x = 0; y = h; }
                else if(corner == "DR") { x = w; y = h; }

                else if(corner ==  "U") { x = 0; y = 0;  w *= 2; }
                else if(corner ==  "D") { x = 0; y = h;  w *= 2; }

                else if(corner ==  "R") { x = w; y = 0;  h *= 2; }
                else if(corner ==  "L") { x = 0; y = 0;  h *= 2; }

                else if(corner ==  "F") { x = 0; y = 0;  w *= 2; h *= 2; }

                proc_p = proc;
                proc_x = x;
                proc_y = y;
                proc_w = w;
                proc_h = h;
                new Thread(new ThreadStart( delayed_SetWindowPos )).Start();
            }
            //}}}
        // delayed_SetWindowPos {{{
        private static void delayed_SetWindowPos()
        {
            Log("delayed_SetWindowPos: Sleep("+ SETWINDOWGEOMETRY_DELAY +"):");
            Thread.Sleep( SETWINDOWGEOMETRY_DELAY );

            try {
                proc_p.WaitForInputIdle(2000);
                IntPtr  hWnd            = proc_p.MainWindowHandle;
                Log("...Process MainWindowHandle=["+ hWnd +"]:");

                // FOREGROUND
                Log("...SetForegroundWindow:");
                SetForegroundWindow((IntPtr)( hWnd) );

                // TOP
                Log("...BringWindowToTop:");
                BringWindowToTop   ((IntPtr)( hWnd) );

                // POS
                int     wFlags          = (SWP_ASYNCWINDOWPOS | SWP_SHOWWINDOW);
                Log("...SetWindowPos(x=["+proc_x+"@"+proc_y+"] wh=["+proc_w+"x"+proc_h+"]):");
                SetWindowPos(hWnd, HWND_TOPMOST, proc_x, proc_y, proc_w, proc_h, wFlags);
            }
            catch(Exception ex) {
                Log("delayed_SetWindowPos(): "+ ex.Message);
            }
        }
        //}}}
        // {{{
        private const   int     HWND_TOPMOST        = -1;
        private const   int     SWP_ASYNCWINDOWPOS  = 0x4000;
        private const   int     SWP_SHOWWINDOW      = 0x0040;

        private static  Process proc_p              = null;
        private static  int     proc_h              = 0;
        private static  int     proc_w              = 0;
        private static  int     proc_x              = 0;
        private static  int     proc_y              = 0;
        //}}}
        // DllImport {{{
        [DllImport("user32.dll")]
            private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
            private static extern bool BringWindowToTop(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
            public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        // }}}

        // UTIL
        // ellipsis {{{
        public static string ellipsis(string msg, int length)
        {
            msg = msg.Replace("\n","\\n");
            return (msg.Length <= length)
                ?   msg
                :   msg.Substring(0, length-3)+"...";
        }
        //}}}
        public  static string ExToString(Exception ex) //{{{
        {
            string msg = ex.Message.Replace(Environment.NewLine, "");
            return "*** "+ msg +" ***"+ Environment.NewLine
            +      ex.ToString().Replace(":",":"+ Environment.NewLine)
            ;
        }
        //}}}
        public  static bool LaunchServer() // {{{
        {
            byte[] b = new byte[2048];
            try {
                string exePath      =     System.Reflection.Assembly.GetCallingAssembly().Location;
                Log("LaunchServer: exePath=["+exePath+"]");

                string exeDir       = Path.GetDirectoryName(exePath);
                Log("LaunchServer: exeDir=["+exeDir+"]");

                // TRY SERVER SYMBOLIC LINKS .. AS A SHELL COMMAND
                //{{{
                // SERVER_APP_NAME.lnk // {{{
                string server_lnk   = exeDir + Path.DirectorySeparatorChar + SERVER_APP_NAME +".lnk";
                Log(    "LaunchServer trying: server_lnk=["+server_lnk+"]");

                // }}}
                // SERVER_APP_NAME.exe - Shortcut.lnk (i.e. result of Create a link) // {{{
                if( !File.Exists(server_lnk) ) {
                    server_lnk   = exeDir + Path.DirectorySeparatorChar + SERVER_APP_NAME +".exe - Shortcut.lnk";
                    Log("LaunchServer trying: server_lnk=["+server_lnk+"]");
                }

                // }}}
                // SERVER_APP_NAME.exe.lnk // {{{
                if( !File.Exists(server_lnk) ) {
                    server_lnk   = exeDir + Path.DirectorySeparatorChar + SERVER_APP_NAME +".exe.lnk";
                    Log("LaunchServer trying: server_lnk=["+server_lnk+"]");
                }
                if( !File.Exists(server_lnk) )
                    server_lnk = "";

                // }}}
                if(server_lnk != "") {
                    Log("...found server_lnk=["+server_lnk+"]");
                    Log("...ExecuteCommandAsAdmin(server_lnk):");
                    string result = ExecuteCommandAsAdmin( server_lnk.Replace(" - ","?-?") ); // as in " - Shortcut.lnk"
                    Log("***\n"+result+"\n");

                    if(result.IndexOf("***") < 0)
                        return true;
                }
                //}}}

                // SERVER EXECUTABLE .. AS A RUNNABLE PROCESS
                //{{{
                string server_exe   = exeDir
                    + Path.DirectorySeparatorChar + SERVER_APP_NAME +".exe"
                    ;
                Log("LaunchServer trying: server_exe=["+server_exe+"]");
                if( File.Exists(server_exe) )
                {
                    Log("...ExecuteProcess(server_exe):");
                    Process proc = ExecuteProcess( server_exe );
                    if(proc != null)
                        return true;
                    Log("*** server_exe process could not be executed ");
                }
                //}}}
                else {
                    //{{{
                    /*..*/ server_exe = exeDir
                        + Path.DirectorySeparatorChar + ".."
                        + Path.DirectorySeparatorChar + SERVER_APP_NAME
                        + Path.DirectorySeparatorChar + SERVER_APP_NAME +".exe"
                        ;
                    Log("LaunchServer trying: server_exe=["+server_exe+"]");
                    if( File.Exists(server_exe) )
                    {
                        Log("...ExecuteProcess(server_exe):");
                        Process proc = ExecuteProcess( server_exe );
                        if(proc != null)
                            return true;
                        Log("*** server_exe process could not be executed ");
                    }
                    //}}}
                }
            }
            catch(Exception ex) { Log("LaunchServer(): "+ ex.Message); }

            Log("LaunchServer: *** COULD NOT FIND ["+ SERVER_APP_NAME +"]\n");
            return false;
        }
        //}}}
        public  static string RetrieveLinkerTimestamp() // {{{
        {
            byte[] b = new byte[2048];
            System.IO.Stream s = null;
            try {
                string filePath =     System.Reflection.Assembly.GetCallingAssembly().Location;
                //Log("RetrieveLinkerTimestamp: filePath=["+filePath+"]");

                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally { if(s!=null) s.Close(); }

            const int C_PEHEADEROFFSET          = 60;
            int i                               = System.BitConverter.ToInt32(b,     C_PEHEADEROFFSET);

            const int C_LINKERTIMESTAMPOFFSET   = 8;
            int secondsSince1970                = System.BitConverter.ToInt32(b, i + C_LINKERTIMESTAMPOFFSET);

            System.DateTime dt                  = new System.DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds( secondsSince1970 );
            dt = dt.AddHours  ( System.TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours );

            // COMPILE TIME & CURRENT TIME
            string time_stamp = "V"+dt.ToString("yyMMdd") +" ("+ Get_time_elapsed( dt ) +" old)";

            return time_stamp;
        }
        //}}}
        public  static int    GetUnixTimeSeconds() // {{{
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = DateTime.Now.ToUniversalTime() - epoch;
            return (int)(span.TotalSeconds);
        }
        // }}}
        public  static long   GetUnixTimeMilliSeconds() // {{{
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = DateTime.Now.ToUniversalTime() - epoch;
            return (long)span.TotalMilliseconds;
        }
        //}}}
        public  static string Get_time_elapsed(int secondsSince1970) // {{{
        {
            // UTC
            System.DateTime dt= new System.DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds( secondsSince1970 );

            // LOCALTIME
            dt = dt.AddHours  ( System.TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours );
            return Get_time_elapsed( dt );
        }
        //}}}
        public  static string Get_time_elapsed(System.DateTime date_since) // {{{
        {
            TimeSpan span = DateTime.Now.Subtract( date_since );
            return Get_span_elapsed( span );
        }
        //}}}
        public  static string Get_time_diff(int secs1, int secs2) // {{{
        {
            int from = (secs2 > secs1) ? secs1 : secs2;
            int to   = (secs2 > secs1) ? secs2 : secs1;

            System.DateTime dt_from= new System.DateTime(1970, 1, 1, 0, 0, 0);
            System.DateTime dt_to  = new System.DateTime(1970, 1, 1, 0, 0, 0);

            dt_from       = dt_from.AddSeconds( from );
            dt_to         = dt_to  .AddSeconds( to   );

            TimeSpan span = dt_to.Subtract( dt_from );

            return Get_span_elapsed( span );
        }
        //}}}
        private static string Get_span_elapsed(TimeSpan span) // {{{
        {
            StringBuilder sb = new StringBuilder();

            if     (span.Days    > 730) sb.Append(span.Days / 365 +" years " );
            else if(span.Days    > 365) sb.Append(span.Days / 365 +" year "  );

            else if(span.Days    >  60) sb.Append(span.Days /  30 +" months ");
            else if(span.Days    >  30) sb.Append(span.Days /  30 +" month  ");

            else if(span.Days    >   1) sb.Append(span.Days       +" days "  );
            else if(span.Days    >   0) sb.Append(span.Days       +" day "   );

            else if(span.Hours   >   1) sb.Append(span.Hours      +" hours " );
            else if(span.Hours   >   0) sb.Append(span.Hours      +" hour "  );

            else if(span.Minutes >   0) sb.Append(span.Minutes    +" min "   );

            else                        sb.Append(span.Seconds    +" sec "   );

            return sb.ToString().Trim();
        }
        //}}}

        // SM ServiceMutex
        public static void   SM_aquiredBy(string owner) //{{{
        {
            if(ServiceMutexOwner == "") ServiceMutexOwner = owner;

            ServiceMutexCount += 1;
            //SM_toDash();
        }
        //}}}
        public static void   SM_releasedBy(string owner) //{{{
        {
            if(owner != ServiceMutexOwner)
                Log(MUTEX_DASH, "ServiceMutexAquiredBy=["+ ServiceMutexOwner +" Released by "+ owner +"]");

            ServiceMutexCount -= 1;
            //SM_toDash();
        }
        //}}}
        public static void Log_SMutex_Wait(string caller, string method_id) //{{{
        {
            string s = ">>> MUTEX >>> ("+ServiceMutexCount +") "
                + method_id +" "+ ((ServiceMutexCount != 0) ? "CURRENT " : "PREVIOUS") +" OWNER=["+ServiceMutexOwner +"] "
                + FOLD_OPEN;

            Log_left(caller, s);
        }
        //}}}
        public static void Log_SMutex_Release(string caller, string method_id) //{{{
        {
            string s = "<<< MUTEX <<< ("+ServiceMutexCount +") "
                + method_id +" "+ ((ServiceMutexCount != 0) ? "CURRENT " : "PREVIOUS") +" OWNER=["+ServiceMutexOwner +"] "
                + FOLD_CLOSE;

            Log_left(caller, s);
        }
        //}}}
        public static void Log_SMutex_WaitFailed(string caller, string msg) //{{{
        {
            string s = "*** "+ msg +" COULD NOT ACQUIRE Mutex FOR "+ (WAIT_SERVICE_MUTEX_TIMEOUT_MS / 1000) +"s";

            Log_left(caller, s);
        }
        //}}}
        public static void   SM_toDash() //{{{
        {
        //  MainFormInstance.Invoke( (MethodInvoker)delegate() { Logger.Log(MUTEX_DASH, "ServiceMutexOwner=[("+ServiceMutexCount+") "+ServiceMutexOwner+"]"); });
            Logger.Log(MUTEX_DASH, "ServiceMutexOwner ("+ServiceMutexCount+") ["+ServiceMutexOwner+"]");
        }
        //}}}

        // TAG CMD
        // CmdParser (parse once .. access components when needed) {{{
        public class CmdParser
        {
            // PARSE {{{
            public static void parse(string _cmdLine)
            {
                cmdLine    = _cmdLine;      // keep track of the last one

                cmd        =   cmdLine;
                argLine    =   "";
                args       = new string[1] { cmdLine };
                arg1       = "";

                int idx;
                if( !cmdLine.StartsWith("\"") )
                {
                    idx = cmdLine.IndexOf(' ');
                    if(idx >= 0) {
                        args    = cmdLine.Split(' ');
                        cmd     = args[0];
                        arg1    = args[1];
                        argLine = cmdLine.Substring(cmd.Length+1).Trim();   // agrs[1 .. args.length-1]
                    }
                }
                // quoted "cmd" ...
                else {
                    idx     = cmdLine.IndexOf  ('"', 1  );                  // closing cmd quote
                    cmd     = cmdLine.Substring(1, idx-1);                  // "cmd" (between quotes)
                    argLine = cmdLine.Substring(   idx+1).Trim();           // "quoted cmd" remainder
                    args    = ("no_space_place_holder "+argLine).Split(' ');
                    args[0] = cmd;                                          // substitute no_space_place_holder with real cmd
                    arg1    = args[1];
                }
/*
                Log("...........cmd=["+ cmd         +"]");
                Log("...args.Length=["+ args.Length +"]");
                Log("..........arg1=["+ arg1        +"]");
                Log(".......argLine=["+ argLine     +"]");
*/
                Log( ToString() );

            }
            //}}}
            // getArgValue {{{
            public static string getArgValue(string key, string default_value)
            {
                for(int i=0; i < args.Length; ++i) {
                    string[] kv = args[i].Split('=');
                    if(kv.Length > 1)
                        if(kv[0].ToUpper() == key) { return kv[1]; }
                }
                return default_value;
            }
            // }}}
            // GET CACHED FIELDS {{{
            public static string   cmdLine    = "";    // first      word
            public static string   cmd        = "";    // first      word
            public static string   argLine    = "";    // following  words
            public static string[] args       = {};    // individual words
            public static string   arg1       = "";    // past cmd   word
/*
            public static string   get_cmd    (string _cmdLine) { if(_cmdLine != cmdLine) parse(cmdLine); return cmd;     }
            public static string   get_arg1   (string _cmdLine) { if(_cmdLine != cmdLine) parse(cmdLine); return arg1;    }
            public static string   get_argLine(string _cmdLine) { if(_cmdLine != cmdLine) parse(cmdLine); return argLine; }
            public static string[] get_args   (string _cmdLine) { if(_cmdLine != cmdLine) parse(cmdLine); return args;    }
*/
            //}}}
            // ToString {{{
            public new static string ToString()
            {
                return "CmdParser:\n"
                +".......cmdLine=["+ cmdLine     +"]\n"
                +"...........cmd=["+ cmd         +"]\n"
                +".......argLine=["+ argLine     +"]\n"
                +"...args.Length=["+ args.Length +"]\n"
                +"..........arg1=["+ arg1        +"]\n"
                ;

            }
            //}}}
        }
        // }}}

        // LOG
        public  static void Log(string msg)// {{{
        {
            Log(typeof(Settings).Name, msg);
        }

        // }}}
        public  static void Log(string caller, string msg)// {{{
        {
        //  if(MainFormInstance == null) return;
        //  MainFormInstance.Invoke( (MethodInvoker)delegate() { Logger.Log_left(caller, msg); });
        //  Logger.Log_left(caller, msg);
            Logger.Log     (caller, msg);
        }

        // }}}
        public  static void Log_left(string caller, string msg)// {{{
        {
        //  if(MainFormInstance == null) return;
        //  MainFormInstance.Invoke( (MethodInvoker)delegate() { Logger.Log_left(caller, msg); });
            Logger.Log_left(caller, msg);
        }

        // }}}

        // DASH STATUS
        public static string get_status() //{{{
        {
            string SEP = " =====================================================================";
            // MONITOR {{{

            int   mon_w      =  (int)(Settings.DEV_W * Settings.DEV_DPI / Settings.MON_DPI * Settings.MON_SCALE);
            int   mon_h      =  (int)(Settings.DEV_H * Settings.DEV_DPI / Settings.MON_DPI * Settings.MON_SCALE);

            string mon_wxh   = String.Format("| {0,10} = {1,-10}\n"                  , "MON WxH"   ,              mon_w +"x"+          mon_h);
            string mon_dpi   = String.Format("| {0,10} = {1,-10}\n"                  , "MON_DPI"   , Settings.  MON_DPI +"dpi"              );
            string txt_zoom  = String.Format("| {0,10} = {1,-10} Shift+Ctrl (1..8)\n", "TXT_ZOOM " , Settings.TXT_ZOOM                      );
            string mon_scale = String.Format("| {0,10} = {1,-10}        Alt (1..8)\n", "MON_SCALE" , Settings.MON_SCALE                     );

            string mon_status
                = SEP +" MONITOR:\n"
                + mon_wxh
                + mon_dpi
                + mon_scale
                + txt_zoom
                ;

            //}}}
            // DEVICE {{{
            float  inc_w    = (float)Settings.DEV_W /             (float)Settings.DEV_DPI;
            float  inc_h    = (float)Settings.DEV_H /             (float)Settings.DEV_DPI;
            float  inc_D    = (float)Math.Sqrt(inc_w*inc_w + inc_h*inc_h);
            string inch     = String.Format("{0:0.0}\" ({1:0.0}x{2:0.0})", inc_D, inc_w, inc_h);

            string dev_dpi  = String.Format("| {0,10} = {1,-10}   Shift+Alt (1..8)\n", "DEV_DPI"  , Settings.  DEV_DPI                     );
            string dev_wxh  = String.Format("| {0,10} = {1,-10}        Ctrl (1..0)\n", "DEV WxH"  , Settings.    DEV_W +"x"+ Settings.DEV_H);
            string dev_inch = String.Format("| {0,10} = {1,-10}\n"                   , "DEV INCH" , inch                                   );
            string dev_zoom = String.Format("| {0,10} = {1,-10}\n"                   , "DEV_ZOOM" , Settings.  DEV_ZOOM                    );

            string dev_status
                = SEP +"= DEVICE:\n"
                + dev_dpi
                + dev_wxh
                + dev_inch
                + dev_zoom
                ;

            //}}}
            // MUTEX {{{
            string mutex_status = "";
            if( Settings.UseMutex ) {
                mutex_status
                    = SEP +"=== COMM:\n"
                    +"| Mutex ("+ServiceMutexCount+") ["+ServiceMutexOwner+"]\n"
                    ;

                }
            // }}}
            // KEY_VAL HISTORY {{{
            string key_val_h = String.Format("| {0,10} = {1}\n"                      , "KEY_VAL_HISTORY", Settings.KEY_VAL_HISTORY);
            Settings.KEY_VAL_HISTORY = "";

            //}}}
            // PROFILES {{{
            Get_Profiles_Dict();
            string profiles_status
                = SEP +"=== USER:\n"
                + String.Format("| {0,10} = {1}\n"                                   , "PROFILES", Profiles_Dict.Count)
                ;

            //}}}
            // DASH TEXT {{{
            string msg
                = mon_status
                + dev_status
                + profiles_status
                + mutex_status
                + key_val_h
                ;

            return msg;
            // }}}
        }
        //}}}
    }
}

/*// {{{
" SetWindowPos function:
:!start explorer "https://msdn.microsoft.com/en-us/library/windows/desktop/ms633545(v=vs.85).aspx"

*/ // }}}

