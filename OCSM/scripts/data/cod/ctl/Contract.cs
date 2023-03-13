using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using OCSM.API;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class Contract : Metadata, IComparable<Contract>, IEmptiable, IEquatable<Contract>
	{
		public string Action { get; set; }
		public Attribute.Enum? Attribute { get; set; }
		public Attribute.Enum? AttributeResisted { get; set; }
		public Attribute.Enum? AttributeContested { get; set; }
		public ContractType ContractType { get; set; }
		public string Cost { get; set; }
		public string Duration { get; set; }
		public string Effects { get; set; }
		public string Loophole { get; set; }
		public ContractRegalia Regalia { get; set; }
		public string RollSuccess { get; set; }
		public string RollSuccessExceptional { get; set; }
		public string RollFailure { get; set; }
		public string RollFailureDramatic { get; set; }
		public Dictionary<string, string> SeemingBenefits { get; set; }
		public Skill.Enum? Skill { get; set; }
		
		[JsonIgnore]
		public bool Empty
		{
			get
			{
				return String.IsNullOrEmpty(Action)
					&& Attribute is null
					&& AttributeContested is null
					&& AttributeResisted is null
					&& !(ContractType is ContractType)
					&& String.IsNullOrEmpty(Description)
					&& String.IsNullOrEmpty(Effects)
					&& String.IsNullOrEmpty(Loophole)
					&& String.IsNullOrEmpty(Name)
					&& !(Regalia is ContractRegalia)
					&& String.IsNullOrEmpty(RollFailure)
					&& String.IsNullOrEmpty(RollFailureDramatic)
					&& String.IsNullOrEmpty(RollSuccess)
					&& String.IsNullOrEmpty(RollSuccessExceptional)
					&& !SeemingBenefits.Any()
					&& Skill is null;
			}
		}
		
		public Contract() : base()
		{
			Action = String.Empty;
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
		
		public int CompareTo(Contract contract)
		{
			var ret = 0;
			if(contract is Contract)
			{
				if(ret.Equals(0))
				{
					if(Regalia is ContractRegalia)
						ret = Regalia.CompareTo(contract.Regalia);
					else
						ret = contract.Regalia is ContractRegalia ? 1 : 0;
				}
				if(ret.Equals(0))
				{
					if(ContractType is ContractType)
						ret = ContractType.CompareTo(contract.ContractType);
					else if(contract.ContractType is ContractType)
						ret = 1;
				}
				if(ret.Equals(0))
					ret = Name.Replace("The", String.Empty).Trim().CompareTo(contract.Name.Replace("The", String.Empty).Trim());
				if(ret.Equals(0))
					ret = Action.CompareTo(contract.Action);
				if(ret.Equals(0))
					ret = Cost.CompareTo(contract.Cost);
				if(ret.Equals(0))
					ret = Duration.CompareTo(contract.Duration);
			}
			
			return ret;
		}
		
		public bool Equals(Contract other)
		{
			return base.Equals(other)
				&& Action.Equals(other.Action)
				&& Logic.AreEqualOrNull<Attribute.Enum?>(Attribute, other.Attribute)
				&& Logic.AreEqualOrNull<Attribute.Enum?>(AttributeResisted, other.AttributeResisted)
				&& Logic.AreEqualOrNull<Attribute.Enum?>(AttributeContested, other.AttributeContested)
				&& Logic.AreEqualOrNull<ContractType>(ContractType, other.ContractType)
				&& Cost.Equals(other.Cost)
				&& Duration.Equals(other.Duration)
				&& Effects.Equals(other.Effects)
				&& Loophole.Equals(other.Loophole)
				&& Logic.AreEqualOrNull<ContractRegalia>(Regalia, other.Regalia)
				&& RollFailure.Equals(other.RollFailure)
				&& RollFailureDramatic.Equals(other.RollFailureDramatic)
				&& RollSuccess.Equals(other.RollSuccess)
				&& RollSuccessExceptional.Equals(other.RollSuccessExceptional)
				&& SeemingBenefits.Equals(other.SeemingBenefits)
				&& Logic.AreEqualOrNull<Skill.Enum?>(Skill, other.Skill);
		}
		
		public void Sort()
		{
			SeemingBenefits = SeemingBenefits.OrderBy(e => e.Key)
								.ToDictionary(e => e.Key, e => e.Value);
		}
	}
}
