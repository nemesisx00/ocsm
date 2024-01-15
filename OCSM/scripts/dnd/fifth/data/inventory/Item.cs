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
	
	public override bool Equals(object other) => Equals(other as Item);
	
	public bool Equals(Item other) => base.Equals(other)
		&& Cost.Equals(other?.Cost)
		&& Weight.Equals(other?.Weight);
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		Cost,
		Weight
	);
}
