using System;
using System.Collections.Generic;
using Astound.ReactNative.Modules;
using Microsoft.CodeAnalysis;

namespace Astound.ReactNative.Generators.Mappers;

public class ReactModuleAttributeMapper : IMapper<AttributeData, ReactModuleAttribute>
{
    public ReactModuleAttribute Map(AttributeData source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var result = new ReactModuleAttribute();

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
                case nameof(result.RequiresMainQueueSetup):
                    result.RequiresMainQueueSetup = (bool)argument.Value.Value;
                    break;
            }
        }

        return result;
    }
}

