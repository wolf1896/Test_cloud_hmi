using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "0e2ca34b73ea68a7b12e70b64b1910ab")]
public class OperationDialogContextType : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public FTOptix.CoreBase.MethodInvocation Confirm
    {
        get
        {
            return (FTOptix.CoreBase.MethodInvocation)Refs.GetObject("Confirm");
        }
    }
    public FTOptix.CoreBase.MethodInvocation Cancel
    {
        get
        {
            return (FTOptix.CoreBase.MethodInvocation)Refs.GetObject("Cancel");
        }
    }
    public UAManagedCore.LocalizedText Message
    {
        get
        {
            return (UAManagedCore.LocalizedText)Refs.GetVariable("Message").Value.Value;
        }
        set
        {
            Refs.GetVariable("Message").SetValue(value);
        }
    }
    public IUAVariable MessageVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Message");
        }
    }
#endregion
}
