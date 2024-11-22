using System.Collections.Generic;
using Ocsm.Cofd.Meta;
using Ocsm.Meta;

namespace Ocsm.Cofd;

public static class GameSystemFactory
{
	public const string Name = "CofdMortal";
	
	public static readonly List<string> MetadataTypes = [];
	
	public static void RegisterGameSystem(GameSystemRegistry registry)
	{
		registry.RegisterGameSystem(
			Name,
			typeof(CofdCoreContainer),
			MetadataTypes,
			ResourcePaths.Mortal.NewSheetName,
			ResourcePaths.Mortal.Sheet
		);
	}
}
