namespace Astound.ReactNative.Generators.ObjC.Models;

public class MethodInteropInfo
{
	public string ExportSelector { get; set; }
	public string ObjectiveCName { get; set; }

	public MethodInteropInfo()
	{

	}

	public MethodInteropInfo(string exportSelector, string objectiveCName)
	{
		ExportSelector = exportSelector;
		ObjectiveCName = objectiveCName;
	}
}

