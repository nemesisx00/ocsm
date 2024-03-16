using System;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth.Inventory;

public class ItemContainer() : ItemEquippable(), IComparable<ItemContainer>, IEquatable<ItemContainer>
{
	public List<Item> Items { get; set; } = [];
	
	public int CompareTo(ItemContainer other)
	{
		var ret = base.CompareTo(other);
		
		if(ret == 0)
			ret = Items.Count.CompareTo(other?.Items.Count);
		
		return ret;
	}

	public bool Equals(ItemContainer other) => base.Equals(other)
		&& Items == other?.Items;
	
	public override bool Equals(object obj) => Equals(obj as ItemContainer);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Items);
	
	public double TotalWeight()
	{
		var val = Weight;
		Items.ForEach(i => val += i.Weight);
		return val;
	}
}
