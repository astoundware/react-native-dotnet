using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Astound.ReactNative.Generators.Tests;

class MockAttributeData : AttributeData
{
    INamedTypeSymbol? _attributeClass;
    IMethodSymbol? _attributeConstructor;
    SyntaxReference? _applicationSyntaxReference;
    ImmutableArray<TypedConstant> _constructorArguments = ImmutableArray.Create<TypedConstant>();
    ImmutableArray<KeyValuePair<string, TypedConstant>> _namedArguments =
        ImmutableArray.Create<KeyValuePair<string, TypedConstant>>();

    public void SetAttributeClass(INamedTypeSymbol? attributeClass)
    {
        _attributeClass = attributeClass;
    }

    public void SetAttributeConstructor(IMethodSymbol? constructor)
    {
        _attributeConstructor = constructor;
    }

    public void SetApplicationSyntaxReference(SyntaxReference? syntaxReference)
    {
        _applicationSyntaxReference = syntaxReference;
    }

    public void SetConstructorArguments(ImmutableArray<TypedConstant> arguments)
    {
        _constructorArguments = arguments;
    }

    public void SetNamedArguments(ImmutableArray<KeyValuePair<string, TypedConstant>> arguments)
    {
        _namedArguments = arguments;
    }

    protected override INamedTypeSymbol? CommonAttributeClass => _attributeClass;

    protected override IMethodSymbol? CommonAttributeConstructor => _attributeConstructor;

    protected override SyntaxReference? CommonApplicationSyntaxReference => _applicationSyntaxReference;

    protected override ImmutableArray<TypedConstant> CommonConstructorArguments => _constructorArguments;

    protected override ImmutableArray<KeyValuePair<string, TypedConstant>> CommonNamedArguments => _namedArguments;
}

