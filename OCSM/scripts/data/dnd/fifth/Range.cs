using System;

namespace Ocsm.Dnd.Fifth
{
	public class Range : IComparable<Range>, IEquatable<Range>
	{
		public static Range Melee = new Range() { Short = 5, Long = 0};
		
		public int Short { get; set; }
		public int Long { get; set; }
		
		public Range()
		{
			Short = 5;
			Long = 0;
		}
		
		public int CompareTo(Range other)
		{
			var ret = 0;
			if(other is Range)
			{
				ret = Short.CompareTo(other.Short);
				if(ret.Equals(0))
					ret = Long.CompareTo(other.Long);
			}
			return ret;
		}
		
		public bool Equals(Range range)
		{
			return Short.Equals(range.Short)
				&& Long.Equals(range.Long);
		}
	}
}
