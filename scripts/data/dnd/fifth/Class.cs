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
		
		public int CompareTo(Class c) { return base.CompareTo(c); }
		
		public bool Equals(Class c)
		{
			return base.Equals(c)
				&& c.Level.Equals(Level)
				&& c.HitDie.Equals(HitDie);
		}
	}
}
