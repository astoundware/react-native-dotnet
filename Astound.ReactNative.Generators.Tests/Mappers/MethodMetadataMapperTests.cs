using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Astound.ReactNative.Generators.Mappers;
using Astound.ReactNative.Generators.Models;
using Astound.ReactNative.Generators.Tests;
using Astound.ReactNative.Modules;
using Microsoft.CodeAnalysis;

namespace Astound.ReactNative.macOS.Generators.Tests.Mappers;

[TestClass]
public class MethodMetadataMapperTests
{
	readonly Fixture _fixture = new();
	readonly IMapper<AttributeData, ReactMethodAttribute> _attributeMapper =
		Substitute.For<IMapper<AttributeData, ReactMethodAttribute>>();
	readonly IMapper<IParameterSymbol, ParameterMetadata> _parameterMapper =
		Substitute.For<IMapper<IParameterSymbol, ParameterMetadata>>();
	readonly MethodMetadataMapper _mapper;

	public MethodMetadataMapperTests()
	{
		_mapper = new MethodMetadataMapper(_attributeMapper, _parameterMapper);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentNullException))]
	public void TestConstructorThrowsOnNullAttributeMapper()
    {
		new MethodMetadataMapper(null, _parameterMapper);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentNullException))]
	public void TestConstructorThrowsOnNullParameterMapper()
	{
		new MethodMetadataMapper(_attributeMapper, null);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentNullException))]
	public void TestMapThrowsOnNullSource()
	{
		_mapper.Map(null!);
	}

	[TestMethod]
	public void TestMapReturnsNullWhenAttributeIsNotFound()
	{
		var source = Substitute.For<IMethodSymbol>();
		var attribute = MockFactory.CreateAttributeDataWithClass(_fixture.Create<string>());

		source.GetAttributes().Returns(ImmutableArray.Create(attribute));

		var actual = _mapper.Map(source);

		actual.Should().BeNull();
	}

	[TestMethod]
	public void TestMapAssignsName()
	{
		var source = CreateMethodSymbol();
		var expected = _fixture.Create<string>();

		source.Name.Returns(expected);

		var metadata = _mapper.Map(source);

		Assert.AreEqual(expected, metadata?.Name);
	}

	[TestMethod]
	public void TestMapAssignsAttribute()
	{
		var source = MockFactory.CreateMethodSymbol();
		var knownAttribute = CreateMethodAttribute();
		var unknownAttribute = MockFactory.CreateAttributeDataWithClass(_fixture.Create<string>());
		var expected = _fixture.Create<ReactMethodAttribute>();

		source.GetAttributes().Returns(ImmutableArray.Create(knownAttribute, unknownAttribute));
		_attributeMapper.Map(knownAttribute).Returns(expected);

		var metadata = _mapper.Map(source);

		Assert.AreEqual(expected, metadata?.Attribute);
	}

	[TestMethod]
	public void TestMapAssignsReturnTypeWhenNotVoid()
	{
		var source = CreateMethodSymbol();
		var expected = _fixture.Create<string>();
		var type = MockFactory.CreateNamedTypeSymbol(expected);

		source.ReturnsVoid.Returns(false);
		source.ReturnType.Returns(type);

		var metadata = _mapper.Map(source);

		Assert.AreEqual(expected, metadata?.ReturnTypeName);
	}

	[TestMethod]
	public void TestMapDoesNotAssignReturnTypeWhenVoid()
	{
		var source = CreateMethodSymbol();

		source.ReturnsVoid.Returns(true);

		var metadata = _mapper.Map(source);

		Assert.IsNull(metadata?.ReturnTypeName);
	}

	[TestMethod]
	public void TestMapAssignsParameters()
	{
		var source = CreateMethodSymbol();
		var expected = _fixture.Create<IList<ParameterMetadata>>();
		var paramSymbols = expected.Select(p =>
		{
			var symbol = MockFactory.CreateParameterSymbol(_fixture.Create<string>());

			_parameterMapper.Map(symbol).Returns(p);

			return symbol;
		}).ToImmutableArray();

		source.Parameters.Returns(paramSymbols);

		var metadata = _mapper.Map(source);

		metadata?.Parameters.Should().BeEquivalentTo(expected);
	}

	IMethodSymbol CreateMethodSymbol()
	{
		var result = MockFactory.CreateMethodSymbol();
		var attribute = CreateMethodAttribute();

		result.GetAttributes().Returns(ImmutableArray.Create(attribute));

		return result;
	}

	AttributeData CreateMethodAttribute() =>
		MockFactory.CreateAttributeDataWithClass(typeof(ReactMethodAttribute).FullName!);
}

