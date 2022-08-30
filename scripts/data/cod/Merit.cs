using System;
using OCSM.Meta;

namespace OCSM.CoD
{
	public class Merit : Metadata, IEquatable<Merit>
	{
		public int Value { get; set; }
		
		public Merit(string name, string description = "") : base(name, description) { }
		
		public bool Equals(Merit merit)
		{
			return base.Equals(merit)
				&& merit.Value.Equals(Value);
		}
	}
}
