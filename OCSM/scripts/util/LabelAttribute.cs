using System;
using System.Reflection;

namespace Ocsm;

[AttributeUsage(AttributeTargets.Field)]
public class LabelAttribute(string label) : Attribute
{
	public string Label { get; } = label;
}

public static class LabelExtension
{
	public static string GetLabel(this Enum value) => value.GetType()
		.GetField(value.ToString())
		.GetCustomAttribute<LabelAttribute>(false)?
		.Label ?? value.ToString();
}
