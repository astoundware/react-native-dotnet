using System;
using Astound.ReactNative.Generators.Mappers;
using Astound.ReactNative.Generators.Models;
using Astound.ReactNative.Generators.ObjC.Mappers;
using Astound.ReactNative.Generators.ObjC.Models;

namespace Astound.ReactNative.macOS.Generators.ObjC.Tests.Mappers;

[TestClass]
public class ObjCMethodMapperTests
{
	readonly Fixture _fixture = new();
	readonly IMapper<string, string> _typeMapper;
	readonly ObjCMethodMapper _methodMapper;

	public ObjCMethodMapperTests()
	{
		_typeMapper = Substitute.For<IMapper<string, string>>();
		_methodMapper = new ObjCMethodMapper(_typeMapper);

		_typeMapper.Map(Arg.Any<string>()).Returns(_fixture.Create<string>());
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentNullException))]
	public void TestConstructorThrowsOnNullTypeMapper()
	{
		new ObjCMethodMapper(null);
	}

	[TestMethod]
	[ExpectedException(typeof(ArgumentNullException))]
	public void TestMapThrowsOnNullSource()
	{
		_methodMapper.Map(null);
	}

	[TestMethod]
	public void TestMapWorksWithoutParameters()
	{
		var source = _fixture.Build<MethodMetadata>().With(m => m.Parameters, new ParameterMetadata[] { }).Create();
		var expected = new MethodInteropInfo(source.Name, source.Name);

		var actual = _methodMapper.Map(source);

		actual.Should().BeEquivalentTo(expected);
	}

	[TestMethod]
	public void TestMapCallsTypeMapper()
	{
		var source = _fixture.Create<MethodMetadata>();

		_methodMapper.Map(source);

		foreach (ParameterMetadata param in source.Parameters)
		{
			_typeMapper.Received().Map(param.TypeName);
		}
	}

	[TestMethod]
	public void TestMapWorksWithSingleParameter()
	{
		var source = new MethodMetadata()
		{
			Name = "Test",
			Parameters = new ParameterMetadata[]
			{
					_fixture.Build<ParameterMetadata>().With(p => p.Name, "message").Create()
			}
		};
		var expected = new MethodInteropInfo()
		{
			ExportSelector = "Test:",
			ObjectiveCName = "Test: (NSString*) message"
		};

		_typeMapper.Map(Arg.Any<string>()).Returns("NSString*");

		var actual = _methodMapper.Map(source);

		actual.Should().BeEquivalentTo(expected);
	}

	[TestMethod]
	public void TestMapWorksWithMultipleParameters()
	{
		var source = new MethodMetadata()
		{
			Name = "Test",
			Parameters = new ParameterMetadata[]
			{
					_fixture.Build<ParameterMetadata>().With(p => p.Name, "message").Create(),
					_fixture.Build<ParameterMetadata>().With(p => p.Name, "critical").Create(),
					_fixture.Build<ParameterMetadata>().With(p => p.Name, "count").Create()
			}
		};
		var expected = new MethodInteropInfo()
		{
			ExportSelector = "Test:critical:count:",
			ObjectiveCName = "Test: (NSString*) message critical: (BOOL) critical count: (NSNumber*) count"
		};

		_typeMapper.Map(Arg.Any<string>()).Returns("NSString*", "BOOL", "NSNumber*");

		var actual = _methodMapper.Map(source);

		actual.Should().BeEquivalentTo(expected);
	}
}

