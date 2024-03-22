using System;

namespace Ocsm.Dnd.Fifth.Inventory;

public class ItemEquippable() : Item(), IComparable<ItemEquippable>, IEquatable<ItemEquippable>
{
	public bool Equipped { get; set; }
	
	public int CompareTo(ItemEquippable other)
	{
		var ret = base.CompareTo(other);
		
		if(ret == 0)
			ret = Equipped.CompareTo(other?.Equipped);
		
		return ret;
	}
	
	public bool Equals(ItemEquippable other) => base.Equals(other) && Equipped == other?.Equipped;
	public override bool Equals(object obj) => Equals(obj as ItemEquippable);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Equipped);
}
