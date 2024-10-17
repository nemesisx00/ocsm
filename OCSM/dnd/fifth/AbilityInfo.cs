using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Ocsm.Dnd.Fifth;

public sealed class AbilityInfo() : IEquatable<AbilityInfo>
{
	public static List<AbilityInfo> GenerateBaseAbilityScores(int defaultScore = 10) => [
		new(Abilities.Constitution, defaultScore),
		new(Abilities.Charisma, defaultScore, Skill.ListCharisma()),
		new(Abilities.Dexterity, defaultScore, Skill.ListDexterity()),
		new(Abilities.Intelligence, defaultScore, Skill.ListIntelligence()),
		new(Abilities.Strength, defaultScore, Skill.ListStrength()),
		new(Abilities.Wisdom, defaultScore, Skill.ListWisdom())
	];
	
	public Abilities AbilityType { get; set; }
	
	[JsonIgnore]
	public int BonusTotal => Bonuses.Aggregate(0, (acc, val) => acc + val);
	
	[JsonIgnore]
	public int Modifier => (TotalScore / 2) - 5;
	
	[JsonIgnore]
	public int? OverrideScore { get; set; } = null;
	
	[JsonIgnore]
	public int TotalScore => OverrideScore
		?? Bonuses.Aggregate(Score, (acc, val) => acc + val);
	
	public Proficiency SavingThrow { get; set; }
	public int Score { get; set; }
	public List<Skill> Skills { get; set; } = [];
	
	[JsonIgnore]
	private List<int> Bonuses { get; set; } = [];
	
	public AbilityInfo(Abilities ability, int score = 10, List<Skill> skills = null) : this()
	{
		AbilityType = ability;
		Score = score;
		
		if(skills is not null)
			Skills = skills;
	}
	
	public void AddBonus(int val) => Bonuses.Add(val);
	public void ClearBonuses() => Bonuses.Clear();
	
	public bool Equals(AbilityInfo other) => AbilityType == other?.AbilityType
		&& SavingThrow == other?.SavingThrow
		&& Score == other?.Score
		&& Skills == other?.Skills;
	
	public override bool Equals(object obj) => Equals(obj as AbilityInfo);
	public override int GetHashCode() => HashCode.Combine(AbilityType, SavingThrow, Score, Skills);
}
