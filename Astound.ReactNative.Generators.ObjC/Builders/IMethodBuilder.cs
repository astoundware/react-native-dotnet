using System.CodeDom.Compiler;
using Astound.ReactNative.Generators.Models;

namespace Astound.ReactNative.Generators.ObjC.Builders;

public interface IMethodBuilder
{
	void Build(IndentedTextWriter writer, MethodMetadata method);
}
