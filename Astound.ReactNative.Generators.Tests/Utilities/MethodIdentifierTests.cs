using System;
using Astound.ReactNative.Generators.Models;
using Astound.ReactNative.Generators.Utilities;

namespace Astound.ReactNative.Generators.Tests.Utilities;

[TestClass]
public class MethodIdentifierTests
{
    class MockMethodMetadata : MethodMetadata
    {
        readonly string _output;

        public MockMethodMetadata(string output)
        {
            _output = output;
        }

        public override string ToString()
        {
            return _output;
        }
    }

    Fixture _fixture = new();
    MethodIdentifier _identifier = new();

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestIdentifyThrowsOnNullMethod()
    {
        _identifier.Identify(null);
    }

    [TestMethod]
    public void TestIdentifyReturnsValueWithoutSpecialCharacters()
    {
        var method = new MockMethodMetadata(_fixture.Create<string>());

        var actual = _identifier.Identify(method);

        actual.Should().MatchRegex("^[a-zA-Z0-9]+$");
    }

    [TestMethod]
    public void TestIdentifyIsIdempotent()
    {
        var method = new MockMethodMetadata(_fixture.Create<string>());

        var actual1 = _identifier.Identify(method);
        var actual2 = _identifier.Identify(method);

        actual2.Should().Be(actual1);
    }
}
