using System;
using System.Collections.Generic;
using System.Text.Json;
using OCSM.Meta;
using OCSM.CoD.Meta;

namespace OCSM.CoD.CtL.Meta
{
	public class CoDChangelingContainer : CoDCoreContainer, IMetadataContainer, IEquatable<CoDChangelingContainer>
	{
		public static CoDChangelingContainer initializeWithDefaultValues()
		{
			var container = new CoDChangelingContainer();
			
			container.ContractTypes.Add(new ContractType() { Name = "Common" });
			container.ContractTypes.Add(new ContractType() { Name = "Royal" });
			container.ContractTypes.Add(new ContractType() { Name = "Goblin" });
			
			container.Courts.Add(new Court() { Name = "Autumn" });
			container.Courts.Add(new Court() { Name = "Spring" });
			container.Courts.Add(new Court() { Name = "Summer" });
			container.Courts.Add(new Court() { Name = "Winter" });
			
			container.Kiths.Add(new Kith() { Name = "Artist" });
			container.Kiths.Add(new Kith() { Name = "Bright One" });
			container.Kiths.Add(new Kith() { Name = "Chatelaine" });
			container.Kiths.Add(new Kith() { Name = "Gristlegrinder" });
			container.Kiths.Add(new Kith() { Name = "Helldiver" });
			container.Kiths.Add(new Kith() { Name = "Hunterheart" });
			container.Kiths.Add(new Kith() { Name = "Leechfinger" });
			container.Kiths.Add(new Kith() { Name = "Mirrorskin" });
			container.Kiths.Add(new Kith() { Name = "Nightsinger" });
			container.Kiths.Add(new Kith() { Name = "Notary" });
			container.Kiths.Add(new Kith() { Name = "Playmate" });
			container.Kiths.Add(new Kith() { Name = "Snowskin" });
			
			container.Regalias.Add(new Regalia() { Name = "Crown" });
			container.Regalias.Add(new Regalia() { Name = "Jewels" });
			container.Regalias.Add(new Regalia() { Name = "Mirror" });
			container.Regalias.Add(new Regalia() { Name = "Shield" });
			container.Regalias.Add(new Regalia() { Name = "Steed" });
			container.Regalias.Add(new Regalia() { Name = "Sword" });
			
			container.Seemings.Add(new Seeming() { Name = "Beast" });
			container.Seemings.Add(new Seeming() { Name = "Darkling" });
			container.Seemings.Add(new Seeming() { Name = "Elemental" });
			container.Seemings.Add(new Seeming() { Name = "Fairest" });
			container.Seemings.Add(new Seeming() { Name = "Ogre" });
			container.Seemings.Add(new Seeming() { Name = "Wizened" });
			
			return container;
		}
		
		public List<Contract> Contracts { get; set; }
		public List<ContractType> ContractTypes { get; set; }
		public List<Court> Courts { get; set; }
		public List<Kith> Kiths { get; set; }
		public List<Regalia> Regalias { get; set; }
		public List<Seeming> Seemings { get; set; }
		
		public CoDChangelingContainer() : base()
		{
			Contracts = new List<Contract>();
			ContractTypes = new List<ContractType>();
			Courts = new List<Court>();
			Kiths = new List<Kith>();
			Regalias = new List<Regalia>();
			Seemings = new List<Seeming>();
		}
		
		public override void Deserialize(string json)
		{
			var result = JsonSerializer.Deserialize<CoDChangelingContainer>(json);
			if(result is CoDChangelingContainer ccc)
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
		
		public override bool IsEmpty()
		{
			return base.IsEmpty()
				&& Contracts.Count < 1
				&& ContractTypes.Count < 1
				&& Courts.Count < 1
				&& Kiths.Count < 1
				&& Regalias.Count < 1
				&& Seemings.Count < 1;
		}
		
		public override string Serialize()
		{
			return JsonSerializer.Serialize(this);
		}
		
		public bool Equals(CoDChangelingContainer container)
		{
			return base.Equals(container);
		}
	}
}
