using System;
using System.Collections.Generic;

namespace OCSM
{
	public sealed class Skills : Dictionary<Skill, int>
	{
		public Skills() : base()
		{
			foreach(var s in Skill.toList())
			{
				Add(s, 0);
			}
		}
	}

	public sealed class Skill
	{
		public static Skill Academics = new Skill { Name = "Academics", Type = TraitType.Mental };
		public static Skill Athletics = new Skill { Name = "Athletics", Type = TraitType.Physical };
		public static Skill AnimalKen = new Skill { Name = "Animal Ken", Type = TraitType.Social };
		public static Skill Brawl = new Skill { Name = "Brawl", Type = TraitType.Physical };
		public static Skill Computer = new Skill { Name = "Computer", Type = TraitType.Mental };
		public static Skill Crafts = new Skill { Name = "Crafts", Type = TraitType.Mental };
		public static Skill Drive = new Skill { Name = "Drive", Type = TraitType.Physical };
		public static Skill Empathy = new Skill { Name = "Empathy", Type = TraitType.Social };
		public static Skill Expression = new Skill { Name = "Expression", Type = TraitType.Social };
		public static Skill Firearms = new Skill { Name = "Firearms", Type = TraitType.Physical };
		public static Skill Intimidation = new Skill { Name = "Intimidation", Type = TraitType.Social };
		public static Skill Investigation = new Skill { Name = "Investigation", Type = TraitType.Mental };
		public static Skill Larceny = new Skill { Name = "Larceny", Type = TraitType.Physical };
		public static Skill Medicine = new Skill { Name = "Medicine", Type = TraitType.Mental };
		public static Skill Occult = new Skill { Name = "Occult", Type = TraitType.Mental };
		public static Skill Persuasion = new Skill { Name = "Persuasion", Type = TraitType.Social };
		public static Skill Politics = new Skill { Name = "Politics", Type = TraitType.Mental };
		public static Skill Science = new Skill { Name = "Science", Type = TraitType.Mental };
		public static Skill Socialize = new Skill { Name = "Socialize", Type = TraitType.Social };
		public static Skill Stealth = new Skill { Name = "Stealth", Type = TraitType.Physical };
		public static Skill Streetwise = new Skill { Name = "Streetwise", Type = TraitType.Social };
		public static Skill Subterfuge = new Skill { Name = "Subterfuge", Type = TraitType.Social };
		public static Skill Survival = new Skill { Name = "Survival", Type = TraitType.Physical };
		public static Skill Weaponry = new Skill { Name = "Weaponry", Type = TraitType.Physical };
		
		public static List<Skill> toList()
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
		public TraitType Type { get; private set; }
	}
}
