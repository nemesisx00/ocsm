using System;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Cofd;

public sealed class Skill() : IComparable<Skill>, IEquatable<Skill>
{
	public enum EnumValues
	{
		[Trait(Trait.Category.Mental)]
		Academics,
		
		[Trait(Trait.Category.Physical)]
		Athletics,
		
		[Label("Animal Ken")]
		[Trait(Trait.Category.Social)]
		AnimalKen,
		
		[Trait(Trait.Category.Physical)]
		Brawl,
		
		[Trait(Trait.Category.Mental)]
		Computer,
		
		[Trait(Trait.Category.Mental)]
		Crafts,
		
		[Trait(Trait.Category.Physical)]
		Drive,
		
		[Trait(Trait.Category.Social)]
		Empathy,
		
		[Trait(Trait.Category.Social)]
		Expression,
		
		[Trait(Trait.Category.Physical)]
		Firearms,
		
		[Trait(Trait.Category.Social)]
		Intimidation,
		
		[Trait(Trait.Category.Mental)]
		Investigation,
		
		[Trait(Trait.Category.Physical)]
		Larceny,
		
		[Trait(Trait.Category.Mental)]
		Medicine,
		
		[Trait(Trait.Category.Mental)]
		Occult,
		
		[Trait(Trait.Category.Social)]
		Persuasion,
		
		[Trait(Trait.Category.Mental)]
		Politics,
		
		[Trait(Trait.Category.Mental)]
		Science,
		
		[Trait(Trait.Category.Social)]
		Socialize,
		
		[Trait(Trait.Category.Physical)]
		Stealth,
		
		[Trait(Trait.Category.Social)]
		Streetwise,
		
		[Trait(Trait.Category.Social)]
		Subterfuge,
		
		[Trait(Trait.Category.Physical)]
		Survival,
		
		[Trait(Trait.Category.Physical)]
		Weaponry,
	}
	
	public const int DefaultValue = 0;
	
	/// <returns>Returns a new list Skill instances based on Skill.EnumValues.</returns>
	public static List<Skill> Skills => Enum.GetValues<EnumValues>().Select(s => new Skill(s)).ToList();
	
	public static EnumValues? KindFromString(string text) => Enum.GetValues<EnumValues>()
		.Where(s => s.ToString().Equals(text) || (!string.IsNullOrEmpty(s.GetLabel()) && s.GetLabel().Equals(text)))
		.ToList()
		.FirstOrDefault(null);
	
	public Trait.Category Category { get; set; }
	public EnumValues Kind { get; set; }
	public int Value { get; set; }
	public string Name => Kind.GetLabelOrName();
	
	public Skill(EnumValues skill) : this()
	{
		Category = skill.GetCategory();
		Kind = skill;
		Value = DefaultValue;
	}
	
	public Skill(EnumValues skill, int value) : this(skill) => Value = value;
	
	public int CompareTo(Skill other)
	{
		var ret = Category.CompareTo(other?.Category);
		
		if(ret == 0)
			ret = Kind.CompareTo(other?.Kind);
		
		if(ret == 0)
			ret = Value.CompareTo(other?.Value);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as Skill);
	
	public bool Equals(Skill other) => Category.Equals(other?.Category)
		&& Kind.Equals(other?.Kind)
		&& Value.Equals(other?.Value);
	
	public override int GetHashCode() => HashCode.Combine(
		Category,
		Kind,
		Value
	);
}
