using System;

namespace Ocsm.Dnd.Fifth;

public class DamageDie() : Die(), IComparable<DamageDie>, IEquatable<DamageDie>
{
	public DamageTypes DamageType { get; set; }
	
	public int CompareTo(DamageDie other)
	{
		var ret = base.CompareTo(other);
		
		if(ret == 0)
			ret = DamageType.CompareTo(other?.DamageType);
		
		return ret;
	}
	
	public bool Equals(DamageDie other) => base.Equals(other) && DamageType == other?.DamageType;
	public override bool Equals(object obj) => Equals(obj as DamageDie);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), DamageType);
}
