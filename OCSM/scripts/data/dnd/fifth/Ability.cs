using System;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth;

public sealed class AbilityInfo(Abilities name, int score = 10, List<Skill> skills = null) : IEquatable<AbilityInfo>
{
	public static List<AbilityInfo> GenerateBaseAbilityScores(int defaultScore = 10) => [
		new(Abilities.Constitution, defaultScore),
		new(Abilities.Charisma, defaultScore, Skill.ListCharisma()),
		new(Abilities.Dexterity, defaultScore, Skill.ListDexterity()),
		new(Abilities.Intelligence, defaultScore, Skill.ListIntelligence()),
		new(Abilities.Strength, defaultScore, Skill.ListStrength()),
		new(Abilities.Wisdom, defaultScore, Skill.ListWisdom())
	];
	
	public Abilities Name { get; set; } = name;
	public Proficiency SavingThrow { get; set; }
	public int Score { get; set; } = score;
	public List<Skill> Skills { get; set; } = skills ?? [];
	
	public int Modifier => (Score / 2) - 5;
	
	public bool Equals(AbilityInfo other) => Name == other?.Name
		&& SavingThrow == other?.SavingThrow
		&& Score == other?.Score
		&& Skills == other?.Skills;
	
	public override bool Equals(object obj) => Equals(obj as AbilityInfo);
	public override int GetHashCode() => HashCode.Combine(Name, SavingThrow, Score, Skills);
}
