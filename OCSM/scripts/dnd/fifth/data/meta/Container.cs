using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ocsm.Dnd.Fifth.Inventory;
using Ocsm.Meta;

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
		if(result is DndFifthContainer dfc)
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
	
	public override bool Equals(object other) => Equals(other as DndFifthContainer);
	
	public bool Equals(DndFifthContainer other) => Armors.Equals(other?.Armors)
		&& Backgrounds.Equals(other?.Backgrounds)
		&& Classes.Equals(other?.Classes)
		&& Containers.Equals(other?.Containers)
		&& Features.Equals(other?.Features)
		&& Items.Equals(other?.Items)
		&& Races.Equals(other?.Races)
		&& Weapons.Equals(other?.Weapons);
	
	public override int GetHashCode()
	{
		HashCode hash = new();
			
		Armors.ForEach(o => hash.Add(o));
		Backgrounds.ForEach(o => hash.Add(o));
		Classes.ForEach(o => hash.Add(o));
		Containers.ForEach(o => hash.Add(o));
		Features.ForEach(o => hash.Add(o));
		Items.ForEach(o => hash.Add(o));
		Races.ForEach(o => hash.Add(o));
		Weapons.ForEach(o => hash.Add(o));
		
		return hash.ToHashCode();
	}
}
