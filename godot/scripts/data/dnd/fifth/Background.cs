using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth.Meta;

namespace OCSM.DnD.Fifth
{
	public class Background : Featureful, IComparable<Background>, IEquatable<Background>
	{
		public Background() : base() { }
		
		public int CompareTo(Background background) { return base.CompareTo(background); }
		public bool Equals(Background background) { return base.Equals(background); }
	}
}
