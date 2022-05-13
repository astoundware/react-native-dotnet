namespace Astound.ReactNative.Generators.Models;

public class ParameterMetadata
{
	public string Name { get; set; }
	public string TypeName { get; set; }

	public ParameterMetadata()
	{

	}

	public ParameterMetadata(string name, string typeName)
	{
		Name = name;
		TypeName = typeName;
	}

    public override string ToString()
    {
		return $"{TypeName} {Name}";
    }
}

