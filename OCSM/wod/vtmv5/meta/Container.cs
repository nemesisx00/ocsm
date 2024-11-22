using System;
using System.Text.Json;
using Ocsm.Meta;

namespace Ocsm.Wod.VtmV5.Meta;

public class WodVtmV5Container() : BaseContainer(), IMetadataContainer, IEquatable<WodVtmV5Container>
{
	public static WodVtmV5Container InitializeWithDefaultValues()
	{
		var container = new WodVtmV5Container();
		
		container.Metadata.Add(new() { Types = ["PredatorType"], Name = "Alleycat" });
		container.Metadata.Add(new() { Types = ["PredatorType"], Name = "Bagger" });
		container.Metadata.Add(new() { Types = ["PredatorType"], Name = "Blood Leech" });
		container.Metadata.Add(new() { Types = ["PredatorType"], Name = "Cleaver" });
		container.Metadata.Add(new() { Types = ["PredatorType"], Name = "Consensualist" });
		container.Metadata.Add(new() { Types = ["PredatorType"], Name = "Farmer" });
		container.Metadata.Add(new() { Types = ["PredatorType"], Name = "Osiris" });
		container.Metadata.Add(new() { Types = ["PredatorType"], Name = "Sandman" });
		container.Metadata.Add(new() { Types = ["PredatorType"], Name = "Scene Queen" });
		container.Metadata.Add(new() { Types = ["PredatorType"], Name = "Siren" });
		
		return container;
	}
	
	public override void Deserialize(string json)
	{
		var result = JsonSerializer.Deserialize<WodVtmV5Container>(json);
		if(result is WodVtmV5Container container)
		{
			Metadata = container.Metadata;
		}
	}
	
	public bool Equals(WodVtmV5Container container) => base.Equals(container);
	public override bool Equals(object obj) => Equals(obj as WodVtmV5Container);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode());
	public override bool IsEmpty() => base.IsEmpty();
	public override string Serialize() => JsonSerializer.Serialize(this);
}
