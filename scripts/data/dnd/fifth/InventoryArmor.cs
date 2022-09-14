using System;
using OCSM.Meta;

namespace OCSM.DnD.Fifth
{
	public class InventoryArmor : InventoryItem, IComparable<InventoryArmor>, IEquatable<InventoryArmor>
	{
		public bool AllowDexterityBonus { get; set; }
		public int BaseArmorClass { get; set; }
		public int DexterityBonusLimit { get; set; }
		
		public InventoryArmor() : base()
		{
			AllowDexterityBonus = true;
			BaseArmorClass = 12;
			DexterityBonusLimit = -1;
		}
		
		public InventoryArmor(string name, string description) : base(name, description)
		{
			AllowDexterityBonus = true;
			BaseArmorClass = 12;
			DexterityBonusLimit = -1;
		}
		
		public int CompareTo(InventoryArmor item)
		{
			var ret = base.CompareTo(item);
			if(item is InventoryArmor)
			{
				if(ret.Equals(0))
					ret = BaseArmorClass.CompareTo(item.BaseArmorClass);
				if(ret.Equals(0))
					ret = AllowDexterityBonus.CompareTo(item.AllowDexterityBonus);
				if(ret.Equals(0))
					ret = DexterityBonusLimit.CompareTo(item.DexterityBonusLimit);
			}
			return ret;
		}
		
		public bool Equals(InventoryArmor item)
		{
			return base.Equals(item)
				&& AllowDexterityBonus.Equals(item.AllowDexterityBonus)
				&& BaseArmorClass.Equals(item.BaseArmorClass)
				&& DexterityBonusLimit.Equals(item.DexterityBonusLimit);
		}
	}
}
