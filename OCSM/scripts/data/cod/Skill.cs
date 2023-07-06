using System;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Cofd
{
	public sealed class Skill : IComparable<Skill>, IEquatable<Skill>
	{
		public enum Enum
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
		
		public const long DefaultValue = 0;
		
		/// <returns>Returns a new list Skill instances based on Skill.Enum.</returns>
		public static List<Skill> Skills { get { return System.Enum.GetValues<Enum>().Select(s => new Skill(s)).ToList(); } }
		
		public static Enum? KindFromString(string text)
		{
			Enum? ret = null;
			System.Enum.GetValues<Enum>()
				.Where(s => s.ToString().Equals(text) || (!String.IsNullOrEmpty(s.GetLabel()) && s.GetLabel().Equals(text)))
				.ToList()
				.ForEach(s => ret = s);
			
			return ret;
		}
		
		public Trait.Category Category { get; set; }
		public Enum Kind { get; set; }
		public long Value { get; set; }
		public string Name { get { return Kind.GetLabelOrName(); } }
		
		public Skill() {}
		
		public Skill(Enum skill)
		{
			Category = skill.GetCategory();
			Kind = skill;
			Value = DefaultValue;
		}
		
		public Skill(Enum skill, long value) : this(skill)
		{
			Value = value;
		}
		
		public int CompareTo(Skill other)
		{
			var ret = -1;
			if(other is Skill)
			{
				ret = Category.CompareTo(other.Category);
				if(ret.Equals(0))
					ret = Kind.CompareTo(other.Kind);
				if(ret.Equals(0))
					ret = Value.CompareTo(other.Value);
			}
			return ret;
		}
		
		public bool Equals(Skill skill)
		{
			return Category.Equals(skill.Category)
				&& Kind.Equals(skill.Kind)
				&& Value.Equals(skill.Value);
		}
	}
}
