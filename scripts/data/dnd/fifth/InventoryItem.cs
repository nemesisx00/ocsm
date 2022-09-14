using System;
using OCSM.Meta;

namespace OCSM.DnD.Fifth
{
	public class InventoryItem : Metadata, IComparable<InventoryItem>, IEquatable<InventoryItem>
	{
		public bool Equippable { get; set; }
		public double Weight { get; set; }
		
		public InventoryItem() : base()
		{
			Equippable = false;
			Weight = 0.0;
		}
		
		public InventoryItem(string name, string description) : base(name, description)
		{
			Equippable = false;
			Weight = 0.0;
		}
		
		public int CompareTo(InventoryItem item)
		{
			var ret = base.CompareTo(item);
			if(item is InventoryItem)
			{
				if(ret.Equals(0))
					ret = Equippable.CompareTo(item.Equippable);
				if(ret.Equals(0))
					ret = Weight.CompareTo(item.Weight);
			}
			return ret;
		}
		
		public bool Equals(InventoryItem item)
		{
			return base.Equals(item)
				&& Equippable.Equals(item.Equippable)
				&& Weight.Equals(item.Weight);
		}
	}
}
