using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "HMI_6490_V13_0_00_OptxCom0", Guid = "1a35511ddd43b7d2cc9adbba4080ce46")]
public class FtpFileEntry : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public string FileName
    {
        get
        {
            return (string)Refs.GetVariable("FileName").Value.Value;
        }
        set
        {
            Refs.GetVariable("FileName").SetValue(value);
        }
    }
    public IUAVariable FileNameVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("FileName");
        }
    }
    public float Size
    {
        get
        {
            return (float)Refs.GetVariable("Size").Value.Value;
        }
        set
        {
            Refs.GetVariable("Size").SetValue(value);
        }
    }
    public IUAVariable SizeVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("Size");
        }
    }
    public bool IsDirectory
    {
        get
        {
            return (bool)Refs.GetVariable("IsDirectory").Value.Value;
        }
        set
        {
            Refs.GetVariable("IsDirectory").SetValue(value);
        }
    }
    public IUAVariable IsDirectoryVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("IsDirectory");
        }
    }
#endregion
}
