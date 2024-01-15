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
	
	public override bool Equals(object other) => Equals(other as ItemContainer);
	
	public bool Equals(ItemContainer other) => base.Equals(other)
		&& Items.Equals(other.Items);
	
	public override int GetHashCode()
	{
		HashCode hash = new();
		hash.Add(base.GetHashCode());
		Items.ForEach(i => hash.Add(i));
		return hash.ToHashCode();
	}
	
	public double TotalWeight()
	{
		var val = Weight;
		Items.ForEach(i => val += i.Weight);
		return val;
	}
}
