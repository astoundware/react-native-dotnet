using System;
using System.Linq;
using System.Reflection;

namespace Astound.ReactNative.Modules.ObjC;

public class ReactModuleRegistry : IReactModuleRegistry
{
    readonly IReactFunctions _reactFunctions;

    public ReactModuleRegistry(IReactFunctions reactFunctions)
    {
        _reactFunctions = reactFunctions ?? throw new ArgumentNullException(nameof(reactFunctions));
    }

    public void Register(Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        var objCName = type.FullName.Replace("+", "_").Replace(".", "_");

        _reactFunctions.RegisterModule(objCName);
    }

    public void Register(Assembly assembly)
    {
        if (assembly == null)
        {
            throw new ArgumentNullException(nameof(assembly));
        }

        foreach (Type type in assembly.GetTypes().Where(t => !t.IsAbstract && t.IsDefined(typeof(ReactModuleAttribute))))
        {
            Register(type);
        }
    }
}

