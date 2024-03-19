using System;
using System.Collections.Generic;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth.Meta;

public class Featureful() : Metadata(),  IComparable<Featureful>, IEquatable<Featureful>
{
	public List<Feature> Features { get; set; } = [];
	public List<FeatureSection> Sections { get; set; } = [];
	
	public int CompareTo(Featureful other) => base.CompareTo(other);
	
	public bool Equals(Featureful other) => base.Equals(other)
		&& Features == other?.Features
		&& Sections == other?.Sections;
	
	public override bool Equals(object obj) => Equals(obj as Featureful);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Features, Sections);
}
