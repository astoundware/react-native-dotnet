using System.Collections.Generic;
using System.Text;
using Astound.ReactNative.Modules;

namespace Astound.ReactNative.Generators.Models;

public class MethodMetadata
{
	public string Name { get; set; }
	public string ReturnTypeName { get; set; }
	public IList<ParameterMetadata> Parameters { get; set; }
	public ReactMethodAttribute Attribute { get; set; }

	public MethodMetadata()
	{

	}

	public MethodMetadata(
			string name,
			string returnTypeName,
			IList<ParameterMetadata> parameters,
			ReactMethodAttribute attribute)
	{
		Name = name;
		ReturnTypeName = returnTypeName;
		Parameters = parameters;
		Attribute = attribute;
	}

    public override string ToString()
    {
		var builder = new StringBuilder(ReturnTypeName ?? "void");

		builder.Append($" {Name}(");

		for (var i = 0; i < Parameters?.Count; i++)
        {
			ParameterMetadata parameter = Parameters[i];

			if (parameter == null)
            {
				continue;
            }

			if (i > 0)
            {
				builder.Append(", ");
            }

			builder.Append(parameter);
        }

		builder.Append(")");

		return builder.ToString();
    }
}

