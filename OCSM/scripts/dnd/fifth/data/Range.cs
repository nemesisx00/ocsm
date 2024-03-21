using System;

namespace Ocsm.Dnd.Fifth;

public class Range(int shortRange = 5, int longRange = 0) : IComparable<Range>, IEquatable<Range>
{
	public int Short { get; set; } = shortRange;
	public int Long { get; set; } = longRange;
	
	public int CompareTo(Range other)
	{
		var ret = Short.CompareTo(other?.Short);
		
		if(ret == 0)
			ret = Long.CompareTo(other?.Long);
		
		return ret;
	}

	public bool Equals(Range range) => Short == range?.Short && Long == range?.Long;
	public override bool Equals(object obj) => Equals(obj as Range);
	public override int GetHashCode() => HashCode.Combine(Short, Long);
}
