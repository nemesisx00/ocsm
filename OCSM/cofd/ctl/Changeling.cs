using System.Collections.Generic;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl;

public class Changeling(string name = "") : CofdCore(GameSystemFactory.Name, name)
{
	public readonly static Dictionary<int, int> WyrdGlamour = new(10)
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
	public Pair<Metadata, Metadata> FavoredRegalia { get; set; } = new();
	public List<string> Frailties { get; set; } = [];
	public List<string> Touchstones { get; set; } = [];
}
