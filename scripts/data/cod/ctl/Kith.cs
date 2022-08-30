using System;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class Kith : Metadata, IEquatable<Kith>
	{
		public Kith(string name, string description = "") : base(name, description) { }
		
		public bool Equals(Kith kith)
		{
			return kith.Description.Equals(Description)
				&& kith.Name.Equals(Name);
		}
	}
}
