using System;
using Astound.ReactNative.Generators.Models;

namespace Astound.ReactNative.Generators.Utilities;

public class MethodIdentifier : IMethodIdentifier
{
    public string Identify(MethodMetadata method)
    {
        if (method == null)
        {
            throw new ArgumentNullException(nameof(method));
        }

        int hashCode = method.ToString().GetHashCode();
        byte[] hash = BitConverter.GetBytes(hashCode);
        string base64 = Convert.ToBase64String(hash);

        return base64.Trim('/', '+', '=');
    }
}
