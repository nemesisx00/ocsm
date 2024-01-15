using System;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth;

public sealed class Skill(string name, Proficiency proficient) : IEquatable<Skill>
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
	
	public static List<Skill> GenerateBaseSkills() => [
		new(Names.Acrobatics, Proficiency.NoProficiency),
		new(Names.AnimalHandling, Proficiency.NoProficiency),
		new(Names.Arcana, Proficiency.NoProficiency),
		new(Names.Athletics, Proficiency.NoProficiency),
		new(Names.Deception, Proficiency.NoProficiency),
		new(Names.History, Proficiency.NoProficiency),
		new(Names.Insight, Proficiency.NoProficiency),
		new(Names.Intimidation, Proficiency.NoProficiency),
		new(Names.Investigation, Proficiency.NoProficiency),
		new(Names.Medicine, Proficiency.NoProficiency),
		new(Names.Nature, Proficiency.NoProficiency),
		new(Names.Perception, Proficiency.NoProficiency),
		new(Names.Performance, Proficiency.NoProficiency),
		new(Names.Persuasion, Proficiency.NoProficiency),
		new(Names.Religion, Proficiency.NoProficiency),
		new(Names.SleightOfHand, Proficiency.NoProficiency),
		new(Names.Stealth, Proficiency.NoProficiency),
		new(Names.Survival, Proficiency.NoProficiency),
	];
	
	public static List<Skill> ListDexterity() => [
		new(Names.Acrobatics, Proficiency.NoProficiency),
		new(Names.SleightOfHand, Proficiency.NoProficiency),
		new(Names.Stealth, Proficiency.NoProficiency),
	];
	
	public static List<Skill> ListCharisma() => [
		new(Names.Deception, Proficiency.NoProficiency),
		new(Names.Intimidation, Proficiency.NoProficiency),
		new(Names.Performance, Proficiency.NoProficiency),
		new(Names.Persuasion, Proficiency.NoProficiency),
	];
	
	public static List<Skill> ListIntelligence() => [
		new(Names.Arcana, Proficiency.NoProficiency),
		new(Names.History, Proficiency.NoProficiency),
		new(Names.Investigation, Proficiency.NoProficiency),
		new(Names.Nature, Proficiency.NoProficiency),
		new(Names.Religion, Proficiency.NoProficiency),
	];
	
	public static List<Skill> ListWisdom() => [
		new(Names.AnimalHandling, Proficiency.NoProficiency),
		new(Names.Insight, Proficiency.NoProficiency),
		new(Names.Medicine, Proficiency.NoProficiency),
		new(Names.Perception, Proficiency.NoProficiency),
		new(Names.Survival, Proficiency.NoProficiency),
	];
	
	public static List<Skill> ListStrength() => [
		new(Names.Athletics, Proficiency.NoProficiency),
	];
	
	public string Name { get; set; } = name;
	public Proficiency Proficient { get; set; } = proficient;
	
	public override bool Equals(object other) => Equals(other as Skill);
	
	public bool Equals(Skill skill) => Name.Equals(skill?.Name)
		&& Proficient.Equals(skill?.Proficient);
	
	public override int GetHashCode() => HashCode.Combine(Name, Proficient);
}
