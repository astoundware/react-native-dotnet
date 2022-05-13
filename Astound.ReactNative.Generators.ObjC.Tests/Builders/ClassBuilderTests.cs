using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Astound.ReactNative.Generators.Models;
using Astound.ReactNative.Generators.ObjC.Builders;
using Astound.ReactNative.Generators.Utilities;
using Astound.ReactNative.Modules;

namespace Astound.ReactNative.Generators.ObjC.Tests.Builders;

[TestClass]
public class ClassBuilderTests
{
    readonly Fixture _fixture = new();
    readonly IMethodBuilder _methodBuilder = Substitute.For<IMethodBuilder>();
    readonly IMethodIdentifier _methodIdentifier = Substitute.For<IMethodIdentifier>();
    readonly ClassBuilder _builder;
    readonly string _moduleNameId;
    readonly string _requiresMainQueueSetupId;

    public ClassBuilderTests()
    {
        _builder = new ClassBuilder(_methodBuilder, _methodIdentifier);
        _moduleNameId = _fixture.Create<string>();
        _requiresMainQueueSetupId = _fixture.Create<string>();

        _methodIdentifier.Identify(Arg.Is<MethodMetadata>(m => m != null && m.Name == "ModuleName"))
            .Returns(_moduleNameId);
        _methodIdentifier.Identify(Arg.Is<MethodMetadata>(m => m != null && m.Name == "RequiresMainQueueSetup"))
            .Returns(_requiresMainQueueSetupId);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestConstructorThrowsOnNullMethodBuilder()
    {
        new ClassBuilder(null, _methodIdentifier);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestConstructorThrowsOnNullMethodIdentifier()
    {
        new ClassBuilder(_methodBuilder, null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestBuildThrowsArgumentNullException()
    {
        _builder.Build(null);
    }

    [TestMethod]
    public void TestBuildConstructsClassWithoutMethods()
    {
        var metadata = new ClassMetadata()
        {
            Name = "TestModule",
            Namespace = "My.Native.Modules"
        };
        var expected =
@$"using System;
using System.Runtime.InteropServices;
using Astound.ReactNative.Bindings;
using Foundation;

namespace My.Native.Modules
{{
    public partial class TestModule
    {{
        [Export(""moduleName"")]
        public static string ModuleName_{_moduleNameId}()
            => ""TestModule"";
        
        [Export(""requiresMainQueueSetup"")]
        public static bool RequiresMainQueueSetup_{_requiresMainQueueSetupId}()
            => false;
    }}
}}
";

        var actual = _builder.Build(metadata);

        actual.Should().Be(expected);
    }

    [TestMethod]
    public void TestBuildConstructsClassWithMethods()
    {
        var metadata = new ClassMetadata()
        {
            Name = "TestModule",
            Namespace = "My.Native.Modules",
            Methods = _fixture.CreateMany<MethodMetadata>(3).ToList()
        };
        var methodSources = new List<string>();

        foreach (MethodMetadata method in metadata.Methods)
        {
            var source = _fixture.Create<string>();

            methodSources.Add(source);
            _methodBuilder.When(x => x.Build(Arg.Any<IndentedTextWriter>(), method))
                .Do(x => x.ArgAt<IndentedTextWriter>(0).WriteLine(source));
        }

        var expected =
@$"using System;
using System.Runtime.InteropServices;
using Astound.ReactNative.Bindings;
using Foundation;

namespace My.Native.Modules
{{
    public partial class TestModule
    {{
        [Export(""moduleName"")]
        public static string ModuleName_{_moduleNameId}()
            => ""TestModule"";
        
        [Export(""requiresMainQueueSetup"")]
        public static bool RequiresMainQueueSetup_{_requiresMainQueueSetupId}()
            => false;
        
        {methodSources[0]}
        
        {methodSources[1]}
        
        {methodSources[2]}
    }}
}}
";

        var actual = _builder.Build(metadata);

        actual.Should().Be(expected);
    }

    [TestMethod]
    public void TestBuildUsesCustomModuleName()
    {
        var moduleName = _fixture.Create<string>();
        var metadata = _fixture.Build<ClassMetadata>()
            .With(m => m.Attribute, new ReactModuleAttribute(moduleName))
            .Create();
        var expected =
@$"
        [Export(""moduleName"")]
        public static string ModuleName_{_moduleNameId}()
            => ""{moduleName}"";
";

        var actual = _builder.Build(metadata);

        actual.Should().Contain(expected);
    }

    [TestMethod]
    public void TestBuildUsesRequiresMainQueueSetupValue()
    {
        var metadata = _fixture.Build<ClassMetadata>()
            .With(m => m.Attribute, new ReactModuleAttribute()
            {
                RequiresMainQueueSetup = true
            })
            .Create();
        var expected =
@$"
        [Export(""requiresMainQueueSetup"")]
        public static bool RequiresMainQueueSetup_{_requiresMainQueueSetupId}()
            => true;
";

        var actual = _builder.Build(metadata);

        actual.Should().Contain(expected);
    }
}

