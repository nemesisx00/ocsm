using System;

namespace Ocsm;

public class Die(int sides = 1) : IComparable<Die>, IEquatable<Die>
{
	private static readonly Random Random = new();

	public int Sides { get; set; } = sides;
	
	public int Roll() => Random.Next(Sides);
	
	public int CompareTo(Die other) => Sides.CompareTo(other?.Sides);
	public bool Equals(Die other) => Sides.Equals(other.Sides);
	public override bool Equals(object obj) => Equals(obj as Die);
	public override int GetHashCode() => HashCode.Combine(Sides);
	public override string ToString() => ToString(null);
	public string ToString(int? quantity) => $"{quantity}d{Sides}";
}
