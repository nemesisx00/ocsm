using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using OCSM.Meta;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.DnD.Fifth.Meta
{
	public class DnDFifthContainer : IMetadataContainer, IEquatable<DnDFifthContainer>
	{
		[JsonIgnore]
		public List<Item> AllItems
		{
			get
			{
				var list = new List<Item>();
				list.AddRange(Armors);
				list.AddRange(Containers);
				list.AddRange(Items);
				list.AddRange(Weapons);
				list.Sort();
				return list;
			}
		}
		
		public List<ItemArmor> Armors { get; set; } = new List<ItemArmor>();
		public List<Background> Backgrounds { get; set; } = new List<Background>();
		public List<Class> Classes { get; set; } = new List<Class>();
		public List<ItemContainer> Containers { get; set; } = new List<ItemContainer>();
		public List<Feature> Features { get; set; } = new List<Feature>();
		public List<Item> Items { get; set; } = new List<Item>();
		public List<Race> Races { get; set; } = new List<Race>();
		public List<ItemWeapon> Weapons { get; set; } = new List<ItemWeapon>();
		
		public void Deserialize(string json)
		{
			var result = JsonSerializer.Deserialize<DnDFifthContainer>(json);
			if(result is DnDFifthContainer dfc)
			{
				dfc.Sort();
				
				Armors = dfc.Armors;
				Backgrounds = dfc.Backgrounds;
				Classes = dfc.Classes;
				Containers = dfc.Containers;
				Features = dfc.Features;
				Items = dfc.Items;
				Races = dfc.Races;
				Weapons = dfc.Weapons;
			}
		}
		
		public bool IsEmpty()
		{
			return Armors.Count < 1
				&& Backgrounds.Count < 1
				&& Classes.Count < 1
				&& Containers.Count < 1
				&& Features.Count < 1
				&& Items.Count < 1
				&& Races.Count < 1
				&& Weapons.Count < 1;
		}
		
		public string Serialize()
		{
			return JsonSerializer.Serialize(this);
		}
		
		public void Sort()
		{
			Armors.Sort();
			Backgrounds.Sort();
			Classes.Sort();
			Containers.Sort();
			Features.Sort();
			Items.Sort();
			Races.Sort();
			Weapons.Sort();
		}
		
		public bool Equals(DnDFifthContainer container)
		{
			return Armors.Equals(container.Armors)
				&& Backgrounds.Equals(container.Backgrounds)
				&& Classes.Equals(container.Classes)
				&& Containers.Equals(container.Containers)
				&& Features.Equals(container.Features)
				&& Items.Equals(container.Items)
				&& Races.Equals(container.Races)
				&& Weapons.Equals(container.Weapons);
		}
	}
}
