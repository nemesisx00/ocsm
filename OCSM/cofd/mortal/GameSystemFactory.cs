using System.Collections.Generic;
using Godot;
using Ocsm.Cofd.Meta;
using Ocsm.Meta;

namespace Ocsm.Cofd;

public static class GameSystemFactory
{
	public const string Name = "CofdMortal";
	
	public static readonly List<string> MetadataTypes = [];
	
	public static Node GenerateAddEditMetadata()
	{
		Node node = null;
		
		/*
		var resource = GD.Load<PackedScene>(ResourcePaths.Mortal.Meta.AddEditMetadata);
		if(resource.CanInstantiate())
			node = resource.Instantiate<>();
		*/
		
		return node;
	}
	
	public static void RegisterGameSystem(GameSystemRegistry registry)
	{
		registry.RegisterGameSystem(
			typeof(GameSystemFactory),
			Name,
			typeof(CofdCoreContainer),
			MetadataTypes,
			ResourcePaths.Mortal.NewSheetName,
			ResourcePaths.Mortal.Sheet
		);
	}
}
