using System;
using Ocsm.Meta;

namespace Ocsm.Wod.VtmV5;

public class Discipline() : MetadataNumber(), IComparable<Discipline>, IEquatable<Discipline>
{
	public Trait.Type Category { get; set; }
	public string Resonance { get; set; }
	public BloodResonance ResonanceType { get; set; }
	public string Threat { get; set; }
	
	public int CompareTo(Discipline other)
	{
		var ret = base.CompareTo(other);
		
		if(ret == 0)
			ret = ResonanceType.CompareTo(other?.ResonanceType);
		
		if(ret == 0)
			ret = Category.CompareTo(other?.Category);
		
		return ret;
	}
	
	public override bool Equals(object obj) => Equals(obj as Discipline);
	
	public bool Equals(Discipline other) => base.Equals(other)
		&& Category == other?.Category
		&& Resonance == other?.Resonance
		&& ResonanceType == other?.ResonanceType
		&& Threat == other?.Threat;
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		Category,
		Resonance,
		ResonanceType,
		Threat
	);
}
