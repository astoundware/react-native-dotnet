using System;
using System.CodeDom.Compiler;
using System.IO;
using Astound.ReactNative.Generators.Mappers;
using Astound.ReactNative.Generators.Models;
using Astound.ReactNative.Generators.ObjC.Builders;
using Astound.ReactNative.Generators.ObjC.Models;
using Astound.ReactNative.Generators.Utilities;
using Astound.ReactNative.Modules;

namespace Astound.ReactNative.Generators.ObjC.Tests.Builders;

[TestClass]
public class MethodBuilderTests
{
    readonly Fixture _fixture = new();
    readonly StringWriter _stringWriter = new();
    readonly IndentedTextWriter _indentedWriter;
    readonly IMapper<MethodMetadata, MethodInteropInfo> _interopMapper =
        Substitute.For<IMapper<MethodMetadata, MethodInteropInfo>>();
    readonly IMethodIdentifier _identifier = Substitute.For<IMethodIdentifier>();
    readonly MethodBuilder _builder;
    readonly string _id;
    readonly MethodInteropInfo _interopInfo;

    public MethodBuilderTests()
    {
        _indentedWriter = new IndentedTextWriter(_stringWriter, "    ");
        _builder = new MethodBuilder(_interopMapper, _identifier);
        _id = _fixture.Create<string>();
        _interopInfo = _fixture.Create<MethodInteropInfo>();

        _identifier.Identify(Arg.Any<MethodMetadata>()).Returns(_id);
        _interopMapper.Map(Arg.Any<MethodMetadata>()).Returns(_interopInfo);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestConstructorThrowsOnNullInteropMapper()
    {
        new MethodBuilder(null, _identifier);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestConstructorThrowsOnNullIdentifier()
    {
        new MethodBuilder(_interopMapper, null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestBuildThrowsOnNullWriter()
    {
        _builder.Build(null, new MethodMetadata());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestBuildThrowsOnNullMethod()
    {
        _builder.Build(_indentedWriter, null);
    }

    [TestMethod]
    public void TestBuildPassesMethodToMap()
    {
        var method = _fixture.Create<MethodMetadata>();

        _builder.Build(_indentedWriter, method);

        _interopMapper.Received().Map(method);
    }

    [TestMethod]
    public void TestBuildPassesMethodToIdentify()
    {
        var method = _fixture.Create<MethodMetadata>();

        _builder.Build(_indentedWriter, method);

        _identifier.Received().Identify(method);
    }

    [TestMethod]
    public void TestBuildWritesBothMethods()
    {
        var method = new MethodMetadata()
        {
            Name = "Test"
        };
        var expected =
@$"[Export(""{_interopInfo.ExportSelector}"")]
public void CallTest_{_id}()
    => Test();

[Export(""__rct_export__{_id}"")]
public static IntPtr GetTestMethodInfo_{_id}()
{{
    var info = new RCTMethodInfo()
    {{
        jsName = """",
        objcName = ""{_interopInfo.ObjectiveCName}"",
        isSync = false
    }};
    var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(info));
    Marshal.StructureToPtr(info, ptr, false);
    return ptr;
}}
";

        _builder.Build(_indentedWriter, method);

        var actual = _stringWriter.ToString();

        actual.Should().Be(expected);
    }

    [TestMethod]
    public void TestBuildUsesCustomName()
    {
        var jsName = _fixture.Create<string>();
        var method = new MethodMetadata()
        {
            Name = "Test",
            Attribute = new ReactMethodAttribute(jsName)
        };
        var expected =
@$"[Export(""__rct_export__{_id}"")]
public static IntPtr GetTestMethodInfo_{_id}()
{{
    var info = new RCTMethodInfo()
    {{
        jsName = ""{jsName}"",
        objcName = ""{_interopInfo.ObjectiveCName}"",
        isSync = false
    }};
    var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(info));
    Marshal.StructureToPtr(info, ptr, false);
    return ptr;
}}
";

        _builder.Build(_indentedWriter, method);

        var actual = _stringWriter.ToString();

        actual.Should().EndWith(expected);
    }

    [TestMethod]
    public void TestBuildUsesReturnType()
    {
        var method = new MethodMetadata()
        {
            Name = "Test",
            ReturnTypeName = "string"
        };
        var expected =
@$"[Export(""{_interopInfo.ExportSelector}"")]
public string CallTest_{_id}()
    => Test();
";

        _builder.Build(_indentedWriter, method);

        var actual = _stringWriter.ToString();

        actual.Should().StartWith(expected);
    }

    [TestMethod]
    public void TestBuildWithSingleParameter()
    {
        var method = new MethodMetadata()
        {
            Name = "Test",
            Parameters = new ParameterMetadata[]
            {
                new ParameterMetadata("message", "string")
            }
        };
        var expected =
@$"[Export(""{_interopInfo.ExportSelector}"")]
public void CallTest_{_id}(string message)
    => Test(message);
";

        _builder.Build(_indentedWriter, method);

        var actual = _stringWriter.ToString();

        actual.Should().StartWith(expected);
    }

    [TestMethod]
    public void TestBuildWithMultipleParameters()
    {
        var method = new MethodMetadata()
        {
            Name = "Log",
            Parameters = new ParameterMetadata[]
            {
                new ParameterMetadata("level", "LogLevel"),
                new ParameterMetadata("value", "int"),
                new ParameterMetadata("format", "string")
            }
        };
        var expected =
@$"[Export(""{_interopInfo.ExportSelector}"")]
public void CallLog_{_id}(
    LogLevel level,
    int value,
    string format
)
    => Log(
        level,
        value,
        format
    );
";

        _builder.Build(_indentedWriter, method);

        var actual = _stringWriter.ToString();

        actual.Should().StartWith(expected);
    }
}

