using System;

namespace OCSM.DnD.Fifth.Inventory
{
	public class ItemArmor : Item, IComparable<ItemArmor>, IEquatable<ItemArmor>
	{
		public bool AllowDexterityBonus { get; set; }
		public int BaseArmorClass { get; set; }
		public int DexterityBonusLimit { get; set; }
		public bool LimitDexterityBonus { get; set; }
		public int MinimumStrength { get; set; }
		public bool StealthDisadvantage { get; set; }
		public ArmorType Type { get; set; }
		
		public ItemArmor() : base()
		{
			AllowDexterityBonus = true;
			BaseArmorClass = 0;
			DexterityBonusLimit = -1;
			Equippable = true;
			LimitDexterityBonus = false;
			MinimumStrength = 0;
			StealthDisadvantage = false;
			Type = ArmorType.None;
		}
		
		public ItemArmor(string name, string description) : base(name, description)
		{
			AllowDexterityBonus = true;
			BaseArmorClass = 0;
			DexterityBonusLimit = -1;
			Equippable = true;
			LimitDexterityBonus = false;
			MinimumStrength = 0;
			StealthDisadvantage = false;
			Type = ArmorType.None;
		}
		
		public int CompareTo(ItemArmor item)
		{
			var ret = Type.CompareTo(item.Type);
			if(item is ItemArmor)
			{
				if(ret.Equals(0))
					ret = BaseArmorClass.CompareTo(item.BaseArmorClass);
				if(ret.Equals(0))
					ret = base.CompareTo(item);
				if(ret.Equals(0))
					ret = StealthDisadvantage.CompareTo(item.StealthDisadvantage);
				if(ret.Equals(0))
					ret = AllowDexterityBonus.CompareTo(item.AllowDexterityBonus);
				if(ret.Equals(0))
					ret = LimitDexterityBonus.CompareTo(item.LimitDexterityBonus);
				if(ret.Equals(0))
					ret = DexterityBonusLimit.CompareTo(item.DexterityBonusLimit);
				if(ret.Equals(0))
					ret = MinimumStrength.CompareTo(item.MinimumStrength);
			}
			return ret;
		}
		
		public bool Equals(ItemArmor item)
		{
			return base.Equals(item)
				&& AllowDexterityBonus.Equals(item.AllowDexterityBonus)
				&& BaseArmorClass.Equals(item.BaseArmorClass)
				&& DexterityBonusLimit.Equals(item.DexterityBonusLimit)
				&& LimitDexterityBonus.Equals(item.LimitDexterityBonus)
				&& Type.Equals(item.Type);
		}
	}
}
