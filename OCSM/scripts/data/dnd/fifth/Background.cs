using System;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Dnd.Fifth
{
	public class Background : Featureful, IComparable<Background>, IEquatable<Background>
	{
		public Background() : base() { }
		
		public int CompareTo(Background background) { return base.CompareTo(background); }
		public bool Equals(Background background) { return base.Equals(background); }
	}
}
