using System;
using System.Text.Json.Serialization;
using Ocsm.API;

namespace Ocsm.Cofd;

public class Weapon() : IEmptiable, IEquatable<Weapon>, IComparable<Weapon>
{
	public int Availability { get; set; } = 1;
	public int Capacity { get; set; }
	public int Damage { get; set; }
	public string Name { get; set; } = string.Empty;
	public int RangeLong { get; set; } = 1;
	public int RangeMid { get; set; } = 1;
	public int RangeShort { get; set; } = 1;
	public int Size { get; set; } = 1;
	public string Special { get; set; } = string.Empty;
	public int Strength { get; set; } = 1;
	public WeaponType Type { get; set; }
	
	[JsonIgnore]
	public bool Empty => string.IsNullOrEmpty(Name)
		&& Damage == 0
		&& string.IsNullOrEmpty(Special)
		&& (Type != WeaponType.Ranged || Capacity == 0);
	
	public int CompareTo(Weapon other)
	{
		var ret = Type.CompareTo(other?.Type);
		
		if(ret == 0)
			ret = Availability.CompareTo(other?.Availability);
		
		if(ret == 0)
			ret = Damage.CompareTo(other?.Damage);
		
		if(ret == 0)
			ret = Size.CompareTo(other?.Size);
		
		if(ret == 0)
			ret = Strength.CompareTo(other?.Strength);
		
		if(ret == 0 && Type.Equals(WeaponType.Ranged))
		{
			ret = Capacity.CompareTo(other?.Capacity);
			
			if(ret == 0)
				ret = RangeLong.CompareTo(other?.RangeLong);
			
			if(ret == 0)
				ret = RangeMid.CompareTo(other?.RangeMid);
			
			if(ret == 0)
				ret = RangeShort.CompareTo(other?.RangeShort);
		}
		
		if(ret == 0)
			ret = Special.CompareTo(other?.Special);
		
		if(ret == 0)
			ret = Name.CompareTo(other?.Name);
		
		return ret;
	}
	
	public bool Equals(Weapon other) => Availability.Equals(other?.Availability)
		&& Capacity.Equals(other?.Capacity)
		&& Damage.Equals(other?.Damage)
		&& Name.Equals(other?.Name)
		&& RangeLong.Equals(other?.RangeLong)
		&& RangeMid.Equals(other?.RangeMid)
		&& RangeShort.Equals(other?.RangeShort)
		&& Size.Equals(other?.Size)
		&& Special.Equals(other?.Special)
		&& Strength.Equals(other?.Strength)
		&& Type.Equals(other?.Type);
	
	public override bool Equals(object obj) => Equals(obj as Weapon);
	
	public override int GetHashCode()
	{
		HashCode hash = new();
		hash.Add(Availability);
		hash.Add(Capacity);
		hash.Add(Damage);
		hash.Add(Name);
		hash.Add(RangeLong);
		hash.Add(RangeMid);
		hash.Add(RangeShort);
		hash.Add(Size);
		hash.Add(Special);
		hash.Add(Strength);
		hash.Add(Type);
		return hash.ToHashCode();
	}
}
