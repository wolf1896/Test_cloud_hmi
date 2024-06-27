/*

Author:         Alessandro TONIONI
Date:           30/05/2024

Description:    This script is used to create the istances that compose the list of "Motor list" and "Active Motor" popup.
                The input variable "Number" define the number of istance to create.
                The output variables "Progress" is used to animate the loading bar.
*/

#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Alarm;
using FTOptix.UI;
using FTOptix.RAEtherNetIP;
using FTOptix.NativeUI;
using FTOptix.CoreBase;
using FTOptix.NetLogic;
using FTOptix.AuditSigning;
using FTOptix.EventLogger;
using FTOptix.CommunicationDriver;
using FTOptix.Retentivity;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.System;
using FTOptix.Recipe;
using FTOptix.Core;
using FTOptix.Alarm;
using System.Linq;
using FTOptix.DataLogger;
using FTOptix.Report;
using FTOptix.OPCUAClient;
using FTOptix.TwinCAT;
#endregion

public class RuntimeNetLogic_CreateList_Motor : BaseNetLogic
{
    public override void Start()
    {
        createTask = new LongRunningTask(AsyncTask, LogicObject);
        createTask.Start();
    }

    [ExportMethod]
    public void RuntimeCreationParList()
    {
        createTask = new LongRunningTask(AsyncTask, LogicObject);
        createTask.Start();
    }

    private void AsyncTask()
    {
        Log.Warning("RuntimeNetLogic_CreateList_Motor", "Create start");

        //Clear of any existing object
        Owner.Get("ScrollView_List/VerticalLayout").Children.Clear();
        Owner.Get("ScrollView_Active/VerticalLayout").Children.Clear();

        //Catch the istance number
        var IstanceNumber = LogicObject.GetVariable("Number").Value;

        if (IstanceNumber == 0) 
        {
            Log.Error("RuntimeNetLogic_CreateList_Motor", "Number of istance not set");
        }

        for (int i = 0; i < (IstanceNumber) ; i++)
        {
        
            var WidgetInstanceList = InformationModel.Make<list_ManualServoMotor>("MotorList_" + i);
            var WidgetInstanceActive = InformationModel.Make<list_ManualServoMotor>("MotorActive_" + i);

            WidgetInstanceList.GetVariable("AxisNumber").Value = i;
            WidgetInstanceActive.GetVariable("AxisNumber").Value = i;

            WidgetInstanceActive.GetVariable("AsActiveAxisList").Value = 1;
            
            Owner.Get("ScrollView_List/VerticalLayout").Add(WidgetInstanceList);
            Owner.Get("ScrollView_Active/VerticalLayout").Add(WidgetInstanceActive);
            
            LogicObject.GetVariable("Progress").Value = i;

        }

        Log.Warning("RuntimeNetLogic_CreateList_Motor", "Create end");
        createTask?.Dispose();
    }

    LongRunningTask createTask;
}
