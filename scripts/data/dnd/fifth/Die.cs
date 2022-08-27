using System;

namespace OCSM.DnD.Fifth
{
	public class Die : OCSM.Die
	{
		public static Die d4 = new Die(4);
		public static Die d6 = new Die(6);
		public static Die d8 = new Die(8);
		public static Die d10 = new Die(10);
		public static Die d12 = new Die(12);
		public static Die d20 = new Die(20);
		public static Die d100 = new Die(100);
		
		private Die(int sides) : base(sides) { }
		
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
	}
}
