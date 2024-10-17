using System.Collections.Generic;
using Ocsm.Nodes;

namespace Ocsm.Wod;

public static class GameButtonFactory
{
	public static List<AddSheet> GenerateButtons()
	{
		List<AddSheet> buttons = [];
		
		if(AddSheetButtonFactory.GenerateButton(
			"Vampire: The Masquerade",
			"5th Edition",
			ResourcePaths.VtmV5.Sheet,
			ResourcePaths.VtmV5.NewSheetName,
			$"{ResourcePaths.PrefixAssets}/textures/AnkhVTM.png",
			$"{ResourcePaths.PrefixAssets}/textures/AnkhVTM-disabled.png"
		) is AddSheet vampire)
			buttons.Add(vampire);
		
		return buttons;
	}
}
