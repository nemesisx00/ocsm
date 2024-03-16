using System.Collections.Generic;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth;

public class FifthAdventurer() : Character(GameSystem.Dnd5e)
{
	public List<Ability> Abilities { get; set; } = Ability.generateBaseAbilityScores();
	public string Alignment { get; set; } = string.Empty;
	public Background Background { get; set; }
	public bool BardicInspiration { get; set; }
	public Die BardicInspirationDie { get; set; }
	public string Bonds { get; set; } = string.Empty;
	public List<Class> Classes { get; set; } = [];
	public CoinPurse CoinPurse { get; set; } = new();
	public List<Feature> Features { get; set; } = [];
	public string Flaws { get; set; } = string.Empty;
	public HitPoints HP { get; set; } = new();
	public string Ideals { get; set; } = string.Empty;
	public bool Inspiration { get; set; }
	public List<Item> Inventory { get; set; } = [];
	public string PersonalityTraits { get; set; } = string.Empty;
	public Race Race { get; set; }
	
	public double InventoryWeight
	{
		get
		{
			var val = 0.0;
			Inventory.ForEach(i => {
				if(i is ItemContainer ic)
					val += ic.TotalWeight();
				else
					val += i.Weight;
			});
			return val;
		}
	}
}
