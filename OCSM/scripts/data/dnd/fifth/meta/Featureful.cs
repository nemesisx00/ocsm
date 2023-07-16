using System;
using System.Collections.Generic;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth.Meta
{
	public abstract class Featureful : Metadata,  IComparable<Featureful>, IEquatable<Featureful>
	{
		public List<Feature> Features { get; set; }
		public List<FeatureSection> Sections { get; set; }
		
		public Featureful() : base()
		{
			Features = new List<Feature>();
			Sections = new List<FeatureSection>();
		}
		
		public int CompareTo(Featureful f)
		{
			var ret = 0;
			if(f is Featureful)
			{
				ret = base.CompareTo(f);
			}
			return ret;
		}
		
		public bool Equals(Featureful f)
		{
			return base.Equals(f)
				&& f.Features.Equals(Features)
				&& f.Sections.Equals(Sections);
		}
	}
}
