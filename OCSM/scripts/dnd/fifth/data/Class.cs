using System;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Dnd.Fifth;

public class Class() : Featureful(), IComparable<Class>, IEquatable<Class>
{
	public int Level { get; set; }
	public Die HitDie { get; set; }
	
	public int CompareTo(Class other)
	{
		int ret = base.CompareTo(other);
		
		if(ret == 0)
			ret = Level.CompareTo(other?.Level);
		
		if(ret == 0)
			ret = HitDie.CompareTo(other?.HitDie);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as Class);
	
	public bool Equals(Class other) => base.Equals(other)
		&& Level.Equals(other?.Level)
		&& HitDie.Equals(other?.HitDie);
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		Level,
		HitDie
	);
}
