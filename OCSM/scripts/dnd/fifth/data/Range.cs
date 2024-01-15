using System;

namespace Ocsm.Dnd.Fifth;

public class Range(int maxShort = default, int maxLong = default) : IComparable<Range>, IEquatable<Range>
{
	public static readonly Range Melee = new(5, 0);
	
	public int Short { get; set; } = maxShort;
	public int Long { get; set; } = maxLong;
	
	public int CompareTo(Range other)
	{
		var ret = Short.CompareTo(other?.Short);
		
		if(ret == 0)
			ret = Long.CompareTo(other?.Long);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as Range);
	
	public bool Equals(Range other) => Short.Equals(other?.Short)
		&& Long.Equals(other?.Long);

	public override int GetHashCode() => HashCode.Combine(Short, Long);
}
