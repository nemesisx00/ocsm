using System;
using System.Collections.Generic;
using System.Text.Json;
using OCSM.Meta;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.DnD.Fifth.Meta
{
	public class DnDFifthContainer : IMetadataContainer, IEquatable<DnDFifthContainer>
	{
		public List<InventoryArmor> Armor { get; set; } = new List<InventoryArmor>();
		public List<Background> Backgrounds { get; set; } = new List<Background>();
		public List<Class> Classes { get; set; } = new List<Class>();
		public List<Feature> Features { get; set; } = new List<Feature>();
		public List<Race> Races { get; set; } = new List<Race>();
		
		public void Deserialize(string json)
		{
			var result = JsonSerializer.Deserialize<DnDFifthContainer>(json);
			if(result is DnDFifthContainer dfc)
			{
				dfc.Armor.Sort();
				Armor = dfc.Armor;
				
				dfc.Backgrounds.Sort();
				Backgrounds = dfc.Backgrounds;
				
				dfc.Classes.Sort();
				Classes = dfc.Classes;
				
				dfc.Features.Sort();
				Features = dfc.Features;
				
				dfc.Races.Sort();
				Races = dfc.Races;
			}
		}
		
		public bool IsEmpty()
		{
			return Armor.Count < 1
				&& Backgrounds.Count < 1
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
			return container.Armor.Equals(Armor)
				&& container.Backgrounds.Equals(Backgrounds)
				&& container.Classes.Equals(Classes)
				&& container.Features.Equals(Features)
				&& container.Races.Equals(Races);
		}
	}
}
