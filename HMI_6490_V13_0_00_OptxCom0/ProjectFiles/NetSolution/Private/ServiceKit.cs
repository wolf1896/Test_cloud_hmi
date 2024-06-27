using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "f15b59fbbed1238fc50b0efff276ed12")]
public class ServiceKit : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public bool Cmd_Enable
    {
        get
        {
            return (bool)Refs.GetVariable("Cmd_Enable").Value.Value;
        }
        set
        {
            Refs.GetVariable("Cmd_Enable").SetValue(value);
        }
    }
    public IUAVariable Cmd_EnableVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Cmd_Enable");
        }
    }
    public bool Cmd_Reset
    {
        get
        {
            return (bool)Refs.GetVariable("Cmd_Reset").Value.Value;
        }
        set
        {
            Refs.GetVariable("Cmd_Reset").SetValue(value);
        }
    }
    public IUAVariable Cmd_ResetVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Cmd_Reset");
        }
    }
    public bool Sts_TimeExpired
    {
        get
        {
            return (bool)Refs.GetVariable("Sts_TimeExpired").Value.Value;
        }
        set
        {
            Refs.GetVariable("Sts_TimeExpired").SetValue(value);
        }
    }
    public IUAVariable Sts_TimeExpiredVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Sts_TimeExpired");
        }
    }
    public float Par_WorkedHoursCheck
    {
        get
        {
            return (float)Refs.GetVariable("Par_WorkedHoursCheck").Value.Value;
        }
        set
        {
            Refs.GetVariable("Par_WorkedHoursCheck").SetValue(value);
        }
    }
    public IUAVariable Par_WorkedHoursCheckVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Par_WorkedHoursCheck");
        }
    }
    public float Sts_MillionsCyclesPerformed
    {
        get
        {
            return (float)Refs.GetVariable("Sts_MillionsCyclesPerformed").Value.Value;
        }
        set
        {
            Refs.GetVariable("Sts_MillionsCyclesPerformed").SetValue(value);
        }
    }
    public IUAVariable Sts_MillionsCyclesPerformedVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Sts_MillionsCyclesPerformed");
        }
    }
    public int Sts_WorkTimeSec
    {
        get
        {
            return (int)Refs.GetVariable("Sts_WorkTimeSec").Value.Value;
        }
        set
        {
            Refs.GetVariable("Sts_WorkTimeSec").SetValue(value);
        }
    }
    public IUAVariable Sts_WorkTimeSecVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Sts_WorkTimeSec");
        }
    }
    public int Sts_GlobalWorkTime
    {
        get
        {
            return (int)Refs.GetVariable("Sts_GlobalWorkTime").Value.Value;
        }
        set
        {
            Refs.GetVariable("Sts_GlobalWorkTime").SetValue(value);
        }
    }
    public IUAVariable Sts_GlobalWorkTimeVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Sts_GlobalWorkTime");
        }
    }
    public int Sts_WorkTimeHour
    {
        get
        {
            return (int)Refs.GetVariable("Sts_WorkTimeHour").Value.Value;
        }
        set
        {
            Refs.GetVariable("Sts_WorkTimeHour").SetValue(value);
        }
    }
    public IUAVariable Sts_WorkTimeHourVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Sts_WorkTimeHour");
        }
    }
    public string KitName
    {
        get
        {
            return (string)Refs.GetVariable("KitName").Value.Value;
        }
        set
        {
            Refs.GetVariable("KitName").SetValue(value);
        }
    }
    public IUAVariable KitNameVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("KitName");
        }
    }
    public int Index
    {
        get
        {
            return (int)Refs.GetVariable("Index").Value.Value;
        }
        set
        {
            Refs.GetVariable("Index").SetValue(value);
        }
    }
    public IUAVariable IndexVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Index");
        }
    }
#endregion
}
