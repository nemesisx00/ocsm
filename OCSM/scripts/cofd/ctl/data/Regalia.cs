using System;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl;

public class Regalia() : Metadata(), IEquatable<Regalia>
{
	public override bool Equals(object other) => base.Equals(other);
	public bool Equals(Regalia other) => base.Equals(other);
	public override int GetHashCode() => base.GetHashCode();
}
