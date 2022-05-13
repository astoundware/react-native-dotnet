using Astound.ReactNative.Generators.Models;

namespace Astound.ReactNative.Generators.Utilities;

/// <summary>
/// Provides operations for uniquely-identifying methods.
/// </summary>
public interface IMethodIdentifier
{
    /// <summary>
    /// Generates a <see cref="string"/> that uniquely identifies a specified method.
    /// </summary>
    /// <param name="method">The method to identify.</param>
    /// <returns>A <see cref="string"/> that uniquely identifies the specified method.</returns>
    string Identify(MethodMetadata method);
}
