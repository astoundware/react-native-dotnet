using System;

namespace Astound.ReactNative.Modules;

[AttributeUsage(AttributeTargets.Class)]
public class ReactModuleAttribute : Attribute
{
    public string Name { get; set; }
    public bool RequiresMainQueueSetup { get; set; }

    public ReactModuleAttribute()
    {

    }

    public ReactModuleAttribute(string name)
    {
        Name = name;
    }
}

