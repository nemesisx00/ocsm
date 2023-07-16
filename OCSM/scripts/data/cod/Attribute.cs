using System;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Cofd
{
	public sealed class Attribute : IComparable<Attribute>, IEquatable<Attribute>
	{
		public enum Enum
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
		
		public const long DefaultValue = 1;
		
		public static List<Attribute> Attributes { get { return System.Enum.GetValues<Enum>().Select(a => new Attribute(a)).ToList(); }}
		
		public static Enum? KindFromString(string text)
		{
			Enum? ret = null;
			System.Enum.GetValues<Enum>()
				.Where(a => a.ToString().Equals(text))
				.ToList()
				.ForEach(a => ret = a);
			
			return ret;
		}
		
		public Trait.Category Category { get; set; }
		public Enum Kind { get; set; }
		public long Value { get; set; }
		public string Name { get { return Kind.GetLabelOrName(); } }
		
		public Attribute() {}
		
		public Attribute(Enum attribute)
		{
			Category = attribute.GetCategory();
			Kind = attribute;
			Value = DefaultValue;
		}
		
		public Attribute(Enum attribute, long value = 1) : this(attribute)
		{
			Value = value;
		}
		
		public int CompareTo(Attribute other)
		{
			var ret = -1;
			if(other is Attribute)
			{
				ret = Category.CompareTo(other.Category);
				if(ret.Equals(0))
					ret = Kind.CompareTo(other.Kind);
				if(ret.Equals(0))
					ret = Value.CompareTo(other.Value);
			}
			return ret;
		}
		
		public bool Equals(Attribute other)
		{
			return Category.Equals(other.Category)
				&& Kind.Equals(other.Kind)
				&& Value.Equals(other.Value);
		}
	}
}
