using System;
using System.CodeDom.Compiler;
using System.Linq;
using Astound.ReactNative.Generators.Mappers;
using Astound.ReactNative.Generators.Models;
using Astound.ReactNative.Generators.ObjC.Models;
using Astound.ReactNative.Generators.Utilities;

namespace Astound.ReactNative.Generators.ObjC.Builders;

public class MethodBuilder : IMethodBuilder
{
    readonly IMapper<MethodMetadata, MethodInteropInfo> _interopMapper;
    readonly IMethodIdentifier _identifier;

    public MethodBuilder(IMapper<MethodMetadata, MethodInteropInfo> interopMapper, IMethodIdentifier identifier)
    {
        _interopMapper = interopMapper ?? throw new ArgumentNullException(nameof(interopMapper));
        _identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
    }

    public void Build(IndentedTextWriter writer, MethodMetadata method)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (method == null)
        {
            throw new ArgumentNullException(nameof(method));
        }

        MethodInteropInfo interopInfo = _interopMapper.Map(method);
        string id = _identifier.Identify(method);

        WriteWrapperMethod(writer, method, interopInfo, id);
        writer.WriteLine();
        WriteInfoMethod(writer, method, interopInfo, id);
    }

    void WriteWrapperMethod(IndentedTextWriter writer, MethodMetadata method, MethodInteropInfo interopInfo, string id)
    {
        int paramCount = method.Parameters?.Count ?? 0;

        writer.WriteLine($"[Export(\"{interopInfo.ExportSelector}\")]");
        writer.Write($"public {method.ReturnTypeName ?? "void"} Call{method.Name}_{id}(");

        if (paramCount > 1)
        {
            writer.WriteLine();
            writer.Indent++;

            for (var i = 0; i < paramCount; i++)
            {
                writer.Write(method.Parameters[i]);

                if (i < paramCount - 1)
                {
                    writer.Write(",");
                }

                writer.WriteLine();
            }

            writer.Indent--;
        }
        else if (paramCount > 0)
        {
            writer.Write(method.Parameters.First());
        }

        writer.WriteLine(")");
        writer.Indent++;
        writer.Write($"=> {method.Name}(");

        if (paramCount > 1)
        {
            writer.WriteLine();
            writer.Indent++;

            for (var i = 0; i < method.Parameters.Count; i++)
            {
                writer.Write(method.Parameters[i]?.Name);

                if (i < method.Parameters.Count - 1)
                {
                    writer.Write(",");
                }

                writer.WriteLine();
            }

            writer.Indent--;
        }
        else if (paramCount > 0)
        {
            writer.Write(method.Parameters.First()?.Name);
        }

        writer.WriteLine(");");
        writer.Indent--;
    }

    void WriteInfoMethod(IndentedTextWriter writer, MethodMetadata method, MethodInteropInfo interopInfo, string id)
    {
        writer.WriteLine($"[Export(\"__rct_export__{id}\")]");
        writer.WriteLine($"public static IntPtr Get{method.Name}MethodInfo_{id}()");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("var info = new RCTMethodInfo()");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"jsName = \"{method.Attribute?.Name}\",");
        writer.WriteLine($"objcName = \"{interopInfo.ObjectiveCName}\",");
        writer.WriteLine("isSync = false");
        writer.Indent--;
        writer.WriteLine("};");
        writer.WriteLine("var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(info));");
        writer.WriteLine("Marshal.StructureToPtr(info, ptr, false);");
        writer.WriteLine("return ptr;");
        writer.Indent--;
        writer.WriteLine("}");
    }
}

