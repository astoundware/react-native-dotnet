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

        int hashCode = GetDeterministicHashCode(method.ToString());

        return hashCode.ToString("X");
    }

    // Adapted from https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/
    static int GetDeterministicHashCode(string str)
    {
        unchecked
        {
            int hash1 = (5381 << 16) + 5381;
            int hash2 = hash1;

            for (int i = 0; i < str.Length; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1)
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
    }
}
