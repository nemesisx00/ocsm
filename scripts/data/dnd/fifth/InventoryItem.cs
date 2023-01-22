using System;
using OCSM.Meta;

namespace OCSM.DnD.Fifth
{
	public class InventoryItem : Metadata, IComparable<InventoryItem>, IEquatable<InventoryItem>
	{
		public int Cost { get; set; }
		public bool Equippable { get; set; }
		public double Weight { get; set; }
		
		public InventoryItem() : base()
		{
			Cost = 0;
			Equippable = false;
			Weight = 0.0;
		}
		
		public InventoryItem(string name, string description) : base(name, description)
		{
			Cost = 0;
			Equippable = false;
			Weight = 0.0;
		}
		
		public int CompareTo(InventoryItem item)
		{
			var ret = base.CompareTo(item);
			if(item is InventoryItem)
			{
				if(ret.Equals(0))
					ret = Cost.CompareTo(item.Cost);
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
				&& Cost.Equals(item.Cost)
				&& Equippable.Equals(item.Equippable)
				&& Weight.Equals(item.Weight);
		}
	}
}
