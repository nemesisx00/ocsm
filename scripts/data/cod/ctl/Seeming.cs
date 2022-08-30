using System;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class Seeming : Metadata, IEquatable<Seeming>
	{
		public Seeming(string name, string description = "") : base(name, description) { }
		public bool Equals(Seeming seeming) { return base.Equals(seeming); }
	}
}
