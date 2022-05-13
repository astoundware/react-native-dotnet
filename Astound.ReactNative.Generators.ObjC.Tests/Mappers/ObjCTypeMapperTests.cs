using System;
using Astound.ReactNative.Generators.ObjC.Mappers;

namespace Astound.ReactNative.Generators.ObjC.Tests.Mappers;

[TestClass]
public class ObjCTypeMapperTests
{
    readonly ObjCTypeMapper _mapper = new();

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestMapThrowsOnNullSource()
    {
        _mapper.Map(null);
    }

    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    [DataRow("string[][]")]
    [DataRow("object")]
    [DataRow("System.Object")]
    [DataRow("Foundation.NSObject")]
    public void TestMapThrowsNotSupportedException(string source)
    {
        _mapper.Map(source);
    }

    [TestMethod]
    [DataRow("bool")]
    [DataRow("System.Boolean")]
    public void TestMapReturnsBool(string source)
    {
        var expected = "BOOL";

        var actual = _mapper.Map(source);

        actual.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("double")]
    [DataRow("System.Double")]
    [DataRow("float")]
    [DataRow("System.Single")]
    [DataRow("int")]
    [DataRow("System.Int32")]
    [DataRow("uint")]
    [DataRow("System.UInt32")]
    [DataRow("nint")]
    [DataRow("System.IntPtr")]
    [DataRow("nuint")]
    [DataRow("System.UIntPtr")]
    [DataRow("long")]
    [DataRow("System.Int64")]
    [DataRow("ulong")]
    [DataRow("System.UInt64")]
    [DataRow("short")]
    [DataRow("System.Int16")]
    [DataRow("ushort")]
    [DataRow("System.UInt16")]
    public void TestMapReturnsNSNumber(string source)
    {
        var expected = "NSNumber*";

        var actual = _mapper.Map(source);

        actual.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("string")]
    [DataRow("Foundation.NSString")]
    [DataRow("System.String")]
    public void TestMapReturnsNSString(string source)
    {
        var expected = "NSString*";

        var actual = _mapper.Map(source);

        actual.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("System.Collections.Generic.Dictionary`2[System.Int32,System.String]")]
    [DataRow("System.Collections.Generic.Dictionary`2[System.String,System.Int32]")]
    public void TestMapReturnsNSDictionary(string source)
    {
        var expected = "NSDictionary*";

        var actual = _mapper.Map(source);

        actual.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("int[]")]
    [DataRow("System.Int32[]")]
    [DataRow("string[]")]
    [DataRow("System.String[]")]
    public void TestMapReturnsNSArrayForSingleDimensionArray(string source)
    {
        var expected = "NSArray*";

        var actual = _mapper.Map(source);

        actual.Should().Be(expected);
    }
}
