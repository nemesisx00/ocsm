using System.Collections.Generic;
using Godot;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Dnd.Fifth.Nodes.Meta;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth;

public static class GameSystemFactory
{
	public const string Name = "Dnd5e";
	
	public static readonly List<string> MetadataTypes = [
		"Background",
		"Class",
		"Species",
	];
	
	public static Node GenerateAddEditMetadata()
	{
		Node node = null;
		
		var resource = GD.Load<PackedScene>(ResourcePaths.Fifth.Meta.AddEditMetadata);
		if(resource.CanInstantiate())
			node = resource.Instantiate<DndFifthAddEditMetadata>();
		
		return node;
	}
	
	public static void RegisterGameSystem(GameSystemRegistry registry)
	{
		registry.RegisterGameSystem(
			typeof(GameSystemFactory),
			Name,
			typeof(DndFifthContainer),
			MetadataTypes,
			ResourcePaths.Fifth.NewSheetName,
			ResourcePaths.Fifth.Sheet
		);
	}
}
