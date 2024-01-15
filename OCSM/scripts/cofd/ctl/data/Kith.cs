using System;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl;

public class Kith() : Metadata(), IEquatable<Kith>
{
	public override bool Equals(object other) => base.Equals(other);
	public bool Equals(Kith other) => base.Equals(other);
	public override int GetHashCode() => base.GetHashCode();
}
