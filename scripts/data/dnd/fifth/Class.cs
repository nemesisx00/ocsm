using System;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.DnD.Fifth
{
	public class Class : Featureful, IEquatable<Class>
	{
		public int Level { get; set; }
		public Die HitDie { get; set; }
		
		public Class() : base() { }
		
		public Class(string name, Die die) : this()
		{
			Name = name;
			Level = 1;
			HitDie = die;
		}
		
		public bool Equals(Class c)
		{
			return base.Equals(c)
				&& c.Level.Equals(Level)
				&& c.HitDie.Equals(HitDie);
		}
	}
}
