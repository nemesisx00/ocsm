using System;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class Seeming : Metadata, IEquatable<Seeming>
	{
		public Seeming() : base() { }
		public bool Equals(Seeming seeming) { return base.Equals(seeming); }
	}
}
