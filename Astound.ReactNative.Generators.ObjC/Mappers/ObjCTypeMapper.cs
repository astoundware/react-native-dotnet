using System;
using System.Text.RegularExpressions;
using Astound.ReactNative.Generators.Mappers;

namespace Astound.ReactNative.Generators.ObjC.Mappers;

public class ObjCTypeMapper : IMapper<string, string>
{
	public string Map(string source)
	{
		switch (source)
		{
			case null:
				throw new ArgumentNullException(nameof(source));
			case "bool" or "System.Boolean":
				return "BOOL";
			case "double" or "System.Double"
			or "float" or "System.Single"
			or "int" or "System.Int32"
			or "uint" or "System.UInt32"
			or "nint" or "System.IntPtr"
			or "nuint" or "System.UIntPtr"
			or "long" or "System.Int64"
			or "ulong" or "System.UInt64"
			or "short" or "System.Int16"
			or "ushort" or "System.UInt16":
				return "NSNumber*";
			case "string" or "Foundation.NSString" or "System.String":
				return "NSString*";
			case var s when s == "Foundation.NSDictionary"
			|| s.StartsWith("System.Collections.Generic.Dictionary"):
				return "NSDictionary*";
			case var s when s == "Foundation.NSArray"
			|| Regex.IsMatch(s, @"^(?:(?!\[\]).)*\[\](?!.*\[\])$", RegexOptions.Compiled):
				return "NSArray*";
			default:
				throw new NotSupportedException($"Type {source} is not supported");

		}
	}
}

