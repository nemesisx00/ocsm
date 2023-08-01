using System.Collections.Generic;

namespace Ocsm.Cofd.Ctl;

public class Changeling : CodCore
{
	public static Dictionary<long, long> WyrdGlamour = new Dictionary<long, long>(10)
	{
		{ 1, 10 },
		{ 2, 11 },
		{ 3, 12 },
		{ 4, 13 },
		{ 5, 15 },
		{ 6, 20 },
		{ 7, 25 },
		{ 8, 30 },
		{ 9, 50 },
		{ 10, 75 }
	};
	
	public List<Contract> Contracts { get; set; }
	public Pair<Regalia, Regalia> FavoredRegalia { get; set; }
	public List<string> Frailties { get; set; }
	public List<string> Touchstones { get; set; }
	
	public Changeling() : base(Constants.GameSystem.Cofd.Changeling)
	{
		Contracts = new List<Contract>();
		FavoredRegalia = new Pair<Regalia, Regalia>();
		Frailties = new List<string>();
		Touchstones = new List<string>();
	}
}
