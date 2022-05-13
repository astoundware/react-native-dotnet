using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using Astound.ReactNative.Generators.Models;
using Astound.ReactNative.Generators.Utilities;

namespace Astound.ReactNative.Generators.ObjC.Builders;

public class ClassBuilder : IClassBuilder
{
    readonly IMethodBuilder _methodBuilder;
    readonly IMethodIdentifier _methodIdentifier;

    public ClassBuilder(IMethodBuilder methodBuilder, IMethodIdentifier methodIdentifier)
    {
        _methodBuilder = methodBuilder ?? throw new ArgumentNullException(nameof(methodBuilder));
        _methodIdentifier = methodIdentifier ?? throw new ArgumentNullException(nameof(methodIdentifier));
    }

    public string Build(ClassMetadata metadata)
    {
        if (metadata == null)
        {
            throw new ArgumentNullException(nameof(metadata));
        }

        var stringWriter = new StringWriter();
        var indentedWriter = new IndentedTextWriter(stringWriter, "    ");

        indentedWriter.WriteLine("using System;");
        indentedWriter.WriteLine("using System.Runtime.InteropServices;");
        indentedWriter.WriteLine("using Foundation;");
        indentedWriter.WriteLine("using Astound.ReactNative.Bindings;");
        indentedWriter.WriteLine();
        indentedWriter.WriteLine($"namespace {metadata.Namespace}");
        indentedWriter.WriteLine("{");
        indentedWriter.Indent++;
        indentedWriter.WriteLine($"public partial class {metadata.Name}");
        indentedWriter.WriteLine("{");
        indentedWriter.Indent++;

        var moduleNameMethodMetadata = new MethodMetadata()
        {
            Name = "ModuleName",
            ReturnTypeName = "string"
        };
        string moduleNameMethodId = _methodIdentifier.Identify(moduleNameMethodMetadata);

        moduleNameMethodMetadata.Name = $"{moduleNameMethodMetadata.Name}_{moduleNameMethodId}";

        indentedWriter.WriteLine("[Export(\"moduleName\")]");
        indentedWriter.WriteLine($"public static {moduleNameMethodMetadata}");
        indentedWriter.Indent++;
        indentedWriter.WriteLine($"=> \"{metadata.Attribute?.Name ?? metadata.Name}\";");
        indentedWriter.Indent--;

        indentedWriter.WriteLine();

        var requiresMainQueueSetupMethodMetadata = new MethodMetadata()
        {
            Name = "RequiresMainQueueSetup",
            ReturnTypeName = "bool"
        };
        string requiresMainQueueSetupMethodId = _methodIdentifier.Identify(requiresMainQueueSetupMethodMetadata);

        requiresMainQueueSetupMethodMetadata.Name =
            $"{requiresMainQueueSetupMethodMetadata.Name}_{requiresMainQueueSetupMethodId}";

        indentedWriter.WriteLine("[Export(\"requiresMainQueueSetup\")]");
        indentedWriter.WriteLine($"public static {requiresMainQueueSetupMethodMetadata}");
        indentedWriter.Indent++;
        indentedWriter.WriteLine($"=> {(metadata.Attribute?.RequiresMainQueueSetup ?? false).ToString().ToLower()};");
        indentedWriter.Indent--;

        if (metadata.Methods?.Any() == true)
        {
            indentedWriter.WriteLine();

            for (var i = 0; i < metadata.Methods.Count; i++)
            {
                MethodMetadata method = metadata.Methods[i];

                _methodBuilder.Build(indentedWriter, method);

                if (i < metadata.Methods.Count - 1)
                {
                    indentedWriter.WriteLine();
                }
            }
        }

        indentedWriter.Indent--;
        indentedWriter.WriteLine("}");
        indentedWriter.Indent--;
        indentedWriter.WriteLine("}");

        return stringWriter.ToString();
    }
}

