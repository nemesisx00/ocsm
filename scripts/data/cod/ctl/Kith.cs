using System.Collections.Generic;

namespace OCSM.CoD.CtL
{
	public sealed class Kith
	{
		public const string Artist = "Artist";
		public const string BrightOne = "Bright One";
		public const string Chatelaine = "Chatelaine";
		public const string Gristlegrinder = "Gristlegrinder";
		public const string Helldiver = "Helldiver";
		public const string Hunterheart = "Hunterheart";
		public const string Leechfinger = "Leechfinger";
		public const string Mirrorskin = "Mirrorskin";
		public const string Nightsinger = "Nightsinger";
		public const string Notary = "Notary";
		public const string Playmate = "Playmate";
		public const string Snowskin = "Snowskin";
		
		public static List<string> asList()
		{
			var list = new List<string>();
			list.Add(Artist);
			list.Add(BrightOne);
			list.Add(Chatelaine);
			list.Add(Gristlegrinder);
			list.Add(Helldiver);
			list.Add(Hunterheart);
			list.Add(Leechfinger);
			list.Add(Mirrorskin);
			list.Add(Nightsinger);
			list.Add(Notary);
			list.Add(Playmate);
			list.Add(Snowskin);
			return list;
		}
	}
}
