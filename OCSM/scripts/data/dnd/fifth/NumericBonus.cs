using System;

namespace Ocsm.Dnd.Fifth;

public class NumericBonus : IComparable<NumericBonus>, IEquatable<NumericBonus>
{
	public Abilities Ability { get; set; }
	public bool Add { get; set; }
	public string Name { get; set; } = string.Empty;
	public NumericStats Type { get; set; }
	public int Value { get; set; }
	
	public int CompareTo(NumericBonus other)
	{
		var ret = Type.CompareTo(other?.Type);
		
		if(ret == 0)
			ret = Name.CompareTo(other?.Name);
		
		if(ret == 0)
			ret = Ability.CompareTo(other?.Ability);
		
		if(ret == 0)
			ret = Add.CompareTo(other?.Add);
		
		return ret;
	}
	
	public bool Equals(NumericBonus other) => Ability == other?.Ability
		&& Add == other?.Add
		&& Name == other?.Name
		&& Type == other?.Type
		&& Value == other?.Value;
	
	public override bool Equals(object obj) => Equals(obj as NumericBonus);
	public override int GetHashCode() => HashCode.Combine(Ability, Add, Name, Type, Value);
}
