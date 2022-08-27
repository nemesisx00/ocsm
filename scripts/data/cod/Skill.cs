using System;
using System.Collections.Generic;

namespace OCSM.CoD
{
	public sealed class Skill : IEquatable<Skill>
	{
		public sealed class Names
		{
			public const string Academics = "Academics";
			public const string Athletics = "Athletics";
			public const string AnimalKen = "Animal Ken";
			public const string Brawl = "Brawl";
			public const string Computer = "Computer";
			public const string Crafts = "Crafts";
			public const string Drive = "Drive";
			public const string Empathy = "Empathy";
			public const string Expression = "Expression";
			public const string Firearms = "Firearms";
			public const string Intimidation = "Intimidation";
			public const string Investigation = "Investigation";
			public const string Larceny = "Larceny";
			public const string Medicine = "Medicine";
			public const string Occult = "Occult";
			public const string Persuasion = "Persuasion";
			public const string Politics = "Politics";
			public const string Science = "Science";
			public const string Socialize = "Socialize";
			public const string Stealth = "Stealth";
			public const string Streetwise = "Streetwise";
			public const string Subterfuge = "Subterfuge";
			public const string Survival = "Survival";
			public const string Weaponry = "Weaponry";
		}
		
		public static Skill Academics = new Skill(Names.Academics, TraitType.Mental, 0);
		public static Skill Athletics = new Skill(Names.Athletics, TraitType.Physical, 0);
		public static Skill AnimalKen = new Skill(Names.AnimalKen, TraitType.Social, 0);
		public static Skill Brawl = new Skill(Names.Brawl, TraitType.Physical, 0);
		public static Skill Computer = new Skill(Names.Computer, TraitType.Mental, 0);
		public static Skill Crafts = new Skill(Names.Crafts, TraitType.Mental, 0);
		public static Skill Drive = new Skill(Names.Drive, TraitType.Physical, 0);
		public static Skill Empathy = new Skill(Names.Empathy, TraitType.Social, 0);
		public static Skill Expression = new Skill(Names.Expression, TraitType.Social, 0);
		public static Skill Firearms = new Skill(Names.Firearms, TraitType.Physical, 0);
		public static Skill Intimidation = new Skill(Names.Intimidation, TraitType.Social, 0);
		public static Skill Investigation = new Skill(Names.Investigation, TraitType.Mental, 0);
		public static Skill Larceny = new Skill(Names.Larceny, TraitType.Physical, 0);
		public static Skill Medicine = new Skill(Names.Medicine, TraitType.Mental, 0);
		public static Skill Occult = new Skill(Names.Occult, TraitType.Mental, 0);
		public static Skill Persuasion = new Skill(Names.Persuasion, TraitType.Social, 0);
		public static Skill Politics = new Skill(Names.Politics, TraitType.Mental, 0);
		public static Skill Science = new Skill(Names.Science, TraitType.Mental, 0);
		public static Skill Socialize = new Skill(Names.Socialize, TraitType.Social, 0);
		public static Skill Stealth = new Skill(Names.Stealth, TraitType.Physical, 0);
		public static Skill Streetwise = new Skill(Names.Streetwise, TraitType.Social, 0);
		public static Skill Subterfuge = new Skill(Names.Subterfuge, TraitType.Social, 0);
		public static Skill Survival = new Skill(Names.Survival, TraitType.Physical, 0);
		public static Skill Weaponry = new Skill(Names.Weaponry, TraitType.Physical, 0);
		
		public static Skill byName(string name)
		{
			switch(name)
			{
				case Names.Academics:
					return Academics;
				case Names.Athletics:
					return Athletics;
				case Names.AnimalKen:
					return AnimalKen;
				case Names.Brawl:
					return Brawl;
				case Names.Computer:
					return Computer;
				case Names.Crafts:
					return Crafts;
				case Names.Drive:
					return Drive;
				case Names.Empathy:
					return Empathy;
				case Names.Expression:
					return Expression;
				case Names.Firearms:
					return Firearms;
				case Names.Intimidation:
					return Intimidation;
				case Names.Investigation:
					return Investigation;
				case Names.Larceny:
					return Larceny;
				case Names.Medicine:
					return Medicine;
				case Names.Occult:
					return Occult;
				case Names.Persuasion:
					return Persuasion;
				case Names.Politics:
					return Politics;
				case Names.Science:
					return Science;
				case Names.Socialize:
					return Socialize;
				case Names.Stealth:
					return Stealth;
				case Names.Streetwise:
					return Streetwise;
				case Names.Subterfuge:
					return Subterfuge;
				case Names.Survival:
					return Survival;
				case Names.Weaponry:
					return Weaponry;
				default:
					return null;
			}
		}
		
		public static List<Skill> asList()
		{
			var list = new List<Skill>();
			list.Add(Academics);
			list.Add(Athletics);
			list.Add(AnimalKen);
			list.Add(Brawl);
			list.Add(Computer);
			list.Add(Crafts);
			list.Add(Drive);
			list.Add(Empathy);
			list.Add(Expression);
			list.Add(Firearms);
			list.Add(Intimidation);
			list.Add(Investigation);
			list.Add(Larceny);
			list.Add(Medicine);
			list.Add(Occult);
			list.Add(Persuasion);
			list.Add(Politics);
			list.Add(Science);
			list.Add(Socialize);
			list.Add(Stealth);
			list.Add(Streetwise);
			list.Add(Subterfuge);
			list.Add(Survival);
			list.Add(Weaponry);
			return list;
		}
		
		public Skill(string name, string type, int value)
		{
			Name = name;
			Type = type;
			Value = value;
		}
		
		public string Name { get; private set; }
		public string Type { get; private set; }
		public int Value { get; set; }
		
		public bool Equals(Skill skill)
		{
			return skill.Name.Equals(Name)
				&& skill.Type.Equals(Type)
				&& skill.Value.Equals(Value);
		}
	}
}
