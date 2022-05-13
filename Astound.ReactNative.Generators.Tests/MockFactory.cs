using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Astound.ReactNative.Generators.Tests;

public static class MockFactory
{
    public static TypedConstant CreateTypedConstant(TypedConstantKind kind, object? value)
    {
        var type = typeof(TypedConstant);
        var result = type.Assembly.CreateInstance(
            type.FullName!,
            false,
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new object[] { null!, kind, value! },
            null,
            null);

        return (TypedConstant)result!;
    }

    public static INamedTypeSymbol CreateNamedTypeSymbol()
    {
        var result = Substitute.For<INamedTypeSymbol>();

        result.GetAttributes().Returns(ImmutableArray.Create<AttributeData>());
        result.GetMembers().Returns(ImmutableArray.Create<ISymbol>());

        return result;
    }

    public static INamedTypeSymbol CreateNamedTypeSymbol(string name)
    {
        var result = CreateNamedTypeSymbol();

        result.ToDisplayString().Returns(name);

        return result;
    }

    public static AttributeData CreateAttributeDataWithClass(string className)
    {
        var result = new MockAttributeData();
        var attributeClass = CreateNamedTypeSymbol(className);

        result.SetAttributeClass(attributeClass);

        return result;
    }

    public static AttributeData CreateAttributeDataWithConstructorArgument(object? value)
    {
        var result = new MockAttributeData();
        var constant = CreateTypedConstant(TypedConstantKind.Primitive, value);

        result.SetConstructorArguments(ImmutableArray.Create(constant));

        return result;
    }

    public static AttributeData CreateAttributeDataWithNamedArgument(string key, object? value)
    {
        var result = new MockAttributeData();
        var constant = CreateTypedConstant(TypedConstantKind.Primitive, value);
        var argument = new KeyValuePair<string, TypedConstant>(key, constant);

        result.SetNamedArguments(ImmutableArray.Create(argument));

        return result;
    }

    public static IMethodSymbol CreateMethodSymbol()
    {
        var result = Substitute.For<IMethodSymbol>();

        result.GetAttributes().Returns(ImmutableArray.Create<AttributeData>());
        result.Parameters.Returns(ImmutableArray.Create<IParameterSymbol>());

        return result;
    }

    public static IMethodSymbol CreateMethodSymbol(string name)
    {
        var result = CreateMethodSymbol();

        result.Name.Returns(name);

        return result;
    }

    public static IParameterSymbol CreateParameterSymbol() => Substitute.For<IParameterSymbol>();

    public static IParameterSymbol CreateParameterSymbol(string name)
    {
        var result = CreateParameterSymbol();

        result.Name.Returns(name);

        return result;
    }
}

