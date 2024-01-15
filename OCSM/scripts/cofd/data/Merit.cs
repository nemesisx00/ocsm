using System;
using Ocsm.Meta;

namespace Ocsm.Cofd;

public class Merit() : Metadata(), IEquatable<Merit>
{
	public int Value { get; set; }
	
	public override bool Equals(object other) => Equals(other as Merit);
	
	public bool Equals(Merit other) => base.Equals(other)
		&& Value.Equals(other?.Value);
	
	public override int GetHashCode() => HashCode.Combine(
		Description,
		Icon,
		Name,
		Value
	);
}
