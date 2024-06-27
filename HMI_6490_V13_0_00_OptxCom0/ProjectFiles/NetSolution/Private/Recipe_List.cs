using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "0e33586c9eaeba368638db540bdb7055")]
public class Recipe_List : UAObject
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
    public int Marker
    {
        get
        {
            return (int)Refs.GetVariable("Marker").Value.Value;
        }
        set
        {
            Refs.GetVariable("Marker").SetValue(value);
        }
    }
    public IUAVariable MarkerVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Marker");
        }
    }
#endregion
}
