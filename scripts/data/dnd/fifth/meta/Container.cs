using System;
using System.Collections.Generic;
using System.Text.Json;
using OCSM.Meta;

namespace OCSM.DnD.Fifth.Meta
{
	public class DnDFifthContainer : IMetadataContainer, IEquatable<DnDFifthContainer>
	{
		public List<Background> Backgrounds { get; set; } = new List<Background>();
		public List<Class> Classes { get; set; } = new List<Class>();
		public List<Feature> Features { get; set; } = new List<Feature>();
		public List<Race> Races { get; set; } = new List<Race>();
		
		public void Deserialize(string json)
		{
			var result = JsonSerializer.Deserialize<DnDFifthContainer>(json);
			if(result is DnDFifthContainer dfc)
			{
				Backgrounds = dfc.Backgrounds;
				Classes = dfc.Classes;
				Features = dfc.Features;
				Races = dfc.Races;
			}
		}
		
		public bool IsEmpty()
		{
			return Backgrounds.Count < 1
				&& Classes.Count < 1
				&& Features.Count < 1
				&& Races.Count < 1;
		}
		
		public string Serialize()
		{
			return JsonSerializer.Serialize(this);
		}
		
		public bool Equals(DnDFifthContainer container)
		{
			return container.Backgrounds.Equals(Backgrounds)
				&& container.Classes.Equals(Classes)
				&& container.Features.Equals(Features)
				&& container.Races.Equals(Races);
		}
	}
}
