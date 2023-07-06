using System;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth.Inventory
{
	public class ItemContainer : ItemEquippable, IComparable<ItemContainer>, IEquatable<ItemContainer>
	{
		public List<Item> Items { get; set; }
		
		public ItemContainer() : base()
		{
			Items = new List<Item>();
		}
		
		public int CompareTo(ItemContainer other)
		{
			var ret = base.CompareTo(other);
			if(other is ItemContainer)
			{
				ret = Items.Count.CompareTo(other.Items.Count);
			}
			return ret;
		}
		
		public bool Equals(ItemContainer other)
		{
			return base.Equals(other)
				&& Items.Equals(other.Items);
		}
		
		public double totalWeight()
		{
			var val = Weight;
			Items.ForEach(i => val += i.Weight);
			return val;
		}
	}
}
