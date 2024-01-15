using System;

namespace Ocsm.Dnd.Fifth;

public class Die() : Ocsm.Die(), IComparable<Die>, IEquatable<Die>
{
	public static readonly Die Four = new() { Sides = 4 };
	public static readonly Die Six = new() { Sides = 6 };
	public static readonly Die Eight = new() { Sides = 8 };
	public static readonly Die Ten = new() { Sides = 10 };
	public static readonly Die Twelve = new() { Sides = 12 };
	public static readonly Die Twenty = new() { Sides = 20 };
	public static readonly Die OneHundred = new() { Sides = 100 };
	
	public int Advantage()
	{
		var one = Roll();
		var two = Roll();
		return one > two ? one : two;
	}
	
	public int Disadvantage()
	{
		var one = Roll();
		var two = Roll();
		return one < two ? one : two;
	}

	public int CompareTo(Die other) => base.CompareTo(other);
	public override bool Equals(object other) => Equals(other as Die);
	public bool Equals(Die other) => base.Equals(other: other);
	public override int GetHashCode() => base.GetHashCode();
}
