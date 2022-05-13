using System;

namespace Astound.ReactNative.Modules;

[AttributeUsage(AttributeTargets.Method)]
public class ReactMethodAttribute : Attribute
{
    public string Name { get; set; }

    public ReactMethodAttribute()
    {

    }

    public ReactMethodAttribute(string name)
    {
        Name = name;
    }
}

