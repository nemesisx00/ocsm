using System.Collections.Generic;
using System.Linq;
using Ocsm.Dnd.Fifth.Inventory;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth;

public class FifthAdventurer() : Character(GameSystemFactory.Name)
{
	public List<AbilityInfo> Abilities { get; set; } = AbilityInfo.GenerateBaseAbilityScores();
	public string Alignment { get; set; }
	public Metadata Background { get; set; }
	public bool BardicInspiration { get; set; }
	public Die BardicInspirationDie { get; set; }
	public string Bonds { get; set; }
	public List<ClassData> Classes { get; set; } = [];
	public CoinPurse CoinPurse { get; set; } = new();
	public List<Feature> Features { get; set; } = [];
	public string Flaws { get; set; }
	public HitPoints HP { get; set; } = new();
	public string Ideals { get; set; }
	public bool Inspiration { get; set; }
	public List<Item> Inventory { get; set; } = [];
	public string PersonalityTraits { get; set; }
	public Metadata Species { get; set; }
	
	public double InventoryWeight => Inventory.Aggregate(0.0, (acc, i) => acc + i.TotalWeight());
}
