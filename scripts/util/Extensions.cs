using System;
using System.Reflection;

namespace OCSM
{
	public static class EnumExtensions
	{
		public static string GetLabel(this Enum value)
		{
			return value.GetType()
				.GetField(value.ToString())
				.GetCustomAttribute<LabelAttribute>(false)?
				.Label ?? String.Empty;
		}
	}
}
