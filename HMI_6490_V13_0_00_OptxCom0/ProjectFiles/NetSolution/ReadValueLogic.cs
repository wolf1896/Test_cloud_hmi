/*

Author:         Mattia LUPO
Date:           04/06/2024

Description:    this script is used to populate the graph pens that require two-dimensional Arrays as input.

*/

#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using System.IO;
using FTOptix.Report;
using System.Threading;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using FTOptix.Store;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using FTOptix.RAEtherNetIP;
using FTOptix.CommunicationDriver;
using FTOptix.AuditSigning;
#endregion

public class ReadValueLogic : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        myPeriodicTask = new PeriodicTask(ReadPos, 50, LogicObject);
        myPeriodicTask.Start();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        myPeriodicTask.Dispose();
    }

    public void ReadPos() {
        IUAVariable MatrixOverTravelA;
        IUAVariable MatrixRobotActualPosition;
        IUAVariable MatrixPointPosition;
        IUAVariable MatrixSelectedPointPosition;
        IUAVariable enable;

        float[,] tempVar;
        float[,] tempVar1;
        float[,] tempVar2;
        float[,] tempVar3;

        //OverTravel

        MatrixOverTravelA = LogicObject.GetVariable("OUT_OverTravelScript");
        
        tempVar = (float[,])MatrixOverTravelA.Value.Value;
        
        tempVar[0, 1] = (float)LogicObject.GetVariable("IN_OT_PosY").Value;
        tempVar[1, 1] = (float)LogicObject.GetVariable("IN_OT_PosZ").Value;
        tempVar[0, 2] = (float)LogicObject.GetVariable("IN_OT_NegY").Value;
        tempVar[1, 2] = (float)LogicObject.GetVariable("IN_OT_PosZ").Value;
        tempVar[0, 3] = (float)LogicObject.GetVariable("IN_OT_NegY").Value;
        tempVar[1, 3] = (float)LogicObject.GetVariable("IN_OT_NegZ").Value;
        tempVar[0, 0] = (float)LogicObject.GetVariable("IN_OT_PosY").Value;
        tempVar[1, 0] = (float)LogicObject.GetVariable("IN_OT_NegZ").Value;
        
        MatrixOverTravelA.SetValue(tempVar);
        
        //RobotPosition

        MatrixRobotActualPosition = LogicObject.GetVariable("OUT_RobotActualPosition");
        
        tempVar1 = (float[,])MatrixRobotActualPosition.Value.Value;

        tempVar1[0, 0] = LogicObject.GetVariable("IN_ActualPosX").Value;
        tempVar1[0, 1] = LogicObject.GetVariable("IN_ActualPosY").Value;
        tempVar1[1, 0] = LogicObject.GetVariable("IN_ActualPosX").Value;
        tempVar1[1, 1] = LogicObject.GetVariable("IN_ActualPosY").Value;

        MatrixRobotActualPosition.SetValue(tempVar1);

        
        //Point Position
        
        MatrixPointPosition = LogicObject.GetVariable("OUT_PointPosition");

        tempVar2 = (float[,])MatrixPointPosition.Value.Value;

        tempVar2[0, 0] = (float)LogicObject.GetVariable("IN_PickPosY").Value;
        tempVar2[1, 0] = (float)LogicObject.GetVariable("IN_PickPosZ").Value;
        tempVar2[0, 1] = (float)LogicObject.GetVariable("IN_WaitPickPosY").Value;
        tempVar2[1, 1] = (float)LogicObject.GetVariable("IN_WaitPickPosZ").Value;

        tempVar2[0, 2] = (float)LogicObject.GetVariable("IN_WaitPlacePosY").Value;
        tempVar2[1, 2] = (float)LogicObject.GetVariable("IN_WaitPlacePosZ").Value;
        tempVar2[0, 3] = (float)LogicObject.GetVariable("IN_PlacePosY").Value;
        tempVar2[1, 3] = (float)LogicObject.GetVariable("IN_PlacePosZ").Value;

        MatrixPointPosition.SetValue(tempVar2);

        // For selected point
        MatrixSelectedPointPosition = LogicObject.GetVariable("OUT_SelectedPointPosition");

        tempVar3 = (float[,])MatrixSelectedPointPosition.Value.Value;
        int index = LogicObject.GetVariable("IN_IndexPointPosition").Value;
        enable = LogicObject.GetVariable("IN_EnableChartPoint");


        if (enable.Value && (tempVar3[0, 0] != tempVar2[0, index] || tempVar3[1, 0] != tempVar2[1, index]))
        {
            LogicObject.GetVariable("OUT_EnableChartPoint").Value = true;
            tempVar3[0, 0] = tempVar2[0, index] ;
            tempVar3[1, 0] = tempVar2[1, index] ;

            MatrixSelectedPointPosition.SetValue(tempVar3);
            enable.SetValue(false);
        }
        else if(enable.Value)
        {
            LogicObject.GetVariable("OUT_EnableChartPoint").Value = false;
            enable.SetValue(false);
        }

        
    }

    private PeriodicTask myPeriodicTask;
}
