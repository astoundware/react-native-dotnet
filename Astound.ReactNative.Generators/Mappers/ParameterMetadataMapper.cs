using System;
using Astound.ReactNative.Generators.Models;
using Microsoft.CodeAnalysis;

namespace Astound.ReactNative.Generators.Mappers;

public class ParameterMetadataMapper : IMapper<IParameterSymbol, ParameterMetadata>
{
	public ParameterMetadata Map(IParameterSymbol source)
	{
		if (source == null)
		{
			throw new ArgumentNullException(nameof(source));
		}

		return new ParameterMetadata()
		{
			Name = source.Name,
			TypeName = source.Type.ToDisplayString()
		};
	}
}

