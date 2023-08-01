using System;

namespace Ocsm.Dnd.Fifth;

public class DamageDie : Die, IComparable<DamageDie>, IEquatable<DamageDie>
{
	public enum DamageType
	{
		None = 0,
		Bludgeoning,
		Piercing,
		Slashing,
	}
	
	public DamageType Type { get; set; }
	
	public DamageDie() : base()
	{
		Type = DamageType.None;
	}
	
	public int CompareTo(DamageDie other)
	{
		var ret = 0;
		if(other is DamageDie)
		{
			ret = base.CompareTo(other);
			if(ret.Equals(0))
				ret = Type.CompareTo(other.Type);
		}
		return ret;
	}
	
	public bool Equals(DamageDie other)
	{
		return base.Equals(other)
			&& Type.Equals(other.Type);
	}
}
