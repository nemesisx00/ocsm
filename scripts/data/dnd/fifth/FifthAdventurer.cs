using System;
using System.Collections.Generic;

namespace OCSM.DnD.Fifth
{
	public class FifthAdventurer : Character
	{
		public List<Ability> Abilities { get; set; }
		public string Alignment { get; set; }
		public Background Background { get; set; }
		public bool BardicInspiration { get; set; }
		public Die BardicInspirationDie { get; set; }
		public string Bonds { get; set; }
		public List<Class> Classes { get; set; }
		public CoinPurse CoinPurse { get; set; }
		public List<Feature> Features { get; set; }
		public string Flaws { get; set; }
		public HitPoints HP { get; set; }
		public string Ideals { get; set; }
		public bool Inspiration { get; set; }
		public string PersonalityTraits { get; set; }
		public Race Race { get; set; }
		
		public FifthAdventurer() : base(OCSM.GameSystem.DnD.Fifth)
		{
			Abilities = Ability.generateBaseAbilityScores();
			Alignment = String.Empty;
			Background = null;
			BardicInspiration = false;
			BardicInspirationDie = null;
			Bonds = String.Empty;
			Classes = new List<Class>();
			CoinPurse = new CoinPurse();
			Features = new List<Feature>();
			Flaws = String.Empty;
			HP = new HitPoints();
			Ideals = String.Empty;
			Inspiration = false;
			PersonalityTraits = String.Empty;
			Race = null;
		}
	}
}
