using System;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class Regalia : Metadata, IEquatable<Regalia>
	{
		public Regalia(string name, string description = "") : base(name, description) { }
		
		public bool Equals(Regalia regalia)
		{
			return regalia.Description.Equals(Description)
				&& regalia.Name.Equals(Name);
		}
	}
}
