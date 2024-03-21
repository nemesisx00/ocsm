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
	public List<ItemContainer> Containers { get; set; } = [];
	public List<Featureful> Featurefuls { get; set; } = [];
	public List<Feature> Features { get; set; } = [];
	public List<Item> Items { get; set; } = [];
	public List<ItemWeapon> Weapons { get; set; } = [];
	
	public void Deserialize(string json)
	{
		var result = JsonSerializer.Deserialize<DndFifthContainer>(json);
		if(result is DndFifthContainer container)
		{
			container.Sort();
			
			Armors = container.Armors;
			Containers = container.Containers;
			Featurefuls = container.Featurefuls;
			Features = container.Features;
			Items = container.Items;
			Weapons = container.Weapons;
		}
	}
	
	public bool Equals(DndFifthContainer other) => Armors == other?.Armors
		&& Containers == other?.Containers
		&& Featurefuls == other?.Featurefuls
		&& Features == other?.Features
		&& Items == other?.Items
		&& Weapons == other?.Weapons;
	
	public override bool Equals(object obj) => Equals(obj as DndFifthContainer);
	public override int GetHashCode() => throw new NotImplementedException();
	
	public bool IsEmpty() => Armors.Count == 0
		&& Containers.Count == 0
		&& Featurefuls.Count == 0
		&& Features.Count == 0
		&& Items.Count == 0
		&& Weapons.Count == 0;
	
	public string Serialize() => JsonSerializer.Serialize(this);
	
	public void Sort()
	{
		Armors.Sort();
		Containers.Sort();
		Featurefuls.Sort();
		Features.Sort();
		Items.Sort();
		Weapons.Sort();
	}
}
