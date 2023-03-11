using System;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class ContractType : Metadata, IComparable<ContractType>, IEquatable<ContractType>
	{
		public const string Goblin = "Goblin";
		
		public ContractType() : base() { }
		
		public int CompareTo(ContractType other)
		{
			var ret = 0;
			
			if(other is ContractType)
			{
				if(Name.Equals(Goblin))
					ret = other.Name.Equals(Goblin) ? 0 : 1;
				else
					ret = other.Name.Equals(Goblin) ? -1 : Name.CompareTo(other.Name);
			}
			
			return ret;
		}
		
		public bool Equals(ContractType other) { return base.Equals(other); }
	}
}
