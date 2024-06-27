using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "a57274ea24913730d5985d14323dae7e")]
public class XTS_Mover : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public int Number
    {
        get
        {
            return (int)Refs.GetVariable("Number").Value.Value;
        }
        set
        {
            Refs.GetVariable("Number").SetValue(value);
        }
    }
    public IUAVariable NumberVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Number");
        }
    }
    public int X
    {
        get
        {
            return (int)Refs.GetVariable("X").Value.Value;
        }
        set
        {
            Refs.GetVariable("X").SetValue(value);
        }
    }
    public IUAVariable XVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("X");
        }
    }
    public int Y
    {
        get
        {
            return (int)Refs.GetVariable("Y").Value.Value;
        }
        set
        {
            Refs.GetVariable("Y").SetValue(value);
        }
    }
    public IUAVariable YVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Y");
        }
    }
    public int R
    {
        get
        {
            return (int)Refs.GetVariable("R").Value.Value;
        }
        set
        {
            Refs.GetVariable("R").SetValue(value);
        }
    }
    public IUAVariable RVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("R");
        }
    }
    public int FaultCode
    {
        get
        {
            return (int)Refs.GetVariable("FaultCode").Value.Value;
        }
        set
        {
            Refs.GetVariable("FaultCode").SetValue(value);
        }
    }
    public IUAVariable FaultCodeVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("FaultCode");
        }
    }
    public int LinearPos
    {
        get
        {
            return (int)Refs.GetVariable("LinearPos").Value.Value;
        }
        set
        {
            Refs.GetVariable("LinearPos").SetValue(value);
        }
    }
    public IUAVariable LinearPosVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("LinearPos");
        }
    }
#endregion
}
