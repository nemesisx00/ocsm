using System;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl
{
	public class Regalia : Metadata, IEquatable<Regalia>
	{
		public Regalia() : base() { }
		public bool Equals(Regalia regalia) { return base.Equals(regalia); }
	}
}
