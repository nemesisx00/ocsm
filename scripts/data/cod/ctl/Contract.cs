using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using OCSM.API;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class Contract : Metadata, IEmptiable, IEquatable<Contract>
	{
		public int Action { get; set; }
		public Attribute Attribute { get; set; }
		public Attribute AttributeResisted { get; set; }
		public Attribute AttributeContested { get; set; }
		public ContractType ContractType { get; set; }
		public string Cost { get; set; }
		public string Duration { get; set; }
		public string Effects { get; set; }
		public string Loophole { get; set; }
		public Regalia Regalia { get; set; }
		public string RollSuccess { get; set; }
		public string RollSuccessExceptional { get; set; }
		public string RollFailure { get; set; }
		public string RollFailureDramatic { get; set; }
		public Dictionary<string, string> SeemingBenefits { get; set; }
		public Skill Skill { get; set; }
		
		[JsonIgnore]
		public bool Empty
		{
			get
			{
				return Action < 1
					&& !(Attribute is OCSM.CoD.Attribute)
					&& !(AttributeContested is OCSM.CoD.Attribute)
					&& !(AttributeResisted is OCSM.CoD.Attribute)
					&& !(ContractType is ContractType)
					&& String.IsNullOrEmpty(Description)
					&& String.IsNullOrEmpty(Effects)
					&& String.IsNullOrEmpty(Loophole)
					&& String.IsNullOrEmpty(Name)
					&& !(Regalia is Regalia)
					&& String.IsNullOrEmpty(RollFailure)
					&& String.IsNullOrEmpty(RollFailureDramatic)
					&& String.IsNullOrEmpty(RollSuccess)
					&& String.IsNullOrEmpty(RollSuccessExceptional)
					&& SeemingBenefits.Count < 1;
			}
		}
		
		public Contract() : base()
		{
			Action = 0;
			Attribute = null;
			AttributeResisted = null;
			AttributeContested = null;
			ContractType = null;
			Cost = String.Empty;
			Duration = String.Empty;
			Effects = String.Empty;
			Loophole = String.Empty;
			Regalia = null;
			RollSuccess = String.Empty;
			RollSuccessExceptional = String.Empty;
			RollFailure = String.Empty;
			RollFailureDramatic = String.Empty;
			SeemingBenefits = new Dictionary<string, string>();
			Skill = null;
		}
		
		public bool Equals(Contract contract)
		{
			return base.Equals(contract)
				&& contract.Action.Equals(Action)
				&& Logic.AreEqualOrNull<Attribute>(contract.Attribute, Attribute)
				&& Logic.AreEqualOrNull<Attribute>(contract.AttributeResisted, AttributeResisted)
				&& Logic.AreEqualOrNull<Attribute>(contract.AttributeContested, AttributeContested)
				&& Logic.AreEqualOrNull<ContractType>(contract.ContractType, ContractType)
				&& contract.Cost.Equals(Cost)
				&& contract.Duration.Equals(Duration)
				&& contract.Effects.Equals(Effects)
				&& contract.Loophole.Equals(Loophole)
				&& Logic.AreEqualOrNull<Regalia>(contract.Regalia, Regalia)
				&& contract.RollFailure.Equals(RollFailure)
				&& contract.RollFailureDramatic.Equals(RollFailureDramatic)
				&& contract.RollSuccess.Equals(RollSuccess)
				&& contract.RollSuccessExceptional.Equals(RollSuccessExceptional)
				&& contract.SeemingBenefits.Equals(SeemingBenefits)
				&& Logic.AreEqualOrNull<Skill>(contract.Skill, Skill);
		}
	}
}
