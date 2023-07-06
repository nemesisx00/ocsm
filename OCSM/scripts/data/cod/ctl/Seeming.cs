using System;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl
{
	public class Seeming : Metadata, IEquatable<Seeming>
	{
		public Seeming() : base() { }
		public bool Equals(Seeming seeming) { return base.Equals(seeming); }
	}
}
