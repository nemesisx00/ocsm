using System;
using OCSM.Meta;

namespace OCSM.CoD
{
	public class Merit : Metadata, IEquatable<Merit>
	{
		public int Value { get; set; }
		
		public Merit() : base()
		{
			Value = 0;
		}
		
		public Merit(string name, string description = "") : base(name, description)
		{
			Value = 0;
		}
		
		public Merit(string name, string description = "", int value = 0) : base(name, description)
		{
			Value = value;
		}
		
		public bool Equals(Merit merit)
		{
			return base.Equals(merit)
				&& merit.Value.Equals(Value);
		}
	}
}
