using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "b407cc6041761c9d7a6c6488e1b39056")]
public class Mix_IOData : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public bool Status
    {
        get
        {
            return (bool)Refs.GetVariable("Status").Value.Value;
        }
        set
        {
            Refs.GetVariable("Status").SetValue(value);
        }
    }
    public IUAVariable StatusVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Status");
        }
    }
    public string IO_Name
    {
        get
        {
            return (string)Refs.GetVariable("IO_Name").Value.Value;
        }
        set
        {
            Refs.GetVariable("IO_Name").SetValue(value);
        }
    }
    public IUAVariable IO_NameVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("IO_Name");
        }
    }
    public int ShieldNumber
    {
        get
        {
            return (int)Refs.GetVariable("ShieldNumber").Value.Value;
        }
        set
        {
            Refs.GetVariable("ShieldNumber").SetValue(value);
        }
    }
    public IUAVariable ShieldNumberVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("ShieldNumber");
        }
    }
    public int index
    {
        get
        {
            return (int)Refs.GetVariable("index").Value.Value;
        }
        set
        {
            Refs.GetVariable("index").SetValue(value);
        }
    }
    public IUAVariable indexVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("index");
        }
    }
    public bool Force
    {
        get
        {
            return (bool)Refs.GetVariable("Force").Value.Value;
        }
        set
        {
            Refs.GetVariable("Force").SetValue(value);
        }
    }
    public IUAVariable ForceVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Force");
        }
    }
    public bool OutputType
    {
        get
        {
            return (bool)Refs.GetVariable("OutputType").Value.Value;
        }
        set
        {
            Refs.GetVariable("OutputType").SetValue(value);
        }
    }
    public IUAVariable OutputTypeVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("OutputType");
        }
    }
    public string ShieldName
    {
        get
        {
            return (string)Refs.GetVariable("ShieldName").Value.Value;
        }
        set
        {
            Refs.GetVariable("ShieldName").SetValue(value);
        }
    }
    public IUAVariable ShieldNameVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("ShieldName");
        }
    }
#endregion
}
