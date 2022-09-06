using System;
using System.Collections.Generic;

namespace OCSM.CoD.CtL
{
	public class Changeling : CodCore
	{
		public int Clarity { get; set; }
		public List<Contract> Contracts { get; set; }
		public Court Court { get; set; }
		public List<Regalia> FavoredRegalia { get; set; }
		public List<string> Frailties { get; set; }
		public int GlamourSpent { get; set; }
		public Kith Kith { get; set; }
		public string Needle { get; set; }
		public Seeming Seeming { get; set; }
		public string Thread { get; set; }
		public List<string> Touchstones { get; set; }
		public int Wyrd { get; set; }
		
		public Changeling() : base(OCSM.GameSystem.CoD.Changeling)
		{
			Clarity = 7;
			Contracts = new List<Contract>();
			Court = null;
			FavoredRegalia = new List<Regalia>();
			Frailties = new List<string>();
			GlamourSpent = 0;
			Kith = null;
			Needle = String.Empty;
			Seeming = null;
			Thread = String.Empty;
			Touchstones = new List<string>();
			Wyrd = 1;
		}
	}
}
