using System;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class Court : Metadata, IEquatable<Court>
	{
		public Court(string name, string description = "") : base(name, description) { }
		
		public bool Equals(Court court)
		{
			return court.Description.Equals(Description)
				&& court.Name.Equals(Name);
		}
	}
}
