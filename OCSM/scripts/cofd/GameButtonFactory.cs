using System.Collections.Generic;
using Ocsm.Nodes;

namespace Ocsm.Cofd;

public static class GameButtonFactory
{
	public static List<AddSheet> GenerateButtons()
	{
		List<AddSheet> buttons = [];
		
		if(AddSheetButtonFactory.GenerateButton(
			"Core / Mortal",
			"2nd Edition",
			ScenePaths.Cofd.Mortal.Sheet,
			ScenePaths.Cofd.Mortal.NewSheetName,
			"res://assets/textures/cofd/SkullWOD.png"
		) is AddSheet mortal)
			buttons.Add(mortal);
		
		if(AddSheetButtonFactory.GenerateButton(
			"Changeling: The Lost",
			"2nd Edition",
			ScenePaths.Cofd.Changeling.Sheet,
			ScenePaths.Cofd.Changeling.NewSheetName,
			"res://assets/textures/cofd/SkullCTL.png",
			"res://assets/textures/cofd/SkullCTL-disabled.png"
		) is AddSheet changeling)
			buttons.Add(changeling);
		
		if(AddSheetButtonFactory.GenerateButton(
			"Mage: The Awakening",
			"2nd Edition",
			ScenePaths.Cofd.Mage.Sheet,
			ScenePaths.Cofd.Mage.NewSheetName,
			"res://assets/textures/cofd/SkullMTAw.png",
			"res://assets/textures/cofd/SkullMTAw-disabled.png"
		) is AddSheet mage)
			buttons.Add(mage);
		
		if(AddSheetButtonFactory.GenerateButton(
			"Vampire: The Requiem",
			"2nd Edition",
			ScenePaths.Cofd.Vampire.Sheet,
			ScenePaths.Cofd.Vampire.NewSheetName,
			"res://assets/textures/cofd/SkullVTR.png",
			"res://assets/textures/cofd/SkullVTR-disabled.png"
		) is AddSheet vampire)
			buttons.Add(vampire);
		
		return buttons;
	}
}
