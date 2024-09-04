using System;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth;

public sealed class Skill() : IEquatable<Skill>
{
	public static List<Skill> GenerateBaseSkills() => [
		new(Skills.Acrobatics, Proficiency.NoProficiency),
		new(Skills.AnimalHandling, Proficiency.NoProficiency),
		new(Skills.Arcana, Proficiency.NoProficiency),
		new(Skills.Athletics, Proficiency.NoProficiency),
		new(Skills.Deception, Proficiency.NoProficiency),
		new(Skills.History, Proficiency.NoProficiency),
		new(Skills.Insight, Proficiency.NoProficiency),
		new(Skills.Intimidation, Proficiency.NoProficiency),
		new(Skills.Investigation, Proficiency.NoProficiency),
		new(Skills.Medicine, Proficiency.NoProficiency),
		new(Skills.Nature, Proficiency.NoProficiency),
		new(Skills.Perception, Proficiency.NoProficiency),
		new(Skills.Performance, Proficiency.NoProficiency),
		new(Skills.Persuasion, Proficiency.NoProficiency),
		new(Skills.Religion, Proficiency.NoProficiency),
		new(Skills.SleightOfHand, Proficiency.NoProficiency),
		new(Skills.Stealth, Proficiency.NoProficiency),
		new(Skills.Survival, Proficiency.NoProficiency)
	];
	
	public static List<Skill> ListDexterity() => [
		new(Skills.Acrobatics, Proficiency.NoProficiency),
		new(Skills.SleightOfHand, Proficiency.NoProficiency),
		new(Skills.Stealth, Proficiency.NoProficiency)
	];
	
	public static List<Skill> ListCharisma() => [
		new(Skills.Deception, Proficiency.NoProficiency),
		new(Skills.Intimidation, Proficiency.NoProficiency),
		new(Skills.Performance, Proficiency.NoProficiency),
		new(Skills.Persuasion, Proficiency.NoProficiency)
	];
	
	public static List<Skill> ListIntelligence() => [
		new(Skills.Arcana, Proficiency.NoProficiency),
		new(Skills.History, Proficiency.NoProficiency),
		new(Skills.Investigation, Proficiency.NoProficiency),
		new(Skills.Nature, Proficiency.NoProficiency),
		new(Skills.Religion, Proficiency.NoProficiency)
	];
	
	public static List<Skill> ListWisdom() => [
		new(Skills.AnimalHandling, Proficiency.NoProficiency),
		new(Skills.Insight, Proficiency.NoProficiency),
		new(Skills.Medicine, Proficiency.NoProficiency),
		new(Skills.Perception, Proficiency.NoProficiency),
		new(Skills.Survival, Proficiency.NoProficiency)
	];
	
	public static List<Skill> ListStrength() => [
		new(Skills.Athletics, Proficiency.NoProficiency)
	];
	
	public Skills SkillType { get; set; }
	public Proficiency Proficient { get; set; }
	
	public Skill(Skills skill, Proficiency proficient) : this()
	{
		SkillType = skill;
		Proficient = proficient;
	}
	
	public bool Equals(Skill skill) => SkillType == skill?.SkillType && Proficient == skill?.Proficient;
	public override bool Equals(object obj) => Equals(obj as Skill);
	public override int GetHashCode() => HashCode.Combine(SkillType, Proficient);
}
