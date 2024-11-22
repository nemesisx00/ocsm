using System.Collections.Generic;
using Ocsm.Cofd.Ctl.Meta;
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
	
	public static void RegisterGameSystem(GameSystemRegistry registry)
	{
		registry.RegisterGameSystem(
			Name,
			typeof(CofdChangelingContainer),
			MetadataTypes,
			ResourcePaths.Changeling.NewSheetName,
			ResourcePaths.Changeling.Sheet
		);
	}
}
