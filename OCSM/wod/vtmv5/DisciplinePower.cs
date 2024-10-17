using System;
using Ocsm.Meta;

namespace Ocsm.Wod.VtmV5;

public class DisciplinePower() : MetadataNumber(), IComparable<DisciplinePower>, IEquatable<DisciplinePower>
{
	public MetadataNumber Amalgam { get; set; }
	public string Cost { get; set; }
	public string DicePools { get; set; }
	public string Duration { get; set; }
	public string System { get; set; }
	
	public int CompareTo(DisciplinePower other)
	{
		var ret = base.CompareTo(other);
		
		if(ret == 0)
			ret = Cost.CompareTo(other?.Cost);
		
		if(ret == 0)
			ret = Duration.CompareTo(other?.Duration);
		
		if(ret == 0)
			ret = DicePools.CompareTo(other?.DicePools);
		
		if(ret == 0)
			ret = Amalgam.CompareTo(other?.Amalgam);
		
		return ret;
	}
	
	public override bool Equals(object obj) => Equals(obj as DisciplinePower);
	
	public bool Equals(DisciplinePower other) => base.Equals(other)
		&& Amalgam == other?.Amalgam
		&& Cost == other?.Cost
		&& DicePools == other?.DicePools
		&& Duration == other?.Duration
		&& System == other?.System;
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		Amalgam,
		Cost,
		DicePools,
		Duration,
		System
	);
}
