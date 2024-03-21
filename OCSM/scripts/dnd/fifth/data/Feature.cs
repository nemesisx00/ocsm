using System;
using System.Collections.Generic;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth;

public class Feature() : Metadata(), IComparable<Feature>, IEquatable<Feature>
{
	public string ClassName { get; set; } = string.Empty;
	public List<NumericBonus> NumericBonuses { get; set; } = [];
	public int RequiredLevel { get; set; }
	public List<FeatureSection> Sections { get; set; } = [];
	public string Source { get; set; } = string.Empty;
	public string Text { get; set; } = string.Empty;
	public FeatureTypes FeatureType { get; set; }
	
	public int CompareTo(Feature other)
	{
		var ret = FeatureType.CompareTo(other?.FeatureType);
		
		if(ret == 0)
			ret = RequiredLevel.CompareTo(other?.RequiredLevel);
		
		if(ret == 0)
			ret = Name.CompareTo(other?.Name);
		
		if(ret == 0)
			ret = ClassName.CompareTo(other?.ClassName);
		
		return ret;
	}
	
	public bool Equals(Feature other) => base.Equals(other)
		&& NumericBonuses == other?.NumericBonuses
		&& RequiredLevel == other?.RequiredLevel
		&& Sections == other?.Sections
		&& Source == other?.Source
		&& Text == other?.Text
		&& FeatureType == other?.FeatureType;
	
	public override bool Equals(object obj) => Equals(obj as Feature);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ClassName, NumericBonuses, RequiredLevel, Sections, Source, Text, FeatureType);
}
