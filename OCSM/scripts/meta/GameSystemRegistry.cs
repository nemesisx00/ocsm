using System;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Meta;

public class GameSystemRegistry()
{
	public List<GameSystem> gameSystems = [];
	
	public GameSystem GetGameSystem(string name) => gameSystems.FirstOrDefault(gs => gs.Name == name);
	
	public bool RegisterGameSystem(string name, Type containerType, List<string> metadataTypes, string sheetName, string sheetPath)
	{
		var exists = gameSystems.Where(gs => gs.Name == name).Any();
		
		if(!exists)
		{
			gameSystems.Add(new()
			{
				Name = name,
				MetadataContainerType = containerType,
				MetadataTypes = metadataTypes,
				SheetDefaultName = sheetName,
				SheetResourcePath = sheetPath,
			});
		}
		
		return exists;
	}
	
	public void UnregisterGameSystem(string name)
	{
		if(gameSystems.FirstOrDefault(gs => gs.Name == name) is GameSystem gs)
			gameSystems.Remove(gs);
	}
}
