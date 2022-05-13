using System;
using System.Linq;
using Astound.ReactNative.Modules;
using Astound.ReactNative.Generators.Models;
using Microsoft.CodeAnalysis;

namespace Astound.ReactNative.Generators.Mappers;

public class MethodMetadataMapper : IMapper<IMethodSymbol, MethodMetadata>
{
	readonly IMapper<AttributeData, ReactMethodAttribute> _attributeMapper;
	readonly IMapper<IParameterSymbol, ParameterMetadata> _parameterMapper;

	public MethodMetadataMapper(
	IMapper<AttributeData, ReactMethodAttribute> attributeMapper,
	IMapper<IParameterSymbol, ParameterMetadata> parameterMapper)
	{
		_attributeMapper = attributeMapper ?? throw new ArgumentNullException(nameof(attributeMapper));
		_parameterMapper = parameterMapper ?? throw new ArgumentNullException(nameof(parameterMapper));
	}

	public MethodMetadata Map(IMethodSymbol source)
	{
		if (source == null)
        {
			throw new ArgumentNullException(nameof(source));
        }

		AttributeData attributeData = source.GetAttributes().FirstOrDefault(a =>
			a.AttributeClass?.ToDisplayString() == typeof(ReactMethodAttribute).FullName);

		if (attributeData == null)
		{
			return null;
		}

		return new MethodMetadata()
		{
			Name = source.Name,
			Attribute = _attributeMapper.Map(attributeData),
			ReturnTypeName = source.ReturnsVoid ? null : source.ReturnType.ToDisplayString(),
			Parameters = source.Parameters.Select(p => _parameterMapper.Map(p)).ToList()
		};
	}
}

