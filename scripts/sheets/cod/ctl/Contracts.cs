using System.Collections.Generic;

namespace OCSM
{
	public sealed class Contracts
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
		
		public sealed class Court
		{
			public const string Spring = "Spring";
			public const string Summer = "Summer";
			public const string Autumn = "Autumn";
			public const string Winter = "Winter";
			public const string Goblin = "Goblin";
			
			public static List<string> asList()
			{
				var output = new List<string>();
				output.Add(Spring);
				output.Add(Summer);
				output.Add(Autumn);
				output.Add(Winter);
				output.Add(Goblin);
				return output;
			}
		}
		
		public sealed class Type
		{
			public const string Common = "Common";
			public const string Royal = "Royal";
			
			public static List<string> asList()
			{
				var output = new List<string>();
				output.Add(Common);
				output.Add(Royal);
				return output;
			}
		}
	}
	
	public class Contract : Godot.Object
	{
		public string Action { get; set; }
		public string Cost { get; set; }
		public string Description { get; set; }
		public string Regalia { get; set; }
		public string RegaliaType { get; set; }
		public Attribute PoolAttribute { get; set; }
		public Skill PoolSkill { get; set; }
		public bool PoolWyrd { get; set; }
		public string Duration { get; set; }
		public string Effects { get; set; }
		public string Requirements { get; set; }
	}
}
