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
		public int Action { get; set; }
		public Attribute Attribute { get; set; }
		public Attribute AttributeResisted { get; set; }
		public Attribute AttributeContested { get; set; }
		public string Cost { get; set; }
		public string Description { get; set; }
		public string Duration { get; set; }
		public string Effects { get; set; }
		public string Loophole { get; set; }
		public string Name { get; set; }
		public string Regalia { get; set; }
		public string RegaliaType { get; set; }
		public string RollSuccess { get; set; }
		public string RollSuccessExceptional { get; set; }
		public string RollFailure { get; set; }
		public string RollFailureExceptional { get; set; }
		public Dictionary<string, string> SeemingBenefits { get; set; }
		public Skill Skill { get; set; }
	}
}
