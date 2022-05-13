using System;
using System.Linq;
using System.Text;
using Astound.ReactNative.Generators.Mappers;
using Astound.ReactNative.Generators.Models;
using Astound.ReactNative.Generators.ObjC.Models;

namespace Astound.ReactNative.Generators.ObjC.Mappers;

public class ObjCMethodMapper : IMapper<MethodMetadata, MethodInteropInfo>
{
	IMapper<string, string> _typeMapper;

	public ObjCMethodMapper(IMapper<string, string> typeMapper)
	{
		_typeMapper = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));
	}

	public MethodInteropInfo Map(MethodMetadata source)
	{
		if (source == null)
		{
			throw new ArgumentNullException(nameof(source));
		}

		var exportBuilder = new StringBuilder(source.Name);

		if (source.Parameters?.Any() == true)
		{
			exportBuilder.Append(":");
		}

		var nameBuilder = new StringBuilder(exportBuilder.ToString());

		for (int i = 0; i < source.Parameters?.Count; i++)
		{
			ParameterMetadata parameter = source.Parameters[i];
			string objCTypeName = _typeMapper.Map(parameter.TypeName);

			if (i > 0)
			{
				exportBuilder.Append($"{parameter.Name}:");
				nameBuilder.Append($" {parameter.Name}:");
			}

			nameBuilder.Append($" ({objCTypeName}) {parameter.Name}");
		}

		return new MethodInteropInfo()
		{
			ExportSelector = exportBuilder.ToString(),
			ObjectiveCName = nameBuilder.ToString()
		};
	}
}

