using System;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl;

public class Seeming() : Metadata(), IEquatable<Seeming>
{
	public override bool Equals(object other) => base.Equals(other);
	public bool Equals(Seeming other) => base.Equals(other);
	public override int GetHashCode() => base.GetHashCode();
}
