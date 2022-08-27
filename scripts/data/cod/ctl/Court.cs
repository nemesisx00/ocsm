using System.Collections.Generic;

namespace OCSM.CoD.CtL
{
	public sealed class Court
	{
		public const string Autumn = "Autumn";
		public const string Spring = "Spring";
		public const string Summer = "Summer";
		public const string Winter = "Winter";
		public const string Goblin = "Goblin";
		
		public static List<string> asList()
		{
			var list = new List<string>();
			list.Add(Spring);
			list.Add(Summer);
			list.Add(Autumn);
			list.Add(Winter);
			list.Add(Goblin);
			return list;
		}
	}
}
