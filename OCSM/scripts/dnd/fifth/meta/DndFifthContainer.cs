using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ocsm.Meta;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth.Meta;

public class DndFifthContainer() : IMetadataContainer, IEquatable<DndFifthContainer>
{
	public List<Featureful> Featurefuls { get; set; } = [];
	public List<Feature> Features { get; set; } = [];
	public List<Item> Items { get; set; } = [];
	
	public void Deserialize(string json)
	{
		var result = JsonSerializer.Deserialize<DndFifthContainer>(json);
		if(result is DndFifthContainer container)
		{
			container.Sort();
			
			Featurefuls = container.Featurefuls;
			Features = container.Features;
			Items = container.Items;
		}
	}
	
	public bool Equals(DndFifthContainer other) => Featurefuls == other?.Featurefuls
		&& Features == other?.Features
		&& Items == other?.Items;
	
	public override bool Equals(object obj) => Equals(obj as DndFifthContainer);
	public override int GetHashCode() => throw new NotImplementedException();
	
	public bool IsEmpty() => Featurefuls.Count == 0
		&& Features.Count == 0
		&& Items.Count == 0;
	
	public string Serialize() => JsonSerializer.Serialize(this);
	
	public void Sort()
	{
		Featurefuls.Sort();
		Features.Sort();
		Items.Sort();
	}
}
