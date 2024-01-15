using System;

namespace Ocsm.Dnd.Fifth.Inventory;

public class ItemEquippable() : Item(), IComparable<ItemEquippable>, IEquatable<ItemEquippable>
{
	public bool Equipped { get; set; }
	
	public int CompareTo(ItemEquippable other)
	{
		var ret = base.CompareTo(other);
		
		if(ret.Equals(0))
			ret = Equipped.CompareTo(other?.Equipped);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as ItemEquippable);
	
	public bool Equals(ItemEquippable other) => base.Equals(other)
		&& Equipped.Equals(other.Equipped);
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		Equipped
	);
}
