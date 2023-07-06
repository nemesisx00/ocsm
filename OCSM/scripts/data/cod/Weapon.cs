using System;
using System.Text.Json.Serialization;
using Ocsm.API;

namespace Ocsm.Cofd
{
	public class Weapon : IEmptiable, IEquatable<Weapon>, IComparable<Weapon>
	{
		public string Name { get; set; } = String.Empty;
		public WeaponType Type { get; set; } = WeaponType.Melee;
		public long Availability { get; set; } = 1;
		public long Damage { get; set; } = 0;
		public long Strength { get; set; } = 1;
		public long Size { get; set; } = 1;
		public long Capacity { get; set; } = 0;
		public long RangeShort { get; set; } = 1;
		public long RangeMid { get; set; } = 1;
		public long RangeLong { get; set; } = 1;
		public string Special { get; set; } = String.Empty;
		
		[JsonIgnore]
		public bool Empty
		{
			get
			{
				var empty = String.IsNullOrEmpty(Name)
					&& Damage.Equals(0)
					&& String.IsNullOrEmpty(Special);
				
				if(Type.Equals(WeaponType.Ranged))
					empty = empty && Capacity.Equals(0);
				
				return empty;
			}
		}
		
		public Weapon() {}
		
		public int CompareTo(Weapon other)
		{
			var ret = 0;
			
			if(other is Weapon)
			{
				ret = Type.CompareTo(other.Type);
				if(ret.Equals(0))
					ret = other.Availability.CompareTo(Availability);
				if(ret.Equals(0))
					ret = other.Damage.CompareTo(Damage);
				if(ret.Equals(0))
					ret = other.Size.CompareTo(Size);
				if(ret.Equals(0))
					ret = Strength.CompareTo(other.Strength);
				
				if(ret.Equals(0) && Type.Equals(WeaponType.Ranged))
				{
					ret = other.Capacity.CompareTo(Capacity);
					if(ret.Equals(0))
						ret = other.RangeLong.CompareTo(RangeLong);
					if(ret.Equals(0))
						ret = other.RangeMid.CompareTo(RangeMid);
					if(ret.Equals(0))
						ret = other.RangeShort.CompareTo(RangeShort);
				}
				
				if(ret.Equals(0))
					ret = other.Special.CompareTo(Special);
				if(ret.Equals(0))
					ret = Name.CompareTo(other.Name);
			}
			
			return ret;
		}
		
		public bool Equals(Weapon other)
		{
			return other is Weapon
				&& Name.Equals(other.Name)
				&& Type.Equals(other.Type)
				&& Availability.Equals(other.Availability)
				&& Damage.Equals(other.Damage)
				&& Strength.Equals(other.Strength)
				&& Size.Equals(other.Size)
				&& Capacity.Equals(other.Capacity)
				&& RangeShort.Equals(other.RangeShort)
				&& RangeMid.Equals(other.RangeMid)
				&& RangeLong.Equals(other.RangeLong)
				&& Special.Equals(other.Special);
		}
	}
}
