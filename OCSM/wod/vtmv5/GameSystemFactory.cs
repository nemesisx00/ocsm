using System.Collections.Generic;
using Ocsm.Meta;
using Ocsm.Wod.VtmV5.Meta;

namespace Ocsm.Wod.VtmV5;

public static class GameSystemFactory
{
	public const string Name = "WodVtmV5";
	
	public static readonly List<string> MetadataTypes = [
		"Advantage",
		"Flaw",
		"PredatorType",
	];
	
	public static void RegisterGameSystem(GameSystemRegistry registry)
	{
		registry.RegisterGameSystem(
			Name,
			typeof(WodVtmV5Container),
			MetadataTypes,
			ResourcePaths.VtmV5.NewSheetName,
			ResourcePaths.VtmV5.Sheet
		);
	}
}
