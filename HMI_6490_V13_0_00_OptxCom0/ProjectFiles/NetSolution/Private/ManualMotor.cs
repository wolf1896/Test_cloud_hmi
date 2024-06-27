using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "6cd7e44796f77cd51c251bb69d61998b")]
public class ManualMotor : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public int AxisNumber
    {
        get
        {
            return (int)Refs.GetVariable("AxisNumber").Value.Value;
        }
        set
        {
            Refs.GetVariable("AxisNumber").SetValue(value);
        }
    }
    public IUAVariable AxisNumberVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("AxisNumber");
        }
    }
    public float Sts_Position
    {
        get
        {
            return (float)Refs.GetVariable("Sts_Position").Value.Value;
        }
        set
        {
            Refs.GetVariable("Sts_Position").SetValue(value);
        }
    }
    public IUAVariable Sts_PositionVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Sts_Position");
        }
    }
    public sbyte Cmd_ManualSpeed
    {
        get
        {
            return (sbyte)Refs.GetVariable("Cmd_ManualSpeed").Value.Value;
        }
        set
        {
            Refs.GetVariable("Cmd_ManualSpeed").SetValue(value);
        }
    }
    public IUAVariable Cmd_ManualSpeedVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Cmd_ManualSpeed");
        }
    }
    public bool Cmd_ManualEnable
    {
        get
        {
            return (bool)Refs.GetVariable("Cmd_ManualEnable").Value.Value;
        }
        set
        {
            Refs.GetVariable("Cmd_ManualEnable").SetValue(value);
        }
    }
    public IUAVariable Cmd_ManualEnableVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Cmd_ManualEnable");
        }
    }
    public bool Cmd_PositiveJog
    {
        get
        {
            return (bool)Refs.GetVariable("Cmd_PositiveJog").Value.Value;
        }
        set
        {
            Refs.GetVariable("Cmd_PositiveJog").SetValue(value);
        }
    }
    public IUAVariable Cmd_PositiveJogVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Cmd_PositiveJog");
        }
    }
    public bool Cmd_NegativeJog
    {
        get
        {
            return (bool)Refs.GetVariable("Cmd_NegativeJog").Value.Value;
        }
        set
        {
            Refs.GetVariable("Cmd_NegativeJog").SetValue(value);
        }
    }
    public IUAVariable Cmd_NegativeJogVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Cmd_NegativeJog");
        }
    }
    public bool Sts_BrakeVisible
    {
        get
        {
            return (bool)Refs.GetVariable("Sts_BrakeVisible").Value.Value;
        }
        set
        {
            Refs.GetVariable("Sts_BrakeVisible").SetValue(value);
        }
    }
    public IUAVariable Sts_BrakeVisibleVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Sts_BrakeVisible");
        }
    }
    public bool Cmd_BrakeUnlock
    {
        get
        {
            return (bool)Refs.GetVariable("Cmd_BrakeUnlock").Value.Value;
        }
        set
        {
            Refs.GetVariable("Cmd_BrakeUnlock").SetValue(value);
        }
    }
    public IUAVariable Cmd_BrakeUnlockVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Cmd_BrakeUnlock");
        }
    }
    public bool Sts_AbsHomeVisible
    {
        get
        {
            return (bool)Refs.GetVariable("Sts_AbsHomeVisible").Value.Value;
        }
        set
        {
            Refs.GetVariable("Sts_AbsHomeVisible").SetValue(value);
        }
    }
    public IUAVariable Sts_AbsHomeVisibleVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Sts_AbsHomeVisible");
        }
    }
    public bool Cmd_AbsHomeStart
    {
        get
        {
            return (bool)Refs.GetVariable("Cmd_AbsHomeStart").Value.Value;
        }
        set
        {
            Refs.GetVariable("Cmd_AbsHomeStart").SetValue(value);
        }
    }
    public IUAVariable Cmd_AbsHomeStartVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Cmd_AbsHomeStart");
        }
    }
    public string Name
    {
        get
        {
            return (string)Refs.GetVariable("Name").Value.Value;
        }
        set
        {
            Refs.GetVariable("Name").SetValue(value);
        }
    }
    public IUAVariable NameVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Name");
        }
    }
    public float MaxSpeed
    {
        get
        {
            return (float)Refs.GetVariable("MaxSpeed").Value.Value;
        }
        set
        {
            Refs.GetVariable("MaxSpeed").SetValue(value);
        }
    }
    public IUAVariable MaxSpeedVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MaxSpeed");
        }
    }
    public float MinSpeed
    {
        get
        {
            return (float)Refs.GetVariable("MinSpeed").Value.Value;
        }
        set
        {
            Refs.GetVariable("MinSpeed").SetValue(value);
        }
    }
    public IUAVariable MinSpeedVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MinSpeed");
        }
    }
    public bool Sts_AbsHomeDone
    {
        get
        {
            return (bool)Refs.GetVariable("Sts_AbsHomeDone").Value.Value;
        }
        set
        {
            Refs.GetVariable("Sts_AbsHomeDone").SetValue(value);
        }
    }
    public IUAVariable Sts_AbsHomeDoneVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Sts_AbsHomeDone");
        }
    }
#endregion
}
