using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.DnD.Fifth
{
	public class Race : Featureful, IComparable<Race>, IEquatable<Race>
	{
		public Race() : base() { }
		
		public int CompareTo(Race race) { return base.CompareTo(race); }
		public bool Equals(Race race) { return base.Equals(race); }
	}
}
