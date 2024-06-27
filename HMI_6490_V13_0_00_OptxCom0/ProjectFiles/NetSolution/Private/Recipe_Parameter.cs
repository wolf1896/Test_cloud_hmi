using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "656408b85542c9f85dae99d17d3dfc14")]
public class Recipe_Parameter : UAObject
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
    public float Scope
    {
        get
        {
            return (float)Refs.GetVariable("Scope").Value.Value;
        }
        set
        {
            Refs.GetVariable("Scope").SetValue(value);
        }
    }
    public IUAVariable ScopeVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Scope");
        }
    }
    public float MinUser
    {
        get
        {
            return (float)Refs.GetVariable("MinUser").Value.Value;
        }
        set
        {
            Refs.GetVariable("MinUser").SetValue(value);
        }
    }
    public IUAVariable MinUserVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MinUser");
        }
    }
    public float MaxUser
    {
        get
        {
            return (float)Refs.GetVariable("MaxUser").Value.Value;
        }
        set
        {
            Refs.GetVariable("MaxUser").SetValue(value);
        }
    }
    public IUAVariable MaxUserVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MaxUser");
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
    public float CompareValue
    {
        get
        {
            return (float)Refs.GetVariable("CompareValue").Value.Value;
        }
        set
        {
            Refs.GetVariable("CompareValue").SetValue(value);
        }
    }
    public IUAVariable CompareValueVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("CompareValue");
        }
    }
#endregion
}
