using System;

namespace Ocsm;

public class Die() : IComparable<Die>, IEquatable<Die>
{
	public const string StringFormat = "{1}d{0}";
	public const int MinimumSides = 1;
	
	public static int Roll(int sides) => Random.Next(sides);
	
	private static readonly Random Random = new();
	
	public int Sides { get; set; } = MinimumSides;
	
	public int Roll() => Random.Next(Sides);

	public int CompareTo(Die other) => Sides.CompareTo(other?.Sides);
	public override bool Equals(object other) => Equals(other as Die);
	public bool Equals(Die other) => Sides.Equals(other.Sides);
	public override int GetHashCode() => HashCode.Combine(Sides);
	public override string ToString() => string.Format(StringFormat, Sides, string.Empty);
	public string ToString(int quantity) => string.Format(StringFormat, Sides, quantity);
}
