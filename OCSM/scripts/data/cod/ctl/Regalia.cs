using System;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class Regalia : Metadata, IEquatable<Regalia>
	{
		public Regalia() : base() { }
		public bool Equals(Regalia regalia) { return base.Equals(regalia); }
	}
}
