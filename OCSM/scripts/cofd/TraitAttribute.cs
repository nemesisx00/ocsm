using System;
using System.Reflection;

namespace Ocsm.Cofd;

[AttributeUsage(AttributeTargets.Field)]
public class TraitAttribute(Trait.Category type) : System.Attribute
{
	public Trait.Category Category { get; } = type;
}

public static class TraitExtension
{
	public static Trait.Category GetCategory(this Enum value) => value.GetType()
		.GetField(value.ToString())
		.GetCustomAttribute<TraitAttribute>(false)?
		.Category ?? Trait.Category.Mental;
}
