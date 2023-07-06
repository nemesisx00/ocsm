using System;

namespace Ocsm.Dnd.Fifth
{
	public class Die : Ocsm.Die, IComparable<Die>, IEquatable<Die>
	{
		public static Die d4 = new Die() { Sides = 4 };
		public static Die d6 = new Die() { Sides = 6 };
		public static Die d8 = new Die() { Sides = 8 };
		public static Die d10 = new Die() { Sides = 10 };
		public static Die d12 = new Die() { Sides = 12 };
		public static Die d20 = new Die() { Sides = 20 };
		public static Die d100 = new Die() { Sides = 100 };
		
		public Die() : base() { }
		
		public int advantage()
		{
			var one = roll();
			var two = roll();
			return one > two ? one : two;
		}
		
		public int disadvantage()
		{
			var one = roll();
			var two = roll();
			return one < two ? one : two;
		}
		
		public int CompareTo(Die other) { return base.CompareTo(other); }
		public bool Equals(Die other) { return base.Equals(other); }
	}
}
