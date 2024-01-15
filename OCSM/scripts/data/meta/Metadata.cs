using Godot;
using System;

namespace Ocsm.Meta;

public class Metadata() : IComparable<Metadata>, IEquatable<Metadata>
{
	public string Description { get; set; }
	public Texture2D Icon { get; set; }
	public string Name { get; set; }
	
	public int CompareTo(Metadata other)
	{
		int ret = Name.CompareTo(other?.Name);
			
		if(ret == 0)
			ret = Description.CompareTo(other?.Description);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as Metadata);
	
	public bool Equals(Metadata other) => Logic.AreEqualOrNull(Description, other?.Description)
		&& Logic.AreEqualOrNull(Icon, other?.Icon)
		&& Logic.AreEqualOrNull(Name, other?.Name);
	
	public override int GetHashCode() => HashCode.Combine(
		Description,
		Icon,
		Name
	);
}
