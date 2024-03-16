using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Ocsm.API;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl;

public class Contract() : Metadata(), IComparable<Contract>, IEmptiable, IEquatable<Contract>
{
	public string Action { get; set; } = string.Empty;
	public Traits? Attribute { get; set; } = null;
	public Traits? AttributeResisted { get; set; } = null;
	public Traits? AttributeContested { get; set; } = null;
	public Metadata ContractType { get; set; } = null;
	public string Cost { get; set; } = string.Empty;
	public string Duration { get; set; } = string.Empty;
	public string Effects { get; set; } = string.Empty;
	public string Loophole { get; set; } = string.Empty;
	public Metadata Regalia { get; set; } = null;
	public string RollFailure { get; set; } = string.Empty;
	public string RollFailureDramatic { get; set; } = string.Empty;
	public string RollSuccess { get; set; } = string.Empty;
	public string RollSuccessExceptional { get; set; } = string.Empty;
	public Dictionary<string, string> SeemingBenefits { get; set; }
	public Traits? Skill { get; set; } = null;
	
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
		int ret;
		
		if (Regalia is not null)
			ret = Regalia.CompareTo(contract?.Regalia);
		else
			ret = contract?.Regalia is not null ? 1 : 0;
		
		if(ret == 0)
		{
			if(ContractType is not null)
				ret = ContractType.CompareTo(contract?.ContractType);
			else if(contract?.ContractType is not null)
				ret = 1;
		}
		
		if(ret == 0)
			ret = Name.Replace(Constants.The, string.Empty).Trim().CompareTo(contract?.Name.Replace(Constants.The, string.Empty).Trim());
		
		if(ret == 0)
			ret = Action.CompareTo(contract?.Action);
		
		if(ret == 0)
			ret = Cost.CompareTo(contract?.Cost);
		
		if(ret == 0)
			ret = Duration.CompareTo(contract?.Duration);
		
		return ret;
	}
	
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
	
	public void Sort() => SeemingBenefits = SeemingBenefits.OrderBy(e => e.Key)
		.ToDictionary(e => e.Key, e => e.Value);
	
	public override bool Equals(object obj) => Equals(obj as Contract);
	public override int GetHashCode()
	{
		HashCode hash = new();
		hash.Add(base.GetHashCode());
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
		hash.Add(RollFailure);
		hash.Add(RollFailureDramatic);
		hash.Add(RollSuccess);
		hash.Add(RollSuccessExceptional);
		hash.Add(SeemingBenefits);
		hash.Add(Skill);
		
		return hash.ToHashCode();
	}
}
