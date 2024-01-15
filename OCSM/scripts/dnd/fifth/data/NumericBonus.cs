using System;

namespace Ocsm.Dnd.Fifth;

public class NumericBonus() : IComparable<NumericBonus>, IEquatable<NumericBonus>
{
	public enum StatTypes
	{
		None,
		
		[Label("Ability Score")]
		AbilityScore,
		
		[Label("Armor Class")]
		ArmorClass,
		
		[Label("Initiative")]
		Initiative,
		
		[Label("Maximum Hit Points")]
		MaxHP,
		
		[Label("Walking Speed")]
		Speed,
		
		[Label("Temporary Hit Points")]
		TempHP
	}
	
	public string AbilityName { get; set; }
	public bool Add { get; set; }
	public string Name { get; set; }
	public StatTypes Type { get; set; }
	public int Value { get; set; }
	
	public int CompareTo(NumericBonus other)
	{
		var ret = Type.CompareTo(other?.Type);
		
		if(ret == 0)
			ret = Name.CompareTo(other?.Name);
		
		if(ret == 0)
			ret = AbilityName.CompareTo(other?.AbilityName);
		
		if(ret == 0)
			ret = Add.CompareTo(other?.Add);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as NumericBonus);
	
	public bool Equals(NumericBonus other) => AbilityName.Equals(other?.AbilityName)
		&& Add.Equals(other?.Add)
		&& Name.Equals(other?.Name)
		&& Type.Equals(other?.Type)
		&& Value.Equals(other?.Value);
	
	public override int GetHashCode() => HashCode.Combine(
		AbilityName,
		Add,
		Name,
		Type,
		Value
	);
}
