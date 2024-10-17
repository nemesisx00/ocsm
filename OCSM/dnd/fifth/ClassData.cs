using System;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth;

public class ClassData() : IComparable<ClassData>, IEquatable<ClassData>
{
	public Metadata Class { get; set; }
	public int Level { get; set; }
	public Die HitDie { get; set; }
	public int HitDieCurrent { get; set; } = 1;
	
	public int CompareTo(ClassData other)
	{
		int ret = Level.CompareTo(other?.Level);
		
		if(ret == 0)
			ret = HitDie.CompareTo(other?.HitDie);
		
		return ret;
	}
	
	public bool Equals(ClassData other) => base.Equals(other)
		&& Class == other?.Class
		&& Level == other?.Level
		&& HitDie == other?.HitDie
		&& HitDieCurrent == other?.HitDieCurrent;
	
	public override bool Equals(object obj) => Equals(obj as ClassData);
	public override int GetHashCode() => HashCode.Combine(Class, Level, HitDie, HitDieCurrent);
}
