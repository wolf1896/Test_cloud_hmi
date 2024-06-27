/*

Author:         Alessandro TONIONI
Date:           30/05/2024

Description:    This script is used to create the istances that compose the list of GLOBAL parameters.
                The instances are created only if they parameter limit is greater than 0.
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

public class RuntimeNetLogic_CreateList_Global : BaseNetLogic
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
        Log.Warning("RuntimeNetLogic_CreateList_Global","Create started");

        //Clear of any existing object
        Owner.Get("ScrollView/VerticalLayout").Children.Clear();

        //Read the Array from PLC
        float[] tempVar= Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver/CLX/Tags/Controller Tags/HMI_CONFIG/GLOBAL_PAR_MAX_VALUE").RemoteRead();

        //Catch the Array dimension
        var IstanceNumber = tempVar.Count();
        LogicObject.GetVariable("Number").Value = IstanceNumber;

        for (int i = 0; i < IstanceNumber ; i++)
        {
            //If the Array value in greater than 0 the instance can be created
            if (tempVar[i] > 0) 
            {

                var WidgetInstance = InformationModel.Make<list_GlobalParameter>("Parameter_" + i);
                WidgetInstance.GetVariable("ParameterIndex").Value = i;
                    
                if (i < 10)
                    {
                    LocalizedText Key = new LocalizedText(WidgetInstance.NodeId.NamespaceIndex, "GLOBAL_PAR00"+i);
                    WidgetInstance.GetVariable("Text").Value = Key;
                }
                else if ((i >= 10) && (i < 100))
                {
                    LocalizedText recipeKey = new LocalizedText(WidgetInstance.NodeId.NamespaceIndex, "GLOBAL_PAR0"+i);
                    WidgetInstance.GetVariable("Text").Value = recipeKey;
                }
                else if (i >= 100)
                {
                    LocalizedText recipeKey = new LocalizedText(WidgetInstance.NodeId.NamespaceIndex, "GLOBAL_PAR"+i);
                    WidgetInstance.GetVariable("Text").Value = recipeKey;
                }
                    
                Owner.Get("ScrollView/VerticalLayout").Add(WidgetInstance);
            }
                
            LogicObject.GetVariable("Progress").Value = i;

        }

        Log.Warning("RuntimeNetLogic_CreateList_Global","Create Ended");
        createTask?.Dispose();
    }

    LongRunningTask createTask;
}
