using System.Collections.Generic;
using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Cofd.Ctl.Nodes.Meta;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl;

public static class GameSystemFactory
{
	public const string Name = "CofdChangeling";
	
	public static readonly List<string> MetadataTypes = [
		"ContractRegalia",
		"ContractType",
		"Court",
		"Kith",
		"Regalia",
		"Seeming",
	];
	
	public static Node GenerateAddEditMetadata()
	{
		Node node = null;
		
		var resource = GD.Load<PackedScene>(ResourcePaths.Changeling.Meta.AddEditMetadata);
		if(resource.CanInstantiate())
			node = resource.Instantiate<CofdChangelingAddEditMetadata>();
		
		return node;
	}
	
	public static void RegisterGameSystem(GameSystemRegistry registry)
	{
		registry.RegisterGameSystem(
			typeof(GameSystemFactory),
			Name,
			typeof(CofdChangelingContainer),
			MetadataTypes,
			ResourcePaths.Changeling.NewSheetName,
			ResourcePaths.Changeling.Sheet
		);
	}
}
