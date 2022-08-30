using System;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class ContractType : Metadata, IEquatable<ContractType>
	{
		public ContractType(string name, string description = "") : base(name, description) { }
		
		public bool Equals(ContractType contractType)
		{
			return contractType.Description.Equals(Description)
				&& contractType.Name.Equals(Name);
		}
	}
}
