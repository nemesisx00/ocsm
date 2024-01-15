using System;
using System.Collections.Generic;
using System.Text.Json;
using Ocsm.Meta;

namespace Ocsm.Cofd.Meta;

public class CofdCoreContainer() : IMetadataContainer, IEquatable<CofdCoreContainer>
{
	public List<Merit> Merits { get; set; } = [];
	
	public virtual void Deserialize(string json)
	{
		var result = JsonSerializer.Deserialize<CofdCoreContainer>(json);
		if(result is CofdCoreContainer ccc)
			Merits = ccc.Merits;
	}

	public override bool Equals(object other) => Equals(other as CofdCoreContainer);
	public bool Equals(CofdCoreContainer container) => Merits.Equals(container?.Merits);
	public override int GetHashCode() => HashCode.Combine(Merits);
	public virtual bool IsEmpty() => Merits.Count < 1;
	public virtual string Serialize() => JsonSerializer.Serialize(this);
}
