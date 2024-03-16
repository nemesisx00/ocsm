using System;

namespace Ocsm.Dnd.Fifth;

public class HitDice : IEquatable<HitDice>
{
	public Die Die { get; set; }
	public int Quantity { get; set; }
	public int Max { get; set; }
	
	public HitDice()
	{
		Die = new Die();
		Quantity = Max = 0;
	}
	
	public int spend(uint amount = 1)
	{
		var hp = 0;
		for(var i = 0; i < amount; i++)
		{
			if(Quantity > 0)
			{
				hp += Die.Roll();
				Quantity--;
			}
		}
		return hp;
	}
	
	public void refresh()
	{
		Quantity = Max;
	}
	
	public int CompareTo(HitDice other)
	{
		var ret = 0;
		if(other is HitDice)
		{
			ret = Die.CompareTo(other.Die);
			if(ret.Equals(0))
				ret = Max.CompareTo(other.Max);
			if(ret.Equals(0))
				ret = Quantity.CompareTo(other.Quantity);
		}
		return ret;
	}
	
	public bool Equals(HitDice other)
	{
		return Die.Equals(other.Die)
			&& Quantity.Equals(other.Quantity)
			&& Max.Equals(other.Max);
	}
}
