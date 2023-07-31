using System;
using System.Collections.Generic;
using System.Text.Json;
using Ocsm.Meta;

namespace Ocsm.Cofd.Meta;

public class CofdCoreContainer : IMetadataContainer, IEquatable<CofdCoreContainer>
{
	public List<Merit> Merits { get; set; } = new List<Merit>();
	
	public virtual void Deserialize(string json)
	{
		var result = JsonSerializer.Deserialize<CofdCoreContainer>(json);
		if(result is CofdCoreContainer ccc)
		{
			Merits = ccc.Merits;
		}
	}
	
	public virtual bool IsEmpty()
	{
		return Merits.Count < 1;
	}
	
	public virtual string Serialize()
	{
		return JsonSerializer.Serialize(this);
	}
	
	public bool Equals(CofdCoreContainer container)
	{
		return container.Merits.Equals(Merits);
	}
}
