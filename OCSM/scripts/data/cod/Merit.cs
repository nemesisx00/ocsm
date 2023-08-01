using System;
using Ocsm.Meta;

namespace Ocsm.Cofd;

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
