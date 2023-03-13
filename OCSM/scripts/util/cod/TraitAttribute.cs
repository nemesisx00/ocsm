using System;
using System.Reflection;

namespace OCSM.CoD
{
	[AttributeUsage(AttributeTargets.Field)]
	public class TraitAttribute : System.Attribute
	{
		public Trait.Category Category { get; }
		
		public TraitAttribute(Trait.Category type)
		{
			Category = type;
		}
	}
	
	public static class TraitExtension
	{
		public static Trait.Category GetCategory(this Enum value)
		{
			return value.GetType()
				.GetField(value.ToString())
				.GetCustomAttribute<TraitAttribute>(false)?
				.Category ?? Trait.Category.Mental;
		}
	}
}
