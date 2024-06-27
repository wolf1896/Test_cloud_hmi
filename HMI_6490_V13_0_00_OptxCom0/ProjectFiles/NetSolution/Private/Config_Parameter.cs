using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "f3513c609a1e8ea1ea92c0c7c9755e60")]
public class Config_Parameter : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public float Actual
    {
        get
        {
            return (float)Refs.GetVariable("Actual").Value.Value;
        }
        set
        {
            Refs.GetVariable("Actual").SetValue(value);
        }
    }
    public IUAVariable ActualVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Actual");
        }
    }
    public float Old
    {
        get
        {
            return (float)Refs.GetVariable("Old").Value.Value;
        }
        set
        {
            Refs.GetVariable("Old").SetValue(value);
        }
    }
    public IUAVariable OldVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Old");
        }
    }
    public int ParNum
    {
        get
        {
            return (int)Refs.GetVariable("ParNum").Value.Value;
        }
        set
        {
            Refs.GetVariable("ParNum").SetValue(value);
        }
    }
    public IUAVariable ParNumVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("ParNum");
        }
    }
    public float MinSystem
    {
        get
        {
            return (float)Refs.GetVariable("MinSystem").Value.Value;
        }
        set
        {
            Refs.GetVariable("MinSystem").SetValue(value);
        }
    }
    public IUAVariable MinSystemVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MinSystem");
        }
    }
    public float MaxSystem
    {
        get
        {
            return (float)Refs.GetVariable("MaxSystem").Value.Value;
        }
        set
        {
            Refs.GetVariable("MaxSystem").SetValue(value);
        }
    }
    public IUAVariable MaxSystemVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MaxSystem");
        }
    }
#endregion
}
