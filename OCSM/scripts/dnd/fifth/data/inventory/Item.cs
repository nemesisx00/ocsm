using System;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth.Inventory;

public class Item() : Metadata(), IComparable<Item>, IEquatable<Item>
{
	public int Cost { get; set; }
	public double Weight { get; set; }
	
	public int CompareTo(Item item)
	{
		var ret = base.CompareTo(item);
		
		if(ret == 0)
			ret = Cost.CompareTo(item?.Cost);
		
		if(ret == 0)
			ret = Weight.CompareTo(item?.Weight);
		
		return ret;
	}

	public bool Equals(Item item) => base.Equals(item)
		&& Cost == item?.Cost
		&& Weight == item?.Weight;
	
	public override bool Equals(object obj) => Equals(obj as Item);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Cost, Weight);
}
