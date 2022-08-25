using System.Collections.Generic;

namespace OCSM
{
	public sealed class Seeming
	{
		public const string Beast = "Beast";
		public const string Darkling = "Darkling";
		public const string Elemental = "Elemental";
		public const string Fairest = "Fairest";
		public const string Ogre = "Ogre";
		public const string Wizened = "Wizened";
		
		public static List<string> asList()
		{
			var list = new List<string>();
			list.Add(Beast);
			list.Add(Darkling);
			list.Add(Elemental);
			list.Add(Fairest);
			list.Add(Ogre);
			list.Add(Wizened);
			return list;
		}
	}
}
