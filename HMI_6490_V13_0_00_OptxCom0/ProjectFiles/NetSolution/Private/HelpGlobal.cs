using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "6b3702e85b4e4b439fa3607ff2ac9e20")]
public class HelpGlobal : FTOptix.CoreBase.ValueMapConverter
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
