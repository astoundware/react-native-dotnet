using System;

namespace Astound.ReactNative.Modules;

/// <summary>
/// An attribute that marks whether a class should be exported as a module to JavaScript.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ReactModuleAttribute : Attribute
{
    /// <summary>
    /// The name to use when referencing the module in JavaScript.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Indicates whether the module requires main-queue setup.
    /// </summary>
    public bool RequiresMainQueueSetup { get; set; }

    /// <summary>
    /// Creates a new <see cref="ReactModuleAttribute"/>.
    /// </summary>
    public ReactModuleAttribute()
    {

    }

    /// <summary>
    /// Creates a new <see cref="ReactModuleAttribute"/> with a specified name.
    /// </summary>
    /// <param name="name">The name to use when referencing the module in JavaScript.</param>
    public ReactModuleAttribute(string name)
    {
        Name = name;
    }
}

