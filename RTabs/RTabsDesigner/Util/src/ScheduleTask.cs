// using {{{
using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;

using Util;
// }}}
public static class ScheduleTask
{
    // TASK_TEMPLATE {{{
        private static string TASK_TEMPLATE = ""
        +"<?xml version=\"1.0\" encoding=\"UTF-16\"?>\n"
        +"<Task version=\"1.2\" xmlns=\"http://schemas.microsoft.com/windows/2004/02/mit/task\">\n"
        +""
        +" <RegistrationInfo>\n"
        +"  <Date>___DATE___T11:37:54.5063431</Date>\n"
        +"  <Author>___AUTHOR___</Author>\n"
        +"  <Description>___DESCRIPTION___</Description>\n"
        +" </RegistrationInfo>\n"
        +""
        +" <Triggers>\n"
        +"  <LogonTrigger>\n"
        +"   <Enabled>true</Enabled>\n"
        +"  </LogonTrigger>\n"
        +" </Triggers>\n"
        +""
        +" <Principals>\n"
        +"  <Principal id=\"Author\">\n"
        +"   <UserId>___AUTHOR___</UserId>\n"
        +"   <LogonType>InteractiveToken</LogonType>\n"
        +"   <RunLevel>HighestAvailable</RunLevel>\n"
        +"  </Principal>\n"
        +" </Principals>\n"
        +""
        +" <Settings>\n"
        +"  <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>\n"
        +"  <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>\n"
        +"  <StopIfGoingOnBatteries>true</StopIfGoingOnBatteries>\n"
        +"  <AllowHardTerminate>true</AllowHardTerminate>\n"
        +"  <StartWhenAvailable>false</StartWhenAvailable>\n"
        +"  <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>\n"
        +"  <IdleSettings>\n"
        +"   <StopOnIdleEnd>true</StopOnIdleEnd>\n"
        +"   <RestartOnIdle>false</RestartOnIdle>\n"
        +"  </IdleSettings>\n"
        +"  <AllowStartOnDemand>true</AllowStartOnDemand>\n"
        +"  <Enabled>true</Enabled>\n"
        +"  <Hidden>false</Hidden>\n"
        +"  <RunOnlyIfIdle>false</RunOnlyIfIdle>\n"
        +"  <WakeToRun>false</WakeToRun>\n"
        +"  <ExecutionTimeLimit>PT0S</ExecutionTimeLimit>\n"
        +"  <Priority>7</Priority>\n"
        +" </Settings>\n"
        +""
        +" <Actions Context=\"Author\">\n"
        +"  <Exec>\n"
        +"   <Command>___FILENAME___</Command>\n"
        +"  </Exec>\n"
        +" </Actions>\n"
        +""
        +"</Task>\n"
        ;
    // }}}

    public static bool AddNewTask(string taskName, string description)// {{{
    {
        string fileName = Application.ExecutablePath;

        string author   = Environment.UserName;

        try {
            string mm   = DateTime.Now.Month.ToString(); if(mm.Length < 2)   mm = "0"+mm;
            string dd   = DateTime.Now.Day.ToString()  ; if(dd.Length < 2)   dd = "0"+dd;
            string yy   = DateTime.Now.Year.ToString() ;
            string date = yy+"-"+mm+"-"+dd;

            string task_string  = TASK_TEMPLATE
                .Replace("___FILENAME___"   , fileName           )
                .Replace("___DESCRIPTION___", description        )
                .Replace("___AUTHOR___"     , author             )
                .Replace("___DATE___"       , date               )
                .Replace("\n"               , Environment.NewLine)
                ;

            // write script file
            string task_file_name = Application.StartupPath + "\\Task.xml";

            Log("AddNewTask("+taskName+"):\n"
            +"FILE=["+task_file_name+"]\n"
            +"TASK:\n"
            +"---\n"
            + task_string+"\n"
            +"---"
            );

            Log("...Directory.GetCurrentDirectory=["+ Directory.GetCurrentDirectory() +"]");

            File.WriteAllText(task_file_name, task_string);

            // delete previous task
            string output;
            try {
                output    = Settings.ExecuteCommandAsAdmin(@"schtasks.exe /Delete /tn  '"+ taskName +"' /F");
            } catch(Exception) { }

            // add new task
            output        = Settings.ExecuteCommandAsAdmin(@"schtasks.exe /Create /XML '"+task_file_name +"' /tn '"+taskName+"'");

            Log( output );

            return output.StartsWith("SUCCESS:");
        }
        catch(Exception ex) {
            Log("AddNewTask Error:\n"+ ex);
        }
        return false;
    }
    // }}}

    private static void Log(string msg)// {{{
    {
        Logger.Log(NotePane.PANEL_NAME_AUTOSTART, msg+"\n");
    }
    // }}}
}

