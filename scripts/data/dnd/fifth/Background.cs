using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.DnD.Fifth
{
	public class Background : Featureful, IEquatable<Background>
	{
		public Background() : base() { }
		public Background(string name, string description) : base(name, description) { }
		public Background(string name, string description, List<FeatureSection> sections, List<Feature> features) : base(name, description, sections, features) { }
		
		public bool Equals(Background background)
		{
			return base.Equals(background);
		}
	}
}
