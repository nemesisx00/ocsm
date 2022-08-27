using System.Collections.Generic;

namespace OCSM.CoD.CtL
{
	public sealed class Regalia
	{
		public const string Crown = "Crown";
		public const string Jewels = "Jewels";
		public const string Mirror = "Mirror";
		public const string Shield = "Shield";
		public const string Steed = "Steed";
		public const string Sword = "Sword";
		
		public static List<string> asList()
		{
			var output = new List<string>();
			output.Add(Crown);
			output.Add(Jewels);
			output.Add(Mirror);
			output.Add(Shield);
			output.Add(Steed);
			output.Add(Sword);
			return output;
		}
	}
}
