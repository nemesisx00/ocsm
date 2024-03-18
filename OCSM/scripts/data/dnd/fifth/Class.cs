using System;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Dnd.Fifth;

public class Class() : Featureful(), IComparable<Class>, IEquatable<Class>
{
	public int Level { get; set; }
	public Die HitDie { get; set; }

	public int CompareTo(Class other) => base.CompareTo(other);

	public bool Equals(Class other) => base.Equals(other)
		&& Level == other?.Level
		&& HitDie == other?.HitDie;
	
	public override bool Equals(object obj) => Equals(obj as Class);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Level, HitDie);
}
