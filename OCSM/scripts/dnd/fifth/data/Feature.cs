using System;
using System.Collections.Generic;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth;

public class Feature() : Metadata(), IComparable<Feature>, IEquatable<Feature>
{
	public string ClassName { get; set; }
	public List<NumericBonus> NumericBonuses { get; set; } = [];
	public int RequiredLevel { get; set; }
	public List<FeatureSection> Sections { get; set; } = [];
	public string Source { get; set; }
	public string Text { get; set; }
	public FeatureTypes Type { get; set; }
	
	public int CompareTo(Feature other)
	{
		var ret = Type.CompareTo(other?.Type);
		
		if(ret == 0)
			ret = RequiredLevel.CompareTo(other?.RequiredLevel);
		
		if(ret == 0)
			ret = Name.CompareTo(other?.Name);
		
		if(ret == 0)
			ret = ClassName.CompareTo(other?.ClassName);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as Feature);
	
	public bool Equals(Feature other) => base.Equals(other)
		&& NumericBonuses.Equals(other?.NumericBonuses)
		&& RequiredLevel.Equals(other?.RequiredLevel)
		&& Sections.Equals(other?.Sections)
		&& Source.Equals(other?.Source)
		&& Text.Equals(other?.Text)
		&& Type.Equals(other?.Type);
	
	public override int GetHashCode()
	{
		HashCode hash = new();
		hash.Add(base.GetHashCode());
		hash.Add(ClassName);
		NumericBonuses.ForEach(o => hash.Add(o));
		hash.Add(RequiredLevel);
		Sections.ForEach(o => hash.Add(o));
		hash.Add(Source);
		hash.Add(Text);
		hash.Add(Type);
		return hash.ToHashCode();
	}
}
