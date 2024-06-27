using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "d0b340f3b4212e4e6bb101dae728c394")]
public class Alarm : FTOptix.Alarm.DigitalAlarm
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public int Array
    {
        get
        {
            return (int)Refs.GetVariable("Array").Value.Value;
        }
        set
        {
            Refs.GetVariable("Array").SetValue(value);
        }
    }
    public IUAVariable ArrayVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Array");
        }
    }
    public int Bit
    {
        get
        {
            return (int)Refs.GetVariable("Bit").Value.Value;
        }
        set
        {
            Refs.GetVariable("Bit").SetValue(value);
        }
    }
    public IUAVariable BitVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Bit");
        }
    }
    public UAManagedCore.LocalizedText Text
    {
        get
        {
            return (UAManagedCore.LocalizedText)Refs.GetVariable("Text").Value.Value;
        }
        set
        {
            Refs.GetVariable("Text").SetValue(value);
        }
    }
    public IUAVariable TextVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Text");
        }
    }
    public int Gravity
    {
        get
        {
            return (int)Refs.GetVariable("Gravity").Value.Value;
        }
        set
        {
            Refs.GetVariable("Gravity").SetValue(value);
        }
    }
    public IUAVariable GravityVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Gravity");
        }
    }
#endregion
}
