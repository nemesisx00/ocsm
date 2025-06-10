using System.Collections.Generic;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth;

public static class GameButtonFactory
{
	public static List<AddSheet> GenerateButtons()
	{
		List<AddSheet> buttons = [];
		
		if(AddSheetButtonFactory.GenerateButton(
			"Dungeons & Dragons",
			"5th Edition",
			ResourcePaths.Fifth.Sheet,
			ResourcePaths.Fifth.NewSheetName
		) is AddSheet fifth)
			buttons.Add(fifth);
		
		return buttons;
	}
}
