using System;
using System.Collections.Generic;
using OCSM.Meta;

namespace OCSM.DnD.Fifth.Meta
{
	public abstract class Featureful : Metadata, IEquatable<Featureful>
	{
		public List<Feature> Features { get; set; }
		
		public Featureful() : base()
		{
			Features = new List<Feature>();
		}
		
		public Featureful(string name, string description) : base(name, description)
		{
			Features = new List<Feature>();
		}
		
		public Featureful(string name, string description, List<Feature> features) : base(name, description)
		{
			Features = new List<Feature>(features);
		}
		
		public bool Equals(Featureful f)
		{
			return base.Equals(f)
				&& f.Features.Equals(Features);
		}
	}
}
