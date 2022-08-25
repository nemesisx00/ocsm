using System;
using System.Collections.Generic;

namespace OCSM
{
	public class Mortal : CodCore
	{
		public int Age { get; set; }
		public int Integrity { get; set; }
		public string Faction { get; set; }
		public string GroupName { get; set; }
		public string Vice { get; set; }
		public string Virtue { get; set; }
		
		public Mortal() : base()
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
