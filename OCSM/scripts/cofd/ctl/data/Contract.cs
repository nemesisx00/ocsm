using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Ocsm.Api;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl;

public class Contract() : Metadata(), IComparable<Contract>, IEmptiable, IEquatable<Contract>
{
	private const string The = "The";
	
	public string Action { get; set; }
	public Attribute.EnumValues? Attribute { get; set; }
	public Attribute.EnumValues? AttributeResisted { get; set; }
	public Attribute.EnumValues? AttributeContested { get; set; }
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
	public Dictionary<string, string> SeemingBenefits { get; set; } = [];
	public Skill.EnumValues? Skill { get; set; }
	
	[JsonIgnore]
	public bool Empty => string.IsNullOrEmpty(Action)
		&& Attribute is null
		&& AttributeContested is null
		&& AttributeResisted is null
		&& ContractType is null
		&& string.IsNullOrEmpty(Description)
		&& string.IsNullOrEmpty(Effects)
		&& string.IsNullOrEmpty(Loophole)
		&& string.IsNullOrEmpty(Name)
		&& Regalia is null
		&& string.IsNullOrEmpty(RollFailure)
		&& string.IsNullOrEmpty(RollFailureDramatic)
		&& string.IsNullOrEmpty(RollSuccess)
		&& string.IsNullOrEmpty(RollSuccessExceptional)
		&& SeemingBenefits.Count == 0
		&& Skill is null;
	
	[JsonIgnore]
	public bool ShowResults => !string.IsNullOrEmpty(RollFailure)
		|| !string.IsNullOrEmpty(RollFailureDramatic)
		|| !string.IsNullOrEmpty(RollSuccess)
		|| !string.IsNullOrEmpty(RollSuccessExceptional);
	
	public int CompareTo(Contract contract)
	{
		var ret = 0;
		if(contract is not null)
		{
			if(ret.Equals(0))
			{
				if(Regalia is not null)
					ret = Regalia.CompareTo(contract.Regalia);
				else
					ret = contract.Regalia is not null ? 1 : 0;
			}
			
			if(ret.Equals(0))
			{
				if(ContractType is not null)
					ret = ContractType.CompareTo(contract.ContractType);
				else if(contract.ContractType is not null)
					ret = 1;
			}
			
			if(ret.Equals(0))
				ret = Name.Replace(The, string.Empty)
					.Trim()
					.CompareTo(contract.Name.Replace(The, string.Empty).Trim());
				
			if(ret.Equals(0))
				ret = Action.CompareTo(contract.Action);
			
			if(ret.Equals(0))
				ret = Cost.CompareTo(contract.Cost);
			
			if(ret.Equals(0))
				ret = Duration.CompareTo(contract.Duration);
		}
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as Contract);

	public bool Equals(Contract other) => base.Equals(other)
		&& Action.Equals(other.Action)
		&& Logic.AreEqualOrNull(Attribute, other.Attribute)
		&& Logic.AreEqualOrNull(AttributeResisted, other.AttributeResisted)
		&& Logic.AreEqualOrNull(AttributeContested, other.AttributeContested)
		&& Logic.AreEqualOrNull(ContractType, other.ContractType)
		&& Cost.Equals(other.Cost)
		&& Duration.Equals(other.Duration)
		&& Effects.Equals(other.Effects)
		&& Loophole.Equals(other.Loophole)
		&& Logic.AreEqualOrNull(Regalia, other.Regalia)
		&& RollFailure.Equals(other.RollFailure)
		&& RollFailureDramatic.Equals(other.RollFailureDramatic)
		&& RollSuccess.Equals(other.RollSuccess)
		&& RollSuccessExceptional.Equals(other.RollSuccessExceptional)
		&& SeemingBenefits.Equals(other.SeemingBenefits)
		&& Logic.AreEqualOrNull(Skill, other.Skill);
	
	public override int GetHashCode()
	{
		HashCode hash = new();
		hash.Add(Action);
		hash.Add(Attribute);
		hash.Add(AttributeResisted);
		hash.Add(AttributeContested);
		hash.Add(ContractType);
		hash.Add(Cost);
		hash.Add(Duration);
		hash.Add(Effects);
		hash.Add(Loophole);
		hash.Add(Regalia);
		hash.Add(RollSuccess);
		hash.Add(RollSuccessExceptional);
		hash.Add(RollFailure);
		hash.Add(RollFailureDramatic);
		hash.Add(SeemingBenefits);
		hash.Add(Skill);
		return hash.ToHashCode();
	}
	
	public void Sort()
	{
		SeemingBenefits = SeemingBenefits.OrderBy(e => e.Key)
			.ToDictionary(e => e.Key, e => e.Value);
	}
}
