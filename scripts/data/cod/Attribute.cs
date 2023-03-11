using System;
using System.Collections;
using System.Collections.Generic;

namespace OCSM.CoD
{
	public sealed class Attribute : IEquatable<Attribute>
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
		
		public static Attribute Composure = new Attribute(Names.Composure, TraitType.Social, 1);
		public static Attribute Dexterity = new Attribute(Names.Dexterity, TraitType.Physical, 1);
		public static Attribute Intelligence = new Attribute(Names.Intelligence, TraitType.Mental, 1);
		public static Attribute Manipulation = new Attribute(Names.Manipulation, TraitType.Social, 1);
		public static Attribute Presence = new Attribute(Names.Presence, TraitType.Social, 1);
		public static Attribute Resolve = new Attribute(Names.Resolve, TraitType.Mental, 1);
		public static Attribute Stamina = new Attribute(Names.Stamina, TraitType.Physical, 1);
		public static Attribute Strength = new Attribute(Names.Strength, TraitType.Physical, 1);
		public static Attribute Wits = new Attribute(Names.Wits, TraitType.Mental, 1);
		
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
		
		public static List<Attribute> asList()
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
		
		public Attribute(string name, string type, long value = 1)
		{
			Name = name;
			Type = type;
			Value = value;
		}
		
		public string Name { get; private set; }
		public string Type { get; private set; }
		public long Value { get; set; }
		
		public bool Equals(Attribute attr)
		{
			return attr.Name.Equals(Name)
				&& attr.Type.Equals(Type)
				&& attr.Value.Equals(Value);
		}
	}
}
