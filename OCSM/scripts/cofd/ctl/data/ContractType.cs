using System;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl;

public class ContractType() : Metadata(), IComparable<ContractType>, IEquatable<ContractType>
{
	public const string Goblin = "Goblin";
	
	public int CompareTo(ContractType other)
	{
		var ret = 0;
		
		if(other is not null)
		{
			if(Name.Equals(Goblin))
				ret = other.Name.Equals(Goblin) ? 0 : 1;
			else
				ret = other.Name.Equals(Goblin) ? -1 : Name.CompareTo(other.Name);
		}
		
		return ret;
	}
	
	public override bool Equals(object other) => base.Equals(other);
	public bool Equals(ContractType other) => base.Equals(other);
	public override int GetHashCode() => base.GetHashCode();
}
