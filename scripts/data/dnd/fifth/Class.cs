using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.DnD.Fifth
{
	public class Class : Featureful, IComparable<Class>, IEquatable<Class>
	{
		public int Level { get; set; }
		public Die HitDie { get; set; }
		
		public Class() : base()
		{
			Level = 0;
			HitDie = null;
		}
		
		public Class(Die die) : base()
		{
			Level = 1;
			HitDie = die;
		}
		
		public Class(Die die, string name, string description) : base(name, description)
		{
			Level = 1;
			HitDie = die;
		}
		
		public Class(Die die, string name, string description, List<FeatureSection> sections, List<Feature> features) : base(name, description, sections, features)
		{
			Level = 1;
			HitDie = die;
		}
		
		public int CompareTo(Class c) { return base.CompareTo(c); }
		
		public bool Equals(Class c)
		{
			return base.Equals(c)
				&& c.Level.Equals(Level)
				&& c.HitDie.Equals(HitDie);
		}
	}
}
