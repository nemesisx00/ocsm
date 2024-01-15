using System;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Dnd.Fifth;

public class Background() : Featureful(), IComparable<Background>, IEquatable<Background>
{
	public int CompareTo(Background other) => base.CompareTo(other);
	public override bool Equals(object other) => Equals(other as Background);
	public bool Equals(Background other) => base.Equals(other: other);
	public override int GetHashCode() => base.GetHashCode();
}
