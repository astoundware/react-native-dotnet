using Astound.ReactNative.Generators.Models;

namespace Astound.ReactNative.Generators.ObjC.Builders;

public interface IClassBuilder
{
    string Build(ClassMetadata metadata);
}
