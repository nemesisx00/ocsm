using System;

namespace OCSM
{
	public class Die
	{
		public int Sides { get; set; }
		
		public Die(int sides)
		{
			Sides = sides;
		}
		
		public int roll()
		{
			var random = new Random();
			return random.Next(Sides);
		}
	}
}
