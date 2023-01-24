using System;
using OCSM.Meta;

namespace OCSM.CoD.CtL
{
	public class ContractType : Metadata, IEquatable<ContractType>
	{
		public ContractType() : base() { }
		public bool Equals(ContractType contractType) { return base.Equals(contractType); }
	}
}
