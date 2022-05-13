using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Astound.ReactNative.Generators.Mappers;
using Astound.ReactNative.Generators.Models;
using Astound.ReactNative.Generators.ObjC.Builders;
using Astound.ReactNative.Generators.ObjC.Mappers;
using Astound.ReactNative.Generators.Utilities;
using Astound.ReactNative.Modules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Astound.ReactNative.macOS.Generators;

[Generator]
public class ReactGenerator : IIncrementalGenerator
{
	static readonly IMapper<INamedTypeSymbol, ClassMetadata> _classMetadataMapper;
	static readonly IClassBuilder _classBuilder;

	static ReactGenerator()
	{
		var parameterMapper = new ParameterMetadataMapper();
		var methodAttributeMapper = new ReactMethodAttributeMapper();
		var methodMapper = new MethodMetadataMapper(methodAttributeMapper, parameterMapper);
		var moduleAttributeMapper = new ReactModuleAttributeMapper();

		_classMetadataMapper = new ClassMetadataMapper(moduleAttributeMapper, methodMapper);

		var typeMapper = new ObjCTypeMapper();
		var interopMapper = new ObjCMethodMapper(typeMapper);
		var methodIdentifier = new MethodIdentifier();
		var methodBuilder = new MethodBuilder(interopMapper, methodIdentifier);

		_classBuilder = new ClassBuilder(methodBuilder, methodIdentifier);
	}

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
				transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
			.Where(static m => m is not null)!;

		IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses
			= context.CompilationProvider.Combine(classDeclarations.Collect());

		context.RegisterSourceOutput(compilationAndClasses,
			static (spc, source) => Execute(source.Item1, source.Item2, spc));
	}

	static bool IsSyntaxTargetForGeneration(SyntaxNode node)
		=> node is ClassDeclarationSyntax m && m.AttributeLists.Count > 0;

	static ClassDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
	{
		// we know the node is a ClassDeclarationSyntax thanks to IsSyntaxTargetForGeneration
		var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

		// loop through all the attributes on the class
		foreach (AttributeListSyntax attributeListSyntax in classDeclarationSyntax.AttributeLists)
		{
			foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
			{
				if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
				{
					// weird, we couldn't get the symbol, ignore it
					continue;
				}

				INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
				string fullName = attributeContainingTypeSymbol.ToDisplayString();

				// Is the attribute the [EnumExtensions] attribute?
				if (fullName == typeof(ReactModuleAttribute).FullName)
				{
					// return the enum
					return classDeclarationSyntax;
				}
			}
		}

		// we didn't find the attribute we were looking for
		return null;
	}

	static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context)
	{
		if (classes.IsDefaultOrEmpty)
		{
			// nothing to do yet
			return;
		}

		IEnumerable<ClassDeclarationSyntax> distinctClasses = classes.Distinct();

		IList<ClassMetadata> classesToGenerate = GetTypesToGenerate(compilation, distinctClasses, context.CancellationToken);
		if (classesToGenerate.Count > 0)
		{
			foreach (var classToGenerate in classesToGenerate)
			{
				string source = _classBuilder.Build(classToGenerate);

				context.AddSource($"{classToGenerate.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
			}
		}
	}

	static IList<ClassMetadata> GetTypesToGenerate(Compilation compilation, IEnumerable<ClassDeclarationSyntax> classes, CancellationToken ct)
	{
		// Create a list to hold our output
		var classesToGenerate = new List<ClassMetadata>();

		foreach (ClassDeclarationSyntax classDeclarationSyntax in classes)
		{
			// stop if we're asked to
			ct.ThrowIfCancellationRequested();

			// Get the semantic representation of the class syntax
			SemanticModel semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
			if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
			{
				// something went wrong, bail out
				continue;
			}

			ClassMetadata classMetadata = _classMetadataMapper.Map(classSymbol);

			if (classMetadata == null)
			{
				continue;
			}

			classesToGenerate.Add(classMetadata);
		}

		return classesToGenerate;
	}

}

