/*

Author:         Alessandro TONIONI
Date:           30/05/2024

Description:    This script is used to create the istances that compose the list of SERVICE KIT.
                The input variable "Number" define the number of istance to create;
                The output variable "Progress" is used to animate the loading bar.

*/

#region Using directives

using UAManagedCore;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
#endregion

public class RuntimeNetLogic_CreateServiceKit : BaseNetLogic
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
        Log.Warning("RuntimeNetLogic_CreateServiceKit: Create end");

        //Clear of any existing object
        Owner.Get("ScrollView/VerticalLayout").Children.Clear();

        //Catch the istance number
        var IstanceNumber = LogicObject.GetVariable("Number").Value;

        if (IstanceNumber == 0) 
        {
            Log.Error("RuntimeNetLogic_CreateServiceKit", "Number of istance not set");
        }

        for (int i = 0; i < (IstanceNumber) ; i++)

        {
                var WidgetInstance = InformationModel.Make<list_ServiceKit>("ServiceKit_" + i);
                WidgetInstance.GetVariable("KitNumber").Value = i;

                Owner.Get("ScrollView/VerticalLayout").Add(WidgetInstance);
                LogicObject.GetVariable("Progress").Value = i;
        }

        Log.Warning("RuntimeNetLogic_CreateServiceKit: Create end");

        createTask?.Dispose();
    }

    LongRunningTask createTask;
}
