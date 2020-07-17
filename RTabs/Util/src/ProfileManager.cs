#region using {{{
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System;

#endregion }}}

namespace RTabs
{
    public class ProfileManager
    {
        // XML CONSTANTS {{{

        // PROFILE
        private const string XML_EXTENSION      = ".xml";
        private const string XMLDECLARATION     = "<?xml version='1.0'?>";
        private const string XMLTAG_RTABSPROFILE= "RTABSProfile";

        // OPTIONS
        private const string XMLTAG_DEV_DPI    = "DEV_DPI";
        private const string XMLTAG_DEV_H      = "DEV_H";
        private const string XMLTAG_DEV_W      = "DEV_W";
        private const string XMLTAG_DEV_ZOOM   = "DEV_ZOOM";
        private const string XMLTAG_MON_DPI    = "MON_DPI";
        private const string XMLTAG_MON_SCALE  = "MON_SCALE";
        private const string XMLTAG_OPACITY    = "OPACITY";
        private const string XMLTAG_TXT_ZOOM   = "TXT_ZOOM";

        // PALETTES
        private const string XMLTAG_PALETTE    = "PALETTE";

        // TABS
        private const string  XMLTAG_TAB       = "TAB";
        private const string  XMLTAG_TAG       = "tag";
        private const string  XMLTAG_TEXT      = "text";
        private const string  XMLTAG_TT        = "tt";
        private const string  XMLTAG_TYPE      = "type";
        private const string  XMLTAG_XY_WH     = "xy_wh";
        private const string  XMLTAG_ZOOM      = "zoom";

        //}}}


        // LoadXMLProfile {{{
        public static List<NotePane> LoadXMLProfile(string profileName)
        {
            log("ProfileManager.LoadXMLProfile("+profileName+"):");

            List<NotePane> np_list = new List<NotePane>();

            // access file
            string filePath = Settings.UserProfileFolder + profileName + XML_EXTENSION;
            if( !System.IO.File.Exists( filePath ) ) {
                log("***file not found:\n"+ filePath);
                return np_list;
            }

            // load file
            XmlTextReader reader  = null;
            try {
                NotePane        np = null;
                bool reading_node = false;
                reader = new XmlTextReader( filePath );
                while( reader.Read() )
                {
                    // TAB {{{
                    if(reader.Name == XMLTAG_TAB)
                    {
                        if(reader.NodeType == XmlNodeType.Element) {
                            np = new NotePane();
                            reading_node = true;
                        }
                        else if(reader.NodeType == XmlNodeType.EndElement)
                        {
                            if(np.Type != 0)
                                np_list.Add( np );
                            reading_node = false;
                        }
                    }
                    //}}}
                    AutoPlacePoint_INCR;// TAB elements {{{
                    else if( reading_node ) {
                        if     (reader.Name == XMLTAG_TYPE  ) np.Type  =((string)reader.ReadElementContentAs(typeof(string), "")).Trim();
                        else if(reader.Name == XMLTAG_TAG   ) np.Tag   =((string)reader.ReadElementContentAs(typeof(string), "")).Trim();
                        else if(reader.Name == XMLTAG_ZOOM  ) np.zoom  =((string)reader.ReadElementContentAs(typeof(string), "")).Trim();
                        else if(reader.Name == XMLTAG_XY_WH ) np.xy_wh =((string)reader.ReadElementContentAs(typeof(string), "")).Trim();
                        else if(reader.Name == XMLTAG_TEXT  ) np.text  =((string)reader.ReadElementContentAs(typeof(string), "")).Trim();
                        else if(reader.Name == XMLTAG_TT    ) np.tt    =((string)reader.ReadElementContentAs(typeof(string), "")).Trim();
                    }
                    //}}}
                }
            }
            catch(Exception ex) {
                log("***ProfileManager.LoadProfileXML("+profileName+") Exception:\n"+ ex);
            }
            finally {
                if(reader != null)
                    reader.Close();
            }

            if( np_list.Count > 0)
                log("...return a NotePane list of "+ np_list.Count +" entries");
            else
                log("***Loading "+filePath+" returns an empty NotePane list");
            return np_list;

        }
        //}}}
        // SaveXMLProfile {{{
        public static void SaveXMLProfile(string profileName, List<NotePane> np_list)
        {
            log("ProfileManager.SaveXMLProfile("+profileName+"):");

            // Create file
            string filePath = Settings.UserProfileFolder + profileName + XML_EXTENSION;
            StreamWriter sw = new StreamWriter( filePath );

            // Write header
            sw.Write(      XMLDECLARATION       + "\n");
            sw.Write("<" + XMLTAG_RTABSPROFILE  +">\n");

            // Write NotePane list
            foreach(NotePane np in np_list)
                sw.Write(NotePane_ToXML( np ) +"\n");

            // Write footer
            sw.Write("</"+ XMLTAG_RTABSPROFILE +">\n");

            // Close file
            sw.Close();

        }
        //}}}
        // DeleteProfile {{{
        public static void DeleteProfile(string profileName)
        {
            log("ProfileManager.DeleteProfile("+profileName+"):");

            string filePath = Settings.UserProfileFolder + profileName + XML_EXTENSION;

            if( System.IO.File.Exists( filePath ) ) {
                log("...deleting "+filePath+":");
                System.IO.File.Delete( filePath );
            }

        }
        //}}}

        // NotePane_ToXML {{{
        private static string NotePane_ToXML(NotePane np)
        {
            return " <"+  XMLTAG_TAB    +">"
                +   " <"+  XMLTAG_TYPE  +">"+ string.Format("{0,2}" , np.Type ) +"</"+ XMLTAG_TYPE  +">"
                +   " <"+  XMLTAG_TAG   +">"+ string.Format("{0,3}" , np.Tag  ) +"</"+ XMLTAG_TAG   +">"
                +   " <"+  XMLTAG_ZOOM  +">"+ string.Format("{0,2}" , np.zoom ) +"</"+ XMLTAG_ZOOM  +">"
                +   " <"+  XMLTAG_XY_WH +">"+ string.Format("{0,10}", np.xy_wh) +"</"+ XMLTAG_XY_WH +">"
                +   " <"+  XMLTAG_TEXT  +">"+ string.Format("{0,10}", np.text ) +"</"+ XMLTAG_TEXT  +">"
                +   " <"+  XMLTAG_TT    +">"+ string.Format("{0,10}", np.tt   ) +"</"+ XMLTAG_TT    +">"
                +  " </"+ XMLTAG_TAB    +">"
                ;

        }
        //}}}
        private void log(string msg)// {{{
        {
            Logger.Log(typeof(TabsCollection).Name, msg+"\n");
        }
        // }}}
    }
}

/*
:!start explorer "https://msdn.microsoft.com/en-us/library/System.Xml.XmlReader(v=vs.110).aspx"
*/
