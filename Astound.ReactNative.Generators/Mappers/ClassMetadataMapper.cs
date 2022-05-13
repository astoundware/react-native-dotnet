using System;
using System.Linq;
using Astound.ReactNative.Modules;
using Astound.ReactNative.Generators.Models;
using Microsoft.CodeAnalysis;

namespace Astound.ReactNative.Generators.Mappers;

public class ClassMetadataMapper : IMapper<INamedTypeSymbol, ClassMetadata>
{
    readonly IMapper<AttributeData, ReactModuleAttribute> _attributeMapper;
    readonly IMapper<IMethodSymbol, MethodMetadata> _methodMapper;

    public ClassMetadataMapper(
        IMapper<AttributeData, ReactModuleAttribute> attributeMapper,
        IMapper<IMethodSymbol, MethodMetadata> methodMapper)
    {
        _attributeMapper = attributeMapper ?? throw new ArgumentNullException(nameof(attributeMapper));
        _methodMapper = methodMapper ?? throw new ArgumentNullException(nameof(methodMapper));
    }

    public ClassMetadata Map(INamedTypeSymbol source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        AttributeData attributeData = source.GetAttributes().FirstOrDefault(a =>
            a.AttributeClass.ToDisplayString() == typeof(ReactModuleAttribute).FullName);

        if (attributeData == null)
        {
            return null;
        }

        return new ClassMetadata()
        {
            Name = source.Name,
            Namespace = source.ContainingNamespace?.ToDisplayString(),
            Attribute = _attributeMapper.Map(attributeData),
            Methods = source.GetMembers()
                .OfType<IMethodSymbol>().Select(m => _methodMapper.Map(m))
                .Where(m => m != null)
                .ToList()
        };
    }
}

