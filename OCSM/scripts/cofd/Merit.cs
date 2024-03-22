using System;
using Ocsm.Meta;

namespace Ocsm.Cofd;

public class Merit() : Metadata(), IEquatable<Merit>
{
	public int Value { get; set; }

	public bool Equals(Merit merit) => base.Equals(merit)
		&& Value == merit?.Value;
	
	public override bool Equals(object obj) => Equals(obj as Merit);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);
}
