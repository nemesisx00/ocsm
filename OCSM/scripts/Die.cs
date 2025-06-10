using System;

namespace Ocsm;

public class Die(int sides) : IComparable<Die>, IEquatable<Die>
{
	public const int DefaultHitDieSides = 6;
	
	public static readonly Die D2 = new(2);
	public static readonly Die D4 = new(4);
	public static readonly Die D6 = new(6);
	public static readonly Die D8 = new(8);
	public static readonly Die D10 = new(10);
	public static readonly Die D12 = new(12);
	public static readonly Die D20 = new(20);
	public static readonly Die D100 = new(100);
	
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
