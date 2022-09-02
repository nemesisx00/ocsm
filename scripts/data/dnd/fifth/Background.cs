using System;

namespace OCSM.DnD.Fifth
{
	public class Background : Featureful, IEquatable<Background>
	{
		public Background() : base() { }
		
		public bool Equals(Background background)
		{
			return base.Equals(background);
		}
	}
}
