/*

Author:         Alessandro TONIONI
Date:           30/05/2024

Description:    This script is used to create the istances that compose the list of OutputCard into IO Diagnostic popup.
                The instances are created only if the ShieldType is set as Ouput 0r Mix (in case of ArmorBlock).
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
using System.Linq;
using FTOptix.DataLogger;
using FTOptix.Report;
using FTOptix.OPCUAClient;
using FTOptix.TwinCAT;
#endregion

public class RuntimeNetLogic_CreateCardOutput : BaseNetLogic
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
        Log.Warning("RuntimeNetLogic_CreateCardOutput", "Create start");

        //Clear of any existing object
        Owner.Get("ScrollView/VerticalLayout").Children.Clear();

        //Catch the istance number
        var IstanceNumber = LogicObject.GetVariable("Number").Value;

        if (IstanceNumber == 0)
        {
            Log.Error("RuntimeNetLogic_CreateCardOutput", "Number of instance not set");
        }

        for (int i = 0; i < (IstanceNumber) ; i++)
        {
            //Catch the shield type array   
            int[] ShieldType = Project.Current.GetVariable("Model/LocalTags/IO_ShieldType").Value; 

            //If type is 2 (as output) or 3 (as ArmorBlock) the istance can be created
            if ((ShieldType[i] == 2) || (ShieldType[i]) == 3)
            {
                var WidgetInstance = InformationModel.Make<card_DigitalInputOutput32>("Card_" + i);
                WidgetInstance.GetVariable("Shield").Value = i;
                WidgetInstance.GetVariable("OutputType").Value = true;

                Owner.Get("ScrollView/VerticalLayout").Add(WidgetInstance);
            }

            LogicObject.GetVariable("Progress").Value = i;
        
        }

        Log.Warning("RuntimeNetLogic_CreateCardOutput: Create ended");

        createTask?.Dispose();
    }

    LongRunningTask createTask;
}
