using System;
using System.Collections.Generic;
using System.Text.Json;
using Ocsm.Meta;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth.Meta;

public class DndFifthContainer() : BaseContainer(), IMetadataContainer, IEquatable<DndFifthContainer>
{
	public List<Feature> Features { get; set; } = [];
	public List<Item> Items { get; set; } = [];
	
	public override void Deserialize(string json)
	{
		var result = JsonSerializer.Deserialize<DndFifthContainer>(json);
		if(result is DndFifthContainer container)
		{
			container.Sort();
			
			Metadata = container.Metadata;
			Features = container.Features;
			Items = container.Items;
		}
	}
	
	public bool Equals(DndFifthContainer other) => base.Equals(other)
		&& Features == other?.Features
		&& Items == other?.Items;
	
	public override bool Equals(object obj) => Equals(obj as DndFifthContainer);
	public override int GetHashCode() => throw new NotImplementedException();
	
	public override bool IsEmpty() => base.IsEmpty()
		&& Features.Count == 0
		&& Items.Count == 0;
	
	public override string Serialize() => JsonSerializer.Serialize(this);
	
	public void Sort()
	{
		Metadata.Sort();
		Features.Sort();
		Items.Sort();
	}
}
