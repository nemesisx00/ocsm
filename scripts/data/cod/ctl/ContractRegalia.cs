using System;
using OCSM.Meta;

namespace OCSM.CoD.CtL
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
		
		public int CompareTo(ContractRegalia contractRegalia)
		{
			var ret = 0;
			if(contractRegalia is ContractRegalia)
			{
				if(Name.Equals(Goblin.Name))
					ret = contractRegalia.Name.Equals(Goblin.Name) ? 0 : -1;
				else
					ret = contractRegalia.Name.Equals(Goblin.Name) ? 1 : 0;
			}
			return ret;
		}
		
		public bool Equals(ContractRegalia contractRegalia) { return base.Equals(contractRegalia); }
		public Regalia toRegalia() { return new Regalia() { Description = Description, Icon = Icon, Name = Name }; }
		public Court toCourt() { return new Court() { Description = Description, Icon = Icon, Name = Name }; }
	}
}
