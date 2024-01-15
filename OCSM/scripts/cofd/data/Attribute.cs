using System;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Cofd;

public sealed class Attribute() : IComparable<Attribute>, IEquatable<Attribute>
{
	public enum EnumValues
	{
		[Trait(Trait.Category.Social)]
		Composure,
		
		[Trait(Trait.Category.Physical)]
		Dexterity,
		
		[Trait(Trait.Category.Mental)]
		Intelligence,
		
		[Trait(Trait.Category.Social)]
		Manipulation,
		
		[Trait(Trait.Category.Social)]
		Presence,
		
		[Trait(Trait.Category.Mental)]
		Resolve,
		
		[Trait(Trait.Category.Physical)]
		Stamina,
		
		[Trait(Trait.Category.Physical)]
		Strength,
		
		[Trait(Trait.Category.Mental)]
		Wits,
	}
	
	public const int DefaultValue = 1;
	
	public static List<Attribute> Attributes => Enum.GetValues<EnumValues>()
		.Select(a => new Attribute(a))
		.ToList();
	
	public static EnumValues? KindFromString(string text) => Enum.GetValues<EnumValues>()
		.Where(a => a.ToString().Equals(text))
		.ToList()
		.FirstOrDefault(null);
	
	public Trait.Category Category { get; set; }
	public EnumValues Kind { get; set; }
	public int Value { get; set; }
	public string Name => Kind.GetLabelOrName();
	
	public Attribute(EnumValues attribute) : this()
	{
		Category = attribute.GetCategory();
		Kind = attribute;
		Value = DefaultValue;
	}
	
	public Attribute(EnumValues attribute, int value = 1) : this(attribute) => Value = value;
	
	public int CompareTo(Attribute other)
	{
		var ret = Category.CompareTo(other?.Category);
		
		if(ret.Equals(0))
			ret = Kind.CompareTo(other?.Kind);
		
		if(ret.Equals(0))
			ret = Value.CompareTo(other?.Value);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as Attribute);
	
	public bool Equals(Attribute other) => Category.Equals(other?.Category)
		&& Kind.Equals(other?.Kind)
		&& Value.Equals(other?.Value);
		
	public override int GetHashCode() => HashCode.Combine(
		Category,
		Kind,
		Value
	);
}
