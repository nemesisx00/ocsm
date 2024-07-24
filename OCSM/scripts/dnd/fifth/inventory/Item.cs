using System;
using System.Collections.Generic;
using System.Linq;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth.Inventory;

/**
<summary>
<para>
Generic data structure defining every aspect of an individual item.
</para>
<para>
As it is theoretically possible for a single item to function as armor, a
container, and a weapon simultaneously, the data related to specific subtypes is
composed here. An item qualifies as a subtype if that subtype's data is not null.
</para>
</summary>
*/
public class Item() : Metadata(), IComparable<Item>, IEquatable<Item>
{
	/**
	<summary>
	The metadata specific to armor.
	</summary>
	<remarks>
	If not null, this item is considered armor.
	</remarks>
	*/
	public ArmorData ArmorData { get; set; }
	
	public int Cost { get; set; }
	
	/**
	<summary>
	Whether or not the item is currently equipped.
	</summary>
	<remarks>
	If not null, this item is considered equippable.
	</remarks>
	*/
	public bool? Equipped { get; set; } = null;
	
	/**
	<summary>
	The list of contained items.
	</summary>
	<remarks>
	If not null, this item is considered a container.
	</remarks>
	*/
	public List<Item> Items { get; set; }
	
	/**
	<summary>
	The metadata specific to weapons.
	</summary>
	<remarks>
	If not null, this item is considered a weapon.
	</remarks>
	*/
	public WeaponData WeaponData { get; set; }
	
	public double Weight { get; set; }
	
	public int CompareTo(Item other)
	{
		var ret = base.CompareTo(other);
		
		if(ret == 0)
			ret = Cost.CompareTo(other?.Cost);
		
		if(ret == 0)
			ret = Equipped is not null
				? ((bool)Equipped).CompareTo(other?.Equipped)
				: other?.Equipped is not null ? 1 : 0;
		
		if(ret == 0)
			ret = Items is not null
				? Items.Count.CompareTo(other?.Items.Count)
				: other?.Items is not null ? 1 : 0;
		
		if(ret == 0)
			ret = Weight.CompareTo(other?.Weight);
		
		return ret;
	}
	
	public bool Equals(Item other) => base.Equals(other)
		&& ArmorData == other?.ArmorData
		&& Cost == other?.Cost
		&& Equipped == other?.Equipped
		&& Items == other?.Items
		&& WeaponData == other?.WeaponData
		&& Weight == other?.Weight;
	
	public override bool Equals(object obj) => Equals(obj as Item);
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		ArmorData,
		Cost,
		Equipped,
		Items,
		WeaponData,
		Weight
	);
	
	/**
	<summary>
	The total weight of this item including its contents, if it is a container.
	</summary>
	<returns>
	The total weight of the item.
	</returns>
	*/
	public double TotalWeight() => Items?.Aggregate(Weight, (acc, i) => acc + i.Weight)
		?? Weight;
}
