using System;
using System.Collections.Generic;

namespace OCSM.DnD.Fifth
{
	public sealed class Skill : IEquatable<Skill>
	{
		public sealed class Names
		{
			public const string Acrobatics = "Acrobatics";
			public const string AnimalHandling = "Animal Handling";
			public const string Arcana = "Arcana";
			public const string Athletics = "Athletics";
			public const string Deception = "Deception";
			public const string History = "History";
			public const string Insight = "Insight";
			public const string Intimidation = "Intimidation";
			public const string Investigation = "Investigation";
			public const string Medicine = "Medicine";
			public const string Nature = "Nature";
			public const string Perception = "Perception";
			public const string Performance = "Performance";
			public const string Persuasion = "Persuasion";
			public const string Religion = "Religion";
			public const string SleightOfHand = "Sleight of Hand";
			public const string Stealth = "Stealth";
			public const string Survival = "Survival";
		}
		
		public static List<Skill> generateBaseSkills()
		{
			var list = new List<Skill>();
			list.Add(new Skill(Names.Acrobatics, Proficiency.NoProficiency));
			list.Add(new Skill(Names.AnimalHandling, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Arcana, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Athletics, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Deception, Proficiency.NoProficiency));
			list.Add(new Skill(Names.History, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Insight, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Intimidation, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Investigation, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Medicine, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Nature, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Perception, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Performance, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Persuasion, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Religion, Proficiency.NoProficiency));
			list.Add(new Skill(Names.SleightOfHand, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Stealth, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Survival, Proficiency.NoProficiency));
			return list;
		}
		
		public static List<Skill> listDexterity()
		{
			var list = new List<Skill>();
			list.Add(new Skill(Names.Acrobatics, Proficiency.NoProficiency));
			list.Add(new Skill(Names.SleightOfHand, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Stealth, Proficiency.NoProficiency));
			return list;
		}
		
		public static List<Skill> listCharisma()
		{
			var list = new List<Skill>();
			list.Add(new Skill(Names.Deception, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Intimidation, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Performance, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Persuasion, Proficiency.NoProficiency));
			return list;
		}
		
		public static List<Skill> listIntelligence()
		{
			var list = new List<Skill>();
			list.Add(new Skill(Names.Arcana, Proficiency.NoProficiency));
			list.Add(new Skill(Names.History, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Investigation, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Nature, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Religion, Proficiency.NoProficiency));
			return list;
		}
		
		public static List<Skill> listWisdom()
		{
			var list = new List<Skill>();
			list.Add(new Skill(Names.AnimalHandling, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Insight, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Medicine, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Perception, Proficiency.NoProficiency));
			list.Add(new Skill(Names.Survival, Proficiency.NoProficiency));
			return list;
		}
		
		public static List<Skill> listStrength()
		{
			var list = new List<Skill>();
			list.Add(new Skill(Names.Athletics, Proficiency.NoProficiency));
			return list;
		}
		
		public string Name { get; set; }
		public Proficiency Proficient { get; set; }
		
		public Skill(string name, Proficiency proficient)
		{
			Name = name;
			Proficient = proficient;
		}
		
		public bool Equals(Skill skill)
		{
			return skill.Name.Equals(Name)
				&& skill.Proficient.Equals(Proficient);
		}
	}
}
