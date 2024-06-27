using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "678826c20a92ed219dcad28a8e2143da")]
public class XTS_System : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public int State
    {
        get
        {
            return (int)Refs.GetVariable("State").Value.Value;
        }
        set
        {
            Refs.GetVariable("State").SetValue(value);
        }
    }
    public IUAVariable StateVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("State");
        }
    }
    public int TotalPath
    {
        get
        {
            return (int)Refs.GetVariable("TotalPath").Value.Value;
        }
        set
        {
            Refs.GetVariable("TotalPath").SetValue(value);
        }
    }
    public IUAVariable TotalPathVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("TotalPath");
        }
    }
    public int DCLinkVoltage
    {
        get
        {
            return (int)Refs.GetVariable("DCLinkVoltage").Value.Value;
        }
        set
        {
            Refs.GetVariable("DCLinkVoltage").SetValue(value);
        }
    }
    public IUAVariable DCLinkVoltageVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("DCLinkVoltage");
        }
    }
    public int AuxiliaryVoltage
    {
        get
        {
            return (int)Refs.GetVariable("AuxiliaryVoltage").Value.Value;
        }
        set
        {
            Refs.GetVariable("AuxiliaryVoltage").SetValue(value);
        }
    }
    public IUAVariable AuxiliaryVoltageVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("AuxiliaryVoltage");
        }
    }
    public int MoverInErrorID
    {
        get
        {
            return (int)Refs.GetVariable("MoverInErrorID").Value.Value;
        }
        set
        {
            Refs.GetVariable("MoverInErrorID").SetValue(value);
        }
    }
    public IUAVariable MoverInErrorIDVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MoverInErrorID");
        }
    }
    public int MoverInErrorCode
    {
        get
        {
            return (int)Refs.GetVariable("MoverInErrorCode").Value.Value;
        }
        set
        {
            Refs.GetVariable("MoverInErrorCode").SetValue(value);
        }
    }
    public IUAVariable MoverInErrorCodeVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MoverInErrorCode");
        }
    }
    public bool HomeDone
    {
        get
        {
            return (bool)Refs.GetVariable("HomeDone").Value.Value;
        }
        set
        {
            Refs.GetVariable("HomeDone").SetValue(value);
        }
    }
    public IUAVariable HomeDoneVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("HomeDone");
        }
    }
    public bool RunConsent
    {
        get
        {
            return (bool)Refs.GetVariable("RunConsent").Value.Value;
        }
        set
        {
            Refs.GetVariable("RunConsent").SetValue(value);
        }
    }
    public IUAVariable RunConsentVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("RunConsent");
        }
    }
    public bool RecoveryInProgress
    {
        get
        {
            return (bool)Refs.GetVariable("RecoveryInProgress").Value.Value;
        }
        set
        {
            Refs.GetVariable("RecoveryInProgress").SetValue(value);
        }
    }
    public IUAVariable RecoveryInProgressVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("RecoveryInProgress");
        }
    }
    public bool CounterKmReset
    {
        get
        {
            return (bool)Refs.GetVariable("CounterKmReset").Value.Value;
        }
        set
        {
            Refs.GetVariable("CounterKmReset").SetValue(value);
        }
    }
    public IUAVariable CounterKmResetVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("CounterKmReset");
        }
    }
    public int MoverNumber
    {
        get
        {
            return (int)Refs.GetVariable("MoverNumber").Value.Value;
        }
        set
        {
            Refs.GetVariable("MoverNumber").SetValue(value);
        }
    }
    public IUAVariable MoverNumberVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MoverNumber");
        }
    }
    public bool MinLoad
    {
        get
        {
            return (bool)Refs.GetVariable("MinLoad").Value.Value;
        }
        set
        {
            Refs.GetVariable("MinLoad").SetValue(value);
        }
    }
    public IUAVariable MinLoadVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MinLoad");
        }
    }
    public bool MedLoad
    {
        get
        {
            return (bool)Refs.GetVariable("MedLoad").Value.Value;
        }
        set
        {
            Refs.GetVariable("MedLoad").SetValue(value);
        }
    }
    public IUAVariable MedLoadVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MedLoad");
        }
    }
    public bool MaxLoad
    {
        get
        {
            return (bool)Refs.GetVariable("MaxLoad").Value.Value;
        }
        set
        {
            Refs.GetVariable("MaxLoad").SetValue(value);
        }
    }
    public IUAVariable MaxLoadVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MaxLoad");
        }
    }
    public bool CounterKmExpired
    {
        get
        {
            return (bool)Refs.GetVariable("CounterKmExpired").Value.Value;
        }
        set
        {
            Refs.GetVariable("CounterKmExpired").SetValue(value);
        }
    }
    public IUAVariable CounterKmExpiredVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("CounterKmExpired");
        }
    }
#endregion
}
