/*

Author:         Mattia LUPO
Date:           07/06/2024

Description:    this script is used to sync time between PLC and HMI.

Note:           It is better to synchronize all program times using 'SetSystemTime' because directly setting the system time can cause various errors.
                By centralizing time synchronization through a dedicated function like SetSystemTime, you can avoid these potential pitfalls 
                and maintain the reliability and stability of your program.

*/
#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.Alarm;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.CoreBase;
using FTOptix.RAEtherNetIP;
using FTOptix.Recipe;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.EventLogger;
using FTOptix.DataLogger;
using FTOptix.Report;
using FTOptix.Retentivity;
using FTOptix.System;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using System.Globalization;
#endregion

public class RuntimeNetLogic_TimeSync : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        myPeriodicTask = new PeriodicTask(SetTimeFromPLC, 200, LogicObject);
        myPeriodicTask.Start();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        myPeriodicTask.Dispose();
    }

    public void SetTimeFromPLC() //get PLC time and write into HMI Sysytem time
    {
        /* IUAVariable set_data = LogicObject.GetVariable("SetSystemTime");

        int year = LogicObject.GetVariable("year").RemoteRead();
        int month = LogicObject.GetVariable("month").RemoteRead();
        int day = LogicObject.GetVariable("day").RemoteRead();
        int hour = LogicObject.GetVariable("hour").RemoteRead();
        int minute = LogicObject.GetVariable("minute").RemoteRead();
        int second = LogicObject.GetVariable("second").RemoteRead();

        DateTime date = new DateTime(year, month, day, hour, minute, second);

        //Log.Warning(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        set_data.SetValue(date);
        */

        // Get variables from PLC, set the path in UI using the propertis of the scrip
        int year = LogicObject.GetVariable("year").RemoteRead();
        int month = LogicObject.GetVariable("month").RemoteRead();
        int day = LogicObject.GetVariable("day").RemoteRead();
        int hour = LogicObject.GetVariable("hour").RemoteRead();
        int minute = LogicObject.GetVariable("minute").RemoteRead();
        int second = LogicObject.GetVariable("second").RemoteRead();

        // Set new time in HMI, all program that need time are link to this variable ("SetSystemTime") 
        LogicObject.GetVariable("SetSystemTime").Value = new DateTime(year, month, day, hour, minute, second);
    }

    [ExportMethod] // Use to expose function and make it avaible from hmi UI
    public void SetTimeToPLC() // Take a variable from HMI and write into PLC to set time
    {
        // Get the variable from HMI and send to PLC
        DateTime date = LogicObject.GetVariable("SetPLCDate").Value;

        LogicObject.GetVariable("SetYear").Value = date.Year;
        LogicObject.GetVariable("SetMonth").Value = date.Month;
        LogicObject.GetVariable("SetDay").Value = date.Day;
        LogicObject.GetVariable("SetHour").Value = date.Hour;
        LogicObject.GetVariable("SetMinute").Value = date.Minute;
        LogicObject.GetVariable("SetSecond").Value = 0.0; // IT WORK'S. No need to set seconds, but without it there is a problem in the array lenght, need more investigation.

        Project.Current.GetVariable("Model/CommunicationTags/Mix_SetDateTimeCMD").Value = true; // Use the 'Mix_SetDateTimeCMD' command to change the time with a handshake. This command is designed to manage the handshake variable more effectively.

    }

    private PeriodicTask myPeriodicTask;
}