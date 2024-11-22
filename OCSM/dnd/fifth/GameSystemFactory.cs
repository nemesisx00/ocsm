using System.Collections.Generic;
using Ocsm.Dnd.Fifth.Meta;
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
	
	public static void RegisterGameSystem(GameSystemRegistry registry)
	{
		registry.RegisterGameSystem(
			Name,
			typeof(DndFifthContainer),
			MetadataTypes,
			ResourcePaths.Fifth.NewSheetName,
			ResourcePaths.Fifth.Sheet
		);
	}
}
