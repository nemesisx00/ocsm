using Godot;
using System;

namespace Ocsm.Meta;

public interface IMetadataContainer
{
	void Deserialize(string json);
	bool IsEmpty();
	string Serialize();
}

public class Metadata : IComparable<Metadata>, IEquatable<Metadata>
{
	public string Description { get; set; }
	public Texture2D Icon { get; set; }
	public string Name { get; set; }
	
	public Metadata()
	{
		Description = String.Empty;
		Icon = null;
		Name = String.Empty;
	}
	
	public int CompareTo(Metadata metadata)
	{
		var ret = 0;
		if(metadata is Metadata)
		{
			ret = Name.CompareTo(metadata.Name);
			if(ret.Equals(0))
				ret = Description.CompareTo(metadata.Description);
		}
		return ret;
	}
	
	public bool Equals(Metadata metadata)
	{
		return metadata is Metadata
			&& metadata.Description.Equals(Description)
			&& Logic.AreEqualOrNull<Texture2D>(metadata.Icon, Icon)
			&& metadata.Name.Equals(Name);
	}
}
