using System;
using System.Collections.Generic;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth;

public class Feature() : Metadata(), IComparable<Feature>, IEquatable<Feature>
{
	public FeatureTypes FeatureType { get; set; }
	public List<NumericBonus> NumericBonuses { get; set; } = [];
	public int RequiredLevel { get; set; }
	public List<FeatureSection> Sections { get; set; } = [];
	public string Source { get; set; } = string.Empty;
	public Tags Tags { get; set; } = [];
	public string Text { get; set; }
	
	public int CompareTo(Feature other)
	{
		var ret = FeatureType.CompareTo(other?.FeatureType);
		
		if(ret == 0)
			ret = RequiredLevel.CompareTo(other?.RequiredLevel);
		
		if(ret == 0)
			ret = Name.CompareTo(other?.Name);
		
		if(ret == 0)
		{
			ret = !string.IsNullOrEmpty(Source)
				? Source.CompareTo(other?.Source)
				: 1;
		}
		
		if(ret == 0)
			ret = Tags.CompareTo(other?.Tags);
		
		return ret;
	}
	
	public bool Equals(Feature other) => base.Equals(other)
		&& FeatureType == other?.FeatureType
		&& NumericBonuses == other?.NumericBonuses
		&& RequiredLevel == other?.RequiredLevel
		&& Sections == other?.Sections
		&& Source == other?.Source
		&& Tags == other?.Tags
		&& Text == other?.Text;
	
	public override bool Equals(object obj) => Equals(obj as Feature);
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		FeatureType,
		NumericBonuses,
		RequiredLevel,
		Sections,
		Source,
		Tags,
		Text
	);
}
