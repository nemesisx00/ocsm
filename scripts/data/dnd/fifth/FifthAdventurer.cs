using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth.Inventory;

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
		public List<Item> Inventory { get; set; }
		public string PersonalityTraits { get; set; }
		public Race Race { get; set; }
		
		public double InventoryWeight
		{
			get
			{
				var val = 0.0;
				Inventory.ForEach(i => {
					if(i is ItemContainer ic)
						val += ic.totalWeight();
					else
						val += i.Weight;
				});
				return val;
			}
		}
		
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
			Inventory = new List<Item>();
			PersonalityTraits = String.Empty;
			Race = null;
		}
	}
}
