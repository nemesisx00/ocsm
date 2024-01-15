using System;
using System.Text.Json.Serialization;
using Ocsm.Api;

namespace Ocsm.Cofd;

public class Weapon() : IEmptiable, IEquatable<Weapon>, IComparable<Weapon>
{
	public string Name { get; set; }
	public WeaponType Type { get; set; } = WeaponType.Melee;
	public int Availability { get; set; } = 1;
	public int Damage { get; set; }
	public int Strength { get; set; } = 1;
	public int Size { get; set; } = 1;
	public int Capacity { get; set; }
	public int RangeShort { get; set; } = 1;
	public int RangeMid { get; set; } = 1;
	public int RangeLong { get; set; } = 1;
	public string Special { get; set; }
	
	[JsonIgnore]
	public bool Empty
	{
		get
		{
			var empty = string.IsNullOrEmpty(Name)
				&& Damage == 0
				&& string.IsNullOrEmpty(Special);
			
			if(Type.Equals(WeaponType.Ranged))
				empty = empty && Capacity == 0;
			
			return empty;
		}
	}
	
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
	
	public override bool Equals(object other) => Equals(other as Weapon);
	
	public bool Equals(Weapon other) => Name.Equals(other?.Name)
		&& Type.Equals(other?.Type)
		&& Availability.Equals(other?.Availability)
		&& Damage.Equals(other?.Damage)
		&& Strength.Equals(other?.Strength)
		&& Size.Equals(other?.Size)
		&& Capacity.Equals(other?.Capacity)
		&& RangeShort.Equals(other?.RangeShort)
		&& RangeMid.Equals(other?.RangeMid)
		&& RangeLong.Equals(other?.RangeLong)
		&& Special.Equals(other?.Special);
	
	public override int GetHashCode()
	{
		HashCode hash = new();
		hash.Add(Name);
		hash.Add(Type);
		hash.Add(Availability);
		hash.Add(Damage);
		hash.Add(Strength);
		hash.Add(Size);
		hash.Add(Capacity);
		hash.Add(RangeShort);
		hash.Add(RangeMid);
		hash.Add(RangeLong);
		hash.Add(Special);
		return hash.ToHashCode();
	}
}
