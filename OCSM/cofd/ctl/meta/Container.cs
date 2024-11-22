using System;
using System.Collections.Generic;
using System.Text.Json;
using Ocsm.Meta;
using Ocsm.Cofd.Meta;

namespace Ocsm.Cofd.Ctl.Meta;

public class CofdChangelingContainer() : CofdCoreContainer(), IMetadataContainer, IEquatable<CofdChangelingContainer>
{
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
	public override void InitializeWithDefaultValues()
	{
		Metadata.Clear();
		
		Metadata.AddRange([
			new() { Types = ["ContractRegalia"], Name = "Crown" },
			new() { Types = ["ContractRegalia"], Name = "Jewels" },
			new() { Types = ["ContractRegalia"], Name = "Mirror" },
			new() { Types = ["ContractRegalia"], Name = "Shield" },
			new() { Types = ["ContractRegalia"], Name = "Steed" },
			new() { Types = ["ContractRegalia"], Name = "Sword" },
			new() { Types = ["ContractRegalia"], Name = "Autumn" },
			new() { Types = ["ContractRegalia"], Name = "Spring" },
			new() { Types = ["ContractRegalia"], Name = "Summer" },
			new() { Types = ["ContractRegalia"], Name = "Winter" },
			new() { Types = ["ContractRegalia"], Name = "Goblin" },
			new() { Types = ["ContractType"], Name = "Common" },
			new() { Types = ["ContractType"], Name = "Royal" },
			new() { Types = ["ContractType"], Name = "Goblin" },
			new() { Types = ["Court"], Name = "Autumn" },
			new() { Types = ["Court"], Name = "Spring" },
			new() { Types = ["Court"], Name = "Summer" },
			new() { Types = ["Court"], Name = "Winter" },
			new() { Types = ["Kith"], Name = "Artist" },
			new() { Types = ["Kith"], Name = "Bright One" },
			new() { Types = ["Kith"], Name = "Chatelaine" },
			new() { Types = ["Kith"], Name = "Gristlegrinder" },
			new() { Types = ["Kith"], Name = "Helldiver" },
			new() { Types = ["Kith"], Name = "Hunterheart" },
			new() { Types = ["Kith"], Name = "Leechfinger" },
			new() { Types = ["Kith"], Name = "Mirrorskin" },
			new() { Types = ["Kith"], Name = "Nightsinger" },
			new() { Types = ["Kith"], Name = "Notary" },
			new() { Types = ["Kith"], Name = "Playmate" },
			new() { Types = ["Kith"], Name = "Snowskin" },
			new() { Types = ["Regalia"], Name = "Crown" },
			new() { Types = ["Regalia"], Name = "Jewels" },
			new() { Types = ["Regalia"], Name = "Mirror" },
			new() { Types = ["Regalia"], Name = "Shield" },
			new() { Types = ["Regalia"], Name = "Steed" },
			new() { Types = ["Regalia"], Name = "Sword" },
			new() { Types = ["Seeming"], Name = "Beast" },
			new() { Types = ["Seeming"], Name = "Darkling" },
			new() { Types = ["Seeming"], Name = "Elemental" },
			new() { Types = ["Seeming"], Name = "Fairest" },
			new() { Types = ["Seeming"], Name = "Ogre" },
			new() { Types = ["Seeming"], Name = "Wizened" },
		]);
	}
	
	public bool Equals(CofdChangelingContainer container) => base.Equals(container);
	public override bool Equals(object obj) => Equals(obj as CofdChangelingContainer);
	public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Contracts);
	public override bool IsEmpty() => base.IsEmpty() && Contracts.Count < 1;
	public override string Serialize() => JsonSerializer.Serialize(this);
}
