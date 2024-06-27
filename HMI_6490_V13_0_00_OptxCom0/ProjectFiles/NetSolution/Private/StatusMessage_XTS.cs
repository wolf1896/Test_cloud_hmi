using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "91c4b02e023529c64c365e6a6d772766")]
public class StatusMessage_XTS : FTOptix.CoreBase.ValueMapConverter
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public UAManagedCore.IUAObject Pairs
    {
        get
        {
            return (UAManagedCore.IUAObject)Refs.GetObject("Pairs");
        }
    }
#endregion
}
