using System;
using System.Collections.Generic;

namespace OCSM
{
	public class Changeling : CodCore
	{
		public int Clarity { get; set; }
		public List<Contract> Contracts { get; set; }
		public string Kith { get; set; }
		public string Seeming { get; set; }
		public string Court { get; set; }
		public string Needle { get; set; }
		public string Thread { get; set; }
		public int Wyrd { get; set; }
		
		public Changeling() : base()
		{
			Clarity = 7;
			Contracts = new List<Contract>();
			Kith = String.Empty;
			Seeming = String.Empty;
			Court = String.Empty;
			Needle = String.Empty;
			Thread = String.Empty;
			Wyrd = 1;
		}
	}
}
