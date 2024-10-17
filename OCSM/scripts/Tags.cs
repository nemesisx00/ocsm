using System;
using System.Collections.Generic;

namespace Ocsm;

public class Tags() : HashSet<string>(), IComparable<Tags>, IEquatable<Tags>
{
	public int CompareTo(Tags other)
	{
		int ret = 0;
		
		foreach(var tag in this)
		{
			ret = other is not null && other.Contains(tag)
				? 0
				: 1;
			
			if(ret != 0)
				break;
		}
		
		return ret;
	}
	
	public override bool Equals(object obj) => Equals(obj as Tags);
	public bool Equals(Tags other) => this == other;
	public override int GetHashCode() => HashCode.Combine(this);
}
