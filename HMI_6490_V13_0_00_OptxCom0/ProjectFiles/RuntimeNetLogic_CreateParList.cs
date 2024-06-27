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
#endregion

public class RuntimeNetLogic_CreateParList : BaseNetLogic

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
        Log.Warning("CreateParameterList Started");
            
        Owner.Get("ScrollView/VerticalLayout").Children.Clear();

        for (int i = 0; i <= 110; i++)
        {

            /*

            var panel = InformationModel.MakeObject<Panel>("Parameter_"+i);
            panel.Height = 40;
            panel.HorizontalAlignment = HorizontalAlignment.Stretch;

            var label = InformationModel.MakeObject<Label>("Path");
            label.Text = "RECIPE_PAR";
            if (i != 0)
            {
                if (i < 10)
                    label.Text += "00" + i;
                else if ((i >= 10) && (i < 100))
                    label.Text += "0" + i;
                else if (i >= 100)
                    label.Text += i;
            }

            label.LeftMargin = 20;
            label.VerticalAlignment = VerticalAlignment.Center;
            panel.Add(label);
            */

            var parWidgetInstance = InformationModel.Make<list_RecipeParameter>("Parameter_" + i);
            parWidgetInstance.GetVariable("ParameterIndex").Value = i;
            if (i != 0)
            {
                if (i < 10)
                    parWidgetInstance.GetVariable("Text").Value = "RECIPE_PAR00" + i;
                else if ((i >= 10) && (i < 100))
                    parWidgetInstance.GetVariable("Text").Value = "RECIPE_PAR0" + i;
                else if (i >= 100)
                    parWidgetInstance.GetVariable("Text").Value = "RECIPE_PAR" + i;
            }
            Owner.Get("ScrollView/VerticalLayout").Add(parWidgetInstance);
        }
        Log.Warning("CreateParameterList Ended");
        createTask?.Dispose();
    }

    LongRunningTask createTask;
}
