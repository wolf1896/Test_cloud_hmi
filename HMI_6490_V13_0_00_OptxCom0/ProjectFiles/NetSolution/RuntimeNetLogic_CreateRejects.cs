/*

Author:         Alessandro TONIONI
Date:           30/05/2024

Description:    This script is used to create the istances that compose the list of REJECTS.
                The input variable "Number" define the number of istance to create;
                The output variable"Progress" is used to animate the loading bar.

*/

#region Using directives

using UAManagedCore;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
#endregion

public class RuntimeNetLogic_CreateRejects : BaseNetLogic
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
        Log.Warning("RuntimeNetLogic_CreateRejects", "Create start");

        //Clear of any existing object
        Owner.Get("ScrollView/VerticalLayout").Children.Clear();

        //Catch the istance number
        var IstanceNumber = LogicObject.GetVariable("Number").Value;

        if (IstanceNumber == 0) 
        {
            Log.Error("RuntimeNetLogic_CreateRejects","Number of istance not set");
        }

        for (int i = 0; i < IstanceNumber; i++)

        {        

            var WidgetInstance = InformationModel.Make<list_Reject>("Reject_" + i);
            WidgetInstance.GetVariable("RejectNumber").Value = i;

            if (i < 10)
                {
                LocalizedText Key = new LocalizedText(WidgetInstance.NodeId.NamespaceIndex, "REJECT0"+i);
                WidgetInstance.GetVariable("Text").Value = Key;
                }
            else if ((i >= 10) && (i < 100))
                {
                LocalizedText recipeKey = new LocalizedText(WidgetInstance.NodeId.NamespaceIndex, "REJECT"+i);
                WidgetInstance.GetVariable("Text").Value = recipeKey;
                }

            Owner.Get("ScrollView/VerticalLayout").Add(WidgetInstance);
            LogicObject.GetVariable("Progress").Value = i;
        
        
        }

        Log.Warning("RuntimeNetLogic_CreateRejects", "Create end");

        createTask?.Dispose();
    }

    LongRunningTask createTask;
}
