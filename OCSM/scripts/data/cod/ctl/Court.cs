using System;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class Court : Metadata, IEquatable<Court>
	{
		public Court() : base() { }
		public bool Equals(Court court) { return base.Equals(court); }
	}
}
