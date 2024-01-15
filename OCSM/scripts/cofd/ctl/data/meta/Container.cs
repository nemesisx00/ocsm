using System;
using System.Collections.Generic;
using System.Text.Json;
using Ocsm.Cofd.Meta;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl.Meta;

public class CofdChangelingContainer() : CofdCoreContainer(), IMetadataContainer, IEquatable<CofdChangelingContainer>
{
	public static CofdChangelingContainer InitializeWithDefaultValues() => new()
	{
		ContractTypes = [
			new() { Name = "Common" },
			new() { Name = "Royal" },
			new() { Name = "Goblin" }
		],
		
		Courts = [
			new() { Name = "Autumn" },
			new() { Name = "Spring" },
			new() { Name = "Summer" },
			new() { Name = "Winter" }
		],
		
		Kiths = [
			new() { Name = "Artist" },
			new() { Name = "Bright One" },
			new() { Name = "Chatelaine" },
			new() { Name = "Gristlegrinder" },
			new() { Name = "Helldiver" },
			new() { Name = "Hunterheart" },
			new() { Name = "Leechfinger" },
			new() { Name = "Mirrorskin" },
			new() { Name = "Nightsinger" },
			new() { Name = "Notary" },
			new() { Name = "Playmate" },
			new() { Name = "Snowskin" }
		],
		
		Regalias = [
			new() { Name = "Crown" },
			new() { Name = "Jewels" },
			new() { Name = "Mirror" },
			new() { Name = "Shield" },
			new() { Name = "Steed" },
			new() { Name = "Sword" }
		],
		
		Seemings = [
			new() { Name = "Beast" },
			new() { Name = "Darkling" },
			new() { Name = "Elemental" },
			new() { Name = "Fairest" },
			new() { Name = "Ogre" },
			new() { Name = "Wizened" },
		]
	};
	
	public List<Contract> Contracts { get; set; } = [];
	public List<ContractType> ContractTypes { get; set; } = [];
	public List<Court> Courts { get; set; } = [];
	public List<Kith> Kiths { get; set; } = [];
	public List<Regalia> Regalias { get; set; } = [];
	public List<Seeming> Seemings { get; set; } = [];
	
	public override void Deserialize(string json)
	{
		var result = JsonSerializer.Deserialize<CofdChangelingContainer>(json);
		if(result is CofdChangelingContainer ccc)
		{
			Contracts = ccc.Contracts;
			ContractTypes = ccc.ContractTypes;
			Courts = ccc.Courts;
			Kiths = ccc.Kiths;
			Merits = ccc.Merits;
			Regalias = ccc.Regalias;
			Seemings = ccc.Seemings;
		}
	}
	
	public override bool IsEmpty() => base.IsEmpty()
		&& Contracts.Count == 0
		&& ContractTypes.Count == 0
		&& Courts.Count == 0
		&& Kiths.Count == 0
		&& Regalias.Count == 0
		&& Seemings.Count == 0;
	
	public override string Serialize() => JsonSerializer.Serialize(this);
	public override bool Equals(object obj) => Equals(obj as CofdChangelingContainer);
	public bool Equals(CofdChangelingContainer container) => base.Equals(container);
	
	public override int GetHashCode() => HashCode.Combine(
		Contracts,
		ContractTypes,
		Courts,
		Kiths,
		Merits,
		Regalias,
		Seemings
	);
}
