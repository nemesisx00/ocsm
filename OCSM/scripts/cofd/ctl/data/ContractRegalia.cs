using System;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl;

public class ContractRegalia() : Metadata(), IComparable<ContractRegalia>, IEquatable<ContractRegalia>
{
	public static readonly ContractRegalia Goblin = new()
	{
		Description = string.Empty,
		Icon = null,
		Name = "Goblin"
	};
	
	public static ContractRegalia From(Regalia regalia) => new() { Description = regalia.Description, Icon = regalia.Icon, Name = regalia.Name };
	public static ContractRegalia From(Court court) => new() { Description = court.Description, Icon = court.Icon, Name = court.Name };

	public int CompareTo(ContractRegalia other)
	{
		int ret;
		
		if (other is not null)
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
	
	public override bool Equals(object other) => base.Equals(other);
	public bool Equals(ContractRegalia other) => base.Equals(other);
	public override int GetHashCode() => base.GetHashCode();
	public Regalia ToRegalia() => new() { Description = Description, Icon = Icon, Name = Name };
	public Court ToCourt() => new() { Description = Description, Icon = Icon, Name = Name };
}
