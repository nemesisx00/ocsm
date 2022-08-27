using System.Collections.Generic;

namespace OCSM.CoD.CtL
{
	public class Contract
	{
		public int Action { get; set; } = 0;
		public Attribute Attribute { get; set; }
		public Attribute AttributeResisted { get; set; }
		public Attribute AttributeContested { get; set; }
		public string ContractType { get; set; }
		public string Cost { get; set; }
		public string Description { get; set; }
		public string Duration { get; set; }
		public string Effects { get; set; }
		public string Loophole { get; set; }
		public string Name { get; set; }
		public string Regalia { get; set; }
		public string RollSuccess { get; set; }
		public string RollSuccessExceptional { get; set; }
		public string RollFailure { get; set; }
		public string RollFailureDramatic { get; set; }
		public Dictionary<string, string> SeemingBenefits { get; set; } = new Dictionary<string, string>();
		public Skill Skill { get; set; }
	}
	
	public sealed class ContractType
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
