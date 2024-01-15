using System;

namespace Ocsm;

[AttributeUsage(AttributeTargets.Field)]
public class LabelAttribute(string label) : Attribute
{
	public string Label { get; } = label;
}
