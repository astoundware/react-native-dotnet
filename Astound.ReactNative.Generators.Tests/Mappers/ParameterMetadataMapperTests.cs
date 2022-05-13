using System;
using Astound.ReactNative.Generators.Mappers;
using Microsoft.CodeAnalysis;

namespace Astound.ReactNative.macOS.Generators.Tests.Mappers;

[TestClass]
public class ParameterMetadataMapperTests
{
	readonly Fixture _fixture = new();
	readonly ParameterMetadataMapper _mapper = new();

	[TestMethod]
	[ExpectedException(typeof(ArgumentNullException))]
	public void TestMapThrowsOnNullSource()
    {
		_mapper.Map(null);
    }

	[TestMethod]
	public void TestMapAssignsName()
    {
		var source = Substitute.For<IParameterSymbol>();
		var expected = _fixture.Create<string>();

		source.Name.Returns(expected);

		var metadata = _mapper.Map(source);

		Assert.AreEqual(expected, metadata?.Name);
    }

	[TestMethod]
	public void TestMapAssignsTypeName()
	{
		var source = Substitute.For<IParameterSymbol>();
		var type = Substitute.For<ITypeSymbol>();
		var expected = _fixture.Create<string>();

		type.ToDisplayString().Returns(expected);
		source.Type.Returns(type);

		var metadata = _mapper.Map(source);

		Assert.AreEqual(expected, metadata?.TypeName);
	}
}

