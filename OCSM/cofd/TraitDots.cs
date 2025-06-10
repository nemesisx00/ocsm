using System;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Cofd;

/**
<summary>
Represents a single trait with an associated dot value.
</summary>
<remarks>
The primary purpose is to unify the data structures used to represent
Attributes and Skills but it may have additional utility in the future.
</remarks>
*/
public class TraitDots(Traits trait, int value = 1) : IComparable<TraitDots>, IEquatable<TraitDots>
{
	public static List<TraitDots> Attributes => Enum.GetValues<Traits>()
		.Where(t => t.GetTraitType() == Trait.Type.Attribute)
		.Select(t => new TraitDots(t))
		.ToList();
	
	public static List<TraitDots> Skills => Enum.GetValues<Traits>()
		.Where(t => t.GetTraitType() == Trait.Type.Skill)
		.Select(t => new TraitDots(t))
		.ToList();
	
	public static Traits? KindFromString(string text) => Enum.GetValues<Traits>()
		.Where(a => a.ToString() == text)
		.FirstOrDefault();
	
	public Trait.Category Category { get; set; } = trait.GetTraitCategory();
	public Traits Kind { get; set; } = trait;
	public Trait.Type Type { get; set; } = trait.GetTraitType();
	public int Value { get; set; } = value;
	
	public string Name => Kind.GetLabel();
	
	public TraitDots() : this(Traits.Academics, 1) {}
	
	public int CompareTo(TraitDots other)
	{
		var ret = Type.CompareTo(other?.Type);
		
		if(ret == 0)
			Category.CompareTo(other?.Category);
		
		if(ret == 0)
			ret = Kind.CompareTo(other?.Kind);
		
		if(ret == 0)
			ret = Value.CompareTo(other?.Value);
		
		return ret;
	}
	
	public bool Equals(TraitDots other) => Category == other?.Category
		&& Kind == other?.Kind
		&& Type == other?.Type
		&& Value == other?.Value;
	
	public override bool Equals(object obj) => Equals(obj as TraitDots);
	public override int GetHashCode() => HashCode.Combine(Category, Kind, Value);
}
