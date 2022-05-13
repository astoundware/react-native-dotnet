using System;
using System.Reflection;

namespace Astound.ReactNative.Modules;

/// <summary>
/// Provides operations for registering native modules.
/// </summary>
public interface IReactModuleRegistry
{
    /// <summary>
    /// Registers a specified type as a React module.
    /// </summary>
    /// <param name="type">The type of the module to register.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
    void Register(Type type);

    /// <summary>
    /// Registers all types with <see cref="ReactMethodAttribute"/> defined in a specified assembly as React modules.
    /// </summary>
    /// <param name="assembly">The assembly to search.</param>
    /// <exception cref="ArgumentNullException"><paramref name="assembly"/> is <c>null</c>.</exception>
    void Register(Assembly assembly);
}

