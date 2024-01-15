using System.Collections.Generic;

namespace Ocsm.Cofd.Ctl;

public class Changeling() : CofdCore(Constants.GameSystem.Cofd.Changeling)
{
	public static readonly Dictionary<int, int> WyrdGlamour = new(10)
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
	
	public List<Contract> Contracts { get; set; } = [];
	public Pair<Regalia, Regalia> FavoredRegalia { get; set; } = default;
	public List<string> Frailties { get; set; } = [];
	public List<string> Touchstones { get; set; } = [];
}
