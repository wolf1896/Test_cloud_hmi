/*

Author:         Riccardo BOCCIA (Stage)
Date:           30/05/2024

Description:    This script is used to create the Movers that compose the XTS circuit visualizer and and converts
                their linear position expressed in millimeters to a Cartesian position, to align them with the circuit design
                
                The code is predisposed to change the movers path relative to real application (CW/CCW, Offset)
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
using FTOptix.Core;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
#endregion

public class RuntimeNetLogic_MoverVisManager : BaseNetLogic
{
    // Constant variables:
    private int MAX_LINEAR_LENGTH; // mm
    private int MIN_LINEAR_LENGTH; // mm
    private int MIN_CURVE_LENGTH; // mm
    private int MAX_CURVE_LENGTH; // mm
    private int CURVE_SECTION_RADIUS_PX; // px

    //Defining variables:
    LongRunningTask createTask;
    public double Straight_section_length;
    public double Straight_section_length_px;
    public double Curve_section_length;
    public double Stage_1;
    public double Stage_2;
    public double Stage_3;
    public double Stage_4;
    public double X;
    public double Y;
    public double R;
    public int[] FaultCode;
    public short[] LinearPos;
    public float DirectionOfRotation;
    public short Offset;
    public double Pos;
    public short NumberOfMovers;
    public double ActualPosition;
    public double sin;
    public double cos;
    public int SelectedMover;
    
    public override void Start()
    {
        // Getting variables that are going to be constant along running time:
        gettingConstantVariables();
        // Getting variables from project:
        gettingVariables();
        // Checking variables
        createMovers();
        if (DirectionOfRotation == 0) { // First cycle to setup movers' position
            MovementCW(0, NumberOfMovers);
        }
        else if (DirectionOfRotation == 1) {
            MovementCCW(0, NumberOfMovers);
        }
          
        createTask = new LongRunningTask(Manager, LogicObject);
        createTask.Start();
    }
    //GESTIRE INPUT COSTANTI DA PANNELLO
    //GESTIRE VETTORI

    public void createMovers() {

        //Svuota il contenitore "Movers"
        Owner.Get("Movers").Children.Clear();

        //Legge il numero di Mover da creare dal Owner (il contenitore)
        NumberOfMovers = LogicObject.GetVariable("NumberOfMovers").Value;

        if (NumberOfMovers == 0) 
        {
            Log.Error("RuntimeNetLogic_MoverVisManager","Number of mover not set");
        }

        Log.Warning("RuntimeNetLogic_MoverVisManager","Create started");

        for (int i = 0; i < NumberOfMovers; i++) {
        
            //Crea l'istanza e gli da un nome
            var WidgetInstance = InformationModel.Make<Mover>("Mover_" + i);

            //Assegna la variabile "Number"
            WidgetInstance.GetVariable("Number").Value = i;
                
            //Aggiungi l'istanza nel panrNumbenello "Movers"
            Owner.Get("Movers").Add(WidgetInstance);
   
        }

        Log.Warning("RuntimeNetLogic_MoverVisManager","Create ended");
    }

    public void gettingConstantVariables() {
        // These variables are supposed to be constant and to not be changed along running time
        MAX_LINEAR_LENGTH = 4000; // mm NOT USED AT THE MOMENT
        MIN_LINEAR_LENGTH = 0; // mm NOT USED AT THE MOMENT
        MIN_CURVE_LENGTH = 0; // mm NOT USED AT THE MOMENT
        MAX_CURVE_LENGTH = 1000; // mm NOT USED AT THE MOMENT
        CURVE_SECTION_RADIUS_PX = 60; // px
    }

    public void gettingVariables() {
        // These variables can change along running time
        FaultCode = LogicObject.GetVariable("FaultCode").Value;
        LinearPos = LogicObject.GetVariable("LinearPos").Value;
        NumberOfMovers = LogicObject.GetVariable("NumberOfMovers").Value;
        DirectionOfRotation = (float) LogicObject.GetVariable("DirectionOfRotation").Value;
        Offset = (short) LogicObject.GetVariable("Offset").Value;
        Pos = (double) LogicObject.GetVariable("Pos").Value;
        Straight_section_length = (double) LogicObject.GetVariable("Straight_section_length").Value; // mm
        Straight_section_length_px = (double) LogicObject.GetVariable("Straight_section_length_px").Value; // px
        Curve_section_length = (double) LogicObject.GetVariable("Curve_section_length").Value; // mm
        Stage_1 = Straight_section_length; // mm
        Stage_2 = Stage_1 + Curve_section_length; // mm
        Stage_3 = Stage_2 + Straight_section_length; // mm
        Stage_4 = Stage_3 + Curve_section_length; // mm
        SelectedMover = LogicObject.GetVariable("SelectedMover").Value;
    }

    public void valuesToBeUpdated() {
        DirectionOfRotation = (float) LogicObject.GetVariable("DirectionOfRotation").Value;
        Offset = (short) LogicObject.GetVariable("Offset").Value;
        Straight_section_length_px = ((Straight_section_length/250)*118)+30;
        Straight_section_length = (double) LogicObject.GetVariable("Straight_section_length").Value; // mm
        Curve_section_length = (double) LogicObject.GetVariable("Curve_section_length").Value; // mm
        Pos = (double) LogicObject.GetVariable("Pos").Value;
        LinearPos = (short []) LogicObject.GetVariable("LinearPos").Value;
        NumberOfMovers = LogicObject.GetVariable("NumberOfMovers").Value;
        SelectedMover = LogicObject.GetVariable("SelectedMover").Value;
        Stage_1 = Straight_section_length; // mm
        Stage_2 = Stage_1 + Curve_section_length; // mm
        Stage_3 = Stage_2 + Straight_section_length; // mm
        Stage_4 = Stage_3 + Curve_section_length; // mm
    }

    public void Manager() {
        while (true) {
            valuesToBeUpdated();
            
            if (DirectionOfRotation == 0) { // CW Clockwise
                MovementCW(SelectedMover, SelectedMover+1);
            }
            else if (DirectionOfRotation == 1) { // CCW Counterclockwise
                MovementCCW(SelectedMover, SelectedMover+1);
            }
        }
    }

    public void ActualPos(int i) {
        
        if (LinearPos[i] > (Stage_4 - Offset)) {
            ActualPosition = LinearPos[i] - (Stage_4 - Offset);
        }
        else {
            ActualPosition = LinearPos[i] + Offset;
        }
    }

    public void MovementCCW(int lowerLimit, int upperLimit) { // Counterclockwise
        for (int i = lowerLimit; i < upperLimit; i++) {
            
            ActualPos(i);
            
            if (ActualPosition >= 0 && ActualPosition <= Stage_1) { // STAGE_1
                X = Straight_section_length_px - (ActualPosition / (Straight_section_length / Straight_section_length_px));
                Y = -(CURVE_SECTION_RADIUS_PX);
                R = 0;
            }

            else if (ActualPosition > Stage_1 && ActualPosition <= Stage_2) { // STAGE_2
                
                if ((Curve_section_length / 2) > ActualPosition - Stage_1) {
                    cos = Math.Cos((1.57079 * ((Curve_section_length / 2) - (ActualPosition - Stage_1))) / (Curve_section_length / 2));
                    sin = Math.Sin((1.57079 * ((Curve_section_length / 2) - (ActualPosition - Stage_1))) / (Curve_section_length / 2));
                }
                else {
                    cos = Math.Cos((1.57079 * ((ActualPosition - Stage_1) - (Curve_section_length / 2))) / (Curve_section_length / 2));
                    sin = -(Math.Sin((1.57079 * ((ActualPosition - Stage_1) - (Curve_section_length / 2))) / (Curve_section_length / 2)));
                }

                X = -(CURVE_SECTION_RADIUS_PX) * cos;
                Y = CURVE_SECTION_RADIUS_PX * (1-sin) - CURVE_SECTION_RADIUS_PX;
                R = -((ActualPosition - Stage_1) * (180 / Curve_section_length));
            }

            else if (ActualPosition > Stage_2 && ActualPosition <= Stage_3) { // STAGE_3
                X = (ActualPosition - Stage_2) / (Straight_section_length / Straight_section_length_px);
                Y = CURVE_SECTION_RADIUS_PX;
                R = 0;
            }

            else if (ActualPosition > Stage_3 && ActualPosition <= Stage_4) { // STAGE_4
                
                if ((Curve_section_length / 2) > ActualPosition - Stage_3) {
                    cos = Math.Cos((1.57079 * ((Curve_section_length / 2) - (ActualPosition - Stage_3))) / (Curve_section_length / 2));
                    sin = Math.Sin((1.57079 * ((Curve_section_length / 2) - (ActualPosition - Stage_3))) / (Curve_section_length / 2));
                }
                else {
                    cos = Math.Cos((1.57079 * ((ActualPosition - Stage_3) - (Curve_section_length / 2))) / (Curve_section_length / 2));
                    sin = -(Math.Sin((1.57079 * ((ActualPosition - Stage_3) - (Curve_section_length / 2))) / (Curve_section_length / 2)));
                }
                X = Straight_section_length_px + CURVE_SECTION_RADIUS_PX * cos;
                Y = CURVE_SECTION_RADIUS_PX * sin;
                R = -((ActualPosition - Stage_3) * (180 / Curve_section_length));
            }
            
            Owner.Get<Mover>("Movers/Mover_" + i).LeftMargin = (float) X + 45;
            Owner.Get<Mover>("Movers/Mover_" + i).TopMargin = (float) Y + 45;
            Owner.Get<Button>("Movers/Mover_" + i + "/Button").Rotation = (float) R;
            }
    }

    public void MovementCW(int lowerLimit, int upperLimit) { // Clockwise
        for (int i = lowerLimit; i < upperLimit; i++) {
        
            ActualPos(i);
            
            if (ActualPosition >= 0 && ActualPosition <= Stage_1) { // STAGE_1
                
                X = Straight_section_length_px - ((Stage_1 - ActualPosition) / (Straight_section_length / Straight_section_length_px));
                Y = -(CURVE_SECTION_RADIUS_PX);
                R = 0;
            }

            else if (ActualPosition > Stage_1 && ActualPosition <= Stage_2) { // STAGE_2
                
                if ((Curve_section_length / 2) > ActualPosition - Stage_1) {
                    cos = Math.Cos((1.57079 * ((Curve_section_length / 2) - (ActualPosition - Stage_1)) / (Curve_section_length / 2)));
                    sin = Math.Sin((1.57079 * ((Curve_section_length / 2) - (ActualPosition - Stage_1)) / (Curve_section_length / 2)));
                }
                else {
                    cos = Math.Cos((1.57079 * ((ActualPosition - Stage_1) - (Curve_section_length / 2))) / (Curve_section_length / 2));
                    sin = -(Math.Sin((1.57079 * ((ActualPosition - Stage_1) - (Curve_section_length / 2))) / (Curve_section_length / 2)));
                }

                X = CURVE_SECTION_RADIUS_PX * cos + Straight_section_length_px;
                Y = CURVE_SECTION_RADIUS_PX * (1-sin) - CURVE_SECTION_RADIUS_PX;
                R = ((ActualPosition - Stage_1) * (180 / Curve_section_length));
            }

            else if (ActualPosition > Stage_2 && ActualPosition <= Stage_3) { // STAGE_3
                
                X = Straight_section_length_px - ((ActualPosition - Stage_2) / (Straight_section_length / Straight_section_length_px));
                Y = CURVE_SECTION_RADIUS_PX;
                R = 0;
            }

            else if (ActualPosition > Stage_3 && ActualPosition <= Stage_4) { // STAGE_4
                
                if ((Curve_section_length / 2) > ActualPosition - Stage_3) {
                    cos = Math.Cos((1.57079 * ((Curve_section_length / 2) - (ActualPosition - Stage_3)) / (Curve_section_length / 2)));
                    sin = Math.Sin((1.57079 * ((Curve_section_length / 2) - (ActualPosition - Stage_3)) / (Curve_section_length / 2)));
                }
                else {
                    cos = Math.Cos((1.57079 * ((ActualPosition - Stage_3) - (Curve_section_length / 2))) / (Curve_section_length / 2));
                    sin = -(Math.Sin((1.57079 * ((ActualPosition - Stage_3) - (Curve_section_length / 2))) / (Curve_section_length / 2)));
                }

                X = -(CURVE_SECTION_RADIUS_PX) * cos;
                Y = CURVE_SECTION_RADIUS_PX * sin;
                R = ((ActualPosition - Stage_3) * (180 / Curve_section_length));
            }
            
            Owner.Get<Mover>("Movers/Mover_" + i).LeftMargin = (float) X + 45;
            Owner.Get<Mover>("Movers/Mover_" + i).TopMargin = (float) Y + 45;
            Owner.Get<Button>("Movers/Mover_" + i + "/Button").Rotation = (float) R;
            }
    }

    public override void Stop()
    {
        createTask?.Dispose();
        createTask = null;
    }
}
