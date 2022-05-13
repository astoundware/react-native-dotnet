using System;

namespace Astound.ReactNative.Modules;

/// <summary>
/// An attribute that marks whether a method should be exported to JavaScript.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ReactMethodAttribute : Attribute
{
    /// <summary>
    /// The name to use when calling the method in JavaScript.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Creates a new <see cref="ReactMethodAttribute"/>.
    /// </summary>
    public ReactMethodAttribute()
    {

    }

    /// <summary>
    /// Creates a new <see cref="ReactMethodAttribute"/> with a specified name.
    /// </summary>
    /// <param name="name">The name to use when calling the method in JavaScript.</param>
    public ReactMethodAttribute(string name)
    {
        Name = name;
    }
}

