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
		
		public override string ToString()
		{
			string output = "Attributes { ";
			foreach(var a in Keys)
			{
				output += String.Format("{0}: {1}, ", a.Name, this[a]);
			}
			output += " }";
			return output;
		}
	}
	
	public sealed class Attribute
	{
		public sealed class Names
		{
			public const string Composure = "Composure";
			public const string Dexterity = "Dexterity";
			public const string Intelligence = "Intelligence";
			public const string Manipulation = "Manipulation";
			public const string Presence = "Presence";
			public const string Resolve = "Resolve";
			public const string Stamina = "Stamina";
			public const string Strength = "Strength";
			public const string Wits = "Wits";
		}
		
		public static Attribute Composure = new Attribute { Name = Names.Composure, Type = TraitType.Social };
		public static Attribute Dexterity = new Attribute { Name = Names.Dexterity, Type = TraitType.Physical };
		public static Attribute Intelligence = new Attribute { Name = Names.Intelligence, Type = TraitType.Mental };
		public static Attribute Manipulation = new Attribute { Name = Names.Manipulation, Type = TraitType.Social };
		public static Attribute Presence = new Attribute { Name = Names.Presence, Type = TraitType.Social };
		public static Attribute Resolve = new Attribute { Name = Names.Resolve, Type = TraitType.Mental };
		public static Attribute Stamina = new Attribute { Name = Names.Stamina, Type = TraitType.Physical };
		public static Attribute Strength = new Attribute { Name = Names.Strength, Type = TraitType.Physical };
		public static Attribute Wits = new Attribute { Name = Names.Wits, Type = TraitType.Mental };
		
		public static Attribute byName(string name)
		{
			switch(name)
			{
				case Names.Composure:
					return Composure;
				case Names.Dexterity:
					return Dexterity;
				case Names.Intelligence:
					return Intelligence;
				case Names.Manipulation:
					return Manipulation;
				case Names.Presence:
					return Presence;
				case Names.Resolve:
					return Resolve;
				case Names.Stamina:
					return Stamina;
				case Names.Strength:
					return Strength;
				case Names.Wits:
					return Wits;
				default:
					return null;
			}
		}
		
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
