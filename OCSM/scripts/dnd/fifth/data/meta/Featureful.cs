using System;
using System.Collections.Generic;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth.Meta;

public abstract class Featureful() : Metadata(),  IComparable<Featureful>, IEquatable<Featureful>
{
	public List<Feature> Features { get; set; } = [];
	public List<FeatureSection> Sections { get; set; } = [];
	
	public int CompareTo(Featureful other) => base.CompareTo(other);
	
	public override bool Equals(object other) => Equals(other as Featureful);
	
	public bool Equals(Featureful other) => base.Equals(other)
		&& Features.Equals(other?.Features)
		&& Sections.Equals(other?.Sections);
	
	public override int GetHashCode()
	{
		HashCode hash = new();
		hash.Add(base.GetHashCode());
		Features.ForEach(o => hash.Add(o));
		Sections.ForEach(o => hash.Add(o));
		return hash.ToHashCode();
	}
}
