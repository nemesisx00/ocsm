using System;

namespace OCSM.DnD.Fifth
{
	public class Race : Featureful, IEquatable<Race>
	{
		public Race() : base() { }
		
		public bool Equals(Race race)
		{
			return base.Equals(race);
		}
	}
}
