using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Godot;
using Ocsm.Meta;

namespace Ocsm.Cofd.Meta;

public class CofdCoreContainer() : BaseContainer(), IMetadataContainer, IEquatable<CofdCoreContainer>
{
	public List<Merit> Merits { get; set; } = [];
	
	public override void Deserialize(string json)
	{
		var result = JsonSerializer.Deserialize<CofdCoreContainer>(json);
		if(result is CofdCoreContainer container)
		{
			Metadata = container.Metadata;
			Merits = container.Merits;
		}
	}
	
	public override bool IsEmpty() => base.IsEmpty() && Merits.Count < 1;
	public override string Serialize() => JsonSerializer.Serialize(this);
	public bool Equals(CofdCoreContainer other) => base.Equals(other) && Merits == other?.Merits;
	public override bool Equals(object obj) => Equals(obj as CofdCoreContainer);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Merits);
}
