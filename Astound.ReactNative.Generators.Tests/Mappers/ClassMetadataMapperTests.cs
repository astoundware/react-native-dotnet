using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Astound.ReactNative.Generators.Mappers;
using Astound.ReactNative.Generators.Models;
using Astound.ReactNative.Modules;
using Microsoft.CodeAnalysis;

namespace Astound.ReactNative.Generators.Tests.Mappers;

[TestClass]
public class ClassMetadataMapperTests
{
    readonly Fixture _fixture = new();
    readonly IMapper<AttributeData, ReactModuleAttribute> _attributeMapper =
        Substitute.For<IMapper<AttributeData, ReactModuleAttribute>>();
    readonly IMapper<IMethodSymbol, MethodMetadata> _methodMapper =
        Substitute.For<IMapper<IMethodSymbol, MethodMetadata>>();
    readonly ClassMetadataMapper _mapper;

    public ClassMetadataMapperTests()
    {
        _mapper = new ClassMetadataMapper(_attributeMapper, _methodMapper);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestConstructorThrowsOnNullAttributeMapper()
    {
        new ClassMetadataMapper(null, _methodMapper);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestConstructorThrowsOnNullMethodMapper()
    {
        new ClassMetadataMapper(_attributeMapper, null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestMapThrowsOnNullSource()
    {
        _mapper.Map(null);
    }

    [TestMethod]
    public void TestMapReturnsNullWhenModuleAttributeIsNotFound()
    {
        var source = Substitute.For<INamedTypeSymbol>();
        var attribute = MockFactory.CreateAttributeDataWithClass(_fixture.Create<string>());

        source.GetAttributes().Returns(ImmutableArray.Create(attribute));

        var actual = _mapper.Map(source);

        actual.Should().BeNull();
    }

    [TestMethod]
    public void TestMapAssignsName()
    {
        var source = CreateModuleSymbol();
        var expected = _fixture.Create<string>();

        source.Name.Returns(expected);

        var metadata = _mapper.Map(source);

        Assert.AreEqual(expected, metadata?.Name);
    }

    [TestMethod]
    public void TestMapAssignsNamespace()
    {
        var source = CreateModuleSymbol();
        var expected = _fixture.Create<string>();
        var namespaceSymbol = Substitute.For<INamespaceSymbol>();

        namespaceSymbol.ToDisplayString().Returns(expected);
        source.ContainingNamespace.Returns(namespaceSymbol);

        var metadata = _mapper.Map(source);

        Assert.AreEqual(expected, metadata?.Namespace);
    }

    [TestMethod]
    public void TestMapAssignsAttribute()
    {
        var source = MockFactory.CreateNamedTypeSymbol();
        var knownAttribute = CreateModuleAttribute();
        var unknownAttribute = MockFactory.CreateAttributeDataWithClass(_fixture.Create<string>());
        var expected = _fixture.Create<ReactModuleAttribute>();

        source.GetAttributes().Returns(ImmutableArray.Create(knownAttribute, unknownAttribute));
        _attributeMapper.Map(knownAttribute).Returns(expected);

        var metadata = _mapper.Map(source);

        Assert.AreEqual(expected, metadata?.Attribute);
    }

    [TestMethod]
    public void TestMapAssignsMethods()
    {
        var source = CreateModuleSymbol();
        var methods = _fixture.Create<IList<MethodMetadata>>();
        var members = new List<ISymbol>();
        var expected = new List<MethodMetadata>();

        for (var i = 0; i < methods.Count; i++)
        {
            MethodMetadata method = methods[i];
            var knownSymbol = MockFactory.CreateMethodSymbol(_fixture.Create<string>());
            var unknownSymbol = Substitute.For<ISymbol>();

            members.Add(knownSymbol);
            // include some symbols that aren't methods
            members.Add(unknownSymbol);

            if (i % 2 > 0)
            {
                _methodMapper.Map(knownSymbol).Returns(method);
                expected.Add(method);
            }
            else
            {
                // include some methods that can't be mapped
                _methodMapper.Map(knownSymbol).Returns(default(MethodMetadata));
            }
        }

        var immutableMembers = members.ToImmutableArray();

        source.GetMembers().Returns(immutableMembers);

        var metadata = _mapper.Map(source);

        metadata?.Methods.Should().BeEquivalentTo(expected);
    }

    INamedTypeSymbol CreateModuleSymbol()
    {
        var result = MockFactory.CreateNamedTypeSymbol();
        var attribute = CreateModuleAttribute();

        result.GetAttributes().Returns(ImmutableArray.Create(attribute));

        return result;
    }

    AttributeData CreateModuleAttribute() =>
        MockFactory.CreateAttributeDataWithClass(typeof(ReactModuleAttribute).FullName!);
}

