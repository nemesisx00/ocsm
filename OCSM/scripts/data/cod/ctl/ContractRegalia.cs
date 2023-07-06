using System;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl
{
	public class ContractRegalia : Metadata, IComparable<ContractRegalia>, IEquatable<ContractRegalia>
	{
		public static ContractRegalia Goblin = new ContractRegalia()
		{
			Description = "",
			Icon = null,
			Name = "Goblin"
		};
		
		public static ContractRegalia From(Regalia regalia) { return new ContractRegalia() { Description = regalia.Description, Icon = regalia.Icon, Name = regalia.Name }; }
		public static ContractRegalia From(Court court) { return new ContractRegalia() { Description = court.Description, Icon = court.Icon, Name = court.Name }; }
		
		public ContractRegalia() : base() { }
		
		public int CompareTo(ContractRegalia other)
		{
			var ret = 0;
			
			if(other is ContractRegalia)
			{
				if(Name.Equals(Goblin.Name))
					ret = other.Name.Equals(Goblin.Name) ? 0 : 1;
				else if(other.Name.Equals(Goblin.Name))
					ret = -1;
				else
					ret = Name.CompareTo(other.Name);
			}
			else
				ret = -1;
			
			return ret;
		}
		
		public bool Equals(ContractRegalia other) { return base.Equals(other); }
		public Regalia toRegalia() { return new Regalia() { Description = Description, Icon = Icon, Name = Name }; }
		public Court toCourt() { return new Court() { Description = Description, Icon = Icon, Name = Name }; }
	}
}
