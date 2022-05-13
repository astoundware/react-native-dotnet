using System;
using System.Collections.Generic;
using Astound.ReactNative.Modules;
using Microsoft.CodeAnalysis;

namespace Astound.ReactNative.Generators.Mappers;

public class ReactMethodAttributeMapper : IMapper<AttributeData, ReactMethodAttribute>
{
    public ReactMethodAttribute Map(AttributeData source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var result = new ReactMethodAttribute();

        if (source.ConstructorArguments.Length > 0)
        {
            result.Name = (string)source.ConstructorArguments[0].Value;
        }

        foreach (KeyValuePair<string, TypedConstant> argument in source.NamedArguments)
        {
            switch (argument.Key)
            {
                case nameof(result.Name):
                    result.Name = (string)argument.Value.Value;
                    break;
            }
        }

        return result;
    }
}

