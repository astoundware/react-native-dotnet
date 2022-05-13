using Astound.ReactNative.Generators.Models;

namespace Astound.ReactNative.Generators.Tests.Models;

[TestClass]
public class ParameterMetadataTests
{
    [TestMethod]
    public void ToStringContainsTypeNameAndName()
    {
        var parameter = new ParameterMetadata()
        {
            Name = "format",
            TypeName = "string"
        };

        var actual = parameter.ToString();

        actual.Should().Be("string format");
    }
}
