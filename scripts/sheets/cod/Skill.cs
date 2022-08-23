using System;
using System.Collections.Generic;

namespace OCSM
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
		
		public static Skill Academics = new Skill { Name = Names.Academics, Type = TraitType.Mental, Value = 0 };
		public static Skill Athletics = new Skill { Name = Names.Athletics, Type = TraitType.Physical, Value = 0 };
		public static Skill AnimalKen = new Skill { Name = Names.AnimalKen, Type = TraitType.Social, Value = 0 };
		public static Skill Brawl = new Skill { Name = Names.Brawl, Type = TraitType.Physical, Value = 0 };
		public static Skill Computer = new Skill { Name = Names.Computer, Type = TraitType.Mental, Value = 0 };
		public static Skill Crafts = new Skill { Name = Names.Crafts, Type = TraitType.Mental, Value = 0 };
		public static Skill Drive = new Skill { Name = Names.Drive, Type = TraitType.Physical, Value = 0 };
		public static Skill Empathy = new Skill { Name = Names.Empathy, Type = TraitType.Social, Value = 0 };
		public static Skill Expression = new Skill { Name = Names.Expression, Type = TraitType.Social, Value = 0 };
		public static Skill Firearms = new Skill { Name = Names.Firearms, Type = TraitType.Physical, Value = 0 };
		public static Skill Intimidation = new Skill { Name = Names.Intimidation, Type = TraitType.Social, Value = 0 };
		public static Skill Investigation = new Skill { Name = Names.Investigation, Type = TraitType.Mental, Value = 0 };
		public static Skill Larceny = new Skill { Name = Names.Larceny, Type = TraitType.Physical, Value = 0 };
		public static Skill Medicine = new Skill { Name = Names.Medicine, Type = TraitType.Mental, Value = 0 };
		public static Skill Occult = new Skill { Name = Names.Occult, Type = TraitType.Mental, Value = 0 };
		public static Skill Persuasion = new Skill { Name = Names.Persuasion, Type = TraitType.Social, Value = 0 };
		public static Skill Politics = new Skill { Name = Names.Politics, Type = TraitType.Mental, Value = 0 };
		public static Skill Science = new Skill { Name = Names.Science, Type = TraitType.Mental, Value = 0 };
		public static Skill Socialize = new Skill { Name = Names.Socialize, Type = TraitType.Social, Value = 0 };
		public static Skill Stealth = new Skill { Name = Names.Stealth, Type = TraitType.Physical, Value = 0 };
		public static Skill Streetwise = new Skill { Name = Names.Streetwise, Type = TraitType.Social, Value = 0 };
		public static Skill Subterfuge = new Skill { Name = Names.Subterfuge, Type = TraitType.Social, Value = 0 };
		public static Skill Survival = new Skill { Name = Names.Survival, Type = TraitType.Physical, Value = 0 };
		public static Skill Weaponry = new Skill { Name = Names.Weaponry, Type = TraitType.Physical, Value = 0 };
		
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
