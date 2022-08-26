using System;
using System.Collections.Generic;

namespace OCSM
{
	public class Changeling : CodCore
	{
		public int Clarity { get; set; }
		public List<Contract> Contracts { get; set; }
		public string Court { get; set; }
		public List<string> FavoredRegalia { get; set; }
		public List<string> Frailties { get; set; }
		public int GlamourSpent { get; set; }
		public string Kith { get; set; }
		public string Needle { get; set; }
		public string Seeming { get; set; }
		public string Thread { get; set; }
		public List<string> Touchstones { get; set; }
		public int Wyrd { get; set; }
		
		public Changeling() : base()
		{
			GameSystem = OCSM.GameSystem.Cod.Changeling;
			Clarity = 7;
			Contracts = new List<Contract>();
			Court = String.Empty;
			FavoredRegalia = new List<string>(2);
			Frailties = new List<string>();
			GlamourSpent = 0;
			Kith = String.Empty;
			Needle = String.Empty;
			Seeming = String.Empty;
			Thread = String.Empty;
			Touchstones = new List<string>();
			Wyrd = 1;
		}
	}
}
