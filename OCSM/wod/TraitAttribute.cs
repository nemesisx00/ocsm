using System;
using System.Reflection;

namespace Ocsm.Wod;

[AttributeUsage(AttributeTargets.Field)]
public class TraitAttribute(Trait.Category category, Trait.Type type) : System.Attribute
{
	public Trait.Category Category { get; } = category;
	public Trait.Type Type { get; } = type;
}

public static class TraitExtension
{
	public static Trait.Category GetTraitCategory(this Enum value) => value.GetType()
		.GetField(value.ToString())
		.GetCustomAttribute<TraitAttribute>(false)?
		.Category ?? Trait.Category.Mental;
	
	public static Trait.Type GetTraitType(this Enum value) => value.GetType()
		.GetField(value.ToString())
		.GetCustomAttribute<TraitAttribute>(false)?
		.Type ?? Trait.Type.Attribute;
}
