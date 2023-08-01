using System;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth.Inventory;

public class ItemEquippable : Item, IComparable<ItemEquippable>, IEquatable<ItemEquippable>
{
	public bool Equipped { get; set; }
	
	public ItemEquippable() : base()
	{
		Equipped = false;
	}
	
	public int CompareTo(ItemEquippable other)
	{
		var ret = base.CompareTo(other);
		if(other is ItemEquippable)
		{
			if(ret.Equals(0))
				ret = Equipped.CompareTo(other.Equipped);
		}
		return ret;
	}
	
	public bool Equals(ItemEquippable other)
	{
		return base.Equals(other)
			&& Equipped.Equals(other.Equipped);
	}
}
