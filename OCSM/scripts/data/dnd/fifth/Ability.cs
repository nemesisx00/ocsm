using System;
using System.Collections.Generic;

namespace OCSM.DnD.Fifth
{
	public sealed class Ability : IEquatable<Ability>
	{
		public sealed class Names
		{
			public const string Charisma = "Charisma";
			public const string Constitution = "Constitution";
			public const string Dexterity = "Dexterity";
			public const string Intelligence = "Intelligence";
			public const string Strength = "Strength";
			public const string Wisdom = "Wisdom";
			public const string EnumHint = " ," + Constitution + "," + Charisma + "," + Dexterity + "," + Intelligence + "," + Strength + "," + Wisdom;
			
			public static List<string> asList()
			{
				var list = new List<string>();
				list.Add(Names.Charisma);
				list.Add(Names.Constitution);
				list.Add(Names.Dexterity);
				list.Add(Names.Intelligence);
				list.Add(Names.Strength);
				list.Add(Names.Wisdom);
				return list;
			}
		}
		
		public static List<Ability> generateBaseAbilityScores(int defaultScore = 10)
		{
			var list = new List<Ability>();
			list.Add(new Ability(Names.Constitution, defaultScore));
			list.Add(new Ability(Names.Charisma, Skill.listCharisma(), defaultScore));
			list.Add(new Ability(Names.Dexterity, Skill.listDexterity(), defaultScore));
			list.Add(new Ability(Names.Intelligence, Skill.listIntelligence(), defaultScore));
			list.Add(new Ability(Names.Strength, Skill.listStrength(), defaultScore));
			list.Add(new Ability(Names.Wisdom, Skill.listWisdom(), defaultScore));
			return list;
		}
		
		public const int DefaultScore = 10;
		
		public string Name { get; set; }
		public Proficiency SavingThrow { get; set; }
		public int Score { get; set; }
		public List<Skill> Skills { get; set; }
		
		public int Modifier { get { return (Score / 2) - 5; } }
		
		public Ability()
		{
			Name = String.Empty;
			Score = DefaultScore;
			SavingThrow = Proficiency.NoProficiency;
			Skills = new List<Skill>();
		}
		
		public Ability(string name, int score = 10) : this()
		{
			Name = name;
			Score = score;
		}
		
		public Ability(string name, List<Skill> skills, int score = 10) : this(name, score)
		{
			Skills = new List<Skill>(skills);
		}
		
		public bool Equals(Ability ability)
		{
			return ability.Name.Equals(Name)
				&& ability.SavingThrow.Equals(SavingThrow)
				&& ability.Score.Equals(Score)
				&& ability.Skills.Equals(Skills);
		}
	}
}
