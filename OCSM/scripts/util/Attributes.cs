using System;

namespace Ocsm
{
	[AttributeUsage(AttributeTargets.Field)]
	public class LabelAttribute : Attribute
	{
		public string Label { get; }

		public LabelAttribute(string label)
		{
			Label = label;
		}
	}
}
