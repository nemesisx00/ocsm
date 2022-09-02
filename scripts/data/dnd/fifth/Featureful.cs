using System;
using System.Collections.Generic;
using OCSM.Meta;

namespace OCSM.DnD.Fifth
{
	public abstract class Featureful : Metadata, IEquatable<Featureful>
	{
		public List<Feature> Features { get; set; }
		
		public Featureful() : base()
		{
			Features = new List<Feature>();
		}
		
		public bool Equals(Featureful f)
		{
			return base.Equals(f)
				&& f.Features.Equals(Features);
		}
	}
}
