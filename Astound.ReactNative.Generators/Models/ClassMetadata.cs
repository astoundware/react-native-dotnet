using System.Collections.Generic;
using Astound.ReactNative.Modules;

namespace Astound.ReactNative.Generators.Models;

public class ClassMetadata
{
	public string Name { get; set; }
	public string Namespace { get; set; }
	public IList<MethodMetadata> Methods { get; set; }
	public ReactModuleAttribute Attribute { get; set; }

	public ClassMetadata()
	{

	}

	public ClassMetadata(string name, string @namespace, IList<MethodMetadata> methods, ReactModuleAttribute attribute)
	{
		Name = name;
		Namespace = @namespace;
		Methods = methods;
		Attribute = attribute;
	}
}

