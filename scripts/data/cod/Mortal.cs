using System;

namespace OCSM.CoD
{
	public class Mortal : CodCore
	{
		public long Age { get; set; }
		public long Integrity { get; set; }
		public string Faction { get; set; }
		public string GroupName { get; set; }
		public string Vice { get; set; }
		public string Virtue { get; set; }
		
		public Mortal() : base(Constants.GameSystem.CoD.Mortal)
		{
			Age = -1;
			Integrity = 7;
			Faction = String.Empty;
			GroupName = String.Empty;
			Vice = String.Empty;
			Virtue = String.Empty;
		}
	}
}
