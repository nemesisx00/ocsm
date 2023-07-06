using System;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl
{
	public class Court : Metadata, IEquatable<Court>
	{
		public Court() : base() { }
		public bool Equals(Court court) { return base.Equals(court); }
	}
}
