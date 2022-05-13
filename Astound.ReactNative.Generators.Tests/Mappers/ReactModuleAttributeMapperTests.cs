using System;
using Astound.ReactNative.Generators.Mappers;
using Astound.ReactNative.Modules;

namespace Astound.ReactNative.Generators.Tests.Mappers;

[TestClass]
public class ReactModuleAttributeMapperTests
{
    readonly Fixture _fixture = new();
    readonly ReactModuleAttributeMapper _mapper = new();

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestMapThrowsOnNullSource()
    {
        _mapper.Map(null);
    }

    [TestMethod]
    public void TestMapAssignsNameFromConstructor()
    {
        var name = _fixture.Create<string>();
        var expected = new ReactModuleAttribute(name);
        var source = MockFactory.CreateAttributeDataWithConstructorArgument(name);

        var actual = _mapper.Map(source);

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void TestMapAssignsNameFromNamedArguments()
    {
        var name = _fixture.Create<string>();
        var expected = new ReactModuleAttribute()
        {
            Name = name
        };
        var source = MockFactory.CreateAttributeDataWithNamedArgument(nameof(expected.Name), name);

        var actual = _mapper.Map(source);

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void TestMapAssignsRequiresMainQueueSetupFromNamedArguments()
    {
        var requiresMainQueueSetup = _fixture.Create<bool>();
        var expected = new ReactModuleAttribute()
        {
            RequiresMainQueueSetup = requiresMainQueueSetup
        };
        var source = MockFactory.CreateAttributeDataWithNamedArgument(
            nameof(expected.RequiresMainQueueSetup),
            requiresMainQueueSetup);

        var actual = _mapper.Map(source);

        actual.Should().BeEquivalentTo(expected);
    }
}

