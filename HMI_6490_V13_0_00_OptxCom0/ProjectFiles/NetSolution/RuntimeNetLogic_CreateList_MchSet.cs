/*

Author:         Alessandro TONIONI
Date:           30/05/2024

Description:    This script is used to create the istances that compose the list of Mechanincal Reference.
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

public class RuntimeNetLogic_CreateList_MchSet : BaseNetLogic
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
        Log.Warning("RuntimeNetLogic_CreateList_MchSet", "Create start");

        //Clear of any existing object
        Owner.Get("ScrollView/VerticalLayout").Children.Clear();

        //Catch the istance number
        var IstanceNumber = LogicObject.GetVariable("Number").Value;

        if (IstanceNumber == 0) 
        {
            Log.Error("RuntimeNetLogic_CreateList_MchSet","Number of istance not set");
        }

        for (int i = 0; i < (IstanceNumber) ; i++)
        {
        
            var WidgetInstance = InformationModel.Make<list_MechanicalSettings>("Parameter_" + i);
            WidgetInstance.GetVariable("ParameterIndex").Value = i;
                   
            Owner.Get("ScrollView/VerticalLayout").Add(WidgetInstance);

            LogicObject.GetVariable("Progress").Value = i;

        }

        Log.Warning("RuntimeNetLogic_CreateList_MchSet", "Create ended");
        createTask?.Dispose();
    }

    LongRunningTask createTask;
}
