using System;

namespace Ocsm.Dnd.Fifth.Inventory;

public class ItemArmor : ItemEquippable, IComparable<ItemArmor>, IEquatable<ItemArmor>
{
	public enum ArmorTypes
	{
		None = 0,
		[Label("Light Armor")]
		Light,
		[Label("Medium Armor")]
		Medium,
		[Label("Heavy Armor")]
		Heavy,
		[Label("Shield")]
		Shield,
	}
	
	public bool AllowDexterityBonus { get; set; }
	public int BaseArmorClass { get; set; }
	public int DexterityBonusLimit { get; set; }
	public bool LimitDexterityBonus { get; set; }
	public int MinimumStrength { get; set; }
	public bool StealthDisadvantage { get; set; }
	public ArmorTypes ArmorType { get; set; }
	
	public ItemArmor() : base()
	{
		AllowDexterityBonus = true;
		BaseArmorClass = 0;
		DexterityBonusLimit = -1;
		LimitDexterityBonus = false;
		MinimumStrength = 0;
		StealthDisadvantage = false;
		ArmorType = ArmorTypes.None;
	}
	
	public int CompareTo(ItemArmor other)
	{
		var ret = ArmorType.CompareTo(other?.ArmorType);
	
		if(ret == 0)
			ret = BaseArmorClass.CompareTo(other?.BaseArmorClass);
		
		if(ret == 0)
			ret = base.CompareTo(other);
		
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
	
	public bool Equals(ItemArmor item) => base.Equals(item)
		&& AllowDexterityBonus == item.AllowDexterityBonus
		&& BaseArmorClass == item.BaseArmorClass
		&& DexterityBonusLimit == item.DexterityBonusLimit
		&& LimitDexterityBonus == item.LimitDexterityBonus
		&& ArmorType == item.ArmorType;
	
	public override bool Equals(object obj) => Equals(obj as ItemArmor);
	
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
