using System;

namespace Ocsm.Dnd.Fifth;

public class HitDice(int sides) : IEquatable<HitDice>
{
	public Die Die { get; set; } = new(sides);
	public int Max { get; set; }
	public int Quantity { get; set; }
	
	public int Spend(uint amount = 1)
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
	
	public void Refresh() => Quantity = Max;
	
	public int CompareTo(HitDice other)
	{
		var ret = Die.CompareTo(other?.Die);
		
		if(ret == 0)
			ret = Max.CompareTo(other?.Max);
		
		if(ret == 0)
			ret = Quantity.CompareTo(other?.Quantity);
		
		return ret;
	}
	
	public bool Equals(HitDice other) => Die == other?.Die
		&& Quantity == other?.Quantity
		&& Max == other?.Max;
	
	public override bool Equals(object obj) => Equals(obj as HitDice);
	public override int GetHashCode() => HashCode.Combine(Die, Max, Quantity);
}
