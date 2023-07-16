using System;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Dnd.Fifth
{
	public class Race : Featureful, IComparable<Race>, IEquatable<Race>
	{
		public Race() : base() { }
		
		public int CompareTo(Race race) { return base.CompareTo(race); }
		public bool Equals(Race race) { return base.Equals(race); }
	}
}
