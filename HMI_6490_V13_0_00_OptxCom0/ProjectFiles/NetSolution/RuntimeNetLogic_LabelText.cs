/*

Author:         Alessandro TONIONI
Date:           30/05/2024

Description:    This script is used to write the Text Localized on dictionary, used on Cards

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
using FTOptix.UI;
using FTOptix.Core;
#endregion

public class RuntimeNetLogic_LabelText : BaseNetLogic
{
    public override void Start()
    {
        var Num =  LogicObject.GetVariable("ParNum").Value;

        if (Num < 10)
            {
            var Key = new LocalizedText("RECIPE_PAR00"+Num);
            var Translation = InformationModel.LookupTranslation(Key);
            LogicObject.GetVariable("Text").Value = Translation.Text;
            }
        else if ((Num >= 10) && (Num < 100))
            {
            var Key = new LocalizedText("RECIPE_PAR0"+Num);
            var Translation = InformationModel.LookupTranslation(Key);
            LogicObject.GetVariable("Text").Value = Translation.Text;
            }
        else if (Num >= 100)
            {
            var Key = new LocalizedText("RECIPE_PAR"+Num);
            var Translation = InformationModel.LookupTranslation(Key);
            LogicObject.GetVariable("Text").Value = Translation.Text;
            }
        
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
}
