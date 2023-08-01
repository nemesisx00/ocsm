using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Ocsm.API;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl;

public class Contract : Metadata, IComparable<Contract>, IEmptiable, IEquatable<Contract>
{
	public string Action { get; set; } = String.Empty;
	public Attribute.Enum? Attribute { get; set; } = null;
	public Attribute.Enum? AttributeResisted { get; set; } = null;
	public Attribute.Enum? AttributeContested { get; set; } = null;
	public ContractType ContractType { get; set; } = null;
	public string Cost { get; set; } = String.Empty;
	public string Duration { get; set; } = String.Empty;
	public string Effects { get; set; } = String.Empty;
	public string Loophole { get; set; } = String.Empty;
	public ContractRegalia Regalia { get; set; } = null;
	public string RollSuccess { get; set; } = String.Empty;
	public string RollSuccessExceptional { get; set; } = String.Empty;
	public string RollFailure { get; set; } = String.Empty;
	public string RollFailureDramatic { get; set; } = String.Empty;
	public Dictionary<string, string> SeemingBenefits { get; set; }
	public Skill.Enum? Skill { get; set; } = null;
	
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
	
	[JsonIgnore]
	public bool ShowResults
	{
		get {
			return !String.IsNullOrEmpty(RollFailure)
				|| !String.IsNullOrEmpty(RollFailureDramatic)
				|| !String.IsNullOrEmpty(RollSuccess)
				|| !String.IsNullOrEmpty(RollSuccessExceptional);
		}
	}
	
	public Contract() : base() {}
	
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
