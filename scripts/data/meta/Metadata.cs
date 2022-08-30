using Godot;
using System;

namespace OCSM.Meta
{
	public interface IMetadataContainer
	{
		void Deserialize(string json);
		bool IsEmpty();
		string Serialize();
	}
	
	public class Metadata : IEquatable<Metadata>
	{
		public string Description { get; set; }
		public Texture Icon { get; set; }
		public string Name { get; set; }
		
		public Metadata()
		{
			Description = String.Empty;
			Icon = null;
			Name = String.Empty;
		}
		
		public Metadata(string name, string description = "") : this()
		{
			Description = description;
			Name = name;
		}
		
		public bool Equals(Metadata metadata)
		{
			return metadata is Metadata
				&& metadata.Description.Equals(Description)
				&& Logic.AreEqualOrNull<Texture>(metadata.Icon, Icon)
				&& metadata.Name.Equals(Name);
		}
	}
}
