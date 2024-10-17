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
		
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractRegalia, Name = "Crown" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractRegalia, Name = "Jewels" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractRegalia, Name = "Mirror" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractRegalia, Name = "Shield" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractRegalia, Name = "Steed" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractRegalia, Name = "Sword" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractRegalia, Name = "Autumn" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractRegalia, Name = "Spring" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractRegalia, Name = "Summer" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractRegalia, Name = "Winter" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractRegalia, Name = "Goblin" });
		
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractType, Name = "Common" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractType, Name = "Royal" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingContractType, Name = "Goblin" });
		
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingCourt, Name = "Autumn" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingCourt, Name = "Spring" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingCourt, Name = "Summer" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingCourt, Name = "Winter" });
		
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Artist" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Bright One" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Chatelaine" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Gristlegrinder" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Helldiver" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Hunterheart" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Leechfinger" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Mirrorskin" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Nightsinger" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Notary" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Playmate" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingKith, Name = "Snowskin" });
		
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingRegalia, Name = "Crown" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingRegalia, Name = "Jewels" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingRegalia, Name = "Mirror" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingRegalia, Name = "Shield" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingRegalia, Name = "Steed" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingRegalia, Name = "Sword" });
		
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingSeeming, Name = "Beast" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingSeeming, Name = "Darkling" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingSeeming, Name = "Elemental" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingSeeming, Name = "Fairest" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingSeeming, Name = "Ogre" });
		container.Metadata.Add(new() { Type = MetadataType.CofdChangelingSeeming, Name = "Wizened" });
		
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
