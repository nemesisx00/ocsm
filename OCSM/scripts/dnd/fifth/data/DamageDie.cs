using System;

namespace Ocsm.Dnd.Fifth;

public class DamageDie() : Die(), IComparable<DamageDie>, IEquatable<DamageDie>
{
	public enum DamageType
	{
		None = 0,
		Bludgeoning,
		Piercing,
		Slashing,
	}
	
	public DamageType Type { get; set; }
	
	public int CompareTo(DamageDie other)
	{
		var ret = base.CompareTo(other);
		
		if(ret == 0)
			ret = Type.CompareTo(other?.Type);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as DamageDie);
	
	public bool Equals(DamageDie other) => base.Equals(other)
		&& Type.Equals(other.Type);
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		Type
	);
}
