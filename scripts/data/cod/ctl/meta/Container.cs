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
			
			container.ContractTypes.Add(new ContractType("Common"));
			container.ContractTypes.Add(new ContractType("Royal"));
			container.ContractTypes.Add(new ContractType("Goblin"));
			
			container.Courts.Add(new Court("Autumn"));
			container.Courts.Add(new Court("Spring"));
			container.Courts.Add(new Court("Summer"));
			container.Courts.Add(new Court("Winter"));
			
			container.Kiths.Add(new Kith("Artist"));
			container.Kiths.Add(new Kith("Bright One"));
			container.Kiths.Add(new Kith("Chatelaine"));
			container.Kiths.Add(new Kith("Gristlegrinder"));
			container.Kiths.Add(new Kith("Helldiver"));
			container.Kiths.Add(new Kith("Hunterheart"));
			container.Kiths.Add(new Kith("Leechfinger"));
			container.Kiths.Add(new Kith("Mirrorskin"));
			container.Kiths.Add(new Kith("Nightsinger"));
			container.Kiths.Add(new Kith("Notary"));
			container.Kiths.Add(new Kith("Playmate"));
			container.Kiths.Add(new Kith("Snowskin"));
			
			container.Regalias.Add(new Regalia("Crown"));
			container.Regalias.Add(new Regalia("Jewels"));
			container.Regalias.Add(new Regalia("Mirror"));
			container.Regalias.Add(new Regalia("Shield"));
			container.Regalias.Add(new Regalia("Steed"));
			container.Regalias.Add(new Regalia("Sword"));
			
			container.Seemings.Add(new Seeming("Beast"));
			container.Seemings.Add(new Seeming("Darkling"));
			container.Seemings.Add(new Seeming("Elemental"));
			container.Seemings.Add(new Seeming("Fairest"));
			container.Seemings.Add(new Seeming("Ogre"));
			container.Seemings.Add(new Seeming("Wizened"));
			
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
				Flaws = ccc.Flaws;
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
