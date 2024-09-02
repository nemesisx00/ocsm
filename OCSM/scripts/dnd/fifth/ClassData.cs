using System;

namespace Ocsm.Dnd.Fifth;

public class ClassData() : IComparable<ClassData>, IEquatable<ClassData>
{
	public int Level { get; set; }
	public Die HitDie { get; set; }

	public int CompareTo(ClassData other)
	{
		int ret = Level.CompareTo(other?.Level);
		
		if(ret == 0)
			ret = HitDie.CompareTo(other?.HitDie);
		
		return ret;
	}
	
	public bool Equals(ClassData other) => base.Equals(other)
		&& Level == other?.Level
		&& HitDie == other?.HitDie;
	
	public override bool Equals(object obj) => Equals(obj as ClassData);
	public override int GetHashCode() => HashCode.Combine(Level, HitDie);
}
