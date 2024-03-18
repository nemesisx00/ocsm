using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ocsm.Meta;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth.Meta;

public class DndFifthContainer() : IMetadataContainer, IEquatable<DndFifthContainer>
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
	
	public List<ItemArmor> Armors { get; set; } = [];
	public List<Background> Backgrounds { get; set; } = [];
	public List<Class> Classes { get; set; } = [];
	public List<ItemContainer> Containers { get; set; } = [];
	public List<Feature> Features { get; set; } = [];
	public List<Item> Items { get; set; } = [];
	public List<Race> Races { get; set; } = [];
	public List<ItemWeapon> Weapons { get; set; } = [];
	
	public void Deserialize(string json)
	{
		var result = JsonSerializer.Deserialize<DndFifthContainer>(json);
		if(result is DndFifthContainer container)
		{
			container.Sort();
			
			Armors = container.Armors;
			Backgrounds = container.Backgrounds;
			Classes = container.Classes;
			Containers = container.Containers;
			Features = container.Features;
			Items = container.Items;
			Races = container.Races;
			Weapons = container.Weapons;
		}
	}
	
	public bool Equals(DndFifthContainer other) => Armors == other?.Armors
		&& Backgrounds == other?.Backgrounds
		&& Classes == other?.Classes
		&& Containers == other?.Containers
		&& Features == other?.Features
		&& Items == other?.Items
		&& Races == other?.Races
		&& Weapons == other?.Weapons;
	
	public override bool Equals(object obj) => Equals(obj as DndFifthContainer);
	public override int GetHashCode() => throw new NotImplementedException();
	
	public bool IsEmpty() => Armors.Count == 0
		&& Backgrounds.Count == 0
		&& Classes.Count == 0
		&& Containers.Count == 0
		&& Features.Count == 0
		&& Items.Count == 0
		&& Races.Count == 0
		&& Weapons.Count == 0;
	
	public string Serialize() => JsonSerializer.Serialize(this);
	
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
}
