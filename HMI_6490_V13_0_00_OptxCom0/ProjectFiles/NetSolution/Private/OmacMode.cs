using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "d605a90e184c3b828bc597de1c18239b")]
public class OmacMode : FTOptix.CoreBase.ValueMapConverter
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
