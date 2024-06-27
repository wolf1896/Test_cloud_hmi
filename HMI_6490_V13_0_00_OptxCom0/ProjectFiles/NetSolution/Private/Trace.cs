using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "3025d8629f19af61c96bd399054c9c4a")]
public class Trace : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public int[] Value
    {
        get
        {
            return (int[])Refs.GetVariable("Value").Value.Value;
        }
        set
        {
            Refs.GetVariable("Value").SetValue(value);
        }
    }
    public IUAVariable ValueVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Value");
        }
    }
    public int MaxValue
    {
        get
        {
            return (int)Refs.GetVariable("MaxValue").Value.Value;
        }
        set
        {
            Refs.GetVariable("MaxValue").SetValue(value);
        }
    }
    public IUAVariable MaxValueVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MaxValue");
        }
    }
    public int MinValue
    {
        get
        {
            return (int)Refs.GetVariable("MinValue").Value.Value;
        }
        set
        {
            Refs.GetVariable("MinValue").SetValue(value);
        }
    }
    public IUAVariable MinValueVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MinValue");
        }
    }
    public string Units
    {
        get
        {
            return (string)Refs.GetVariable("Units").Value.Value;
        }
        set
        {
            Refs.GetVariable("Units").SetValue(value);
        }
    }
    public IUAVariable UnitsVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Units");
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
