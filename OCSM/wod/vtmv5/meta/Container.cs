using System;
using System.Text.Json;
using Ocsm.Meta;

namespace Ocsm.Wod.VtmV5.Meta;

public class WodVtmV5Container() : BaseContainer(), IMetadataContainer, IEquatable<WodVtmV5Container>
{
	public override void Deserialize(string json)
	{
		var result = JsonSerializer.Deserialize<WodVtmV5Container>(json);
		if(result is WodVtmV5Container container)
		{
			Metadata = container.Metadata;
		}
	}
	
	public override void InitializeWithDefaultValues()
	{
		Metadata.Clear();
		
		Metadata.AddRange([
			new() { Types = ["PredatorType"], Name = "Alleycat" },
			new() { Types = ["PredatorType"], Name = "Bagger" },
			new() { Types = ["PredatorType"], Name = "Blood Leech" },
			new() { Types = ["PredatorType"], Name = "Cleaver" },
			new() { Types = ["PredatorType"], Name = "Consensualist" },
			new() { Types = ["PredatorType"], Name = "Farmer" },
			new() { Types = ["PredatorType"], Name = "Osiris" },
			new() { Types = ["PredatorType"], Name = "Sandman" },
			new() { Types = ["PredatorType"], Name = "Scene Queen" },
			new() { Types = ["PredatorType"], Name = "Siren" },
		]);
	}
	
	public bool Equals(WodVtmV5Container container) => base.Equals(container);
	public override bool Equals(object obj) => Equals(obj as WodVtmV5Container);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode());
	public override bool IsEmpty() => base.IsEmpty();
	public override string Serialize() => JsonSerializer.Serialize(this);
}
