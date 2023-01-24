using System;

namespace OCSM
{
	public class Die : IComparable<Die>, IEquatable<Die>
	{
		public int Sides { get; set; }
		
		public Die()
		{
			Sides = 1;
		}
		
		public int roll()
		{
			var random = new Random();
			return random.Next(Sides);
		}
		
		public int CompareTo(Die other)
		{
			var ret = 0;
			if(other is Die)
			{
				ret = Sides.CompareTo(other.Sides);
			}
			return ret;
		}
		
		public bool Equals(Die other)
		{
			return Sides.Equals(other.Sides);
		}
	}
}
