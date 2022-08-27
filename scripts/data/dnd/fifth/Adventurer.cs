using System;
using System.Collections.Generic;

namespace OCSM.DnD.Fifth
{
	public class Adventurer : Character
	{
		public List<AbilityScore> AbilityScores { get; set; }
		public List<SavingThrow> SavingThrows { get; set; }
		public List<Skill> Skills { get; set; }
		
		public Adventurer() : base(OCSM.GameSystem.DnD.Fifth)
		{
			AbilityScores = new List<AbilityScore>();
			SavingThrows = new List<SavingThrow>();
			Skills = new List<Skill>();
		}
	}
}
