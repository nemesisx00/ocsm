using System;
using System.Collections.Generic;

namespace OCSM.DnD.Fifth
{
	public class FifthAdventurer : Character
	{
		public List<AbilityScore> AbilityScores { get; set; }
		public Background Background { get; set; }
		public List<Class> Classes { get; set; }
		public List<Feature> Features { get; set; }
		public Race Race { get; set; }
		public List<SavingThrow> SavingThrows { get; set; }
		public List<Skill> Skills { get; set; }
		
		public FifthAdventurer() : base(OCSM.GameSystem.DnD.Fifth)
		{
			AbilityScores = AbilityScore.generateBaseAbilityScores();
			Background = null;
			Classes = new List<Class>();
			Features = new List<Feature>();
			Race = null;
			SavingThrows = SavingThrow.generateBaseSavingThrows();
			Skills = Skill.generateBaseSkills();
		}
	}
}
