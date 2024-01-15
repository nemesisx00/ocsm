using System;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth;

public sealed class Ability() : IEquatable<Ability>
{
	public sealed class Names
	{
		public const string Charisma = "Charisma";
		public const string Constitution = "Constitution";
		public const string Dexterity = "Dexterity";
		public const string Intelligence = "Intelligence";
		public const string Strength = "Strength";
		public const string Wisdom = "Wisdom";
		
		public static List<string> AsList() => [
			Charisma,
			Constitution,
			Dexterity,
			Intelligence,
			Strength,
			Wisdom,
		];
	}
	
	public static List<Ability> GenerateBaseAbilityScores(int? defaultScore = null) => [
		new(Names.Constitution, defaultScore ?? DefaultScore),
		new(Names.Charisma, Skill.ListCharisma(), defaultScore ?? DefaultScore),
		new(Names.Dexterity, Skill.ListDexterity(), defaultScore ?? DefaultScore),
		new(Names.Intelligence, Skill.ListIntelligence(), defaultScore ?? DefaultScore),
		new(Names.Strength, Skill.ListStrength(), defaultScore ?? DefaultScore),
		new(Names.Wisdom, Skill.ListWisdom(), defaultScore ?? DefaultScore),
	];
	
	public const int DefaultScore = 10;
	
	public string Name { get; set; }
	public Proficiency SavingThrow { get; set; }
	public int Score { get; set; } = DefaultScore;
	public List<Skill> Skills { get; set; } = [];
	
	public int Modifier => (Score / 2) - 5;
	
	public Ability(string name, int? score = null) : this()
	{
		Name = name;
		Score = score ?? DefaultScore;
	}
	
	public Ability(string name, List<Skill> skills, int score = 10) : this(name, score)
		=> Skills = new List<Skill>(skills);
	
	public override bool Equals(object other) => Equals(other as Ability);
	
	public bool Equals(Ability other) => Name.Equals(other?.Name)
		&& SavingThrow.Equals(other?.SavingThrow)
		&& Score.Equals(other?.Score)
		&& Skills.Equals(other?.Skills);
	
	public override int GetHashCode()
	{
		HashCode hash = new();
		hash.Add(Name);
		hash.Add(SavingThrow);
		hash.Add(Score);
		Skills.ForEach(s => hash.Add(s));
		return hash.ToHashCode();
	}
}
