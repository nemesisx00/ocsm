using System.Collections.Generic;
using Godot;
using Ocsm.Meta;
using Ocsm.Nodes.Meta;
using Ocsm.Wod.VtmV5.Meta;
using Ocsm.Wod.VtmV5.Nodes.Meta;

namespace Ocsm.Wod.VtmV5;

public static class GameSystemFactory
{
	public const string Name = "WodVtmV5";
	
	public static readonly List<string> MetadataTypes = [
		"Advantage",
		"Flaw",
		"PredatorType",
	];
	
	public static Node GenerateAddEditMetadata()
	{
		Node node = null;
		
		var resource = GD.Load<PackedScene>(ResourcePaths.VtmV5.Meta.AddEditMetadata);
		if(resource.CanInstantiate())
			node = resource.Instantiate<WodVtmV5AddEditMetadata>();
		
		return node;
	}
	
	public static void RegisterGameSystem(GameSystemRegistry registry)
	{
		registry.RegisterGameSystem(
			typeof(GameSystemFactory),
			Name,
			typeof(WodVtmV5Container),
			MetadataTypes,
			ResourcePaths.VtmV5.NewSheetName,
			ResourcePaths.VtmV5.Sheet
		);
	}
}
