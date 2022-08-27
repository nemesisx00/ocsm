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
		
		public static List<Skill> generateNewAbilityScores()
		{
			var list = new List<Skill>();
			list.Add(new Skill(Names.Acrobatics, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.AnimalHandling, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Arcana, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Athletics, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Deception, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.History, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Insight, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Intimidation, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Investigation, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Medicine, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Nature, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Perception, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Performance, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Persuasion, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Religion, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.SleightOfHand, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Stealth, Proficiency.Enum.NoProficiency));
			list.Add(new Skill(Names.Survival, Proficiency.Enum.NoProficiency));
			return list;
		}
		
		public string Name { get; set; }
		public Proficiency.Enum Proficient { get; set; }
		
		public Skill(string name, Proficiency.Enum proficient)
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
