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

public class DesignTime_AlarmCreate : BaseNetLogic
{
    [ExportMethod]
    public void Create()
    {
        Project.Current.Get("Alarms").Children.Clear();

        int alarmNum = 0;

        // Get variable from propertis UI
        IUAVariable ArrayDim = Project.Current.GetVariable("Others/NetLogic/DesignTime_AlarmCreate/Array Number");
        IUAVariable LastSeverity_1 = Project.Current.GetVariable("Others/NetLogic/DesignTime_AlarmCreate/Last with severity 1");
        IUAVariable LastSeverity_2 = Project.Current.GetVariable("Others/NetLogic/DesignTime_AlarmCreate/Last with severity 2");

        for (int Array = 0; Array<=ArrayDim.Value; Array++)
        {
            for (int Bit = 0; Bit<32; Bit++)
            {
                
                //Prendo il Type
                var ParWidgetType = Project.Current.Get ("Model/CommunicationTags/Alarm");
                var parWidgetInstance = InformationModel.MakeObject ("Alarm_" + alarmNum, ParWidgetType.NodeId);
                Project.Current.Get("Alarms").Children.Add(parWidgetInstance);
        
                var parWidget = Project.Current.Get("Alarms/Alarm_"+ alarmNum);
                parWidget.GetVariable("Array").Value = Array;
                parWidget.GetVariable("Bit").Value = Bit;

                {
                    if (alarmNum <=LastSeverity_1.Value)
                        {
                        parWidget.GetVariable("Gravity").Value = 1;
                        }
                    else if ((alarmNum > LastSeverity_1.Value) && (alarmNum <= LastSeverity_2.Value))
                        {
                        parWidget.GetVariable("Gravity").Value = 2;
                        }
                    else if (alarmNum > LastSeverity_2.Value)
                        {
                        parWidget.GetVariable("Gravity").Value = 3;
                        }
                }

                {
                    if (alarmNum < 10)
                        {
                        LocalizedText AlarmKey = new LocalizedText(parWidget.NodeId.NamespaceIndex, "ALARM00"+alarmNum);
                        parWidget.GetVariable("Text").Value = AlarmKey;
                        }
                    else if ((alarmNum >= 10) && (alarmNum < 100))
                        {
                        LocalizedText AlarmKey = new LocalizedText(parWidget.NodeId.NamespaceIndex, "ALARM0"+alarmNum);
                        parWidget.GetVariable("Text").Value = AlarmKey;
                        }
                    else if (alarmNum >= 100)
                        {
                        LocalizedText AlarmKey = new LocalizedText(parWidget.NodeId.NamespaceIndex, "ALARM"+alarmNum);
                        parWidget.GetVariable("Text").Value = AlarmKey;
                        }
                }

                alarmNum++;
            }

        }
    }
}
