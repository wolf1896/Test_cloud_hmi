using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "7d12cda35979ccceab418f5fdda5b469")]
public class Temperature_Parameter : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public float SetPoint
    {
        get
        {
            return (float)Refs.GetVariable("SetPoint").Value.Value;
        }
        set
        {
            Refs.GetVariable("SetPoint").SetValue(value);
        }
    }
    public IUAVariable SetPointVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("SetPoint");
        }
    }
    public float ActualTemp
    {
        get
        {
            return (float)Refs.GetVariable("ActualTemp").Value.Value;
        }
        set
        {
            Refs.GetVariable("ActualTemp").SetValue(value);
        }
    }
    public IUAVariable ActualTempVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("ActualTemp");
        }
    }
    public float AlarmThreshold
    {
        get
        {
            return (float)Refs.GetVariable("AlarmThreshold").Value.Value;
        }
        set
        {
            Refs.GetVariable("AlarmThreshold").SetValue(value);
        }
    }
    public IUAVariable AlarmThresholdVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("AlarmThreshold");
        }
    }
    public float StopThreshold
    {
        get
        {
            return (float)Refs.GetVariable("StopThreshold").Value.Value;
        }
        set
        {
            Refs.GetVariable("StopThreshold").SetValue(value);
        }
    }
    public IUAVariable StopThresholdVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("StopThreshold");
        }
    }
    public float Hysteresys
    {
        get
        {
            return (float)Refs.GetVariable("Hysteresys").Value.Value;
        }
        set
        {
            Refs.GetVariable("Hysteresys").SetValue(value);
        }
    }
    public IUAVariable HysteresysVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Hysteresys");
        }
    }
    public float LowCurrent
    {
        get
        {
            return (float)Refs.GetVariable("LowCurrent").Value.Value;
        }
        set
        {
            Refs.GetVariable("LowCurrent").SetValue(value);
        }
    }
    public IUAVariable LowCurrentVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("LowCurrent");
        }
    }
    public float HighCurrent
    {
        get
        {
            return (float)Refs.GetVariable("HighCurrent").Value.Value;
        }
        set
        {
            Refs.GetVariable("HighCurrent").SetValue(value);
        }
    }
    public IUAVariable HighCurrentVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("HighCurrent");
        }
    }
    public bool Visible
    {
        get
        {
            return (bool)Refs.GetVariable("Visible").Value.Value;
        }
        set
        {
            Refs.GetVariable("Visible").SetValue(value);
        }
    }
    public IUAVariable VisibleVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Visible");
        }
    }
    public int ZoneNumber
    {
        get
        {
            return (int)Refs.GetVariable("ZoneNumber").Value.Value;
        }
        set
        {
            Refs.GetVariable("ZoneNumber").SetValue(value);
        }
    }
    public IUAVariable ZoneNumberVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("ZoneNumber");
        }
    }
    public float SelftuneCmd
    {
        get
        {
            return (float)Refs.GetVariable("SelftuneCmd").Value.Value;
        }
        set
        {
            Refs.GetVariable("SelftuneCmd").SetValue(value);
        }
    }
    public IUAVariable SelftuneCmdVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("SelftuneCmd");
        }
    }
    public float Offset
    {
        get
        {
            return (float)Refs.GetVariable("Offset").Value.Value;
        }
        set
        {
            Refs.GetVariable("Offset").SetValue(value);
        }
    }
    public IUAVariable OffsetVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Offset");
        }
    }
    public float ProportionalBand
    {
        get
        {
            return (float)Refs.GetVariable("ProportionalBand").Value.Value;
        }
        set
        {
            Refs.GetVariable("ProportionalBand").SetValue(value);
        }
    }
    public IUAVariable ProportionalBandVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("ProportionalBand");
        }
    }
    public float IntegralTime
    {
        get
        {
            return (float)Refs.GetVariable("IntegralTime").Value.Value;
        }
        set
        {
            Refs.GetVariable("IntegralTime").SetValue(value);
        }
    }
    public IUAVariable IntegralTimeVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("IntegralTime");
        }
    }
    public float DerivativeTime
    {
        get
        {
            return (float)Refs.GetVariable("DerivativeTime").Value.Value;
        }
        set
        {
            Refs.GetVariable("DerivativeTime").SetValue(value);
        }
    }
    public IUAVariable DerivativeTimeVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("DerivativeTime");
        }
    }
    public float MaxTemperatureLimit
    {
        get
        {
            return (float)Refs.GetVariable("MaxTemperatureLimit").Value.Value;
        }
        set
        {
            Refs.GetVariable("MaxTemperatureLimit").SetValue(value);
        }
    }
    public IUAVariable MaxTemperatureLimitVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("MaxTemperatureLimit");
        }
    }
    public float OutputPower
    {
        get
        {
            return (float)Refs.GetVariable("OutputPower").Value.Value;
        }
        set
        {
            Refs.GetVariable("OutputPower").SetValue(value);
        }
    }
    public IUAVariable OutputPowerVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("OutputPower");
        }
    }
#endregion
}
