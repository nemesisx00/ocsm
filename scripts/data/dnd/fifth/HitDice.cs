using System;

namespace OCSM.DnD.Fifth
{
	public class HitDice : IEquatable<HitDice>
	{
		public Die Die { get; set; }
		public int Quantity { get; set; }
		public int Max { get; set; }
		
		public HitDice(Die die, int max)
		{
			Die = die;
			Quantity = Max = max;
		}
		
		public int spend(uint amount = 1)
		{
			var hp = 0;
			for(var i = 0; i < amount; i++)
			{
				if(Quantity > 0)
				{
					hp += Die.roll();
					Quantity--;
				}
			}
			return hp;
		}
		
		public void refresh()
		{
			Quantity = Max;
		}
		
		public bool Equals(HitDice hitDice)
		{
			return hitDice.Die.Equals(Die)
				&& hitDice.Quantity.Equals(Quantity)
				&& hitDice.Max.Equals(Max);
		}
	}
}
