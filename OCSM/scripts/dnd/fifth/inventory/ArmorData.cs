using System;

namespace Ocsm.Dnd.Fifth.Inventory;

public class ArmorData() : IComparable<ArmorData>, IEquatable<ArmorData>
{
	public bool AllowDexterityBonus { get; set; } = true;
	public int BaseArmorClass { get; set; }
	public int DexterityBonusLimit { get; set; } = -1;
	public bool LimitDexterityBonus { get; set; }
	public int MinimumStrength { get; set; }
	public bool StealthDisadvantage { get; set; }
	public ArmorTypes ArmorType { get; set; }
	
	public int CompareTo(ArmorData other)
	{
		var ret = ArmorType.CompareTo(other?.ArmorType);
	
		if(ret == 0)
			ret = BaseArmorClass.CompareTo(other?.BaseArmorClass);
		
		if(ret == 0)
			ret = StealthDisadvantage.CompareTo(other?.StealthDisadvantage);
		
		if(ret == 0)
			ret = AllowDexterityBonus.CompareTo(other?.AllowDexterityBonus);
		
		if(ret == 0)
			ret = LimitDexterityBonus.CompareTo(other?.LimitDexterityBonus);
		
		if(ret == 0)
			ret = DexterityBonusLimit.CompareTo(other?.DexterityBonusLimit);
		
		if(ret == 0)
			ret = MinimumStrength.CompareTo(other?.MinimumStrength);
		
		return ret;
	}
	
	public bool Equals(ArmorData item) => base.Equals(item)
		&& AllowDexterityBonus == item?.AllowDexterityBonus
		&& BaseArmorClass == item?.BaseArmorClass
		&& DexterityBonusLimit == item?.DexterityBonusLimit
		&& LimitDexterityBonus == item?.LimitDexterityBonus
		&& ArmorType == item?.ArmorType;
	
	public override bool Equals(object obj) => Equals(obj as ArmorData);
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		AllowDexterityBonus,
		BaseArmorClass,
		DexterityBonusLimit,
		LimitDexterityBonus,
		MinimumStrength,
		StealthDisadvantage,
		ArmorType
	);
}
