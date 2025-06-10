using System;
using System.Collections.Generic;
using Ocsm.Meta;

namespace Ocsm.Wod.VtmV5;

public class Clan() : Metadata(), IComparable<Clan>, IEquatable<Clan>
{
	public string Bane { get; set; }
	public string Compulsion { get; set; }
	List<Discipline> Disciplines { get; set; } = [];
	
	public int CompareTo(Clan other) => base.CompareTo(other);
	public override bool Equals(object obj) => Equals(obj as Clan);
	
	public bool Equals(Clan other) => base.Equals(other)
		&& Bane == other?.Bane
		&& Compulsion == other?.Compulsion
		&& Disciplines == other?.Disciplines;
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		Bane,
		Compulsion,
		Disciplines
	);
}
