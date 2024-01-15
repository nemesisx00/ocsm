using System;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl;

public class Court() : Metadata(), IEquatable<Court>
{
	public override bool Equals(object other) => base.Equals(other);
	public bool Equals(Court other) => base.Equals(other);
	public override int GetHashCode() => base.GetHashCode();
}
