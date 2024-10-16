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
			ScenePaths.Dnd.Fifth.Sheet,
			ScenePaths.Dnd.Fifth.NewSheetName
		) is AddSheet fifth)
			buttons.Add(fifth);
		
		return buttons;
	}
}
