using System;

namespace Astound.ReactNative.Generators.Mappers;

/// <summary>
/// Provides operations for mapping an object of one type to another.
/// </summary>
/// <typeparam name="TSource">The type of the source object.</typeparam>
/// <typeparam name="TDestination">The type of the destination object.</typeparam>
public interface IMapper<TSource, TDestination>
{
	/// <summary>
    /// Maps a source object to a destination type.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <returns>The mapped destination object.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
	TDestination Map(TSource source);
}
