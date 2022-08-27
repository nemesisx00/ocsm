using System;
using System.Collections.Generic;

namespace OCSM.DnD.Fifth
{
	public sealed class AbilityScore : IEquatable<AbilityScore>
	{
		public sealed class Names
		{
			public const string Constitution = "Constitution";
			public const string Charisma = "Charisma";
			public const string Dexterity = "Dexterity";
			public const string Intelligence = "Intelligence";
			public const string Strength = "Strength";
			public const string Wisdom = "Wisdom";
		}
		
		public static List<AbilityScore> generateNewAbilityScores(int defaultScore = 10)
		{
			var list = new List<AbilityScore>();
			list.Add(new AbilityScore(Names.Constitution, defaultScore));
			list.Add(new AbilityScore(Names.Charisma, defaultScore));
			list.Add(new AbilityScore(Names.Dexterity, defaultScore));
			list.Add(new AbilityScore(Names.Intelligence, defaultScore));
			list.Add(new AbilityScore(Names.Strength, defaultScore));
			list.Add(new AbilityScore(Names.Wisdom, defaultScore));
			return list;
		}
		
		public string Name { get; set; }
		public int Score { get; set; }
		public int Modifier { get { return (Score / 2) - 5; } }
		
		public AbilityScore(string name, int score = 10)
		{
			Name = name;
			Score = score;
		}
		
		public bool Equals(AbilityScore abilityScore)
		{
			return abilityScore.Name.Equals(Name)
				&& abilityScore.Score.Equals(Score);
		}
	}
}
