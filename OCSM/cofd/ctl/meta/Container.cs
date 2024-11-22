using System;
using System.Collections.Generic;
using System.Text.Json;
using Ocsm.Meta;
using Ocsm.Cofd.Meta;

namespace Ocsm.Cofd.Ctl.Meta;

public class CofdChangelingContainer() : CofdCoreContainer(), IMetadataContainer, IEquatable<CofdChangelingContainer>
{
	public static CofdChangelingContainer InitializeWithDefaultValues()
	{
		var container = new CofdChangelingContainer();
		
		container.Metadata.Add(new() { Types = ["ContractRegalia"], Name = "Crown" });
		container.Metadata.Add(new() { Types = ["ContractRegalia"], Name = "Jewels" });
		container.Metadata.Add(new() { Types = ["ContractRegalia"], Name = "Mirror" });
		container.Metadata.Add(new() { Types = ["ContractRegalia"], Name = "Shield" });
		container.Metadata.Add(new() { Types = ["ContractRegalia"], Name = "Steed" });
		container.Metadata.Add(new() { Types = ["ContractRegalia"], Name = "Sword" });
		container.Metadata.Add(new() { Types = ["ContractRegalia"], Name = "Autumn" });
		container.Metadata.Add(new() { Types = ["ContractRegalia"], Name = "Spring" });
		container.Metadata.Add(new() { Types = ["ContractRegalia"], Name = "Summer" });
		container.Metadata.Add(new() { Types = ["ContractRegalia"], Name = "Winter" });
		container.Metadata.Add(new() { Types = ["ContractRegalia"], Name = "Goblin" });
		
		container.Metadata.Add(new() { Types = ["ContractType"], Name = "Common" });
		container.Metadata.Add(new() { Types = ["ContractType"], Name = "Royal" });
		container.Metadata.Add(new() { Types = ["ContractType"], Name = "Goblin" });
		
		container.Metadata.Add(new() { Types = ["Court"], Name = "Autumn" });
		container.Metadata.Add(new() { Types = ["Court"], Name = "Spring" });
		container.Metadata.Add(new() { Types = ["Court"], Name = "Summer" });
		container.Metadata.Add(new() { Types = ["Court"], Name = "Winter" });
		
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Artist" });
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Bright One" });
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Chatelaine" });
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Gristlegrinder" });
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Helldiver" });
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Hunterheart" });
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Leechfinger" });
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Mirrorskin" });
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Nightsinger" });
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Notary" });
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Playmate" });
		container.Metadata.Add(new() { Types = ["Kith"], Name = "Snowskin" });
		
		container.Metadata.Add(new() { Types = ["Regalia"], Name = "Crown" });
		container.Metadata.Add(new() { Types = ["Regalia"], Name = "Jewels" });
		container.Metadata.Add(new() { Types = ["Regalia"], Name = "Mirror" });
		container.Metadata.Add(new() { Types = ["Regalia"], Name = "Shield" });
		container.Metadata.Add(new() { Types = ["Regalia"], Name = "Steed" });
		container.Metadata.Add(new() { Types = ["Regalia"], Name = "Sword" });
		
		container.Metadata.Add(new() { Types = ["Seeming"], Name = "Beast" });
		container.Metadata.Add(new() { Types = ["Seeming"], Name = "Darkling" });
		container.Metadata.Add(new() { Types = ["Seeming"], Name = "Elemental" });
		container.Metadata.Add(new() { Types = ["Seeming"], Name = "Fairest" });
		container.Metadata.Add(new() { Types = ["Seeming"], Name = "Ogre" });
		container.Metadata.Add(new() { Types = ["Seeming"], Name = "Wizened" });
		
		return container;
	}
	
	public List<Contract> Contracts { get; set; } = [];
	
	public override void Deserialize(string json)
	{
		var result = JsonSerializer.Deserialize<CofdChangelingContainer>(json);
		if(result is CofdChangelingContainer container)
		{
			Contracts = container.Contracts;
			Merits = container.Merits;
			Metadata = container.Metadata;
		}
	}
	
	public bool Equals(CofdChangelingContainer container) => base.Equals(container);
	public override bool Equals(object obj) => Equals(obj as CofdChangelingContainer);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Contracts);
	public override bool IsEmpty() => base.IsEmpty() && Contracts.Count < 1;
	public override string Serialize() => JsonSerializer.Serialize(this);
}
