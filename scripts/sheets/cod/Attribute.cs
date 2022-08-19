using System;
using System.Collections.Generic;

namespace OCSM
{
	public sealed class Attributes : Dictionary<Attribute, int>
	{
		public Attributes() : base()
		{
			foreach(var a in Attribute.toList())
			{
				Add(a, 1);
			}
		}
	}

	public sealed class Attribute
	{
		public static Attribute Composure = new Attribute { Name = "Composure", Type = TraitType.Social };
		public static Attribute Dexterity = new Attribute { Name = "Dexterity", Type = TraitType.Physical };
		public static Attribute Intelligence = new Attribute { Name = "Intelligence", Type = TraitType.Mental };
		public static Attribute Manipulation = new Attribute { Name = "Manipulation", Type = TraitType.Social };
		public static Attribute Presence = new Attribute { Name = "Presence", Type = TraitType.Social };
		public static Attribute Resolve = new Attribute { Name = "Resolve", Type = TraitType.Mental };
		public static Attribute Stamina = new Attribute { Name = "Stamina", Type = TraitType.Physical };
		public static Attribute Strength = new Attribute { Name = "Strength", Type = TraitType.Physical };
		public static Attribute Wits = new Attribute { Name = "Wits", Type = TraitType.Mental };
		
		public static List<Attribute> toList()
		{
			var list = new List<Attribute>();
			list.Add(Composure);
			list.Add(Dexterity);
			list.Add(Intelligence);
			list.Add(Manipulation);
			list.Add(Presence);
			list.Add(Resolve);
			list.Add(Stamina);
			list.Add(Strength);
			list.Add(Wits);
			return list;
		}
		
		public string Name { get; private set; }
		public TraitType Type { get; private set; }
	}
}
