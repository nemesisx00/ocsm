using System;
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Dnd.Fifth;

public class Race() : Featureful(), IComparable<Race>, IEquatable<Race>
{
	public int CompareTo(Race other) => base.CompareTo(other);
	public override bool Equals(object other) => Equals(other as Race);
	public bool Equals(Race other) => base.Equals(other);
	public override int GetHashCode() => base.GetHashCode();
}
