namespace Ocsm.Dnd.Fifth.Inventory;

public enum WeaponTypes
{
	[Label("")]
	None = 0,
	
	[Label("Simple")]
	Simple = SimpleMelee | SimpleRanged,
	
	[Label("Simple Melee")]
	SimpleMelee = 1,
	
	[Label("Simple Ranged")]
	SimpleRanged = 2,
	
	[Label("Martial")]
	Martial = MartialMelee | MartialRanged,
	
	[Label("Martial Melee")]
	MartialMelee = 4,
	
	[Label("Martial Ranged")]
	MartialRanged = 8,
	
	[Label("Improvised")]
	Improvised = 16,
}
