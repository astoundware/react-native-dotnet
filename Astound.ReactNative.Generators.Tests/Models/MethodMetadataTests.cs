using Astound.ReactNative.Generators.Models;

namespace Astound.ReactNative.Generators.Tests.Models;

[TestClass]
public class MethodMetadataTests
{
    readonly Fixture _fixture = new();

    [TestMethod]
    public void TestToStringStartsWithReturnTypeWhenNotNull()
    {
        var type = _fixture.Create<string>();
        var metadata = new MethodMetadata()
        {
            ReturnTypeName = type
        };

        var actual = metadata.ToString();

        actual.Should().StartWith(type);
    }

    [TestMethod]
    public void TestToStringStartsWithVoidWhenReturnTypeIsNull()
    {
        var metadata = new MethodMetadata();

        var actual = metadata.ToString();

        actual.Should().StartWith("void");
    }

    [TestMethod]
    public void TestToStringContainsNameWhenNotNull()
    {
        var name = _fixture.Create<string>();
        var metadata = new MethodMetadata()
        {
            Name = name
        };

        var actual = metadata.ToString();

        actual.Should().Contain(name);
    }

    [TestMethod]
    public void TestToStringEndsWithParenthesesWhenParameterIsNull()
    {
        var metadata = new MethodMetadata();

        var actual = metadata.ToString();

        actual.Should().EndWith("()");
    }

    [TestMethod]
    public void TestToStringEndsWithParenthesesWhenParameterIsEmpty()
    {
        var metadata = new MethodMetadata()
        {
            Parameters = new ParameterMetadata[] { }
        };

        var actual = metadata.ToString();

        actual.Should().EndWith("()");
    }

    [TestMethod]
    public void TestToStringIncludesSingleParameterInParentheses()
    {
        var metadata = new MethodMetadata()
        {
            Parameters = new ParameterMetadata[]
            {
                new ParameterMetadata("value", "int")
            }
        };

        var actual = metadata.ToString();

        actual.Should().EndWith("(int value)");
    }

    [TestMethod]
    public void TestToStringIncludesMultipleParametersInParentheses()
    {
        var metadata = new MethodMetadata()
        {
            Parameters = new ParameterMetadata[]
            {
                new ParameterMetadata("value", "string"),
                new ParameterMetadata("count", "int")
            }
        };

        var actual = metadata.ToString();

        actual.Should().EndWith("(string value, int count)");
    }

    [TestMethod]
    public void TestToStringAppendsAllTokens()
    {
        var metadata = new MethodMetadata()
        {
            Name = "Format",
            ReturnTypeName = "string",
            Parameters = new ParameterMetadata[]
            {
                new ParameterMetadata("value", "int")
            }
        };

        var actual = metadata.ToString();

        actual.Should().Be("string Format(int value)");
    }
}

