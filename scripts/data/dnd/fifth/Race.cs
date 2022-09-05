using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.DnD.Fifth
{
	public class Race : Featureful, IEquatable<Race>
	{
		public Race() : base() { }
		public Race(string name, string description) : base(name, description) { }
		public Race(string name, string description, List<FeatureSection> sections, List<Feature> features) : base(name, description, sections, features) { }
		
		public bool Equals(Race race)
		{
			return base.Equals(race);
		}
	}
}
