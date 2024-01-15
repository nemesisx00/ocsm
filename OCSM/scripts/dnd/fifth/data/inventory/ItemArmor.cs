using System;

namespace Ocsm.Dnd.Fifth.Inventory;

public class ItemArmor() : ItemEquippable(), IComparable<ItemArmor>, IEquatable<ItemArmor>
{
	public enum ArmorType
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
	
	public bool AllowDexterityBonus { get; set; } = true;
	public int BaseArmorClass { get; set; }
	public int DexterityBonusLimit { get; set; } = -1;
	public bool LimitDexterityBonus { get; set; }
	public int MinimumStrength { get; set; }
	public bool StealthDisadvantage { get; set; }
	public ArmorType Type { get; set; }
	
	public int CompareTo(ItemArmor item)
	{
		var ret = Type.CompareTo(item?.Type);
		
		if(ret == 0)
			ret = BaseArmorClass.CompareTo(item?.BaseArmorClass);
		
		if(ret == 0)
			ret = base.CompareTo(item);
		
		if(ret == 0)
			ret = StealthDisadvantage.CompareTo(item?.StealthDisadvantage);
		
		if(ret == 0)
			ret = AllowDexterityBonus.CompareTo(item?.AllowDexterityBonus);
		
		if(ret == 0)
			ret = LimitDexterityBonus.CompareTo(item?.LimitDexterityBonus);
		
		if(ret == 0)
			ret = DexterityBonusLimit.CompareTo(item?.DexterityBonusLimit);
		
		if(ret == 0)
			ret = MinimumStrength.CompareTo(item?.MinimumStrength);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as ItemArmor);
	
	public bool Equals(ItemArmor other) => base.Equals(other)
		&& AllowDexterityBonus.Equals(other.AllowDexterityBonus)
		&& BaseArmorClass.Equals(other.BaseArmorClass)
		&& DexterityBonusLimit.Equals(other.DexterityBonusLimit)
		&& LimitDexterityBonus.Equals(other.LimitDexterityBonus)
		&& Type.Equals(other.Type);
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		AllowDexterityBonus,
		BaseArmorClass,
		DexterityBonusLimit,
		LimitDexterityBonus,
		MinimumStrength,
		StealthDisadvantage,
		Type
	);
}
