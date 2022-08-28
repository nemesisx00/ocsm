using Godot;
using System;

namespace OCSM.Meta
{
	public class Metadata : IEquatable<Metadata>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Texture Icon { get; set; }
		
		public Metadata()
		{
			Name = String.Empty;
			Description = String.Empty;
			Icon = null;
		}
		
		public Metadata(string name, string description = "") : this()
		{
			Name = name;
			Description = description;
		}
		
		public bool Equals(Metadata metadata)
		{
			return metadata.Name.Equals(Name)
				&& metadata.Description.Equals(Description)
				&& (
					(metadata.Icon is Texture && metadata.Icon.Equals(Icon))
					|| (metadata.Icon == null && Icon == null)
				);
		}
	}
}
