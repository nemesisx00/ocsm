using System;
using System.Collections.Generic;
using System.Text.Json;
using OCSM.Meta;

namespace OCSM.CoD.Meta
{
	public class CoDCoreContainer : IMetadataContainer, IEquatable<CoDCoreContainer>
	{
		public List<Merit> Merits { get; set; } = new List<Merit>();
		public List<Merit> Flaws { get; set; } = new List<Merit>();
		
		public virtual void Deserialize(string json)
		{
			var result = JsonSerializer.Deserialize<CoDCoreContainer>(json);
			if(result is CoDCoreContainer ccc)
			{
				Merits = ccc.Merits;
			}
		}
		
		public virtual bool IsEmpty()
		{
			return Flaws.Count < 1
				&& Merits.Count < 1;
		}
		
		public virtual string Serialize()
		{
			return JsonSerializer.Serialize(this);
		}
		
		public bool Equals(CoDCoreContainer container)
		{
			return container.Merits.Equals(Merits);
		}
	}
}
