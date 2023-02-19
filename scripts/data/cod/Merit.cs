using System;
using OCSM.Meta;

namespace OCSM.CoD
{
	public class Merit : Metadata, IEquatable<Merit>
	{
		public long Value { get; set; }
		
		public Merit() : base()
		{
			Value = 0;
		}
		
		public bool Equals(Merit merit)
		{
			return base.Equals(merit)
				&& merit.Value.Equals(Value);
		}
	}
}
