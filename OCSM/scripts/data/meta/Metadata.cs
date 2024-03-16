using Godot;
using System;

namespace Ocsm.Meta;

/**
<summary>
A single entry of metadata.

<para>
The Type property is used to differentiate instances for use in a given input
field on a character sheet.
</para>
</summary>
*/
public class Metadata() : IComparable<Metadata>, IEquatable<Metadata>
{
	public string Description { get; set; } = string.Empty;
	public Texture2D Icon { get; set; }
	public string Name { get; set; } = string.Empty;
	public MetadataType Type { get; set; }
	
	public int CompareTo(Metadata other)
	{
		var ret = Type.CompareTo(other?.Type);
		
		if(ret == 0)
			ret = Name.CompareTo(other?.Name);
		
		if(ret == 0)
			ret = Description.CompareTo(other?.Description);
		
		return ret;
	}
	
	public bool Equals(Metadata other) => Description == other?.Description
		&& Logic.AreEqualOrNull(Icon, other?.Icon)
		&& Name == other?.Name
		&& Type == other?.Type;
	
	public override bool Equals(object obj) => Equals(obj as Metadata);
	public override int GetHashCode() => HashCode.Combine(Description, Icon, Name, Type);
}
