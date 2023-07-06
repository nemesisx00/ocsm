using System;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl
{
	public class Kith : Metadata, IEquatable<Kith>
	{
		public Kith() : base() { }
		public bool Equals(Kith kith) { return base.Equals(kith); }
	}
}
