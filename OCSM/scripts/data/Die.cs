using System;

namespace Ocsm;

public class Die : IComparable<Die>, IEquatable<Die>
{
	public const string StringFormat = "{1}d{0}";
	
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
	
	public bool Equals(Die other) { return Sides.Equals(other.Sides); }
	public override string ToString() { return String.Format(StringFormat, Sides, String.Empty); }
	public string ToString(int quantity) { return String.Format(StringFormat, Sides, quantity); }
}
