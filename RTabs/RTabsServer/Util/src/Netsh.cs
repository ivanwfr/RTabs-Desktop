// using {{{
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Util;
using System.Text.RegularExpressions;
using System.Net;
// }}}
/*
:!start explorer "https://msdn.microsoft.com/en-us/library/System.Net.NetworkInformation.NetworkInterface(v=vs.110).aspx"
:!start explorer "https://technet.microsoft.com/en-us/library/dd734783(v=ws.10).aspx"
*/
public static class Netsh
{
    // GetIPAddress {{{
    public static string GetIPAddress(string hostNameOrAddress)
    {
        Log("GetIPAddress("+ hostNameOrAddress +")");
        try {

            IPHostEntry ipHostEntry = Dns.GetHostEntry( hostNameOrAddress );
            Log("...IPHostEntry.AddressList.Length: "+ ipHostEntry.AddressList.Length);

            IPAddress ipAddress = null;
            foreach(IPAddress ipa in ipHostEntry.AddressList)
            {
                Log(" "+ ipa);
                if(ipa.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) // i.e. IPV4
                {
                    ipAddress = ipa;
                    Log("...resolved ipAddress=["+ ipa +"]");
                }
            }

            if(ipAddress != null) return ipAddress.ToString();
            else                  return "";

        }
        catch(Exception ex) {
            Log("GetHostEntry(hostNameOrAddress=["+ hostNameOrAddress +"] Exception:\n"+ex.Message);
        }
        return "";
    }
    // }}}
    // GetIP {{{
    public static string GetIP()
    {
        string filter   = @"IP Address:\s*((([0-9])+\.){3}([0-9]+){1})";
        string cmd      =  "netsh interface ipv4 show config";
        string result   = CommandFilter(cmd, filter);
        Log("GetIP: "+ result);
        return result;
    }
    //}}}
    // GetSUBNET {{{
    public static string GetSUBNET()
    {
        string filter   = @"Subnet.*mask\s(((([0-9])+\.){3}([0-9]+){1}))";
        string cmd      =  "netsh interface ipv4 show config";
        string result   = CommandFilter(cmd, filter);
        Log("GetSUBNET: "+ result);
        return result;
    }
    //}}}
    //  TODO string filter   = @"((([0-9a-fA-F]){2}[-:]){5}([0-9a-fA-F]){2})";
/*
    // GetConfig {{{
    public static string GetConfig()
    {
        string  filter  = ClipboardAsync.GetText();
        if(    (filter.IndexOf('(') < 0)
            || (filter.IndexOf(')') < 0)
          )
            filter      = "(.*)";

        string cmd      = "netsh interface ipv4 show config";
        string result   = CommandFilter(cmd, filter, true);
        Log("GetConfig: "+ result);
        return result;
    }
    //}}}
*/
    // GetMAC {{{
    public static string GetMAC(string ip)
    {

            /*
               Physical Address. . . . . . . . . : 5C-F3-70-68-06-C1
               Physical Address. . . . . . . . . : E0-CB-4E-8D-91-BB
               IPv4 Address. . . . . . . . . . . : 192.168.1.14(Preferred) 
               Physical Address. . . . . . . . . : 00-50-56-C0-00-01
               IPv4 Address. . . . . . . . . . . : 192.168.133.1(Preferred) 
               Physical Address. . . . . . . . . : 00-50-56-C0-00-08
               IPv4 Address. . . . . . . . . . . : 192.168.153.1(Preferred) 
               Physical Address. . . . . . . . . : 00-00-00-00-00-00-00-E0
               Physical Address. . . . . . . . . : 00-00-00-00-00-00-00-E0
               Physical Address. . . . . . . . . : 00-00-00-00-00-00-00-E0
               Physical Address. . . . . . . . . : 00-00-00-00-00-00-00-E0
               Physical Address. . . . . . . . . : 00-00-00-00-00-00-00-E0
               Physical Address. . . . . . . . . : 00-00-00-00-00-00-00-E0
             */
        string cmd     = "ipconfig /all";
        string filter  = ".*(physical|ipv4).*";
        string result  = CommandFilter(cmd, filter, true);

        string[] lines = result.Split('\n');
        result = "";

        string mac = "";
        Regex regex = new Regex(".*: ");
        for(int i=0; i< lines.Length; ++i)
        {
            string line = regex.Replace(lines[i], "");
            if( line.StartsWith(ip) )
                break;
            else
                mac = line;
        }

        Log("GetMAC("+ ip +") = "+ mac);
        return mac;
    }
    //}}}
    // CommandFilter {{{
    public static string CommandFilter(string cmd, string filter) { return CommandFilter(cmd, filter, false); }
    public static string CommandFilter(string cmd, string filter, bool verbose)// {{{
    {
        string result = Settings.ExecuteCommandAsAdmin( cmd );
        return ResultFilter(result, filter, verbose);
    }
    //}}}
    private static string ResultFilter(string result, string filter, bool verbose)// {{{
    {
        try {
            string answer   = "";

            Regex  regex    = new Regex(filter, RegexOptions.IgnoreCase);// | RegexOptions.Multiline);

            // value capture {{{
            if( !verbose )
            {
                Match match = regex.Match( result );
                if( match.Success )
                {
                    Group g = match.Groups[1];
                    answer  =   g.Captures[0].ToString();
                }

            }
            //}}}
            // output filter {{{
            else {
                MatchCollection matches = regex.Matches( result );
                if(matches.Count > 0)
                {
                    foreach(Match match in matches) {
                        //if(answer != "") answer += "\n";
                        answer += match.Value;
                        if( !verbose ) break;
                    }
                }
            }
            //}}}
            return answer.Replace("\r","\n");
        }
        catch(Exception ex) {
            return ""
                + "*** filter=["+ filter    +"]\n"
                + "*** "        + ex.Message
                ;
        }
    }
    //}}}
    // }}}

    // AllowThisProgram {{{
    public static string AllowThisProgram(string Direction)// {{{
    {
        Log("AllowThisProgram");

        int port_from = 10*(Settings.Port / 10); int port_to = port_from + 10;

        string Protocol     = "TCP";
        string LocalPorts   = string.Format("{0}-{1}", port_from, port_to);
        string RemotePorts  = "";

        Log("......Protocol=["+ Protocol    +"]");
        Log("...RemotePorts=["+ RemotePorts +"]");
        Log("....LocalPorts=["+ LocalPorts  +"]");
        Log(".....Direction=["+ Direction   +"]");

        // Windows Firewall seems to have some issues with mounted partitions where this project was located at design time
        string path  = ""; // AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName;
        // opening for "Any" programs on port nnnn

        return AllowProgram(Settings.APP_NAME, path, Protocol, RemotePorts, LocalPorts, Direction);
    }
    //}}}
    // }}}
    // AllowProgram {{{
    private static string AllowProgram(string programName, string programPath, string Protocol, string RemotePorts, string LocalPorts, string Direction)// {{{
    {
        Log("AllowProgram");
        Log("...programName=["+ programName +"]");
        Log("...programPath=["+ programPath +"]");
        Log("......Protocol=["+ Protocol    +"]");
        Log("...RemotePorts=["+ RemotePorts +"]");
        Log("....LocalPorts=["+ LocalPorts  +"]");
        Log(".....Direction=["+ Direction   +"]");

        // netsh advfirewall firewall    add rule name="NetBIOS TCP Port 139" dir=in action=allow protocol=TCP localport=139
        // netsh advfirewall firewall delete rule name="NetBIOS TCP Port 139"                     protocol=TCP localport=139

        string rule_name = programName ;

        // CmdDelete {{{
        string CmdDelete = ""
            +"netsh advfirewall firewall"
            +" delete rule name='"+ rule_name +"'"
            +" protocol="         + Protocol
            ;

        if(LocalPorts .Length > 0) CmdDelete += " localport=\"" + LocalPorts  + "\"";
        if(RemotePorts.Length > 0) CmdDelete += " remoteport=\""+ RemotePorts + "\"";
        if(programPath.Length > 0) CmdDelete += " program=\""   + programPath + "\"";

        Log("=== CmdDelete=["+CmdDelete+"]");

        string result   = Settings.ExecuteCommandAsAdmin( CmdDelete );
        Log("=== ExecuteCommandAsAdmin:\n"+result.Replace(Environment.NewLine,"\n"));

        //}}}
        // CmdAdd {{{
        string CmdAdd = ""
            +"netsh advfirewall firewall"
            +" add rule name='"   + rule_name +"'"
            +" protocol="         + Protocol
            +" dir="              + Direction.ToLower()
            +" action=allow"
            ;

        if(LocalPorts .Length > 0) CmdAdd += " localport=\""  + LocalPorts  + "\""; else LocalPorts  = "Any";
        if(RemotePorts.Length > 0) CmdAdd += " remoteport=\"" + RemotePorts + "\""; else RemotePorts = "Any";
        if(programPath.Length > 0) CmdAdd += " program=\""    + programPath + "\""; else programPath = "Any";

        string description
            =" Allow "+ Direction
            +" "+       programPath +" program"
            +" using "+ Protocol
            ;

        if     (LocalPorts .IndexOf('-') > 0) description +=  "\n- local port range "+ LocalPorts;
        else if(LocalPorts.Length        > 0) description +=  "\n- local port"       + LocalPorts;

        if     (RemotePorts.IndexOf('-') > 0) description += "\n- remote port range "+ RemotePorts;
        else if(RemotePorts.Length       > 0) description += "\n- remote port"       + RemotePorts;

        CmdAdd += " description='"+description+"'";


        Log("=== CmdAdd=["+CmdAdd+"]");

        result = Settings.ExecuteCommandAsAdmin( CmdAdd );
        Log("=== ExecuteCommandAsAdmin:\n"+result.Replace(Environment.NewLine,"\n"));

        //}}}
        if( result.ToUpper().StartsWith("OK") ) return          description +"\n"+ result;
        else                                    return "***\n"+ description +"\n"+ result;
    }
    //}}}
    // }}}
//   AddNewRule {{{
//    private static bool AddNewRule(string RuleName, string Description, string RemoteAddresses, string RemotePorts, string LocalPorts, string Protocol, string Action, string Direction, string ApplicationName, bool DontUpdate)
//    {
//        Log("AddNewRule");
//        Log("..........RuleName=["+ RuleName         +"]");
//        Log(".......Description=["+ Description      +"]");
//        Log("...RemoteAddresses=["+ RemoteAddresses  +"]");
//        Log(".......RemotePorts=["+ RemotePorts      +"]");
//        Log("........LocalPorts=["+ LocalPorts       +"]");
//        Log("..........Protocol=["+ Protocol         +"]");
//        Log("............Action=["+ Action           +"]");
//        Log("...ApplicationName=["+ ApplicationName  +"]");
//        Log("........DontUpdate=["+ DontUpdate       +"]");
//
//        return false;
//    }
//    // }}}
    private static void Log(string msg)// {{{
    {
        Logger.Log(NotePane.PANEL_NAME_NETSH, msg+"\n");
    }
    // }}}
}

