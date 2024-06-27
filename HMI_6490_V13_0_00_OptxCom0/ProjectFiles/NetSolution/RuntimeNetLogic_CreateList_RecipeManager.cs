/*

Author:         Alessandro TONIONI
Date:           30/05/2024

Description:    This script is used to create the istances that compose the list of Recipes.
                The output variables "Number" and "Progress" are used to animate the loading bar.

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

public class RuntimeNetLogic_CreateList_RecipeManager : BaseNetLogic
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
        Log.Warning("RuntimeNetLogic_CreateList_RecipeManager", "Create started");

        //Clear of any existing object
        Owner.Get("ScrollView/VerticalLayout").Children.Clear();

        //Catch the Array dimension
        var IstanceNumber =  LogicObject.GetVariable("Number").Value;

        //List start from i=1 because Recipe 0 is use as bridge to run "Compare" and "Backup" functions
        for (int i = 1; i < IstanceNumber; i++)
        {
            var WidgetInstance = InformationModel.Make<list_RecipeSelection>("Parameter_" + i);
            WidgetInstance.GetVariable("RecipeNumber").Value = i;
                   
            Owner.Get("ScrollView/VerticalLayout").Add(WidgetInstance);

            LogicObject.GetVariable("Progress").Value = i;
        }

        Log.Warning("RuntimeNetLogic_CreateList_RecipeManager", "Create ended");
        createTask?.Dispose();
    }

    LongRunningTask createTask;
}
