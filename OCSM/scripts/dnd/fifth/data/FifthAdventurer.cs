using System.Collections.Generic;
using System.Linq;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth;

public class FifthAdventurer() : Character(Constants.GameSystem.Dnd.Fifth)
{
	public List<Ability> Abilities { get; set; } = Ability.GenerateBaseAbilityScores();
	public string Alignment { get; set; }
	public Background Background { get; set; }
	public bool BardicInspiration { get; set; }
	public Die BardicInspirationDie { get; set; }
	public string Bonds { get; set; }
	public List<Class> Classes { get; set; } = [];
	public CoinPurse CoinPurse { get; set; } = new();
	public List<Feature> Features { get; set; } = [];
	public string Flaws { get; set; }
	public HitPoints HP { get; set; }
	public string Ideals { get; set; }
	public bool Inspiration { get; set; }
	public List<Item> Inventory { get; set; } = [];
	public string PersonalityTraits { get; set; }
	public Race Race { get; set; }

	public double InventoryWeight => Inventory.Sum(i => i is ItemContainer ic ? ic.TotalWeight() : i.Weight);
}
