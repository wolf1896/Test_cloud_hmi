using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "40c73bf27d7745f55535ff9f7db00c40")]
public class Mix_MechanicalSettings : UAObject
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
    public string Description
    {
        get
        {
            return (string)Refs.GetVariable("Description").Value.Value;
        }
        set
        {
            Refs.GetVariable("Description").SetValue(value);
        }
    }
    public IUAVariable DescriptionVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Description");
        }
    }
    public int Color
    {
        get
        {
            return (int)Refs.GetVariable("Color").Value.Value;
        }
        set
        {
            Refs.GetVariable("Color").SetValue(value);
        }
    }
    public IUAVariable ColorVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Color");
        }
    }
#endregion
}
