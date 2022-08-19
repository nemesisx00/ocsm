using System;
using System.Collections.Generic;

namespace OCSM
{
	public class Mortal : CodCore
	{
		public int Integrity { get; set; }
		public string Faction { get; set; }
		public string GroupName { get; set; }
		public string Vice { get; set; }
		public string Virtue { get; set; }
		
		public Mortal() : base()
		{
			Integrity = 7;
			Faction = String.Empty;
			GroupName = String.Empty;
			Vice = String.Empty;
			Virtue = String.Empty;
		}

		public override string ToString()
		{
			var output = "{ ";
			output += "Core: " + base.ToString() + ", ";
			output += "Faction: '" + Faction + "', ";
			output += "GroupName: '" + GroupName + "', ";
			output += "Vice: '" + Vice + "', ";
			output += "Virtue: '" + Virtue + "', ";
			output += "Integrity: " + Integrity + ", ";
			output += " }";
			return output;
		}
	}
}
